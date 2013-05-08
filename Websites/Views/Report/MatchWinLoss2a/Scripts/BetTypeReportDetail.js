

var betTypeReportDetail = {
    isFirstLoad: true,
    reportWidth: 0,
    reportDate: '',
    sportName: '',
    betType: 0,
    betTypeName: '',
    liveType: 0,
    frozenBetTypeReport: '',
    scrollBetTypeReport: '',
    frozenBetTypeReport_Footer: '',
    scrollBetTypeReport_Footer: '',
    initPage: function (reportDate, sportName, betType, betTypeName, liveType) {
        betTypeReportDetail.reportDate = reportDate;
        betTypeReportDetail.sportName = sportName;
        betTypeReportDetail.betType = betType;
        betTypeReportDetail.betTypeName = betTypeName;
        betTypeReportDetail.liveType = liveType;
        betTypeReportDetail.reportWidth = 1120;

        $('#betTypeReportDetail_ExportExcel').click(function () {
            betTypeReportDetail.exportExcel('BetTypeReportDetail');
        });
        var gridOptsBetTypeReportDetail = {
            tblContentId: 'betTypeReportDetail_Container',
            isShowPager: false
        };
        $("#betTypeReportDetail").initGrid(gridOptsBetTypeReportDetail);
        betTypeReportDetail.viewReport(reportDate, sportName, betType, betTypeName, liveType);
    },
    viewReport: function (reportDate, sportName, betType, betTypeName, liveType) {
        try { parent.parent.BlockUI(); } catch (e) { }
        var data = {
            reportDate: reportDate,
            sportName: sportName,
            betType: betType,
            liveType: liveType
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchWinLoss/SelBetTypeReportDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = sportName + ' PRODUCT ' +
                            betTypeReportDetail.getLiveTypeName(liveType) + betTypeName.toUpperCase() +
                            ' BETTYPE WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#betTypeReportDetail_Title").html(title);
                if (rs != null && rs.ReportDetails != null &&
                                  rs.ReportDetails.length > 0) {
                    betTypeReportDetail.renderSportTypeReport_Main(rs, reportDate, sportName, betType, betTypeName, liveType);
                }
                else {
                    betTypeReportDetail.renderBetTypeReport_NoData();
                }
            },
            error: function () {
                betTypeReportDetail.renderBetTypeReport_NoData();
            },
            complete: function () {
                betTypeReportDetail.renderBetTypeReport(reportDate, sportName);
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
    renderSportTypeReport_Main: function (rs, reportDate, sportName, betType, betTypeName, liveType) {
        betTypeReportDetail.scrollBetTypeReport = '';
        betTypeReportDetail.frozenBetTypeReport = '';
        betTypeReportDetail.scrollBetTypeReport_Footer = '';
        betTypeReportDetail.frozenBetTypeReport_Footer = '';

        var betTypeGroupDetailLength = rs.ReportDetails.length;
        var betTypeGroupTotalDetail = rs.Total;
        for (var i = 0; i < betTypeGroupDetailLength; i++) {
            var reportDetail = rs.ReportDetails[i];
            betTypeReportDetail.renderBetTypeReport_Row(reportDetail, "BetTypeDetail");
        }
        betTypeReportDetail.renderBetTypeReport_Row(betTypeGroupTotalDetail, "BetTypeTotal");

        betTypeReportDetail.frozenBetTypeReport = StringFormatter.removeSpaceIE9(betTypeReportDetail.frozenBetTypeReport);
        betTypeReportDetail.scrollContent = StringFormatter.removeSpaceIE9(betTypeReportDetail.scrollBetTypeReport);
        $("#betTypeReportDetail_RowHeader tbody").empty().append(betTypeReportDetail.frozenBetTypeReport);
        $("#betTypeReportDetail_Container tbody").empty().append(betTypeReportDetail.scrollBetTypeReport);
        $("#betTypeReportDetail").find(".row_footer_content thead").empty().append(betTypeReportDetail.frozenBetTypeReport_Footer);
        $("#betTypeReportDetail").find(".col_footer_content thead").empty().append(betTypeReportDetail.scrollBetTypeReport_Footer);
    },
    renderBetTypeReport_Row: function (detail, rowType) {
        var css = '';
        var rowHeaderHtml = '';
        var scrollcontentHtml = '';
        switch (rowType) {
            case 'BetTypeDetail':
                rowHeaderHtml = '<td><div class="col120 c" style="">' +
                                        betTypeReportDetail.getBetTypeName(detail.LiveIndicator.Id, detail.BetType.Name, detail.MatchCode) +
                                        '</div></td><td><div class="col100 r" style="">' + detail.HomeName +
                                        '</div></td><td><div class="col30 c" style="">-vs-</div></td><td><div class="col100 l" style="">' + detail.AwayName + '</div></td>';
                scrollcontentHtml = '<td><div class="col80 r">' + detail.WinLost +
                                    '</div></td><td><div class="col80 r">' + detail.Position + '</div></td>';
                break;
            case 'BetTypeTotal':
                css = 'title';
                rowHeaderHtml = '<td colspan="4" class="nonBorderRight">\
                                     <div class="c b " style="width: 0px; height: 17px;">&nbsp</div>\
                                 </td>';
                scrollcontentHtml = '<td colspan="2" class="">\
                                        <div class="r b" style="width: 531px; height: 17px;">' + detail.Name +
                                        '</div>\
                                     </td>';
                break;
            default:
                break;
        }
        var scrollContent = '<tr class="' + css + '">' +
                                        rowHeaderHtml +
                                        scrollcontentHtml +
                                        '<td><div class="col80 r">' + detail.Commission +
                                        '</div></td><td><div class="col80 r">' + detail.VirtualComm +
                                        '</div></td><td><div class="col80 r b">' + detail.LineTotal +
                                        '</div></td><td><div class="col80 r b">' + detail.VirtualLineTotal +
                                        '</div></td><td><div class="col80 r">' + detail.Variance +
                                        '</div></td><td><div class="col80 r">' + detail.Turnover +
                                        '</div></td><td><div class="col80 r">' + detail.Margin +
                                        '</div></td></tr>';


        var frozenContent = '<tr class="' + css + '">' + rowHeaderHtml + '</tr>';
        if (rowType != 'BetTypeTotal') {
            betTypeReportDetail.scrollBetTypeReport += scrollContent;
            betTypeReportDetail.frozenBetTypeReport += frozenContent;
        } else {
            betTypeReportDetail.frozenBetTypeReport_Footer += scrollContent;
            betTypeReportDetail.scrollBetTypeReport_Footer += frozenContent;
        }
    },
    renderBetTypeReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="4">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="9">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="4">&nbsp</td></tr>';
        $("#betTypeReportDetail_RowHeader tbody").empty().append(frozenHeader);
        $("#betTypeReportDetail_Container tbody").empty().append(html);

        $("#betTypeReportDetail").find(".row_footer_content thead").empty();
        $("#betTypeReportDetail").find(".col_footer_content thead").empty();
        return html;
    },
    renderBetTypeReport: function (reportDate, sportName) {
        var gridOptsBetTypeReportDetail = {
            tblContentId: 'betTypeReportDetail_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 550,
            gridWidth: betTypeReportDetail.reportWidth,
            freezeWidth: 365,
            freezeHeight: 33,
            numberFreezeCol: 2
        };
        $("#betTypeReportDetail").removeClass("hidden");
        $("#betTypeReportDetail").gridScroller(gridOptsBetTypeReportDetail);
    },
    getBetTypeName: function (liveType, bettypeName, matchCode) {
        var displayBetTypeName = '';
        switch (liveType) {
            case 1:
                displayBetTypeName = '<span class="b">!Live</span> - ';
                break;
        }
        displayBetTypeName += bettypeName + " " + matchCode;
        return displayBetTypeName;
    },
    getLiveTypeName: function (liveType) {
        var displayLiveTypeName = '';
        switch (liveType) {
            case '1':
                displayLiveTypeName = 'LIVE-';
                break;
        }
        return displayLiveTypeName;
    },
    exportExcel: function (exportType) {
        var reportDate = '';
        switch (exportType) {
            case 'BetTypeReportDetail':
                reportDate = betTypeReportDetail.reportDate;
                break;
        }

        var url = 'BetTypeReportDetail?';
        var parameters = {
            ExportType: exportType,
            ReportDate: reportDate,
            SportName: betTypeReportDetail.sportName,
            BetType: betTypeReportDetail.betType,
            BetTypeName: betTypeReportDetail.betTypeName,
            LiveType: betTypeReportDetail.liveType
        };
        url += $.param(parameters);
        $('#frm').attr('src', url);
    }
}



$(document).ready(function () {
    if (betTypeReportDetail.isFirstLoad) {
        betTypeReportDetail.initPage(reportDate, sportName, betType, betTypeName, liveType);
        betTypeReportDetail.isFirstLoad = false;
    }
}); 
