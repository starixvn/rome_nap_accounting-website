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
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Customer.Entities.AutoSuspend;
    using Sunplus.Accounting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoSuspendRepository : IAutoSuspendRepository
    {
        /// <summary>
        /// Gets the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Info.</returns>
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
                Username = CastAide.AsString(rawCustomerInfo["Username"]),
                RoleId = CastAide.AsInt32(rawCustomerInfo["RoleId"]),
                UplineAutoSuspendSoccer = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendUplineSoccer"]),
                UplineAutoSuspendOtherSports = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendUplineOtherSports"]),
                AutoSuspendSoccer = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendSoccer"]),
                AutoSuspendOtherSports = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendOtherSports"]),
                AutoSuspendSoccerDownlines = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendDownlinesSoccer"]),
                AutoSuspendOtherSportsDownlines = CastAide.AsBoolean(rawCustomerInfo["AutoSuspendDownlinesOtherSports"])
            };

            return result;
        }

        /// <summary>
        /// Updates the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        public Result UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            Hashtable hashtableParams = new Hashtable();
            hashtableParams.Add("custid", criteria.CustomerId);
            hashtableParams.Add("userid", criteria.UserId);
            hashtableParams.Add("ip", criteria.IP);
            Hashtable outputParams = new Hashtable();
            hashtableParams.Add("roleid", criteria.RoleId);
            hashtableParams.Add("autosuspend", criteria.AutoSuspendSoccer);
            hashtableParams.Add("autosuspendother", criteria.AutoSuspendOtherSports);
            hashtableParams.Add("errcode", 0);
            hashtableParams.Add("errmsg", string.Empty);
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(hashtableParams);
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }

            Result updateResult = new Result
            {
                ErrorCode = CastAide.AsInt32(outputParams["errcode"]),
                ErrorMessage = CastAide.AsString(outputParams["errmsg"])
            };

            return updateResult;
        }
    }
}
