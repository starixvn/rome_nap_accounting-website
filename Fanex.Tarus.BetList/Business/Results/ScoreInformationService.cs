// --------------------------------------------------------------------------
// <copyright file="ScoreInformationService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using System.Collections.Generic;
    using Criteria.BetBuilder;
    using CustomerSetting.Module.Core.Constants;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using ONEbook.NG2.API;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// The class ScoreInformationService.
    /// </summary>
    public class ScoreInformationService : IScoreInformationService
    {
        private IScoreInformationDataAccess _scoreInformationDataAccess;

        public ScoreInformationService(IScoreInformationDataAccess scoreInformationDataAccess)
        {
            this._scoreInformationDataAccess = scoreInformationDataAccess ?? new ScoreInformationDataAccess();
        }

        public ScoreInformationService()
            : this(null)
        {
        }
       
        public string GetResult(int matchId, int sportType, int betType, long betId, int handicap2, string transdesc)
        {
            List<Sportsbook> sportsbooks;
            if (betType == BetList.Constants.BetTypeContants.BetBuilder)
            {
                var criteria = new GetResultBetBuilderDetailCriteria
                {
                    MatchId = matchId,
                    MarketId = int.Parse(betId.ToString()),
                    BetType = betType
                };                
                sportsbooks = this._scoreInformationDataAccess.GetScoreInformation(criteria);
            }
            else
            {
                var criteria = new GetScoreInformationCriteria()
                {
                    MatchId = matchId,
                    BetType = betType,
                    BetId = betId,
                    TransDesc = transdesc
                };
                sportsbooks = this._scoreInformationDataAccess.GetScoreInformation(criteria, handicap2);
            }

            if(IsSupport(sportType))
            {
                ResultSetting resultSetting = GetFanexResultSetting(sportType);
                return ResultEngine.HtmlParser(sportsbooks, resultSetting).Replace("original-title", " title ");
            }
            
            return string.Empty;
        }

        public string GetNG2Result(int matchId)
        {
            var service = new Services();
            var req = new ONEbook.NG2.API.DTO.ShowTicketResult.Request();
            req.Auth.SiteId = 3000;
            req.UserId = AppSession.AccId;
            req.Call.GambleId = matchId;
            var res = service.ShowTicketResult(req);
            if (res.Result.IsSuccess)
            {
                if (string.IsNullOrWhiteSpace(res.Result.Url))
                {
                    return "<div>There is no information available.</div>";
                }

                /*using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(res.Result.Url);

                    return "<link rel=\"stylesheet\" type=\"text/css\" href=\"css/ng2.css\"/>" + s;
                }*/

                return "<script type=\"text/javascript\">location.href='" + res.Result.Url + "';</script>";
            }

            return "<div style=\"color: #ff0000; font-weight: bold\">" + res.Result.ErrorDescription + "</div>";
        }

        private static bool IsSupport(int sportType)
        {
            if (sportType <= 0)
            {
                return false;
            }

            return true;
        }

       
        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                IsPopupOpenFromBetList = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisiblePopUpTitle = true,
                HighlightWinnerTeam = true
            };
        }
    }
}