// --------------------------------------------------------------------------
// <copyright file="VirtualTennisResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    /// <summary>
    /// The class VirtualTennisResultService.
    /// </summary>
    public class VirtualTennisResultService : IVirtualTennisResultService
    {
        private IVirtualTennisResultDataAccess _virtualTennisResultDataAccess;

        public VirtualTennisResultService(IVirtualTennisResultDataAccess virtualRacingResultDataAccess)
        {
            this._virtualTennisResultDataAccess = virtualRacingResultDataAccess ?? new VirtualTennisResultDataAccess();
        }

        public VirtualTennisResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType)
        {
            var criteria = GetCriteria(matchId, null);
            var results = this._virtualTennisResultDataAccess.GetVirtualTennisPopupResult(criteria);
            ResultSetting resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(results, resultSetting);
        }

        private static GetVirtualTennisResultCriteria GetCriteria(int matchId, int? raceNumber)
        {
            return new GetVirtualTennisResultCriteria()
            {
                MatchId = matchId,
                RaceNo = raceNumber
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = false,
                HighlightWinnerTeam = true
            };
        }
    }
}