// --------------------------------------------------------------------------
// <copyright file="GetGoldDeluxeStatusCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.GoldDeluxe
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Gold Deluxe status criteria.
    /// </summary>
    public class GetGoldDeluxeStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int ProductId
        {
            get
            {
                return 11;
            }
        }

        public override string GetSettingKey()
        {
            return "Acc_3rdProductSelSuperStatus";
        }
    }
}
