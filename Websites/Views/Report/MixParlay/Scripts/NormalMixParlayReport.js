

var normalMixParlayReport = {
    reportDate: null,
    betType: null,
    betTypeId: null,
    frozenHeader: '',
    scrollContent: '',
    reportWidth: 705,
    initPage: function (reportDate, betTypeId, betType) {
        normalMixParlayReport.reportDate = reportDate;
        normalMixParlayReport.betTypeId = betTypeId;
        normalMixParlayReport.betType = betType;
        normalMixParlayReport.getNormalMixParlayReport(reportDate, betTypeId, betType);
    },
    getNormalMixParlayReport: function (reportDate, betTypeId, betType) {
        try { parent.parent.BlockUI(); } catch (e) { }
        var data = {
            reportDate: reportDate,
            betType: betTypeId
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MixParlay/SelDirectMixParlay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(new Date(reportDate));
                $("#normalMixParlayWLReport_Title").html(title);
                if (rs != null && rs.ReportDetails && rs.ReportDetails.length > 0) {
                    normalMixParlayReport.renderNormalMixParlayReport_Main(rs, reportDate, betTypeId, betType);
                }
                else {
                    normalMixParlayReport.renderNormalMixParlayReport_NoData();
                }
            },
            error: function () {
                normalMixParlayReport.renderNormalMixParlayReport_NoData();
            },
            complete: function () {
                normalMixParlayReport.renderNormalMixParlayReport(reportDate, betTypeId, betType);
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
    renderNormalMixParlayReport_NoData: function () {
        html = '<tr>\
                    <td class="b c" style="height: 19px;" colspan="1">&nbsp</td>\
                    <td class="b c" style="height: 19px;" colspan="6">There is no data.</td>\
                </tr>';
        var frozenHeader = '<tr><td class="b c" style="height: 19px;" colspan="1">&nbsp</td></tr>';
        $("#normalMixParlayWLReport_RowHeader tbody").empty().append(frozenHeader);
        $("#normalMixParlayWLReport_Container tbody").empty().append(html);

        $("#normalMixParlayWLReport").find(".row_footer_content thead").empty();
        $("#normalMixParlayWLReport").find(".col_footer_content thead").empty();
        return html;
    },
    renderNormalMixParlayReport_Main: function (rs, reportDate, betTypeId, betType) {
        normalMixParlayReport.frozenHeader = '';
        normalMixParlayReport.scrollContent = '';
        var reportDetails = rs.ReportDetails;
        for (var i = 0; i < reportDetails.length; i++) {
            normalMixParlayReport.renderDailyWLRow(reportDetails[i], reportDate);
        }
        normalMixParlayReport.renderReportFooter(rs);

        normalMixParlayReport.frozenHeader = StringFormatter.removeSpaceIE9(normalMixParlayReport.frozenHeader);
        normalMixParlayReport.scrollContent = StringFormatter.removeSpaceIE9(normalMixParlayReport.scrollContent);
        $("#normalMixParlayWLReport_RowHeader tbody").empty().append(normalMixParlayReport.frozenHeader);
        $("#normalMixParlayWLReport_Container tbody").empty().append(normalMixParlayReport.scrollContent);
    },
    renderDailyWLRow: function (dwlRow, reportDate) {
        switch (dwlRow.RowType.Id) {
            case 0:
                normalMixParlayReport.normalRowHtml(dwlRow, reportDate);
                break;
            case 3:
                normalMixParlayReport.sportTotalRowHtml(dwlRow, reportDate);
                break;
            case 9:
                normalMixParlayReport.emptyRowHtml(dwlRow, reportDate);
                break;
        }
    },
    normalRowHtml: function (dwlRow, reportDate) {
        if (dwlRow) {
            normalMixParlayReport.scrollContent +=
                '<tr class="" >\
                    <td>\
                        <div class="col120 b l">' + dwlRow.Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.WinLoss) + '</div>\
                    </td>\
                    <td>\
                        <div class="col90 r">' + StringFormatter.formatNumber(dwlRow.Position) + '</div>\
                    </td>\
                        <td>\
                        <div class="col80 r">' + StringFormatter.formatNumber(dwlRow.Commission) + '</div>\
                    </td>\
                        <td>\
                        <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</div>\
                    </td>\
                        <td>\
                        <div class="col100 r">' + StringFormatter.formatNumber(dwlRow.Turnover) + '</div>\
                    </td>\
                        <td>\
                        <div class="col80 r" style="">' +
                                (dwlRow.Turnover.toFixed(2) != 0 ? StringFormatter.getDisplayColorOfValueWithSpecialText(dwlRow.Margin, '%') : 'N/A') + '</div>\
                    </td>\
                </tr>';
            normalMixParlayReport.frozenHeader +=
                '<tr class="" >\
                    <td>\
                        <div class="col120 b l">' + dwlRow.Name + '</div>\
                    </td>\
                </tr>';
        }
    },
    sportTotalRowHtml: function (dwlRow, reportDate) {
        if (dwlRow) {
            normalMixParlayReport.scrollContent +=
                '<tr class="total2" style="font-weight:bold; background-color:#d0e8fb;">\
                    <td>\
                        <div class="col120 b l">' + dwlRow.Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.WinLoss) + '</div>\
                    </td>\
                    <td>\
                        <div class="col90 r">' + StringFormatter.formatNumber(dwlRow.Position) + '</div>\
                    </td>\
                        <td>\
                        <div class="col80 r">' + StringFormatter.formatNumber(dwlRow.Commission) + '</div>\
                    </td>\
                        <td>\
                        <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</div>\
                    </td>\
                        <td>\
                        <div class="col100 r">' + StringFormatter.formatNumber(dwlRow.Turnover) + '</div>\
                    </td>\
                        <td>\
                        <div class="col80 r" style="">' +
                            (dwlRow.Turnover.toFixed(2) != 0 ? StringFormatter.getDisplayColorOfValueWithSpecialText(dwlRow.Margin, '%') : 'N/A') + '</div>\
                    </td>\
                </tr>';
            normalMixParlayReport.frozenHeader +=
                '<tr class="total2" style="font-weight:bold; background-color:#d0e8fb;">\
                    <td>\
                        <div class="col120 b l">' + dwlRow.Name + '</div>\
                    </td>\
                </tr>';
        }
    },
    emptyRowHtml: function (dwlRow, reportDate) {
        normalMixParlayReport.scrollContent += '<tr>\
                                                    <td colspan ="7" class ="c" style="height:18px;"></td>\
                                                </tr>';
        normalMixParlayReport.frozenHeader += '<tr>\
                                                    <td class ="c nonBorderRight" style="height:18px;"></td>\
                                                </tr>';
    },
    renderReportFooter: function (dailyWLReport) {
        normalMixParlayReport.renderAllTotal(dailyWLReport.Total.NormalTotal);
    },
    renderAllTotal: function (total) {
        if (total) {
            lineTotalColor = "#B5C4D0";
            normalMixParlayReport.scrollContent +=
                '<tr>\
                    <td>\
                        <div class="col120 r b">All Total:</div>\
                    </td>\
                    <td>\
                        <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(total.WinLoss) + '</div>\
                    </td>\
                    <td>\
                        <div class="col90 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Position) + '</div>\
                    </td>\
                        <td>\
                        <div class="col80 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Commission) + '</div>\
                    </td>\
                    <td style="background-color: ' + lineTotalColor + '">\
                        <div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(total.LineTotal) + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b" style="">' + StringFormatter.formatNumber(total.Turnover) + '</div>\
                    </td>\
                    <td>\
                        <div class="col80 r" style="font-weight: normal;">' +
                        (total.Turnover.toFixed(2) != 0 ? StringFormatter.getDisplayColorOfValueWithSpecialText(total.Margin, '%') : 'N/A') + '</div>\
                    </td>\
                </tr>';
            normalMixParlayReport.frozenHeader +=
                '<tr>\
                    <td>\
                        <div class="col120 r b">All Total:</div>\
                    </td>\
                </tr>';
        }
    },
    renderNormalMixParlayReport: function (reportDate, betTypeId, betType) {
        var gridOptsNormalMixParlayReport = {
            tblContentId: 'normalMixParlayWLReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            gridWidth: normalMixParlayReport.reportWidth,
            freezeWidth: 120,
            freezeHeight: 200,
            numberFreezeCol: 1
        };
        $("#normalMixParlayWLReport").removeClass("hidden");
        $("#normalMixParlayWLReport").gridScroller(gridOptsNormalMixParlayReport);
    },
    exportExcel: function () {
        var url = 'MixParlayPopup?ExportType=NormalReport' +
                    '&reportDate=' + normalMixParlayReport.reportDate +
                    '&BetTypeId=' + normalMixParlayReport.betTypeId;
        $('#frm').attr('src', url);
    }
}