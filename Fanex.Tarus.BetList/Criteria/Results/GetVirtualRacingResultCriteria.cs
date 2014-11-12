// --------------------------------------------------------------------------
// <copyright file="GetVirtualRacingResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria GetVirtualRacingResultCriteria.
    /// </summary>
    public class GetVirtualRacingResultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "MatchID")]
        public int MatchId { get; set; }

        public int? RaceNo { get; set; }

        public int LeagueID { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_HRSelMatchResult2";
        }
    }
}
