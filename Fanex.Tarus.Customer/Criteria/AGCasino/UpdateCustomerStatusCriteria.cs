// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AGCasino
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Fanex.Tarus.Customer.Entities.AGCasino;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;

    /// <summary>
    /// Update Customer Status Criteria.
    /// </summary>
    public class UpdateCustomerStatusCriteria
    {
        public string CustomerId { get; set; }

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

        public int RoleId { get; set; }

        public List<ProductInfo> ProductInfo { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustomerUpdAGCasinoStatus";
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
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("ip", IP);
            storeProcedureParams.Add("roleid", RoleId);

            var sportLength = ProductInfo == null ? 0 : ProductInfo.Count;
            for (var i = 0; i < sportLength; i++)
            {
                var sport = ProductInfo[i];
                var sportName = sport.Name.ToLower();
                if (string.Equals(sportName, "AG Casino", StringComparison.OrdinalIgnoreCase))
                {
                    storeProcedureParams.Add("isenableag", sport.Enable);
                    storeProcedureParams.Add("applytomember", sport.EnableDownlines);
                }
            }

            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            storeProcedureParams.Add("langkey", Language);
            return storeProcedureParams;
        }
    }
}
