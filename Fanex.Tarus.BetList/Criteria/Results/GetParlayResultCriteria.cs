// --------------------------------------------------------------------------
// <copyright file="GetParlayResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetParlayResultCriteria.
    /// </summary>
    public class GetParlayResultCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResultParlay";
        }

        public override bool IsValid()
        {
            return RefNo > 0;
        }
    }
}
