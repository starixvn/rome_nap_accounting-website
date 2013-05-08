


var wlByCurrency = {
    fromDate: '',
    toDate: '',
    excludeTest: true,
    drawFilter: 1,
    sportType: '',
    currency: '',
    reportDate: '',
    showWLByCurrencyByDate: function (excludeTest, drawFilter, sportType, currency, reportDate) {
        try { parent.BlockUI(); } catch (e) { }
        var gridOptsWLByDateReport = {
            tblContentId: 'wlCurrencyByDateReport_Container',
            isShowPager: false
        };
        $("#wlCurrencyByDateReport").initGrid(gridOptsWLByDateReport);
        $("#wlCurrencyByDateReport_ExportExcel").click(function () {
            wlByCurrency.exportCurrencyByDateReport("CurrencyByDate");
        });

        wlByCurrency.excludeTest = excludeTest;
        wlByCurrency.drawFilter = drawFilter;
        wlByCurrency.sportType = sportType;
        wlByCurrency.currency = currency;
        wlByCurrency.reportDate = reportDate;
        var data = {
            date: wlByCurrency.reportDate,
            excludeTest: wlByCurrency.excludeTest,
            drawFilter: wlByCurrency.drawFilter,
            sportType: wlByCurrency.sportType,
            currency: wlByCurrency.currency
        };
        $.ajax({
            type: "POST",
            url: "GetWinlossDetailByCurrency",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'Virtual Sports Win Loss Details - ' + wlByCurrency.reportDate + " - " + wlByCurrency.currency;
                $("#wlCurrencyByDateReport_Title").html(title);
                if (rs && rs.WinlossDetailList && rs.WinlossDetailList.length > 0) {
                    wlByCurrency.renderWinLossByDateReport_MainReport(rs);
                }
                else {
                    wlByCurrency.renderWinLossByDateReport_NoData();
                }
            },
            error: function () {
                wlByCurrency.renderWinLossByDateReport_NoData();
            },
            complete: function () {
                wlByCurrency.renderWinLossByDateReport();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderWinLossByDateReport_MainReport: function (rs) {
        var frozenHeader = '';
        var scrollContent = '';
        var rowHeaderHTML = '';

        var wlDetailList = rs.WinlossDetailList;
        var wlDetailListLength = wlDetailList.length;
        for (var i = 0; i < wlDetailListLength; i++) {
            var wlDetail = wlDetailList[i];
            rowHeaderHTML = '<td><div class="col120 c">' + wlDetail.Name + '</div></td>';
            frozenHeader += '<tr>' + rowHeaderHTML + '</tr>';
            scrollContent += '<tr>' +
                                    rowHeaderHTML +
                                    '<td><div class="col120 r">' + wlDetail.Betcount + '</div></td>\
                                    <td><div class="col120 r">' + wlDetail.Turnover + '</div></td>\
                                    <td><div class="col120 r">' + wlDetail.WLMember + '</div></td>\
                                    <td><div class="col120 r">' + wlDetail.Position + '</div></td>\
                                    <td><div class="col120 r">' + wlDetail.WLCompany + '</div></td>\
                                    <td><div class="col160 r">' + wlDetail.AmountDueto + '</div></td>\
                                </tr>';
        }

        var frozenFooter = '';
        var scrollFooterContent = '';
        var rowFooterHTML = '';
        var total = rs.Total;
        rowHeaderHTML = '<th><div class="c b" style="width: 120px;">' + total.Name + '</div></th>';
        frozenFooter += '<tr class="vsTotal">' + rowHeaderHTML + '</tr>';
        scrollFooterContent += '<tr class="vsTotal">' +
                                    rowHeaderHTML +
                                    '<th><div class="col120 r b">' + total.Betcount + '</div></th>\
                                    <th><div class="col120 r b">' + total.Turnover + '</div></th>\
                                    <th><div class="col120 r b">' + total.WLMember + '</div></th>\
                                    <th><div class="col120 r b">' + total.Position + '</div></th>\
                                    <th><div class="col120 r b">' + total.WLCompany + '</div></th>\
                                    <th><div class="col160 r b">' + total.AmountDueto + '</div></th>\
                                </tr>';

        frozenFooter = stringformat.removeSpaceIE9(frozenFooter);
        scrollFooterContent = stringformat.removeSpaceIE9(scrollFooterContent);
        frozenHeader = stringformat.removeSpaceIE9(frozenHeader);
        scrollContent = stringformat.removeSpaceIE9(scrollContent);

        var reportGrid = $("#wlCurrencyByDateReport");
        reportGrid.find(".row_footer_content thead").empty().append(frozenFooter);
        reportGrid.find(".col_footer_content thead").empty().append(scrollFooterContent);
        $("#wlCurrencyByDateReport_RowHeader tbody").empty().append(frozenHeader);
        $("#wlCurrencyByDateReport_Container tbody").empty().append(scrollContent);

    },
    renderWinLossByDateReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="6">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;">&nbsp</td></tr>';
        frozenHeader = stringformat.removeSpaceIE9(frozenHeader);
        html = stringformat.removeSpaceIE9(html);
        $("#wlCurrencyByDateReport_RowHeader tbody").empty().append(frozenHeader);
        $("#wlCurrencyByDateReport_Container tbody").empty().append(html);

        var reportGrid = $("#wlCurrencyByDateReport");
        reportGrid.find(".row_footer_content thead").empty();
        reportGrid.find(".col_footer_content thead").empty();
        return html;
    },
    renderWinLossByDateReport: function () {
        var gridOptsWinLossReport = {
            tblContentId: 'wlCurrencyByDateReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 120,
            gridWidth: 915,
            freezeWidth: 120,
            freezeHeight: 100,
            numberFreezeCol: 1
        };
        var reportGrid = $("#wlCurrencyByDateReport");
        reportGrid.removeClass("hidden");
        reportGrid.gridScroller(gridOptsWinLossReport);
    },
    backToMainReport: function () {
        var wlCurrencyByDateReport = $("#wlCurrencyByDateReport");
        wlCurrencyByDateReport.addClass("hidden");

        var reportGrid = $("#mainWinLossReport");
        reportGrid.removeClass("hidden");
    },
    exportCurrencyByDateReport: function (exportType) {
        var url = 'WinLossReport?ExportType=' + exportType + '&ReportDate=' + wlByCurrency.reportDate +
                  '&IsExcludeTest=' + wlByCurrency.excludeTest + '&DrawFilter=' + wlByCurrency.drawFilter +
                  '&SportType=' + wlByCurrency.sportType + '&Currency=' + wlByCurrency.currency;
        $('#frm').attr('src', url);
    }
}