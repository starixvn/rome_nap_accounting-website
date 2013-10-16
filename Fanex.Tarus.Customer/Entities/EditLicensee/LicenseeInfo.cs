// --------------------------------------------------------------------------
// <copyright file="LicenseeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.EditLicensee
{
    using System;

    [Serializable]
    public class LicenseeInfo
    {
        public string Site { get; set; }

        public string Currency { get; set; }

        public bool EnableNumberGame { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
