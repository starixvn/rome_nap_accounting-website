// --------------------------------------------------------------------------
// <copyright file="GetCustomerSettingCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.AGCasino
{
    using System.Collections;

    /// <summary>
    /// Get Customer Setting Criteria.
    /// </summary>
    public class GetCustomerSettingCriteria
    {
        public int CustomerId { get; set; }

        public int UplineId { get; set; }

        public int RoleId { get; set; }

        public int ExchangeId { get; set; }

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
