CommonSportInfo = function () {
    var self = this;
    self.Tables = {
        report: null,
        report_Header: null,
        report_Body: null
    };

    self.CacheData = {
        reportWidth: 780,
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

CommonSportInfo.prototype.InitPage = function () {
    this.CachedDOMElements();
};

CommonSportInfo.prototype.CachedDOMElements = function () {
    var self = this;
    self.Tables.report = $(".commonSportReport_ScoreInfo");
    self.Tables.report_Header = $("#headerInfo_CommonSport_ScoreInfo");
    self.Tables.report_Body = self.Tables.report.find("tbody");
};

CommonSportInfo.prototype.RenderReport = function (report, params) {
    var self = this;
    self.Params.reportParams = params;

    var headerTitle = scoreInfoManager.RenderScoreTitle(report);
    self.Tables.report_Header.html(headerTitle);
    var bodyData = '';
    var maxColumnNo = 5;
    if (report) {
        var isHomeWin = report.HomeFinalScore > report.AwayFinalScore;
        var isAwayWin = report.AwayFinalScore > report.HomeFinalScore;

        bodyData += '<tr>\
                      <td rowspan="2">' + report.EventTime + '</td>\
                      <td style="height: 19px;" class="l ' + (isHomeWin ? 'b' : '') + '">' +
                          report.HomeTeamName +
                          (report.IsNeutral ? " (N)" : "") +
                      '</td>\
                      <td class="b c">' + report.HomeHTScore + '</td>\
                      <td class="b c">' + report.HomeFinalScore + '</td>\
                      <td class="b c ' + (report.IsPending ? 'status-pending' : '') + '" rowspan="2">' + report.EventStatus + '</td>\
                  </tr>';

        bodyData += '<tr>\
                    <td style="height: 19px;" class="l ' + (isAwayWin ? 'b' : '') + '">' +
                          report.AwayTeamName +
                    '</td>\
                    <td class="b c">' + report.AwayHTScore + '</td>\
                    <td class="b c">' + report.AwayFinalScore + '</td>\
                 </tr>';
    }
    else {
        bodyData = '<tr>\
                    <td colspan="' + maxColumnNo + '" class="b c">There is no information available.</td>\
                 </tr>';
    }

    self.Tables.report_Body.empty().append(bodyData);
    self.Tables.report.removeClass("hidden");
};