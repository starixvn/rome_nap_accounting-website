// --------------------------------------------------------------------------
// <copyright file="UpdateLicenseeInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.EditLicensee
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities.EditLicensee;

    public class UpdateLicenseeInfoCriteria
    {
        public string Site { get; set; }

        public string Currency { get; set; }

        public bool EnableNumberGame { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public List<SettingInfo> SettingInfo { get; set; }

        public List<VirtualSportsSettingInfo> VirtualSportsSettingInfo { get; set; }

        public string Editor { get; set; }

        public string GetSettingKey()
        {
            return "Customer_EditLicensee_UpdateLicenseeInfo";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
