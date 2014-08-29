// --------------------------------------------------------------------------
// <copyright file="GetCustomerStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AGCasino
{
    using System.Collections;

    /// <summary>
    /// Get Customer Status Criteria.
    /// </summary>
    public class GetCustomerStatusCriteria
    {
        public int RoleId { get; set; }

        public int CustomerId { get; set; }

        public string GetSettingKey()
        {
            return "Customer_AGCasino_GetCustomerSetting";
        }

        public bool IsValid()
        {
            return true;
        }

        public Hashtable GetSPParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", CustomerId);
            return storeProcedureParams;
        }
    }
}
