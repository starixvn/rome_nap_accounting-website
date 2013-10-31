using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Fanex.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccountStatement
    {
        private AccountStatement()
        {
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 02/08/2006
        /// call store procedure Rpt_AllStatement
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="fromdate">from date</param>
        /// <param name="todate">to date</param>
        /// <returns>dataresult</returns>
        public static DbResult Acc_AllStatement(DateTime fromdate, DateTime todate, int custid, string username,
                                                int allStatementNumber)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("username", username);
            p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
            p.Add("todate", FormatHandler.ResetEndDate(todate));

            string spKey = "";
            try
            {
                if (allStatementNumber == 1)
                {
                    spKey = "AllStatement";
                }
                else if (allStatementNumber == 2)
                {
                    p.Add("showuser", 0);
                    spKey = "Acc_AllStatement_FULL2";
                }
                else if (allStatementNumber == 3)
                {
                    spKey = "Acc_Statement";
                }
                else if (allStatementNumber == 4)
                {
                    spKey = "Acc_AllStatement4Agent";
                }
                else if (allStatementNumber == 5)
                {
                    spKey = "Acc_AllStatementCompany";
                }

                using (ObjectDb db = new ObjectDb(spKey, 600))
                {
                    db.AssignParamValues(p);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-21
        public static DbResult Acc_MemberStatement(DateTime fromdate, DateTime todate, int custid, DateTime BalanceUpDay,
                                                   bool IncludeAccBal)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            p.Add("balanceupday", BalanceUpDay);
            p.Add("includeaccbal", IncludeAccBal);

            try
            {
                using (ObjectDb db = new ObjectDb("AllStatement_Full", 600))
                {
                    db.AssignParamValues(p);
                    
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-24
        public static DbResult Acc_DetailStatement(DateTime fromdate, DateTime todate, int custid)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            try
            {
                result = DbHelper.Execute("Acc_Statement", p, DBType.WAREHOUSE, 300);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-24
        public static DbResult Acc_AuditStatement(int custid)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            try
            {
                result = DbHelper.Execute("Acc_AllStatement4Agent", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_CompanyStatement(DateTime fromdate, DateTime todate)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            try
            {
                result = DbHelper.Execute("Acc_AllStatementCompany", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="transtype"></param>
        /// <returns></returns>
        public static DbResult Acc_AllStatementCompanyDetail(DateTime fdate, string transtype)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("fromdate", fdate);
            p.Add("transtype", transtype);
            try
            {
                result = DbHelper.Execute("Acc_AllStatementCompanyDetail", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Delete transaction(statement)
        /// </summary>
        /// Phuong.Nguyen updated
        /// <param name="refno"></param>
        /// <param name="custid"></param>
        /// <param name="userid"></param>
        public static IbcError Acc_BettransDelTransaction(long refno, int custid, int userid, string IP)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_BettransDelTransaction", 1200))
                {
                    var p = new Hashtable();
                    p.Add("refno", refno);
                    p.Add("custid", custid);
                    p.Add("userid", userid);
                    p.Add("ip", IP);
                    p.Add("errcode", 0);
                    p.Add("errmsg", "");

                    db.AssignParamValues(p);
                    db.ExecuteNonQuery();

                    return new IbcError()
                    {
                        ErrorCode = DNAide.Core.CastAide.AsInt32(db.GetParameterValue("errcode")),
                        ErrorMsg = string.Format("{0}", db.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new IbcError()
                {
                    ErrorCode = 100,
                    ErrorMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 08/12/2006
        /// call store procedure Rpt_AgentStatement
        /// this function support for AgentStatement.aspx
        /// </summary>
        /// Phuong.Nguyen updated
        /// <param name="custid">custid</param>
        /// <returns>dataresult</returns>
        public static DbResult RptAgentStatement(int custid)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custid);

                using (ObjectDb db = new ObjectDb("AgentStatement", 600))
                {
                    db.AssignParamValues(p);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 02/08/2006
        /// call store procedure Rpt_BetListByDayAll
        /// </summary>
        /// Phuong.Nguyen updated <2008-12-03>
        /// <param name="fromdate">fromdate </param>
        /// <param name="custid"> custid</param>
        /// <returns> dataresult</returns>
        public static DataSet RptBetListByDayAll(DateTime fromdate, int custid)
        {
            var result = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);

                using (ObjectDb db = new ObjectDb("BetListByDayAll",600))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private class BetListStoreProcedure : DataAccess
        {
            public Sunplus.Accounting.Core.Configurations.StoreProcedureElement GetBetListStoreProcedure(BetListSection.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }

        private class TransactionStoreProcedure : DataAccess
        {
            public Sunplus.Accounting.Core.Configurations.StoreProcedureElement GetTransactionStoreProcedure(TransactionGroupSection.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}