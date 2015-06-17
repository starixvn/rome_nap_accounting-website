// --------------------------------------------------------------------------
// <copyright file="GetMicroGamingStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.MicroGaming
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Configurations;

    public class GetStatusCriteria : CriteriaBase
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
                return AppSettings.ProductIdMicrogaming;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_MicroGaming_GetStatus";
        }
    }
}
