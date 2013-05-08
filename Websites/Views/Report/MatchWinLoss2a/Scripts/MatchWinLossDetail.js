



var matchWinLossDetail = {
    frozenSportTypeReport: '',
    scrollSportTypeReport: '',
    frozenSportTypeReport_Footer: '',
    scrollSportTypeReport_Footer: '',
    reportHeight: 0,
    isViewReportComplete: false,
    viewSummaryReportBySportType: function () {
        matchWinLossDetail.isViewReportComplete = false;
        var data = {
            'reportDate': matchWinLoss2a.lastReportDate
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchWinLoss/SelSummaryReportBySportType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                matchWinLossDetail.renderWLSummaryBySportTypeReport(rs, matchWinLoss2a.lastReportDate);
            },
            error: function () {
                matchWinLossDetail.renderWLSummaryBySportTypeReport_NoData();
            },
            complete: function () {
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryBySportTypeReport_RowHeader");
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryBySportTypeReport_Container");
                var gridOptsWLSummaryBySportReport = {
                    tblContentId: 'wlSummaryBySportTypeReport_Container',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: false,
                    gridHeight: matchWinLossDetail.reportHeight,
                    gridWidth: 648,
                    freezeWidth: 170,
                    freezeHeight: 183,
                    numberFreezeCol: 1
                };
                $("#wlSummaryBySportTypeReport").removeClass("hidden");
                $("#wlSummaryBySportTypeReport").gridScroller(gridOptsWLSummaryBySportReport);
                matchWinLoss2aUtils.enableReportButton();

                matchWinLossDetail.isViewReportComplete = true;
                if (wlSummaryByBetType.isViewReportComplete == true) {
                    matchWinLoss2aUtils.unblockUI();
                }
            }
        });
    },
    renderWLSummaryBySportTypeReport: function (rs, reportDate) {
        if (rs && (rs.NonThirdPartyDetails.length > 0 || (rs.ThirdPartyDetails != null &&
                    rs.ThirdPartyDetails.ReportDetails.length > 0))) {
            matchWinLossDetail.renderWLSummaryBySportTypeReport_Main(rs, reportDate);
        } else {
            matchWinLossDetail.renderWLSummaryBySportTypeReport_NoData();
        }
    },
    renderWLSummaryBySportTypeReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="1">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="7">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="1">&nbsp</td></tr>';
        $("#wlSummaryBySportTypeReport_RowHeader tbody").empty().append(frozenHeader);
        $("#wlSummaryBySportTypeReport_Container tbody").empty().append(html);

        $("#wlSummaryBySportTypeReport").find(".row_footer_content thead").empty();
        $("#wlSummaryBySportTypeReport").find(".col_footer_content thead").empty();
        return html;
    },
    renderWLSummaryBySportTypeReport_Main: function (rs, reportDate) {
        var nonThirdPartyLength = rs.NonThirdPartyDetails.length;
        matchWinLossDetail.scrollSportTypeReport = '';
        matchWinLossDetail.frozenSportTypeReport = '';
        matchWinLossDetail.scrollSportTypeReport_Footer = '';
        matchWinLossDetail.frozenSportTypeReport_Footer = '';
        for (var i = 0; i < nonThirdPartyLength; i++) {
            var sportDetail = rs.NonThirdPartyDetails[i];
            var sportDetailLength = sportDetail.ReportDetails.length;
            var sportTotalDetail = sportDetail.Total;
            for (var j = 0; j < sportDetailLength; j++) {
                var reportDetail = sportDetail.ReportDetails[j];
                matchWinLossDetail.renderWLSummaryBySportTypeReport_Row(reportDetail, "BetTypeSummary", reportDate, sportTotalDetail.Name);
            }
            matchWinLossDetail.renderWLSummaryBySportTypeReport_Row(sportTotalDetail, "SportTypeSummary", reportDate);
        }
        matchWinLossDetail.renderWLSummaryBySportTypeReport_Row(rs.NonThirdPartyGrandTotal, "GrandTotalNon3rdSport");
        if (rs.IsEightRMActive && rs.IsEightRMActive == true) {
            matchWinLossDetail.renderWLSummaryBySportTypeReport_RMRow(rs.EightRMTotal, rs.EightRMName, rs.EightRMTurnover, rs.EightRMMargin);
        }
        if (rs.IsBARMActive && rs.IsBARMActive == true) {
            matchWinLossDetail.renderWLSummaryBySportTypeReport_RMRow(rs.BARMTotal, rs.BARMName, rs.BARMTurnover, rs.BARMMargin);
        }
        var thirdPartyLength = rs.ThirdPartyDetails.ReportDetails.length;
        for (var k = 0; k < thirdPartyLength; k++) {
            var thirdDetail = rs.ThirdPartyDetails.ReportDetails[k];
            matchWinLossDetail.renderWLSummaryBySportTypeReport_Row(thirdDetail, "ThirdParty");
        }
        matchWinLossDetail.renderWLSummaryBySportTypeReport_Row(rs.Total, "GrandTotal");
        matchWinLossDetail.frozenSportTypeReport = StringFormatter.removeSpaceIE9(matchWinLossDetail.frozenSportTypeReport);
        matchWinLossDetail.scrollSportTypeReport = StringFormatter.removeSpaceIE9(matchWinLossDetail.scrollSportTypeReport);
        $("#wlSummaryBySportTypeReport_RowHeader tbody").empty().append(matchWinLossDetail.frozenSportTypeReport);
        $("#wlSummaryBySportTypeReport_Container tbody").empty().append(matchWinLossDetail.scrollSportTypeReport);
        $("#wlSummaryBySportTypeReport").find(".row_footer_content thead").empty().append(matchWinLossDetail.frozenSportTypeReport_Footer);
        $("#wlSummaryBySportTypeReport").find(".col_footer_content thead").empty().append(matchWinLossDetail.scrollSportTypeReport_Footer);
    },
    renderWLSummaryBySportTypeReport_Row: function (detail, rowType, reportDate, sportName) {
        var css = '';
        var rowHeaderHtml = '';
        switch (rowType) {
            case 'BetTypeSummary':
                var betTypeName = matchWinLossDetail.getBetTypeName(detail.LiveIndicator.Id, detail.BetType.Name);
                rowHeaderHtml = '<td>\
                                    <div class="col160 l betTypeLink" style="' + (betTypeName == '' ? "height: 17px;" : "") + '" onclick="matchWinLossDetail.viewBetTypeReportDetail(\'' + reportDate + '\',\'' + sportName + '\',\'' + detail.BetType.Id + '\',\'' + detail.BetType.Name + '\',\'' + detail.LiveIndicator.Id + '\')">' +
                                        betTypeName + '</div>\
                                  </td>';
                break;
            case 'SportTypeSummary':
                css = 'total2';
                rowHeaderHtml = '<td>\
                                     <div class="col160 r b" style="color:#0066CC; cursor:pointer;margin-left: 10px;' + (detail.Name == '' ? "height: 17px;" : "") + '" onclick="matchWinLossDetail.viewSportTypeReportDetail(\'' + reportDate + '\',\'' + detail.Name + '\')">' + detail.Name + '</div>\
                                 </td>';

                break;
            case 'GrandTotalNon3rdSport':
            case 'GrandTotal':
                css = 'title';
                rowHeaderHtml = '<td>\
                                     <div class="col160 r b" style="margin-left: 10px;">' + detail.Name + '</div>\
                                 </td>';
                break;
            case "ThirdParty":
                if (detail.ThirdPartyType == 2) {
                    rowHeaderHtml = '<td>\
                                        <div class="col160 c betTypeLink" style="' + (detail.Name == '' ? "height: 17px;" : "") + '" onclick="matchWinLossDetail.viewThirdPartyReportDetail(\'' + detail.Name + '\')">' +
                                            detail.Name + '</div>\
                                      </td>';
                }
                else {
                    rowHeaderHtml = '<td>\
                                     <div class="col160 c" style="margin-left: 10px;' + (detail.Name == '' ? "height: 17px;" : "") + '">' + detail.Name + '</div>\
                                 </td>';
                }
                break;
            default:
                break;
        }
        var scrollContent = '<tr class="' + css + '">' +
                                        rowHeaderHtml +
                                        '<td>\
                                            <div class="col80 r">' + detail.Commission + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + detail.VirtualComm + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r b">' + detail.LineTotal + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r b">' + detail.VirtualLineTotal + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r">' + detail.Variance + '</div>\
                                        </td>\
                                           <td>\
                                            <div class="col80 r">' + detail.Turnover + '</div>\
                                        </td>\
                                   <td>\
                                            <div class="col80 r">' + detail.Margin + '</div>\
                                        </td>\
                              </tr>';
        var frozenContent = '<tr class="' + css + '">' + rowHeaderHtml + '</tr>';
        if (rowType != 'GrandTotal') {
            matchWinLossDetail.scrollSportTypeReport += scrollContent;
            matchWinLossDetail.frozenSportTypeReport += frozenContent;
        } else {
            matchWinLossDetail.frozenSportTypeReport_Footer += scrollContent;
            matchWinLossDetail.scrollSportTypeReport_Footer += frozenContent;
        }
    },
    renderWLSummaryBySportTypeReport_RMRow: function (total, name, turnover, margin) {
        var css = 'totalRM';
        var rowHeaderHtml = '';
        rowHeaderHtml = '<td class="nonBorderRight">\
                            <div class="col160 r" style="height: 17px;"></div>\
                         </td>';

        var scrollContent = '<tr class="' + css + '">' +
                                        rowHeaderHtml +
                                        '<td colspan="2">\
                                            <div class="col160 r b">' + name + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r b">' + total + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r b">' + total + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col100 r">0.00</div>\
                                        </td>\
                                           <td>\
                                            <div class="col80 r">' + turnover + '</div>\
                                        </td>\
                                        <td>\
                                            <div class="col80 r">' + margin + '</div>\
                                        </td>\
                                    </tr>';
        var frozenContent = '<tr class="' + css + '">' + rowHeaderHtml + '</tr>';
        matchWinLossDetail.scrollSportTypeReport += scrollContent;
        matchWinLossDetail.frozenSportTypeReport += frozenContent;
    },
    getBetTypeName: function (liveType, bettypeName) {
        var displayBetTypeName = '';
        switch (liveType) {
            case 1:
                displayBetTypeName = '<span class="b">!Live</span> - ';
                break;
        }
        displayBetTypeName += bettypeName;
        return displayBetTypeName;
    },
    viewSportTypeReportDetail: function (reportDate, sportName) {
        ClosePopup();
        var casinoReportUrl = '../MatchWinLoss/SportTypeReportDetail?ReportDate=' + reportDate +
                                                         '&SportName=' + sportName;
        OpenPopup(casinoReportUrl, "", 1180, 600, false, false, window, 3000, null, null);
    },
    viewBetTypeReportDetail: function (reportDate, sportName, betTypeId, betTypeName, liveType) {
        ClosePopup();

        var casinoReportUrl = '../MatchWinLoss/BetTypeReportDetail?';
        var parameters = {
            ReportDate: reportDate,
            SportName: sportName,
            BetType: betTypeId,
            BetTypeName: betTypeName,
            LiveType: liveType
        };
        casinoReportUrl += $.param(parameters);
        OpenPopup(casinoReportUrl, "", 1180, 600, false, false, window, 3000, null, null);
    },
    viewThirdPartyReportDetail: function (sportName) {
        ClosePopup();
        if (sportName == 'Casino') {
            var casinoReportUrl = '../MatchWinLoss/ThirdPartyReportDetail?ReportDate=' + matchWinLoss2a.lastReportDate +
                                                         '&BetType=' + sportName;
            OpenPopup(casinoReportUrl, "", 880, 600, false, false, window, 3000, null, null);
        }
    }
}