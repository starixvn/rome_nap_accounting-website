// --------------------------------------------------------------------------
// <copyright file="DoReplicateLicenseeSettingsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.ReplicateLicenseeSettings
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DoReplicateLicenseeSettingsCriteria
    {
        public string LicenseeName { get; set; }

        public string GetSettingKey()
        {
            return "Customer_ReplicateLicenseeSettings_DoAction";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
