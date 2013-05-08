// --------------------------------------------------------------------------
// <copyright file="LiveCasinoReport_DWL2hService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Business.DailyWinLoss2h
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Resource;
    using Fanex.Tarus.Reports.Constants;
    using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public partial class DailyWinLoss2hService : IDailyWinLoss2hService
    {
        #region LiveCasinoReport

        public DailyWinLoss2hReport SelLiveCasinoReport(DailyWinLoss2hParam param)
        {
            var result = new DailyWinLoss2hReport
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
            };

            try
            {
                #region Declaration

                DataTable dataTable = _dailyWinLoss2hAccess.SelLiveCasinoReport(param);

                var emptyRow = new DailyWinLoss2hItem
                {
                    RowTypeDWL2h = new Entities.Sport.BaseModel
                    {
                        Id = RowTypeConstants.EmptyRow
                    }
                };

                var rowIndex = 0;

                #endregion Declaration

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    #region ForEach

                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dailyWinLoss2hItemX = new DailyWinLoss2hItem
                        {
                            Id = CastAide.AsInt32(row["Bettype"]),
                            Name = CacheRef.GetBettype(CastAide.AsInt32(row["Bettype"])),
                            WinLoss = CastAide.AsDouble(row["WinLost"]),
                            Position = CastAide.AsDouble(row["Position"]),
                            Commission = CastAide.AsDouble(row["Commission"]),
                            LineTotal = CastAide.AsDouble(row["LineTotal"]),
                            Turnover = CastAide.AsDouble(row["Turnover"]),

                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.Normal
                            },
                            RowIndex = rowIndex
                        };
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;
                        result.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                        rowIndex++;
                        dailyWinLoss2hItemX.Round();
                        result.ReportDetails.Add(dailyWinLoss2hItemX);
                    }

                    #endregion ForEach

                    result.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotal,
                        result.ReportTotal.NormalTotal.Turnover) * 100;
                    result.ReportTotal.NormalTotal.Round();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public void ExportExcel_LiveCasinoMain(DailyWinLoss2hParam param)
        {
            try
            {
                DailyWinLoss2hReport report = SelLiveCasinoReport(param);
                const int startCol = 0;
                var currentRow = 0;
                var reportName = param.ReportFileName.Replace(" ", string.Empty);
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var excelGemboxUtils = new ExcelGemboxUtils();
                var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

                wsReport.Columns[startCol].Width = 20 * 256;
                wsReport.Columns[startCol + 1].Width = 15 * 256;
                wsReport.Columns[startCol + 2].Width = 15 * 256;
                ExportHeader_CasinoReport(ref wsReport, startCol, ref currentRow, param.ReportTitle);

                if (report != null && report.ReportDetails.Count > 0)
                {
                    Export2ExcelDailyWLDirectLiveCasinoContent(ref wsReport, startCol, ref currentRow, report);
                }
                else
                {
                    this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + 6);
                }

                ReportService.RespondStream(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void Export2ExcelDailyWLDirectLiveCasinoContent(ref ExcelWorksheet wsReport, int startCol,
            ref int currentRow, DailyWinLoss2hReport report)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(253, 252, 252);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", bgColor, false, false);
            var numberFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var percentFormat = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);
            var numberDefaultFormatTotal = GetStyleIntNumberLiveCasino(".00", bgColor, true, false);
            var numberFormatTotal = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, true, true);
            var percentFormatTotal = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, true, true);
            var numberDefaultTotalFormatLineTotal = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(181, 196, 208), true, true);
            var allTotalSummary = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AllTotalSummary");
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowcommonStyle = excelGemboxUtils.GetStyleText(evenRowColor.R, evenRowColor.G, evenRowColor.B);
            var evenRowbgFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowbgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRowbgTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, true, true);
            var evenRownumberDefaultFormat = GetStyleIntNumberLiveCasino(".00", evenRowColor, false, false);

            ExcelCell currentCell;
            var currentCol = startCol;
            var isEvenRow = true;

            foreach (DailyWinLoss2hItem winLossDetail in report.ReportDetails)
            {
                currentCol = startCol;
                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                currentCell.Value = winLossDetail.Name;
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRowbgFormat : numberFormat);
                currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.WinLoss), 2);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.Position), 2);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.Commission), 2);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRowbgFormat : numberFormat);
                currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.LineTotal), 2);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.Turnover), 2);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = (isEvenRow ? evenRowbgFormatPercent : percentFormat);
                if (Math.Round(CastAide.AsDouble(winLossDetail.Turnover), 2) != 0)
                {
                    currentCell.Value = Math.Round(CastAide.AsDouble(winLossDetail.Margin), 2) / 100;
                }
                else
                {
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = "N/A";
                }

                currentCol++;
                currentRow++;
                isEvenRow = !isEvenRow;
            }

            this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + 6);

            #region TotalRow

            currentCol = startCol;
            var normalTotal = report.ReportTotal.NormalTotal;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberFormatTotal;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = normalTotal.Name;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberFormatTotal;
            currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.WinLoss), 2);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormatTotal;
            currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.Position), 2);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormatTotal;
            currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.Commission), 2);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormatLineTotal;
            currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.LineTotal), 2);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormatTotal;
            currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.Turnover), 2);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = percentFormatTotal;
            if (CastAide.AsDouble(normalTotal.Turnover) != 0)
            {
                currentCell.Value = Math.Round(CastAide.AsDouble(normalTotal.Margin), 2) / 100;
            }
            else
            {
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                currentCell.Value = "N/A";
            }

            currentCol++;

            #endregion TotalRow
        }

        #endregion LiveCasinoReport
    }
}