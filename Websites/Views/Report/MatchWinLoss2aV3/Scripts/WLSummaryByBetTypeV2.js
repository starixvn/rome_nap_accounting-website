


var wlSummaryByBetTypeV2 = {
    reportHeight: 0,
    isViewReportComplete: false,
    viewSummaryReportByBetType: function () {
        wlSummaryByBetTypeV2.isViewReportComplete = false;
        var data = {
            'reportDate': matchWinLoss2aV2.lastReportDate
        };
        matchWinLoss2aUtils.blockUI();
        ClosePopup();
        matchWinLoss2aUtils.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "WLSummaryByBetType/SelSummaryReportByBetType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                wlSummaryByBetTypeV2.renderWLSummaryByBetTypeReport(rs, matchWinLoss2aV2.lastReportDate);
            },
            error: function () {
                wlSummaryByBetTypeV2.renderWLSummaryByBetTypeReport_NoData();
            },
            complete: function () {
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryByBetTypeReport_RowHeader");
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryByBetTypeReport_Container");
                var gridOptsWLSummaryByBetTypeReport = {
                    tblContentId: 'wlSummaryByBetTypeReport_Container',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: false,
                    gridHeight: wlSummaryByBetTypeV2.reportHeight,
                    freezeWidth: 120,
                    freezeHeight: 200,
                    numberFreezeCol: 1
                };
                $("#wlSummaryByBetTypeReport").removeClass("hidden");
                $("#wlSummaryByBetTypeReport").gridScroller(gridOptsWLSummaryByBetTypeReport);
                wlSummaryByBetTypeV2.make2TableHeaderSameHeight($("#wlSummaryByBetTypeReport").find(".free-cell-top-left"), $("#wlSummaryByBetTypeReport").find(".row_header_content"));

                wlSummaryByBetTypeV2.isViewReportComplete = true;
                if (matchWinLossDetailV2.isViewReportComplete == true) {
                    matchWinLoss2aUtils.unblockUI();
                }
            }
        });
    },
    renderWLSummaryByBetTypeReport: function (rs, reportDate) {
        if (rs && rs.ReportDetails.length > 0) {
            wlSummaryByBetTypeV2.renderWLSummaryByBetTypeReport_Main(rs, "", reportDate);
        } else {
            wlSummaryByBetTypeV2.renderWLSummaryByBetTypeReport_NoData();
        }
    },
    renderWLSummaryByBetTypeReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="1">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="13">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="1">&nbsp</td></tr>';
        $("#wlSummaryByBetTypeReport_RowHeader tbody").empty().append(frozenHeader);
        $("#wlSummaryByBetTypeReport_Container tbody").empty().append(html);

        $("#wlSummaryByBetTypeReport").find(".row_footer_content thead").empty();
        $("#wlSummaryByBetTypeReport").find(".col_footer_content thead").empty();
        matchWinLossDetailV2.renderWLSummaryBySportTypeReport_NoData();
        return html;
    },
    renderWLSummaryByBetTypeReport_Main: function (rs, cssClass, reportDate) {
        var mainHTML = '';
        var frozenHTML = '';
        var reportDetailsLength = rs.ReportDetails.length;
        var classSuffix = '';
        for (var i = 0; i < reportDetailsLength; i++) {
            var reportDetail = rs.ReportDetails[i];
            if ($.trim(reportDetail.BetType) == "Mix Parlay") {
                mainHTML += '<tr class="' + cssClass + '">\
                                <td><div class="c col120"><span style="color:#0066CC; cursor:pointer" onclick="wlSummaryByBetTypeV2.showMixparlayDetail(\'' + reportDate + '\',\'' + reportDetail.BetTypeId + '\',\'' + reportDetail.BetType + '\')">' + reportDetail.BetType + '</span></div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Soccer_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Soccer_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Soccer + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Soccer + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Others_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Others_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Others + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Others + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotalReal + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotalVirtual + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.Variance + '</div></td>\
                                <td><div class="r col100" style="">' + reportDetail.Turnover + '</div></td>\
                                <td><div class="r col60" style="">' + reportDetail.Margin + '&nbsp;&nbsp;</div></td>\
                </tr>';

                frozenHTML += '<tr class="' + cssClass + '">\
                                    <td><div class="c col120"><span style="color:#0066CC; cursor:pointer" onclick="wlSummaryByBetTypeV2.showMixparlayDetail(\'' + reportDate + '\',\'' + reportDetail.BetTypeId + '\',\'' + reportDetail.BetType + '\')">' + reportDetail.BetType + '</span></div></td>\
                               </tr>';

            }
            else {
                var betTypeName = reportDetail.BetType + wlSummaryByBetTypeV2.getBetTypeSuffix(reportDetail.BetTypeId);
                mainHTML += '<tr class="' + cssClass + '">\
                                <td><div class="c col120" style="' + (betTypeName == '' ? "height: 17px;" : "") + '">' + betTypeName + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Soccer_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Soccer_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Soccer + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Soccer + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Others_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Others_Live + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotal_Others + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.VirtualLineTotal_Others + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotalReal + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.LineTotalVirtual + '</div></td>\
                                <td><div class="r col80" style="">' + reportDetail.Variance + '</div></td>\
                                <td><div class="r col100" style="">' + reportDetail.Turnover + '</div></td>\
                                <td><div class="r col60" style="">' + reportDetail.Margin + '&nbsp;&nbsp;</div></td>\
                         </tr>';
                frozenHTML += '<tr class="' + cssClass + '">\
                                 <td><div class="c col120" style="' + (betTypeName == '' ? "height: 17px;" : "") + '">' + betTypeName + '</div></td>\
                               </tr>';
            }
        }
        frozenHTML = StringFormatter.removeSpaceIE9(frozenHTML);
        mainHTML = StringFormatter.removeSpaceIE9(mainHTML);
        $("#wlSummaryByBetTypeReport_RowHeader tbody").empty().append(frozenHTML);
        $("#wlSummaryByBetTypeReport_Container tbody").empty().append(mainHTML);
        wlSummaryByBetTypeV2.renderWLSummaryByBettypeReport_Footer(rs.Total);
    },
    renderWLSummaryByBettypeReport_Footer: function (reportDetail) {
        var mainHTML = '';
        var frozenHTML = '';
        mainHTML += '<tr>\
                        <th><div class="c col120" style="height: 39px;">' + reportDetail.BetType + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotal_Soccer_Live + '</div></th>\
                        <th><div class="r col80">' + reportDetail.VirtualLineTotal_Soccer_Live + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotal_Soccer + '</div></th>\
                        <th><div class="r col80">' + reportDetail.VirtualLineTotal_Soccer + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotal_Others_Live + '</div></th>\
                        <th><div class="r col80">' + reportDetail.VirtualLineTotal_Others_Live + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotal_Others + '</div></th>\
                        <th><div class="r col80">' + reportDetail.VirtualLineTotal_Others + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotalReal + '</div></th>\
                        <th><div class="r col80">' + reportDetail.LineTotalVirtual + '</div></th>\
                        <th><div class="r col80">' + reportDetail.Variance + '</div></th>\
                        <th><div class="r col100">' + reportDetail.Turnover + '</div></th>\
                        <th><div class="r col60">' + reportDetail.Margin + '&nbsp;&nbsp;</div></th>\
                    </tr>';
        frozenHTML += '<tr>\
                            <th><div class="c col120 table-cell" style="height: 39px;">' + reportDetail.BetType + '</div></th>\
                       </tr>';

        $("#wlSummaryByBetTypeReport").find(".row_footer_content thead").empty().append(frozenHTML);
        $("#wlSummaryByBetTypeReport").find(".col_footer_content thead").empty().append(mainHTML);
    },
    getBetTypeSuffix: function (betTypeId) {
        var suffix = '';
        switch (betTypeId) {
            case 1201:
            case 1203:
            case 1204:
            case 1205:
            case 1206:
            case 1224:
                suffix = "<br/> (Virtual Sports)";
                break;
        }

        if (1201 <= betTypeId && betTypeId <= 1299) {
            suffix = "<br/> (Virtual Sports)";
        }

        return suffix;
    },
    showMixparlayDetail: function (reportDate, betTypeId, betType) {
        ClosePopup();
        if (new Date(reportDate) < new Date(beginSupportMPDate)) {
            var mixParlayReportUrl = '../MixParlay/MixParlayPopUp?reportDate=' + reportDate +
                                        '&BetTypeId=' + betTypeId +
                                        '&BetType=' + betType;
            OpenPopup(mixParlayReportUrl, "", 800, 600, false, false, window, 3000, null, null);

        }
        else {

            var mixParlayReportUrl = '../MixParlay/DailyParlayWinLoss?reportDate=' + reportDate +
                                        '&IsRedirect=true' +
                                        '&BetTypeId=' + betTypeId +
                                        '&BetType=' + betType;
            window.location = mixParlayReportUrl;

        }
    },
    make2TableHeaderSameHeight: function (tableA, tableB) {
        var rowsA = tableA.find("thead tr");
        var rowsB = tableB.find("thead tr");
        var numberOfRows = (rowsA.length < rowsB.length) ? rowsA.length : rowsB.length;
        for (i = 0; i < numberOfRows; i++) {
            var currentRowA = $(rowsA[i]);
            var currentRowB = $(rowsB[i]);
            var cellsA = currentRowA.find("th");
            var cellsB = currentRowB.find("th");
            var cellsALength = (cellsA ? cellsA.length : 0);
            var cellsBLength = (cellsB ? cellsB.length : 0);
            var numberOfCells = (cellsALength < cellsBLength) ? cellsALength : cellsBLength;
            for (j = 0; j < numberOfCells; j++) {
                var currentHeightA = $(cellsA[j]).height();
                var currentHeightB = $(cellsB[j]).height();
                if (currentHeightA > currentHeightB) {
                    $(cellsB[j]).css("height", currentHeightA + 6 + "px").css("vertical-align", "middle");
                }
                else if (currentHeightA != currentHeightB) {
                    $(cellsA[j]).css("height", currentHeightB + "px").css("vertical-align", "middle");
                }
            }
        }
    }
}