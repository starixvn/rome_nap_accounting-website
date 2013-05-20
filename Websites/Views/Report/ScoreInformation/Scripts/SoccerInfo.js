SoccerInfo = function () {
    var self = this;
    self.Tables = {
        report: null,
        report_Header: null,
        report_Body: null
    };

    self.CacheData = {
        reportWidth: 750
    };

    self.Params = {
        reportParams: {
            ReportDate: null,
            SportType: null,
            SportTypeName: null,
            BetType: null,
            BetTypeName: null,
            LiveIndicator: null,
            Hedge: null,

            LeagueId: null,
            LeagueName: null,
            MatchCode: null,
            HomeTeam: null,
            HomeTeamName: null,
            AwayTeam: null,
            AwayTeamName: null
        }
    };
};

SoccerInfo.prototype.InitPage = function () {
    this.CachedDOMElements();
};

SoccerInfo.prototype.CachedDOMElements = function () {
    var self = this;
    self.Tables.report = $(".soccerReport");
    self.Tables.report_Header = $("#headerInfo_Soccer");
    self.Tables.report_Body = self.Tables.report.find("tbody");
};

SoccerInfo.prototype.RenderReport = function (report, params) {
    var self = this;
    self.Params.reportParams = params;

    var headerTitle = "<span class='sportTypeName_Header'>" + self.Params.reportParams.SportTypeName + "</span>" + " - " + self.Params.reportParams.LeagueName;
    headerTitle += "<br />";
    headerTitle += "Bettype: " + (self.Params.reportParams.LiveIndicator ? "LIVE " : "NON-LIVE ") + self.Params.reportParams.BetTypeName;
    headerTitle += "<br />";
    headerTitle += "Trader: " + report.TraderName;

    var bodyData = '';
    var isHomeWin = report.HomeFinalScore > report.AwayFinalScore;
    var isAwayWin = report.AwayFinalScore > report.HomeFinalScore;

    bodyData += '<tr>\
                      <td rowspan="2">' + report.EventTime + '</td>\
                      <td style="height: 19px;" class="l ' + (isHomeWin ? 'b' : '') + '">' +
                          report.HomeTeamName +
                          (report.HTWinTeam == report.HomeTeamId ? "<div class='firstGoal'>&nbsp;</div>" : "") +
                          (report.FinalWinTeam == report.HomeTeamId ? "<div class='lastGoal'>&nbsp;</div>" : "") +
                      '</td>\
                      <td class="b c">' + report.HomeHTScore + '</td>\
                      <td class="b c">' + report.HomeFinalScore + '</td>\
                      <td class="b c" rowspan="2">' + report.EventStatus + '</td>\
                  </tr>';

    bodyData += '<tr>\
                    <td style="height: 19px;" class="l ' + (isAwayWin ? 'b' : '') + '">' +
                          report.AwayTeamName +
                          (report.HTWinTeam == report.AwayTeamId ? "<div class='firstGoal'>&nbsp;</div>" : "") +
                          (report.FinalWinTeam == report.AwayTeamId ? "<div class='lastGoal'>&nbsp;</div>" : "") +
                    '</td>\
                    <td class="b c">' + report.AwayHTScore + '</td>\
                    <td class="b c">' + report.AwayFinalScore + '</td>\
                 </tr>';

    self.Tables.report_Body.empty().append(bodyData);
    self.Tables.report_Header.html(headerTitle);
    self.Tables.report.removeClass("hidden");
};