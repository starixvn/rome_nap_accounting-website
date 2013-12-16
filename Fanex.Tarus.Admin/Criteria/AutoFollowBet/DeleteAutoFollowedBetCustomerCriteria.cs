// --------------------------------------------------------------------------
// <copyright file="DeleteAutoFollowedCustomerBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.AutoFollowBet
{
    using System.Collections.Generic;
    using Fanex.Tarus.Admin.Models.AutoFollowBet;
    using System.Collections;

    /// <summary>
    /// Delete Auto Follow Bet Criteria.
    /// </summary>
    public class DeleteAutoFollowedBetCustomerCriteria
    {
        public string Username { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public Hashtable GetStoreProcedureParams()
        {
            var storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("username", Username);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }

        public string GetSettingKey()
        {
            return "Admin_AutoFollowBetManagement_Delete";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
