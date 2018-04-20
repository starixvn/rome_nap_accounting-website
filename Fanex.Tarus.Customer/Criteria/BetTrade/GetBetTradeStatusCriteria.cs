// --------------------------------------------------------------------------
// <copyright file="GetBetTradeStatusCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.BetTrade
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Configurations;

    public class GetBetTradeStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public int RoleId { get; set; }

        public int ProductId
        {
            get
            {
                return AppSettings.ProductIdBetTrade;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_BetTrade_GetStatus";
        }
    }
}
