// --------------------------------------------------------------------------
// <copyright file="ViewKenoPTCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Entities.Criteria.ViewPT
{
    using System.Collections;

    /// <summary>
    /// View Keno PT Criteria.
    /// </summary>
    public class ViewKenoPTCriteria
    {
        public string Username { get; set; }

        public int SportId { get; set; }

        public int BetType { get; set; }

        public bool IsLive { get; set; }

        public bool HasCommission { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public Hashtable GetSpParam()
        {
            var spParam = new Hashtable();
            spParam.Add("username", Username);
            spParam.Add("bettype", BetType);
            spParam.Add("islive", IsLive);
            spParam.Add("hascomm", HasCommission);
            spParam.Add("errcode", 0);
            spParam.Add("errmsg", string.Empty);

            return spParam;
        }

        public string GetSettingKey()
        {
            return "ViewPT_Keno";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
