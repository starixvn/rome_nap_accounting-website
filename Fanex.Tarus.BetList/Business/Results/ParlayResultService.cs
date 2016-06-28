// --------------------------------------------------------------------------
// <copyright file="ParlayResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    using System.Linq;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    /// <summary>
    /// The class ParlayResultService.
    /// </summary>
    public class ParlayResultService : IParlayResultService
    {
        private IParlayResultDataAccess _parlayResultDataAccess;

        public ParlayResultService(IParlayResultDataAccess parlayResultDataAccess)
        {
            this._parlayResultDataAccess = parlayResultDataAccess ?? new ParlayResultDataAccess();
        }

        public ParlayResultService()
            : this(null)
        {
        }

        public string GetResult(long refNo)
        {
            var parlayData = this._parlayResultDataAccess.GetParlayResult(new GetParlayResultCriteria
            {
                RefNo = refNo
            });

            return ResultEngine.HtmlParser(parlayData.ToList(), this.GetFanexResultSetting()).Replace("original-title", " title ");
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <returns>The object of ResultSetting.</returns>
        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                IsOutright = false,
                IsParlayPopUp = true,
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