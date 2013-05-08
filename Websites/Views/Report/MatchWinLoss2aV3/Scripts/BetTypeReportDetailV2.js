

var betTypeReportDetailV2 = {
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
        betTypeReportDetailV2.reportDate = reportDate;
        betTypeReportDetailV2.sportName = sportName;
        betTypeReportDetailV2.betType = betType;
        betTypeReportDetailV2.betTypeName = betTypeName;
        betTypeReportDetailV2.liveType = liveType;
        betTypeReportDetailV2.reportWidth = 1130;

        $('#betTypeReportDetail_ExportExcel').click(function () {
            betTypeReportDetailV2.exportExcel('BetTypeReportDetail');
        });
        var gridOptsBetTypeReportDetail = {
            tblContentId: 'betTypeReportDetail_Container',
            isShowPager: false
        };
        $("#betTypeReportDetail").initGrid(gridOptsBetTypeReportDetail);
        betTypeReportDetailV2.viewReport(reportDate, sportName, betType, betTypeName, liveType);
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
                            betTypeReportDetailV2.getLiveTypeName(liveType) + betTypeName.toUpperCase() +
                            ' BETTYPE WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#betTypeReportDetail_Title").html(title);
                if (rs != null && rs.ReportDetails != null &&
                                  rs.ReportDetails.length > 0) {
                    betTypeReportDetailV2.renderSportTypeReport_Main(rs, reportDate, sportName, betType, betTypeName, liveType);
                }
                else {
                    betTypeReportDetailV2.renderBetTypeReport_NoData();
                }
            },
            error: function () {
                betTypeReportDetailV2.renderBetTypeReport_NoData();
            },
            complete: function () {
                betTypeReportDetailV2.renderBetTypeReport(reportDate, sportName);
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
        betTypeReportDetailV2.scrollBetTypeReport = '';
        betTypeReportDetailV2.frozenBetTypeReport = '';
        betTypeReportDetailV2.scrollBetTypeReport_Footer = '';
        betTypeReportDetailV2.frozenBetTypeReport_Footer = '';

        var betTypeGroupDetailLength = rs.ReportDetails.length;
        var betTypeGroupTotalDetail = rs.Total;
        for (var i = 0; i < betTypeGroupDetailLength; i++) {
            var reportDetail = rs.ReportDetails[i];
            betTypeReportDetailV2.renderBetTypeReport_Row(reportDetail, "BetTypeDetail");
        }
        betTypeReportDetailV2.renderBetTypeReport_Row(betTypeGroupTotalDetail, "BetTypeTotal");

        betTypeReportDetailV2.frozenBetTypeReport = StringFormatter.removeSpaceIE9(betTypeReportDetailV2.frozenBetTypeReport);
        betTypeReportDetailV2.scrollContent = StringFormatter.removeSpaceIE9(betTypeReportDetailV2.scrollBetTypeReport);
        $("#betTypeReportDetail_RowHeader tbody").empty().append(betTypeReportDetailV2.frozenBetTypeReport);
        $("#betTypeReportDetail_Container tbody").empty().append(betTypeReportDetailV2.scrollBetTypeReport);
        $("#betTypeReportDetail").find(".row_footer_content thead").empty().append(betTypeReportDetailV2.frozenBetTypeReport_Footer);
        $("#betTypeReportDetail").find(".col_footer_content thead").empty().append(betTypeReportDetailV2.scrollBetTypeReport_Footer);
    },
    renderBetTypeReport_Row: function (detail, rowType) {
        var css = '';
        var rowHeaderHtml = '';
        var scrollcontentHtml = '';
        switch (rowType) {
            case 'BetTypeDetail':
                rowHeaderHtml = '<td><div class="col120 c" style="">' +
                                        betTypeReportDetailV2.getBetTypeName(detail.LiveIndicator.Id, detail.BetType.Name, detail.MatchCode) +
                                        '</div></td><td><div class="col100 r" style="">' + detail.HomeName +
                                        '</div></td><td><div class="col30 c" style="">-vs-</div></td><td><div class="col100 l" style="">' + detail.AwayName + '</div></td>';
                rowHeaderHtml += '<td><div class="col80 r">' + detail.WinLost +
                                    '</div></td><td><div class="col80 r">' + detail.Position + '</div></td>';
                break;
            case 'BetTypeTotal':
                css = 'title';
                rowHeaderHtml = '<td colspan="6" class="">\
                                     <div class="r b " style="width: 535px; height: 17px;">' + detail.Name + '</div>\
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
            betTypeReportDetailV2.scrollBetTypeReport += scrollContent;
            betTypeReportDetailV2.frozenBetTypeReport += frozenContent;
        } else {
            betTypeReportDetailV2.frozenBetTypeReport_Footer += frozenContent;
            betTypeReportDetailV2.scrollBetTypeReport_Footer += scrollContent;
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
            gridWidth: betTypeReportDetailV2.reportWidth,
            freezeWidth: 535,
            freezeHeight: 33,
            numberFreezeCol: 4
        };
        $("#betTypeReportDetail").removeClass("hidden");
        $("#betTypeReportDetail").gridScroller(gridOptsBetTypeReportDetail);
        var innerFrame = $("#innerFrame", parent.document.body);
        innerFrame.width(innerFrame.width() - 10);
        var reportDetail = $("#betTypeReportDetail");
        reportDetail.width(reportDetail.width() - 2);
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
                reportDate = betTypeReportDetailV2.reportDate;
                break;
        }

        var url = 'BetTypeReportDetail?';
        var parameters = {
            ExportType: exportType,
            ReportDate: reportDate,
            SportName: betTypeReportDetailV2.sportName,
            BetType: betTypeReportDetailV2.betType,
            BetTypeName: betTypeReportDetailV2.betTypeName,
            LiveType: betTypeReportDetailV2.liveType
        };
        url += $.param(parameters);
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    if (betTypeReportDetailV2.isFirstLoad) {
        betTypeReportDetailV2.initPage(reportDate, sportName, betType, betTypeName, liveType);
        betTypeReportDetailV2.isFirstLoad = false;
    }
}); 
