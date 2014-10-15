//-------------------------------------------------------------------------
// <copyright file="CheckImportDirectAccountFileCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Fanex.Tarus.Infrastructure.Common;

    /// <summary>
    /// Check Import Direct Account File Criteria.
    /// </summary>
    public class CheckImportDirectAccountFileCriteria
    {
        public List<string> UsernameList { get; set; }

        public int UserId { get; set; }

        public string GetSettingKey()
        {
            return "Customer_ConfigureDirectAccount_CheckImportDirectAccount";
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
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("listtransfer", GetListTrangsfer());
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }

        private string GetListTrangsfer()
        {
            string listTransfer = "<ListCustDirect>";
            for (var i = 0; i < UsernameList.Count; i++)
            {
                listTransfer += "<User Seq=\"" + (i + 1) + "\"" + " Username=\"" + UsernameList[i] + "\"></User>";
            }

            listTransfer += "</ListCustDirect>";
            return listTransfer;
        }
    }
}