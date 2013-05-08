$(document).ready(function () {
    PokerWinloss.init(balanceupDay);
});
var Helper = {
    renderCalendarFromTo: function (ids, opts) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);

            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids.split(',')[0]).datepicker("setDate", new Date(opts.maxDate));
        $(ids.split(',')[1]).datepicker("setDate", new Date(opts.maxDate));
    },
    initCalendar: function (yesterday) {
        $('#spMonth').monPickr({
            today: yesterday,
            minDate: '7/20/2012',
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });
    }

};
var PokerWinloss = {
    fromDate: '',
    toDate: '',
    isViewWH: false,
    init: function (dateBal) {
        var arr = dateBal.split(' ');
        $('#txtTitle').html('Data is available ' + arr[0] + ' onward. For more historic data, please click to ');
        var gridOptsWinLossReport = {
            tblContentId: 'mainPokerWinLossReport_Container',
            isShowPager: false
        };
        $("#mainPokerWinLossReport").initGrid(gridOptsWinLossReport);
    },
    GetPokerWinloss: function (type, isViewWH) {
        try { parent.BlockUI(); } catch (e) { }
        var reportGrid = $("#wlCurrencyByDateReport");
        reportGrid.addClass("hidden");
        if (type == 2) {
            var date = yesterday.split(' ');
            $('#txtfromdate').val(date[0]);
            $('#txttodate').val(date[0]);
        }
        else if (type == 3) {
            var today = currDay.split(' ')
            $('#txtfromdate').val(today[0]);
            $('#txttodate').val(today[0]);
        }
        if (isViewWH) {
            PokerWinloss.fromDate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
            PokerWinloss.toDate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        }
        else {
            PokerWinloss.fromDate = $('#txtfromdate').val();
            PokerWinloss.toDate = $('#txttodate').val();
        }
        PokerWinloss.isViewWH = isViewWH;
        var data = {
            fromDate: PokerWinloss.fromDate,
            toDate: PokerWinloss.toDate,
            isViewWH: PokerWinloss.isViewWH
        };
        $.ajax({
            type: "POST",
            url: "GetPokerWinloss",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'P2P Win/Loss From ' + PokerWinloss.fromDate + ' To ' + PokerWinloss.toDate;
                $("#mainPokerWinLossReport_Title").html(title);
                if (rs && rs.ListPokerWLDate && rs.ListPokerWLDate.length > 0) {
                    PokerWinloss.renderWinLossReport_MainReport(rs, isViewWH);
                }
                else {
                    PokerWinloss.renderWinLossReport_NoData();
                }
            },
            error: function () {
                PokerWinloss.renderWinLossReport_NoData();
            },
            complete: function () {
                PokerWinloss.renderWinLossReport();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    },

    renderWinLossReport_MainReport: function (rs, type) {

        var frozenHeader = '';
        var scrollContent = '';
        var rowHeaderHTML = '';

        var wlPokerByDateList = rs.ListPokerWLDate;
        var wlPokerByDateLength = wlPokerByDateList.length;
        for (var i = 0; i < wlPokerByDateLength; i++) {
            var pokerByDate = wlPokerByDateList[i];
            var pokerDetailList = pokerByDate.ListPokerWLDetail;
            var pokerDetailLength = pokerDetailList.length;
            for (var j = 0; j < pokerDetailLength; j++) {
                var pokerDetail = pokerDetailList[j];
                if (j == 0) {
                    rowHeaderHTML = '<td rowspan="' + pokerDetailLength + '"><div class="col80 c">' + pokerByDate.Date + '</div></td>';
                }
                else {
                    rowHeaderHTML = '';
                }
                rowHeaderHTML += '<td>\
                                    <div class="col100 c">\
                                        <div style="color:#0061DB" class="currencyLink" onclick="PokerWinlossDetail.showPokerWLByCurrencyByDate(\'' + pokerDetail.Name + '\',' + pokerDetail.CurrencyId + ',\'' + pokerByDate.Date + '\',' + type + ');">' + pokerDetail.Name +
                                        '</div>\
                                    </div>\
                                  </td>';
                frozenHeader += '<tr>' + rowHeaderHTML + '</tr>';
                scrollContent += '<tr>' +
                                    rowHeaderHTML +
                                    '<td><div class="col80 r">' + pokerDetail.GrossComm + '</div></td>\
                                    <td><div class="col80 r">' + pokerDetail.MemberWL + '</div></td>\
                                    <td><div class="col80 r">' + pokerDetail.MemberComm + '</div></td>\
                                    <td><div class="col80 r">' + pokerDetail.TotalMember + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.AgentComm) < 0 ? 'red' : '0') + '">' + pokerDetail.AgentComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.MasterComm) < 0 ? 'red' : '0') + '">' + pokerDetail.MasterComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.SuperComm) < 0 ? 'red' : '0') + '">' + pokerDetail.SuperComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.CompanyComm) < 0 ? 'red' : '0') + '">' + pokerDetail.CompanyComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.GrossCommBase) < 0 ? 'red' : '0') + '">' + pokerDetail.GrossCommBase + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(pokerDetail.CompanyCommBase) < 0 ? 'red' : '0') + '">' + pokerDetail.CompanyCommBase + '</div></td>\
                                </tr>';
            }
        }

        var total = rs.Total;
        var totalLength = total.length;
        for (var k = 0; k < totalLength; k++) {
            var totalDetail = total[k];
            if (k == 0) {
                rowHeaderHTML = '<td rowspan="' + totalLength + '"><div class="col108 c b" style="">Total</div></td>';
            }
            else {
                rowHeaderHTML = '';
            }
            rowHeaderHTML += '<td><div class="col100 c" style="">' + totalDetail.Name + '</div></td>';
            frozenHeader += '<tr class="vsTotal">' + rowHeaderHTML + '</tr>';
            scrollContent += '<tr class="vsTotal">' +
                                rowHeaderHTML +
                                '<td><div class="col80 r">' + totalDetail.GrossComm + '</div></td>\
                                <td><div class="col80 r">' + totalDetail.MemberWL + '</div></td>\
                                <td><div class="col80 r">' + totalDetail.MemberComm + '</div></td>\
                                <td><div class="col80 r">' + totalDetail.TotalMember + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.AgentComm) < 0 ? 'red' : '0') + '">' + totalDetail.AgentComm + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.MasterComm) < 0 ? 'red' : '0') + '">' + totalDetail.MasterComm + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.SuperComm) < 0 ? 'red' : '0') + '">' + totalDetail.SuperComm + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.CompanyComm) < 0 ? 'red' : '0') + '">' + totalDetail.CompanyComm + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.GrossCommBase) < 0 ? 'red' : '0') + '">' + totalDetail.GrossCommBase + '</div></td>\
                                <td><div class="col80 r ' + (parseFloat(totalDetail.CompanyCommBase) < 0 ? 'red' : '0') + '">' + totalDetail.CompanyCommBase + '</div></td>\
                            </tr>';
        }

        var frozenFooter = '';
        var scrollFooterContent = '';
        var rowFooterHTML = '';
        rowHeaderHTML = '<th colspan="2"><div class="r b" style="width:185px; height:17px">&nbsp;</div></th>';
        frozenFooter += '<tr class="vsGrandTotal">' + rowHeaderHTML + '</tr>';
        scrollFooterContent += '<tr class="vsGrandTotal">' +
                                rowHeaderHTML +
                                '<th colspan="8"><div class="r b" style="width:675px; height:17px">Grand Total</div></th>\
                                <th><div class="col80 r b" style="height:17px">' + rs.GrossCommBaseGrand + '</div></th>\
                                <th><div class="col80 r b" style="height:17px">' + rs.CompanyCommBaseGrand + '</div></th>\
                            </tr>';
        frozenFooter = stringformat.removeSpaceIE9(frozenFooter);
        scrollFooterContent = stringformat.removeSpaceIE9(scrollFooterContent);
        frozenHeader = stringformat.removeSpaceIE9(frozenHeader);
        scrollContent = stringformat.removeSpaceIE9(scrollContent);

        $("#mainPokerWinLossReport").find(".row_footer_content thead").empty().append(frozenFooter);
        $("#mainPokerWinLossReport").find(".col_footer_content thead").empty().append(scrollFooterContent);
        $("#mainPokerWinLossReport_RowHeader tbody").empty().append(frozenHeader);

        $("#mainPokerWinLossReport_Container tbody").empty().append(scrollContent);

    },

    renderWinLossReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 17px;" colspan="2">&nbsp</td>\
                    <td class="b c" style="height: 17px;" colspan="10">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 17px;" colspan="2">&nbsp</td></tr>';
        frozenHeader = stringformat.removeSpaceIE9(frozenHeader);
        html = stringformat.removeSpaceIE9(html);
        $("#mainPokerWinLossReport_RowHeader tbody").empty().append(frozenHeader);
        $("#mainPokerWinLossReport_Container tbody").empty().append(html);

        var reportGrid = $("#mainPokerWinLossReport");
        reportGrid.find(".row_footer_content thead").empty();
        reportGrid.find(".col_footer_content thead").empty();
        return html;
    },

    renderWinLossReport: function () {
        var gridOptsWinLossReport = {
            tblContentId: 'mainPokerWinLossReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            gridWidth: 375,
            gridHeight: 100,
            freezeWidth: 185,
            numberFreezeCol: 2
        };
        var reportGrid = $("#mainPokerWinLossReport");
        reportGrid.removeClass("hidden");
        reportGrid.gridScroller(gridOptsWinLossReport);
    },

    exportMainReport: function (exportType) {

        var url = 'LoadPokerWinloss?ExportType=' + exportType + '&FromDate=' + PokerWinloss.fromDate +
                  '&ToDate=' + PokerWinloss.toDate + '&isViewWH=' + PokerWinloss.isViewWH;
        $('#frm').attr('src', url);
    }


};

