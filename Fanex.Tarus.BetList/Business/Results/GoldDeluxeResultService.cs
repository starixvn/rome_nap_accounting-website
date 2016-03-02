// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2015.05.22">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using System.Collections.Generic;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.GoldDeluxe.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    /// <summary>
    /// The class ScoreInformationService.
    /// </summary>
    public class GoldDeluxeResultService : IGoldDeluxeResultService
    {
        private IGoldDeluxeResultDataAccess _goldDeluxeResultService;

        public GoldDeluxeResultService(IGoldDeluxeResultDataAccess goldDeluxeDataAccess)
        {
            this._goldDeluxeResultService = goldDeluxeDataAccess ?? new GoldDeluxeResultDataAccess();
        }

        public GoldDeluxeResultService()
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
        public string GetResult(long transId, int bettype)
        {
            var criteria = GetCriteria(transId, bettype);
            List<GoldDeluxe> goldDeluxe = this._goldDeluxeResultService.GetGoldDeluxePopUpResult(criteria);

            if (goldDeluxe != null && goldDeluxe.Count > 0)
            {
                return ResultEngine.HtmlParser(goldDeluxe, GetFanexResultSetting());
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

        private static GetGoldDeluxePopUpResultCriteria GetCriteria(long transId, int bettype)
        {
            return new GetGoldDeluxePopUpResultCriteria()
            {
                Bettype = bettype,
                TransID = transId
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.GoldDeluxe,
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