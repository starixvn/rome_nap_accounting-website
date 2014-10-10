// --------------------------------------------------------------------------
// <copyright file="ClearBalanceMembersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="David.tran" date="2014.10.01">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AutoTransferAll
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClearBalanceMembersCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int CustId { get; set; }

        public string AgentName { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BettransInsTransferAllMembers";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
