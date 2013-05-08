

var sportTypeReportDetail = {
    isFirstLoad: true,
    reportWidth: 0,
    reportDate: '',
    sportName: '',
    frozenSportTypeReport: '',
    scrollSportTypeReport: '',
    frozenSportTypeReport_Footer: '',
    scrollSportTypeReport_Footer: '',
    initPage: function (reportDate, sportName) {
        sportTypeReportDetail.reportDate = reportDate;
        sportTypeReportDetail.sportName = sportName;
        sportTypeReportDetail.reportWidth = 1120;

        $('#sportReportDetail_ExportExcel').click(function () {
            sportTypeReportDetail.exportExcel('SportTypeReportDetail');
        });

        var gridOptsSportReportDetail = {
            tblContentId: 'sportReportDetail_Container',
            isShowPager: false
        };
        $("#sportReportDetail").initGrid(gridOptsSportReportDetail);
        sportTypeReportDetail.viewReport(reportDate, sportName);
    },
    viewReport: function (reportDate, sportName) {
        try {
            parent.parent.BlockUI();
        }
        catch (e) {
        }
        var data = {
            reportDate: reportDate,
            sportName: sportName
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchWinLoss/SelSportTypeReportDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = sportName + ' PRODUCT WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#sportReportDetail_Title").html(title);
                if (rs != null && rs.BetTypeGroup != null &&
                                  rs.BetTypeGroup.length > 0) {
                    sportTypeReportDetail.renderSportTypeReport_Main(rs, reportDate, sportName);
                }
                else {
                    sportTypeReportDetail.renderSportTypeReport_NoData();
                }
            },
            error: function () {
                sportTypeReportDetail.renderSportTypeReport_NoData();
            },
            complete: function () {
                sportTypeReportDetail.renderSportTypeReport(reportDate, sportName);
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
    renderSportTypeReport_Main: function (rs, reportDate, sportName) {
        sportTypeReportDetail.scrollSportTypeReport = '';
        sportTypeReportDetail.frozenSportTypeReport = '';
        sportTypeReportDetail.scrollSportTypeReport_Footer = '';
        sportTypeReportDetail.frozenSportTypeReport_Footer = '';

        var betTypeGroupLength = rs.BetTypeGroup.length;
        for (var i = 0; i < betTypeGroupLength; i++) {
            var betTypeGroupDetail = rs.BetTypeGroup[i];
            var betTypeGroupDetailLength = betTypeGroupDetail.ReportDetails.length;
            var betTypeGroupTotalDetail = betTypeGroupDetail.Total;
            for (var j = 0; j < betTypeGroupDetailLength; j++) {
                var reportDetail = betTypeGroupDetail.ReportDetails[j];
                sportTypeReportDetail.renderSportTypeReport_Row(reportDetail, "BetTypeDetail");
            }
            sportTypeReportDetail.renderSportTypeReport_Row(betTypeGroupTotalDetail, "BetTypeTotal");
        }
        sportTypeReportDetail.renderSportTypeReport_Row(rs.Total, "SportTotal");

        sportTypeReportDetail.frozenSportTypeReport = StringFormatter.removeSpaceIE9(sportTypeReportDetail.frozenSportTypeReport);
        $("#sportReportDetail_RowHeader tbody").empty().append(sportTypeReportDetail.frozenSportTypeReport);
        sportTypeReportDetail.scrollSportTypeReport = StringFormatter.removeSpaceIE9(sportTypeReportDetail.scrollSportTypeReport);
        $("#sportReportDetail_Container tbody").empty().append(sportTypeReportDetail.scrollSportTypeReport);
        $("#sportReportDetail").find(".row_footer_content thead").empty().append(sportTypeReportDetail.frozenSportTypeReport_Footer);
        $("#sportReportDetail").find(".col_footer_content thead").empty().append(sportTypeReportDetail.scrollSportTypeReport_Footer);
    },
    renderSportTypeReport_Row: function (detail, rowType) {
        var css = '';
        var rowHeaderHtml = '';
        var scrollcontentHtml = '';
        switch (rowType) {
            case 'BetTypeDetail':
                rowHeaderHtml = '<td>\
                                    <div class="col120 c" style="">' +
                                        sportTypeReportDetail.getBetTypeName(detail.LiveIndicator.Id, detail.BetType.Name, detail.MatchCode) + '</div>\
                                  </td>\
                                  <td>\
                                    <div class="col100 r" style="">' + detail.HomeName + '</div>\
                                  </td>\
                                  <td>\
                                    <div class="col30 c" style="">-vs-</div>\
                                  </td>\
                                  <td>\
                                    <div class="col100 l" style="">' + detail.AwayName + '</div>\
                                  </td>';
                scrollcontentHtml = '<td>\
                                        <div class="col80 r">' + detail.WinLost + '</div>\
                                    </td>\
                                    <td>\
                                        <div class="col80 r">' + detail.Position + '</div>\
                                    </td>';
                break;
            case 'BetTypeTotal':
                css = 'total1';
                rowHeaderHtml = '<td colspan="4" class="nonBorderRight">\
                                     <div class="c b " style="height: 17px;">&nbsp</div>\
                                 </td>';
                scrollcontentHtml = '<td colspan="2" class="">\
                                        <div class="r b" style="height: 17px;">' + detail.Name + '</div>\
                                     </td>';
                break;
            case 'SportTotal':
                css = 'title';
                rowHeaderHtml = '<td colspan="4" class="nonBorderRight" style="">\
                                     <div class="c b " style="width: 0px; height: 17px;">&nbsp</div>\
                                 </td>';
                scrollcontentHtml = '<td colspan="2" class="">\
                                        <div class="r b" style="width: 531px; height: 17px;">' + detail.Name + '</div>\
                                     </td>';
                break;
            default:
                break;
        }
        var scrollContent = '<tr class="' + css + '">' +
                                        rowHeaderHtml +
                                        scrollcontentHtml +
                                        '<td>\
                                            <div class="col80 r">' + detail.Commission + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + detail.VirtualComm + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r b">' + detail.LineTotal + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r b">' + detail.VirtualLineTotal + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + detail.Variance + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + detail.Turnover + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + detail.Margin + '</div>\
                                        </td>\
                                    </tr>';


        var frozenContent = '<tr class="' + css + '">' + rowHeaderHtml + '</tr>';
        if (rowType != 'SportTotal') {
            sportTypeReportDetail.scrollSportTypeReport += scrollContent;
            sportTypeReportDetail.frozenSportTypeReport += frozenContent;
        } else {
            sportTypeReportDetail.frozenSportTypeReport_Footer += scrollContent;
            sportTypeReportDetail.scrollSportTypeReport_Footer += frozenContent;
        }
    },
    renderSportTypeReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="4">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="9">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="4">&nbsp</td></tr>';
        $("#sportReportDetail_RowHeader tbody").empty().append(frozenHeader);
        $("#sportReportDetail_Container tbody").empty().append(html);

        $("#sportReportDetail").find(".row_footer_content thead").empty();
        $("#sportReportDetail").find(".col_footer_content thead").empty();
        return html;
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
    renderSportTypeReport: function (reportDate, sportName) {
        var gridOptsSportReportDetail = {
            tblContentId: 'sportReportDetail_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 550,
            gridWidth: sportTypeReportDetail.reportWidth,
            freezeWidth: 365,
            freezeHeight: 50,
            numberFreezeCol: 2
        };
        $("#sportReportDetail").removeClass("hidden");
        $("#sportReportDetail").gridScroller(gridOptsSportReportDetail);
    },
    exportExcel: function (exportType) {
        var reportDate = '';
        switch (exportType) {
            case 'SportTypeReportDetail':
                reportDate = sportTypeReportDetail.reportDate;
                break;
        }
        var url = 'SportTypeReportDetail?ExportType=' + exportType +
                  '&ReportDate=' + reportDate + '&SportName=' + sportTypeReportDetail.sportName;
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    if (sportTypeReportDetail.isFirstLoad) {
        sportTypeReportDetail.initPage(reportDate, sportName);
        sportTypeReportDetail.isFirstLoad = false;
    }
}); 
