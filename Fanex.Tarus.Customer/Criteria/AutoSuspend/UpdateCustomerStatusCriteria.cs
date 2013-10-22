// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerStatusCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateCustomerStatusCriteria
    {
        public string CustomerId { get; set; }

        public int RoleId { get; set; }

        public bool UplineAutoSuspendSoccer { get; set; }

        public bool UplineAutoSuspendOtherSports { get; set; }

        public bool AutoSuspendSoccer { get; set; }

        public bool AutoSuspendOtherSports { get; set; }

        public string GetSettingKey()
        {
            return "Customer_AutoSuspend_UpdateCustomerStatus";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
