// --------------------------------------------------------------------------
// <copyright file="UpdateBetTradeStatusCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.BetTrade
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Helper;

    public class UpdateBetTradeStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsEnable { get; set; }

        public bool ApplyToMember { get; set; }

        public int ProductId
        {
            get
            {
                return AppSettings.ProductIdBetTrade;
            }
        }

        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string IP
        {
            get
            {
                return Utils.GetClientIP();
            }
        }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_BetTrade_UpdateStatus";
        }
    }
}
