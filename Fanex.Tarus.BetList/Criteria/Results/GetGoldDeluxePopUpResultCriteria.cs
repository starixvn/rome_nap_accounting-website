// --------------------------------------------------------------------------
// <copyright file="GetGoldDeluxePopUpResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2015.05.22">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Gold Deluxe Pop Up Result Criteria.
    /// </summary>
    public class GetGoldDeluxePopUpResultCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int Bettype { get; set; }

        public long TransID { get; set; }

        public override string GetSettingKey()
        {
            return "All_BetList_MatchResultGDSel";
        }

        public override bool IsValid()
        {
            return TransID > 0;
        }
    }
}
