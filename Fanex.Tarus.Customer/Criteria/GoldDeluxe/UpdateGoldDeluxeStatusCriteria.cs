// --------------------------------------------------------------------------
// <copyright file="UpdateGoldDeluxeStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.GoldDeluxe
{
    using System.Globalization;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Gold Deluxe status criteria.
    /// </summary>
    public class UpdateGoldDeluxeStatusCriteria : GetGoldDeluxeStatusCriteria
    {
        public bool IsEnable { get; set; }

        public bool ApplyToMember { get; set; }

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

        public string Language
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_3rdProductUpdSuperStatus";
        }
    }
}
