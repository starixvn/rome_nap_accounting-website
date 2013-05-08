using System;
using System.Collections;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Core.LoggingLib;
using Fanex.Data;
using Sunplus.Accounting.Entities;
using DNAide.Core;

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
                using (ObjectDb objDB = new ObjectDb("Acc_Con_AdminSelPwdVerify"))
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

        public static IDataReader AccInformationSelect(int custid)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                var spElement = GetUserStoreProcedure(UserControlSession.StoreCollection.AccountInfo, "CustomerSelUserInfo");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
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
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdUserInfo"))
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
