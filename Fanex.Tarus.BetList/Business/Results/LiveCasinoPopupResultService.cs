// --------------------------------------------------------------------------
// <copyright file="LiveCasinoPopupResultService.cs" company="Nexcel Solutions Vietnam">
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
    /// The class LiveCasinoPopupResultService.
    /// </summary>
    public class LiveCasinoPopupResultService : ILiveCasinoPopupResultService
    {
        private ILiveCasinoPopupResultDataAccess _liveCasinoPopupResultDataAccess;

        public LiveCasinoPopupResultService(ILiveCasinoPopupResultDataAccess liveCasinoPopupResultDataAccess)
        {
            this._liveCasinoPopupResultDataAccess = liveCasinoPopupResultDataAccess ?? new LiveCasinoPopupResultDataAccess();
        }

        public LiveCasinoPopupResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int betType, string league)
        {
            var criteria = new GetLiveCasinoPopupResultCriteria()
            {
                MatchId = matchId,
                BetType = betType,
                League = league
            };

            return ResultEngine.HtmlParser(this._liveCasinoPopupResultDataAccess.GetLiveCasinoPopupResult(criteria), GetFanexResultSetting());
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.LiveCasino,
                IsOutRight = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = false,
                HighlightWinnerTeam = true
            };
        }
    }
}