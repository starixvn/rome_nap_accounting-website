TennisInfo = function () {
    var self = this;
    self.Tables = {
        reportContainer: null,
        report: null,
        report_Header: null,
        report_Body: null,

        setXContent: null,
        setXContent_Body: null
    };

    self.CacheData = {
        reportWidth: 820,
        reportHeight: 200
    };

    self.Params = {
        reportParams: {
            MatchID: null,
            Bettype: null,
            ReportDate: null,
            SportType: null,
            SportTypeName: null,
            BetTypeName: null,
            LiveIndicator: null,
            Hedge: null,

            LeagueId: null,
            LeagueName: null,
            MatchCode: null,
            HomeTeam: null,
            HomeTeamName: null,
            AwayTeam: null,
            AwayTeamName: null,
            BetId: null
        }
    };
};
TennisInfo.prototype.InitPage = function () {
    this.CachedDOMElements();
};

TennisInfo.prototype.CachedDOMElements = function () {
    var self = this;
    self.Tables.reportContainer = $("#tennisReportContainer_ScoreInfo");
    self.Tables.report = self.Tables.reportContainer.find(".tennisReport_ScoreInfo");
    self.Tables.report_Header = $("#headerInfo_Tennis_ScoreInfo");
    self.Tables.report_Body = self.Tables.report.find("tbody");

    self.Tables.setXContent = self.Tables.reportContainer.find(".setXContent");
    self.Tables.setXContent_Body = self.Tables.setXContent.find("tbody");
};

TennisInfo.prototype.RenderReport = function (report, params) {
    var self = this;
    self.Params.reportParams = params;
    var headerTitle = scoreInfoManager.RenderScoreTitle(report);
    self.Tables.report_Header.html(headerTitle);

    var bodyData = '';
    var maxColumnNo = 10;
    if (report) {
        var isHomeWin = report.HomeFinalScore > report.AwayFinalScore;
        var isAwayWin = report.AwayFinalScore > report.HomeFinalScore;
        var numberOfSets = scoreInfoManager.Enums.NumberOfSet.Tennis;

        bodyData += '<tr>\
                      <td rowspan="2">' + report.EventTime + '</td>\
                      <td style="height: 19px;" class="l ' + (isHomeWin ? 'b' : '') + '">' +
                          report.HomeTeamName +
                          (report.IsNeutral ? " (N)" : "") +
                      '</td>';
        for (var i = 0; i < numberOfSets; i++) {
            bodyData += '<td class="b c">' + report.HomeScores[i] + '</td>';
        }

        bodyData += '<td class="b c">' + report.HomeTotalGamesScore_Str + '</td>\
                     <td class="b c">' + report.HomeFinalScore + '</td>\
                     <td class="b c ' + (report.IsPending ? 'status-pending' : '') + '" rowspan="2">' + report.EventStatus + '</td>\
                  </tr>';

        bodyData += '<tr>\
                    <td style="height: 19px;" class="l ' + (isAwayWin ? 'b' : '') + '">' +
                          report.AwayTeamName +
                    '</td>';

        for (var i = 0; i < numberOfSets; i++) {
            bodyData += '<td class="b c">' + report.AwayScores[i] + '</td>';
        }

        bodyData += '<td class="b c">' + report.AwayTotalGamesScore_Str + '</td>\
                     <td class="b c">' + report.AwayFinalScore + '</td>\
                 </tr>';

        if (self.Params.reportParams.BetId &&
            self.Params.reportParams.BetId != '' &&
            scoreInfoManager.Enums.BetType.StartSetX_BetTypeRange <= self.Params.reportParams.Bettype &&
            self.Params.reportParams.Bettype <= scoreInfoManager.Enums.BetType.EndSetX_BetTypeRange) {
            self.RenderSetXContent(report.SetXInformation);
        }
    } else {
        bodyData = '<tr>\
                    <td colspan="' + maxColumnNo + '" class="b c">There is no information available.</td>\
                 </tr>';
    }

    self.Tables.report_Body.empty().append(bodyData);
    self.Tables.report.removeClass("hidden");
};
TennisInfo.prototype.RenderSetXContent = function (report) {
    var self = this;
    var bodyData = '';
    if (report) {
        var homeData = '';
        var awayData = '';
        bodyData += '<tr class="title">\
                        <td rowspan="3">\
                            <div>Set ' + report.Id + ' Score</div>\
                        </td>';
        homeData += '<tr>';
        awayData += '<tr>';

        var scoreLength = report.HomeScores.length;
        for (var i = 0; i < scoreLength; i++) {
            bodyData += '<td class="col40">' + (i + 1) + '</td>';
            homeData += '<td class="col40">' + report.HomeScores[i] + '</td>';
            awayData += '<td class="col40">' + report.AwayScores[i] + '</td>';
        }

        bodyData += '</tr>';
        homeData += '</tr>';
        awayData += '</tr>';

        bodyData += homeData;
        bodyData += awayData;

        self.Tables.setXContent_Body.empty().append(bodyData);
        self.Tables.setXContent.removeClass("hidden");
    } else {
        self.Tables.setXContent_Body.empty();
        self.Tables.setXContent.addClass("hidden");
    }
};