using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using DNAide.Utils;
using Fanex.Tarus.LiveCasino.DataAccess.Report;
using Fanex.Tarus.LiveCasino.Entities.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;

using Fanex.Tarus.LiveCasino.Entities.Betlist;

namespace Fanex.Tarus.LiveCasino.Business.Report
{
    public class ReportManager : BaseReport, IRequiresSessionState, IReportManager 
    {

        protected readonly IReportDataAccess ReportAccess;
        public string _keyDailyWLDirectLC = AppSession.Username + ".SelDailyWLDirectLC";
        private const string GameStatisticSessionKey = "GameStatistic";
        private const string GameStatisticMappedSessionKey = "GameStatisticMapped";

        public ReportManager(): this(null)
        {
            
        }

        public ReportManager(IReportDataAccess reportDataAccess)
        {
            ReportAccess = reportDataAccess ?? new ReportDataAccess();
        }

        public List<DailyWinlossDetail> GetDailyWinlossDetail()
        {
            try
            {
                return ReportAccess.GetDailyWinlossDetail();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new List<DailyWinlossDetail>();
        }

        #region GameStatistic
        public GameStatisticsBO GetGameStatisticsData(GameStatisticsParams dbparams)
        {
            try
            {
                var gameStatistic = new GameStatisticsBO();
                GameStatisticsBO data;
                var items = new GameStatisticsBO();
                var isSumTotal = true;
                #region get data
                data = ReportAccess.GetGameStatistic(dbparams);
                #endregion

                //Get Data Paging
                #region compare with total bet and total win
                List<GameStatisticsDTO> itemFilter = null;
                #region betless && betmore
                if ((!string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore))
                    && (string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess) &&
                                      CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore)
                                  select q).ToList();
                }
                #endregion
                #region winless=  && winmore
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                    string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                      CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore)
                                  select q).ToList();
                }
                #endregion
                #region winless
                else if (!string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore) &&
                    string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess)
                                  select q).ToList();
                }
                #endregion
                #region winmore
                else if (string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore) &&
                    string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore)
                                  select q).ToList();
                }
                #endregion
                #region betless
                else if (string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore) &&
                    !string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess)
                                  select q).ToList();
                }
                #endregion
                #region betmore
                else if (string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore) &&
                    string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore)
                                  select q).ToList();
                }
                #endregion
                #region betless && winless
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore)) &&
                    (!string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                      CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess)
                                  select q).ToList();
                }
                #endregion
                #region betless && winmore
                else if ((string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                    (!string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore) &&
                                       CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess)
                                  select q).ToList();
                }
                #endregion
                #region betmore && winless
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore)) &&
                    (string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                       CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore)
                                  select q).ToList();
                }
                #endregion
                #region betmore && winmore
                else if ((string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                    (string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore) &&
                                       CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore)
                                  select q).ToList();
                }
                #endregion

                #region betless && winless  && winmore
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                (!string.IsNullOrEmpty(dbparams.betLess) && string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      (CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                       CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore)) &&
                                      (CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess))
                                  select q).ToList();
                }
                #endregion
                #region betmore  && winless && winmore
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                    (!string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      (CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                       CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore)) &&
                                       CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess)
                                  select q).ToList();
                }
                #endregion
                #region betless && betmore && winless
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && string.IsNullOrEmpty(dbparams.winMore)) &&
                (!string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                      (CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess) &&
                                       CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore))
                                  select q).ToList();
                }
                #endregion
                #region betless && betmore && winmore
                else if ((string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                (!string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                       CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore) &&
                                      (CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess) &&
                                       CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore))
                                  select q).ToList();
                }
                #endregion
                else if ((!string.IsNullOrEmpty(dbparams.winLess) && !string.IsNullOrEmpty(dbparams.winMore)) &&
                    (!string.IsNullOrEmpty(dbparams.betLess) && !string.IsNullOrEmpty(dbparams.betMore)))
                {
                    itemFilter = (from q in data.Items
                                  where
                                      (CastAide.AsDouble(q.TotalWins) <= CastAide.AsDouble(dbparams.winLess) &&
                                       CastAide.AsDouble(q.TotalWins) >= CastAide.AsDouble(dbparams.winMore)) &&
                                      (CastAide.AsDouble(q.TotalBets) <= CastAide.AsDouble(dbparams.betLess) &&
                                       CastAide.AsDouble(q.TotalBets) >= CastAide.AsDouble(dbparams.betMore))
                                  select q).ToList();
                }
                else
                {
                    isSumTotal = false;
                    itemFilter = (from q in data.Items select q).ToList();
                }

                #endregion
                gameStatistic.total = data.Items.Count;
                gameStatistic.Items = new List<GameStatisticsDTO>();

                #region add items to session
                items.Items = itemFilter.Select(c => c).ToList();
                items.GrandTotal = data.GrandTotal;
                SessionExtension.AddSession(HttpContext.Current.Session, GameStatisticSessionKey, items);
                var currencyList = (from q in items.Items select q.CurrencyName).Distinct().ToList();
                #endregion

                if (gameStatistic.total > 0)
                {
                    switch (dbparams.reporttype)
                    {
                        case (int)FilterGameCondition.Daily:
                        case (int)FilterGameCondition.Monthly:
                        case (int)FilterGameCondition.GameType:
                        case (int)FilterGameCondition.GameTable:
                        case (int)FilterGameCondition.Company:
                            gameStatistic.Items = GetGameStatisticWithGroup(items.Items);
                            break;
                        case (int)FilterGameCondition.Username:
                        case (int)FilterGameCondition.Currency:
                            gameStatistic.Items = items.Items;
                            break;
                    }
                }

                #region Total
                if (isSumTotal)
                {
                    gameStatistic.TotalItems = new List<GameStatisticsDTO>();
                    gameStatistic.GrandTotal = new List<GameStatisticsDTO>();
                    if (items.Items.Count > 0)
                    {
                        foreach (var currency in currencyList)
                        {
                            var totalItems = (from q in items.Items where q.CurrencyName == currency select q).ToList();

                            gameStatistic.TotalItems.Add(new GameStatisticsDTO()
                            {
                                CurrencyName = currency,
                                ReportType = string.Format("Total {0}", currency),
                                Company = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.Company)).Sum(), 2),
                                GameIncome = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.GameIncome)).Sum(), 2),
                                GamePayout = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.GamePayout.Replace("%", ""))).Sum(), 2) + "%",
                                NoGames = NumberFormat.DecFormat((from c in totalItems select CastAide.AsInt32(c.NoGames)).Sum(), 0),
                                NoPlayers = NumberFormat.DecFormat((from c in totalItems select CastAide.AsInt32(c.NoPlayers)).Sum(), 0),
                                TotalBets = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.TotalBets)).Sum(), 2),
                                TotalRefund = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.TotalRefund)).Sum(), 2),
                                TotalWins = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.TotalWins)).Sum(), 2),
                                CompanyTO = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.CompanyTO)).Sum(), 2),
                                CompanyComm = NumberFormat.DecFormat((from c in totalItems select CastAide.AsDouble(c.CompanyComm)).Sum(), 2)
                            });
                        }

                        if (dbparams.currency != 0 && dbparams.currency != -1)
                        {
                            var currencyName = CacheUtils.GetExchangeName(dbparams.currency);
                            gameStatistic.GrandTotal.Add(new GameStatisticsDTO()
                            {
                                CurrencyName = currencyName,
                                ReportType = "Grand Total " + currencyName,
                                Company = gameStatistic.TotalItems[0].Company,
                                GameIncome = gameStatistic.TotalItems[0].GameIncome,
                                GamePayout = gameStatistic.TotalItems[0].GamePayout,
                                NoGames = gameStatistic.TotalItems[0].NoGames,
                                NoPlayers = gameStatistic.TotalItems[0].NoPlayers,
                                TotalBets = gameStatistic.TotalItems[0].TotalBets,
                                TotalRefund = gameStatistic.TotalItems[0].TotalRefund,
                                TotalWins = gameStatistic.TotalItems[0].TotalWins,
                                CompanyTO = gameStatistic.TotalItems[0].CompanyTO,
                                CompanyComm = gameStatistic.TotalItems[0].CompanyComm
                            });
                        }

                        gameStatistic.GrandTotal.Add(new GameStatisticsDTO()
                        {
                            CurrencyName = "RM",
                            ReportType = "Grand Total RM",
                            Company = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.CompanyRM)).Sum(), 2),
                            GameIncome = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.GameIncomeRM)).Sum(), 2),
                            GamePayout = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.GamePayoutRM.Replace("%", ""))).Sum(), 2) + "%",
                            NoGames = NumberFormat.DecFormat((from c in items.Items select CastAide.AsInt32(c.NoGames)).Sum(), 0),
                            NoPlayers = NumberFormat.DecFormat((from c in items.Items select CastAide.AsInt32(c.NoPlayers)).Sum(), 0),
                            TotalBets = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.TotalBetsRM)).Sum(), 2),
                            TotalRefund = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.TotalRefundRM)).Sum(), 2),
                            TotalWins = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.TotalWinsRM)).Sum(), 2),
                            CompanyComm = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.CompanyCommRM)).Sum(), 2),
                            CompanyTO = NumberFormat.DecFormat((from c in items.Items select CastAide.AsDouble(c.CompanyTORM)).Sum(), 2)
                        });
                    }

                }
                else
                {
                    gameStatistic.TotalItems = data.TotalItems;
                    gameStatistic.GrandTotal = data.GrandTotal;
                }
                gameStatistic.total = gameStatistic.Items.Count;
                #endregion

                return gameStatistic;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("timeout expired."))
                {
                    return new GameStatisticsBO()
                    {
                        total = -100
                    };
                }

                LogExtensions.ExceptionHandler(ex);
            }
            return new GameStatisticsBO();
        }

        public GameStatisticsBO GetGameStatistic(GameStatisticsParams dbparams)
        {
            try
            {
                GameStatisticsBO getData = null;
                if (dbparams.fromcache)
                {
                    var getDataFromcache = SessionExtension.GetSession(HttpContext.Current.Session, GameStatisticMappedSessionKey);
                    if (getDataFromcache != null)
                    {
                        getData = (GameStatisticsBO)getDataFromcache;
                    }
                    else
                    {
                        getData = GetGameStatisticsData(dbparams);
                        SessionExtension.AddSession(HttpContext.Current.Session, GameStatisticMappedSessionKey, getData);
                    }
                }
                else
                {
                    getData = GetGameStatisticsData(dbparams);
                    SessionExtension.AddSession(HttpContext.Current.Session, GameStatisticMappedSessionKey, getData);
                }


                var gameStatistic = new GameStatisticsBO();
                if (getData != null && getData.Items != null && getData.Items.Count > 0)
                {
                    List<GameStatisticsDTO> items = getData.Items;

                    if (dbparams.sort.SortType.Length > 0)
                    {

                        if (dbparams.reporttype == (int)FilterGameCondition.Username ||
                            dbparams.reporttype == (int)FilterGameCondition.Currency ||
                            dbparams.sort.SortCol.ToLower().Equals("reporttype"))
                        {
                            SortList(items, dbparams.sort);
                        }
                        else
                        {
                            #region sort all items - after add to group
                            var getDataNotMapped = (GameStatisticsBO)SessionExtension.GetSession(HttpContext.Current.Session,
                                                                               GameStatisticSessionKey);
                            if (getDataNotMapped != null)
                            {
                                List<GameStatisticsDTO> sortingItems = getDataNotMapped.Items;
                                SortList(sortingItems, dbparams.sort);
                                items = GetGameStatisticWithGroup(sortingItems);
                            }
                            #endregion
                        }
                    }

                    var pagingItems = items.Select(c => c).Skip(dbparams.pagesize * (dbparams.pageindex - 1)).Take(
                            dbparams.pagesize).ToList();
                    gameStatistic.Items = pagingItems;
                    gameStatistic.GrandTotal = getData.GrandTotal;
                    gameStatistic.TotalItems = getData.TotalItems;
                    gameStatistic.total = getData.total;
                }
                else
                {
                    gameStatistic.total = getData.total;
                }
                return gameStatistic;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new GameStatisticsBO();
        }

        void SortList<T>(List<T> dataSource, Sorting sorting)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(sorting.SortCol);
            if (propInfo == null)
                return;
            bool isAsc = false;
            if (sorting.SortType.ToLower().Equals("asc"))
                isAsc = true;
            else
                isAsc = false;
            if (!sorting.IsDecimal)
            {
                if (sorting.IsDate)
                {
                    Comparison<T> compare = delegate(T a, T b)
                    {
                        object valueA = isAsc ? CastAide.AsDateTime(propInfo.GetValue(a, null)) : CastAide.AsDateTime(propInfo.GetValue(b, null));
                        object valueB = isAsc ? CastAide.AsDateTime(propInfo.GetValue(b, null)) : CastAide.AsDateTime(propInfo.GetValue(a, null));

                        return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                    };
                    dataSource.Sort(compare);
                }
                else
                {
                    Comparison<T> compare = delegate(T a, T b)
                    {
                        object valueA = isAsc ? (propInfo.GetValue(a, null)) : (propInfo.GetValue(b, null));
                        object valueB = isAsc ? (propInfo.GetValue(b, null)) : (propInfo.GetValue(a, null));

                        return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                    };
                    dataSource.Sort(compare);
                }

            }
            else
            {
                Comparison<T> compare = delegate(T a, T b)
                {
                    object valueA = isAsc ? CastAide.AsDecimal(propInfo.GetValue(a, null).ToString().Replace("%", "")) : CastAide.AsDecimal(propInfo.GetValue(b, null).ToString().Replace("%", ""));
                    object valueB = isAsc ? CastAide.AsDecimal(propInfo.GetValue(b, null).ToString().Replace("%", "")) : CastAide.AsDecimal(propInfo.GetValue(a, null).ToString().Replace("%", ""));

                    return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                };
                dataSource.Sort(compare);
            }
        }

        private List<GameStatisticsDTO> GetGameStatisticWithGroup(List<GameStatisticsDTO> list)
        {

            var items = new List<GameStatisticsDTO>();
            List<string> listDaily = list.Select(s => s.ReportType).Distinct().ToList();
            foreach (string daily in listDaily)
            {
                var item = new GameStatisticsDTO();
                item.ReportType = daily;
                item.Items = new List<GameStatisticsDTO>();
                item.Items.AddRange(list.Where(s => s.ReportType == daily).ToList());
                items.Add(item);
            }
            return items;
        }

        private GameStatisticsDTO SetGameStatisticsIncomeAndPayout(GameStatisticsDTO item)
        {
            item.GameIncome =
                            NumberFormat.DecFormat(
                                CastAide.AsDecimal(item.TotalBets) - CastAide.AsDecimal(item.TotalWins), 2);
            if (CastAide.AsDecimal(item.TotalBets) == 0)
            {
                item.GamePayout = "N/A";
            }
            else
            {
                item.GamePayout = NumberFormat.DecFormat(
                   CastAide.AsDecimal(item.TotalWins) / CastAide.AsDecimal(item.TotalBets), 2) + "%";
            }


            return item;
        }

        #endregion

        public ListDailyWL GetDailyWLDirectLC(string winlossdate)
        {
            var objDailyWL = new ListDailyWL();
            var listdailyWL = new List<DailyWinloss>();
            double winlost = 0, position = 0, commission = 0, turnover = 0, linetotal = 0, margin = 0;
            var total = new DailyWinloss();
            try
            {
                DataTable datatable = ReportAccess.GetDailyWinlossLC(winlossdate);
                if (datatable != null || datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in datatable.Rows)
                    {
                        listdailyWL.Add(new DailyWinloss()
                        {
                            bettype = CacheRef.GetBettype(row["bettype"]),
                            winloss = NumberFormat.DecFormat(row["winlost"].ToString(), 2),
                            position = NumberFormat.DecFormat(row["position"].ToString(), 2),
                            comm = NumberFormat.DecFormat(row["commission"].ToString(), 2),
                            turnover = NumberFormat.DecFormat(row["turnover"].ToString(), 2),
                            linetotal = NumberFormat.DecFormat(row["linetotal"].ToString(), 2),
                            margin = CastAide.AsDouble(row["Turnover"]) == 0
                                                                   ? "N/A"
                                                                   : NumberFormat.DecFormat(
                                                                       CastAide.AsDouble(row["LineTotal"]) /
                                                                       CastAide.AsDouble(row["Turnover"]) *
                                                                       100, 2)

                        });
                        winlost += CastAide.AsDouble(row["winlost"]);
                        position += CastAide.AsDouble(row["position"]);
                        commission += CastAide.AsDouble(row["commission"]);
                        turnover += CastAide.AsDouble(row["turnover"]);
                        linetotal += CastAide.AsDouble(row["linetotal"]);
                    }
                    objDailyWL.Items = listdailyWL;
                    
                    total.margin = NumberFormat.DecFormat(turnover == 0 ? "N/A" : (linetotal / turnover * 100).ToString(), 2);
                    total.winloss = NumberFormat.DecFormat(winlost.ToString(), 2);
                    total.position = NumberFormat.DecFormat(position.ToString(), 2);
                    total.comm = NumberFormat.DecFormat(commission.ToString(), 2);
                    total.turnover = NumberFormat.DecFormat(turnover.ToString(), 2);
                    total.linetotal = NumberFormat.DecFormat(linetotal.ToString(), 2);
                    objDailyWL.Total = total;

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyDailyWLDirectLC, objDailyWL);
            return objDailyWL;
        }
        
        #region game statistics = filter

        private List<Category> GetAllCompanyAllowPlayLiveCasino()
        {
            var result = new List<Category>();
            try
            {
                result.Add(new Category()
                               {
                                   Id = -1,
                                   Name = "IBC",
                                   Selected = true
                               });
                var getData = ReportAccess.GetCompanyAllowPlayLiveCasino();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                                       {
                                           Id = CastAide.AsInt32(row["LicenseeID"]),
                                           Name = CastAide.AsString(row["Licenseename"]),
                                           Selected = false
                                       });
                    }
                }
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private List<Category> GetCurrency()
        {
            var result = new List<Category>();
            try
            {
                result.Add(new Category()
                               {
                                   Id = -1,
                                   Name = "Base RM",
                                   Selected = true
                               });
                var getData = ReportAccess.GetCurrency();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["ExchangeID"]),
                            Name = CastAide.AsString(row["Currency"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private List<Category> GetGameType()
        {
            var result = new List<Category>();
            try
            {
                var getData = ReportAccess.GetGameType();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["typeID"]),
                            Name = CastAide.AsString(row["typenamee"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public List<Category> GetGameTableByGameType(int gameType)
        {
            var result = new List<Category>();
            try
            {
                var getData = ReportAccess.GetGameTableByGameType(gameType);
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["GameTableID"]),
                            Name = CastAide.AsString(row["GameTable"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
        public GameSearchBO GetGameFilterInfo()
        {
            var result = new GameSearchBO();
            
            var config = LiveCasinoSection.Instance;
            #region filter by game info
            #region get data from config
            #endregion

            #region assign company data
            result.Company = GetAllCompanyAllowPlayLiveCasino();
            #endregion

            #region assign game type data
            result.GameType = GetGameType();
            #endregion

            #region assign currency data

            result.Currency = GetCurrency();
            #endregion

            #region assign game table data
            #endregion
            #endregion

            #region filter by report type
            var reportType = config.Report[0].ReportType;
            var reportFilter = from r in reportType.Cast<FilterElement>().Where(i => i.Active) select r;
            if (reportFilter != null && reportFilter.Count() > 0)
            {
                result.ReportType = new List<Category>();
                foreach (FilterElement filter in reportFilter)
                {
                   result.ReportType.Add(new Category()
                                             {
                                                 Id = filter.Id,
                                                 Name = filter.Name, 
                                                 Selected = filter.Selected
                                             });
                }
            }

            #endregion
            return result;
        }
        #endregion

        public List<BetlistResultLC> GetBetlistResultLC(string matchid, string bettype)
        {
            var betlistResultLC = new List<BetlistResultLC>();
            DataTable dtb = new DataTable();
            try
            {
                dtb = ReportAccess.GetBetlistResultLC(matchid);
                foreach (DataRow row in dtb.Rows)
                {
                    #region Game Type

                    string showtime = CastAide.AsString(row["showtime"]);
                    #endregion

                    #region Results
                    string betstatusresult = "";
                    string betstatus = CastAide.AsString(row["betstatus"]);
                    string[] lstbetstatus = betstatus.Split(',');
                    for (int i = 0; i < lstbetstatus.Length; i++)
                    {
                        if (i < lstbetstatus.Length - 1)
                        {
                            if (CastAide.AsInt32(bettype) == 1101)
                            {
                                string result = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(lstbetstatus[i]));
                                if (i < 2)
                                    betstatusresult += result + "; ";
                                else
                                    betstatusresult += result + ", ";
                            }
                            else if (CastAide.AsInt32(bettype) == 1102)
                            {
                                betstatusresult += CacheRef.GetRouletteBetItem(CastAide.AsInt32(lstbetstatus[i])) + ";";
                            }
                        }
                        else
                        {
                            if (CastAide.AsInt32(bettype) == 1101)
                            {
                                string result = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(lstbetstatus[i]));
                                if (lstbetstatus.Length > 2)
                                    betstatusresult += result;
                                else
                                    betstatusresult += result + ";-";
                            }
                            else if (CastAide.AsInt32(bettype) == 1102)
                            {
                                betstatusresult += CacheRef.GetRouletteBetItem(CastAide.AsInt32(lstbetstatus[i]));
                            }
                        }
                    }
                    #endregion

                    betlistResultLC.Add(new BetlistResultLC()
                    {
                        KickoffTime = CastAide.AsString(row["eventdate"]),
                        FinalAwayScore = CastAide.AsString(row["finalawayscore"]),
                        FinalHomeScore = CastAide.AsString(row["finalhomescore"]),
                        GameType = showtime,
                        Results = betstatusresult,
                        Status = CastAide.AsString(row["eventstatus"]),

                    });
                }

               
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            

            return betlistResultLC;

        }

    }
}
