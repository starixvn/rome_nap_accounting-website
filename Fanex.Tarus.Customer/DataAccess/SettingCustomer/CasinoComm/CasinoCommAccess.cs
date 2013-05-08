using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities;
using System.Collections;
using Fanex.Data;
using Sunplus.Accounting.Core.Extensions;
using System.Data;
using DNAide.Core;
using DNAide.Utils;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.CasinoComm
{
    public class CasinoCommAccess: ICasinoCommAccess
    {
        public Result UpdateCasinoComm(int custId, decimal valuleComm, int userId, string ip)
        {
            var result = new Result();
            try
            {
                if (custId != 0)
                {
                    using (var dbContext = new ObjectDb("UpdateSuperLiveCSComm"))
                    {
                        dbContext.AddInParameter("custid", DbType.Int32, custId);
                        dbContext.AddInParameter("v162discount_1101", DbType.Decimal, valuleComm);
                        dbContext.AddInParameter("ip", DbType.String, ip);
                        dbContext.AddInParameter("userid", DbType.Int32, userId);
                        dbContext.AddOutParameter("errmsg", DbType.String, 1000);
                        dbContext.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                        dbContext.ExecuteDataSet();
                        result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                        result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));
                    }
                }
                else
                {
                    result.ErrorMessage = "Invalid User !";
                    result.ErrorCode = 600;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
            
        }

        public int GetCustIdByUsername(string username)
        {
            int custId = 0;
            try
            {

                using (var dbContext = new ObjectDb("GetCustIdByUsername"))
                {
                    dbContext.AddInParameter("custname", DbType.String, username);
                    var ds = dbContext.ExecuteDataSet();
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        custId = CastAide.AsInt32(row["custid"]);
                    }
                }
            }

            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return custId;
        }


        public ResultCasinoComm CheckUsername(string username)
        {
            var result = new ResultCasinoComm();
            try
            {
                DataTable dtb = null;
                using (var dbContext = new ObjectDb("GetCommSuper"))
                {
                    dbContext.AddInParameter("username", DbType.String, username);
                    dbContext.AddOutParameter("errmsg", DbType.String, 1000);
                    dbContext.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                    var ds = dbContext.ExecuteDataSet();
                    result.ErrorMessage = CastAide.AsString(dbContext.GetParameterValue("ErrMsg"));
                    result.ErrorCode = CastAide.AsInt32(dbContext.GetParameterValue("ErrCode"));
                    if (result.ErrorCode == 0 )
                    {
                        dtb = ds.Tables[0];
                        foreach (DataRow row in dtb.Rows)
                        {
                            result.valueComm = NumberFormat.DecFormat(CastAide.AsDecimal(row["sdiscount"]), 3);
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}
