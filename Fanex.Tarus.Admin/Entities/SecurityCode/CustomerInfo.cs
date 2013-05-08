// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Entities.SecurityCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Utils;
    using Fanex.Security;

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerInfo
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string SecurityCode
        {
            get;
            set;
        }

        public bool IsSelected { get; set; }

        public bool IsValid { get; set; }

        public bool IsSubAccount { get; set; }

        public List<CustomerInfo> SubAccounts;

        public string SecurityHashCode
        {
            get
            {
                var sc = this.SecurityCode;
                if (string.IsNullOrEmpty(sc))
                {
                    return string.Empty;
                }

                if (this.UserId > 0 && this.RoleId > 0)
                {
                    if (this.RoleId == 1) // member
                    {
                        sc = Encryption.CfsEnCode(sc);
                    }
                    else
                    {
                        var md5 = Cryptography.ComputeHash("MD5", this.UserId.ToString(), "1qaw3+{:P)");
                        sc = Cryptography.ComputeHash("SHA1", sc, md5);
                    }
                }
                return sc;
            }
        }
    }
}
