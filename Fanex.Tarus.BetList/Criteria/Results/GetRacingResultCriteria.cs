// --------------------------------------------------------------------------
// <copyright file="GetRacingResultCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.Results
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria GetRacingResultCriteria.
    /// </summary>
    public class GetRacingResultCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int LeagueID { get; set; }

        public int RaceNo { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_SelMatchResult2";
        }

        public override bool IsValid()
        {
            return EventDate > DateTime.MinValue && RaceNo > 0;
        }
    }
}
