using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class Transfer : DataAccess
    {
        private static StoreProcedureElement GetCustomerStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;
        }

        /// <summary>
        /// call SP BettransInsTransferAdmin
        /// @author:Vu Hanh
        /// </summary>
        /// <param name="fromCustId">fromCustId</param>
        /// <param name="toUserName">toUserName</param>
        /// <param name="transDate">transDate</param>
        /// <param name="amount">amount</param>
        /// <param name="creator">creator</param>
        /// <param name="lang">lang</param>
        /// <returns></returns>
        public static Boolean BettransInsTransferAdmin(int fromCustId, string toUserName, DateTime transDate,
                                                       double amount, int creator, string lang)
        {
            Boolean isUpdated = false;
            try
            {
                if (lang == null)
                {
                    throw new ArgumentNullException(lang);
                }
                var hash = new Hashtable();
                hash.Add("fromcustid", fromCustId);
                hash.Add("tousername", toUserName);
                hash.Add("transdate", transDate);
                hash.Add("amount", amount);
                hash.Add("creator", creator);
                if (lang.Equals("zh-CN"))
                {
                    hash.Add("language", "CS");
                }
                else if (lang.Equals("ja-JP"))
                {
                    hash.Add("language", "J");
                }
                else
                {
                    hash.Add("language", "E");
                }

                DbHelper.ExecuteCmd("BettransInsTransfer_Admin", hash, DBType.MAIN);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return isUpdated;
        }

        /// <summary>
        /// call SP CustomerSelMultipleTransfer
        /// @author:Vu Hanh
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="custid">custId</param>
        /// <param name="closed">closed</param>
        /// <returns></returns>
        public static DbResult CustomerSelMultipleTransfer(int custId, int closed)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                hash.Add("closed", closed);
                
                using (ObjectDb db = new ObjectDb("CustomerSelMultipleTransfer"))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    rs = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Insert Bettrans Transfer
        /// store procedure: Acc_BettransInsTransfer
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="_accId"></param>
        /// <param name="_fromCustId"></param>
        /// <param name="_toUser"></param>
        /// <param name="_exRate"></param>
        /// <param name="_amount"></param>
        /// <param name="_remark"></param>
        /// <param name="_transDate"></param>
        /// <param name="_ip"></param>
        /// <returns></returns>
        public static Result BettransInsertTransfer(int _accId, int _fromCustId, string _toUser, double _exRate,
                                                      double _amount, string _remark, string _ip)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_BettransInsTransfer"))
                {
                    var p = new Hashtable();
                    p.Add("userid", _accId); // int
                    p.Add("fromcustid", _fromCustId); // int
                    p.Add("tousername", _toUser); // nvarchar
                    p.Add("exchangerate", _exRate); // smallmoney
                    p.Add("amount", _amount); // money
                    p.Add("remark", _remark); // nvarchar
                    // p.Add("transdate", _transDate);     // date
                    p.Add("language", "E"); // varchar
                    //p.Add("yesterdaydata", _isYesterday); // bit
                    p.Add("errcode", 0); // int
                    p.Add("errmsg", string.Empty); // varchar
                    p.Add("ip", _ip); // varchar

                    objDb.AssignParamValues(p);
                    objDb.ExecuteNonQuery();

                    var errMsg = DNAide.Core.CastAide.AsString(objDb.GetParameterValue("errmsg"));
                    var errCode = DNAide.Core.CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    return new Result { ErrorCode = errCode, ErrorMessage = errMsg };
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

        /// <summary>
        /// Confirm Insert Bettrans Transfer
        /// store procedure: Acc_BettransInsTransfer_Confirm
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="_fromUserName"></param>
        /// <param name="_toUser"></param>
        /// <param name="_exRate"></param>
        /// <returns></returns>
        public static DbResult BettransInsertTransfer_Confirm(string _fromUserName, string _toUser, double _exRate)
        {
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromusername", _fromUserName);
                p.Add("tousername", _toUser);
                p.Add("exchangerate", _exRate);
                p.Add("fromcurrency", string.Empty);
                p.Add("tocurrency", string.Empty);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);
                p.Add("language", "E");

                dr = DbHelper.Execute("Acc_BettransInsTransfer_Confirm", p, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="custID"></param>
        /// <param name="agentName"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Result TransferAllMember(int custID, string agentName, string ip)
        {
            using (ObjectDb db = new ObjectDb("Acc_BettransInsTransferAllMembers", 600))
            {
                var p = new Hashtable();
                p.Add("agentname", agentName);
                p.Add("userid", custID);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);
                p.Add("ip", ip);

                db.AssignParamValues(p);
                db.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(db.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", db.GetParameterValue("errmsg"))
                };
            }
        }

        public static Result TransferAllDownlines(int custID, string agentName, string ip)
        {
            using (ObjectDb db = new ObjectDb("Acc_ClearBalanceAllDownlines", 600))
            {
                var p = new Hashtable();
                p.Add("agentname", agentName);
                p.Add("userid", custID);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);
                p.Add("ip", ip);

                db.AssignParamValues(p);
                db.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(db.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", db.GetParameterValue("errmsg"))
                };
            }
        }
    }
}