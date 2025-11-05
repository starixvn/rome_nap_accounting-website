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
    using Fanex.BetList.Core.Entities;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using Sunplus.Accounting.Core.Common;
    using System.Linq;

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

            var distinctParlayData = parlayData.Select(p => new Parlay
            {
                SportId = p.SportId,
                LeagueId = p.LeagueId,
                Sportbook = p.Sportbook.GroupBy(sb => sb.MatchId).Select(g => g.First()).ToList()
            });

            return ResultEngine.HtmlParser(distinctParlayData.ToList(), this.GetFanexResultSetting()).Replace("original-title", " title ");
        }

        public string Get3rdResult(long refNo)
        {
            var parlayData = this._parlayResultDataAccess.GetParlay3rdResult(new GetParlay3rdResultCriteria
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
                IsPopupOpenFromBetList = true,
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