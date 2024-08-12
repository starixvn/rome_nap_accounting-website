// --------------------------------------------------------------------------
// <copyright file="GetParlayResultCriteria.cs" company="">
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
    /// The criteria GetParlayResultCriteria.
    /// </summary>
    public class GetParlay3rdResultCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey() => "Age_BetList_SelMatch3rdResultParlay";

        public override bool IsValid() => RefNo > 0;
    }
}
