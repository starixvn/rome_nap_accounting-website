// --------------------------------------------------------------------------
// <copyright file="ResetSecurityCodeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ResetSecurityCodeCriteria
    {
        public int Custid { get; set; }

        public int Userid { get; set; }

        public bool IsForcedChangeSecurityCode { get; set; }

        public string Securitycode { get; set; }

        public int RoleId { get; set; }

        public string Language
        {
            get
            {
                return "en";
            }
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string IP { get; set; }

        public string GetSettingKey()
        {
            return "DBA_ResetSecurityCode_Agent";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
