// --------------------------------------------------------------------------
// <copyright file="VirtualRacingResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    /// <summary>
    /// The class VirtualRacingResultService.
    /// </summary>
    public class VirtualRacingResultService : IVirtualRacingResultService
    {
        private IVirtualRacingResultDataAccess _virtualRacingResultDataAccess;

        public VirtualRacingResultService(IVirtualRacingResultDataAccess virtualRacingResultDataAccess)
        {
            this._virtualRacingResultDataAccess = virtualRacingResultDataAccess ?? new VirtualRacingResultDataAccess();
        }

        public VirtualRacingResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int raceNumber, int sportType)
        {
            var criteria = GetCriteria(matchId, raceNumber);
            var results = this._virtualRacingResultDataAccess.GetRacingPopupResult(criteria);
            ResultSetting resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(results, resultSetting);
        }

        private static GetVirtualRacingResultCriteria GetCriteria(int matchId, int? raceNumber)
        {
            return new GetVirtualRacingResultCriteria()
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
                IsOutRight = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = false
            };
        }
    }
}