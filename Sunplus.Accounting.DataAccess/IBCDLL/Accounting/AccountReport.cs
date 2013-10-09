namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Web;

    using DNAide.Core;
    using DNAide.Data;

    using Fanex.Data;

    using IBCUtils;

    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.Report;
    using Sunplus.Accounting.Entities;
    using Sunplus.Accounting.Entities.SpecialDelete;

    public sealed class AccountReport : DataAccess
    {
        #region Constructors and Destructors

        /// <summary>
        /// private constructor
        /// </summary>
        private AccountReport()
        {
        }

        #endregion

        //Phuong.Nguyen Modified <2008-12-02>

        #region Public Methods and Operators

        public static DbResult AccBetsSelBetCheckM(string refno)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("refno", refno);


                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "BetsSelBetCheckM");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call Store BetsSelBetCheckM
        /// @author Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <param name="todate">todate</param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static DbResult Acc_BettransSelDelSpecial_MixParlay(Hashtable param) //DNAide Lib
        {
            DbResult rs = null;
            try
            {
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "BettransSelDelSpecial_MixParlay");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Result Acc_CreditTermUpdRemark(Hashtable hash)
        {
            var result = new Result();
            result.ErrorHandler = new System.Collections.Generic.Dictionary<int, string>();
            try
            {
                using (ObjectDb objdb = new ObjectDb("Acc_CreditTermUpdRemark", 500))
                {
                    objdb.ExecuteNonQuery();

                    result.ErrorCode = CastAide.AsInt32(objdb.GetParameterValue("errcode"));
                    result.ErrorMessage = string.Format("{0}", objdb.GetParameterValue("errmsg"));
                    result.ErrorHandler.Add(0, string.Format("{0}", objdb.GetParameterValue("accountremarkdate")));

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="TransID"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public static DbResult Acc_HedgeDel(long TransID, int Creator)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("transid", TransID);
                param.Add("creator", Creator);

                using (ObjectDb db = new ObjectDb("HedgeDel"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }              
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="CustId"></param>
        /// <returns></returns>
        public static DbResult Acc_HedgeSel(long TransID)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("transid", TransID); //Acc_HedgeSel

                using (ObjectDb db = new ObjectDb("HedgeSel"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransID"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public static DbResult Acc_HedgeUpd(Hashtable hash)
        {
            DbResult result = null;
            try
            {

                using (ObjectDb db = new ObjectDb("HedgeUpd"))
                {
                    db.AssignParamValues(hash);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);

                    var errcode = CastAide.AsInt32(db.GetParameterValue("errcode"));
                    var errmsg = CastAide.AsString(db.GetParameterValue("errmsg"));

                    if (errcode != 0)
                    {
                        throw new Exception(errmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_BalMovement_SAg(bool isBase)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();

                if (isBase)
                {
                    StoreProcedureElement spElement =
                        ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_BaseBalMovement_SAg"];
                    result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
                }
                else
                {
                    StoreProcedureElement spElement =
                        ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_OriginalBalMovement_SAg"];
                    result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_BetRatioByUser(double actual_stake)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("actual_stake", actual_stake);

                using (ObjectDb db = new ObjectDb("BetRatioByUser"))
                {
                    db.AssignParamValues(p);
                    var ds = db.ExecuteDataSet();
                    var hash = db.GetOuputParams();

                    result = new DbResult(hash, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_BetRatioByUser14(double actual_stake)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("actual_stake", actual_stake);

                using (ObjectDb db = new ObjectDb("BetRatioByUser_14"))
                {
                    db.AssignParamValues(p);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="fdate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_CreditTerm(Hashtable p)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("CreditTerm"))
                {
                    db.AssignParamValues(p);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_CustomerTurnoverBySite(
            DateTime fdate, DateTime tdate, string sitefilter, string fromfilter)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fdate));
                p.Add("todate", FormatHandler.ResetEndDate(tdate));
                p.Add("site", sitefilter);
                p.Add("from", fromfilter);

                using (ObjectDb db = new ObjectDb("CustomerTurnoverBySite"))
                {
                    db.AssignParamValues(p);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_DeleteBet(DateTime fdate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("fdate", fdate);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Others, "Delebet");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="tdate"></param>
        /// <param name="CustId"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_HedgeRecord(DateTime fdate, DateTime tdate, int CustId)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fdate));
                param.Add("todate", FormatHandler.ResetEndDate(tdate));
                param.Add("custid", CustId);
                param.Add("isallow", 0);

                using (ObjectDb db = new ObjectDb("HedgeRecord"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="roleid"></param>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_InstallAmount(int roleid)
        {
            //DataResult result = null;
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("roleid", roleid);

                using(ObjectDb db = new ObjectDb("InstallAmount", 300))
                {
                    db.AssignParamValues(p);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameMemberWinlossByMatch(DateTime date)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameMemberWinlossByMatch(DateTime date, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);
                    param.Add("siteid", SiteID);
                    param.Add("currency", Currency);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameMemberWinlossByMatch_details(
            DateTime date, bool islive, int bettype, int matchid)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch_details"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);
                    param.Add("islive", islive);
                    param.Add("matchid", matchid);
                    param.Add("bettype", bettype);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();
                    result = new DbResult(output, ds);
                }
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameMemberWinlossByMatch_details(
            DateTime date, bool islive, int bettype, int matchid, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch_details"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);
                    param.Add("islive", islive);
                    param.Add("matchid", matchid);
                    param.Add("bettype", bettype);
                    param.Add("siteid", SiteID);
                    param.Add("currency", Currency);
                    param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameWinlossByDate(DateTime fdate, DateTime tdate)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameWinlossByDate"))
                {
                    var param = new Hashtable();
                    param.Add("fromdate", fdate);
                    param.Add("todate", tdate);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_NumberGameWinlossByDate(DateTime fdate, DateTime tdate, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameWinlossByDate"))
                {
                    var param = new Hashtable();
                    param.Add("fromdate", fdate);
                    param.Add("todate", tdate);
                    param.Add("siteid", SiteID);
                    param.Add("currency", Currency);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();
                    result = new DbResult(output, ds);
                }                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_12BET(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_12Bet");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_12BET_Detail_Agent(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_12Bet_Details_Agent");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_12BET_Detail_Member(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday,
            int recommend)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", recommend);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                //param.Add("recommend", recommend);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_12Bet_Details_Member");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_AB88(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_AB88");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_AB88_Detail(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_AB88_Details");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_AB88_Detail_Agent(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_AB88_Details_Agent");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_AB88_Detail_Member(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                //param.Add("recommend", recommend);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_AB88_Details_Member");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_ALog(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);

                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_Winloss_ALog"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_ALog_Detail(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);

                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_WinLoss_ALog_Details"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_Fun88(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_AB88");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_Fun88_Detail(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_AB88_Details");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_M88(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);

                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_WinLoss_M88"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_M88_Detail(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);

                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_WinLoss_M88_Details"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="db"></param>
        /// <param name="fdate"></param>
        /// <param name="tdate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_WinLoss_Sporttype(string db, DateTime fdate, DateTime tdate)
        {
            DbResult result = null;
            try
            {
              
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fdate));
                param.Add("todate", FormatHandler.ResetEndDate(tdate));
                param.Add("winlost_8rm", 0);


                using (ObjectDb objectDb = new ObjectDb("WinLoss_Sporttype"))
                {
                    objectDb.AssignParamValues(param);
                    var ds = objectDb.ExecuteDataSet();
                    var output = objectDb.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_TLC(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int reporttype,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("reporttype", reporttype);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_TLC");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_TLC_Detail(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_TLC_Details");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_WinLoss_TLC_Detail_Agent(
            int custid,
            DateTime fromdate,
            DateTime todate,
            bool showsb,
            bool showhr,
            int basecurrency,
            DateTime balanceupday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                param.Add("showsb", showsb);
                param.Add("showhr", showhr);
                param.Add("basecurrency", basecurrency);
                param.Add("balanceupday", balanceupday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_TLC_Details_Agent");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_12BET_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_12Bet_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_AB88_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_AB88_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_ALog_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_Winloss_ALog_SelCurrency"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_Fun88_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_AB88_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_M88_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_Winloss_M88_SelCurrency"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_Rpt_Winloss_TLC_SelCurrency()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_TLC_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_TraderPerformanceDetail(DateTime fdate, DateTime tdate, int IsSoccer)
        {
            DbResult result = null;
            var param = new Hashtable();
            param.Add("fromdate", fdate);
            param.Add("todate", tdate);
            param.Add("issoccer", IsSoccer);

            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_TraderPerformanceDetail"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_TraderPerformanceDetail14(DateTime fdate, DateTime tdate, int IsSoccer)
        {
            DbResult result = null;
            var param = new Hashtable();
            param.Add("fromdate", fdate);
            param.Add("todate", tdate);
            param.Add("issoccer", IsSoccer);

            try
            {
                StoreProcedureElement spElement =
                    ReportSession.Instance[ReportSession.StoreCollection.Trader]["Acc_Rpt_TraderPerformanceDetail_14"];
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);

                using (ObjectDb db = new ObjectDb("Acc_Rpt_TraderPerformanceDetail_14", 600))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_TraderPerformanceSum(Hashtable hash)
        {
            DbResult result = null;
            try
            {

                using (ObjectDb db = new ObjectDb("Acc_Rpt_TraderPerformanceSum", 600))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_TraderPerformanceSum14(Hashtable hash)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_TraderPerformanceSum_14", 600))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Execute SP BetTransDel
        /// @PhuongDN
        /// </summary>
        /// <param name="transno"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DeleteResult BetTransDel(long transno, string uname, string pwd, int custid, int uid, string reason)
        {                         
            try
            {                 
                using (ObjectDb objDb = new ObjectDb("Acc_BettransDelBet"))
                {
                    var p = new Hashtable();
                    p.Add("transid", transno);
                    p.Add("username", uname);
                    p.Add("password", pwd);
                    p.Add("userid", uid);
                    p.Add("custid", custid);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    p.Add("reason", reason);

                    objDb.AssignParamValues(p);                    

                    var privateMessage = objDb.ExecuteDataSet();
                    
                    return new DeleteResult()
                    {
                        Error = new Result()
                        {
                            ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                            ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"))
                        },
                        PrivateMessage = privateMessage

                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);                
            }

            return new DeleteResult();
        }

        public static DeleteResult BetTransDelParlay(long transId, int userId, string uname, string pwd, int custId, string reason)
        {
            var error = new IbcError();
            try
            {                
                using (ObjectDb objDb = new ObjectDb("Acc_BettransDelBet_Parlay"))
                {
                    var p = new Hashtable();
                    p.Add("transid", transId);
                    p.Add("custid", custId);
                    p.Add("userid", userId);
                    p.Add("username", uname);
                    p.Add("password", pwd);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    p.Add("reason", reason);

                    objDb.AssignParamValues(p);                    
                    
                    var privateMessage = objDb.ExecuteDataSet();

                    return new DeleteResult()
                    {
                        Error = new Result()
                        {
                            ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                            ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"))
                        },
                        PrivateMessage = privateMessage

                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);                
            }

            return new DeleteResult();
        }

        /// <summary>
        /// Select bet list
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="custid"></param>
        /// <param name="listType">1: default-->from date to date; 2: betlist by day of a user; 3: bet list by ip of a user on a day</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static DataSet BetsSelBetList(DateTime fromdate, DateTime todate, int custid, int listType, string ip)
        {
            var rs = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                p.Add("todate", FormatHandler.ResetEndDate(todate));
                p.Add("custid", custid);
                p.Add("ip", ip);

                if (listType == 2)
                {
                    using (var db = new ObjectDb("BetListByDayAll", 180))
                    {
                        db.AssignParamValues(p);
                        rs = db.ExecuteDataSet();
                    }
                }
                else
                {
                    p.Add("movebetsday", ReportUtils.BalanceUp.BalanceUpDay);
                    using (var db = new ObjectDb("BetsSelBetList", 180))
                    {
                        db.AssignParamValues(p);
                        rs = db.ExecuteDataSet();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Select bet list
        /// Simon Create: 04/05/2009
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="ip"></param>
        public static DbResult GetBetList(DateTime fromdate, string ip)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                p.Add("ip", ip);

                StoreProcedureElement spElement = GetBetListStoreProcedure("BetlistByIp_Username");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Select bet list Other Sports
        /// Simon Create: 04/05/2009
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="custid"></param>
        /// <param name="ip"></param>
        public static DataSet GetBetListOtherSports(DateTime fromdate, int custid, string ip)
        {
            var rs = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                p.Add("custid", custid);
                p.Add("ip", ip);

                using (var db = new ObjectDb("ByIPOtherSports", 180))
                {
                    db.AssignParamValues(p);
                    rs = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Select bet list Soccer
        /// Simon Create: 04/05/2009
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="custid"></param>
        /// <param name="ip"></param>
        public static DataSet GetBetListSoccer(DateTime fromdate, int custid, string ip)
        {
            var rs = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                p.Add("custid", custid);
                p.Add("ip", ip);

                using (var db = new ObjectDb("BetListByIP_Soccer", 180))
                {
                    db.AssignParamValues(p);
                    rs = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static StoreProcedureElement GetBetListStoreProcedure(string alias)
        {
            StoreProcedureCollection sps = BetListSection.Instance[BetListSection.StoreCollection.BetListStores];
            return sps != null ? sps[alias] : null;
        }

        public static DbResult GetDelaReportWinLoss(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showHR,
            int reportType,
            int baseCurrency,
            DateTime balanceUpday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("showsb", showSB);
                param.Add("showhr", showHR);
                param.Add("reporttype", reportType);
                param.Add("basecurrency", baseCurrency);
                param.Add("balanceupday", balanceUpday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_Dela");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult GetDelaReportWinLossDetail(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showHR,
            int baseCurrency,
            DateTime balanceUpday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("showsb", showSB);
                param.Add("showhr", showHR);
                param.Add("basecurrency", baseCurrency);
                param.Add("balanceupday", balanceUpday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_Dela_Details");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static StoreProcedureElement GetReportStoreProcedure(
            ReportSession.StoreCollection storeCollection, string alias)
        {
            StoreProcedureCollection sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }

        public static DbResult GetSpondemoReportWinLoss(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showHR,
            int reportType,
            int baseCurrency,
            DateTime balanceUpday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("showsb", showSB);
                param.Add("showhr", showHR);
                param.Add("reporttype", reportType);
                param.Add("basecurrency", baseCurrency);
                param.Add("balanceupday", balanceUpday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Acc_Rpt_Winloss_Spondemo");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult GetSpondemoReportWinLossDetail(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showHR,
            int baseCurrency,
            DateTime balanceUpday)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("showsb", showSB);
                param.Add("showhr", showHR);
                param.Add("basecurrency", baseCurrency);
                param.Add("balanceupday", balanceUpday);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "WinLoss_Spondemo_Details");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Phuong.Nguyen: 2008-12-30
        /// Updated <2009-01-09>
        /// Get Horse Racing race result
        /// </summary>
        /// <param name="leagueid"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public static DataSet HRRaceResult(int leagueid, string raceno, string eventdate)
        {
            try
            {                
                using (ObjectDb objDB = new ObjectDb("Acc_BetList_HRResults", 180))
                {
                    var p = new Hashtable();
                    p.Add("leagueid", leagueid);
                    p.Add("raceno", raceno);
                    p.Add("eventdate", eventdate);

                    objDB.AssignParamValues(p);

                    return objDB.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        /// <summary>
        /// Simon: 2009-12-01
        /// Updated <2009-01-09>
        /// Get Horse Racing race result SCR
        /// </summary>
        /// <param name="leagueid"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public static DbResult HRRaceResultSCR(int leagueid, string raceno, string eventdate)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceno);
                p.Add("eventdate", eventdate);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Racing, "Set_HR_ResultSCRSel");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult NumberGameWinlossByDate(
            DateTime fdate, DateTime tdate, string companies, string currencies, string bettypess)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameWinlossByDate"))
                {
                    var param = new Hashtable();
                    param.Add("fromdate", fdate);
                    param.Add("todate", tdate);
                    param.Add("siteid", companies);
                    param.Add("currency", currencies);
                    param.Add("bettype", bettypess);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult NumberGameWinlossByMatch(
            DateTime date, string companies, string currencies, string bettypes)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);
                    param.Add("siteid", companies);
                    param.Add("currency", currencies);
                    param.Add("bettype", bettypes);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult NumberGameWinlossByMatchDetail(
            DateTime date, bool islive, int matchid, string companies, string currencies, int bettype)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGameMemberWinlossByMatch_details"))
                {
                    var param = new Hashtable();
                    param.Add("date", date);
                    param.Add("islive", islive);
                    param.Add("matchid", matchid);
                    param.Add("bettype", bettype);
                    param.Add("siteid", companies);
                    param.Add("currency", currencies);
                    param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// call ReportSel store procedure
        /// @Author: Hoang, Minh Anh Tu
        /// </summary>
        /// <returns>DataResult Object contains all the report's name and links</returns>
        /// Phuong.Nguyen modified
        public static DataTable ReportSel()
        {
            DbResult rs = null;
            try
            {
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "ReportSel");
                rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            if (rs != null)
            {
                return rs.ResultSet.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// call Store Rpt_AllCustomerBalance1
        /// @author Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <returns></returns>
        public static DbResult RptAllCustomerBalance(DateTime fromdate)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", fromdate); //datetime
                param.Add("totalbasebalance", 0); //int
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "AllCustomerBalance1");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call Store Rpt_AllCustomerBalanceByCurrency
        /// @author Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static DbResult RptAllCustomerBalanceByCurrency(DateTime fromdate, int currency)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", fromdate); //datetime
                param.Add("currency", currency); //int
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "AllCustomerBalanceByCurrency");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Excecute store procedure Rpt_AllStatementCompany
        /// @PhuongDN
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult RptAllStatementCompany(DateTime fromDate, DateTime toDate)
        {
            DbResult ds = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                param.Add("todate", FormatHandler.ResetEndDate(toDate));

                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Others, "AllStatementCompany");
                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// call Store Rpt_CompanyDebitCredit
        /// @Author:Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static DbResult RptCompanyDebitCredit(DateTime fromdate)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("fromdate", fromdate);
                hash.Add("totalbasedebitrunning", 0);
                hash.Add("totalbasedebitcomplete", 0);
                hash.Add("totalbasecredit", 0);
                hash.Add("totalbasecompanybalance", 0);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Others, "CompanyCreditDebit");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call Store Rpt_CustomerWinloss
        /// @author Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <param name="todate">todate</param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static DbResult RptCustomerWinloss(DateTime fromdate, DateTime todate)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fromdate)); //datetime
                param.Add("todate", FormatHandler.ResetEndDate(todate)); //datetime
                param.Add("totalbasewinlost", 0);
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "CustomerWinLoss");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call 4 store procedure to get the winlost of a super agent
        /// @Author: Hoang, Minh Anh Tu
        /// </summary>
        /// <param name="matchdate"> date want to see the win lost</param>
        /// <returns> a data results set </returns>
        public static DbResult RptDayWinLost(int roleid, int custid, DateTime matchdate)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("matchdate", matchdate);
                switch (roleid)
                {
                    case 3: // master
                        p.Add("superid", custid);
                        StoreProcedureElement spElementM = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Main, "BalanceWithDate_Master");
                        rs = DbHelper.Execute(spElementM.Name, p, spElementM.DbName, 180);
                        break;
                    case 2: // agent
                        p.Add("masterid", custid);
                        StoreProcedureElement spElementA = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Main, "BalanceWithDate_Agent");
                        rs = DbHelper.Execute(spElementA.Name, p, spElementA.DbName, 180);
                        break;
                    case 1: // member
                        p.Add("agentid", custid);
                        StoreProcedureElement spElementMe = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Main, "BalanceWithDate_Member");
                        rs = DbHelper.Execute(spElementMe.Name, p, spElementMe.DbName, 180);
                        break;
                    default: // super agent
                        StoreProcedureElement spElementS = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Main, "BalanceWithDate_Super");
                        rs = DbHelper.Execute(spElementS.Name, p, spElementS.DbName, 180);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <returns></returns>
        public static DbResult RptMABalance(
            string db, int roleid, int custid, bool isYesterday, string username, string firstletter, string currency)
        {
            DbResult result = null;
            try
            {
                //string spNameSuffix = ((isYesterday) ? "_Yesterday_1" : "_1");                
                var p = new Hashtable();

                if (!string.IsNullOrEmpty(username) && !username.Equals("All"))
                {
                    p.Add("keyword", username);
                }

                if (!string.IsNullOrEmpty(firstletter) && !firstletter.Equals("All"))
                {
                    p.Add("firstchar", firstletter);
                }

                if (!string.IsNullOrEmpty(currency))
                {
                    p.Add("currency", currency);
                }

                if (string.IsNullOrEmpty(db))
                {
                    db = "rep";
                }
                string dbType = (string.Compare(db, "rep", true) == 0) ? DBType.REPORT : DBType.MAIN;

                if (roleid == 3) //master
                {
                    p.Add("superid", custid);

                    if (isYesterday)
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "masterbalanceyesterday");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }
                    else
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "masterbalance");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }

                    //result = DbHelper.Execute("Acc_Rpt_Balance_Master" + spNameSuffix, p, dbType, 600);
                }
                else if (roleid == 2) //Agent
                {
                    p.Add("masterid", custid);
                    if (isYesterday)
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "agentbalanceyesterday");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }
                    else
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "agentbalance");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }

                    //result = DbHelper.Execute("Acc_Rpt_Balance_Agent" + spNameSuffix, p, DBType.WAREHOUSE, 600);
                }
                else if (roleid == 1) //member
                {
                    p.Add("agentid", custid);
                    if (isYesterday)
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "memberbalanceyesterday");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }
                    else
                    {
                        StoreProcedureElement spElement = GetReportStoreProcedure(
                            ReportSession.StoreCollection.Balance, "memberbalance");
                        result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
                    }

                    //result = DbHelper.Execute("Acc_Rpt_Balance_Member" + spNameSuffix, p, DBType.WAREHOUSE, 600);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// CITS
        /// </summary>
        /// <returns></returns>
        public static DbResult RptSABalance(string db, bool isYesterday, Hashtable p)
        {
            DbResult result = null;
            try
            {
                if (isYesterday)
                {
                    StoreProcedureElement spElement = GetReportStoreProcedure(
                        ReportSession.StoreCollection.Balance, "superbalanceyesterday");
                    result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
                    
                }
                else
                {
                    StoreProcedureElement spElement = GetReportStoreProcedure(
                        ReportSession.StoreCollection.Balance, "superbalance");
                    result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
                }
            }
            catch (Exception ex)
            {
                    LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }


        //public static string RptSABalance_AddTotal(string db, bool isYesterday, Hashtable p)
        //{
        //    string resultstr = "";
        //    DbResult result = null;
        //    try
        //    {
        //        if (isYesterday)
        //        {
        //            StoreProcedureElement spElement = GetReportStoreProcedure(
        //                ReportSession.StoreCollection.Balance, "superbalanceyesterday");
        //            result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
        //            resultstr = result.Outputs["Total"].ToString();
                    
        //        }
        //        else
        //        {
        //            StoreProcedureElement spElement = GetReportStoreProcedure(
        //                ReportSession.StoreCollection.Balance, "superbalance");
        //            result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
        //            resultstr = result.Outputs["Total"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogExtensions.ExceptionHandler(ex);
        //    }
        //    return resultstr;
        //}

        /// <summary>
        /// CITS
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="roleid"></param>
        /// <param name="custid"></param>
        /// <returns></returns>

        public static DbResult Acc_Rpt_TreeWinlost(DateTime fdate, int roleid, int custid)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", fdate);
                p.Add("roleid", roleid);
                p.Add("custid", custid);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);
                p.Add("virtualwinlost_8rm", 0);
                p.Add("virtualwinlost_barm", 0);
                //if (roleid == 4)
                //{
                //    p.Add("winlost_12betagent", 0);
                //    p.Add("virtualwinlost_12betagent", 0);
                //}
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Others, "TreeWinLoss");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
               
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// call Store Rpt_SelParlayBets
        /// @author Vu Thi Hong Hanh
        /// </summary>
        /// <param name="fromdate">fromdate</param>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static DbResult RptSelParlayBets(DateTime fromdate, DateTime todate)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                param.Add("todate", FormatHandler.ResetEndDate(todate));
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Main, "SelParlayBets");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult SelectCurrenciesOfDelaForWinLossReport()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_Dela_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult SelectCurrenciesOfSpondemoForWinLossReport()
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                StoreProcedureElement spElement = GetReportStoreProcedure(
                    ReportSession.StoreCollection.Trader, "Winloss_Spondemo_SelCurrency");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #endregion

        #region Methods

        private static StoreProcedureElement GetCreditTermStoreProcedure(
            CreditTermSession.StoreCollection storeCollection, string aliasName)
        {
            StoreProcedureCollection sps = CreditTermSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        #endregion
    }
}
