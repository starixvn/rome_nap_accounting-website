ScoreInfoManager = function () {
    var self = this;
    self.SoccerInfo = new SoccerInfo();

    self.Tables = {
        scoreInformationPopupContainer: null,
        scoreInformationPopup: null
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

    self.CacheData = {
        reportViewHtml: null
    };
};

ScoreInfoManager.prototype.InitPage = function () {
    this.CachedDOMElements();
    this.SoccerInfo.InitPage();
    this.CacheData.reportViewHtml = this.Tables.scoreInformationPopupContainer.html();
};

ScoreInfoManager.prototype.CachedDOMElements = function () {
    var self = this;
    self.Tables.scoreInformationPopupContainer = $("#scoreInformationPopupContainer");
    self.Tables.scoreInformationPopup = $("#scoreInformationPopup");
};

ScoreInfoManager.prototype.GetReport = function (reportDate,
    sportType,
    sportTypeName,
    betType,
    betTypeName,
    liveIndicator,
    hedge,
    leagueId,
    leagueName,
    matchCode,
    homeTeam,
    homeTeamName,
    awayTeam,
    awayTeamName) {
    var self = this;

    try { parent.BlockUI(); } catch (e) { }

    this.Params.reportParams = {
        ReportDate: reportDate,
        SportType: sportType,
        SportTypeName: sportTypeName,
        BetType: betType,
        BetTypeName: betTypeName,
        LiveIndicator: liveIndicator,
        Hedge: hedge,

        LeagueId: leagueId,
        LeagueName: leagueName,
        MatchCode: matchCode,
        HomeTeam: homeTeam,
        HomeTeamName: homeTeamName,
        AwayTeam: awayTeam,
        AwayTeamName: awayTeamName
    };

    $.ajax({
        type: "POST",
        url: window.rootUrl + "DailyWinLoss2hSoccer/GetScoreInformation",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(self.Params.reportParams),
        beforeSend: function () {
        },
        success: function (rs) {
            self.RenderReport(rs);
        },
        error: function (event, jqXHR, ajaxSettings) {
            self.RenderReport(null);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
};

ScoreInfoManager.prototype.RenderReport = function (report) {
    var self = this;

    self.SoccerInfo.RenderReport(report, self.Params.reportParams);


    self.Tables.scoreInformationPopup.removeClass("hidden");
    self.Tables.scoreInformationPopup.dialog({
        title: "Score Information",
        autoOpen: false,
        height: 400,
        width: 750,
        modal: false,
        resizable: false,
        close: function (event, ui) {
            self.Tables.scoreInformationPopup.dialog("destroy").remove();
            self.Tables.scoreInformationPopupContainer.append(self.CacheData.reportViewHtml);
            self.Tables.scoreInformationPopup = $("#scoreInformationPopup");
            self.SoccerInfo.InitPage();
        }
    });

    self.Tables.scoreInformationPopup.dialog("open");
};

var scoreInfoManager = new ScoreInfoManager();