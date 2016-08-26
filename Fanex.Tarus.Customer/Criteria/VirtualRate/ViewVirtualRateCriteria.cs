// --------------------------------------------------------------------------
// <copyright file="ViewVirtualRateCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    /// <summary>
    /// View Virtual Rate Criteria.
    /// </summary>
    public class ViewVirtualRateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public string RoleName { get; set; }

        public bool IsTest
        {
            get
            {
                return WebSettings.IsVirtualRateTest;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_GetVirtualRate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
