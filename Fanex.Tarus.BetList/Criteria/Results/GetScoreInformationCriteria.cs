// --------------------------------------------------------------------------
// <copyright file="GetScoreInformationCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria GetScoreInformationCriteria.
    /// </summary>
    public class GetScoreInformationCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int BetType { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        public long BetId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_SelMatchResult";
        }

        public override bool IsValid()
        {
            return this.MatchId > 0;
        }
    }
}
