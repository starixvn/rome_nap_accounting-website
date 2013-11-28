using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fanex.Data;
using Fanex.Tarus.LiveCasino.Business;
using Fanex.Tarus.LiveCasino.Entities.Report;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Data;
using System.Collections;
using Sunplus.Accounting.Core.Extensions;


namespace Fanex.Tarus.LiveCasino.DataAccess.Report
{
    public class ReportDataAccess : BaseDataAccess, IReportDataAccess
    {

        public ReportDataAccess()
        {
            var balanceDate = GetBalanceUpDate();

            BalanceUpDate = balanceDate["BalanceUpDay"];
            MovebetDate = balanceDate["MoveBetsDay"];

        }

        public List<DailyWinlossDetail> GetDailyWinlossDetail()
        {
            using (ObjectDb objDB = new ObjectDb("LiveCasinoDailyWinloss"))
            {
                var listDailyWinlossDetail = new List<DailyWinlossDetail>();
                //objDB.AddInParameter("matchid", DbType.Int32, MatchId);
                var ds = objDB.ExecuteDataSet();

                //Convert DataSet to List<DailyWinlossDetail>

                return listDailyWinlossDetail;
            }
        }

        public GameStatisticsBO GetGameStatistic(GameStatisticsParams dbparams)
        {
            var result = new GameStatisticsBO();
            result.Items = new List<GameStatisticsDTO>();
            result.GrandTotal = new List<GameStatisticsDTO>();

            try
            {
                using (ObjectDb objDB = new ObjectDb("GameStatistics_DW", 1200))
                {

                    var list = new List<GameStatisticsDTO>();
                    objDB.AddInParameter("fromdate", DbType.String, dbparams.fromdate);
                    objDB.AddInParameter("todate", DbType.String, dbparams.todate);
                    objDB.AddInParameter("MoveBetDay", DbType.String, dbparams.movebetsday);
                    //objDB.AddInParameter("BalanceUpDay", DbType.String, dbparams.balanceupday);
                    objDB.AddInParameter("username", DbType.String, dbparams.username);
                    objDB.AddInParameter("excludeTest", DbType.Boolean, dbparams.excludetest);
                    objDB.AddInParameter("SiteID", DbType.Int32, dbparams.site);
                    objDB.AddInParameter("GameTypeID", DbType.Int32, dbparams.gametype);
                    objDB.AddInParameter("GameTableID", DbType.Int32, dbparams.gametable);
                    objDB.AddInParameter("currency", DbType.Int32, dbparams.currency);
                    objDB.AddInParameter("ReportBy", DbType.Int32, dbparams.reporttype);
                    var ds = objDB.ExecuteDataSet();
                    //CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));

                    if (ds.Tables.Count > 0)
                    {
                        #region items
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var item = new GameStatisticsDTO()
                            {
                                ReportType = GetBettypeName(dbparams.reporttype, row),
                                NoPlayers = dbparams.reporttype != (int)FilterGameCondition.Username ? NumberFormat.DecFormat(row["NoOfPlayers"], 0) : "0",
                                NoGames = NumberFormat.DecFormat(row["NoOfGames"], 0),
                                CurrencyName = CastAide.AsString(row["Currency"]),
                                TotalBets = NumberFormat.DecFormat(row["TotalBets"], 2),
                                TotalBetsRM = NumberFormat.DecFormat(row["TotalBets_RM"], 2),
                                TotalWins = NumberFormat.DecFormat(row["TotalWins"], 2),
                                TotalWinsRM = NumberFormat.DecFormat(row["TotalWins_RM"], 2),
                                GameIncome = NumberFormat.DecFormat(row["GameIncome"], 2),
                                GameIncomeRM = NumberFormat.DecFormat(row["GameIncome_RM"], 2),
                                GamePayout = NumberFormat.DecFormat(row["GamePayout"], 2) + "%",
                                GamePayoutRM = NumberFormat.DecFormat(row["GamePayout_RM"], 2) + "%",
                                TotalRefund = NumberFormat.DecFormat(row["TotalRefund"], 2),
                                TotalRefundRM = NumberFormat.DecFormat(row["TotalRefund_RM"], 2),
                                Company = NumberFormat.DecFormat(row["CompanyNetIncome"], 2),
                                CompanyRM = NumberFormat.DecFormat(row["CompanyNetIncome_RM"], 2),
                                CompanyTO = NumberFormat.DecFormat(row["CompanyTurnOver"], 2),
                                CompanyTORM = NumberFormat.DecFormat(row["CompanyTurnOver_RM"], 2),
                                CompanyComm = NumberFormat.DecFormat(row["CompanyComm"], 2),
                                CompanyCommRM = NumberFormat.DecFormat(row["CompanyComm_RM"], 2)
                            };

                            list.Add(item);
                        }
                        result.Items = list;

                        #endregion

                        #region total and grand total
                        var dtGrandTotal = ds.Tables[1];
                        var totalRows = dtGrandTotal.Rows.Count;
                        if (totalRows > 0)
                        {
                            #region total - each currency

                            result.TotalItems = new List<GameStatisticsDTO>();

                            for (int i = 0; i < totalRows - 1; i++)
                            {
                                result.TotalItems.Add(new GameStatisticsDTO()
                                {
                                    ReportType =
                                        string.Format("Total {0}",
                                                      dtGrandTotal.Rows[i]["Currency"]),
                                    NoPlayers =
                                        dbparams.reporttype != (int)FilterGameCondition.Username
                                            ? NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[i]["NoOfPlayers"], 0)
                                            : "0",
                                    NoGames =
                                        NumberFormat.DecFormat(dtGrandTotal.Rows[i]["NoOfGames"],
                                                               0),
                                    CurrencyName =
                                        CastAide.AsString(dtGrandTotal.Rows[i]["Currency"]),
                                    TotalBets =
                                        NumberFormat.DecFormat(dtGrandTotal.Rows[i]["TotalBets"],
                                                               2),
                                    TotalWins =
                                        NumberFormat.DecFormat(dtGrandTotal.Rows[i]["TotalWins"],
                                                               2),
                                    GameIncome =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[i]["GameIncome"], 2),
                                    GamePayout =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[i]["GamePayout"], 2) + "%",
                                    TotalRefund =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[i]["TotalRefund"], 2),
                                    Company =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[i]["CompanyNetIncome"], 2),
                                    CompanyTO =
                                            NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[i]["CompanyTurnOver"], 2),
                                    CompanyComm = NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[i]["CompanyComm"], 2)
                                });
                            }

                            #endregion

                            #region grand total

                            #region differency RM

                            if (dbparams.currency != -1 && dbparams.currency != 0 && totalRows >= 2)
                            {
                                var currencyName = CacheUtils.GetExchangeName(dbparams.currency);

                                result.GrandTotal.Add(new GameStatisticsDTO
                                {
                                    ReportType = "Grand Total " + currencyName,
                                    NoPlayers =
                                        dbparams.reporttype != (int)FilterGameCondition.Username
                                            ? NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[totalRows - 2]["NoOfPlayers"], 0)
                                            : "0",
                                    NoGames =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["NoOfGames"], 0),
                                    CurrencyName =
                                        CastAide.AsString(
                                            dtGrandTotal.Rows[totalRows - 2]["Currency"]),
                                    TotalBets =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["TotalBets"], 2),
                                    TotalWins =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["TotalWins"], 2),
                                    GameIncome =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["GameIncome"], 2),
                                    GamePayout =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["GamePayout"], 2) +
                                        "%",
                                    TotalRefund =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["TotalRefund"], 2),
                                    Company =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 2]["CompanyNetIncome"],
                                            2),
                                    CompanyTO =
                                            NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[totalRows - 2]["CompanyTurnOver"], 2),
                                    CompanyComm = NumberFormat.DecFormat(
                                                dtGrandTotal.Rows[totalRows - 2]["CompanyComm"], 2)
                                });
                            }

                            #endregion

                            #region base RM

                            result.GrandTotal.Add(new GameStatisticsDTO()
                            {
                                ReportType = "Grand Total RM",
                                NoPlayers =
                                    dbparams.reporttype != (int)FilterGameCondition.Username
                                        ? NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 1]["NoOfPlayers"], 0)
                                        : "0",
                                NoGames =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["NoOfGames"], 0),
                                CurrencyName = "RM",
                                TotalBets =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["TotalBets"], 2),
                                TotalWins =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["TotalWins"], 2),
                                GameIncome =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["GameIncome"], 2),
                                GamePayout =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["GamePayout"], 2) + "%",
                                TotalRefund =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["TotalRefund"], 2),
                                Company =
                                    NumberFormat.DecFormat(
                                        dtGrandTotal.Rows[totalRows - 1]["CompanyNetIncome"], 2),
                                CompanyTO =
                                        NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 1]["CompanyTurnOver"], 2),
                                CompanyComm = NumberFormat.DecFormat(
                                            dtGrandTotal.Rows[totalRows - 1]["CompanyComm"], 2)
                            });

                            #endregion

                            #endregion
                        }

                        #endregion
                    }
                    
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
        
        public DataTable GetDailyWinlossLC(string winlossdate)
        {
            var datatable = new DataTable();
            string spalias = string.Empty;
            var arrdate = new Dictionary<string, DateTime>();
            arrdate = GetBalanceUpDate();
            if (CastAide.AsDateTime(winlossdate) > MovebetDate)
                spalias = "DailyWLDirect_LiveCasino";
            else if (CastAide.AsDateTime(winlossdate) >= BalanceUpDate)
                spalias = "DailyWLDirect_LiveCasino_14";
            else
                spalias = "DailyWLDirect_LiveCasino_WH";
            
            using (ObjectDb objDB = new ObjectDb(spalias))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, CastAide.AsDateTime(winlossdate));
                var listDailyWL = new List<DailyWinloss>();
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
           
        }

        protected Dictionary<string, DateTime> GetBalanceUpDate()
        {
            var arrdate = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("Core_ReportService_GetBalanceUpDay"))
            {
                var ds = objDB.ExecuteDataSet();
                DataRow rs = ds.Tables[0].Rows[0];
                arrdate.Add("BalanceUpDay", CastAide.AsDateTime(rs["balanceupday"]));
                arrdate.Add("MoveBetsDay", CastAide.AsDateTime(rs["movebetsday"]));

            }
            return arrdate;
        }
        
        protected DateTime BalanceUpDate
        {
            get;
            set;
        }

        protected DateTime MovebetDate { get; set; }
        
        public DataTable GetBetlistResultLC(string matchid)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_BetList_SelMatchResult"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, CastAide.AsInt32(matchid));
                var listDailyWL = new List<DailyWinloss>();
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }

        private string GetBettypeName(int reportType, DataRow row)
        {
            var result = string.Empty;
            switch (reportType)
            {
                case (int)FilterGameCondition.Daily:
                    result = CastAide.AsDateTime(row["Date"]).ToShortDateString();
                    break;
                case (int)FilterGameCondition.Monthly:
                    result = string.Format("{0}-{1}", row["Year"], row["Month"]);
                    break;
                case (int)FilterGameCondition.Username:
                    result = CastAide.AsString(row["Username"]);
                    break;
                case (int)FilterGameCondition.GameType:
                    result = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(row["GameTypeID"]), "E");
                    break;
                case (int)FilterGameCondition.GameTable:
                    result = RemotingRef.RefData.GetLeagueName(CastAide.AsString(row["GameTableID"]), "E");
                    break;
                case (int)FilterGameCondition.Company:
                    result = CastAide.AsString(row["Company"]);
                    break;
                case (int)FilterGameCondition.Currency:
                    result = CastAide.AsString(row["Currency"]);
                    break;
                default:
                    result = CastAide.AsString(row["Currency"]);
                    break;
            }
            return result;
        }

        public DataTable GetCompanyAllowPlayLiveCasino()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_LicenseeSel"))
            {
                var ds = objDB.ExecuteDataSet();
                if(ds!=null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetCurrency()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetGameTableByGameType(int gameType)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_GameTableSel"))
            {
                objDB.AddInParameter("GameTypeID", DbType.Int32, gameType);
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetGameType()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_GameTypeSel"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }
        
    }
}
