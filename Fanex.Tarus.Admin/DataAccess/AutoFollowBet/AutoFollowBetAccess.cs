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
    using System;
    using System.Data;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;
    using Sunplus.Accounting.Core.Extensions;

    public class AutoFollowBetAccess : IAutoFollowBetAccess
    {
        #region IAutoFollowBetAccess Members

        public DataTable AutoFollowBetSelCustomer(GetAutoFollowedBetCriteria criteria)
        {
            var datatable = new DataTable();
            try
            {

                using (var dbContext = new ObjectDb("Acc_AutoFollowBetSelCustomer"))
                {
                    dbContext.AddInParameter("status", DbType.Int32, criteria.Status);
                    var ds = dbContext.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                        datatable = ds.Tables[0];
                }

            }

            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return datatable;

        }

        public Result AutoFollowBetDelCustomer(string username)
        {
            var result = new Result();
            using (var dbContext = new ObjectDb("Acc_AutoFollowBetDelCustomer"))
            {
                dbContext.AddInParameter("username", DbType.String, username);
                dbContext.AddOutParameter("ErrMsg", DbType.String, 4000);
                dbContext.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                dbContext.ExecuteNonQuery();
                result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));

            }
            return result;
        }

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

        public Result AutoFollowBetUpdCustomer(string username, double rate)
        {
            var result = new Result();
            using (var dbContext = new ObjectDb("Acc_AutoFollowBetUpdCustomer"))
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

        #endregion
    }
}