var PokerWinlossDetail = {
    currency: 0,
    reportDate: '',
    isViewWH: false,
    showPokerWLByCurrencyByDate: function (currency, currencyId, reportDate, type) {
        try { parent.BlockUI(); } catch (e) { }
        var reportGrid = $("#mainPokerWinLossReport");
        reportGrid.addClass("hidden");
        var gridOptsWLByDateReport = {
            tblContentId: 'wlCurrencyByDateReport_Container',
            isShowPager: false
        };
        $("#wlCurrencyByDateReport").initGrid(gridOptsWLByDateReport);
        $("#wlCurrencyByDateReport_ExportExcel").click(function () {
            PokerWinlossDetail.exportMainReport("CurrencyByDate");
        });
        PokerWinlossDetail.currency = currencyId;
        PokerWinlossDetail.reportDate = reportDate;
        PokerWinlossDetail.isViewWH = type;
        var data = {
            date: PokerWinlossDetail.reportDate,
            currency: PokerWinlossDetail.currency,
            isViewWH : PokerWinlossDetail.isViewWH
        };
        
          $.ajax({
            type: "POST",
            url: "GetPokerWinlossDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
               
                var title = 'P2P Win Loss Detail - ' + PokerWinlossDetail.reportDate + " - " + currency;
                
                $("#wlCurrencyByDateReport_Title").html(title);
                if (rs && rs.PokerWinlossDetailList && rs.PokerWinlossDetailList.length > 0) {
                    PokerWinlossDetail.renderWinLossByDateReport_MainReport(rs);
                }
                else {
                    PokerWinlossDetail.renderWinLossByDateReport_NoData();
                }
            },
            error: function () {
                PokerWinlossDetail.renderWinLossByDateReport_NoData();
            },
            complete: function () {
                PokerWinlossDetail.renderWinLossByDateReport();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
     renderWinLossByDateReport_MainReport: function (rs) {
        var frozenHeader = '';
        var scrollContent = '';
        var rowHeaderHTML = '';

        var wlDetailList = rs.PokerWinlossDetailList;
        var wlDetailListLength = wlDetailList.length;
        for (var i = 0; i < wlDetailListLength; i++) {
            var wlDetail = wlDetailList[i];
            rowHeaderHTML = '<td><div class="col100 c">' + wlDetail.Name + '</div></td>';
            frozenHeader += '<tr>' + rowHeaderHTML + '</tr>';
            scrollContent += '<tr>' +
                                    rowHeaderHTML +
                                    '<td><div class="col80 r">' + wlDetail.GrossComm + '</div></td>\
                                    <td><div class="col80 r">' + wlDetail.MemberWL + '</div></td>\
                                    <td><div class="col80 r">' + wlDetail.MemberComm + '</div></td>\
                                    <td><div class="col80 r">' + wlDetail.TotalMember + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.AgentComm) < 0 ? 'red' : '0') + '">' + wlDetail.AgentComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.MasterComm) < 0 ? 'red' : '0') + '">' + wlDetail.MasterComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.SuperComm) < 0 ? 'red' : '0') + '">' + wlDetail.SuperComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.CompanyComm) < 0 ? 'red' : '0') + '">' + wlDetail.CompanyComm + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.GrossCommBase) < 0 ? 'red' : '0') + '">' + wlDetail.GrossCommBase + '</div></td>\
                                    <td><div class="col80 r ' + (parseFloat(wlDetail.CompanyCommBase) < 0 ? 'red' : '0') + '">' + wlDetail.CompanyCommBase + '</div></td>\
                                </tr>';
        }

        var frozenFooter = '';
        var scrollFooterContent = '';
        var rowFooterHTML = '';
        rowHeaderHTML = '<th colspan="2"><div class="r b" style="width: 185px;">&nbsp;</div></th>';
        frozenFooter += '<tr class="vsGrandTotal">' + rowHeaderHTML + '</tr>';
        scrollFooterContent += '<tr class="vsGrandTotal">' +
                                rowHeaderHTML +
                                '<th colspan="7"><div class="r b" style="width:590px">Grand Total</div></th>\
                                <th><div class="col80 r b">' + rs.GrossCommBaseGrand + '</div></th>\
                                <th><div class="col80 r b">' + rs.CompanyCommBaseGrand + '</div></th>\
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
                    <td class="b c" style="height: 19px;" colspan="10">There is no data.</td>\
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
            //gridWidth: 815,
            freezeWidth: 100,
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

        var reportGrid = $("#mainPokerWinLossReport");
        reportGrid.removeClass("hidden");
    },

    exportMainReport: function (exportType) {
        var url = 'LoadPokerWinloss?ExportType=' + exportType + '&ReportDate=' + PokerWinlossDetail.reportDate +
                  '&Currency=' + PokerWinlossDetail.currency + '&isViewWH=' + PokerWinlossDetail.isViewWH;
        $('#frm').attr('src', url);
    }

};
var isInitedMonthlyCalendar = false;
function SwitchCalendarType(type) {
    var reportGrid = $("#mainPokerWinLossReport");
    reportGrid.addClass("hidden");
    var wlCurrencyByDateReport = $("#wlCurrencyByDateReport");
    wlCurrencyByDateReport.addClass("hidden");
    switch (type) {
        case 1:
            $('#tblDate1').hide();
            $('#tblDate2').show();
            if (!isInitedMonthlyCalendar) {
                isInitedMonthlyCalendar = true;
                Helper.initCalendar(beforeyesterday);
            }
            break;
        case 2:
            $('#tblDate2').hide();
            $('#tblDate1').show();
            break;
    }
}