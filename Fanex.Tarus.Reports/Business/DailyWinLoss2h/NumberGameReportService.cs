// --------------------------------------------------------------------------
// <copyright file="NumberGameReportService.cs" company="Nexcel Solutions Vietnam">
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
    using System.Drawing;
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
        #region NumberGameReport

        public LiveAndNoneLiveReportDWL2h DailyWLDirect_NumberGame(DailyWinLoss2hParam param)
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
                    }
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
                    }
                }
            };

            try
            {
                Hashtable outParam = new Hashtable();
                DataSet ds = _dailyWinLoss2hAccess.DailyWLDirect_NumberGame(param, ref outParam);
                if (ds != null && ds.Tables.Count > 0)
                {
                    #region Declaration

                    DataTable dtReport = ds.Tables[0];
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

                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();

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

                        dailyWinLoss2hItemX.Bettype = new BaseModel
                        {
                            Id = dailyWinLoss2hItemX.Id,
                            Name = dailyWinLoss2hItemX.Name
                        };

                        if (isHavingHedgeColumn)
                        {
                            dailyWinLoss2hItemX.Hedge = CastAide.AsString(row["IsHedge"]);
                        }

                        dailyWinLoss2hItemX.AliasName = GetBetTypeName(dailyWinLoss2hItemX);
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;
                        dailyWinLoss2hItemX.MarginVirtual = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotalVirtual,
                            dailyWinLoss2hItemX.TurnoverVirtual) * 100;

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
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public void ExportToExcel_NumberGameMain(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = DailyWLDirect_NumberGame(param);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            if (dml2hReport != null)
            {
                ExportReport_NumberGameMain(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
                this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + 10);
                ExportReport_NumberGameMain(ref wsReport, dml2hReport.LiveReport, startCol, ref currentRow, param, true);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportReport_NumberGameMain(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReport report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var reportHeaderTitle = string.Format("NUMBER GAME {0} PRODUCT WIN/LOSS REPORT", isLive ? "\"LIVE\"" : "\"NON - LIVE\"");
            ExportHeader_SoccerMain(ref wsReport, startCol, ref currentRow, reportHeaderTitle);
            ExportBody_SoccerMain(ref wsReport, report, startCol, ref currentRow, param, isLive);
        }

        public void ExportToExcel_NumberGameDetails(DailyWinLoss2hParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelDailyWLDirectDetails(param);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", Color.FromArgb(253, 252, 252), false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowcommonStyle = excelGemboxUtils.GetStyleText(evenRowColor.R, evenRowColor.G, evenRowColor.B);
            var evenRowbgFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowbgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRowbgTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, true, true);
            var evenRownumberDefaultFormat = GetStyleIntNumberLiveCasino(".00", evenRowColor, false, false);

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            var isEvenRow = true;
            var maxColIndex = 10;

            #endregion ExcelFormat

            var reportHeaderTitle = param.ReportTitle;
            ExportHeader_NumberGameDetails(ref wsReport, startCol, ref currentRow, reportHeaderTitle, param);

            #region Body

            if (dml2hReport != null && dml2hReport.ReportDetails.Count > 0)
            {
                foreach (var detail in dml2hReport.ReportDetails)
                {
                    switch (detail.RowTypeDWL2h.Id)
                    {
                        case RowTypeConstants.Normal:
                            #region NormalRow

                            currentCol = startCol;
                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            currentCell.Value = detail.MatchInfo.Name;
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.WinLoss);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Position);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Commission);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotal);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotalVirtual);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Variance);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Turnover);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.TurnoverVirtual);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormatPercent : bgFormatPercent);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.Turnover, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.Margin) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormatPercent : bgFormatPercent);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.TurnoverVirtual, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.MarginVirtual) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;
                            currentRow++;
                            isEvenRow = !isEvenRow;
                            break;

                            #endregion NormalRow
                        case RowTypeConstants.SportTotal:
                            #region TotalRow

                            currentCol = startCol;
                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol);
                            currentCellRange.Merged = true;
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Style.FillPattern.SetSolid(Color.FromArgb(136, 136, 136));
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.WinLoss);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Position);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Commission);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotal);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotalVirtual);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Variance);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Turnover);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.TurnoverVirtual);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormatPercent;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.Turnover, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.Margin) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormatPercent;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.TurnoverVirtual, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.MarginVirtual) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;
                            currentRow++;
                            break;

                            #endregion TotalRow
                        case RowTypeConstants.EmptyRow:
                            break;
                        case RowTypeConstants.LeagueHeadLine:
                            #region LeagueHeadLine

                            currentCol = startCol;
                            currentCol++;
                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + maxColIndex - 1);
                            currentCellRange.Merged = true;
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
                            currentCellRange.Value = detail.Name;
                            currentCol += 10;
                            currentRow++;

                            break;
                            #endregion LeagueHeadLine
                    }
                }

                ExportNormalTotal_NumberGameDetails(ref wsReport, dml2hReport.ReportTotal.NormalTotal, startCol, ref currentRow, param);
                ExportIBCTotal_NumberGameDetails(ref wsReport, dml2hReport.ReportTotal, startCol, ref currentRow, param);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);
            }

            #endregion Body

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportHeader_NumberGameDetails(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle,
            DailyWinLoss2hParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var maxColIndex = 10;

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;
            wsReport.Columns[currentCol++].Width = 4000;

            currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, startCol + maxColIndex);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = reportHeaderTitle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow++;

            //FirstRow
            currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Code";
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "WinLoss";
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Position";
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Comm";
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 1);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Line Total";
            currentCol += 2;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Variance";
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 1);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Turnover";
            currentCol += 2;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 1);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Margin";
            currentCol += 2;
            currentRow++;

            //SecondRow
            currentCol = startCol + 4;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Real";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Virtual";
            currentCol++;

            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Real";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Virtual";
            currentCol++;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Real";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = "Virtual";
            currentCol++;
            currentRow++;
        }

        private void ExportNormalTotal_NumberGameDetails(ref ExcelWorksheet wsReport,
            DailyWinLoss2hItem total,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", Color.FromArgb(253, 252, 252), false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCellRange.Value =
                (param.IsLive != 0 ? "Live! " : "") + CacheRef.GetBettype(param.BetType) +
                (param.IsHedge ? " (Hegde)" : "") + " " + Labels.Total + ":";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.WinLoss);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Position);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Commission);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.LineTotal);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.LineTotalVirtual);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Variance);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Turnover);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.TurnoverVirtual);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormatPercent;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            if (Math.Round(total.Turnover, 2) != 0)
            {
                currentCell.Value = CastAide.AsDouble(total.Margin) / 100;
            }
            else
            {
                currentCell.Value = "N/A";
            }

            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormatPercent;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            if (Math.Round(total.TurnoverVirtual, 2) != 0)
            {
                currentCell.Value = CastAide.AsDouble(total.MarginVirtual) / 100;
            }
            else
            {
                currentCell.Value = "N/A";
            }

            currentCol++;
            currentRow++;
        }

        private void ExportIBCTotal_NumberGameDetails(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReportTotal detail,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param)
        {
            var allIBCTotal = detail.EightRMTotal.WinLoss;
            if (_barmConfig.IsActive)
            {
                allIBCTotal += detail.BARMTotal.WinLoss;
            }

            if (allIBCTotal == 0)
            {
                return;
            }

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var ibcTotalSummary = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IBCTotalSummary");

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCellRange.Value = "IBC(8RM" +
                (_barmConfig.IsActive ? " + " + _barmConfig.DisplayName : "") + ") Total:";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = CastAide.AsDecimal(allIBCTotal);
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 7);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCol += 8;
        }

        #endregion NumberGameReport
    }
}