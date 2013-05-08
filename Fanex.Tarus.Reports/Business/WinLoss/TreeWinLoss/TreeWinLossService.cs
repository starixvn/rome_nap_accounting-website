// -----------------------------------------------------------------------
// <copyright file="TreeWinLossService.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.WinLoss.TreeWinLoss
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Reports.DataAccess.WinLoss.TreeWinLoss;
    using Fanex.Tarus.Reports.Entities.WinLoss.TreeWinLoss;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TreeWinLossService : ITreeWinLossService
    {
        private DateTime moveBetsDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay;
        private readonly ITreeWinLossDataAccess _treeWinLossDataAccess;
        private readonly string _treeWinLossSessionKey = "Fanex.Tarus.Reports.Business.WinLoss.TreeWinLoss";
        public TreeWinLossService()
            : this(null)
        {
        }

        public TreeWinLossService(ITreeWinLossDataAccess dataAccess)
        {
            _treeWinLossDataAccess = dataAccess ?? new TreeWinLossDataAccess();
        }

        #region Product
        public TreeWinLoss GetTreeWinLossReportByProduct(string fromDate,
                                                            string toDate,
                                                            bool isShowSB,
                                                            bool isShowNG,
                                                            bool isShowRB,
                                                            bool isShowLCS,
                                                            bool isShowCS,
                                                            bool isShowP2P,
                                                            bool isShowBI,
                                                            bool isShowVS)
        {
            var winlossModel = new TreeWinLoss();
            AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, null);

            try
            {
                var winloss14 = new TreeWinLoss();
                var winlossWH = new TreeWinLoss();

                Task<TreeWinLoss>[] tasks = new Task<TreeWinLoss>[2];

                if (CastAide.AsDateTime(toDate) > moveBetsDay && CastAide.AsDateTime(fromDate) > moveBetsDay)
                {
                    winloss14 = GetTreeWinLossReportByProduct_DateRange(
                                                                       fromDate,
                                                                       toDate,
                                                                       isShowSB,
                                                                       isShowNG,
                                                                       isShowRB,
                                                                       isShowLCS,
                                                                       isShowCS,
                                                                       isShowP2P,
                                                                       isShowBI,
                                                                       isShowVS,
                                                                       false);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportByProduct_DateRange(
                                                                     fromDate,
                                                                     toDate,
                                                                     isShowSB,
                                                                     isShowNG,
                                                                     isShowRB,
                                                                     isShowLCS,
                                                                     isShowCS,
                                                                     isShowP2P,
                                                                     isShowBI,
                                                                     isShowVS,
                                                                     true);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) >= moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByProduct_DateRange(
                                                                       moveBetsDay.AddDays(1).ToString(),
                                                                       toDate,
                                                                       isShowSB,
                                                                       isShowNG,
                                                                       isShowRB,
                                                                       isShowLCS,
                                                                       isShowCS,
                                                                       isShowP2P,
                                                                       isShowBI,
                                                                       isShowVS,
                                                                       false);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) < moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByProduct_DateRange(
                                                                     fromDate,
                                                                     moveBetsDay.ToString(),
                                                                     isShowSB,
                                                                     isShowNG,
                                                                     isShowRB,
                                                                     isShowLCS,
                                                                     isShowCS,
                                                                     isShowP2P,
                                                                     isShowBI,
                                                                     isShowVS,
                                                                     true);
                        });
                    }
                    else
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    Task.WaitAll(tasks);

                    winlossWH = tasks[1].Result;
                    winloss14 = tasks[0].Result;

                }
                List<TreeWinLossDetail> treeWinlossListTmp = new List<TreeWinLossDetail>();

                treeWinlossListTmp = winloss14.WinLoss.Concat(winlossWH.WinLoss).ToList();

                winlossModel.WinLoss = treeWinlossListTmp.GroupBy(x => x.Id)
                                              .Select(y => new TreeWinLossDetail
                                              {
                                                  Id = string.Format("{0}", y.Key),
                                                  Title = CacheUtils.GetExchangeName(CastAide.AsInt32(y.Key)),
                                                  Turnover = y.Sum(x => x.Turnover),
                                                  WinLoss = y.Sum(x => x.WinLoss),
                                                  Comm = y.Sum(x => x.Comm),
                                                  Total = y.Sum(x => x.Total),
                                                  BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                  BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                  BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                  BaseRMVariance = y.Sum(x => x.BaseRMVariance),
                                                  VirtualRate = y.Sum(x => x.VirtualRate),
                                                  Margin = y.Sum(x => x.BaseRMTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMTotal) / y.Sum(x => x.BaseRMTurnover) * 100)
                                              }).ToList();


                winlossModel.GrandTotal.BaseRMTurnover = winloss14.GrandTotal.BaseRMTurnover + winlossWH.GrandTotal.BaseRMTurnover;
                winlossModel.GrandTotal.BaseRMTotal = winloss14.GrandTotal.BaseRMTotal + winlossWH.GrandTotal.BaseRMTotal;
                winlossModel.GrandTotal.BaseRMVirtualTotal = winloss14.GrandTotal.BaseRMVirtualTotal + winlossWH.GrandTotal.BaseRMVirtualTotal;
                winlossModel.GrandTotal.BaseRMVariance = winloss14.GrandTotal.BaseRMVariance + winlossWH.GrandTotal.BaseRMVariance;
                winlossModel.GrandTotal.Margin = winlossModel.GrandTotal.BaseRMTurnover == 0 ? 0 : (winlossModel.GrandTotal.BaseRMTotal / winlossModel.GrandTotal.BaseRMTurnover * 100);

                winlossModel.Total8RM.BaseRMTurnover = winloss14.Total8RM.BaseRMTurnover + winlossWH.Total8RM.BaseRMTurnover;
                winlossModel.Total8RM.BaseRMTotal = winloss14.Total8RM.BaseRMTotal + winlossWH.Total8RM.BaseRMTotal;
                winlossModel.Total8RM.BaseRMVirtualTotal = winloss14.Total8RM.BaseRMVirtualTotal + winlossWH.Total8RM.BaseRMVirtualTotal;
                winlossModel.Total8RM.BaseRMVariance = winloss14.Total8RM.BaseRMVariance + winlossWH.Total8RM.BaseRMVariance;
                winlossModel.Total8RM.Margin = winlossModel.Total8RM.BaseRMTurnover == 0 ? 0 : (winlossModel.Total8RM.BaseRMTotal / winlossModel.Total8RM.BaseRMTurnover * 100);

                winlossModel.TotalBARM.BaseRMTurnover = winloss14.TotalBARM.BaseRMTurnover + winlossWH.TotalBARM.BaseRMTurnover;
                winlossModel.TotalBARM.BaseRMTotal = winloss14.TotalBARM.BaseRMTotal + winlossWH.TotalBARM.BaseRMTotal;
                winlossModel.TotalBARM.BaseRMVirtualTotal = winloss14.TotalBARM.BaseRMVirtualTotal + winlossWH.TotalBARM.BaseRMVirtualTotal;
                winlossModel.TotalBARM.BaseRMVariance = winloss14.TotalBARM.BaseRMVariance + winlossWH.TotalBARM.BaseRMVariance;
                winlossModel.TotalBARM.Margin = winlossModel.TotalBARM.BaseRMTurnover == 0 ? 0 : (winlossModel.TotalBARM.BaseRMTotal / winlossModel.TotalBARM.BaseRMTurnover * 100);

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVariance = winloss14.Total.BaseRMVariance + winlossWH.Total.BaseRMVariance;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMTurnover == 0 ? 0 : (winlossModel.Total.BaseRMTotal / winlossModel.Total.BaseRMTurnover * 100);

                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportByProduct_DateRange(string fromDate,
                                                            string toDate,
                                                            bool isShowSB,
                                                            bool isShowNG,
                                                            bool isShowRB,
                                                            bool isShowLCS,
                                                            bool isShowCS,
                                                            bool isShowP2P,
                                                            bool isShowBI,
                                                            bool isShowVS,
                                                            bool isWarehouse)
        {
            var winloss = new TreeWinLoss();

            try
            {
                var ouputParams = new Hashtable();
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReportByProducts(
                                                            fromDate,
                                                            toDate,
                                                            isShowSB,
                                                            isShowNG,
                                                            isShowRB,
                                                            isShowLCS,
                                                            isShowCS,
                                                            isShowP2P,
                                                            isShowBI,
                                                            isShowVS,
                                                            isWarehouse,
                                                            ref ouputParams);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVariance = 0;
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        baseRMTotal += CastAide.AsDouble(winlossDetail["totalrm"]);
                        baseRMTurnover += CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        baseRMVirtualTotal += CastAide.AsDouble(winlossDetail["virtual_total"]);
                        baseRMVariance += CastAide.AsDouble(winlossDetail["variance"]);
                        double margin = CastAide.AsDouble(winlossDetail["turnoverrm"]) == 0 ? 0 : CastAide.AsDouble(winlossDetail["totalrm"]) / CastAide.AsDouble(winlossDetail["turnoverrm"]) * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["currency"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),
                                BaseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]),
                                BaseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]),
                                BaseRMVariance = CastAide.AsDouble(winlossDetail["variance"]),
                                BaseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]),
                                Margin = margin
                            });
                        #endregion
                    }


                    #region Total
                    winloss.Total = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover,
                        BaseRMTotal = baseRMTotal,
                        BaseRMVariance = baseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal
                    };
                    #endregion

                    #region 8RM
                    winloss.Total8RM = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = CastAide.AsDouble(ouputParams["turnover_8rm"]),
                        BaseRMTotal = CastAide.AsDouble(ouputParams["total_8rm"]),
                        BaseRMVariance = CastAide.AsDouble(ouputParams["variance_8rm"]),
                        BaseRMVirtualTotal = CastAide.AsDouble(ouputParams["virtualtotal_8rm"])
                    };
                    #endregion

                    #region BARM
                    winloss.TotalBARM = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = CastAide.AsDouble(ouputParams["turnover_barm"]),
                        BaseRMTotal = CastAide.AsDouble(ouputParams["total_barm"]),
                        BaseRMVariance = CastAide.AsDouble(ouputParams["variance_barm"]),
                        BaseRMVirtualTotal = CastAide.AsDouble(ouputParams["virtualtotal_barm"])
                    };
                    #endregion

                    #region grand Total
                    winloss.GrandTotal = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover + winloss.Total8RM.BaseRMTurnover + winloss.TotalBARM.BaseRMTurnover,
                        BaseRMTotal = baseRMTotal + winloss.Total8RM.BaseRMTotal + winloss.TotalBARM.BaseRMTotal,
                        BaseRMVariance = baseRMVariance + winloss.Total8RM.BaseRMVariance + winloss.TotalBARM.BaseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal + winloss.Total8RM.BaseRMVirtualTotal + winloss.TotalBARM.BaseRMVirtualTotal
                    };
                    #endregion

                    winloss.WinLoss = winloss.WinLoss.OrderBy(c => c.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winloss;
        }
        #endregion

        #region Currency
        public TreeWinLoss GetTreeWinLossReportByCurrency(string fromDate,
                                                string toDate,
                                                int currency,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS)
        {
            var winlossModel = new TreeWinLoss();
            AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, null);
            try
            {
                var winloss14 = new TreeWinLoss();
                var winlossWH = new TreeWinLoss();

                Task<TreeWinLoss>[] tasks = new Task<TreeWinLoss>[2];
                if (CastAide.AsDateTime(toDate) > moveBetsDay && CastAide.AsDateTime(fromDate) > moveBetsDay)
                {
                    winloss14 = GetTreeWinLossReportByCurrency_DateRange(
                                                                 fromDate,
                                                                 toDate,
                                                                 currency,
                                                                 isShowSB,
                                                                 isShowNG,
                                                                 isShowRB,
                                                                 isShowLCS,
                                                                 isShowCS,
                                                                 isShowP2P,
                                                                 isShowBI,
                                                                 isShowVS, false);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportByCurrency_DateRange(
                                                                fromDate,
                                                                toDate,
                                                                currency,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) >= moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByCurrency_DateRange(
                                                                 moveBetsDay.AddDays(1).ToString(),
                                                                 toDate,
                                                                 currency,
                                                                 isShowSB,
                                                                 isShowNG,
                                                                 isShowRB,
                                                                 isShowLCS,
                                                                 isShowCS,
                                                                 isShowP2P,
                                                                 isShowBI,
                                                                 isShowVS, false);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) < moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByCurrency_DateRange(
                                                                fromDate,
                                                                moveBetsDay.ToString(),
                                                                currency,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                        });
                    }
                    else
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    Task.WaitAll(tasks);

                    winlossWH = tasks[1].Result;
                    winloss14 = tasks[0].Result;
                }
                List<TreeWinLossDetail> treeWinlossListTmp = new List<TreeWinLossDetail>();
                treeWinlossListTmp = winloss14.WinLoss.Concat(winlossWH.WinLoss).ToList();

                winlossModel.WinLoss = treeWinlossListTmp.GroupBy(x => new { x.Id, x.Title, x.VirtualRate })
                                               .Select(y => new TreeWinLossDetail
                                               {
                                                   Id = y.Key.Id,
                                                   Title = y.Key.Title,
                                                   Turnover = y.Sum(x => x.Turnover),
                                                   WinLoss = y.Sum(x => x.WinLoss),
                                                   Comm = y.Sum(x => x.Comm),
                                                   Total = y.Sum(x => x.Total),
                                                   BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                   BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                   BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                   BaseRMVariance = y.Sum(x => x.BaseRMVariance),
                                                   VirtualRate = y.Key.VirtualRate,
                                                   Margin = y.Sum(x => x.BaseRMTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMTotal) / y.Sum(x => x.BaseRMTurnover) * 100)
                                               }).ToList();

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVariance = winloss14.Total.BaseRMVariance + winlossWH.Total.BaseRMVariance;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMTurnover == 0 ? 0 : (winlossModel.Total.BaseRMTotal / winlossModel.Total.BaseRMTurnover * 100);

                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportByCurrency_DateRange(string fromDate,
                                                string toDate,
                                                int currency,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var winloss = new TreeWinLoss();
            try
            {
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReportByCurrency(
                                                            fromDate,
                                                            toDate,
                                                            currency,
                                                            isShowSB,
                                                            isShowNG,
                                                            isShowRB,
                                                            isShowLCS,
                                                            isShowCS,
                                                            isShowP2P,
                                                            isShowBI,
                                                            isShowVS,
                                                            isWarehouse);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    winloss.WinLoss = new List<TreeWinLossDetail>();
                    double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVariance = 0;
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        baseRMTotal += CastAide.AsDouble(winlossDetail["totalrm"]);
                        baseRMTurnover += CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        baseRMVirtualTotal += CastAide.AsDouble(winlossDetail["virtual_total"]);
                        baseRMVariance += CastAide.AsDouble(winlossDetail["variance"]);
                        double margin = CastAide.AsDouble(winlossDetail["turnoverrm"]) == 0 ? 0 : CastAide.AsDouble(winlossDetail["totalrm"]) / CastAide.AsDouble(winlossDetail["turnoverrm"]) * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["srecommend"]),
                                Title = string.Format("{0}", winlossDetail["username"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),
                                BaseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]),
                                BaseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]),
                                BaseRMVariance = CastAide.AsDouble(winlossDetail["variance"]),
                                BaseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]),
                                VirtualRate = CastAide.AsDouble(winlossDetail["virtualrate"]),
                                Margin = margin
                            });
                        #endregion
                    }

                    #region Total
                    winloss.Total = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover,
                        BaseRMTotal = baseRMTotal,
                        BaseRMVariance = baseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        Margin = baseRMTurnover == 0 ? 0 : (baseRMTotal / baseRMTurnover * 100)
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winloss;
        }
        #endregion

        #region Super
        public TreeWinLoss GetTreeWinLossReportBySuper(string fromDate,
                                                string toDate,
                                                int superId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS)
        {
            var winlossModel = new TreeWinLoss();
            AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, null);
            try
            {
                var winloss14 = new TreeWinLoss();
                var winlossWH = new TreeWinLoss();

                Task<TreeWinLoss>[] tasks = new Task<TreeWinLoss>[2];
                if (CastAide.AsDateTime(toDate) > moveBetsDay && CastAide.AsDateTime(fromDate) > moveBetsDay)
                {
                    winloss14 = GetTreeWinLossReportBySuper_DateRange(
                                                                 fromDate,
                                                                  toDate,
                                                                  superId,
                                                                  isShowSB,
                                                                  isShowNG,
                                                                  isShowRB,
                                                                  isShowLCS,
                                                                  isShowCS,
                                                                  isShowP2P,
                                                                  isShowBI,
                                                                  isShowVS, false);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportBySuper_DateRange(
                                                                fromDate,
                                                                toDate,
                                                                superId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) >= moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportBySuper_DateRange(
                                                                moveBetsDay.AddDays(1).ToString(),
                                                                 toDate,
                                                                 superId,
                                                                 isShowSB,
                                                                 isShowNG,
                                                                 isShowRB,
                                                                 isShowLCS,
                                                                 isShowCS,
                                                                 isShowP2P,
                                                                 isShowBI,
                                                                 isShowVS, false);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) < moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportBySuper_DateRange(
                                                                fromDate,
                                                                moveBetsDay.ToString(),
                                                                superId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                        });
                    }
                    else
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    Task.WaitAll(tasks);

                    winlossWH = tasks[1].Result;
                    winloss14 = tasks[0].Result;
                }
                List<TreeWinLossDetail> treeWinlossListTmp = new List<TreeWinLossDetail>();
                treeWinlossListTmp = winloss14.WinLoss.Concat(winlossWH.WinLoss).ToList();

                winlossModel.WinLoss = treeWinlossListTmp.GroupBy(x => new { x.Id, x.Title, x.VirtualRate })
                                               .Select(y => new TreeWinLossDetail
                                               {
                                                   Id = y.Key.Id,
                                                   Title = y.Key.Title,
                                                   Turnover = y.Sum(x => x.Turnover),
                                                   WinLoss = y.Sum(x => x.WinLoss),
                                                   Comm = y.Sum(x => x.Comm),
                                                   Total = y.Sum(x => x.Total),
                                                   BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                   BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                   BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                   BaseRMVariance = y.Sum(x => x.BaseRMVariance),
                                                   VirtualRate = y.Key.VirtualRate,
                                                   Margin = y.Sum(x => x.BaseRMTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMTotal) / y.Sum(x => x.BaseRMTurnover) * 100)
                                               }).ToList();

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVariance = winloss14.Total.BaseRMVariance + winlossWH.Total.BaseRMVariance;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMTurnover == 0 ? 0 : (winlossModel.Total.BaseRMTotal / winlossModel.Total.BaseRMTurnover * 100);

                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportBySuper_DateRange(string fromDate,
                                                string toDate,
                                                int superId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var winloss = new TreeWinLoss();
            try
            {
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReportBySuper(
                                                            fromDate,
                                                            toDate,
                                                            superId,
                                                            isShowSB,
                                                            isShowNG,
                                                            isShowRB,
                                                            isShowLCS,
                                                            isShowCS,
                                                            isShowP2P,
                                                            isShowBI,
                                                            isShowVS,
                                                            isWarehouse);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    winloss.WinLoss = new List<TreeWinLossDetail>();
                    double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVariance = 0;
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        baseRMTotal += CastAide.AsDouble(winlossDetail["totalrm"]);
                        baseRMTurnover += CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        baseRMVirtualTotal += CastAide.AsDouble(winlossDetail["virtual_total"]);
                        baseRMVariance += CastAide.AsDouble(winlossDetail["variance"]);
                        double margin = CastAide.AsDouble(winlossDetail["turnoverrm"]) == 0 ? 0 : CastAide.AsDouble(winlossDetail["totalrm"]) / CastAide.AsDouble(winlossDetail["turnoverrm"]) * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["mrecommend"]),
                                Title = string.Format("{0}", winlossDetail["username"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),
                                BaseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]),
                                BaseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]),
                                BaseRMVariance = CastAide.AsDouble(winlossDetail["variance"]),
                                BaseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]),
                                VirtualRate = CastAide.AsDouble(winlossDetail["virtualrate"]),
                                Margin = margin
                            });
                        #endregion
                    }

                    #region Total
                    winloss.Total = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover,
                        BaseRMTotal = baseRMTotal,
                        BaseRMVariance = baseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        Margin = baseRMTurnover == 0 ? 0 : (baseRMTotal / baseRMTurnover * 100)
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winloss;
        }
        #endregion

        #region Master
        public TreeWinLoss GetTreeWinLossReportByMaster(string fromDate,
                                                string toDate,
                                                int masterId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS)
        {
            var winlossModel = new TreeWinLoss();
            AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, null);
            try
            {
                var winloss14 = new TreeWinLoss();
                var winlossWH = new TreeWinLoss();

                Task<TreeWinLoss>[] tasks = new Task<TreeWinLoss>[2];
                if (CastAide.AsDateTime(toDate) > moveBetsDay && CastAide.AsDateTime(fromDate) > moveBetsDay)
                {
                    winloss14 = GetTreeWinLossReportByMaster_DateRange(
                                                                   fromDate,
                                                                  toDate,
                                                                  masterId,
                                                                  isShowSB,
                                                                  isShowNG,
                                                                  isShowRB,
                                                                  isShowLCS,
                                                                  isShowCS,
                                                                  isShowP2P,
                                                                  isShowBI,
                                                                  isShowVS, false);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportByMaster_DateRange(
                                                                fromDate,
                                                                toDate,
                                                                masterId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) >= moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByMaster_DateRange(
                                                                  moveBetsDay.AddDays(1).ToString(),
                                                                 toDate,
                                                                 masterId,
                                                                 isShowSB,
                                                                 isShowNG,
                                                                 isShowRB,
                                                                 isShowLCS,
                                                                 isShowCS,
                                                                 isShowP2P,
                                                                 isShowBI,
                                                                 isShowVS, false);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) < moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByMaster_DateRange(
                                                                fromDate,
                                                                moveBetsDay.ToString(),
                                                                 masterId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS, true);
                        });
                    }
                    else
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    Task.WaitAll(tasks);

                    winlossWH = tasks[1].Result;
                    winloss14 = tasks[0].Result;
                }
                List<TreeWinLossDetail> treeWinlossListTmp = new List<TreeWinLossDetail>();
                treeWinlossListTmp = winloss14.WinLoss.Concat(winlossWH.WinLoss).ToList();

                winlossModel.WinLoss = treeWinlossListTmp.GroupBy(x => new { x.Id, x.Title, x.VirtualRate })
                                               .Select(y => new TreeWinLossDetail
                                               {
                                                   Id = y.Key.Id,
                                                   Title = y.Key.Title,
                                                   Turnover = y.Sum(x => x.Turnover),
                                                   WinLoss = y.Sum(x => x.WinLoss),
                                                   Comm = y.Sum(x => x.Comm),
                                                   Total = y.Sum(x => x.Total),
                                                   BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                   BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                   BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                   BaseRMVariance = y.Sum(x => x.BaseRMVariance),
                                                   VirtualRate = y.Key.VirtualRate,
                                                   Margin = y.Sum(x => x.BaseRMTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMTotal) / y.Sum(x => x.BaseRMTurnover) * 100)
                                               }).ToList();

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVariance = winloss14.Total.BaseRMVariance + winlossWH.Total.BaseRMVariance;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMTurnover == 0 ? 0 : (winlossModel.Total.BaseRMTotal / winlossModel.Total.BaseRMTurnover * 100);

                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportByMaster_DateRange(string fromDate,
                                               string toDate,
                                               int masterId,
                                               bool isShowSB,
                                               bool isShowNG,
                                               bool isShowRB,
                                               bool isShowLCS,
                                               bool isShowCS,
                                               bool isShowP2P,
                                               bool isShowBI,
                                               bool isShowVS,
                                                bool isWarehouse)
        {
            var winloss = new TreeWinLoss();
            try
            {
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReportByMaster(
                                                            fromDate,
                                                            toDate,
                                                            masterId,
                                                            isShowSB,
                                                            isShowNG,
                                                            isShowRB,
                                                            isShowLCS,
                                                            isShowCS,
                                                            isShowP2P,
                                                            isShowBI,
                                                            isShowVS,
                                                            isWarehouse);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    winloss.WinLoss = new List<TreeWinLossDetail>();
                    double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVariance = 0;
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        baseRMTotal += CastAide.AsDouble(winlossDetail["totalrm"]);
                        baseRMTurnover += CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        baseRMVirtualTotal += CastAide.AsDouble(winlossDetail["virtual_total"]);
                        baseRMVariance += CastAide.AsDouble(winlossDetail["variance"]);
                        double margin = CastAide.AsDouble(winlossDetail["turnoverrm"]) == 0 ? 0 : CastAide.AsDouble(winlossDetail["totalrm"]) / CastAide.AsDouble(winlossDetail["turnoverrm"]) * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["recommend"]),
                                Title = string.Format("{0}", winlossDetail["username"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),
                                BaseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]),
                                BaseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]),
                                BaseRMVariance = CastAide.AsDouble(winlossDetail["variance"]),
                                BaseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]),
                                VirtualRate = CastAide.AsDouble(winlossDetail["virtualrate"]),
                                Margin = margin
                            });
                        #endregion
                    }

                    #region Total
                    winloss.Total = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover,
                        BaseRMTotal = baseRMTotal,
                        BaseRMVariance = baseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        Margin = baseRMTurnover == 0 ? 0 : (baseRMTotal / baseRMTurnover * 100)
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winloss;
        }
        #endregion

        #region Agent
        public TreeWinLoss GetTreeWinLossReportByAgent(string fromDate,
                                                string toDate,
                                                int agentId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS)
        {
            var winlossModel = new TreeWinLoss();
            AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, null);
            try
            {
                var winloss14 = new TreeWinLoss();
                var winlossWH = new TreeWinLoss();

                Task<TreeWinLoss>[] tasks = new Task<TreeWinLoss>[2];
                if (CastAide.AsDateTime(toDate) > moveBetsDay && CastAide.AsDateTime(fromDate) > moveBetsDay)
                {
                    winloss14 = GetTreeWinLossReportByAgent_DateRange(
                                                                 fromDate,
                                                                  toDate,
                                                                  agentId,
                                                                  isShowSB,
                                                                  isShowNG,
                                                                  isShowRB,
                                                                  isShowLCS,
                                                                  isShowCS,
                                                                  isShowP2P,
                                                                  isShowBI,
                                                                  isShowVS,
                                                                  false);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportByAgent_DateRange(
                                                                fromDate,
                                                                toDate,
                                                                agentId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS,
                                                                true);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) >= moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByAgent_DateRange(
                                                                moveBetsDay.AddDays(1).ToString(),
                                                                 toDate,
                                                                 agentId,
                                                                 isShowSB,
                                                                 isShowNG,
                                                                 isShowRB,
                                                                 isShowLCS,
                                                                 isShowCS,
                                                                 isShowP2P,
                                                                 isShowBI,
                                                                 isShowVS,
                                                                 false);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) < moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportByAgent_DateRange(
                                                                fromDate,
                                                                moveBetsDay.ToString(),
                                                                agentId,
                                                                isShowSB,
                                                                isShowNG,
                                                                isShowRB,
                                                                isShowLCS,
                                                                isShowCS,
                                                                isShowP2P,
                                                                isShowBI,
                                                                isShowVS,
                                                                true);
                        });
                    }
                    else
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    Task.WaitAll(tasks);

                    winlossWH = tasks[1].Result;
                    winloss14 = tasks[0].Result;
                }
                List<TreeWinLossDetail> treeWinlossListTmp = new List<TreeWinLossDetail>();
                treeWinlossListTmp = winloss14.WinLoss.Concat(winlossWH.WinLoss).ToList();

                winlossModel.WinLoss = treeWinlossListTmp.GroupBy(x => new { x.Id, x.Title, x.VirtualRate })
                                               .Select(y => new TreeWinLossDetail
                                               {
                                                   Id = y.Key.Id,
                                                   Title = y.Key.Title,
                                                   Turnover = y.Sum(x => x.Turnover),
                                                   WinLoss = y.Sum(x => x.WinLoss),
                                                   Comm = y.Sum(x => x.Comm),
                                                   Total = y.Sum(x => x.Total),
                                                   BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                   BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                   BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                   BaseRMVariance = y.Sum(x => x.BaseRMVariance),
                                                   VirtualRate = y.Key.VirtualRate,
                                                   Margin = y.Sum(x => x.BaseRMTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMTotal) / y.Sum(x => x.BaseRMTurnover) * 100)
                                               }).ToList();

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVariance = winloss14.Total.BaseRMVariance + winlossWH.Total.BaseRMVariance;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMTurnover == 0 ? 0 : (winlossModel.Total.BaseRMTotal / winlossModel.Total.BaseRMTurnover * 100);

                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportByAgent_DateRange(string fromDate,
                                              string toDate,
                                              int agentId,
                                              bool isShowSB,
                                              bool isShowNG,
                                              bool isShowRB,
                                              bool isShowLCS,
                                              bool isShowCS,
                                              bool isShowP2P,
                                              bool isShowBI,
                                              bool isShowVS,
                                              bool isWarehouse)
        {
            var winloss = new TreeWinLoss();
            try
            {
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReportByAgent(
                                                            fromDate,
                                                            toDate,
                                                            agentId,
                                                            isShowSB,
                                                            isShowNG,
                                                            isShowRB,
                                                            isShowLCS,
                                                            isShowCS,
                                                            isShowP2P,
                                                            isShowBI,
                                                            isShowVS,
                                                            isWarehouse);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    winloss.WinLoss = new List<TreeWinLossDetail>();
                    double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVariance = 0;
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        baseRMTotal += CastAide.AsDouble(winlossDetail["totalrm"]);
                        baseRMTurnover += CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        baseRMVirtualTotal += CastAide.AsDouble(winlossDetail["virtual_total"]);
                        baseRMVariance += CastAide.AsDouble(winlossDetail["variance"]);
                        double margin = CastAide.AsDouble(winlossDetail["turnoverrm"]) == 0 ? 0 : CastAide.AsDouble(winlossDetail["totalrm"]) / CastAide.AsDouble(winlossDetail["turnoverrm"]) * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["custid"]),
                                Title = string.Format("{0}", winlossDetail["username"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),
                                BaseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]),
                                BaseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]),
                                BaseRMVariance = CastAide.AsDouble(winlossDetail["variance"]),
                                BaseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]),
                                VirtualRate = CastAide.AsDouble(winlossDetail["virtualrate"]),
                                Margin = margin
                            });
                        #endregion
                    }

                    #region Total
                    winloss.Total = new TreeWinLossDetail()
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        Turnover = 0,
                        WinLoss = 0,
                        Comm = 0,
                        Total = 0,
                        BaseRMTurnover = baseRMTurnover,
                        BaseRMTotal = baseRMTotal,
                        BaseRMVariance = baseRMVariance,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        Margin = baseRMTurnover == 0 ? 0 : (baseRMTotal / baseRMTurnover * 100)
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winloss;
        }
        #endregion


        public IList<HistoryDetail> GetVirtualRateUpdateHistory(int customerId)
        {
            var histories = new List<HistoryDetail>();
            try
            {
                var getReport = _treeWinLossDataAccess.GetVirtualRateUpdateHistory(customerId);
                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbHistory = getReport.Tables[0];
                    foreach (DataRow history in tbHistory.Rows)
                    {
                        histories.Add(new HistoryDetail()
                        {
                            ChangedBy = string.Format("{0}", history["username"]),
                            UpdatedDate = string.Format("{0}", history["logdate"]),
                            Value = string.Format("{0}", history["virtualrate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return histories;
        }

        public void ExportTreeWinLossReportToExcelFile(string reportTitle,
                                            int reportType,
                                             string fromDate,
                                            string toDate,
                                            int uplineId,
                                            string uplineName,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "TreeWinLossReport";
                var excelFile = new ExcelFile();
                var getSession = (TreeWinLoss)AppSession.GetSession(HttpContext.Current.Session, _treeWinLossSessionKey);
                #region get new data when session null
                if (getSession == null)
                {
                    if (reportType == 0)
                    {
                        var winLoss = GetTreeWinLossReportByProduct(fromDate,
                                                    toDate,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS);
                    }
                    else
                    {
                        switch (reportType)
                        {
                            case 1://get all supers by currency                                
                                getSession = GetTreeWinLossReportByCurrency(fromDate,
                                                    toDate,
                                                    uplineId,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS);
                                break;
                            case 2://get all masters by superid
                                getSession = GetTreeWinLossReportBySuper(fromDate,
                                                    toDate,
                                                    uplineId,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS);
                                break;
                            case 3://get all agents by master id                                
                                getSession = GetTreeWinLossReportByMaster(fromDate,
                                                    toDate,
                                                    uplineId,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS);
                                break;
                            case 4://get all members by agent id
                                getSession = GetTreeWinLossReportByAgent(fromDate,
                                                    toDate,
                                                    uplineId,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS);
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion
                var wsReport = excelFile.Worksheets.Add(string.Format("TreeWinLoss"));
                #endregion

                #region header
                reportTitle = string.Format("{0} - From {1} to {2}", reportTitle, fromDate, toDate);
                ExportTreeWinLossReportToExcelFileAssignHeader(startRow, startCol, ref wsReport, reportTitle, reportType);
                #endregion

                #region content
                if (getSession != null && getSession.WinLoss.Count > 0)
                {
                    var nextRow = 1;
                    ExportTreeWinLossReportToExcelFileAssignBody(startRow, startCol, ref nextRow, ref wsReport, getSession.WinLoss, reportType, isShowSB,
                                             isShowNG,
                                             isShowRB,
                                             isShowLCS,
                                             isShowCS,
                                             isShowP2P,
                                             isShowBI,
                                             isShowVS);
                    ExportTreeWinLossReportToExcelFileAssignFooter(startRow, startCol, ref nextRow, ref wsReport, getSession, reportType, isShowSB,
                                             isShowNG,
                                             isShowRB,
                                             isShowLCS,
                                             isShowCS,
                                             isShowP2P,
                                             isShowBI,
                                             isShowVS);
                }
                #endregion

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}_From_{1:MMddyyyy}_To_{2:MMddyyyy}.xls", reportName, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #region private functions
        private void ExportTreeWinLossReportToExcelFileAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title, int reportType)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            if (reportType != 0)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Value = "No";
                wsReport.Columns[startCol].Width = 255 * 10;

                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = reportType == 0 ? "Currency" : "Username";
                wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;
            }

            else
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Value = reportType == 0 ? "Currency" : "Username";
                wsReport.Columns[startCol].Width = 255 * 20;
            }

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Win Loss";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Comm";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Total";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;
            #region base RM group
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 4)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 4)).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 4)).Value = "Base RM";

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow, startCol + (nextCol)].Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow, startCol + (nextCol)].Value = "Total";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow, startCol + (nextCol)].Value = "Virtual Total";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow, startCol + (nextCol)].Value = "Variance";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow, startCol + (nextCol)].Value = "Margin (%)";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;
            #endregion

            #region virtual rate
            if (reportType > 0)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Virtual Rate (%)";
                wsReport.Columns[startCol + (nextCol)].Width = 255 * 10;
            }
            #endregion

            #region header title
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Value = title;
            #endregion
        }

        private void ExportTreeWinLossReportToExcelFileAssignBody(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, List<TreeWinLossDetail> winloss, int reportType, 
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            var cellNumberNormalFormat = GetStyleDecimalFormat(".00", Color.FromArgb(255, 255, 255), false, false);
            var cellSpecialNumberNormalFormat = GetStyleDecimalFormat(".00", Color.FromArgb(255, 255, 255), false, true);
            var cellTextFormat = new ExcelGemboxUtils().GetStyleText(255, 255, 255);
            cellTextFormat.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            foreach (TreeWinLossDetail winlossDetail in winloss)
            {
                string turnover = string.Empty;

                var baseRMTotal = CastAide.AsDouble(winlossDetail.BaseRMTotal);
                var baseRMVariance = CastAide.AsDouble(winlossDetail.BaseRMVariance);
                var nextCol = 0;
                if (reportType != 0)
                {
                    wsReport.Cells[startRow + nextRow, startCol].Style = cellTextFormat;
                    wsReport.Cells[startRow + nextRow, startCol].Value = nextRow;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellTextFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winlossDetail.Title;
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol].Style = cellTextFormat;
                    wsReport.Cells[startRow + nextRow, startCol].Value = winlossDetail.Title;
                }

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.Turnover);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.WinLoss);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.Comm);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.Total);
              

                if (isShowSB == true ||
               isShowNG == true ||
               isShowRB == true ||
               isShowLCS == true ||
               isShowVS == true)
                {
                    turnover = NumberFormat.DoubleFormat(winlossDetail.Turnover, 2);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.BaseRMTurnover);
                }
                else
                {
                    turnover = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                     wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
               
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = baseRMTotal;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = baseRMVariance;
                if (baseRMTotal > 0 && baseRMVariance != 0)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Color = Color.Red;
                }

                if (turnover == "N/A" || turnover == "0.00")
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.Margin);
                }
               

                if (reportType > 0)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellTextFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winlossDetail.VirtualRate;
                }
                nextRow++;
            }

        }

        private void ExportTreeWinLossReportToExcelFileAssignFooter(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, TreeWinLoss winloss, int reportType,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            var cellNumberNormalFormat = GetStyleDecimalFormat(".00", Color.FromArgb(205, 222, 255), true, false);
            int nextCol = 0;
            if (reportType == 0)
            {
                nextCol = 4;
            }
            else
            {
                nextCol = 5;
            }
            #region view by member level
            if (reportType > 0)
            {
                string turnover = string.Empty;

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Value = "Total";

                if (isShowSB == true ||
               isShowNG == true ||
               isShowRB == true ||
               isShowLCS == true ||
               isShowVS == true)
                {
                    turnover = NumberFormat.DoubleFormat(winloss.Total.BaseRMTurnover, 2);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTurnover);
                }
                else
                {
                    turnover = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }

               
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVariance);
                if (turnover == "N/A" || turnover == "0.00")
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.Margin);
                }

               

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
            }
            #endregion
            #region view all currencies
            else
            {
                string turnoverTotal = string.Empty;
                string turnover8RM = string.Empty;
                string turnoverBARM = string.Empty;
                string turnoverGrandTotal = string.Empty;


                #region total
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Value = string.Empty;

                if (isShowSB == true ||
                 isShowNG == true ||
                 isShowRB == true ||
                 isShowLCS == true ||
                 isShowVS == true)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTurnover);
                    turnoverTotal = NumberFormat.DoubleFormat(winloss.Total.BaseRMTurnover, 2);
                }
                else
                {
                    turnoverTotal = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                   
                }

               
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVariance);

                 if (turnoverTotal == "N/A" || turnoverTotal == "0.00")
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                    }
                    else
                    {
                         wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                         wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.Margin);
                    }

               
                nextCol = 3;
                nextRow++;
                #endregion

                cellNumberNormalFormat.FillPattern.SetSolid(Color.FromArgb(255, 255, 255));
                #region 8rm
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Value = string.Empty;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "8RM";
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                if (isShowSB == true ||
                isShowNG == true ||
                isShowRB == true ||
                isShowLCS == true ||
                isShowVS == true)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMTurnover);
                    turnover8RM = NumberFormat.DoubleFormat(winloss.Total8RM.BaseRMTurnover, 2);
                }
                else
                {
                    turnover8RM = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                }

              
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMVariance);

                if (turnover8RM == "N/A" || turnover8RM == "0.00")
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.Margin);
                }

               

                nextCol = 3;
                nextRow++;
                #endregion

                #region barm
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Value = string.Empty;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "BARM";
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                if (isShowSB == true ||
               isShowNG == true ||
               isShowRB == true ||
               isShowLCS == true ||
               isShowVS == true)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMTurnover);
                    turnoverBARM = NumberFormat.DoubleFormat(winloss.TotalBARM.BaseRMTurnover, 2);
                }
                else
                {
                    turnoverBARM = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }

              
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMVariance);
                if (turnoverBARM == "N/A" || turnoverBARM == "0.00")
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.Margin);
                }
               
                nextCol = 3;
                nextRow++;
                #endregion

                #region grand total
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Value = string.Empty;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "Total";
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (isShowSB == true ||
             isShowNG == true ||
             isShowRB == true ||
             isShowLCS == true ||
             isShowVS == true)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMTurnover);
                    turnoverGrandTotal = NumberFormat.DoubleFormat(winloss.GrandTotal.BaseRMTurnover, 2);
                }
                else
                {
                    turnoverGrandTotal = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                }
                
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMVirtualTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMVariance);
                if (turnoverGrandTotal == "N/A" || turnoverGrandTotal == "0.00")
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.Margin);
                }
                
                nextCol = 3;
                nextRow++;
                #endregion
            }
            #endregion
        }

        void SortList<T>(List<T> dataSource, string sortingCol, string sortType)
        {

            PropertyInfo propInfo = typeof(T).GetProperty(sortingCol);

            if (propInfo == null)
                return;


            bool IsAsc = false;
            if (sortType == "asc")
                IsAsc = true;
            else if (sortType == "desc")
                IsAsc = false;

            Comparison<T> compare = delegate(T a, T b)
            {
                object valueA = IsAsc ? CastAide.AsDecimal(propInfo.GetValue(a, null)) : CastAide.AsDecimal(propInfo.GetValue(b, null));
                object valueB = IsAsc ? CastAide.AsDecimal(propInfo.GetValue(b, null)) : CastAide.AsDecimal(propInfo.GetValue(a, null));

                return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
            };
            dataSource.Sort(compare);

        }

        private CellStyle GetStyleDecimalFormat(string symbol, Color bgColor, bool isBold, bool isPossitiveColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = string.Format("{0}[>0]#,##0.00;[<0]-#,##0.00;#,##0.00", isPossitiveColor ? "[RED]" : "");
            style.FillPattern.SetSolid(bgColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isBold)
            {
                style.Font.Weight = ExcelFont.BoldWeight;
            }

            return style;
        }
        #endregion


        public TreeWinLoss SortingTreeWinLossReport(string sortCol, string sortType)
        {
            var result = new TreeWinLoss();
            try
            {
                var getSession = (TreeWinLoss)AppSession.GetSession(HttpContext.Current.Session, _treeWinLossSessionKey);
                if (getSession != null)
                {
                    SortList(getSession.WinLoss, sortCol, sortType);
                    return getSession;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}