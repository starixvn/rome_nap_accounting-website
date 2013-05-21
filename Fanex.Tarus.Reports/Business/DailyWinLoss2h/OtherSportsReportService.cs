// --------------------------------------------------------------------------
// <copyright file="DailyWinLoss2hOtherSportsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Business.DailyWinLoss2h
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Resource;
    using Fanex.Tarus.Reports.Constants;
    using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
    using Fanex.Tarus.Reports.Entities.Sport;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public partial class DailyWinLoss2hService : IDailyWinLoss2hService
    {
        #region OtherSportsReport

        public LiveAndNoneLiveReportDWL2h SelOtherSportsReport(DailyWinLoss2hParam param)
        {
            var result = new LiveAndNoneLiveReportDWL2h
           {
               NoneLiveReport = new DailyWinLoss2hReport
               {
                   ReportDetails = new List<DailyWinLoss2hItem>(),
                   ReportTotal = new DailyWinLoss2hReportTotal
                   {
                       NormalTotal = new DailyWinLoss2hItem
                       {
                           Name = Labels.All_Total + ":"
                       },
                       EightRMTotal = new DailyWinLoss2hItem(),
                       BARMTotal = new DailyWinLoss2hItem(),
                       AllTotal = new DailyWinLoss2hItem(),
                       GrandTotal = new DailyWinLoss2hGrandTotal()
                   },
                   IsBARMActive = _barmConfig.IsActive
               },
               LiveReport = new DailyWinLoss2hReport
               {
                   ReportDetails = new List<DailyWinLoss2hItem>(),
                   ReportTotal = new DailyWinLoss2hReportTotal
                   {
                       NormalTotal = new DailyWinLoss2hItem
                       {
                           Name = Labels.All_Total + ":"
                       },
                       EightRMTotal = new DailyWinLoss2hItem(),
                       BARMTotal = new DailyWinLoss2hItem(),
                       AllTotal = new DailyWinLoss2hItem(),
                       GrandTotal = new DailyWinLoss2hGrandTotal()
                   },
                   IsBARMActive = _barmConfig.IsActive
               }
           };

            try
            {
                Hashtable outParam = new Hashtable();
                DataSet dbResult = _dailyWinLoss2hAccess.SelOtherSportsReport(param, ref outParam);
                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    #region Declaration

                    DataTable dtReport = dbResult.Tables[0];
                    bool isHavingHedgeColumn = false;
                    if (dtReport.Columns.Contains("IsHedge"))
                    {
                        isHavingHedgeColumn = true;
                    }

                    var emptyRow = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.EmptyRow
                        }
                    };

                    DailyWinLoss2hItem sportTotal = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.SportTotal
                        }
                    };

                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();
                    DataRow[] rows = dtReport.Select(null, "LiveIndicator ASC, SportType ASC");

                    #endregion Declaration
                    #region ForEach

                    foreach (DataRow row in rows)
                    {
                        dailyWinLoss2hItemX = new DailyWinLoss2hItem
                        {
                            Id = CastAide.AsInt32(row["SportType"]),
                            Name = CacheRef.GetSportName(CastAide.AsInt32(row["SportType"])),
                            WinLoss = CastAide.AsDouble(row["WinLost"]),
                            Position = CastAide.AsDouble(row["Position"]),
                            Commission = CastAide.AsDouble(row["Commission"]),
                            LineTotal = CastAide.AsDouble(row["RealLineTotal"]),
                            LineTotalVirtual = CastAide.AsDouble(row["VirtualLineTotal"]),
                            IsLive = CastAide.AsBoolean(row["LiveIndicator"]),

                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.Normal
                            },

                            Turnover = CastAide.AsDouble(row["Turnover"]),
                            TurnoverVirtual = 100
                        };

                        if (isHavingHedgeColumn)
                        {
                            dailyWinLoss2hItemX.Hedge = CastAide.AsString(row["IsHedge"]);
                        }

                        dailyWinLoss2hItemX.AliasName = dailyWinLoss2hItemX.Name;
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;
                        dailyWinLoss2hItemX.MarginVirtual = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotalVirtual,
                            dailyWinLoss2hItemX.TurnoverVirtual) * 100;

                        dailyWinLoss2hItemX.IsApplyVirtualRate = ReportService.IsApplyVirtualRate(dailyWinLoss2hItemX.Id);
                        dailyWinLoss2hItemX.Variance = ReportService.CalculateVariance(
                            dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.LineTotalVirtual);
                        if (dailyWinLoss2hItemX.IsLive)
                        {
                            result.LiveReport.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.LiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                        else
                        {
                            result.NoneLiveReport.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.NoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                    }

                    #endregion ForEach

                    result.NoneLiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.NoneLiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.NoneLiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.NoneLiveReport.ReportTotal.NormalTotal.MarginVirtual = ReportService.CalculateMargin(result.NoneLiveReport.ReportTotal.NormalTotal.LineTotalVirtual,
                        result.NoneLiveReport.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.LiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.LiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.LiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.LiveReport.ReportTotal.NormalTotal.MarginVirtual = ReportService.CalculateMargin(result.LiveReport.ReportTotal.NormalTotal.LineTotalVirtual,
                        result.LiveReport.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.NoneLiveReport.ReportTotal.NormalTotal.Round();
                    result.LiveReport.ReportTotal.NormalTotal.Round();
                    if (dbResult.Tables.Count > 1)
                    {
                        DataTable dt8RM = dbResult.Tables[1];
                        result.LiveReport.ReportTotal.EightRMTotal.WinLoss = CastAide.AsDouble(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                        result.NoneLiveReport.ReportTotal.EightRMTotal.WinLoss = CastAide.AsDouble(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    }

                    if (dbResult.Tables.Count > 2)
                    {
                        DataTable dtBARM = dbResult.Tables[2];
                        result.LiveReport.ReportTotal.BARMTotal.WinLoss = CastAide.AsDouble(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        result.NoneLiveReport.ReportTotal.BARMTotal.WinLoss = CastAide.AsDouble(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public LiveAndNoneLiveReportDWL2h SelOtherSportsReportBySport(DailyWinLoss2hParam param)
        {
            var result = new LiveAndNoneLiveReportDWL2h
            {
                NoneLiveReport = new DailyWinLoss2hReport
                {
                    ReportDetails = new List<DailyWinLoss2hItem>(),
                    ReportTotal = new DailyWinLoss2hReportTotal
                    {
                        NormalTotal = new DailyWinLoss2hItem
                        {
                            Name = Labels.All_Total + ":"
                        },
                        EightRMTotal = new DailyWinLoss2hItem(),
                        BARMTotal = new DailyWinLoss2hItem(),
                        AllTotal = new DailyWinLoss2hItem(),
                        GrandTotal = new DailyWinLoss2hGrandTotal()
                    },
                    IsBARMActive = _barmConfig.IsActive
                },
                LiveReport = new DailyWinLoss2hReport
                {
                    ReportDetails = new List<DailyWinLoss2hItem>(),
                    ReportTotal = new DailyWinLoss2hReportTotal
                    {
                        NormalTotal = new DailyWinLoss2hItem
                        {
                            Name = Labels.All_Total + ":"
                        },
                        EightRMTotal = new DailyWinLoss2hItem(),
                        BARMTotal = new DailyWinLoss2hItem(),
                        AllTotal = new DailyWinLoss2hItem(),
                        GrandTotal = new DailyWinLoss2hGrandTotal()
                    },
                    IsBARMActive = _barmConfig.IsActive
                }
            };

            try
            {
                Hashtable outParam = new Hashtable();
                DataSet dbResult = _dailyWinLoss2hAccess.SelOtherSportsReportBySport(param, ref outParam);

                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    #region Declaration

                    DataTable dtReport = dbResult.Tables[0];
                    bool isHavingHedgeColumn = false;
                    if (dtReport.Columns.Contains("IsHedge"))
                    {
                        isHavingHedgeColumn = true;
                    }

                    var emptyRow = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.EmptyRow
                        }
                    };

                    DailyWinLoss2hItem sportTotal = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.SportTotal
                        }
                    };

                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();
                    //DataRow[] rows = dtReport.Select(null, "LiveIndicator ASC, Bettype ASC");

                    #endregion Declaration
                    #region ForEach

                    foreach (DataRow row in dtReport.Rows)
                    {
                        dailyWinLoss2hItemX = new DailyWinLoss2hItem
                        {
                            Id = CastAide.AsInt32(row["BetType"]),
                            Name = CacheRef.GetBettype(CastAide.AsInt32(row["BetType"])),
                            WinLoss = CastAide.AsDouble(row["WinLost"]),
                            Position = CastAide.AsDouble(row["Position"]),
                            Commission = CastAide.AsDouble(row["Commission"]),
                            LineTotal = CastAide.AsDouble(row["RealLineTotal"]),
                            LineTotalVirtual = CastAide.AsDouble(row["VirtualLineTotal"]),
                            IsLive = CastAide.AsBoolean(row["LiveIndicator"]),

                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.Normal
                            },

                            Turnover = CastAide.AsDouble(row["Turnover"]),
                            TurnoverVirtual = 100
                        };

                        if (isHavingHedgeColumn)
                        {
                            dailyWinLoss2hItemX.Hedge = CastAide.AsString(row["IsHedge"]);
                        }

                        dailyWinLoss2hItemX.Bettype = new BaseModel
                        {
                            Id = dailyWinLoss2hItemX.Id,
                            Name = dailyWinLoss2hItemX.Name
                        };

                        dailyWinLoss2hItemX.AliasName = GetBetTypeName(dailyWinLoss2hItemX);
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;
                        dailyWinLoss2hItemX.MarginVirtual = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotalVirtual,
                            dailyWinLoss2hItemX.TurnoverVirtual) * 100;

                        dailyWinLoss2hItemX.IsApplyVirtualRate = ReportService.IsApplyVirtualRate(dailyWinLoss2hItemX.Id);
                        dailyWinLoss2hItemX.Variance = ReportService.CalculateVariance(
                            dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.LineTotalVirtual);

                        if (dailyWinLoss2hItemX.IsLive)
                        {
                            result.LiveReport.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.LiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                        else
                        {
                            result.NoneLiveReport.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.NoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                    }

                    #endregion ForEach

                    result.NoneLiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.NoneLiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.NoneLiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.NoneLiveReport.ReportTotal.NormalTotal.MarginVirtual = ReportService.CalculateMargin(result.NoneLiveReport.ReportTotal.NormalTotal.LineTotalVirtual,
                       result.NoneLiveReport.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.LiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.LiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.LiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.LiveReport.ReportTotal.NormalTotal.MarginVirtual = ReportService.CalculateMargin(result.LiveReport.ReportTotal.NormalTotal.LineTotalVirtual,
                        result.LiveReport.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.NoneLiveReport.ReportTotal.NormalTotal.Round();
                    result.LiveReport.ReportTotal.NormalTotal.Round();
                    if (dbResult.Tables.Count > 1)
                    {
                        DataTable dt8RM = dbResult.Tables[1];
                        result.LiveReport.ReportTotal.EightRMTotal.WinLoss = CastAide.AsDouble(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                        result.NoneLiveReport.ReportTotal.EightRMTotal.WinLoss = CastAide.AsDouble(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    }

                    if (dbResult.Tables.Count > 2)
                    {
                        DataTable dtBARM = dbResult.Tables[2];
                        result.LiveReport.ReportTotal.BARMTotal.WinLoss = CastAide.AsDouble(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        result.NoneLiveReport.ReportTotal.BARMTotal.WinLoss = CastAide.AsDouble(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public void ExportToExcel_OtherSportsMain(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReport(param);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            if (dml2hReport != null)
            {
                ExportReport_OtherSportsMain(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
                this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + 10);
                ExportReport_OtherSportsMain(ref wsReport, dml2hReport.LiveReport, startCol, ref currentRow, param, true);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        public void ExportToExcel_OtherSportsMain_NoneLive(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReport(param);
            if (dml2hReport != null)
            {
                ExportReport_OtherSportsMain(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        public void ExportToExcel_OtherSportsMain_Live(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReport(param);

            if (dml2hReport != null)
            {
                ExportReport_OtherSportsMain(ref wsReport, dml2hReport.LiveReport, startCol, ref currentRow, param, true);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportReport_OtherSportsMain(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReport report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var reportHeaderTitle = string.Format("OTHER SPORTS {0} PRODUCT WIN/LOSS REPORT", isLive ? "\"LIVE\"" : "\"NON - LIVE\"");
            ExportHeader_SoccerMain(ref wsReport, startCol, ref currentRow, reportHeaderTitle);
            ExportBody_SoccerMain(ref wsReport, report, startCol, ref currentRow, param, isLive);
        }

        public void ExportToExcel_OtherSportsBySport(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReportBySport(param);

            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            if (dml2hReport != null)
            {
                ExportReport_OtherSportsBySport(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
                this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + 10);
                ExportReport_OtherSportsBySport(ref wsReport, dml2hReport.LiveReport, startCol, ref currentRow, param, true);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        public void ExportToExcel_OtherSportsBySport_NoneLive(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReportBySport(param);
            if (dml2hReport != null)
            {
                ExportReport_OtherSportsBySport(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        public void ExportToExcel_OtherSportsBySport_Live(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelOtherSportsReportBySport(param);
            if (dml2hReport != null)
            {
                ExportReport_OtherSportsBySport(ref wsReport, dml2hReport.LiveReport, startCol, ref currentRow, param, true);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportReport_OtherSportsBySport(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReport report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var reportHeaderTitle = string.Format("OTHER SPORTS|{0} {1} PRODUCT WIN/LOSS REPORT",
                param.SportTypeName.ToUpper(),
                isLive ? "\"LIVE\"" : "\"NON - LIVE\"");
            ExportHeader_SoccerMain(ref wsReport, startCol, ref currentRow, reportHeaderTitle);
            ExportBody_SoccerMain(ref wsReport, report, startCol, ref currentRow, param, isLive);
        }

        #endregion OtherSportsReport
    }
}