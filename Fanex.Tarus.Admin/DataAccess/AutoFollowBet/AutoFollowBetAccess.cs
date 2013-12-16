// --------------------------------------------------------------------------
// <copyright file="AutoFollowBetAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.AutoFollowBet
{
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;

    public class AutoFollowBetAccess : IAutoFollowBetAccess
    {
        public Result AutoFollowBetInsCustomer(string username, double rate)
        {
            var result = new Result();
            using (var dbContext = new ObjectDb("Acc_AutoFollowBetInsCustomer"))
            {
                dbContext.AddInParameter("username", DbType.String, username);
                dbContext.AddInParameter("rate", DbType.Double, rate);
                dbContext.AddOutParameter("ErrMsg", DbType.String, 4000);
                dbContext.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                dbContext.ExecuteNonQuery();
                result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));

            }
            return result;
        }

        public List<FollowedCustomer> GetAutoFollowedBetCustomer(GetAutoFollowedBetCustomerCriteria criteria)
        {
            var datatable = new DataTable();
            using (var databaseContext = new ObjectDb(criteria.GetSettingKey()))
            {
                databaseContext.AssignParamValues(criteria.GetStoreProcedureParams());
                var dataset = databaseContext.ExecuteDataSet();
                if (dataset.Tables.Count > 0)
                {
                    datatable = dataset.Tables[0];
                }
            }

            var listFollowedCustomer = new List<FollowedCustomer>();
            datatable.DefaultView.Sort = "username DESC";
            foreach (DataRow row in datatable.Rows)
            {
                listFollowedCustomer.Add(new FollowedCustomer
                {
                    Username = CastAide.AsString(row["username"]),
                    Rate = CastAide.AsDouble(row["rate"]),
                    BAStatus = true,

                    SourceId = CastAide.AsInt32(row["src"]),
                    LiveStatus = CastAide.AsInt32(row["FollowLive"]),
                    FollowedHDP = CastAide.AsInt32(row["Follow1"]),
                    Followed1stHDP = CastAide.AsInt32(row["Follow7"]),
                    FollowedOverUnder = CastAide.AsInt32(row["Follow3"]),
                    Followed1stOverUnder = CastAide.AsInt32(row["Follow8"])
                });
            }

            return listFollowedCustomer;
        }

        public Result DeleteAutoFollowedBetCustomer(DeleteAutoFollowedBetCustomerCriteria criteria)
        {
            var result = new Result();
            using (var dbContext = new ObjectDb(criteria.GetSettingKey()))
            {
                dbContext.AssignParamValues(criteria.GetStoreProcedureParams());
                dbContext.ExecuteNonQuery();
                result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));
            }

            return result;
        }

        public Result UpdateAutoFollowedBetCustomer(UpdateAutoFollowedBetCustomerCriteria criteria)
        {
            var result = new Result();
            using (var dbContext = new ObjectDb(criteria.GetSettingKey()))
            {
                dbContext.AssignParamValues(criteria.GetStoreProcedureParams());
                dbContext.ExecuteNonQuery();
                result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));
            }

            return result;
        }
    }
}
