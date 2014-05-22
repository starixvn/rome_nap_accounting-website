// --------------------------------------------------------------------------
// <copyright file="LimitNewMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.LimitNewMember
{
    using System;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// LimitNewMemberCriteria class.
    /// </summary>
    public class LimitNewMemberCriteria
    {
        private const int MaxLimit = 9999;

        public string Super { get; set; }

        // U: update | Q: query.
        public string Mode { get; set; }

        public int Limit { get; set; }

        public string Language
        {
            get
            {
                return "E";
            }
        }

        public string IP
        {
            get
            {
                return Utils.GetClientIP();
            }
        }

        public int Creator
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string GetSettingKey()
        {
            return "CustomerMaxNewMemPerDay";
        }

        public bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(this.Super) && 
                string.Equals(this.Mode, "U", StringComparison.OrdinalIgnoreCase)) &&
                this.Limit <= MaxLimit;
        }
    }
}
