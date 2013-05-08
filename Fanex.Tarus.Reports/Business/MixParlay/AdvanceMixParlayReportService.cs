
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Reports.Entities.MixParlay;
using Fanex.Tarus.Reports.Helpers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

namespace Fanex.Tarus.Reports.Business.MixParlay
{
    public partial class MixParlayReportService : IMixParlayReportService
    {
        public LiveAndNoneLiveReport SelAdvanceMixParlayReport(DateTime fromDate, DateTime toDate, bool isViewDW, int betType)
        {
            var result = new LiveAndNoneLiveReport
            {
                NoneLiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail()
                    },
                    LiveType = new Entities.Sport.BaseModel
                    {
                        Id = LivetypeConstants.NoneLive,
                        Name = AccCommon.GetLiveName(LivetypeConstants.NoneLive)
                    }
                },
                LiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail()
                    },
                    LiveType = new Entities.Sport.BaseModel
                    {
                        Id = LivetypeConstants.Live,
                        Name = AccCommon.GetLiveName(LivetypeConstants.Live)
                    }
                },
                MixLiveAndNoneLiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail()
                    },
                    LiveType = new Entities.Sport.BaseModel
                    {
                        Id = LivetypeConstants.MixLiveAndNoneLive,
                        Name = AccCommon.GetLiveName(LivetypeConstants.MixLiveAndNoneLive)
                    }
                },
                Total = new ReportTotal
                {
                    NormalTotal = new ReportDetail()
                }
            };

            try
            {
                DataTable dtReport = _mixParlayReportAccess.SelAdvanceMixParlayReport(fromDate, toDate, isViewDW, betType);

                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    #region Declaration

                    var emptyRow = new ReportDetail
                    {
                        RowType = new DisplayRowType
                        {
                            Id = RowType.EmptyRow
                        }
                    };

                    ReportDetail sportTotal = new ReportDetail
                    {
                        RowType = new DisplayRowType
                        {
                            Id = RowType.SportTotal
                        },
                        LiveType = new Entities.Sport.BaseModel()
                    };

                    var reportDetailX = new ReportDetail();
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
                        reportDetailX = new ReportDetail
                        {
                            Id = CastAide.AsInt32(row["BetType"]),
                            Name = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(row["BetType"]), "EN"),
                            Status = CastAide.AsString(row["Status"]),
                            WinLoss = CastAide.AsDecimal(row["WinLost"]),
                            Position = CastAide.AsDecimal(row["Position"]),
                            Commission = CastAide.AsDecimal(row["Commission"]),
                            LineTotal = CastAide.AsDecimal(row["LineTotal"]),
                            Turnover = CastAide.AsDecimal(row["Turnover"]),

                            RowType = new DisplayRowType
                            {
                                Id = RowType.Normal
                            },
                            SportType = new Entities.Sport.Sporttype
                            {
                                Id = CastAide.AsInt32(row["SportType"])
                            }
                        };
                        reportDetailX.AliasName = reportDetailX.Name + "(" + reportDetailX.Status + ")";
                        reportDetailX.SportType.Name = AccCommon.GetSportName(reportDetailX.SportType.Id);
                        reportDetailX.Margin = CalculateMargin(reportDetailX);

                        #region SportTypeChangeProccess

                        liveType = CastAide.AsInt32(row["LiveType"]);

                        if (reportDetailX.SportType.Id != currentSporttype || liveType != sportTotal.LiveType.Id)
                        {
                            sportTotal.Id = currentSporttype;
                            sportTotal.Name = AccCommon.GetSportName(currentSporttype);
                            sportTotal.Margin = CalculateMargin(sportTotal);
                            switch (sportTotal.LiveType.Id)
                            {
                                case 0:
                                    result.NoneLiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                    result.NoneLiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                    result.NoneLiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                    result.NoneLiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                    result.NoneLiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                    result.NoneLiveReport.ReportDetails.Add(sportTotal);
                                    break;
                                case 1:
                                    result.LiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                    result.LiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                    result.LiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                    result.LiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                    result.LiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                    result.LiveReport.ReportDetails.Add(sportTotal);
                                    break;
                                case 2:
                                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                    result.MixLiveAndNoneLiveReport.ReportDetails.Add(sportTotal);
                                    break;
                            }

                            #region InitSportTotal

                            sportTotal = new ReportDetail
                            {
                                RowType = new DisplayRowType
                                {
                                    Id = RowType.SportTotal
                                },
                                LiveType = new Entities.Sport.BaseModel
                                {
                                    Id = liveType
                                }
                            };

                            sportTotal.WinLoss = reportDetailX.WinLoss;
                            sportTotal.Position = reportDetailX.Position;
                            sportTotal.Commission = reportDetailX.Commission;
                            sportTotal.LineTotal = reportDetailX.LineTotal;
                            sportTotal.Turnover = reportDetailX.Turnover;

                            #endregion InitSportTotal
                            currentSporttype = reportDetailX.SportType.Id;
                        }
                        else
                        {
                            #region CalculateSportTotal

                            sportTotal.WinLoss += reportDetailX.WinLoss;
                            sportTotal.Position += reportDetailX.Position;
                            sportTotal.Commission += reportDetailX.Commission;
                            sportTotal.LineTotal += reportDetailX.LineTotal;
                            sportTotal.Turnover += reportDetailX.Turnover;

                            #endregion CalculateSportTotal
                        }

                        #endregion SportTypeChangeProccess

                        switch (liveType)
                        {
                            case 0:
                                result.NoneLiveReport.ReportDetails.Add(reportDetailX);
                                break;
                            case 1:
                                result.LiveReport.ReportDetails.Add(reportDetailX);
                                break;
                            case 2:
                                result.MixLiveAndNoneLiveReport.ReportDetails.Add(reportDetailX);
                                break;
                        }

                    }

                    #endregion ForEach
                    #region TotalOfLastSport

                    if (liveType != -1)
                    {
                        sportTotal.Margin = CalculateMargin(sportTotal);
                        sportTotal.Id = currentSporttype;
                        sportTotal.Name = AccCommon.GetSportName(currentSporttype);

                        switch (sportTotal.LiveType.Id)
                        {
                            case 0:
                                result.NoneLiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                result.NoneLiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                result.NoneLiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                result.NoneLiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                result.NoneLiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                result.NoneLiveReport.ReportDetails.Add(sportTotal);
                                break;
                            case 1:
                                result.LiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                result.LiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                result.LiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                result.LiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                result.LiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                result.LiveReport.ReportDetails.Add(sportTotal);
                                break;
                            case 2:
                                result.MixLiveAndNoneLiveReport.Total.NormalTotal.WinLoss += sportTotal.WinLoss;
                                result.MixLiveAndNoneLiveReport.Total.NormalTotal.Position += sportTotal.Position;
                                result.MixLiveAndNoneLiveReport.Total.NormalTotal.Commission += sportTotal.Commission;
                                result.MixLiveAndNoneLiveReport.Total.NormalTotal.LineTotal += sportTotal.LineTotal;
                                result.MixLiveAndNoneLiveReport.Total.NormalTotal.Turnover += sportTotal.Turnover;
                                result.MixLiveAndNoneLiveReport.ReportDetails.Add(sportTotal);
                                break;
                        }
                    }

                    #endregion TotalOfLastSport

                    result.NoneLiveReport.Total.NormalTotal.Margin = CalculateMargin(result.NoneLiveReport.Total.NormalTotal);
                    result.LiveReport.Total.NormalTotal.Margin = CalculateMargin(result.LiveReport.Total.NormalTotal);
                    result.MixLiveAndNoneLiveReport.Total.NormalTotal.Margin = CalculateMargin(result.MixLiveAndNoneLiveReport.Total.NormalTotal);

                    #region CalculateReportTotal

                    result.Total.NormalTotal.WinLoss = result.NoneLiveReport.Total.NormalTotal.WinLoss +
                        result.LiveReport.Total.NormalTotal.WinLoss +
                        result.MixLiveAndNoneLiveReport.Total.NormalTotal.WinLoss;
                    result.Total.NormalTotal.Position = result.NoneLiveReport.Total.NormalTotal.Position +
                        result.LiveReport.Total.NormalTotal.Position +
                        result.MixLiveAndNoneLiveReport.Total.NormalTotal.Position;
                    result.Total.NormalTotal.Commission = result.NoneLiveReport.Total.NormalTotal.Commission +
                         result.LiveReport.Total.NormalTotal.Commission +
                         result.MixLiveAndNoneLiveReport.Total.NormalTotal.Commission;
                    result.Total.NormalTotal.LineTotal = result.NoneLiveReport.Total.NormalTotal.LineTotal +
                         result.LiveReport.Total.NormalTotal.LineTotal +
                         result.MixLiveAndNoneLiveReport.Total.NormalTotal.LineTotal;
                    result.Total.NormalTotal.Turnover = result.NoneLiveReport.Total.NormalTotal.Turnover +
                       result.LiveReport.Total.NormalTotal.Turnover +
                       result.MixLiveAndNoneLiveReport.Total.NormalTotal.Turnover;
                    result.Total.NormalTotal.Margin = CalculateMargin(result.Total.NormalTotal);

                    #endregion CalculateReportTotal

                }
                SessionExtension.AddSession(HttpContext.Current.Session, _advanceMixParlayReport, result);
         
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public void Export2Excel_MixParlayAdvanceMain(DateTime fromDate, DateTime toDate, int betType)
        {
            var report = (LiveAndNoneLiveReport)SessionExtension.GetSession(HttpContext.Current.Session, _advanceMixParlayReport);
            if (report == null)
            {
                if(fromDate <=  Fanex.Tarus.Reports.DataAccess.BaseDataAccess.BalanceUp.MoveBetDay)
                    report = SelAdvanceMixParlayReport(fromDate, toDate, true , betType);
                else
                    report = SelAdvanceMixParlayReport(fromDate, toDate, false, betType);
            }

            var reportFileName = string.Format(Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.ReportFileName, fromDate);
            var reportTitle = string.Format(Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.ReportTitle, fromDate);

            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var reportName = reportFileName.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var endCol = startCol + 8;

            #endregion ExcelFormat
            #region ReportHeader

            wsReport.Columns[startCol].Width = 20 * 256;
            wsReport.Columns[startCol + 1].Width = 15 * 256;
            wsReport.Columns[startCol + 2].Width = 15 * 256;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = reportTitle;
            currentRow++;
            Export2Excel_MixParlayAdvanceMainHeader(ref wsReport, ref currentRow, startCol);

            #endregion ReportHeader

            if (report != null && (report.NoneLiveReport.ReportDetails.Count > 0 ||
                                    report.LiveReport.ReportDetails.Count > 0 ||
                                    report.MixLiveAndNoneLiveReport.ReportDetails.Count > 0))
            {
                #region ReportContent

                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.LiveReport, Color.FromArgb(221, 217, 196));
                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.NoneLiveReport, Color.FromArgb(202, 240, 145));
                Export2Excel_MixParlayAdvanceMainContent(ref wsReport, startCol, ref currentRow,
                    report.MixLiveAndNoneLiveReport, Color.FromArgb(197, 217, 241));

                #endregion ReportContent
                #region ReportFooter

                Export2Excel_MixParlayAdvanceMainFooter(ref wsReport, startCol, currentRow, report.Total.NormalTotal);

                #endregion ReportFooter
            }
            else
            {
                #region EmptyReport

                wsReport.Columns[startCol].Width = 20 * 256;
                wsReport.Columns[startCol + 1].Width = 15 * 256;
                wsReport.Columns[startCol + 2].Width = 15 * 256;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                currentRow++;

                #endregion EmptyReport
            }

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        private void Export2Excel_MixParlayAdvanceMainHeader(ref ExcelWorksheet wsReport,
            ref int currentRow, int startCol)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeaderNormalFormat = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = string.Empty;
            wsReport.Columns[currentCol].Width = 6000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Sporttype;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Bettype;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.WinLoss;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Position;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Comm;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.LineTotal;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Turnover;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Margin;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

        }

        private void Export2Excel_MixParlayAdvanceMainContent(ref ExcelWorksheet wsReport,
            int startCol, ref int currentRow, Report liveTypeReport, Color colorLiveFormat)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var numberDefaultSubFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(222, 210, 190), true, false, false);
            var numberDefaultTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(33, 89, 103), true, false, false);

            var numberFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false,true);
            var numberSubFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(222, 210, 190), true, true);
            var numberTotalFormat = excelGemboxUtils.GetStyleIntNumberTotal(".00", Color.FromArgb(33, 89, 103), true, true);

            var numberSportFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), true, true, false);
            var numberLiveFormat = excelGemboxUtils.GetStyleIntNumber(".00", colorLiveFormat, true, true, false);

            var percentFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(253, 252, 252), false, true);
            var percentSubFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(222, 210, 190), true, true);
            var percentTotalFormat = excelGemboxUtils.GetStyleIntNumberTotal(".00%", Color.FromArgb(33, 89, 103), true, true);

            var currentCol = startCol;
            var endRow = currentRow + liveTypeReport.ReportDetails.Count - 1;

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, startCol).Style = numberLiveFormat;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, startCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, startCol).Value = liveTypeReport.LiveType.Name;

            var sportRowSpan = 0;
            var lastRow = currentRow;

            foreach (ReportDetail reportDetail in liveTypeReport.ReportDetails)
            {
                currentCol = startCol;
                switch (reportDetail.RowType.Id)
                {
                    case RowType.Normal:
                        currentCol += 2;
                        wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                        wsReport.Cells[currentRow, currentCol].Value = reportDetail.AliasName;
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.WinLoss);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Position);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Commission);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.LineTotal);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Turnover);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = percentFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (CastAide.AsDouble(reportDetail.Turnover) != 0)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Margin) / 100;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = _notAvailableString;
                        }
                        currentCol++;

                        currentRow++;
                        sportRowSpan++;

                        break;
                    case RowType.SportTotal:

                        currentCol = startCol;
                        currentCol += 1;
                        wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                        wsReport.Cells[currentRow, currentCol].Value = string.Empty;
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberSubFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.SubTotal;
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberSubFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.WinLoss);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultSubFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Position);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultSubFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Commission);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberSubFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.LineTotal);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultSubFormat;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Turnover);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = percentSubFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (CastAide.AsDouble(reportDetail.Turnover) != 0)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportDetail.Margin) / 100;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = _notAvailableString;
                        }


                        currentRow++;

                        #region SporttypeHeader

                        var sportTypeCol = startCol + 1;
                        endRow = lastRow + sportRowSpan - 1;

                        wsReport.Cells.GetSubrangeAbsolute(lastRow, sportTypeCol, endRow, sportTypeCol).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(lastRow, sportTypeCol, endRow, sportTypeCol).
                            Style = numberSportFormat;
                        wsReport.Cells.GetSubrangeAbsolute(lastRow, sportTypeCol, endRow, sportTypeCol).
                            Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(lastRow, sportTypeCol, endRow, sportTypeCol).
                            Value = string.Format("{0}", reportDetail.Name);

                        sportRowSpan = 0;
                        lastRow = currentRow;

                        #endregion SporttypeHeader

                        break;
                }

            }

            #region AllTotal

            var normalTotal = liveTypeReport.Total.NormalTotal;
            currentCol = startCol;

            wsReport.Cells[currentRow, currentCol].Style = commonStyle;
            wsReport.Cells[currentRow, currentCol].Value = string.Empty;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment =
                HorizontalAlignmentStyle.Center;
            wsReport.Cells[currentRow, currentCol].Value = string.Empty;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment =
               HorizontalAlignmentStyle.Center;
            wsReport.Cells[currentRow, currentCol].Style.Font.Color = Color.White;
            wsReport.Cells[currentRow, currentCol].Value = Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.Total;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.WinLoss);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.Font.Color = Color.White;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.Position);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.Font.Color = Color.White;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.Commission);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.LineTotal);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.Font.Color = Color.White;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.Turnover);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = percentTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            if (CastAide.AsDouble(normalTotal.Turnover) != 0)
            {
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(normalTotal.Margin) / 100;
            }
            else
            {
                wsReport.Cells[currentRow, currentCol].Value = _notAvailableString;
            }
            currentCol++;

            currentRow++;

            #endregion AllTotal
        }

        private void Export2Excel_MixParlayAdvanceMainFooter(ref ExcelWorksheet wsReport,
            int startCol, int currentRow, ReportDetail grandTotal)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var numberTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, true);
            var percentTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00%", Color.FromArgb(218, 241, 187), true, true);

            var numberDefaultTotalFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, false, false);
            var numberDefaultTotalFormatLineTotal = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(181, 196, 208), true, true, false);

            var currentCol = startCol;
            var endCol = currentCol + 2;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style =
                numberTotalFormat;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value =
                Fanex.Tarus.Reports.Resource.AdvanceMixParlayReport.GrandTotal;
            currentCol += 3;

            wsReport.Cells[currentRow, currentCol].Style = numberTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.WinLoss);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.Position);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.Commission);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormatLineTotal;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.LineTotal);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = numberDefaultTotalFormat;
            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.Turnover);
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = percentTotalFormat;
            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            if (CastAide.AsDouble(grandTotal.Turnover) != 0)
            {
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(grandTotal.Margin) / 100;
            }
            else
            {
                wsReport.Cells[currentRow, currentCol].Value = _notAvailableString;
            }
            currentCol++;
        }

    }
}
