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
                Rate: "#,###"
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
        columns.push({
            HeaderText: viewResources.Bet_Type,
            Width: 140,
            Name: "ColBetType",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        columns.push({
            HeaderText: viewResources.Live_NonLive,
            Width: 140,
            Name: "ColLiveType",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        columns.push({
            HeaderText: viewResources.Virtual_Rate + ' (' + viewResources.Symbol_Percent + ')',
            Width: 160,
            Name: "ColVirtualRate",
            HeaderAlign: "center",
            CellAlign: "right"
        });
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
        if (logLength) {
            var maxColumnIndex = 7;
            for (var i = 0; i < logLength; i++) {
                var historyLog = historyLogReport.HistoryLog[i];
                var detailCount = historyLog.HistoryLogDetailList.length;
                var isFirstRow = true;
                var rowSpan = 0;

                var sportBetTypeRow = [];
                var firstInfo = null;
                var firstVirtualRate = null;

                for (var j = 0; j < detailCount; j++) {
                    var columns = [];
                    var detailHistoryLog = historyLog.HistoryLogDetailList[j];
                    if (detailHistoryLog.UpdateType == self.Constants.UpdateType.Status ||
                        detailHistoryLog.UpdateType == self.Constants.UpdateType.LicenseeStatus) {
                        if (isFirstRow) {
                            firstInfo = {
                                detail: detailHistoryLog,
                                index: i
                            };

                            rowSpan++;
                            isFirstRow = false;
                            continue;
                        } else {
                            columns.push({
                                Name: "ColSport",
                                Value: viewResources.Virtual_Rate + ": " +
                                    (detailHistoryLog.EnableVirtualRate ? viewResources.Enabled : viewResources.Disabled),
                                MergeColNumber: (maxColumnNumber - 3)
                            });
                        }
                    } else {
                        var sportLiveTypeCount = detailHistoryLog.VirtualRate.length;
                        for (var k = 0; k < sportLiveTypeCount; k++) {
                            var virtualRate = detailHistoryLog.VirtualRate[k];
                            if (virtualRate.HasData) {
                                if (isFirstRow) {
                                    firstInfo = {
                                        detail: detailHistoryLog,
                                        index: i,
                                        rate: virtualRate,
                                        betTypeName: detailHistoryLog.BetTypeList[k].Name
                                    };
                                    rowSpan++;
                                    isFirstRow = false;
                                    continue;
                                } else {
                                    columns = [];
                                    columns.push({ Name: "ColSport", Value: detailHistoryLog.SportName });
                                    columns.push({
                                        Name: "ColBetType",
                                        Value: detailHistoryLog.BetTypeList[k].Name
                                    });
                                    columns.push({
                                        Name: "ColLiveType",
                                        Value: self.getLiveTypeName(detailHistoryLog.LiveType)
                                    });
                                    columns.push({
                                        Name: "ColVirtualRate",
                                        Value: numeral((virtualRate.PercentRate - 1)*100).format(self.Constants.Format.Number.Rate)
                                    });
                                    sportBetTypeRow.push({
                                        Columns: columns
                                    });
                                    rowSpan++;
                                }

                            }
                        }
                    }
                }

                bodyData = bodyData.concat([self.getRow(historyLog, firstInfo, true, rowSpan)]);
                bodyData = bodyData.concat(sportBetTypeRow);
            }
        }

        return bodyData;
    };
    self.getRow = function (historyLog, firstInfo, isFirstRow, rowSpan) {
        var columns = [];
        var logInfo = firstInfo.detail;
        var maxColumnNumber = 7;
        if (isFirstRow) {
            columns.push({ Name: "ColNo", Value: (firstInfo.index + 1), MergeRowNumber: rowSpan });
            columns.push({ Name: "ColDate", Value: historyLog.EditDateString, MergeRowNumber: rowSpan });
        }

        if (logInfo.UpdateType == self.Constants.UpdateType.Status ||
            logInfo.UpdateType == self.Constants.UpdateType.LicenseeStatus) {
            columns.push({
                Name: "ColSport",
                Value: viewResources.Virtual_Rate + ": " +
                    (logInfo.EnableVirtualRate ? viewResources.Enabled : viewResources.Disabled),
                MergeColNumber: (maxColumnNumber - 3)
            });
        } else {
            columns.push({
                Name: "ColSport",
                Value: logInfo.SportName
            });
            columns.push({
                Name: "ColBetType",
                Value: firstInfo.betTypeName
            });
            columns.push({
                Name: "ColLiveType",
                Value: self.getLiveTypeName(logInfo.LiveType)
            });
            columns.push({
                Name: "ColVirtualRate",
                Value: numeral((firstInfo.rate.PercentRate - 1) * 100).format(self.Constants.Format.Number.Rate)
            });
        }

        if (isFirstRow) {
            columns.push({ Name: "ColChangedBy", Value: historyLog.Editor, MergeRowNumber: rowSpan });
        }

        return {
            Columns: columns
        };
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