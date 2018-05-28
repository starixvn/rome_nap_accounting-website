// --------------------------------------------------------------------------
// <copyright file="GetLiveCasinoPopupResultCriteria.cs" company="">
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
    /// The criteria GetLiveCasinoPopupResultCriteria.
    /// </summary>
    public class GetLiveCasinoPopupResultCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int BetType { get; set; }

        public string League { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_SelMatchResult";
        }

        public override bool IsValid()
        {
            return MatchId > 0 && BetType > 0;
        }
    }
}
