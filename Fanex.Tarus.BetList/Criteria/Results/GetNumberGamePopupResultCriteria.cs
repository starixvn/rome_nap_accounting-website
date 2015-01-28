// --------------------------------------------------------------------------
// <copyright file="GetNumberGamePopupResultCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// The criteria GetNumberGamePopupResultCriteria.
    /// </summary>
    public class GetNumberGamePopupResultCriteria : CriteriaBase
    {
        public int MatchID { get; set; }

        public override string GetSettingKey()
        {
            return "SelMatchResultNumberGame";
        }

        public override bool IsValid()
        {
            return MatchID > 0;
        }
    }
}
