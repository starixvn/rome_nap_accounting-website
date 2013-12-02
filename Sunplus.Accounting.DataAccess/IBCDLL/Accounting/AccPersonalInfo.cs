using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccPersonalInfo
    {
        private static StoreProcedureElement GetStoreProcedure(SportbookPtSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = SportbookPtSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        private static StoreProcedureElement GetUserStoreProcedure(UserControlSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = UserControlSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        public static Result AccPasswordVerify(string accName, int accId, string pass)
        {
            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_Con_AdminSelPwdVerify", 600))
                {
                    var param = new Hashtable();
                    param.Add("username", accName);
                    param.Add("userpwd", pass);
                    param.Add("userid", accId);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    objDB.AssignParamValues(param);
                    objDB.ExecuteNonQuery();

                    result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 1;
                result.ErrorMessage = ex.Message;
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public static DataSet AccInformationSelect(int custid)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                
                using (ObjectDb db = new ObjectDb("CustomerSelUserInfo", 600))
                {
                    db.AssignParamValues(param);
                    return db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public static void AccInformationUpdate(Hashtable param)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdUserInfo", 600))
                {
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    objDb.AssignParamValues(param);
                    objDb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objDb.GetParameterValue("errcode")) > 0)
                    {
                        throw new AccException(CastAide.AsString(objDb.GetParameterValue("errmsg")));
                    }
                }
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
    }
}
