//-------------------------------------------------------------------------
// <copyright file="UpdateDirectAccountStatusCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount
{
    using System.Collections;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Customer Status Criteria.
    /// </summary>
    public class UpdateDirectAccountStatusCriteria
    {
        public int CustomerId { get; set; }

        public string Username { get; set; }

        public bool IsCheckedDirectStatus { get; set; }

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

        public string GetSettingKey()
        {
            return "Customer_ConfigureDirectAccount_UpdateStatus";
        }

        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>Store Procedure Parameters.</returns>
        public Hashtable GetSPParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", CustomerId);
            storeProcedureParams.Add("creator", UserId);
            storeProcedureParams.Add("ip", IP);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }
    }
}