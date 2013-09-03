using System;
using System.Collections;
using System.Linq;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    using Fanex.Data;
    using Sunplus.Accounting.Core.Common;

    public sealed class CustomerUpdateStatus : DataAccess
    {
        private StoreProcedureElement GetCustomerStoreProcedure(string aliasName)
        {
            return GetStoreProcedure(Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
        }

        /// <summary>
        /// Update Open/Closed status for customer
        /// sp: Acc_CustomerUpdClosed
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        ///  protected StoreProcedureElement GetStoreProcedure(string site)
        public static void CustomerUpdateClosed(int accid, int custid, bool isClosed, string Ip, int roleId = 0)
        {
            try
            {
                if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL)
                    && roleId == 4 &&
                    !WebSettings.ExternalAdminUsersEditSuperStatus.Contains(AppSession.AccId.ToString()))
                {
                    return;
                }

                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdClosed", 600))
                {
                    Hashtable param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("closed", isClosed);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);// Vinh add Ip for closed member

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

        public static void CustomerUpdateAutoMPT(int accid, int custid, bool isEnabled, string Ip)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disableautompt", !isEnabled);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);// Vinh add Ip for closed member

                using (ObjectDb objDB = new ObjectDb("Acc_CustomerUpdMaster_DisableAutoMPT"))
                {
                    objDB.AssignParamValues(param);
                    objDB.ExecuteNonQuery();

                    if (CastAide.AsInt32(objDB.GetParameterValue("errcode")) > 0)
                    {
                        throw new AccException(CastAide.AsString(objDB.GetParameterValue("errmsg")));
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

        /// <summary>
        /// Update suspended status for customer
        /// sp: Acc_CustomerUpdClosed
        /// @Vinh
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateSuspend(int accid, int custid, bool isSpecial, string Ip)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_CustomerUpdSuspended"))
                {
                    var param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("suspended", isSpecial);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);

                    objDB.AssignParamValues(param);
                    objDB.ExecuteNonQuery();

                    if (CastAide.AsInt32(objDB.GetParameterValue("errcode")) > 0)
                    {
                        throw new AccException(CastAide.AsString(objDB.GetParameterValue("errmsg")));
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

        /// <summary>
        /// Update DisableOutright  status for customer
        /// sp: Acc_CustomerUpdDisableOutright
        /// @Vinh
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateDisableOutright(int accid, int custid, bool disableoutright, string Ip)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdDisableOutright", 600))
                {
                    var param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("disableoutright", disableoutright);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);

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

        /// <summary>
        /// update special status for customer
        /// sp: Acc_CustomerUpdImportant
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateSpecial(int accid, int custid, bool isSpecial)
        {
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdImportant", 300))
                {
                    Hashtable param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("important", isSpecial);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objdb.GetParameterValue("errcode")) > 0)
                        throw new AccException(objdb.GetParameterValue("errmsg").ToString());
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

        /// <summary>
        /// Update dangerous status member for customer
        /// sp: Acc_CustomerUpdDangerCust
        /// @viet hoang
        /// @dungtong changed :use the new integer field danger
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        // public static void CustomerUpdateDangerous(int accid, int custid, bool isDangerous)
        public static bool CustomerUpdateDangerous(int accid, int custid, int dangerLevel, int dangertype)
        {
            bool isBA = false;
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdDangercust", 300))
                {
                    Hashtable param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("dangertype", dangertype);
                    param.Add("dangerlevel", dangerLevel);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("isba", 0);
                    param.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objdb.GetParameterValue("errcode")) > 0)
                        throw new AccException(string.Format("{0}", objdb.GetParameterValue("errmsg")));
                    isBA = CastAide.AsInt32(objdb.GetParameterValue("isba")) == 0 ? false : true;
                }
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return isBA;
        }

        /// <summary>
        /// Update disableHR status for customer
        /// sp: Acc_CustomerUpdDisableHR
        /// @phuong.nguyen
        /// Create date: <2008-10-22>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledHR(int accid, int custid, bool disabledHR, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disablehorseracing", disabledHR);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownline", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableHR", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Update disableOpenTote status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledHROpenTote(int accid, int custid, bool disabledHR, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disableopentote", disabledHR);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownline", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableOpenTote", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Update DisabledBingoGame status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledBingoGame(int accid, int custid, bool disabledBingo, string Ip, bool isEnabledDownline, bool isSync)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_UpdBingoStatus", 600))
                {
                    Hashtable param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("disabledbingo", disabledBingo);
                    param.Add("syncbg", isSync);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("loginip", Ip);
                    param.Add("enabledownline", isEnabledDownline);

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

        /// <summary>
        /// Update DisabledNumberGame status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledNumberGame(int accid, int custid, bool disabledNumber, string Ip, bool isDisableDownline)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdDisableNumber", 600))
                {
                    var param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("disablenumber", disabledNumber);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);
                    param.Add("disabledownline", isDisableDownline);

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

        /// <summary>
        /// Update Limit Per IP
        /// @huynguyen
        /// Create date: <2010-10-15>
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="limitperid"></param>
        /// <param name="Ip"></param>
        /// <param name="isDisableDownline"></param>
        public static void CustomerUpdateLimitPerIP(int accid, int custid, int limitperid, string Ip, bool isDisableDownline)
        {
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdIPLimit", 300))
                {
                    var param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("limitperip", limitperid);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);
                    param.Add("removedownlines", isDisableDownline);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objdb.GetParameterValue("errcode")) > 0)
                        throw new AccException(objdb.GetParameterValue("errmsg").ToString());
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

        /// <summary>
        /// Update disableFinancial status for customer
        /// sp: Acc_CustomerUpdDisableFincancial
        /// @phuong.nguyen
        /// Create date: <2008-10-22>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledFinancial(int accid, int custid, bool disabledFinancial, string Ip, bool isUpdDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disablefinancial", disabledFinancial);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownlinefi", isUpdDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableFI", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Update customer Internal status
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="autoSuspend"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static void CustomerUpdInternal(int custId, bool isInternal, int userId, string ip)
        {
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdInternal", 300))
                {
                    var param = new Hashtable();
                    param.Add("userid", userId);
                    param.Add("custid", custId);
                    param.Add("isinternal", isInternal);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", ip);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objdb.GetParameterValue("errcode")) > 0)
                        throw new AccException(objdb.GetParameterValue("errmsg").ToString());
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

        public static void CustomerUpdateSelectedBetting(int accid, int custid, bool selectedBetting, string Ip, bool isUpdDownline)
        {
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdSelectedBetting", 300))
                {
                    var param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("selectedbetting", selectedBetting);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    param.Add("ip", Ip);
                    param.Add("removedownlines", isUpdDownline);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();

                    if (CastAide.AsInt32(objdb.GetParameterValue("errcode")) > 0)
                        throw new AccException(objdb.GetParameterValue("errmsg").ToString());
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

        // update level dangerous for customer
        //@sp: Acc_CustomerUpdDangercust
        // by: faisall.tran
        public static Result UpdateLevelDangerous(int custId, int updateDownline, int dangerType, int dangerLevel, int userId, string ip)
        {
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdDangercust", 300))
                {
                    var param = new Hashtable();
                    param.Add("custid", custId);
                    param.Add("updatedownline", updateDownline);
                    param.Add("dangertype", dangerType);
                    param.Add("dangerlevel", dangerLevel);
                    param.Add("userid", userId);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", "Updated Successfully");
                    param.Add("ip", ip);

                    objdb.AssignParamValues(param);
                    objdb.ExecuteNonQuery();
                    return new Result()
                    {
                        ErrorCode = CastAide.AsInt32(objdb.GetParameterValue("errcode")),
                        ErrorMessage = string.Format("{0}", objdb.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    ErrorCode = 100,
                    ErrorMessage = ex.Message
                };
            }
        }

        public void CustomerUpdateDisabledLiveCasino(int accid, int custid, bool disabledLiveCS, string ip, bool isEnableDownlines)
        {
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_CustomerUpdDisableLiveCS", 300))
                {
                    Hashtable param = new Hashtable();
                    param.Add("userid", accid);
                    param.Add("custid", custid);
                    param.Add("disabledlivecs", disabledLiveCS);
                    param.Add("language", "E");
                    param.Add("ip", ip);
                    param.Add("enabledownlineslivecs", isEnableDownlines);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    if (CastAide.AsInt32(db.GetParameterValue("errcode")) > 0)
                        throw new AccException(CastAide.AsString(db.GetParameterValue("errmsg")));
                }
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public void CustomerUpdateDisabledVirtualSports(int accid, int custid, bool disabled, string ip, bool isDisableDownlines)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disable", disabled);
                param.Add("language", "E");
                param.Add("ip", ip);
                param.Add("disabledownlines", isDisableDownlines);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                using (ObjectDb db = new ObjectDb("Acc_CustomerUpdDisableVS", 120))
                {
                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    if (CastAide.AsInt32(db.GetParameterValue("errcode")) > 0)
                        throw new AccException(CastAide.AsString(db.GetParameterValue("errmsg")));
                }
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}