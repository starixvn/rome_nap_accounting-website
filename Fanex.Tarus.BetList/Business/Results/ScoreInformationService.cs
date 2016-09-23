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

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betId">The bet identifier.</param>
        /// <param name="isSupport">If set to <c>true</c> [is support].</param>
        /// <returns>
        /// The result for SPORTSBOOK.
        /// </returns>
        public string GetResult(int matchId, int betType, long betId, out bool isSupport)
        {
            isSupport = false;
            var criteria = GetCriteria(matchId, betType, betId);
            List<Sportsbook> sportsbooks = this._scoreInformationDataAccess.GetScoreInformation(ref criteria);

            if (2501 <= betType && betType <= 2699)
            {
                isSupport = true;
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

                    return "<script type=\"text/javascript\">location.href='" + res.Result.Url + "';</script>";
                }

                return "<div style=\"color: #ff0000; font-weight: bold\">" + res.Result.ErrorDescription + "</div>";
            }
            else
            {
                if (IsSupport(criteria.SportType))
                {
                    isSupport = true;
                    ResultSetting resultSetting = GetFanexResultSetting(criteria);
                    return ResultEngine.HtmlParser(sportsbooks, resultSetting).Replace("original-title", " title ");
                }
            }

            return string.Empty;
        }

        private static bool IsSupport(int sportType)
        {
            if (sportType <= 0)
            {
                return false;
            }

            return true;
        }

        private static GetScoreInformationCriteria GetCriteria(int matchId, int betType, long betId)
        {
            return new GetScoreInformationCriteria()
            {
                MatchId = matchId,
                BetType = betType,
                BetId = betId
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting(GetScoreInformationCriteria criteria)
        {
            return new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisiblePopUpTitle = true,
                HighlightWinnerTeam = true
            };
        }
    }
}