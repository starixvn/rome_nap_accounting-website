




var advanceMixParlayReport = {
    reportDate: null,
    betType: null,
    betTypeId: null,
    frozenHeader: '',
    scrollContent: '',
    reportWidth: 955,
    initPage: function (reportDate, betTypeId, betType) {

        advanceMixParlayReport.reportDate = reportDate;
        advanceMixParlayReport.betTypeId = betTypeId;
        advanceMixParlayReport.betType = betType;
        advanceMixParlayReport.getAdvanceMixParlayReport(reportDate, betTypeId, betType);
    },
    getAdvanceMixParlayReport: function (reportDate, betTypeId, betType) {

        try { parent.parent.BlockUI(); } catch (e) { }
        var isViewDW = false;
        if (reportDate <= movebetday) {
            isViewDW = true;
        }
        var data = {
            fromDate: reportDate,
            toDate: reportDate,
            isViewDW: isViewDW,
            betType: betTypeId
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MixParlay/SelAdvanceMixParlayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#advanceMixParlayWLReport_Title").html(title);
                if (rs != null && (rs.NoneLiveReport.ReportDetails.length > 0 ||
                                   rs.LiveReport.ReportDetails.length > 0 ||
                                   rs.MixLiveAndNoneLiveReport.ReportDetails.length > 0)) {
                    advanceMixParlayReport.renderAdvanceMixParlayReport_Main(rs, reportDate, betTypeId, betType);
                }
                else {
                    advanceMixParlayReport.renderAdvanceMixParlayReport_NoData();
                }
            },
            error: function () {
                advanceMixParlayReport.renderAdvanceMixParlayReport_NoData();
            },
            complete: function () {
                advanceMixParlayReport.renderAdvanceMixParlayReport(reportDate, betTypeId, betType);
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
    renderAdvanceMixParlayReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="3">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="6">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="3">&nbsp</td></tr>';
        $("#advanceMixParlayWLReport_RowHeader tbody").empty().append(frozenHeader);
        $("#advanceMixParlayWLReport_Container tbody").empty().append(html);

        $("#advanceMixParlayWLReport").find(".row_footer_content thead").empty();
        $("#advanceMixParlayWLReport").find(".col_footer_content thead").empty();
        return html;
    },
    renderAdvanceMixParlayReport_Main: function (rs, reportDate, betTypeId, betType) {
        advanceMixParlayReport.frozenHeader = '';
        advanceMixParlayReport.scrollContent = '';
        advanceMixParlayReport.renderRow(rs.LiveReport);
        advanceMixParlayReport.renderRow(rs.NoneLiveReport);
        advanceMixParlayReport.renderRow(rs.MixLiveAndNoneLiveReport);
        advanceMixParlayReport.renderAdvanceMixParlayReport_Footer(rs.Total.NormalTotal);

        advanceMixParlayReport.frozenHeader = StringFormatter.removeSpaceIE9(advanceMixParlayReport.frozenHeader);
        advanceMixParlayReport.scrollContent = StringFormatter.removeSpaceIE9(advanceMixParlayReport.scrollContent);
        $("#advanceMixParlayWLReport_RowHeader tbody").empty().append(advanceMixParlayReport.frozenHeader);
        $("#advanceMixParlayWLReport_Container tbody").empty().append(advanceMixParlayReport.scrollContent);
    },
    renderRow: function (row) {
        var scrollHtml = '';
        var frozenHtml = '';
        var reportDetails = row.ReportDetails;
        var totalItems = reportDetails.length;
        if (totalItems == 0) {
            return;
        }

        scrollHtml = '<tr>';
        frozenHtml = '<tr>';
        var css = '';
        switch (row.LiveType.Name) {
            case 'Live':
                css = "live";
                break;
            case 'Non Live':
                css = "nonlive";
                break;
            default:
                css = "mix";
                break;
        }
        scrollHtml += '<td rowspan="' + totalItems + '" class="' + css + '"><div class="col120 c b">' + row.LiveType.Name + '</div></td>'
        frozenHtml += '<td rowspan="' + totalItems + '" class="' + css + '"><div class="col120 c b">' + row.LiveType.Name + '</div></td>';

        var sportHtml = "";
        var sportHtmlFrozen = "";
        var sportRowSpan = 0;
        var numSport = 0;
        for (var i = 0; i < totalItems; i++) {
            var detail = reportDetails[i];
            switch (detail.RowType.Id) {
                case 0:
                    if (sportRowSpan > 0) {
                        sportHtml += '<tr>';
                        sportHtmlFrozen += '<tr>';
                    }
                    sportHtml += '<td><div class="col120 l">' + detail.AliasName + '</div></td>\
                                    <td><div class="col100 r">' + StringFormatter.getDisplayColorOfValue(detail.WinLoss) + '</div></td>\
                                    <td><div class="col90 r">' + StringFormatter.formatNumber(detail.Position) + '</div></td>\
                                    <td><div class="col80 r">' + StringFormatter.formatNumber(detail.Commission) + '</div></td>\
                                    <td><div class="col100 r">' + StringFormatter.getDisplayColorOfValue(detail.LineTotal) + '</div></td>\
                                    <td><div class="col100 r">' + StringFormatter.formatNumber(detail.Turnover) + '</div></td>\
                                    <td><div class="col80 r">' + (detail.Turnover.toFixed(2) != 0 ? (StringFormatter.getDisplayColorOfValueWithSpecialText(detail.Margin, '%')) : 'N/A') + '</div></td>';

                    sportHtml += '</tr>';
                    sportHtmlFrozen += '<td><div class="col120 l">' + detail.AliasName + '</div></td></tr>';
                    sportRowSpan++;
                    break;
                case 3:
                    sportHtml += '<tr>\
                                    <td><div class="col120 c" style="height: 18px;"></div></td>\
                                    <td style="background-color:#C4BD97"><div class="col120 c b" >Sub Total</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b" >' + StringFormatter.getDisplayColorOfValue(detail.WinLoss) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col90 r b">' + StringFormatter.formatNumber(detail.Position) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col80 r b">' + StringFormatter.formatNumber(detail.Commission) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(detail.LineTotal) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b">' + StringFormatter.formatNumber(detail.Turnover) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col80 r b">' + (detail.Turnover.toFixed(2) != 0 ? (StringFormatter.getDisplayColorOfValueWithSpecialText(detail.Margin, '%')) : 'N/A') + '</div></td>\
                                </tr>';

                    sportHtmlFrozen += '<tr>\
                                        <td><div class="col120 c" style="height: 18px;"></div></td>\
                                        <td style="background-color:#C4BD97"><div class="col120 c b" >Sub Total</div></td>\
                                    </tr>';
                    if (numSport > 0) {
                        scrollHtml += '<tr>';
                        frozenHtml += '<tr>';
                    }
                    scrollHtml += '<td rowspan="' + sportRowSpan + '"><div class="col120 c b">' + detail.Name + '</div></td>';
                    frozenHtml += '<td rowspan="' + sportRowSpan + '"><div class="col120 c b">' + detail.Name + '</div></td>';
                    scrollHtml += sportHtml;
                    frozenHtml += sportHtmlFrozen;
                    sportHtml = "";
                    sportHtmlFrozen = "";
                    sportRowSpan = 0;
                    numSport++;
                    break;
            }
        }

        var normalTotal = row.Total.NormalTotal;
        scrollHtml += '<tr style="color: White;">\
                        <td><div class="col120 c" style="height: 18px;"></div></td>\
                        <td style="background-color:#215967; height: 18px;"><div class="col120 c" ></div></td>\
                        <td style="background-color:#215967"><div class="col120 c b" style="color: White;">Total</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b ">' + StringFormatter.getDisplayColorOfValue(normalTotal.WinLoss) + '</div></td>\
                        <td style="background-color:#215967"><div class="col90 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Position) + '</div></td>\
                        <td style="background-color:#215967"><div class="col80 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Commission) + '</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(normalTotal.LineTotal) + '</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Turnover) + '</div></td>\
                        <td style="background-color:#215967"><div class="col80 r b">' + (normalTotal.Turnover.toFixed(2) != 0 ? (StringFormatter.getDisplayColorOfValueWithSpecialText(normalTotal.Margin, '%')) : 'N/A') + '</div></td>\
                    </tr>';
        frozenHtml += '<tr>\
                            <td><div class="col120 c" style="height: 18px;"></div></td>\
                            <td style="background-color:#215967; height: 18px;"><div class="col120 c" ></div></td>\
                            <td style="background-color:#215967"><div class="col120 c b" style="color: White;">Total</div></td>\
                       </tr>';

        advanceMixParlayReport.frozenHeader += frozenHtml;
        advanceMixParlayReport.scrollContent += scrollHtml;
    },
    renderAdvanceMixParlayReport_Footer: function (reportTotal) {
        var mainHTML = '';
        var frozenHTML = '';
        mainHTML += '<tr>\
                        <td colspan="3" style="background-color:#D8E4BC"><div class="c b" style="width: 370px">Grand Total</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(reportTotal.WinLoss) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col90 r b">' + StringFormatter.formatNumber(reportTotal.Position) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col80 r b">' + StringFormatter.formatNumber(reportTotal.Commission) + '</div></td>\
                        <td style="background-color:#B5C4D0"><div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(reportTotal.LineTotal) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col100 r b ">' + StringFormatter.formatNumber(reportTotal.Turnover) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col80 r b">' + (reportTotal.LineTotal.toFixed(2) != 0 ? StringFormatter.getDisplayColorOfValueWithSpecialText(reportTotal.Margin, '%') : 'N/A') + '</div></td>\
                    </tr>';
        frozenHTML += '<tr>\
                        <td colspan="3" style="background-color:#D8E4BC"><div class="c b" style="width: 370px">Grand Total</div></td>\
                       </tr>';

        $("#advanceMixParlayWLReport").find(".row_footer_content thead").empty().append(frozenHTML);
        $("#advanceMixParlayWLReport").find(".col_footer_content thead").empty().append(mainHTML);
    },
    renderAdvanceMixParlayReport: function (reportDate, betTypeId, betType) {
        var gridOptsAdvanceMixParlayReport = {
            tblContentId: 'advanceMixParlayWLReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 10,
            gridWidth: advanceMixParlayReport.reportWidth,
            freezeWidth: 370,
            freezeHeight: 200,
            numberFreezeCol: 3
        };
        $("#advanceMixParlayWLReport").removeClass("hidden");
        $("#advanceMixParlayWLReport").gridScroller(gridOptsAdvanceMixParlayReport);
    },
    exportExcel: function () {
        var url = 'MixParlayPopup?ExportType=AdvanceReport' +
                    '&fromDate=' + advanceMixParlayReport.reportDate +
                    '&toDate=' + advanceMixParlayReport.reportDate +
                    '&BetTypeId=' + advanceMixParlayReport.betTypeId;
        $('#frm').attr('src', url);
    }
}