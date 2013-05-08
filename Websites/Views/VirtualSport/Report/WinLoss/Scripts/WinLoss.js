
$(document).ready(function () {
    VirtualSportWinloss.loadSportType();
    VirtualSportWinloss.initCalendar(dateVS);
});

var VirtualSportWinloss = {
    fromDate: '',
    toDate: '',
    excludeTest: true,
    drawFilter: 1,
    sportType: '',
    init: function () {
        var gridOptsWinLossReport = {
            tblContentId: 'mainWinLossReport_Container',
            isShowPager: false
        };
        $("#mainWinLossReport").initGrid(gridOptsWinLossReport);
        $("mainWinLossReport_ExportExcel").click(function () {
            VirtualSportWinloss.exportMainReport("MainReport");
        });
    },
    initCalendar: function (dateVS) {
        var lastdate = '';
        if (lastMonth <= new Date(dateVS).getMonth())
            lastdate = currDate;
        else
            lastdate = lastMonth + '/' + lastDay + '/' + lastYear;
        $('#spMonth').monPickr({
            today: lastdate,
            maxDate: currDate,
            minDate: dateVS,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });
    },
    loadSportType: function () {
        $.ajax({
            type: "POST",
            url: "GetSportType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs.length > 0) {
                    VirtualSportWinloss.renderSportTypeSelected(rs, 'ddlSportType');
                }

            }
        });
    },
    renderSportTypeSelected: function (rs, id) {
        var tempData = [];
        for (var i = 0; i < rs.length; i++) {
            tempData.push({
                Id: id + rs[i].Id.toString(),
                Value: rs[i].Id,
                Text: rs[i].Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });

        }
        $("#" + id).multiSelect(
        {
            minWidth: 180,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Selected',
            onCheck: function () {
            }
        });
    },
    getSelectedSport: function () {
        var id = 'ddlSportType';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        // result += $('#ddlSportUser1').is(':checked') ? '1,' : '';

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");
        return result;
    },
    getDailyWinloss: function () {
        try { parent.BlockUI(); } catch (e) { }
        var wlCurrencyByDateReport = $("#wlCurrencyByDateReport");
        wlCurrencyByDateReport.addClass("hidden");
        $('#errMsg').hide();
        VirtualSportWinloss.fromDate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
        VirtualSportWinloss.toDate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        VirtualSportWinloss.excludeTest = $("#chbExclude").attr("checked") ? true : false;
        VirtualSportWinloss.drawFilter = $("#ddlFilter").val();
        VirtualSportWinloss.sportType = VirtualSportWinloss.getSelectedSport();
        var data = {
            fromDate: VirtualSportWinloss.fromDate,
            toDate: VirtualSportWinloss.toDate,
            excludeTest: VirtualSportWinloss.excludeTest,
            drawFilter: VirtualSportWinloss.drawFilter,
            sportType: VirtualSportWinloss.sportType
        };
        $.ajax({
            type: "POST",
            url: "GetDailyWinloss",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'Virtual Sports Win Loss - From ' + VirtualSportWinloss.fromDate + " to " + VirtualSportWinloss.toDate;

                $("#mainWinLossReport_Title").html(title);
                if (rs && rs.WLByDateList && rs.WLByDateList.length > 0) {
                    VirtualSportWinloss.init();
                    VirtualSportWinloss.renderWinLossReport_MainReport(rs);
                    VirtualSportWinloss.renderWinLossReport();
                }
                else {
                    VirtualSportWinloss.renderWinLossReport_NoData();
                }
            },
            error: function () {
                VirtualSportWinloss.renderWinLossReport_NoData();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderWinLossReport_MainReport: function (rs) {
        var frozenHeader = '';
        var scrollContent = '';
        var rowHeaderHTML = '';

        var wlByDateList = rs.WLByDateList;
        var wlByDateListLength = wlByDateList.length;
        for (var i = 0; i < wlByDateListLength; i++) {
            var wlByDate = wlByDateList[i];
            var wlDetailList = wlByDate.WLDetailList;
            var wlDetailListLength = wlDetailList.length;
            for (var j = 0; j < wlDetailListLength; j++) {
                var wlDetail = wlDetailList[j];
                if (j == 0) {
                    rowHeaderHTML = '<td rowspan="' + wlDetailListLength + '"><div class="col80 c">' + wlByDate.Date + '</div></td>';
                }
                else {
                    rowHeaderHTML = '';
                }
                rowHeaderHTML += '<td>\
                                    <div class="col100 c b">\
                                        <div class="currencyLink" onclick="VirtualSportWinloss.showWLByCurrencyByDate(\'' + wlDetail.Name + '\',\'' + wlByDate.Date + '\');">' + wlDetail.Name +
                                        '</div>\
                                    </div>\
                                  </td>';
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
        }

        var total = rs.Total;
        var totalLength = total.length;
        for (var k = 0; k < totalLength; k++) {
            var totalDetail = total[k];
            if (k == 0) {
                rowHeaderHTML = '<td rowspan="' + totalLength + '"><div class="col80 c b" style="">Total</div></td>';
            }
            else {
                rowHeaderHTML = '';
            }
            rowHeaderHTML += '<td><div class="col100 c" style="">' + totalDetail.Name + '</div></td>';
            frozenHeader += '<tr class="vsTotal">' + rowHeaderHTML + '</tr>';
            scrollContent += '<tr class="vsTotal">' +
                                rowHeaderHTML +
                                '<td><div class="col120 r">' + totalDetail.Betcount + '</div></td>\
                                <td><div class="col120 r">' + totalDetail.Turnover + '</div></td>\
                                <td><div class="col120 r">' + totalDetail.WLMember + '</div></td>\
                                <td><div class="col120 r">' + totalDetail.Position + '</div></td>\
                                <td><div class="col120 r">' + totalDetail.WLCompany + '</div></td>\
                                <td><div class="col160 r">' + totalDetail.AmountDueto + '</div></td>\
                            </tr>';
        }

        var frozenFooter = '';
        var scrollFooterContent = '';
        var rowFooterHTML = '';
        var grandTotal = rs.GrandTotal;
        var grandTotalLength = grandTotal.length;
        for (var l = 0; l < grandTotalLength; l++) {
            var grandTotalDetail = grandTotal[l];
            rowHeaderHTML = '<th colspan="2"><div class="c b" style="width: 185px;">' + grandTotalDetail.Name + '</div></th>';
            frozenFooter += '<tr class="vsGrandTotal">' + rowHeaderHTML + '</tr>';
            scrollFooterContent += '<tr class="vsGrandTotal">' +
                                rowHeaderHTML +
                                '<th><div class="col120 r b">' + grandTotalDetail.Betcount + '</div></th>\
                                <th><div class="col120 r b">' + grandTotalDetail.Turnover + '</div></th>\
                                <th><div class="col120 r b">' + grandTotalDetail.WLMember + '</div></th>\
                                <th><div class="col120 r b">' + grandTotalDetail.Position + '</div></th>\
                                <th><div class="col120 r b">' + grandTotalDetail.WLCompany + '</div></th>\
                                <th><div class="col160 r b">' + grandTotalDetail.AmountDueto + '</div></th>\
                            </tr>';
        }

        frozenFooter = stringformat.removeSpaceIE9(frozenFooter);
        scrollFooterContent = stringformat.removeSpaceIE9(scrollFooterContent);
        frozenHeader = stringformat.removeSpaceIE9(frozenHeader);
        scrollContent = stringformat.removeSpaceIE9(scrollContent);

        $("#mainWinLossReport").find(".row_footer_content thead").empty().append(frozenFooter);
        $("#mainWinLossReport").find(".col_footer_content thead").empty().append(scrollFooterContent);
        $("#mainWinLossReport_RowHeader tbody").empty().append(frozenHeader);
        $("#mainWinLossReport_Container tbody").empty().append(scrollContent);

    },
    renderWinLossReport_NoData: function () {
        var reportGrid = $("#mainWinLossReport");
        reportGrid.addClass("hidden");
        showAlertMessage("errMsg", "There is no data.", 1);
        $('#errMsg').show();
    },
    renderWinLossReport: function () {
        var gridOptsWinLossReport = {
            tblContentId: 'mainWinLossReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 120,
            gridWidth: 980,
            freezeWidth: 185,
            freezeHeight: 100,
            numberFreezeCol: 2
        };
        var reportGrid = $("#mainWinLossReport");
        reportGrid.removeClass("hidden");
        reportGrid.gridScroller(gridOptsWinLossReport);
    },
    showWLByCurrencyByDate: function (currency, reportDate) {
        var reportGrid = $("#mainWinLossReport");
        reportGrid.addClass("hidden");
        wlByCurrency.showWLByCurrencyByDate(VirtualSportWinloss.excludeTest, VirtualSportWinloss.drawFilter,
                                            VirtualSportWinloss.sportType, currency, reportDate);
    },
    exportMainReport: function (exportType) {
        var url = 'WinLossReport?ExportType=' + exportType + '&FromDate=' + VirtualSportWinloss.fromDate +
                  '&ToDate=' + VirtualSportWinloss.toDate + '&IsExcludeTest=' + VirtualSportWinloss.excludeTest +
                  '&DrawFilter=' + VirtualSportWinloss.drawFilter + '&SportType=' + VirtualSportWinloss.sportType;
        $('#frm').attr('src', url);
    }
}

