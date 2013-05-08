


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Reports.Entities.MatchWinLoss;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Fanex.Tarus.Reports.Helpers;

namespace Fanex.Tarus.Reports.Business.MatchWinLoss
{
    public partial class WLSummaryByBetTypeService : IWLSummaryByBetTypeService
    {
        #region IMethods

        public void ExportWLSummaryByBetTypeReport(DateTime reportDate, string reportName)
        {
            try
            {
                #region GetCurrentReport

                var wLSummaryByBetTypeReport = (WLSummaryByBetTypeReport)SessionExtension.GetSession(HttpContext.Current.Session, WLSummaryReportByBetType);
                if (wLSummaryByBetTypeReport == null)
                {
                    wLSummaryByBetTypeReport = SelSummaryReportByBetType(reportDate);
                }

                #endregion GetCurrentReport
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes

                ExportWLSummaryByBetTypeReport(ref ws, startRow, startColumn,
                     ref currentRow, wLSummaryByBetTypeReport, reportDate, reportName);
                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void ExportWLSummaryByBetTypeReport(ref ExcelWorksheet ws, int startRow, int startColumn,
            ref int currentRow, WLSummaryByBetTypeReport wLSummaryByBetTypeReport,
            DateTime reportDate, string reportName)
        {
            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            var gembox = new ExcelGemboxUtils();
            var currentColumn = startColumn;
            var endRow = currentRow;
            var endColumn = currentColumn;

            var headerTextStyle = gembox.GetStyleTitle(181, 196, 208);
            var textStyle = gembox.GetStyleTitle(255, 255, 255);

            #endregion ExcelTypes
            #region WLSummaryByBetTypeReportHeader

            //FirstRow
            currentColumn = startColumn;
            endColumn = currentColumn;
            endRow = currentRow + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.BetType;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 50;

            currentColumn++;
            endColumn = currentColumn + 1;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.SoccerLive;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;


            currentColumn += 2;
            endColumn = currentColumn + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.SoccerNonLive;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

            currentColumn += 2;
            endColumn = currentColumn + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.OtherLive;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

            currentColumn += 2;
            endColumn = currentColumn + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.OtherNonLive;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

            currentColumn += 2;
            endColumn = currentColumn + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.LineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

            currentColumn += 2;
            endColumn = currentColumn;
            endRow = currentRow + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Variance;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Turnover;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow + 1;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Margin;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            //SecondRow
            currentRow++;
            currentColumn = startColumn;
            endRow = currentRow;

            //SoccerLive
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Real;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Virtual;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            //Soccer
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Real;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Virtual;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            //OthersLive
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Real;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Virtual;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            //Others
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Real;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Virtual;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            //LineTotal
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Real;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;
            currentColumn++;
            ws.Cells[currentRow, currentColumn].Value = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.Virtual;
            ws.Cells[currentRow, currentColumn].Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            #endregion WLSummaryByBetTypeReportHeader
            #region WLSummaryByBetTypeReportContent

            if (wLSummaryByBetTypeReport != null && wLSummaryByBetTypeReport.ReportDetails.Count > 0)
            {
                Color bgColor = Color.FromArgb(230, 233, 240);
                AssignWLbyBetTypeExport(ref ws, ref currentRow, wLSummaryByBetTypeReport.ReportDetails,
                    false, bgColor, startColumn);

                List<WLSummaryByBetType> listWLbyBettypeTotal = new List<WLSummaryByBetType>();
                listWLbyBettypeTotal.Add(wLSummaryByBetTypeReport.Total);
                Color totalBgColor = Color.FromArgb(136, 170, 255);
                AssignWLbyBetTypeExport(ref ws, ref currentRow, listWLbyBettypeTotal,
                    true, totalBgColor, startColumn);
            }
            else
            {
                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 13;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = textStyle;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;
            }

            #endregion WLSummaryByBetTypeReportContent
        }

        private void AssignWLbyBetTypeExport(ref ExcelWorksheet ws, ref int currentRow,
            List<WLSummaryByBetType> reportDetails, bool isTotal, Color bgColor, int startColumn)
        {
            var gembox = new ExcelGemboxUtils();
            var rightTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                        HorizontalAlignmentStyle.Right, isTotal);
            var centerTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                        HorizontalAlignmentStyle.Center, isTotal);
            var numberFormart = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B, isTotal);

            var currentColumn = startColumn;

            foreach (WLSummaryByBetType reportDetail in reportDetails)
            {
                currentRow++;
                currentColumn = startColumn;
                ws.Cells[currentRow, currentColumn].Value = reportDetail.BetType + GetBetTypeSuffix(reportDetail.BetTypeId);
                ws.Cells[currentRow, currentColumn].Style = centerTextFormat;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotal_Soccer_Live);
                ws.Cells[currentRow, currentColumn].Style = numberFormart;

                currentColumn++;
                if (reportDetail.VirtualLineTotal_Soccer_Live == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualLineTotal_Soccer_Live);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotal_Soccer);
                ws.Cells[currentRow, currentColumn].Style = numberFormart;

                currentColumn++;
                if (reportDetail.VirtualLineTotal_Soccer == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualLineTotal_Soccer);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotal_Others_Live);
                ws.Cells[currentRow, currentColumn].Style = numberFormart;

                currentColumn++;
                if (reportDetail.VirtualLineTotal_Others_Live == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualLineTotal_Others_Live);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotal_Others);
                ws.Cells[currentRow, currentColumn].Style = numberFormart;

                currentColumn++;
                if (reportDetail.VirtualLineTotal_Others == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualLineTotal_Others);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotalReal);
                ws.Cells[currentRow, currentColumn].Style = numberFormart;

                currentColumn++;
                if (reportDetail.LineTotalVirtual == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotalVirtual);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                if (reportDetail.Variance == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Variance);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                if (reportDetail.Turnover == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Turnover);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }

                currentColumn++;
                if (reportDetail.Margin == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Margin);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;
                }
            }
        }

        private string GetBetTypeSuffix(int betTypeId)
        {
            var suffix = string.Empty;
            switch (betTypeId)
            {
                case Fanex.Tarus.Reports.Constants.BetType.HandicapVS:
                case Fanex.Tarus.Reports.Constants.BetType.OverUnderVS:
                case Fanex.Tarus.Reports.Constants.BetType.CorrectScoreVS:
                case Fanex.Tarus.Reports.Constants.BetType._1x2VS:
                case Fanex.Tarus.Reports.Constants.BetType.TotalGoalVS:
                case Fanex.Tarus.Reports.Constants.BetType.DoubleChanceVS:
                    suffix = " (" + Fanex.Tarus.Reports.Resource.Language.VirtualSportsName + ")";
                    break;
            }

            if (1201 <= betTypeId && betTypeId <= 1299)
            {
                suffix = " (" + Fanex.Tarus.Reports.Resource.Language.VirtualSportsName + ")";
            }

            return suffix;
        }

        #endregion IMethods
    }
}
