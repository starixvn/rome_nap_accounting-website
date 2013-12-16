// --------------------------------------------------------------------------
// <copyright file="UpdateAutoFollowedBetCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.AutoFollowBet
{
    using System.Collections;
    using System;

    /// <summary>
    /// Update Auto Follow Bet Criteria.
    /// </summary>
    public class UpdateAutoFollowedBetCustomerCriteria
    {
        public string Username { get; set; }

        public decimal Rate { get; set; }

        public int SourceId { get; set; }

        public int LiveStatus { get; set; }

        public int FollowedHDP { get; set; }

        public int Followed1stHDP { get; set; }

        public int FollowedOverUnder { get; set; }

        public int Followed1stOverUnder { get; set; }

        /// <summary>
        /// Gets the store procedure parameters.
        /// </summary>
        /// <returns>The store procedure parameters.</returns>
        public Hashtable GetStoreProcedureParams()
        {
            var storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("username", string.IsNullOrEmpty(Username) ? string.Empty : Username);
            storeProcedureParams.Add("rate", Rate);
            storeProcedureParams.Add("followlive", LiveStatus);
            storeProcedureParams.Add("handicap", FollowedHDP);
            storeProcedureParams.Add("1sthandicap", Followed1stHDP);
            storeProcedureParams.Add("overunder", FollowedOverUnder);
            storeProcedureParams.Add("1stoverunder", Followed1stOverUnder);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }

        public string GetSettingKey()
        {
            return "Admin_AutoFollowBetManagement_Update";
        }

        public bool IsValid()
        {
            return 0 <= Rate &&
                Rate <= 3 &&
                Math.Round(Rate, 2) == Rate;
        }
    }
}
