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
        private readonly string _treeWinLossSessionKey = "Fanex.Tarus.Reports.Business.WinLoss.TreeWinLoss." + AppSession.Username;
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
                    if (CastAide.AsDateTime(toDate) > moveBetsDay)
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

                    if (CastAide.AsDateTime(fromDate) <= moveBetsDay)
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
                                                  BaseRMVirtualTurnover = y.Sum(x => x.BaseRMVirtualTurnover),
                                                  BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                  BaseRMTOVariancePlus = y.Sum(x => x.BaseRMTOVariancePlus),
                                                  BaseRMTOVarianceMinus = y.Sum(x => x.BaseRMTOVarianceMinus),
                                                  BaseRMWLVariancePlus = y.Sum(x => x.BaseRMWLVariancePlus),
                                                  BaseRMWLVarianceMinus = y.Sum(x => x.BaseRMWLVarianceMinus),
                                                  VirtualRate = y.Sum(x => x.VirtualRate),
                                                  Margin = y.Sum(x => x.BaseRMVirtualTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMVirtualTotal) / y.Sum(x => x.BaseRMVirtualTurnover) * 100)
                                              }).ToList();


                winlossModel.GrandTotal.BaseRMTurnover = winloss14.GrandTotal.BaseRMTurnover + winlossWH.GrandTotal.BaseRMTurnover;
                winlossModel.GrandTotal.BaseRMTotal = winloss14.GrandTotal.BaseRMTotal + winlossWH.GrandTotal.BaseRMTotal;
                winlossModel.GrandTotal.BaseRMVirtualTotal = winloss14.GrandTotal.BaseRMVirtualTotal + winlossWH.GrandTotal.BaseRMVirtualTotal;
                winlossModel.GrandTotal.BaseRMVirtualTurnover = winloss14.GrandTotal.BaseRMVirtualTurnover + winlossWH.GrandTotal.BaseRMVirtualTurnover;
                winlossModel.GrandTotal.BaseRMTOVariancePlus = winloss14.GrandTotal.BaseRMTOVariancePlus + winlossWH.GrandTotal.BaseRMTOVariancePlus;
                winlossModel.GrandTotal.BaseRMTOVarianceMinus = winloss14.GrandTotal.BaseRMTOVarianceMinus + winlossWH.GrandTotal.BaseRMTOVarianceMinus;
                winlossModel.GrandTotal.BaseRMWLVariancePlus = winloss14.GrandTotal.BaseRMWLVariancePlus + winlossWH.GrandTotal.BaseRMWLVariancePlus;
                winlossModel.GrandTotal.BaseRMWLVarianceMinus = winloss14.GrandTotal.BaseRMWLVarianceMinus + winlossWH.GrandTotal.BaseRMWLVarianceMinus;
                winlossModel.GrandTotal.Margin = winlossModel.GrandTotal.BaseRMVirtualTurnover == 0 ? 0 : (winlossModel.GrandTotal.BaseRMVirtualTotal / winlossModel.GrandTotal.BaseRMVirtualTurnover * 100);

                winlossModel.Total8RM.BaseRMTurnover = winloss14.Total8RM.BaseRMTurnover + winlossWH.Total8RM.BaseRMTurnover;
                winlossModel.Total8RM.BaseRMTotal = winloss14.Total8RM.BaseRMTotal + winlossWH.Total8RM.BaseRMTotal;
                winlossModel.Total8RM.BaseRMVirtualTotal = winloss14.Total8RM.BaseRMVirtualTotal + winlossWH.Total8RM.BaseRMVirtualTotal;
                winlossModel.Total8RM.BaseRMVirtualTurnover = winloss14.Total8RM.BaseRMVirtualTurnover + winlossWH.Total8RM.BaseRMVirtualTurnover;
                winlossModel.Total8RM.BaseRMTOVariancePlus = winloss14.Total8RM.BaseRMTOVariancePlus + winlossWH.Total8RM.BaseRMTOVariancePlus;
                winlossModel.Total8RM.BaseRMTOVarianceMinus = winloss14.Total8RM.BaseRMTOVarianceMinus + winlossWH.Total8RM.BaseRMTOVarianceMinus;
                winlossModel.Total8RM.BaseRMWLVariancePlus = winloss14.Total8RM.BaseRMWLVariancePlus + winlossWH.Total8RM.BaseRMWLVariancePlus;
                winlossModel.Total8RM.BaseRMWLVarianceMinus = winloss14.Total8RM.BaseRMWLVarianceMinus + winlossWH.Total8RM.BaseRMWLVarianceMinus;
                winlossModel.Total8RM.Margin = winlossModel.Total8RM.BaseRMVirtualTurnover == 0 ? 0 : (winlossModel.Total8RM.BaseRMVirtualTotal / winlossModel.Total8RM.BaseRMVirtualTurnover * 100);

                winlossModel.TotalBARM.BaseRMTurnover = winloss14.TotalBARM.BaseRMTurnover + winlossWH.TotalBARM.BaseRMTurnover;
                winlossModel.TotalBARM.BaseRMTotal = winloss14.TotalBARM.BaseRMTotal + winlossWH.TotalBARM.BaseRMTotal;
                winlossModel.TotalBARM.BaseRMVirtualTotal = winloss14.TotalBARM.BaseRMVirtualTotal + winlossWH.TotalBARM.BaseRMVirtualTotal;
                winlossModel.TotalBARM.BaseRMVirtualTurnover = winloss14.TotalBARM.BaseRMVirtualTurnover + winlossWH.TotalBARM.BaseRMVirtualTurnover;
                winlossModel.TotalBARM.BaseRMTOVariancePlus = winloss14.TotalBARM.BaseRMTOVariancePlus + winlossWH.TotalBARM.BaseRMTOVariancePlus;
                winlossModel.TotalBARM.BaseRMTOVarianceMinus = winloss14.TotalBARM.BaseRMTOVarianceMinus + winlossWH.TotalBARM.BaseRMTOVarianceMinus;
                winlossModel.TotalBARM.BaseRMWLVariancePlus = winloss14.TotalBARM.BaseRMWLVariancePlus + winlossWH.TotalBARM.BaseRMWLVariancePlus;
                winlossModel.TotalBARM.BaseRMWLVarianceMinus = winloss14.TotalBARM.BaseRMWLVarianceMinus + winlossWH.TotalBARM.BaseRMWLVarianceMinus;
                winlossModel.TotalBARM.Margin = winlossModel.TotalBARM.BaseRMVirtualTurnover == 0 ? 0 : (winlossModel.TotalBARM.BaseRMVirtualTotal / winlossModel.TotalBARM.BaseRMVirtualTurnover * 100);

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVirtualTurnover = winloss14.Total.BaseRMVirtualTurnover + winlossWH.Total.BaseRMVirtualTurnover;
                winlossModel.Total.BaseRMTOVariancePlus = winloss14.Total.BaseRMTOVariancePlus + winlossWH.Total.BaseRMTOVariancePlus;
                winlossModel.Total.BaseRMTOVarianceMinus = winloss14.Total.BaseRMTOVarianceMinus + winlossWH.Total.BaseRMTOVarianceMinus;
                winlossModel.Total.BaseRMWLVariancePlus = winloss14.Total.BaseRMWLVariancePlus + winlossWH.Total.BaseRMWLVariancePlus;
                winlossModel.Total.BaseRMWLVarianceMinus = winloss14.Total.BaseRMWLVarianceMinus + winlossWH.Total.BaseRMWLVarianceMinus;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMVirtualTurnover == 0 ? 0 : (winlossModel.Total.BaseRMVirtualTotal / winlossModel.Total.BaseRMVirtualTurnover * 100);

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

                double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVirtualTO = 0, baseRMTOVariancePlus = 0, baseRMTOVarianceMinus = 0, baseRMWLVariancePlus = 0, baseRMWLVarianceMinus = 0;
                double _baseRMTotal = 0, _baseRMTurnover = 0, _baseRMVirtualTotal = 0, _baseRMVirtualTO = 0, _baseRMTOVariancePlus = 0, _baseRMTOVarianceMinus = 0, _baseRMWLVariancePlus = 0, _baseRMWLVarianceMinus = 0;

                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        _baseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]);
                        _baseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        _baseRMVirtualTO = CastAide.AsDouble(winlossDetail["Virtual_Turnover"]);
                        _baseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]);

                        _baseRMTOVariancePlus = CastAide.AsDouble(winlossDetail["TOVariance+"]);
                        _baseRMTOVarianceMinus = CastAide.AsDouble(winlossDetail["TOVariance-"]);
                        _baseRMWLVariancePlus = CastAide.AsDouble(winlossDetail["WLVariance+"]);
                        _baseRMWLVarianceMinus = CastAide.AsDouble(winlossDetail["WLVariance-"]);

                        baseRMTotal += _baseRMTotal;
                        baseRMTurnover += _baseRMTurnover;
                        baseRMVirtualTO += _baseRMVirtualTO;
                        baseRMVirtualTotal += _baseRMVirtualTotal;

                        baseRMTOVariancePlus += _baseRMTOVariancePlus;
                        baseRMTOVarianceMinus += _baseRMTOVarianceMinus;
                        baseRMWLVariancePlus += _baseRMWLVariancePlus;
                        baseRMWLVarianceMinus += _baseRMWLVarianceMinus;

                        //double margin = _baseRMVirtualTO == 0 ? 0 : _baseRMVirtualTotal / _baseRMVirtualTO * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail["currency"]),
                                //Title = CacheUtils.GetExchangeName(CastAide.AsInt32(winlossDetail["currency"])),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),

                                BaseRMTurnover = _baseRMTurnover,
                                BaseRMTotal = _baseRMTotal,
                                BaseRMTOVariancePlus = _baseRMTOVariancePlus,
                                BaseRMTOVarianceMinus = _baseRMTOVarianceMinus,
                                BaseRMWLVariancePlus = _baseRMWLVariancePlus,
                                BaseRMWLVarianceMinus = _baseRMWLVarianceMinus,
                                BaseRMVirtualTotal = _baseRMVirtualTotal,
                                BaseRMVirtualTurnover = _baseRMVirtualTO,
                                //Margin = margin
                            });
                        #endregion
                    }


                    #region Total
                    //double margin_total = baseRMVirtualTO == 0 ? 0 : baseRMVirtualTotal / baseRMVirtualTO * 100;
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
                        BaseRMTOVariancePlus = baseRMTOVariancePlus,
                        BaseRMTOVarianceMinus = baseRMTOVarianceMinus,
                        BaseRMWLVariancePlus = baseRMWLVariancePlus,
                        BaseRMWLVarianceMinus = baseRMWLVarianceMinus,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        BaseRMVirtualTurnover = baseRMVirtualTO,
                        //Margin = margin_total
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
                        BaseRMVirtualTurnover = CastAide.AsDouble(ouputParams["turnover_8rm"]),

                        BaseRMTotal = CastAide.AsDouble(ouputParams["total_8rm"]),
                        BaseRMVirtualTotal = CastAide.AsDouble(ouputParams["virtualtotal_8rm"])
                        
                    };
                    //winloss.Total8RM.Margin = winloss.Total8RM.BaseRMTurnover == 0 ? 0 : winloss.Total8RM.BaseRMTotal / winloss.Total8RM.BaseRMTurnover * 100;
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
                        BaseRMVirtualTurnover = CastAide.AsDouble(ouputParams["turnover_barm"]),

                        BaseRMTotal = CastAide.AsDouble(ouputParams["total_barm"]),                        
                        BaseRMVirtualTotal = CastAide.AsDouble(ouputParams["virtualtotal_barm"])
                    };
                    //winloss.TotalBARM.Margin = winloss.TotalBARM.BaseRMTurnover == 0 ? 0 : winloss.TotalBARM.BaseRMTotal / winloss.TotalBARM.BaseRMVirtualTurnover * 100;
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
                        BaseRMTOVariancePlus = baseRMTOVariancePlus ,
                        BaseRMTOVarianceMinus = baseRMTOVarianceMinus,
                        BaseRMWLVariancePlus = baseRMWLVariancePlus,
                        BaseRMWLVarianceMinus = baseRMWLVarianceMinus,
                        BaseRMVirtualTotal = baseRMVirtualTotal + winloss.Total8RM.BaseRMVirtualTotal + winloss.TotalBARM.BaseRMVirtualTotal,
                        BaseRMVirtualTurnover = baseRMVirtualTO + winloss.Total8RM.BaseRMVirtualTurnover + winloss.TotalBARM.BaseRMVirtualTurnover
                    };
                    //winloss.GrandTotal.Margin = winloss.GrandTotal.BaseRMVirtualTurnover == 0 ? 0 : winloss.GrandTotal.BaseRMVirtualTotal / winloss.GrandTotal.BaseRMVirtualTurnover * 100;
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

        #region Custom
        public TreeWinLoss GetTreeWinLossReport(string fromDate,
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
                                              int reportType)
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
                    winloss14 = GetTreeWinLossReportCustom_DateRange(
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
                                                                 isShowVS, false, reportType);
                }
                else if (CastAide.AsDateTime(toDate) <= moveBetsDay && CastAide.AsDateTime(fromDate) <= moveBetsDay)
                {
                    winlossWH = GetTreeWinLossReportCustom_DateRange(
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
                                                                isShowVS, true, reportType);
                }
                else
                {
                    if (CastAide.AsDateTime(toDate) > moveBetsDay)
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportCustom_DateRange(
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
                                                                 isShowVS, false, reportType);
                        });
                    }
                    else
                    {
                        tasks[0] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return new TreeWinLoss();
                        });
                    }

                    if (CastAide.AsDateTime(fromDate) <= moveBetsDay)
                    {
                        tasks[1] = Task<TreeWinLoss>.Factory.StartNew(() =>
                        {
                            return GetTreeWinLossReportCustom_DateRange(
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
                                                                isShowVS, true, reportType);
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
                                                   Id = string.Format("{0}", y.Key.Id),
                                                   Title = y.Key.Title,
                                                   Turnover = y.Sum(x => x.Turnover),
                                                   WinLoss = y.Sum(x => x.WinLoss),
                                                   Comm = y.Sum(x => x.Comm),
                                                   Total = y.Sum(x => x.Total),
                                                   BaseRMTurnover = y.Sum(x => x.BaseRMTurnover),
                                                   BaseRMTotal = y.Sum(x => x.BaseRMTotal),
                                                   BaseRMVirtualTurnover = y.Sum(x => x.BaseRMVirtualTurnover),
                                                   BaseRMVirtualTotal = y.Sum(x => x.BaseRMVirtualTotal),
                                                   BaseRMTOVariancePlus = y.Sum(x => x.BaseRMTOVariancePlus),
                                                   BaseRMTOVarianceMinus = y.Sum(x => x.BaseRMTOVarianceMinus),
                                                   BaseRMWLVariancePlus = y.Sum(x => x.BaseRMWLVariancePlus),
                                                   BaseRMWLVarianceMinus = y.Sum(x => x.BaseRMWLVarianceMinus),
                                                   VirtualRate = y.Sum(x => x.VirtualRate),
                                                   Margin = y.Sum(x => x.BaseRMVirtualTurnover) == 0 ? 0 : (y.Sum(x => x.BaseRMVirtualTotal) / y.Sum(x => x.BaseRMVirtualTurnover) * 100)
                                               }).ToList();

                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMTurnover = winloss14.Total.BaseRMTurnover + winlossWH.Total.BaseRMTurnover;
                winlossModel.Total.BaseRMTotal = winloss14.Total.BaseRMTotal + winlossWH.Total.BaseRMTotal;
                winlossModel.Total.BaseRMVirtualTotal = winloss14.Total.BaseRMVirtualTotal + winlossWH.Total.BaseRMVirtualTotal;
                winlossModel.Total.BaseRMVirtualTurnover = winloss14.Total.BaseRMVirtualTurnover + winlossWH.Total.BaseRMVirtualTurnover;
                winlossModel.Total.BaseRMTOVariancePlus = winloss14.Total.BaseRMTOVariancePlus + winlossWH.Total.BaseRMTOVariancePlus;
                winlossModel.Total.BaseRMTOVarianceMinus = winloss14.Total.BaseRMTOVarianceMinus + winlossWH.Total.BaseRMTOVarianceMinus;
                winlossModel.Total.BaseRMWLVariancePlus = winloss14.Total.BaseRMWLVariancePlus + winlossWH.Total.BaseRMWLVariancePlus;
                winlossModel.Total.BaseRMWLVarianceMinus = winloss14.Total.BaseRMWLVarianceMinus + winlossWH.Total.BaseRMWLVarianceMinus;
                winlossModel.Total.Margin = winlossModel.Total.BaseRMVirtualTurnover == 0 ? 0 : (winlossModel.Total.BaseRMVirtualTotal / winlossModel.Total.BaseRMVirtualTurnover * 100);


                AppSession.AddSession(HttpContext.Current.Session, _treeWinLossSessionKey, winlossModel);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossModel;
        }

        private TreeWinLoss GetTreeWinLossReportCustom_DateRange(string fromDate,
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
                                                bool isWarehouse,
                                                int reportType)
        {
            var winloss = new TreeWinLoss();
            try
            {
                var getReport = _treeWinLossDataAccess.GetTreeWinLossReport(
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
                                                            isWarehouse, 
                                                            reportType);

                double baseRMTotal = 0, baseRMTurnover = 0, baseRMVirtualTotal = 0, baseRMVirtualTO = 0, baseRMTOVariancePlus = 0, baseRMTOVarianceMinus = 0, baseRMWLVariancePlus = 0, baseRMWLVarianceMinus = 0;
                double _baseRMTotal = 0, _baseRMTurnover = 0, _baseRMVirtualTotal = 0, _baseRMVirtualTO = 0, _baseRMTOVariancePlus = 0, _baseRMTOVarianceMinus = 0, _baseRMWLVariancePlus = 0, _baseRMWLVarianceMinus = 0;

                if (getReport != null && getReport.Tables != null && getReport.Tables.Count > 0 && getReport.Tables[0].Rows.Count > 0)
                {
                    var tbWinLoss = getReport.Tables[0];
                    winloss.WinLoss = new List<TreeWinLossDetail>();

                    string IDMap = string.Empty;
                    switch (reportType)
                    {
                        case 1://get all supers by currency
                            IDMap = "srecommend";
                            break;
                        case 2://get all masters by superid
                            IDMap = "mrecommend";
                            break;
                        case 3://get all agents by master id
                            IDMap = "recommend";
                            break;
                        case 4://get all members by agent id
                            IDMap = "custid";
                            break;
                        default:
                            break;
                    }
                    foreach (DataRow winlossDetail in tbWinLoss.Rows)
                    {
                        _baseRMTotal = CastAide.AsDouble(winlossDetail["totalrm"]);
                        _baseRMTurnover = CastAide.AsDouble(winlossDetail["turnoverrm"]);
                        _baseRMVirtualTO = CastAide.AsDouble(winlossDetail["Virtual_Turnover"]);
                        _baseRMVirtualTotal = CastAide.AsDouble(winlossDetail["virtual_total"]);

                        _baseRMTOVariancePlus = CastAide.AsDouble(winlossDetail["TOVariance+"]);
                        _baseRMTOVarianceMinus = CastAide.AsDouble(winlossDetail["TOVariance-"]);
                        _baseRMWLVariancePlus = CastAide.AsDouble(winlossDetail["WLVariance+"]);
                        _baseRMWLVarianceMinus = CastAide.AsDouble(winlossDetail["WLVariance-"]);


                        baseRMTotal += _baseRMTotal;
                        baseRMTurnover += _baseRMTurnover;
                        baseRMVirtualTO += _baseRMVirtualTO;
                        baseRMVirtualTotal += _baseRMVirtualTotal;

                        baseRMTOVariancePlus += _baseRMTOVariancePlus;
                        baseRMTOVarianceMinus += _baseRMTOVarianceMinus;
                        baseRMWLVariancePlus += _baseRMWLVariancePlus;
                        baseRMWLVarianceMinus += _baseRMWLVarianceMinus;

                        //double margin = _baseRMVirtualTO == 0 ? 0 : _baseRMVirtualTotal / _baseRMVirtualTO * 100;
                        #region wl detail
                        winloss.WinLoss.Add(
                            new TreeWinLossDetail()
                            {
                                Id = string.Format("{0}", winlossDetail[IDMap]),
                                Title = string.Format("{0}", winlossDetail["username"]),
                                Turnover = CastAide.AsDouble(winlossDetail["turnover"]),
                                WinLoss = CastAide.AsDouble(winlossDetail["winloss"]),
                                Comm = CastAide.AsDouble(winlossDetail["commission"]),
                                Total = CastAide.AsDouble(winlossDetail["total"]),

                                BaseRMTurnover = _baseRMTurnover,
                                BaseRMTotal = _baseRMTotal,
                                BaseRMTOVariancePlus = _baseRMTOVariancePlus,
                                BaseRMTOVarianceMinus = _baseRMTOVarianceMinus,
                                BaseRMWLVariancePlus = _baseRMWLVariancePlus,
                                BaseRMWLVarianceMinus = _baseRMWLVarianceMinus,
                                BaseRMVirtualTotal = _baseRMVirtualTotal,
                                BaseRMVirtualTurnover = _baseRMVirtualTO,

                                //Margin = margin,

                                VirtualRate = CastAide.AsDouble(winlossDetail["virtualrate"]),
                            });
                        #endregion
                    }

                    #region Total
                    //double margin_total = baseRMVirtualTO == 0 ? 0 : baseRMVirtualTotal / baseRMVirtualTO * 100;
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
                        BaseRMTOVariancePlus = baseRMTOVariancePlus,
                        BaseRMTOVarianceMinus = baseRMTOVarianceMinus,
                        BaseRMWLVariancePlus = baseRMWLVariancePlus,
                        BaseRMWLVarianceMinus = baseRMWLVarianceMinus,
                        BaseRMVirtualTotal = baseRMVirtualTotal,
                        BaseRMVirtualTurnover = baseRMVirtualTO,
                        //Margin = margin_total
                    };
                    #endregion
                }

                winloss.WinLoss = winloss.WinLoss.OrderBy(c => c.Title).ToList();
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
                        getSession = GetTreeWinLossReport(fromDate,
                                                    toDate,
                                                    uplineId,
                                                    isShowSB,
                                                    isShowNG,
                                                    isShowRB,
                                                    isShowLCS,
                                                    isShowCS,
                                                    isShowP2P,
                                                    isShowBI,
                                                    isShowVS,
                                                    reportType);
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
                    startRow = 3;
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
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Value = "No";
                wsReport.Columns[startCol].Width = 255 * 10;

                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow + 1, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = reportType == 0 ? "Currency" : "Username";
                wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;
            }
            else
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow + 1, startCol).Value = reportType == 0 ? "Currency" : "Username";
                wsReport.Columns[startCol].Width = 255 * 20;
            }

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow + 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow + 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = "Win Loss";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow + 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = "Comm";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow + 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = "Total";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;
            #region base RM group
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 8)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 8)).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 1), startRow - 1, startCol + (nextCol + 1 + 8)).Value = "Base RM";

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "Turnover";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[5].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "Virtual Turnover";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[6].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow, startCol + (nextCol + 2)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow, startCol + (nextCol + 2)).Value = "Turnover Variance";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow, startCol + (nextCol + 2)).Merged = true;
            wsReport.Columns[7].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Value = "+";
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Merged = true;

            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Value = "-";
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Merged = true;
            wsReport.Columns[8].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol), startRow, startCol + (nextCol + 1)).Value = "Winlost Variance";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol), startRow, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[9].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Value = "+";
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol), startRow + 1, startCol + (nextCol)).Merged = true;

            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "-";
            wsReport.Cells.GetSubrangeAbsolute(startRow + 1, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[10].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "Total";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[11].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "Virtual Total";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[12].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol += 1), startRow + 1, startCol + (nextCol + 1)).Style = bgHeaderFormat; ;
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Value = "Margin (%)";
            wsReport.Cells.GetSubrangeAbsolute(startRow, startCol + (nextCol + 1), startRow + 1, startCol + (nextCol + 1)).Merged = true;
            wsReport.Columns[13].Width = 255 * 15;

            #endregion

            #region virtual rate
            if (reportType > 0)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 2 ), startRow + 1, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow + 1, startCol + nextCol).Value = "Virtual Rate (%)";
                wsReport.Columns[startCol + (nextCol)].Width = 255 * 10;

                nextCol--;                
            }
            #endregion

            #region header title
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol +=1)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + nextCol).Value = title;
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
                string turnoverVirtual = string.Empty;

                var baseRMTotal = CastAide.AsDouble(winlossDetail.BaseRMTotal);
                var baseRMVariance = CastAide.AsDouble(winlossDetail.BaseRMTOVariancePlus);
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
                    turnover = NumberFormat.DoubleFormat(winlossDetail.BaseRMTurnover, 2);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.BaseRMTurnover);

                    turnoverVirtual = NumberFormat.DoubleFormat(winlossDetail.BaseRMVirtualTurnover, 2);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winlossDetail.BaseRMVirtualTurnover);
                }
                else
                {
                    turnover = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = turnover;

                    turnoverVirtual = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = turnoverVirtual;
                }

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMTOVariancePlus,2));

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMTOVarianceMinus,2));

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMWLVariancePlus,2));
                if (winlossDetail.Total > 0 && CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMWLVariancePlus,2)) != 0)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Color = Color.Red;
                }

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat; 
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMWLVarianceMinus,2));
                if (winlossDetail.Total > 0 && CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMWLVarianceMinus, 2)) != 0) 
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Color = Color.Red;
                }
                

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMTotal, 2));

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.BaseRMVirtualTotal, 2));

                if (turnoverVirtual == "N/A" || turnoverVirtual == "0.00") 
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellSpecialNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value =  CastAide.AsDouble(NumberFormat.DoubleFormat(winlossDetail.Margin,2));
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
                    turnover = NumberFormat.DoubleFormat(winloss.Total.BaseRMVirtualTurnover, 2);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTurnover);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTurnover);
                }
                else
                {
                    turnover = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                    turnover = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
               
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTOVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTOVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMWLVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMWLVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTotal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTotal);

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
                string turnoverVirtualTotal = string.Empty;
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

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTurnover); 
                    turnoverVirtualTotal = NumberFormat.DoubleFormat(winloss.Total.BaseRMVirtualTurnover, 2);
                }
                else
                {
                    turnoverTotal = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                    turnoverVirtualTotal = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }

               
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTOVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTOVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMWLVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMWLVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMTotal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total.BaseRMVirtualTotal);

                if (turnoverVirtualTotal == "N/A" || turnoverVirtualTotal == "0.00") 
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

                    turnover8RM = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }

              
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMTOVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMTOVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMWLVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMWLVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMTotal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.Total8RM.BaseRMVirtualTotal);

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

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMVirtualTurnover);
                    turnoverBARM = NumberFormat.DoubleFormat(winloss.TotalBARM.BaseRMVirtualTurnover, 2); 
                }
                else
                {
                    turnoverBARM = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                    turnoverBARM = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }

              
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMTOVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMTOVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMWLVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMWLVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMTotal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.TotalBARM.BaseRMVirtualTotal);

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

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMVirtualTurnover);
                    turnoverGrandTotal = NumberFormat.DoubleFormat(winloss.GrandTotal.BaseRMVirtualTurnover, 2); 
                }
                else
                {
                    turnoverGrandTotal = "N/A";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "N/A";
                }
                
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMTOVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMTOVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMWLVariancePlus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMWLVarianceMinus);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMTotal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winloss.GrandTotal.BaseRMVirtualTotal);

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