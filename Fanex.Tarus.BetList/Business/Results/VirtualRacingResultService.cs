// --------------------------------------------------------------------------
// <copyright file="VirtualRacingResultService.cs" company="">
// Copyright (c) . All rights reserved.
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
    /// The class VirtualRacingResultService.
    /// </summary>
    public class VirtualRacingResultService : IVirtualRacingResultService
    {
        private readonly IVirtualRacingResultDataAccess _virtualRacingResultDataAccess;

        public VirtualRacingResultService(IVirtualRacingResultDataAccess virtualRacingResultDataAccess)
        {
            _virtualRacingResultDataAccess = virtualRacingResultDataAccess ?? new VirtualRacingResultDataAccess();
        }

        public VirtualRacingResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType)
        {
            var criteria = GetCriteria(matchId);
            var results = _virtualRacingResultDataAccess.GetRacingPopupResult(criteria);
            ResultSetting resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(results, resultSetting);
        }

        private static GetVirtualRacingResultCriteria GetCriteria(int matchId)
        {
            return new GetVirtualRacingResultCriteria()
            {
                MatchId = matchId
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