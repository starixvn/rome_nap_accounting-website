// --------------------------------------------------------------------------
// <copyright file="DailyWinLoss2hOtherSportsService.cs" company="Nexcel Solutions Vietnam">
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
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Resource;
    using Fanex.Tarus.Reports.Constants;
    using Fanex.Tarus.Reports.DataAccess.DailyWinLoss2hOtherSports;
    using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
    using Sunplus.Accounting.Business.Report.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public partial class DailyWinLoss2hOtherSportsService : IDailyWinLoss2hOtherSportsService
    {
        #region IFields

        // GET: /WinLoss2hOtherSportsAccess/
        private readonly IDailyWinLoss2hOtherSportsAccess _dailyWinLoss2hOtherSportsAccess;

        private RMReportUtils _barmConfig = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);

        #endregion IFields
        #region IConstructions

        public DailyWinLoss2hOtherSportsService()
            : this(null)
        {
        }

        public DailyWinLoss2hOtherSportsService(IDailyWinLoss2hOtherSportsAccess dailyWinLoss2hOtherSportsAccess)
        {
            _dailyWinLoss2hOtherSportsAccess = dailyWinLoss2hOtherSportsAccess ?? new DailyWinLoss2hOtherSportsAccess();
        }

        #endregion IConstructions
        #region IMethods

        public DailyWinLoss2hReport DailyWLDirect_Sporttype(DailyWinLoss2hParam param)
        {
            var result = new DailyWinLoss2hReport
            {
                ReportDetails = new List<DailyWinLoss2hItem>(),
                ReportTotal = new DailyWinLoss2hReportTotal
                {
                    NormalTotal = new DailyWinLoss2hItem
                    {
                        Name = Labels.Total + ":"
                    },
                    EightRMTotal = new DailyWinLoss2hItem(),
                    BARMTotal = new DailyWinLoss2hItem(),
                    AllTotal = new DailyWinLoss2hItem
                    {
                        Name = Labels.All_Total + ":"
                    },
                    GrandTotal = new DailyWinLoss2hGrandTotal()
                },
                IsBARMActive = _barmConfig.IsActive
            };

            try
            {
                Hashtable outputParam = new Hashtable();
                double winlossEightRM = 0;
                double winlossBARM = 0;
                DataSet rs = _dailyWinLoss2hOtherSportsAccess.DailyWLDirect_Sporttype(param, ref outputParam);
                if (rs != null && rs.Tables.Count > 0 && rs.Tables[0].Rows.Count > 0)
                {
                    #region Declaration

                    DataTable dataTable = rs.Tables[0];
                    //dataTable.DefaultView.Sort = "SportType ASC, IBC ASC";

                    int currentSporttype = 0;
                    bool isHavingIBC = false;
                    int sportIndex = 0;
                    double excludeFromGrandTotal = 0;
                    DailyWinLoss2hItem sportTotal = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.SportTotal
                        },
                        Name = Labels.Total + ":"
                    };

                    var emptyRow = new DailyWinLoss2hItem
                    {
                        RowTypeDWL2h = new Entities.Sport.BaseModel
                        {
                            Id = RowTypeConstants.EmptyRow
                        }
                    };

                    if (dataTable.Rows.Count > 0)
                    {
                        currentSporttype = CastAide.AsInt32(dataTable.Rows[0]["SportType"]);
                    }

                    #endregion Declaration
                    #region ForEach
                    var dailyWinLoss2hItemX = new DailyWinLoss2hItem();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        dailyWinLoss2hItemX = new DailyWinLoss2hItem
                        {
                            Id = CastAide.AsInt32(row["SportType"]),
                            Name = CastAide.AsString(row["SportName"]),
                            WinLoss = CastAide.AsDouble(row["WinLost"]),
                            Position = CastAide.AsDouble(row["Position"]),
                            Commission = CastAide.AsDouble(row["Commission"]),
                            LineTotal = CastAide.AsDouble(row["RealLineTotal"]),
                            RowTypeDWL2h = new Entities.Sport.BaseModel(),
                            LineTotalVirtual = CastAide.AsDouble(row["VirtualLineTotal"]),

                            Turnover = CastAide.AsDouble(row["Turnover"]),
                            TurnoverVirtual = 100
                        };

                        dailyWinLoss2hItemX.RowTypeDWL2h.Id = GetRowTypeFromIBC(CastAide.AsInt32(row["ibc"]));
                        dailyWinLoss2hItemX.IsApplyVirtualRate = ReportService.IsApplyVirtualRate(dailyWinLoss2hItemX.Id);
                        if (dailyWinLoss2hItemX.RowTypeDWL2h.Id == RowTypeConstants.BARM && !_barmConfig.IsActive)
                        {
                            continue;
                        }

                        switch (dailyWinLoss2hItemX.RowTypeDWL2h.Id)
                        {
                            case RowTypeConstants.Normal:
                                break;
                            case RowTypeConstants.EightRM:
                                if (!isHavingIBC)
                                {
                                    isHavingIBC = true;
                                }

                                dailyWinLoss2hItemX.AliasName = dailyWinLoss2hItemX.Name + " 8RM";
                                break;
                            case RowTypeConstants.BARM:
                                if (!isHavingIBC)
                                {
                                    isHavingIBC = true;
                                }

                                dailyWinLoss2hItemX.AliasName = dailyWinLoss2hItemX.Name + " " + _barmConfig.DisplayName;
                                break;
                        }

                        dailyWinLoss2hItemX.Variance = ReportService.CalculateVariance(dailyWinLoss2hItemX.LineTotal,
                          dailyWinLoss2hItemX.LineTotalVirtual);
                        dailyWinLoss2hItemX.Margin = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotal,
                            dailyWinLoss2hItemX.Turnover) * 100;
                        dailyWinLoss2hItemX.MarginVirtual = ReportService.CalculateMargin(dailyWinLoss2hItemX.LineTotalVirtual,
                            dailyWinLoss2hItemX.TurnoverVirtual) * 100;

                        #region CalculateReportTotal

                        if (ReportService.IsSportBookSport(dailyWinLoss2hItemX.Id))
                        {
                            switch (dailyWinLoss2hItemX.RowTypeDWL2h.Id)
                            {
                                case RowTypeConstants.Normal:
                                    result.ReportTotal.NormalTotal.Add(dailyWinLoss2hItemX);
                                    break;
                                case RowTypeConstants.EightRM:
                                    result.ReportTotal.EightRMTotal.Add(dailyWinLoss2hItemX);
                                    winlossEightRM += dailyWinLoss2hItemX.LineTotal;
                                    break;
                                case RowTypeConstants.BARM:
                                    result.ReportTotal.BARMTotal.Add(dailyWinLoss2hItemX);
                                    winlossBARM += dailyWinLoss2hItemX.LineTotal;
                                    break;
                            }
                        }

                        #endregion CalculateReportTotal
                        #region SportTypeChangeProccess

                        if (dailyWinLoss2hItemX.Id != currentSporttype)
                        {
                            sportIndex++;

                            if (isHavingIBC)
                            {
                                result.IsHavingIBC = true;
                                sportTotal.Round();
                                result.ReportDetails.Add(sportTotal);
                                isHavingIBC = false;
                            }

                            result.ReportDetails.Add(emptyRow);

                            #region InitSportTotal

                            if (IsExcludeFromGrandTotal(currentSporttype))
                            {
                                excludeFromGrandTotal += sportTotal.LineTotal;
                            }

                            sportTotal = new DailyWinLoss2hItem
                            {
                                RowTypeDWL2h = new Entities.Sport.BaseModel
                                {
                                    Id = RowTypeConstants.SportTotal
                                },
                                Name = Labels.Total + ":"
                            };

                            if (dailyWinLoss2hItemX.RowTypeDWL2h.Id != RowTypeConstants.BARM || _barmConfig.IsActive)
                            {
                                sportTotal.Clone(dailyWinLoss2hItemX);
                            }

                            #endregion InitSportTotal

                            currentSporttype = dailyWinLoss2hItemX.Id;
                        }
                        else
                        {
                            if (dailyWinLoss2hItemX.RowTypeDWL2h.Id != RowTypeConstants.BARM || _barmConfig.IsActive)
                            {
                                sportTotal.Add(dailyWinLoss2hItemX);
                            }
                        }

                        dailyWinLoss2hItemX.RowIndex = sportIndex;
                        dailyWinLoss2hItemX.Round();
                        result.ReportDetails.Add(dailyWinLoss2hItemX);

                        #endregion SportTypeChangeProccess
                    }

                    #endregion ForEach
                    #region TotalOfLastSport

                    sportIndex++;
                    if (isHavingIBC)
                    {
                        result.IsHavingIBC = true;
                        sportTotal.Variance = ReportService.CalculateVariance(sportTotal.LineTotal,
                           sportTotal.LineTotalVirtual);
                        sportTotal.Round();
                        result.ReportDetails.Add(sportTotal);
                        isHavingIBC = false;
                    }

                    result.ReportDetails.Add(emptyRow);
                    if (IsExcludeFromGrandTotal(currentSporttype))
                    {
                        excludeFromGrandTotal += sportTotal.LineTotal;
                    }

                    currentSporttype = dailyWinLoss2hItemX.Id;

                    #endregion TotalOfLastSport

                    result.ReportTotal.NormalTotal.Margin =
                        ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotal,
                                                      result.ReportTotal.NormalTotal.Turnover) * 100;
                    result.ReportTotal.NormalTotal.MarginVirtual =
                        ReportService.CalculateMargin(result.ReportTotal.NormalTotal.LineTotalVirtual,
                                                      result.ReportTotal.NormalTotal.TurnoverVirtual) * 100;

                    result.ReportTotal.AllTotal = CalculateAllTotal(result.ReportTotal.NormalTotal,
                                                                    result.ReportTotal.EightRMTotal,
                                                                    result.ReportTotal.BARMTotal);
                    CalculateReportGrandTotal(ref result, outputParam, winlossEightRM, winlossBARM, excludeFromGrandTotal);

                    result.ReportTotal.NormalTotal.Round();
                    result.ReportTotal.AllTotal.Round();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        #region ExportToExcel

        public void ExportToExcel(DailyWinLoss2hParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = DailyWLDirect_Sporttype(param);
            var excelGemboxUtils = new ExcelGemboxUtils();

            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);
            var bgLineTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(130, 199, 255), true, true);
            var bgTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, true, true);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", Color.FromArgb(253, 252, 252), false, false);

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowCommonStyle = excelGemboxUtils.GetStyleText(205, 222, 243);
            var evenRowFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRowTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, true, true);
            var evenRowNumberDefaultFormat = GetStyleIntNumberLiveCasino(".00", evenRowColor, false, false);

            var ibcSuffix = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IBCSuffix");
            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            var maxColIndex = 10;

            #endregion ExcelFormat

            var reportHeaderTitle = param.ReportTitle;
            ExportMainReportHeader(ref wsReport, startCol, ref currentRow, reportHeaderTitle);

            #region Body

            if (dml2hReport != null && dml2hReport.ReportDetails.Count > 0)
            {
                foreach (var item in dml2hReport.ReportDetails)
                {
                    var isEvenRow = item.RowIndex % 2 == 0;
                    switch (item.RowTypeDWL2h.Id)
                    {
                        case RowTypeConstants.Normal:
                            currentCol = startCol;
                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowCommonStyle : commonStyle);
                            currentCell.Value = item.Name;
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.WinLoss);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.Position);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.Commission);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.LineTotal);
                            currentCol++;

                            if (item.IsApplyVirtualRate)
                            {
                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.LineTotalVirtual);
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.Variance);
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.Turnover);
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.TurnoverVirtual);
                                currentCol++;
                            }
                            else
                            {
                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = "N/A";
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowFormat : bgFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = "N/A";
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.Turnover);
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowNumberDefaultFormat : numberDefaultFormat);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = "N/A";
                                currentCol++;
                            }


                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowFormatPercent : bgFormatPercent);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (CastAide.AsDouble(item.Turnover) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(item.Margin) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowFormatPercent : bgFormatPercent);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (CastAide.AsDouble(item.TurnoverVirtual) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(item.MarginVirtual) / 100;
                            }
                            else
                            {
                                currentCell.Value = "N/A";
                            }

                            currentCol++;
                            currentRow++;
                            break;
                        case RowTypeConstants.EightRM:
                            currentCol = startCol;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = commonStyle;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                            currentCell.Value = item.AliasName;
                            currentCol++;

                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Merged = true;
                            currentCol += 3;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.LineTotal);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(item.LineTotalVirtual);
                            currentCol++;

                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 4);
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Merged = true;
                            currentCol += 5;
                            currentRow++;
                            break;
                        case RowTypeConstants.BARM:
                            if (_barmConfig.IsActive)
                            {
                                currentCol = startCol;
                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = commonStyle;
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                                currentCell.Value = item.AliasName;
                                currentCol++;

                                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                                currentCellRange.Style = commonStyle;
                                currentCellRange.Merged = true;
                                currentCol += 3;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = bgFormat;
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.LineTotal);
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = bgFormat;
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = CastAide.AsDecimal(item.LineTotalVirtual);
                                currentCol++;

                                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 4);
                                currentCellRange.Style = commonStyle;
                                currentCellRange.Merged = true;
                                currentCol += 5;
                                currentRow++;
                            }
                            break;
                        case RowTypeConstants.SportTotal:
                            break;
                        case RowTypeConstants.EmptyRow:
                            this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);
                            break;
                    }
                }

                ExportMainReportFooter(ref wsReport, dml2hReport, startCol, ref currentRow);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + maxColIndex);
            }

            #endregion Body

            ReportService.RespondStream(excelFile, reportName);
        }

        private void ExportMainReportHeader(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            CellRange currentCellRange;
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
            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + maxColIndex);
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
            currentCellRange.Value = "SportType";
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

        private void ExportMainReportFooter(ref ExcelWorksheet wsReport,
            DailyWinLoss2hReport report,
            int startCol,
            ref int currentRow)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = GetStyleIntNumberLiveCasino(".00", Color.FromArgb(253, 252, 252), false, false);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);
            var currentCol = startCol;
            ExcelCell currentCell;
            var total = report.ReportTotal;

            #region GrandTotal

            var grandTotal = total.GrandTotal;
            currentCol = startCol;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = Labels.Grand_Total.ToUpper() + ":";
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(grandTotal.Real);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(grandTotal.Virtual);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(grandTotal.Variance);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCol++;
            currentRow++;

            #endregion GrandTotal
        }

        private CellStyle GetStyleIntNumberLiveCasino(string symbol,
            Color bgColor,
            bool isBold,
            bool isPossitiveColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            //style.NumberFormat = string.Format("{0}#,##0{1}_);[<0]-#,##0{2};#,##0{3}", isPossitiveColor ? "[RED][>0]" : "", symbol, symbol, symbol);
            style.NumberFormat = string.Format("#,##0{0}_);-#,##0{1}", symbol, symbol);
            style.FillPattern.SetSolid(bgColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isBold)
            {
                style.Font.Weight = ExcelFont.BoldWeight;
            }

            return style;
        }

        private CellStyle GetCommonStyleDWL2h()
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.FillPattern.SetSolid(Color.FromArgb(255, 255, 255));
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;

            return style;
        }

        private void ExportEmptyRow(ref ExcelWorksheet wsReport,
            ref int currentRow,
            int startColumn,
            int endColumn)
        {
            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startColumn, currentRow, endColumn);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var textStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            currentCellRange.Merged = true;
            currentCellRange.Style = textStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentRow++;
        }

        private void ExportEmptyReport(ref ExcelWorksheet wsReport,
            ref int currentRow,
            int startColumn,
            int endColumn)
        {
            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow,
                startColumn,
                currentRow,
                endColumn);

            var excelGemboxUtils = new ExcelGemboxUtils();
            var textStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            currentCellRange.Merged = true;
            currentCellRange.Value = Labels.EmptyReportMessage;
            currentCellRange.Style = textStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentRow++;
        }

        #endregion ExportToExcel

        private void CalculateReportGrandTotal(ref DailyWinLoss2hReport dailyWinLoss2hReport,
            Hashtable outputParam,
            double winlossEightRM,
            double winlossBARM,
            double excludeValue)
        {
            dailyWinLoss2hReport.ReportTotal.GrandTotal.Virtual = dailyWinLoss2hReport.ReportTotal.NormalTotal.WinLoss +
                                                            dailyWinLoss2hReport.ReportTotal.NormalTotal.Position +
                                                            dailyWinLoss2hReport.ReportTotal.NormalTotal.Commission +
                                                            dailyWinLoss2hReport.ReportTotal.EightRMTotal.LineTotalVirtual;
            if (_barmConfig.IsActive)
            {
                dailyWinLoss2hReport.ReportTotal.GrandTotal.Virtual += dailyWinLoss2hReport.ReportTotal.BARMTotal.LineTotalVirtual;
            }

            double linetotal_actual = CastAide.AsDouble(outputParam["linetotal"], 0); //Real
            double winLost_8rm_actual = winlossEightRM; //Real

            dailyWinLoss2hReport.ReportTotal.GrandTotal.Real = linetotal_actual + winLost_8rm_actual;
            if (_barmConfig.IsActive)
            {
                dailyWinLoss2hReport.ReportTotal.GrandTotal.Real += winlossBARM;
            }

            dailyWinLoss2hReport.ReportTotal.GrandTotal.Variance = ReportService.CalculateVariance(
                  dailyWinLoss2hReport.ReportTotal.GrandTotal.Real,
                  dailyWinLoss2hReport.ReportTotal.GrandTotal.Virtual);

            dailyWinLoss2hReport.ReportTotal.GrandTotal.Variance -= excludeValue;
        }

        private DailyWinLoss2hItem CalculateAllTotal(DailyWinLoss2hItem normalTotal,
            DailyWinLoss2hItem ibcTotal,
            DailyWinLoss2hItem barmTotal)
        {
            DailyWinLoss2hItem allTotal = new DailyWinLoss2hItem();
            allTotal.Add(normalTotal);
            allTotal.Add(ibcTotal);

            if (_barmConfig.IsActive)
            {
                allTotal.Add(barmTotal);
            }

            allTotal.Margin = ReportService.CalculateMargin(allTotal.LineTotal, allTotal.Turnover) * 100;
            allTotal.MarginVirtual = ReportService.CalculateMargin(allTotal.LineTotalVirtual, allTotal.TurnoverVirtual) * 100;

            return allTotal;
        }

        private int GetRowTypeFromIBC(int ibcValue)
        {
            return ibcValue;
        }

        private bool IsExcludeFromGrandTotal(int sporttype)
        {
            return sporttype == Constants.Sports.HorseRacing ||
                   sporttype == Constants.Sports.Casino ||
                   sporttype == Constants.Sports.BingoGame ||
                   sporttype == Constants.Sports.LiveCasino;
        }

        private string GetBetTypeName(DailyWinLoss2hItem dwl2hItem)
        {
            string bettypeName = dwl2hItem.Bettype.Name;

            if (string.IsNullOrEmpty(dwl2hItem.Bettype.Name))
            {
                switch (dwl2hItem.Bettype.Id)
                {
                    case 71:
                        bettypeName = "Casino";
                        break;
                    case 72:
                        bettypeName = "P2P";
                        break;
                    case 252:
                        bettypeName = "IBCbet Bingo";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(dwl2hItem.Hedge) &&
                dwl2hItem.Hedge.Contains("(hedge)"))
            {
                switch (dwl2hItem.Bettype.Id)
                {
                    case 1:
                        bettypeName = "HDP (hedge)";
                        break;
                    case 3:
                        bettypeName = "OU (hedge)";
                        break;
                    case 7:
                        bettypeName = "1st HDP (hedge)";
                        break;
                    case 8:
                        bettypeName = "1st OU (hedge)";
                        break;
                }
            }
            else
            {
                switch (dwl2hItem.Bettype.Id)
                {
                    case 1:
                        bettypeName = "HDP";
                        break;
                    case 3:
                        bettypeName = "OU";
                        break;
                    case 4:
                        bettypeName = "CS";
                        break;
                    case 30:
                        bettypeName = "1st CS";
                        break;
                    case 6:
                        bettypeName = "TG";
                        break;
                    case 7:
                        bettypeName = "1st HDP";
                        break;
                    case 8:
                        bettypeName = "1st OU";
                        break;
                    case 14:
                        bettypeName = "FG/LG";
                        break;
                    case 1001:
                        bettypeName = "Kick Off";
                        break;
                    default:
                        break;
                }
            }

            return bettypeName;
        }

        private int SortReportDetail(DailyWinLoss2hItem dwlA, DailyWinLoss2hItem dwlB)
        {
            if (dwlA.GroupName.StartsWith("*") && !dwlB.GroupName.StartsWith("*"))
            {
                return -1;
            }

            if (!dwlA.GroupName.StartsWith("*") && dwlB.GroupName.StartsWith("*"))
            {
                return 1;
            }

            if (dwlA.GroupName == dwlB.GroupName &&
                dwlA.GroupId != dwlB.GroupId)
            {
                return dwlA.GroupId.CompareTo(dwlB.GroupId);
            }

            if (dwlA.GroupName == dwlB.GroupName)
            {
                if (dwlA.RowTypeDWL2h.Id == dwlB.RowTypeDWL2h.Id)
                {
                    if (dwlA.MatchInfo != null && dwlB.MatchInfo != null)
                    {
                        return string.Compare(dwlA.MatchInfo.Name, dwlB.MatchInfo.Name);
                    }
                    return 0;
                }

                if (dwlA.RowTypeDWL2h.Id == RowTypeConstants.LeagueHeadLine)
                {
                    return -1;
                }
                if (dwlB.RowTypeDWL2h.Id == RowTypeConstants.LeagueHeadLine)
                {
                    return 1;
                }
                return dwlA.RowTypeDWL2h.Id.CompareTo(dwlB.RowTypeDWL2h.Id);
            }

            return dwlA.GroupName.CompareTo(dwlB.GroupName);
        }

        #endregion IMethods
    }
}
