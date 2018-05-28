// --------------------------------------------------------------------------
// <copyright file="RacingResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    /// <summary>
    /// The class RacingResultService.
    /// </summary>
    public class RacingResultService : IRacingResultService
    {
        private readonly IRacingResultDataAccess _racingResultDataAccess;

        public RacingResultService(IRacingResultDataAccess racingResultDataAccess)
        {
            _racingResultDataAccess = racingResultDataAccess ?? new RacingResultDataAccess();
        }

        public RacingResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId)
        {
            var criteria = GetCriteria(matchId);
            IEnumerable<Racing> racings = _racingResultDataAccess.GetRacingResult(ref criteria);
            ResultSetting resultSetting = GetFanexResultSetting(criteria.SportType);

            return ResultEngine.HtmlParser<Racing>(racings.ToList(), resultSetting);
        }

        private static GetRacingResultCriteria GetCriteria(int matchId)
        {
            return new GetRacingResultCriteria()
            {
                MatchId = matchId
            };
        }

        /// <summary>
        /// Gets result string.
        /// </summary>
        /// <param name="sportType">Sport type id.</param>
        /// <returns>ResultSetting object.</returns>
        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = sportType == 0 ? SportType.HorseRacing : (SportType)sportType,
                LanguageCode = "en-US",
                VisibleDetailedLink = false,
                VisibleFreeHeader = true,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };
        }
    }
}