using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Fanex.Data;
using DNAide.Core;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class Credit
    {
        /// <summary>
        /// Select Credit for customer
        /// SP: Acc_CustomerSelCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_custId"></param>
        /// <returns></returns>
        public static IDataReader CustomerSelectCredit(int _custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", _custId);
                
                using (ObjectDb db = new ObjectDb("CustomerSelCredit"))
                {
                    db.AssignParamValues(param);
                    return db.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Update New Credit for customer
        /// SP: Acc_CustomerUpdCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_accId"></param>
        /// <param name="_custId"></param>
        /// <param name="_newCredit"></param>
        /// <returns></returns>
        public static void CustomerUpdateCredit(int _accId, int _custId, decimal _newCredit)
        {
            try
            {                
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdCredit", 600))
                {
                    var param = new Hashtable();
                    param.Add("userid", _accId);
                    param.Add("custid", _custId);
                    param.Add("newcredit", _newCredit);
                    param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    objDb.AssignParamValues(param);

                    objDb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objDb.GetParameterValue("errcode")) > 0)
                        throw new AccException(CastAide.AsString(objDb.GetParameterValue("errmsg")));
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

    public sealed class UserCredit
    {
        /// <summary>
        /// Select Agent Credit --
        /// store procedure: Acc_CustomerSelUsedCredit_Ag --
        /// @viet hoang
        /// </summary>        
        /// <param name="_agentId"></param>
        /// <returns></returns>
        public static DataTable SelectAgentCredit(int _agentId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelUsedCredit_Ag", 600))
                {
                    var param = new Hashtable();
                    param.Add("agentid", _agentId);

                    objDB.AssignParamValues(param);

                    var ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }

                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// Select Master Credit --
        /// Store Procedure: Acc_CustomerSelUsedCredit_MAg --
        /// @viet hoang
        /// </summary>
        /// <param name="_masterId"></param>
        /// <returns></returns>
        public static DataTable SelectMasterCredit(int _masterId)
        {
            DataTable dt = new DataTable();
            try
            {
               
                using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelUsedCredit_MAg", 600))
                {
                    var param = new Hashtable();
                    param.Add("masterid", _masterId);

                    objDB.AssignParamValues(param);

                    var ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// Select Super Credit Details --
        /// Store Procedure: Acc_CustomerSelUsedCredit_SAg --
        /// @viet hoang
        /// </summary>
        /// <param name="_superId"></param>
        /// <returns></returns>
        public static DataTable SelectSuperCredit(int _superId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_CustomerSelUsedCredit_SAg", 600))
                {
                    var param = new Hashtable();
                    param.Add("superid", _superId);

                    objDB.AssignParamValues(param);
                    var ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// Agent Reset Member Credit
        /// SP: Acc_CustomerUpdResetMemberCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_agentId"></param>
        /// <param name="closedOnly"></param>
        /// <returns></returns>
        public static void ResetMemberCredit(int _userId, int _agentId, bool closedOnly)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdResetMemberCredit", 600))
                {
                    var param = new Hashtable();
                    param.Add("userid", _userId);
                    param.Add("agentid", _agentId);
                    param.Add("closedonly", closedOnly);
                    param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    objDb.AssignParamValues(param);

                    objDb.ExecuteNonQuery();
                    if(CastAide.AsInt32(objDb.GetParameterValue("errcode")) > 0)
                        throw new AccException(string.Format("{0}", objDb.GetParameterValue("errmsg")));
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
