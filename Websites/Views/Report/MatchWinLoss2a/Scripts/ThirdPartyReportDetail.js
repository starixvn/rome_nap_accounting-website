

var thirdPartyReportDetail = {
    isFirstLoad: true,
    reportDate: null,
    betType: '',
    reportWidth: 0,
    frozenHeader: '',
    scrollContent: '',
    initPage: function (reportDate, betType) {
        thirdPartyReportDetail.reportDate = reportDate;
        thirdPartyReportDetail.betType = betType;
        thirdPartyReportDetail.reportWidth = 830;
        var gridOptsThirdPartyReport = {
            tblContentId: 'thirdPartyReport_Container',
            isShowPager: false
        };
        $("#thirdPartyReport").initGrid(gridOptsThirdPartyReport);
        thirdPartyReportDetail.viewReport(reportDate, betType);
    },
    viewReport: function (reportDate, betType) {
        try { parent.parent.BlockUI(); } catch (e) { }
        var data = {
            reportDate: reportDate
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchWinLoss/SelThirdPartyCasinoReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'Casino Daily Report: ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#thirdPartyReport_Title").html(title);
                if (rs != null && rs.SportReportDetails != null &&
                    rs.SportReportDetails.length > 0) {
                    thirdPartyReportDetail.renderThirdPartyReport_MainReport(rs, reportDate, betType);
                }
                else {
                    thirdPartyReportDetail.renderThirdPartyReport_NoData();
                }
            },
            error: function () {
                thirdPartyReportDetail.renderThirdPartyReport_NoData();
            },
            complete: function () {
                thirdPartyReportDetail.renderThirdPartyReport(reportDate, betType);
                try {
                    parent.parent.jQuery.unblockUI();
                    if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
                        console.clear();
                    }
                }
                catch (e) {
                }
            }
        });
    },
    renderThirdPartyReport_MainReport: function (rs, reportDate, betType) {
        thirdPartyReportDetail.frozenHeader = '';
        thirdPartyReportDetail.scrollContent = '';

        var sportReports = rs.SportReportDetails;
        var sportReportLength = sportReports.length;
        for (var i = 0; i < sportReportLength; i++) {
            var sr = sportReports[i];
            var sportReportDetails = sr.ReportDetails;
            var sportReportDetailLength = sportReportDetails.length;
            for (var j = 0; j < sportReportDetailLength; j++) {
                var detail = sportReportDetails[j];
                thirdPartyReportDetail.frozenHeader +=
                        '<tr>' + (j == 0 ?
                            '<td class="b c col120" style="" rowspan="' + (sportReportDetailLength + 1) + '">' + sr.Total.BetType.Name + '</td>' : '') +
                            '<td class="c col100" style="">' + detail.Currency + '</td>\
                         </tr>';
                thirdPartyReportDetail.scrollContent +=
                       '<tr>' + (j == 0 ?
                            '<td class="b c col120" style="" rowspan="' + (sportReportDetailLength + 1) + '">' + sr.Total.BetType.Name + '</td>' : '') +
                            '<td class="c col100" style="">' + detail.Currency + '</td>\
                            <td class="r col100" style="">' + detail.Turnover + '</td>\
                            <td class="r col100" style="">' + detail.WinLost + '</td>\
                            <td class="r col90" style="">' + detail.Position + '</td>\
                            <td class="r col80" style="">' + detail.Commission + '</td>\
                            <td class="r col100" style="">' + detail.LineTotal + '</td>\
                            <td class="r col100" style="">' + detail.Margin + '</td>\
                        </tr>';

            }

            var total = sr.Total;
            thirdPartyReportDetail.frozenHeader +=
                        '<tr>\
                            <td class="b c col100" style="">' + total.Name + '</td>\
                         </tr>';
            thirdPartyReportDetail.scrollContent +=
                       '<tr>\
                            <td class="b c col100" style="">' + total.Name + '</td>\
                            <td class="b r col100" style="">' + total.Turnover + '</td>\
                            <td class="b r col100" style="">' + total.WinLost + '</td>\
                            <td class="b r col90" style="">' + total.Position + '</td>\
                            <td class="b r col80" style="">' + total.Commission + '</td>\
                            <td class="b r col100" style="">' + total.LineTotal + '</td>\
                            <td class="b r col100" style="">' + total.Margin + '</td>\
                        </tr>';

            thirdPartyReportDetail.renderThirdPartyReport_EmptyRow();
        }

        var reportByCurrencyDetails = rs.ReportByCurrency.ReportDetails;
        var rbcLength = reportByCurrencyDetails.length;
        var css = '';
        for (var k = 0; k < rbcLength; k++) {
            var detailByCurrency = reportByCurrencyDetails[k];
            css = (k % 2 == 0 ? 'csEvenRow' : 'csOddRow');
            thirdPartyReportDetail.frozenHeader +=
                        '<tr class="' + css + '">' + (k == 0 ?
                            '<td class="b c col120" style="" rowspan="' + (rbcLength + 1) + '">' + rs.ReportByCurrency.Total.Name + '</td>' : '') +
                            '<td class="b c col100" style="">' + detailByCurrency.Currency + '</td>\
                         </tr>';
            thirdPartyReportDetail.scrollContent +=
                       '<tr class="' + css + '">' + (k == 0 ?
                            '<td class="b c col120" style="" rowspan="' + (sportReportDetailLength + 1) + '">' + sr.Total.Name + '</td>' : '') +
                            '<td class="b c col100" style="">' + detailByCurrency.Currency + '</td>\
                            <td class="b r col100" style="">' + detailByCurrency.Turnover + '</td>\
                            <td class="b r col100" style="">' + detailByCurrency.WinLost + '</td>\
                            <td class="b r col90" style="">' + detailByCurrency.Position + '</td>\
                            <td class="b r col80" style="">' + detailByCurrency.Commission + '</td>\
                            <td class="b r col100" style="">' + detailByCurrency.LineTotal + '</td>\
                            <td class="b r col100" style="">' + detailByCurrency.Margin + '</td>\
                        </tr>';

        }

        $("#thirdPartyReport_RowHeader tbody").empty().append(thirdPartyReportDetail.frozenHeader);
        $("#thirdPartyReport_Container tbody").empty().append(thirdPartyReportDetail.scrollContent);

        thirdPartyReportDetail.renderThirdPartyReport_Footer(rs.GrandTotal, reportDate, betType);
    },
    renderThirdPartyReport_Footer: function (grandTotal, reportDate, betType) {
        var frozenHeader =
                        '<tr class="csGrandTotal">\
                            <th class="b c" style="width: 225px;" colspan="2">' + grandTotal.Name + '</th>\
                         </tr>';
        var scrollContent =
                       '<tr class="csGrandTotal">\
                            <th class="b c" style="width: 225px;" colspan="2">' + grandTotal.Name + '</th>\
                            <th class="b col100" style="text-align: right;">' + grandTotal.Turnover + '</th>\
                            <th class="b col100" style="text-align: right;">' + grandTotal.WinLost + '</th>\
                            <th class="b col90" style="text-align: right;">' + grandTotal.Position + '</th>\
                            <th class="b col80" style="text-align: right;">' + grandTotal.Commission + '</th>\
                            <th class="b col100" style="text-align: right;">' + grandTotal.LineTotal + '</th>\
                            <th class="b col100" style="text-align: right;">' + grandTotal.Margin + '</th>\
                        </tr>';
        $("#thirdPartyReport").find(".row_footer_content thead").empty().append(frozenHeader);
        $("#thirdPartyReport").find(".col_footer_content thead").empty().append(scrollContent);
    },
    renderThirdPartyReport_EmptyRow: function () {
        thirdPartyReportDetail.frozenHeader +=
                '<tr>\
                    <td class="b c col220" style="" colspan="2">&nbsp</td>\
                 </tr>';
        thirdPartyReportDetail.scrollContent +=
                '<tr>\
                    <td class="b c" style="width: 225px;" colspan="2">&nbsp</td>\
                    <td class="b c" style="width: 490px;" colspan="6">&nbsp</td>\
                </tr>';

    },
    renderThirdPartyReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="2">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="6">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="2">&nbsp</td></tr>';
        frozenHeader = StringFormatter.removeSpaceIE9(frozenHeader);
        html = StringFormatter.removeSpaceIE9(html);
        $("#thirdPartyReport_RowHeader tbody").empty().append(frozenHeader);
        $("#thirdPartyReport_Container tbody").empty().append(html);

        $("#thirdPartyReport").find(".row_footer_content thead").empty();
        $("#thirdPartyReport").find(".col_footer_content thead").empty();
        return html;
    },
    renderThirdPartyReport: function (reportDate, betType) {
        var gridOptsThirdPartyReport = {
            tblContentId: 'thirdPartyReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 50,
            gridWidth: thirdPartyReportDetail.reportWidth,
            freezeWidth: 225,
            freezeHeight: 33,
            numberFreezeCol: 2
        };
        $("#thirdPartyReport").removeClass("hidden");
        $("#thirdPartyReport").gridScroller(gridOptsThirdPartyReport);
    },
    exportExcel: function () {
        var url = 'ThirdPartyReport?ExportType=Casino' +
                    '&ReportDate=' + thirdPartyReportDetail.reportDate +
                    '&BetType=' + thirdPartyReportDetail.betType;
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    if (thirdPartyReportDetail.isFirstLoad) {
        thirdPartyReportDetail.initPage(reportDate, betType);
        thirdPartyReportDetail.isFirstLoad = false;
    }
}); 
