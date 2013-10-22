// --------------------------------------------------------------------------
// <copyright file="AutoSuspendRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.AutoSuspend
{
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Customer.Entities.AutoSuspend;
    using DNAide.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoSuspendRepository : IAutoSuspendRepository
    {
        public CustomerInfo GetCustomerStatus(GetCustomerStatusCriteria criteria)
        {
            Hashtable hashtableParams = new Hashtable();
            hashtableParams.Add("custid", criteria.CustomerId);
            DataTable dataTable = null;
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(hashtableParams);
                var dataset = objDb.ExecuteDataSet();
                if (dataset != null &&
                    dataset.Tables != null &&
                    dataset.Tables.Count > 0)
                {
                    dataTable = dataset.Tables[0];
                }
            }

            CustomerInfo result = null;
            if (dataTable == null ||
                dataTable.Rows.Count == 0)
            {
                return result;
            }

            var rawCustomerInfo = dataTable.Rows[0];
            result = new CustomerInfo
            {
                Username = CastAide.AsString(rawCustomerInfo["username"]),
                RoleId = CastAide.AsInt32(rawCustomerInfo["roleid"]),
                UplineAutoSuspendSoccer = true && CastAide.AsBoolean(rawCustomerInfo["DisableDownlines151_31"]),
                UplineAutoSuspendOtherSports = true && CastAide.AsBoolean(rawCustomerInfo["DisableDownlines151_31"]),
                AutoSuspendSoccer = true,
                AutoSuspendOtherSports = true
            };

            return result;
        }
    }
}
