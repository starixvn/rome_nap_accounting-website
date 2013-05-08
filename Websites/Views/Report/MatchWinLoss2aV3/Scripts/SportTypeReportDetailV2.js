

var sportTypeReportDetailV2 = {
    isFirstLoad: true,
    reportWidth: 0,
    reportDate: '',
    sportName: '',
    frozenSportTypeReport: '',
    scrollSportTypeReport: '',
    frozenSportTypeReport_Footer: '',
    scrollSportTypeReport_Footer: '',
    initPage: function (reportDate, sportName) {
        sportTypeReportDetailV2.reportDate = reportDate;
        sportTypeReportDetailV2.sportName = sportName;
        sportTypeReportDetailV2.reportWidth = 1130;

        $('#sportReportDetail_ExportExcel').click(function () {
            sportTypeReportDetailV2.exportExcel('SportTypeReportDetail');
        });

        var gridOptsSportReportDetail = {
            tblContentId: 'sportReportDetail_Container',
            isShowPager: false
        };
        $("#sportReportDetail").initGrid(gridOptsSportReportDetail);
        sportTypeReportDetailV2.viewReport(reportDate, sportName);
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
                    sportTypeReportDetailV2.renderSportTypeReport_Main(rs, reportDate, sportName);
                }
                else {
                    sportTypeReportDetailV2.renderSportTypeReport_NoData();
                }
            },
            error: function () {
                sportTypeReportDetailV2.renderSportTypeReport_NoData();
            },
            complete: function () {
                sportTypeReportDetailV2.renderSportTypeReport(reportDate, sportName);
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
        sportTypeReportDetailV2.scrollSportTypeReport = '';
        sportTypeReportDetailV2.frozenSportTypeReport = '';
        sportTypeReportDetailV2.scrollSportTypeReport_Footer = '';
        sportTypeReportDetailV2.frozenSportTypeReport_Footer = '';

        var betTypeGroupLength = rs.BetTypeGroup.length;
        for (var i = 0; i < betTypeGroupLength; i++) {
            var betTypeGroupDetail = rs.BetTypeGroup[i];
            var betTypeGroupDetailLength = betTypeGroupDetail.ReportDetails.length;
            var betTypeGroupTotalDetail = betTypeGroupDetail.Total;
            for (var j = 0; j < betTypeGroupDetailLength; j++) {
                var reportDetail = betTypeGroupDetail.ReportDetails[j];
                sportTypeReportDetailV2.renderSportTypeReport_Row(reportDetail, "BetTypeDetail");
            }
            sportTypeReportDetailV2.renderSportTypeReport_Row(betTypeGroupTotalDetail, "BetTypeTotal");
        }
        sportTypeReportDetailV2.renderSportTypeReport_Row(rs.Total, "SportTotal");

        sportTypeReportDetailV2.frozenSportTypeReport = StringFormatter.removeSpaceIE9(sportTypeReportDetailV2.frozenSportTypeReport);
        $("#sportReportDetail_RowHeader tbody").empty().append(sportTypeReportDetailV2.frozenSportTypeReport);
        sportTypeReportDetailV2.scrollSportTypeReport = StringFormatter.removeSpaceIE9(sportTypeReportDetailV2.scrollSportTypeReport);
        $("#sportReportDetail_Container tbody").empty().append(sportTypeReportDetailV2.scrollSportTypeReport);
        $("#sportReportDetail").find(".row_footer_content thead").empty().append(sportTypeReportDetailV2.frozenSportTypeReport_Footer);
        $("#sportReportDetail").find(".col_footer_content thead").empty().append(sportTypeReportDetailV2.scrollSportTypeReport_Footer);
    },
    renderSportTypeReport_Row: function (detail, rowType) {
        var css = '';
        var rowHeaderHtml = '';
        var scrollcontentHtml = '';
        switch (rowType) {
            case 'BetTypeDetail':
                rowHeaderHtml = '<td>\
                                    <div class="col120 c" style="">' +
                                        sportTypeReportDetailV2.getBetTypeName(detail.LiveIndicator.Id, detail.BetType.Name, detail.MatchCode) + '</div>\
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
                rowHeaderHtml += '<td>\
                                        <div class="col80 r">' + detail.WinLost + '</div>\
                                    </td>\
                                    <td>\
                                        <div class="col80 r">' + detail.Position + '</div>\
                                    </td>';
                break;
            case 'BetTypeTotal':
                css = 'total1';
                rowHeaderHtml = '<td colspan="6" class="">\
                                        <div class="r b" style="height: 17px;">' + detail.Name + '</div>\
                                 </td>';
                break;
            case 'SportTotal':
                css = 'title';
                rowHeaderHtml = '<td colspan="6" class="">\
                                        <div class="r b" style="width: 535px; height: 17px;">' + detail.Name + '</div>\
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
            sportTypeReportDetailV2.scrollSportTypeReport += scrollContent;
            sportTypeReportDetailV2.frozenSportTypeReport += frozenContent;
        } else {
            sportTypeReportDetailV2.frozenSportTypeReport_Footer += frozenContent;
            sportTypeReportDetailV2.scrollSportTypeReport_Footer += scrollContent;
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
            gridWidth: sportTypeReportDetailV2.reportWidth,
            freezeWidth: 535,
            freezeHeight: 50,
            numberFreezeCol: 4
        };
        $("#sportReportDetail").removeClass("hidden");
        $("#sportReportDetail").gridScroller(gridOptsSportReportDetail);
        var innerFrame = $("#innerFrame", parent.document.body);
        innerFrame.width(innerFrame.width() - 10);
        var reportDetail = $("#sportReportDetail");
        reportDetail.width(reportDetail.width() - 2);
    },
    exportExcel: function (exportType) {
        var reportDate = '';
        switch (exportType) {
            case 'sportTypeReportDetail':
                reportDate = sportTypeReportDetailV2.reportDate;
                break;
        }
        var url = 'sportTypeReportDetail?ExportType=' + exportType +
                  '&ReportDate=' + reportDate + '&SportName=' + sportTypeReportDetailV2.sportName;
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    if (sportTypeReportDetailV2.isFirstLoad) {
        sportTypeReportDetailV2.initPage(reportDate, sportName);
        sportTypeReportDetailV2.isFirstLoad = false;
    }
}); 
