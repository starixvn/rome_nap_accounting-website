// --------------------------------------------------------------------------
// <copyright file="GetCustomerStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AutoSuspend
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCustomerStatusCriteria
    {
        public string CustomerId { get; set; }

        public string GetSettingKey()
        {
            return "Customer_AutoSuspend_GetCustomerStatus";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
