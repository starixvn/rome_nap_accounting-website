// --------------------------------------------------------------------------
// <copyright file="SoccerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Business.DailyWinLoss2hOtherSports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
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
    public partial class DailyWinLoss2hOtherSportsService : IDailyWinLoss2hOtherSportsService
    {
        #region SoccerReport

        public DailyWinLoss2hReport SelDailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            var result = new DailyWinLoss2hReport
            {
                ReportDetails = new List<DailyWinLoss2hItem>(),
                ReportTotal = new DailyWinLoss2hReportTotal
                {
                    NormalTotal = new DailyWinLoss2hItem
                    {
                        Name = CacheRef.GetBettype(param.BetType) + " " + Labels.Total + ":"
                    },
                    EightRMTotal = new DailyWinLoss2hItem(),
                    BARMTotal = new DailyWinLoss2hItem(),
                    AllTotal = new DailyWinLoss2hItem(),
                    GrandTotal = new DailyWinLoss2hGrandTotal()
                },
                IsBARMActive = _barmConfig.IsActive
            };

            try
            {
                #region Declaration

                Hashtable outParam = new Hashtable();
                var dbResult = _dailyWinLoss2hOtherSportsAccess.SelDailyWLDirectDetail(param, ref outParam);

                var emptyRow = new DailyWinLoss2hItem
                {
                    RowTypeDWL2h = new Entities.Sport.BaseModel
                    {
                        Id = RowTypeConstants.EmptyRow
                    }
                };

                #endregion Declaration

                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    DataTable dataTable = dbResult.Tables[0];
                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();
                    List<int> leagueIds = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["LeagueId"])).Distinct().ToList();

                    foreach (var leagueId in leagueIds)
                    {
                        var leagueHeadLine = new DailyWinLoss2hItem
                        {
                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.LeagueHeadLine
                            },
                            Id = leagueId,
                            Name = CacheRef.GetLeagueName(leagueId),
                            GroupId = leagueId
                        };
                        leagueHeadLine.GroupName = leagueHeadLine.Name;

                        DailyWinLoss2hItem sportTotal = new DailyWinLoss2hItem
                        {
                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.SportTotal
                            },
                            Name = Labels.Total + ":",
                            GroupName = leagueHeadLine.Name,
                            GroupId = leagueId
                        };

                        var rowIndex = 0;
                        result.ReportDetails.Add(leagueHeadLine);
                        DataRow[] rows = dataTable.Select("LeagueId='" + leagueId + "'");

                        #region ForEach

                        foreach (DataRow row in rows)
                        {
                            dailyWinLoss2hItemX = new DailyWinLoss2hItem
                            {
                                MatchInfo = new Match
                                {
                                    Id = CastAide.AsInt32(row["matchId"]),
                                    Name = CastAide.AsString(row["MatchCode"]),
                                    HomeTeam = new BaseModel
                                    {
                                        Id = CastAide.AsInt32(row["HomeId"]),
                                        Name = CacheRef.GetTeamName(CastAide.AsInt32(row["HomeId"]))
                                    },
                                    AwayTeam = new BaseModel
                                    {
                                        Id = CastAide.AsInt32(row["AwayId"]),
                                        Name = CacheRef.GetTeamName(CastAide.AsInt32(row["AwayId"]))
                                    },
                                    IsNeutral = CastAide.AsBoolean(row["IsNeutral"])
                                },

                                GroupName = leagueHeadLine.Name,
                                GroupId = leagueHeadLine.Id,
                                WinLoss = CastAide.AsDouble(row["WinLost"]),
                                Position = CastAide.AsDouble(row["Position"]),
                                Commission = CastAide.AsDouble(row["Commission"]),
                                LineTotal = CastAide.AsDouble(row["RealLineTotal"]),
                                LineTotalVirtual = CastAide.AsDouble(row["VirtualLineTotal"]),
                                RowTypeDWL2h = new Entities.Sport.BaseModel
                                {
                                    Id = RowTypeConstants.Normal
                                },
                                RowIndex = rowIndex,

                                Turnover = CastAide.AsDouble(row["RealTurnover"]),
                                TurnoverVirtual = CastAide.AsDouble(row["VirtualTurnover"])
                            };

                            dailyWinLoss2hItemX.Variance = ReportService.CalculateVariance(
                                dailyWinLoss2hItemX.LineTotal,
                                dailyWinLoss2hItemX.LineTotalVirtual);

                            dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(
                                dailyWinLoss2hItemX.LineTotal,
                                dailyWinLoss2hItemX.Turnover) * 100;
                            dailyWinLoss2hItemX.MarginVirtual = ReportService.CalculateMargin(
                                dailyWinLoss2hItemX.LineTotalVirtual,
                                dailyWinLoss2hItemX.TurnoverVirtual) * 100;

                            sportTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.ReportDetails.Add(dailyWinLoss2hItemX);
                            rowIndex++;
                        }

                        #endregion ForEach

                        result.ReportTotal.NormalTotal.Add(sportTotal);
                        if (rowIndex > 1)
                        {
                            sportTotal.Margin = ReportService.CalculateMargin(sportTotal.LineTotal,
                                sportTotal.Turnover) * 100;
                            sportTotal.MarginVirtual = ReportService.CalculateMargin(sportTotal.LineTotalVirtual,
                                sportTotal.TurnoverVirtual) * 100;

                            sportTotal.Round();
                            result.ReportDetails.Add(sportTotal);
                        }
                    }

                    result.ReportDetails.Sort(delegate(DailyWinLoss2hItem dwlA, DailyWinLoss2hItem dwlB)
                    {
                        return SortReportDetail(dwlA, dwlB);
                    });

                    result.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotal,
                             result.ReportTotal.NormalTotal.Turnover) * 100;
                    result.ReportTotal.NormalTotal.MarginVirtual = ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotalVirtual,
                             result.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.ReportTotal.EightRMTotal.WinLoss = CastAide.AsDouble(outParam["winlost_8rm"], 0);
                    result.ReportTotal.BARMTotal.WinLoss = CastAide.AsDouble(outParam["winlost_barm"], 0);
                    result.ReportTotal.NormalTotal.Round();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        private void ExportHeader_SoccerMain(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle)
        {
            var currentCol = startCol;
            ExcelCell currentCell;
            CellRange currentCellRange;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commStyle = GetCommonStyleDWL2h();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var maxColIndex = 10;

            currentCol = startCol;
            wsReport.Columns[currentCol++].Width = 6000;
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
            currentCellRange.Style = commStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = reportHeaderTitle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow++;

            //FirstRow
            currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = bgHeader;
            currentCellRange.Value = "Bettype";
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

        private void ExportBody_SoccerMain(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReport report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", Color.FromArgb(253, 252, 252), false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);
            var currentCol = startCol;

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowcommonStyle = excelGemboxUtils.GetStyleText(evenRowColor.R, evenRowColor.G, evenRowColor.B);
            var evenRowbgFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowbgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRowbgTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, true, true);
            var evenRownumberDefaultFormat = GetStyleIntNumberLiveCasino(".00", evenRowColor, false, false);
            ExcelCell currentCell;
            var maxColIndex = 10;

            if (report != null && report.ReportDetails.Count > 0)
            {
                var isEvenRow = true;
                foreach (var detail in report.ReportDetails)
                {
                    #region MainRow

                    currentCol = startCol;
                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                    currentCell.Value = detail.AliasName; currentCol++;

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

                    #endregion MainRow
                }

                this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);

                #region TotalRow

                var normalTotal = report.ReportTotal.NormalTotal;
                currentCol = startCol;
                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = commonStyle;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Value = normalTotal.Name;
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.WinLoss);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Position);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Commission);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Style.FillPattern.SetSolid(Color.FromArgb(181, 196, 208));
                currentCell.Value = CastAide.AsDecimal(normalTotal.LineTotal);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Style.FillPattern.SetSolid(Color.FromArgb(181, 196, 208));
                currentCell.Value = CastAide.AsDecimal(normalTotal.LineTotalVirtual);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Style.FillPattern.SetSolid(Color.FromArgb(181, 196, 208));
                currentCell.Value = CastAide.AsDecimal(normalTotal.Variance);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Turnover);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Value = CastAide.AsDecimal(normalTotal.TurnoverVirtual);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormatPercent;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (Math.Round(normalTotal.Turnover, 2) != 0)
                {
                    currentCell.Value = CastAide.AsDouble(normalTotal.Margin) / 100;
                }
                else
                {
                    currentCell.Value = "N/A";
                }

                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormatPercent;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (Math.Round(normalTotal.TurnoverVirtual, 2) != 0)
                {
                    currentCell.Value = CastAide.AsDouble(normalTotal.MarginVirtual) / 100;
                }
                else
                {
                    currentCell.Value = "N/A";
                }

                currentCol++;
                currentRow++;

                #endregion TotalRow

                ExportIBCTotalRow_SoccerMain(ref wsReport, report.ReportTotal, startCol, ref currentRow, param, isLive);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);
            }
        }

        private void ExportIBCTotalRow_SoccerMain(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReportTotal detail,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
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

            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var ibcTotalSummary = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IBCTotalSummary");
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            ExcelCell currentCel;
            CellRange currentCellRange;

            currentCel = wsReport.Cells[currentRow, currentCol];
            currentCel.Style = commonStyle;
            currentCel.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCel.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCel.Value = "IBC(8RM" +
                (_barmConfig.IsActive ? " + " + _barmConfig.DisplayName : "") + ") Total:";
            currentCol++;

            currentCel = wsReport.Cells[currentRow, currentCol];
            currentCel.Style = bgFormat;
            currentCel.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCel.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCel.Value = CastAide.AsDecimal(allIBCTotal);
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 8);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCol += 9;
            currentRow++;
        }

        public void ExportToExcel_DailyWLDirectDetails(DailyWinLoss2hParam param)
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
            var evenRownumberDefaultFormat = GetStyleIntNumberLiveCasino(".00", evenRowColor, false, false);

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            var isEvenRow = true;
            var maxColIndex = 13;

            #endregion ExcelFormat

            var reportHeaderTitle = param.ReportTitle;
            ExportHeader_DailyWLDirectDetails(ref wsReport, startCol, ref currentRow, reportHeaderTitle, param);

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

                            var eventDetail = string.Empty;
                            if (param.BetType == Constants.Bettype.Outright || param.SportType == Constants.Sports.Financials)
                            {
                                eventDetail = detail.MatchInfo.HomeTeam.Name + (detail.MatchInfo.IsNeutral ? " (N)" : "");
                                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                                currentCellRange.Merged = true;
                                currentCellRange.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                                currentCellRange.Value = eventDetail;
                                currentCol += 3;
                            }
                            else if (param.SportType != Constants.Sports.NumberGame)
                            {
                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = detail.MatchInfo.HomeTeam.Name;
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = "-vs-";
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                                currentCell.Value = detail.MatchInfo.AwayTeam.Name;
                                currentCol++;
                            }

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
                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
                            currentCellRange.Merged = true;
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Style.FillPattern.SetSolid(Color.FromArgb(124, 152, 164));
                            currentCol += 4;

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
                            isEvenRow = true;
                            break;

                            #endregion TotalRow
                        case RowTypeConstants.EmptyRow:
                            isEvenRow = true;
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
                            currentRow++;
                            isEvenRow = true;
                            break;

                            #endregion LeagueHeadLine
                    }
                }

                ExportNormalTotal_DailyWLDirectDetails(ref wsReport, dml2hReport.ReportTotal.NormalTotal, startCol, ref currentRow, param);
                ExportIBCTotal_DailyWLDirectDetails(ref wsReport, dml2hReport.ReportTotal, startCol, ref currentRow, param);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);
            }

            #endregion Body

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportHeader_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle,
            DailyWinLoss2hParam param)
        {
            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var maxColIndex = 13;

            currentCol = startCol;
            var teamNameColWidth = 4000;
            wsReport.Columns[currentCol++].Width = 3000;
            wsReport.Columns[currentCol++].Width = teamNameColWidth;
            wsReport.Columns[currentCol++].Width = 1000;
            wsReport.Columns[currentCol++].Width = teamNameColWidth;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;
            wsReport.Columns[currentCol++].Width = 3300;

            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, startCol + maxColIndex);
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

            if (param.SportType != Constants.Sports.NumberGame)
            {
                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow + 1, currentCol + 2);
                currentCellRange.Merged = true;
                currentCellRange.Value = "Event";
                currentCellRange.Style = bgHeader;
                currentCol += 3;
            }

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
            currentCol = startCol + 7;
            var currentCell = wsReport.Cells[currentRow, currentCol];
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

        private void ExportNormalTotal_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
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
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCellRange.Value =
                (param.IsLive != 0 ? "Live! " : "") + CacheRef.GetBettype(param.BetType) +
                (param.IsHedge ? " (Hegde)" : "") + " " + Labels.Total + ":";
            currentCol += 4;

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

        private void ExportIBCTotal_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
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

            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            ExcelCell currentCell;
            CellRange currentCellRange;

            var ibcTotalSummary = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IBCTotalSummary");
            var endCol = currentCol + 3;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCellRange.Value = "IBC(8RM" +
                (_barmConfig.IsActive ? " + " + _barmConfig.DisplayName : "") + ") Total:";
            currentCol += 4;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = CastAide.AsDecimal(allIBCTotal);
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 8);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCol += 9;
        }

        #endregion SoccerReport
    }
}