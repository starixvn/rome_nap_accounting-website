// --------------------------------------------------------------------------
// <copyright file="UpdateMicroGamingStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.MicroGaming
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Helper;

    public class UpdateMicroGamingStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsEnable { get; set; }

        public bool ApplyToMember { get; set; }

        public int ProductId
        {
            get
            {
                return AppSettings.ProductIdMicrogaming;
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
            return "Customer_MicroGaming_UpdateStatus";
        }
    }
}
