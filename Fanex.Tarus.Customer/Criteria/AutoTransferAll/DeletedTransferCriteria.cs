// --------------------------------------------------------------------------
// <copyright file="DeletedTransferCriteria.cs" company="">
// Copyright (c) . All rights reserved.
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
    public class DeletedTransferCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerDelAutoTransferAgent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
