var historyLogController;
$(document).ready(function () {
    historyLogController.initPage();
});

var HistoryLogController = function () {
    var self = this;
    self.Constants = {
        UpdateType: {
            Status: 806,
            Rate: 801,
            LicenseeStatus: 2802,
            LicenseeRate: 2800
        },
        LiveType: {
            Live: 1,
            NonLive: 0
        },
        Format: {
            Number: {
                IntPercent: "#,###%"
            }
        }
    };

    self.initPage = function () {
        self.renderHistoryLogReport(historyLogReport);
    };
    self.renderHistoryLogReport = function (historyLogReport) {
        var gridOptions = {
            columns: self.renderHistoryLogReportHeader(historyLogReport),
            showTitle: false,
            bodyRows: self.renderHistoryLogReportBody(historyLogReport),
            footerRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 550,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 17
        };

        $("#virtual-rate-log").fanexGrid(gridOptions);
    };
    self.renderHistoryLogReportHeader = function (historyLogReport) {
        var betTypeCount = historyLogReport.BetTypeList.length;
        var columns = [];
        columns.push({
            HeaderText: viewResources.Symbol_No,
            Width: 60,
            Name: "ColNo",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        columns.push({
            HeaderText: viewResources.Date,
            Width: 140,
            Name: "ColDate",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        columns.push({
            HeaderText: viewResources.Sport,
            Width: 140,
            Name: "ColSport",
            HeaderAlign: "center",
            CellAlign: "left"
        });

        for (var i = 0; i < betTypeCount; i++) {
            var betType = historyLogReport.BetTypeList[i];
            columns.push({
                HeaderText: betType.Name,
                Width: 100,
                Name: "ColBetType" + betType.Id,
                HeaderAlign: "center",
                CellAlign: "right"
            });
        }

        columns.push({
            HeaderText: viewResources.Changed_by,
            Width: 100,
            Name: "ColChangedBy",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        return columns;
    };
    self.renderHistoryLogReportBody = function (historyLogReport) {
        var bodyData = [];
        var logLength = historyLogReport.HistoryLog.length;
        var betTypeCount = historyLogReport.BetTypeList.length;
        if (logLength) {
            for (var i = 0; i < logLength; i++) {
                var historyLog = historyLogReport.HistoryLog[i];
                var detailCount = historyLog.HistoryLogDetailList.length;
                for (var j = 0; j < detailCount; j++) {
                    var columns = [];
                    var detailHistoryLog = historyLog.HistoryLogDetailList[j];
                    if (j == 0) {
                        columns.push({ Name: "ColNo", Value: (i + 1), MergeRowNumber: detailCount });
                        columns.push({ Name: "ColDate", Value: historyLog.EditDateString, MergeRowNumber: detailCount });
                    }

                    if (detailHistoryLog.UpdateType == self.Constants.UpdateType.Status ||
                        detailHistoryLog.UpdateType == self.Constants.UpdateType.LicenseeStatus) {
                        columns.push({
                            Name: "ColSport",
                            Value: viewResources.Enabled_Virtual_Rate + ": " +
                                    (detailHistoryLog.EnableVirtualRate ? viewResources.Symbol_True : viewResources.Symbol_False),
                            MergeColNumber: (betTypeCount + 1)
                        });
                    } else {
                        columns.push({ Name: "ColSport", Value: detailHistoryLog.SportName + ' (' + self.getLiveTypeName(detailHistoryLog.LiveType) + ')' });

                        for (var k = 0; k < betTypeCount; k++) {
                            var virtualRate = detailHistoryLog.VirtualRate[k];
                            var betType = historyLogReport.BetTypeList[k];
                            columns.push({
                                Name: "ColBetType" + betType.Id,
                                Value: (virtualRate.HasData ?
                                        numeral((virtualRate.PercentRate - 1)).format(self.Constants.Format.Number.IntPercent) :
                                        '&nbsp;')
                            });
                        }
                    }

                    if (j == 0) {
                        columns.push({ Name: "ColChangedBy", Value: historyLog.Editor, MergeRowNumber: detailCount });
                    }

                    bodyData.push({
                        Columns: columns
                    });
                }
            }
        }

        return bodyData;
    };
    self.getLiveTypeName = function (liveType) {
        switch (liveType) {
            case self.Constants.LiveType.Live:
                return viewResources.Live;
            case self.Constants.LiveType.NonLive:
                return viewResources.NonLive;
            default:
                return '';
        }
    };
};

historyLogController = new HistoryLogController();