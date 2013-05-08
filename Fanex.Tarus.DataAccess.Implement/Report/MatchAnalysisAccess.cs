
using System;
using System.Data;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Interface.Report;
using Fanex.Tarus.Entities.Report;
using Sunplus.Accounting.Core.Extensions;
using System.Collections;

namespace Fanex.Tarus.DataAccess.Implement.Report
{
    public class MatchAnalysisAccess : IMatchAnalysisAccess
    {
        private string GetStoreProcedureName(int reportType)
        {
            var spName = "Acc_Rpt_MatchAnalysisByCurrency";

            switch (reportType)
            {
                case (int)ReportType.Currency:
                    spName = "Acc_Rpt_MatchAnalysisByCurrency";
                    break;
                case (int)ReportType.Bettype:
                    spName = "Acc_Rpt_MatchAnalysisByBettype";
                    break;
                case (int)ReportType.Super:
                    spName = "Acc_Rpt_MatchAnalysisBySuper";
                    break;
                case (int)ReportType.Member:
                    spName = "Acc_Rpt_MatchAnalysisByMember";
                    break;
                case (int)ReportType.Country:
                    spName = "Acc_Rpt_MatchAnalysisByCountry";
                    break;
                case (int)ReportType.SuperViewOnMaster:
                    spName = "Acc_Rpt_MatchAnalysisBySuper_SAg";
                    break;
                case (int)ReportType.MasterViewOnAgent:
                    spName = "Acc_Rpt_MatchAnalysisBySuper_MAg";
                    break;
                case (int)ReportType.AgentViewOnMember:
                    spName = "Acc_Rpt_MatchAnalysisBySuper_Ag";
                    break;
            }
            return spName;
        }

        public DataTable SelMatchAnalysisByType(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string spName = GetStoreProcedureName(param.ReportType);

                using (ObjectDb db = new ObjectDb(spName))
                {
                    var hashParam = new Hashtable();
                    hashParam.Add("eventdate", param.EventDate);
                    hashParam.Add("matchid", param.MatchId);
                    if (param.Liveindicator != -1)
                    {
                        hashParam.Add("liveindicator", param.Liveindicator);
                    }
                    if (!string.IsNullOrEmpty(param.Bettypes))
                    {
                        hashParam.Add("bettypes", param.Bettypes);
                    }
                    hashParam.Add("custid", param.CustId);
                    hashParam.Add("statusid", param.StatusId);
                    hashParam.Add("movebetsday", param.MoveBetsDay);
                    hashParam.Add("balanceupday", param.BalanceUpDay);
                    db.AssignParamValues(hashParam);

                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                        dt = result.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dt;
        }

        public DataTable SelLeague(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_LeagueSel"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public DataTable SelMatchByLeagueids(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_MatchSel"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    db.AddInParameter("leagueids", DbType.String, param.LeagueIds);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
        
        public DataTable SelBettypeByMatchId(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_BettypeSelFromMatch"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    db.AddInParameter("matchid", DbType.Int64, param.MatchId);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                        dt = result.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
       
    }
}
