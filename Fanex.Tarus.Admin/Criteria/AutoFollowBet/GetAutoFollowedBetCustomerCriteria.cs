// --------------------------------------------------------------------------
// <copyright file="GetAutoFollowedBetCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.AutoFollowBet
{
    using System.Collections;

    /// <summary>
    /// Get Auto Follow BetCriteria.
    /// </summary>
    public class GetAutoFollowedBetCustomerCriteria
    {
        public string Username { get; set; }

        public int BAStatus { get; set; }

        public int Source { get; set; }

        public string LiveStatus { get; set; }

        public string HDP { get; set; }

        public string _1stHDP { get; set; }

        public string OverUnder { get; set; }

        public string _1stOverUnder { get; set; }

        /// <summary>
        /// Gets the store procedure parameters.
        /// </summary>
        /// <returns>The store procedure parameters.</returns>
        public Hashtable GetStoreProcedureParams()
        {
            var storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("username", string.IsNullOrEmpty(Username) ? string.Empty : Username);
            storeProcedureParams.Add("source", Source);
            storeProcedureParams.Add("bastatus", BAStatus);
            storeProcedureParams.Add("followlive", LiveStatus);
            storeProcedureParams.Add("handicap", HDP);
            storeProcedureParams.Add("1sthandicap", _1stHDP);
            storeProcedureParams.Add("overunder", OverUnder);
            storeProcedureParams.Add("1stoverunder", _1stOverUnder);
            return storeProcedureParams;
        }

        public string GetSettingKey()
        {
            return "Admin_AutoFollowBetManagement_Select";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
