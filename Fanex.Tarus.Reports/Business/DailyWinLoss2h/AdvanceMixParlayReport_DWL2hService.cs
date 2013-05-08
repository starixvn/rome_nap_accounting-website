// --------------------------------------------------------------------------
// <copyright file="AdvanceMixParlayReport_DWL2hService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
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
    using Fanex.Tarus.Reports.Constants;
    using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public partial class DailyWinLoss2hService : IDailyWinLoss2hService
    {
        #region AdvanceMixParlayReport

        public LiveAndNoneLiveReportDWL2h SelAdvanceMixParlayReport(DailyWinLoss2hParam param)
        {
            var result = new LiveAndNoneLiveReportDWL2h
            {
                NoneLiveReport = new DailyWinLoss2hReport
                {
                    ReportDetails = new List<DailyWinLoss2hItem>(),
                    ReportTotal = new DailyWinLoss2hReportTotal
                    {
                        NormalTotal = new DailyWinLoss2hItem()
                    },
                    Livetype = new Entities.Sport.BaseModel
                    {
                        Id = LiveType.NoneLive,
                        Name = AccCommon.GetLiveName(LiveType.NoneLive)
                    }
                },
                LiveReport = new DailyWinLoss2hReport
                {
                    ReportDetails = new List<DailyWinLoss2hItem>(),
                    ReportTotal = new DailyWinLoss2hReportTotal
                    {
                        NormalTotal = new DailyWinLoss2hItem()
                    },
                    Livetype = new Entities.Sport.BaseModel
                    {
                        Id = LiveType.Live,
                        Name = AccCommon.GetLiveName(LiveType.Live)
                    }
                },
                MixLiveAndNoneLiveReport = new DailyWinLoss2hReport
                {
                    ReportDetails = new List<DailyWinLoss2hItem>(),
                    ReportTotal = new DailyWinLoss2hReportTotal
                    {
                        NormalTotal = new DailyWinLoss2hItem()
                    },
                    Livetype = new Entities.Sport.BaseModel
                    {
                        Id = LiveType.MixLiveAndNoneLive,
                        Name = AccCommon.GetLiveName(LiveType.MixLiveAndNoneLive)
                    }
                },
                ReportTotal = new DailyWinLoss2hReportTotal
                {
                    NormalTotal = new DailyWinLoss2hItem()
                }
            };

            try
            {
                DataTable dtReport = _dailyWinLoss2hAccess.SelAdvanceMixParlayReport(param);

                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    #region Declaration

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
                        },
                        LiveType = new Entities.Sport.BaseModel()
                    };

                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();
                    int currentSporttype = 0;

                    #endregion Declaration
                    #region ForEach

                    int liveType = -1;
                    var rows = dtReport.Select(null, "LiveType ASC, SportType ASC, Bettype ASC");
                    if (rows.Length > 0)
                    {
                        currentSporttype = CastAide.AsInt32(rows[0]["SportType"]);
                        sportTotal.LiveType.Id = CastAide.AsInt32(rows[0]["LiveType"]);
                    }

                    foreach (DataRow row in rows)
                    {
                        dailyWinLoss2hItemX = new DailyWinLoss2hItem
                        {
                            Id = CastAide.AsInt32(row["BetType"]),
                            Name = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(row["BetType"]), "EN"),
                            Status = CastAide.AsString(row["Status"]),
                            WinLoss = CastAide.AsDouble(row["WinLost"]),
                            Position = CastAide.AsDouble(row["Position"]),
                            Commission = CastAide.AsDouble(row["Commission"]),
                            LineTotal = CastAide.AsDouble(row["LineTotal"]),
                            Turnover = CastAide.AsDouble(row["Turnover"]),

                            RowTypeDWL2h = new Entities.Sport.BaseModel
                            {
                                Id = RowTypeConstants.Normal
                            },
                            Sporttype = new Entities.Sport.Sporttype
                            {
                                Id = CastAide.AsInt32(row["SportType"])
                            }
                        };

                        dailyWinLoss2hItemX.AliasName = dailyWinLoss2hItemX.Name + "(" + dailyWinLoss2hItemX.Status + ")";
                        dailyWinLoss2hItemX.Sporttype.Name = AccCommon.GetSportName(dailyWinLoss2hItemX.Sporttype.Id);
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;

                        #region SportTypeChangeProccess

                        liveType = CastAide.AsInt32(row["LiveType"]);

                        if (dailyWinLoss2hItemX.Sporttype.Id != currentSporttype || liveType != sportTotal.LiveType.Id)
                        {
                            sportTotal.Id = currentSporttype;
                            sportTotal.Name = AccCommon.GetSportName(currentSporttype);
                            sportTotal.Margin = ReportService.CalculateMargin(sportTotal.LineTotal,
                                sportTotal.Turnover) * 100;
                            switch (sportTotal.LiveType.Id)
                            {
                                case 0:
                                    result.NoneLiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                    sportTotal.Round();
                                    result.NoneLiveReport.ReportDetails.Add(sportTotal);
                                    break;
                                case 1:
                                    result.LiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                    sportTotal.Round();
                                    result.LiveReport.ReportDetails.Add(sportTotal);
                                    break;
                                case 2:
                                    result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                    sportTotal.Round();
                                    result.MixLiveAndNoneLiveReport.ReportDetails.Add(sportTotal);
                                    break;
                            }

                            sportTotal = new DailyWinLoss2hItem
                            {
                                RowTypeDWL2h = new Entities.Sport.BaseModel
                                {
                                    Id = RowTypeConstants.SportTotal
                                },
                                LiveType = new Entities.Sport.BaseModel
                                {
                                    Id = liveType
                                }
                            };

                            sportTotal.Clone(dailyWinLoss2hItemX);
                            currentSporttype = dailyWinLoss2hItemX.Sporttype.Id;
                        }
                        else
                        {
                            sportTotal.Add(dailyWinLoss2hItemX);
                        }

                        #endregion SportTypeChangeProccess

                        switch (liveType)
                        {
                            case LiveType.NoneLive:
                                dailyWinLoss2hItemX.Round();
                                result.NoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                                break;
                            case LiveType.Live:
                                dailyWinLoss2hItemX.Round();
                                result.LiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                                break;
                            case LiveType.MixLiveAndNoneLive:
                                dailyWinLoss2hItemX.Round();
                                result.MixLiveAndNoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                                break;
                        }
                    }

                    #endregion ForEach
                    #region TotalOfLastSport

                    if (liveType != -1)
                    {
                        sportTotal.Margin = ReportService.CalculateMargin(sportTotal.LineTotal,
                            sportTotal.Turnover) * 100;
                        sportTotal.Id = currentSporttype;
                        sportTotal.Name = AccCommon.GetSportName(currentSporttype);

                        switch (sportTotal.LiveType.Id)
                        {
                            case LiveType.NoneLive:
                                result.NoneLiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                sportTotal.Round();
                                result.NoneLiveReport.ReportDetails.Add(sportTotal);
                                break;
                            case LiveType.Live:
                                result.LiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                sportTotal.Round();
                                result.LiveReport.ReportDetails.Add(sportTotal);
                                break;
                            case LiveType.MixLiveAndNoneLive:
                                result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.Add(sportTotal);
                                sportTotal.Round();
                                result.MixLiveAndNoneLiveReport.ReportDetails.Add(sportTotal);
                                break;
                        }
                    }

                    #endregion TotalOfLastSport

                    result.NoneLiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.NoneLiveReport.ReportTotal.NormalTotal.LineTotal,
                         result.NoneLiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.LiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.LiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.LiveReport.ReportTotal.NormalTotal.Turnover) * 100;
                    result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.LineTotal,
                        result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.Turnover) * 100;

                    result.NoneLiveReport.ReportTotal.NormalTotal.Round();
                    result.LiveReport.ReportTotal.NormalTotal.Round();
                    result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal.Round();

                    #region CalculateReportTotal

                    result.ReportTotal.NormalTotal.Add(result.NoneLiveReport.ReportTotal.NormalTotal);
                    result.ReportTotal.NormalTotal.Add(result.LiveReport.ReportTotal.NormalTotal);
                    result.ReportTotal.NormalTotal.Add(result.MixLiveAndNoneLiveReport.ReportTotal.NormalTotal);
                    result.ReportTotal.NormalTotal.Margin = ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotal,
                        result.ReportTotal.NormalTotal.Turnover) * 100;
                    result.ReportTotal.NormalTotal.Round();

                    #endregion CalculateReportTotal
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public void Export2Excel_MixParlayAdvanceMain(DailyWinLoss2hParam param)
        {
            var report = SelAdvanceMixParlayReport(param);

            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName.Replace(" ", string.Empty);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            CellRange currentCellRange;
            var currentCol = startCol;

            #endregion ExcelFormat
            #region ReportHeader

            currentCol = startCol;
            wsReport.Columns[currentCol++].Width = 20 * 256;
            wsReport.Columns[currentCol++].Width = 15 * 256;
            wsReport.Columns[currentCol++].Width = 15 * 256;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, startCol + 8);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = param.ReportTitle;
            currentRow++;

            Export2Excel_MixParlayAdvanceMainHeader(ref wsReport, ref currentRow, startCol);

            #endregion ReportHeader

            if (report != null && (report.NoneLiveReport.ReportDetails.Count > 0 ||
                                   report.LiveReport.ReportDetails.Count > 0 ||
                                   report.MixLiveAndNoneLiveReport.ReportDetails.Count > 0))
            {
                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.LiveReport, Color.FromArgb(221, 217, 196));
                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.NoneLiveReport, Color.FromArgb(202, 240, 145));
                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.MixLiveAndNoneLiveReport, Color.FromArgb(197, 217, 241));

                Export2Excel_MixParlayAdvanceMainFooter(ref wsReport, startCol, currentRow, report.ReportTotal.NormalTotal);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + 8);
            }

            ReportService.RespondStream(excelFile, reportName);
        }

        private void Export2Excel_MixParlayAdvanceMainHeader(ref ExcelWorksheet wsReport,
            ref int currentRow, int startCol)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeaderNormalFormat = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var currentCol = startCol;
            ExcelCell currentCell;

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

            currentCol = startCol;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = string.Empty;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Sporttype";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Bettype";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "WinLoss";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Position";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Comm";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Line Total";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Turnover";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeaderNormalFormat;
            currentCell.Value = "Margin";
            currentCol++;
            currentRow++;
        }

        private void Export2Excel_MixParlayAdvanceMainContent(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hReport liveTypeReport,
            Color colorLiveFormat)
        {
            if (liveTypeReport.ReportDetails.Count == 0)
            {
                return;
            }

            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var numberDefaultSubFormat = excelGemboxUtils.GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(222, 210, 190), true, false, false);
            var numberDefaultTotalFormat = excelGemboxUtils.GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(33, 89, 103), true, false, false);

            var numberFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, true);
            var numberSubFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(222, 210, 190), true, true);
            var numberTotalFormat = excelGemboxUtils.GetStyleIntNumberTotal(".00", Color.FromArgb(33, 89, 103), true, true);

            var numberSportFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), true, true);
            var numberLiveFormat = excelGemboxUtils.GetStyleIntNumber(".00", colorLiveFormat, true, true);

            var percentFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(253, 252, 252), false, true);
            var percentSubFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(222, 210, 190), true, true);
            var percentTotalFormat = excelGemboxUtils.GetStyleIntNumberTotal(".00%", Color.FromArgb(33, 89, 103), true, true);

            ExcelCell currentCell;
            CellRange currentCellRange;

            var currentCol = startCol;
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow + liveTypeReport.ReportDetails.Count - 1, startCol);
            currentCellRange.Merged = true;
            currentCellRange.Style = numberLiveFormat;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = liveTypeReport.Livetype.Name;

            var sportRowSpan = 0;
            var lastRow = currentRow;
            foreach (DailyWinLoss2hItem reportDetail in liveTypeReport.ReportDetails)
            {
                currentCol = startCol;
                switch (reportDetail.RowTypeDWL2h.Id)
                {
                    case RowTypeConstants.Normal:
                        currentCol += 2;
                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = commonStyle;
                        currentCell.Value = reportDetail.AliasName;
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.WinLoss);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Position);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Commission);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.LineTotal);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Turnover);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = percentFormat;
                        if (CastAide.AsDouble(reportDetail.Turnover) != 0)
                        {
                            currentCell.Value = CastAide.AsDouble(reportDetail.Margin) / 100;
                        }
                        else
                        {
                            currentCell.Value = "N/A";
                        }

                        currentCol++;
                        currentRow++;
                        sportRowSpan++;
                        break;
                    case RowTypeConstants.SportTotal:
                        currentCol = startCol + 1;
                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = commonStyle;
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberSubFormat;
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        currentCell.Value = "Sub Total";
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberSubFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.WinLoss);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultSubFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Position);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultSubFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Commission);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberSubFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.LineTotal);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = numberDefaultSubFormat;
                        currentCell.Value = CastAide.AsDouble(reportDetail.Turnover);
                        currentCol++;

                        currentCell = wsReport.Cells[currentRow, currentCol];
                        currentCell.Style = percentSubFormat;
                        if (CastAide.AsDouble(reportDetail.Turnover) != 0)
                        {
                            currentCell.Value = CastAide.AsDouble(reportDetail.Margin) / 100;
                        }
                        else
                        {
                            currentCell.Value = "N/A";
                        }

                        currentRow++;

                        #region SporttypeHeader

                        var sportTypeCol = startCol + 1;
                        currentCellRange = wsReport.Cells.GetSubrangeAbsolute(lastRow, sportTypeCol, lastRow + sportRowSpan - 1, sportTypeCol);
                        currentCellRange.Merged = true;
                        currentCellRange.Style = numberSportFormat;
                        currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        currentCellRange.Value = string.Format("{0}", reportDetail.Name);
                        sportRowSpan = 0;
                        lastRow = currentRow;

                        #endregion SporttypeHeader

                        break;
                }

            }

            #region AllTotal

            var normalTotal = liveTypeReport.ReportTotal.NormalTotal;
            currentCol = startCol;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Value = string.Empty;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Style.HorizontalAlignment =
                HorizontalAlignmentStyle.Center;
            currentCell.Value = string.Empty;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Style.HorizontalAlignment =
               HorizontalAlignmentStyle.Center;
            currentCell.Style.Font.Color = Color.White;
            currentCell.Value = "Total";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberTotalFormat;
            currentCell.Value = CastAide.AsDouble(normalTotal.WinLoss);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Style.Font.Color = Color.White;
            currentCell.Value = CastAide.AsDouble(normalTotal.Position);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Style.Font.Color = Color.White;
            currentCell.Value = CastAide.AsDouble(normalTotal.Commission);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberTotalFormat;
            currentCell.Value = CastAide.AsDouble(normalTotal.LineTotal);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Style.Font.Color = Color.White;
            currentCell.Value = CastAide.AsDouble(normalTotal.Turnover);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = percentTotalFormat;
            if (CastAide.AsDouble(normalTotal.Turnover) != 0)
            {
                currentCell.Value = CastAide.AsDouble(normalTotal.Margin) / 100;
            }
            else
            {
                currentCell.Value = "N/A";
            }

            currentCol++;
            currentRow++;

            #endregion AllTotal
        }

        private void Export2Excel_MixParlayAdvanceMainFooter(ref ExcelWorksheet wsReport,
            int startCol, int currentRow, DailyWinLoss2hItem grandTotal)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var numberTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, true);
            var percentTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(218, 241, 187), true, true);

            var numberDefaultTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, false, false);
            var numberDefaultTotalFormatLineTotal = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(181, 196, 208), true, true);

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
            currentCellRange.Merged = true;
            currentCellRange.Style = numberTotalFormat;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = "Grand Total";
            currentCol += 3;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberTotalFormat;
            currentCell.Value = CastAide.AsDouble(grandTotal.WinLoss);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Value = CastAide.AsDouble(grandTotal.Position);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Value = CastAide.AsDouble(grandTotal.Commission);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormatLineTotal;
            currentCell.Value = CastAide.AsDouble(grandTotal.LineTotal);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultTotalFormat;
            currentCell.Value = CastAide.AsDouble(grandTotal.Turnover);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = percentTotalFormat;
            if (CastAide.AsDouble(grandTotal.Turnover) != 0)
            {
                currentCell.Value = CastAide.AsDouble(grandTotal.Margin) / 100;
            }
            else
            {
                currentCell.Value = "N/A";
            }

            currentCol++;
        }

        #endregion AdvanceMixParlayReport
    }
}