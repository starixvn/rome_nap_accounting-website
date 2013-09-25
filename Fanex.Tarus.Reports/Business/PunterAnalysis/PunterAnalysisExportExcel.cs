using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Infrastructure.Caching;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

namespace Fanex.Tarus.Reports.Business.PunterAnalysis
{
    public partial class PunterAnalysisService
    {
        public void ExportExcelTopPunterSummary(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisTotalDTO topPunterSummaryReport = (PunterAnalysisTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterSummary);
            param.UserName = null;
            if (topPunterSummaryReport == null)
            {
                topPunterSummaryReport = SelTopPunterSummary(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterSummary(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterSummary(ref wsReport, startCol, ref currentRow, topPunterSummaryReport, param);
            //ExportExcelCustomerIP_TopPunterSummary(ref excelFile, startCol, topPunterSummaryReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        public void ExportExcelTopPunterSummaryByLeague(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisTotalDTO topPunterSummaryByLeagueReport = (PunterAnalysisTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterSummaryByLeague);
            if (topPunterSummaryByLeagueReport == null)
            {
                topPunterSummaryByLeagueReport = SelTopPunterSummaryByLeague(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();
           

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterSummaryByLeague(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterSummaryByLeague(ref wsReport, startCol, ref currentRow, topPunterSummaryByLeagueReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        public void ExportExcelTopPunterByCustID(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisTotalDTO topPunterByCustIdReport = (PunterAnalysisTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterByCustId);
            if (topPunterByCustIdReport == null)
            {
                topPunterByCustIdReport = SelTopPunterByCustID(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterByCustID(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterByCustID(ref wsReport, startCol, ref currentRow, topPunterByCustIdReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        public void ExportExcelTopPunterByLeagueCustID(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisTotalDTO topPunterByLeagueCustIdReport = (PunterAnalysisTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterByLeagueCustId);
            if (topPunterByLeagueCustIdReport == null)
            {
                topPunterByLeagueCustIdReport = SelTopPunterByLeagueCustID(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterByLeagueCustID(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterByCustID(ref wsReport, startCol, ref currentRow, topPunterByLeagueCustIdReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        public void ExportExcelTopPunterBySporttype(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisGrandTotalDTO topPunterBySporttypeReport = (PunterAnalysisGrandTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterBySporttype);
            if (topPunterBySporttypeReport == null)
            {
                topPunterBySporttypeReport = SelTopPunterBySporttype(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterBySporttype(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterBySporttype(ref wsReport, startCol, ref currentRow, topPunterBySporttypeReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        public void ExportExcelTopPunterByBettype(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisGrandTotalDTO topPunterByBettypeReport = (PunterAnalysisGrandTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterByBettype);
            if (topPunterByBettypeReport == null)
            {
                topPunterByBettypeReport = SelTopPunterByBettype(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterByBettype(ref wsReport, startCol, ref currentRow, topPunterByBettypeReport, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterByBettype(ref wsReport, startCol, ref currentRow, topPunterByBettypeReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        private void ExportHeader_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisParam param)
        {
            const string typeTilte = "Username";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";
            const string ipTitle = "IP";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol + 5;
            var endRow = currentRow;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + 5;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter Analysis from {0} to {1}({2})", param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 2;
            endCol = startCol + 5;

            var maxSportLengthPerRow = 121;
            var sportNameList = string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId);
            if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29")) // All Parlay
            {
                sportNameList = "All";
            }

            if (maxSportLengthPerRow == 0)
            {
                wsReport.Rows[endRow].Height = 200;
            }
            else
            {
                wsReport.Rows[endRow].Height = 200 + ((int)Math.Ceiling((double)(sportNameList.Length + 12) / maxSportLengthPerRow)) * 200;
            }

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value =
                 "User Name: " + (string.IsNullOrEmpty(param.UserName) ? "All" : param.UserName) + "\n" +
                 "Top rows: " + param.TopRow + "- Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n" +
                 "Sport Type: " + sportNameList;

            currentRow += 3;

            #endregion Title

            endCol = startCol + 5;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #region Header

            currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = typeTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = ipTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportBody_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisTotalDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var totalRowColor = Color.FromArgb(196, 217, 238);

            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, false, false);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, true);
            var totalNumberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, false);
            var totalPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, true);
            var totalDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol;
            var endRow = currentRow;

            if (report != null && report.Punters.Count > 0)
            {
                foreach (var punter in report.Punters)
                {
                    #region PunterRow

                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[currentRow, currentCol].Value = punter.Name;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Winlost);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Turnover);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    if (punter.Margin == _notAvailableStr)
                    {
                        wsReport.Cells[currentRow, currentCol].Value = punter.Margin;
                    }
                    else
                    {
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(punter.Margin.Replace("%", "")) / 100;
                    }
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Betcount);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = "view by ip";
                    currentCol++;

                    currentRow++;

                    #endregion PunterRow
                }

                #region TotalRow

                currentCol = startCol;
                var reportTotal = report.Total;
                wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = reportTotal.Name;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Winlost);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberDefaultFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Turnover);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalPercentFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                if (reportTotal.Margin == _notAvailableStr)
                {
                    wsReport.Cells[currentRow, currentCol].Value = reportTotal.Margin;
                }
                else
                {
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportTotal.Margin.Replace("%", "")) / 100;
                }
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Betcount);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = "";
                currentCol++;

                currentRow++;

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 5;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private void ExportHeader_TopPunterSummaryByLeague(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                                        PunterAnalysisParam param)
        {
            const string typeTilte = "Username";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";
            //const string ipTitle = "IP";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol + 5;
            var endRow = currentRow;

            #region Title

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter Analysis from {0} to {1}({2})", param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 1;
            endCol = startCol + 4;
            var sportNameList = (string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId));
            if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29"))
            {
                sportNameList = "All";
            }

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value =
                "Top rows: " + param.TopRow + "- Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n" +
                "Sport Type: " + sportNameList +
                " - League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId));

            currentRow += 2;

            #endregion Title

            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #region Header

            currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = typeTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            //wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            //wsReport.Cells[currentRow, currentCol].Value = ipTitle;
            //wsReport.Columns[currentCol].Width = 4000;
            //currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportBody_TopPunterSummaryByLeague(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisTotalDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var totalRowColor = Color.FromArgb(196, 217, 238);

            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, false, false);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, true);
            var totalNumberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, false);
            var totalPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, true);
            var totalDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol;
            var endRow = currentRow;

            if (report != null && report.Punters.Count > 0)
            {
                foreach (var punter in report.Punters)
                {
                    #region PunterRow

                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[currentRow, currentCol].Value = punter.Name;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Winlost);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Turnover);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(punter.Margin.Replace("%", "")) / 100;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Betcount);
                    currentCol++;

                    //wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    //wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    //wsReport.Cells[currentRow, currentCol].Value = "view by ip";
                    //currentCol++;

                    currentRow++;

                    #endregion PunterRow
                }

                #region TotalRow

                var reportTotal = report.Total;
                currentCol = startCol;
                wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = reportTotal.Name;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Winlost);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberDefaultFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Turnover);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalPercentFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportTotal.Margin.Replace("%", "")) / 100;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Betcount);
                currentCol++;

                //wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                //wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                //wsReport.Cells[currentRow, currentCol].Value = "";
                //currentCol++;

                currentRow++;

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 4;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private void ExportHeader_TopPunterByCustID(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                                        PunterAnalysisParam param)
        {
            const string typeTilte = "Sport Type";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter Analysis from {0} to {1}({2})", param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 2;
            endCol = startCol + colSpan;
            var maxSportLengthPerRow = 121;
            var sportNameList = string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId);
            if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29"))
            {
                sportNameList = "All";
            }

            if (maxSportLengthPerRow == 0)
            {
                wsReport.Rows[endRow].Height = 200;
            }
            else
            {
                wsReport.Rows[endRow].Height = 200 + ((int)Math.Ceiling((double)(sportNameList.Length + 12) / maxSportLengthPerRow)) * 200;
            }
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            var reportSummaryHeader = "User Name: " + (string.IsNullOrEmpty(param.CustName) ? "" : param.CustName);
            switch (param.SearchBy)
            {
                case "0":
                    break;

                case "1":
                    reportSummaryHeader += " - League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId));
                    break;

                default:
                    break;
            }
            reportSummaryHeader += "\n" + "Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n";
            reportSummaryHeader += "Sport Type: " + sportNameList;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportSummaryHeader;

            currentRow += 3;

            #endregion Title

            #region EmptyRow

            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = typeTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportBody_TopPunterByCustID(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisTotalDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var totalRowColor = Color.FromArgb(196, 217, 238);

            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, false, false);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, true);
            var totalNumberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, false);
            var totalPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, true);
            var totalDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol;
            var endRow = currentRow;
            const int colSpan = 4;

            if (report != null && report.Punters.Count > 0)
            {
                foreach (var punter in report.Punters)
                {
                    #region PunterRow

                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[currentRow, currentCol].Value = punter.Name;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Winlost);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Turnover);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    if (punter.Margin == _notAvailableStr)
                    {
                        wsReport.Cells[currentRow, currentCol].Value = punter.Margin;
                    }
                    else
                    {
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(punter.Margin.Replace("%", "")) / 100;
                    }
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punter.Betcount);
                    currentCol++;

                    currentRow++;

                    #endregion PunterRow
                }

                #region TotalRow

                var reportTotal = report.Total;
                currentCol = startCol;
                wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = reportTotal.Name;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Winlost);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberDefaultFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Turnover);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalPercentFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                if (reportTotal.Margin == _notAvailableStr)
                {
                    wsReport.Cells[currentRow, currentCol].Value = reportTotal.Margin;
                }
                else
                {
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(reportTotal.Margin.Replace("%", "")) / 100;
                }
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(reportTotal.Betcount);
                currentCol++;

                currentRow++;

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + colSpan;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private void ExportHeader_TopPunterByLeagueCustID(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisParam param)
        {
            const string typeTilte = "Bet Type";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter {0} from {1} to {2}({3})", param.CustName, param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 2;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value =
                "User Name: " + (string.IsNullOrEmpty(param.CustName) ? "" : param.CustName) + "\n" +
                "Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n" +
                "League: " + (string.IsNullOrEmpty(param.LeagueName) ? "" : param.LeagueName.Replace(",", ", "));

            currentRow += 3;

            #endregion Title

            #region EmptyRow

            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = typeTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportHeader_TopPunterBySporttype(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisParam param)
        {
            const string typeTilte = "Bet Type";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";

            const string noneLiveTitle = "None Live";
            const string liveTitle = "Live";
            const string liveAndNoneLiveTitle = "None Live & Live ";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var colSpan = 12;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value =
                string.Format("Punter Analysis from {0} to {1}({2})", param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 2;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            var reportSummaryHeader = "User Name: " + (string.IsNullOrEmpty(param.CustName) ? "" : param.CustName);
            switch (param.SearchBy)
            {
                case "0":
                    break;

                case "1":
                    reportSummaryHeader += " - League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId));
                    break;

                default:
                    break;
            }

            var sportNameList = (string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId));
            if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29"))
            {
                sportNameList = "All";
            }

            reportSummaryHeader += "\n" + "Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n" +
                 "Sport Type: " + sportNameList;

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportSummaryHeader;
            currentRow += 3;

            #endregion Title

            #region EmptyRow

            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            endRow = currentRow + 1;
            currentCol = startCol;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Style = bgHeader;
            wsReport.Columns[currentCol].Width = 4000;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Value = typeTilte;
            currentCol++;

            endCol = currentCol + 3;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = noneLiveTitle;
            currentCol += 4;

            endCol = currentCol + 3;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = liveTitle;
            currentCol += 4;

            endCol = currentCol + 3;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = liveAndNoneLiveTitle;
            currentCol += 4;
            currentRow++;

            currentCol = startCol + 1;

            #region NoneLiveHeader

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            #endregion NoneLiveHeader

            #region LiveHeader

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            #endregion LiveHeader

            #region LiveAndNoneLiveHeader

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            #endregion LiveAndNoneLiveHeader

            currentRow++;

            #endregion Header
        }

        private void ExportBody_TopPunterBySporttype(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisGrandTotalDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var totalRowColor = Color.FromArgb(196, 217, 238);

            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, false, false);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, true);
            var totalNumberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, false);
            var totalPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, true);
            var totalDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol;
            var endRow = currentRow;
            const int colSpan = 12;

            if (report != null && report.Punters.Count > 0)
            {
                for (int i = 0; i < report.Punters.Count; i++)
                {
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[currentRow, currentCol].Value = report.Punters[i].Name;
                    currentCol++;

                    for (var j = 0; j < report.Punters[i].Columns.Count; j++)
                    {
                        var data = report.Punters[i].Columns[j];

                        #region PunterRow

                        wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (data.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Winlost);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }

                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (data.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Turnover);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (data.HasData != "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        else if (data.Margin == _notAvailableStr)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = data.Margin;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(data.Margin.Replace("%", "")) / 100;
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = intFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (data.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Betcount);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        currentCol++;

                        #endregion PunterRow
                    }

                    currentRow++;
                }

                #region TotalRow

                if (report.Punters.Count > 0)
                {
                    var punterTotal = report.Total;
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currentRow, currentCol].Value = punterTotal.Name;
                    currentCol++;

                    for (var j = 0; j < punterTotal.Columns.Count; j++)
                    {
                        var data = punterTotal.Columns[j];
                        wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Winlost);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalNumberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Turnover);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalPercentFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        if (data.Margin == _notAvailableStr)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = data.Margin;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(data.Margin.Replace("%", "")) / 100;
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(data.Betcount);
                        currentCol++;
                    }

                    currentRow++;
                }

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + colSpan;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private void ExportHeader_TopPunterByBettype(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisGrandTotalDTO report, PunterAnalysisParam param)
        {
            const string typeTilte = "League";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Cnt";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var colSpan = report.Punters.Count * 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value =
                string.Format("Punter Analysis from {0} to {1}({2})", param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            endRow = currentRow + 2;
            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            var reportSummaryHeader = "User Name: " + (string.IsNullOrEmpty(param.CustName) ? "" : param.CustName);
            switch (param.SearchBy)
            {
                case "0":
                    break;

                case "1":
                    reportSummaryHeader += " - League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId));
                    break;

                default:
                    break;
            }

            var sportNameList = (string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId));
            if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29"))
            {
                sportNameList = "All";
            }

            reportSummaryHeader += "\n" + "Order by: " + param.OrderByName + ", " + param.OrderTypeName + "\n" +
                "Sport Type: " + sportNameList +
                " - Bet Type: " + BetTypeName(param.BettypeId) + " - Live Type: " + GetLiveIndicatorName(param.LiveIndicator);

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportSummaryHeader;

            currentRow += 3;

            #endregion Title

            #region LiveTypeTable

            switch (param.PeriodType)
            {
                case PeriodTypeReportConstants.LongRange:
                    ExportLongRangeSummary_TopPunterByBettype(ref wsReport, startCol, ref currentRow, report, param);
                    break;

                default:
                    break;
            }

            #endregion LiveTypeTable

            #region EmptyRow

            colSpan = (report.Punters.Count == 0) ? 4 : report.Punters[0].Columns.Count * 4;
            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            endRow = currentRow + 1;
            currentCol = startCol;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Style = bgHeader;
            wsReport.Columns[currentCol].Width = 9000;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, endRow, currentCol).Value = typeTilte;
            currentCol++;

            if (report.Punters.Count == 0)
            {
                endCol = currentCol + 3;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = string.Empty;
                currentCol += 4;
                currentRow++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = marginTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;
            }
            else
            {
                var headerTitle = "";
                var numberOfColumns = report.Punters[0].Columns.Count;
                var lastIndex = numberOfColumns - 1;
                string _preyear = "";
                for (int i = 0; i < numberOfColumns; i++)
                {
                    var type = report.Punters[0].Columns[i].Type;
                    var year = type.Split('|')[0];
                    var month = CastAide.AsInt32(type.Split('|')[1]);
                    switch (param.PeriodType)
                    {
                        case PeriodTypeReportConstants.Daily:
                            if (i == lastIndex)
                            {
                                headerTitle = "Grand Total";
                            }
                            else
                            {
                                if (_preyear != year)
                                {
                                    _preyear = year;
                                    headerTitle = "Year " + year;
                                }
                                else
                                {
                                    headerTitle = _arrMonths[month - 1];
                                }
                            }
                            break;

                        case PeriodTypeReportConstants.Weekly:
                            if (i == lastIndex)
                            {
                                headerTitle = "Grand Total";
                            }
                            else
                            {
                                string week = type.Split('|')[1].Split('_')[0];
                                string date = type.Split('|')[1].Split('_')[1];
                                headerTitle = "Week " + week + " (" + date + ")";
                            }
                            break;

                        case PeriodTypeReportConstants.Monthly:
                            headerTitle = _arrMonths[month - 1] + " - " + year;
                            break;

                        case PeriodTypeReportConstants.Annually:
                            if (i == 0)
                            {
                                headerTitle = "Year " + year;
                            }
                            else
                            {
                                headerTitle = _arrMonths[month - 1];
                            }
                            break;

                        case PeriodTypeReportConstants.LongRange:
                            if (i == lastIndex)
                            {
                                headerTitle = "Grand Total";
                            }
                            else
                            {
                                headerTitle = "Year " + year;
                            }
                            break;
                    }

                    endCol = currentCol + 3;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = headerTitle;
                    currentCol += 4;
                }
                currentRow++;

                currentCol = startCol + 1;
                for (int i = 0; i < numberOfColumns; i++)
                {
                    wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                    wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
                    wsReport.Columns[currentCol].Width = 4000;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                    wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
                    wsReport.Columns[currentCol].Width = 4000;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                    wsReport.Cells[currentRow, currentCol].Value = marginTitle;
                    wsReport.Columns[currentCol].Width = 4000;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                    wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
                    wsReport.Columns[currentCol].Width = 4000;
                    currentCol++;
                }

                currentRow++;

            #endregion Header
            }
        }

        private void ExportLongRangeSummary_TopPunterByBettype(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisGrandTotalDTO report, PunterAnalysisParam param)
        {
            if (report.GrandTotal == null)
                return;

            const string typeTilte = "Year";
            const string winLossTitle = "Winlost";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            const string betCountTitle = "Bet Count";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            var bgColor = Color.FromArgb(255, 255, 255);
            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            #region EmptyRow

            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region BettypeHeader

            endCol = startCol + colSpan;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = bgHeader;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = GetLiveIndicatorName(param.LiveIndicator) + " - " + BetTypeName(param.BettypeId);
            currentRow++;

            currentCol = startCol;
            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = typeTilte;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winLossTitle;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = turnoverTitle;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = marginTitle;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            currentCol++;
            currentRow++;

            #endregion BettypeHeader

            if (report.GrandTotal != null && report.GrandTotal.Count > 0)
            {
                for (var i = 0; i < report.GrandTotal.Count; i++)
                {
                    var detail = report.GrandTotal[i];

                    #region PunterRow

                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(detail.Id);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(detail.Winlost);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(detail.Turnover);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    if (detail.Margin == _notAvailableStr)
                    {
                        wsReport.Cells[currentRow, currentCol].Value = detail.Margin;
                    }
                    else
                    {
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(detail.Margin.Replace("%", "")) / 100;
                    }
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(detail.Betcount);
                    currentCol++;
                    currentRow++;

                    #endregion PunterRow
                }
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 4;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private void ExportBody_TopPunterByBettype(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                          PunterAnalysisGrandTotalDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var totalRowColor = Color.FromArgb(196, 217, 238);

            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, false);
            var bgPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, true);
            var bgDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", bgColor, false, false);

            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, false, false);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, true);
            var totalNumberDefaultFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, false, false);
            var totalPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, true);
            var totalDefaultPercentFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00%", totalRowColor, false, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var currentCol = startCol;
            var endCol = startCol;
            var endRow = currentRow;
            int colSpan = report.Punters.Count * 4;
            var maxSportLengthPerRow = 40;

            if (report != null && report.Punters.Count > 0)
            {
                for (int i = 0; i < report.Punters.Count; i++)
                {
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsReport.Cells[currentRow, currentCol].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                    wsReport.Cells[currentRow, currentCol].Style.WrapText = true;
                    if (maxSportLengthPerRow == 0 || report.Punters[i].Name.Length <= maxSportLengthPerRow)
                    {
                        wsReport.Rows[currentRow].Height = 230;
                    }
                    else
                    {
                        wsReport.Rows[currentRow].Height = 460;
                    }
                    wsReport.Cells[currentRow, currentCol].Value = report.Punters[i].Name;
                    currentCol++;

                    for (var j = 0; j < report.Punters[i].Columns.Count; j++)
                    {
                        var punterDetail = report.Punters[i].Columns[j];

                        #region PunterRow

                        wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (punterDetail.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punterDetail.Winlost);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }

                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = numberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (punterDetail.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punterDetail.Turnover);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = bgPercentFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (punterDetail.HasData != "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        else if (punterDetail.Margin == _notAvailableStr)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = punterDetail.Margin;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(punterDetail.Margin.Replace("%", "")) / 100;
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = intFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        if (punterDetail.HasData == "1")
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(punterDetail.Betcount);
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = "";
                        }
                        currentCol++;

                        #endregion PunterRow
                    }

                    currentRow++;
                }

                #region TotalRow

                if (report.Punters.Count > 0)
                {
                    var total = report.Total;
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currentRow, currentCol].Value = total.Name;
                    currentCol++;

                    for (var j = 0; j < total.Columns.Count; j++)
                    {
                        var totalDetail = total.Columns[j];
                        wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(totalDetail.Winlost);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalNumberDefaultFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(totalDetail.Turnover);
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalPercentFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        if (totalDetail.Margin == _notAvailableStr)
                        {
                            wsReport.Cells[currentRow, currentCol].Value = totalDetail.Margin;
                        }
                        else
                        {
                            wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(totalDetail.Margin.Replace("%", "")) / 100;
                        }
                        currentCol++;

                        wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                        wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                        wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(totalDetail.Betcount);
                        currentCol++;
                    }

                    currentRow++;
                }

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 4;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        public void ExportExcelTopPunterBySporttypeSum(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisTotalDTO topPunterBySporttypeSumReport = (PunterAnalysisTotalDTO)SessionExtension.GetSession(HttpContext.Current.Session, _topPunterBySporttypeSum);
            if (topPunterBySporttypeSumReport == null)
            {
                topPunterBySporttypeSumReport = SelTopPunterBySporttypeSum(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportHeader_TopPunterByCustID(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_TopPunterByCustID(ref wsReport, startCol, ref currentRow, topPunterBySporttypeSumReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        private void ExportExcelCustomerIP_TopPunterSummary(ref ExcelFile excelFile, int startCol,
                            PunterAnalysisTotalDTO report, PunterAnalysisParam param)
        {
            const string monthTilte = "Month";
            const string yearTitle = "Year";
            const string ipTitle = "IP";
            const string countryTitle = "Country";

            PunterAnalysisParam paramToGetCustIp = new PunterAnalysisParam();
            var wsReport = excelFile.Worksheets.Add("Customer Ip");

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var currentRow = 0;
            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;
            var custIpListOrder = 0;

            for (int i = 0; i < report.Punters.Count; i++)
            {
                var punter = report.Punters[i];

                paramToGetCustIp.CustId = punter.Id;
                paramToGetCustIp.UserName = punter.Name;
                paramToGetCustIp.FromDate = param.FromDate;
                paramToGetCustIp.ToDate = param.ToDate;

                var ipList = SelIPByCustomer(paramToGetCustIp);
                if (ipList == null || ipList.Count <= 0)
                {
                    continue;
                }

                if (custIpListOrder != 0)
                {
                    #region EmptyRow

                    currentCol = startCol;
                    endCol = startCol + 3;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = commonStyle;
                    wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = "";
                    currentCol++;
                    currentRow++;

                    #endregion EmptyRow
                }

                #region Header

                currentCol = startCol;
                endCol = startCol + 3;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Style = bgHeader;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, endCol).Value = punter.Name;
                currentCol++;
                currentRow++;

                currentCol = startCol;
                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = monthTilte;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = yearTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = ipTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = bgHeader;
                wsReport.Cells[currentRow, currentCol].Value = countryTitle;
                wsReport.Columns[currentCol].Width = 4000;
                currentCol++;

                currentRow++;

                #endregion Header

                #region Body

                foreach (var ipInfo in ipList)
                {
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(ipInfo.Month);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(ipInfo.Year);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = ipInfo.IP;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = ipInfo.Country;
                    currentCol++;

                    currentRow++;
                }

                custIpListOrder++;

                #endregion Body
            }
        }

        public void ExportExcelIPByCustId_TopPunterSummary(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            List<PunterAnalysisIpBO> ipByCustIdReport = (List<PunterAnalysisIpBO>)SessionExtension.GetSession(HttpContext.Current.Session, _ipByCustId);
            if (ipByCustIdReport == null)
            {
                ipByCustIdReport = SelIPByCustomer(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportExcelHeader_IPByCustId_TopPunterSummary(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_IPByCustId_TopPunterSummary(ref wsReport, startCol, ref currentRow, ipByCustIdReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        private void ExportExcelHeader_IPByCustId_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                            PunterAnalysisParam param)
        {
            const string monthTilte = "Month";
            const string yearTitle = "Year";
            const string ipTitle = "IP";
            const string countryTitle = "Country";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            var currentCol = startCol;
            var endRow = currentRow;
            var endCol = currentCol;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + 3;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter {0} by IP from {1} to {2}({3})", param.CustName, param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            #endregion Title

            #region EmptyRow

            endCol = startCol + 3;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = monthTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = yearTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = ipTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = countryTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportBody_IPByCustId_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                           List<PunterAnalysisIpBO> report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);
            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            if (report.Count > 0)
            {
                foreach (var ipInfo in report)
                {
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(ipInfo.Month);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(ipInfo.Year);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = ipInfo.IP;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = ipInfo.Country;
                    currentCol++;

                    currentRow++;
                }
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 3;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        public void ExportExcelBetCountByCustId_TopPunterSummary(PunterAnalysisParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));

            var reportName = param.ReportFileName;
            PunterAnalysisBetCountDTO betCountByCustIdReport = (PunterAnalysisBetCountDTO)SessionExtension.GetSession(HttpContext.Current.Session, _betCountByCustId);
            if (betCountByCustIdReport == null)
            {
                betCountByCustIdReport = SelBetCountByCustomer(param);
            }

            var excelGemboxUtils = new ExcelGemboxUtils();

            var endCol = startCol;
            var currentCol = startCol;

            #endregion ExcelFormat

            #region Header

            ExportExcelHeader_BetCountByCustId_TopPunterSummary(ref wsReport, startCol, ref currentRow, param);

            #endregion Header

            #region Body

            ExportBody_BetCountByCustId_TopPunterSummary(ref wsReport, startCol, ref currentRow, betCountByCustIdReport, param);

            #endregion Body

            #region ResponseFile

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion ResponseFile
        }

        private void ExportExcelHeader_BetCountByCustId_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                        PunterAnalysisParam param)
        {
            const string dateTilte = "Date";
            const string betCountTitle = "Bet Count";
            const string winCountTitle = "Win Count";
            const string drawCountTitle = "Draw Count";
            const string winlossTitle = "Winloss";

            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            var currentCol = startCol;
            var endRow = currentRow;
            var endCol = currentCol;

            #region Title

            endRow = currentRow + 1;
            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
            var reportTitle = string.Format("Punter {0} by bet count from {1} to {2}({3})", param.CustName, param.FromDate, param.ToDate, GetPeriodTypeName(param.PeriodType));

            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = reportTitle;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleTitle(252, 213, 180);
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow += 2;

            if (!string.IsNullOrEmpty(param.SportId) || !string.IsNullOrEmpty(param.ProductIds))
            {
                endRow = currentRow;
                endCol = startCol + 4;
                var maxSportLengthPerRow = 121;
                var sportNameList = string.IsNullOrEmpty(param.SportId) ? SportName(param.ProductIds) : SportName(param.SportId);
                if (param.SportId.Contains("0_9") || param.SportId.Contains("0_29"))
                {
                    sportNameList = "All";
                }

                if (maxSportLengthPerRow == 0)
                {
                    wsReport.Rows[endRow].Height = 200;
                }
                else
                {
                    wsReport.Rows[endRow].Height = 200 + ((int)Math.Ceiling((double)(sportNameList.Length + 12) / maxSportLengthPerRow)) * 210;
                }

                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                var extraDetail = "";
                switch (param.SearchBy)
                {
                    case "0":
                        break;

                    case "1":
                        extraDetail += "League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId)) + " - ";
                        break;

                    default:
                        break;
                }

                extraDetail += "Sport Type: " + sportNameList;
                if (!string.IsNullOrEmpty(param.BettypeId))
                {
                    extraDetail += " - Bet Type: " + BetTypeName(param.BettypeId);
                }

                if (!string.IsNullOrEmpty(param.LiveIndicator))
                {
                    extraDetail += " - Live Type: " + GetLiveIndicatorName(param.LiveIndicator);
                }

                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = extraDetail;
                currentRow++;
            }
            else
            {
                switch (param.SearchBy)
                {
                    case "0":
                        break;

                    case "1":
                        endRow = currentRow;
                        endCol = startCol + 4;
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style = excelGemboxUtils.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.WrapText = true;
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                        var extraDetail = "League: " + (string.IsNullOrEmpty(param.LeagueId) ? "" : LeagueName(param.LeagueId));
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, endRow, endCol).Value = extraDetail;
                        currentRow++;
                        break;

                    default:
                        break;
                }
            }

            #endregion Title

            #region EmptyRow

            endCol = startCol + 4;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
            currentRow++;

            #endregion EmptyRow

            #region Header

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = dateTilte;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = betCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = drawCountTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            wsReport.Cells[currentRow, currentCol].Style = bgHeader;
            wsReport.Cells[currentRow, currentCol].Value = winlossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentRow++;

            #endregion Header
        }

        private void ExportBody_BetCountByCustId_TopPunterSummary(ref ExcelWorksheet wsReport, int startCol, ref int currentRow,
                        PunterAnalysisBetCountDTO report, PunterAnalysisParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(142, 179, 231));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", bgColor, false, true);
            var bgFormatFullDigital = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, true);
            var intFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", bgColor, false, false);

            var totalRowColor = Color.FromArgb(196, 217, 238);
            var totalNumberFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis(".00", totalRowColor, true, true);
            var totalIntFormat = excelGemboxUtils.GetStyleIntNumberPunterAnalysis("", totalRowColor, true, false);
            var totalCommonStyle = excelGemboxUtils.GetStyleText(196, 217, 238);

            var currentCol = startCol;
            var colSpan = 4;
            var endCol = startCol + colSpan;
            var endRow = currentRow;

            if (report.Punters.Count > 0)
            {
                foreach (var betCountInfo in report.Punters)
                {
                    currentCol = startCol;
                    wsReport.Cells[currentRow, currentCol].Style = commonStyle;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells[currentRow, currentCol].Value = betCountInfo.Date;
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(betCountInfo.BetCount);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(betCountInfo.WinCount);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = intFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(betCountInfo.DrawCount);
                    currentCol++;

                    wsReport.Cells[currentRow, currentCol].Style = bgFormat;
                    wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(betCountInfo.Winlost);
                    currentCol++;

                    currentRow++;
                }

                #region TotalRow

                var totalDetail = report.Total;
                currentCol = startCol;
                wsReport.Cells[currentRow, currentCol].Style = totalCommonStyle;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currentRow, currentCol].Value = "Total";
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(totalDetail.BetCount);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(totalDetail.WinCount);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalIntFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDouble(totalDetail.DrawCount);
                currentCol++;

                wsReport.Cells[currentRow, currentCol].Style = totalNumberFormat;
                wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currentRow, currentCol].Value = CastAide.AsDecimal(totalDetail.Winlost);
                currentCol++;

                currentRow++;

                #endregion TotalRow
            }
            else
            {
                #region EmptyReport

                endCol = startCol + 4;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style = commonStyle;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EmptyReportMessage");
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, endCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                #endregion EmptyReport
            }
        }

        private string GetSelectedSportsName(string sportIds)
        {
            var selectedSportsName = "";
            if (string.IsNullOrEmpty(sportIds))
            {
                selectedSportsName = "All";
            }

            return selectedSportsName;
        }

        private string GetLiveIndicatorName(string liveIndicator)
        {
            var result = "";
            switch (liveIndicator)
            {
                case "0":
                    result = "None Live";
                    break;

                case "1":
                    result = "Live";
                    break;

                case "2":
                    result = "None Live & Live";
                    break;

                default:
                    break;
            }
            return result;
        }

        private string GetPeriodTypeName(string periodType)
        {
            var result = "";
            switch (periodType)
            {
                case PeriodTypeReportConstants.Daily:
                    result = "Daily";
                    break;

                case PeriodTypeReportConstants.Weekly:
                    result = "Weekly";
                    break;

                case PeriodTypeReportConstants.Monthly:
                    result += "Monthly";
                    break;

                case PeriodTypeReportConstants.Annually:
                    result += "Annually";
                    break;

                case PeriodTypeReportConstants.LongRange:
                    result += "Month Range";
                    break;
            }
            return result;
        }

        private string SportName(string sportIds)
        {
            string names = "";
            try
            {
                var arrId = sportIds.Split(',').Distinct();
                var arrName = new List<string>();

                foreach (string id in arrId)
                {
                    string name = CacheRef.GetSportName(id);
                    if (name.ToLower().Equals("games"))
                    {
                        name = "Poker Games";
                    }
                    if (string.IsNullOrEmpty(name))
                        continue;
                    arrName.Add(name);
                }

                names = string.Join(", ", arrName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return names;
        }

        private string LeagueName(string leagueIds)
        {
            string names = "";
            try
            {
                var arrId = leagueIds.Split(',').Distinct();
                var arrName = new List<string>();

                foreach (string id in arrId)
                {
                    string name = CacheRef.GetLeagueName(id);
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    arrName.Add(name);
                }

                names = string.Join(", ", arrName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return names;
        }

        private string BetTypeName(string betTypeIds)
        {
            string names = "";
            try
            {
                var arrId = betTypeIds.Split(',').Distinct();
                var arrName = new List<string>();

                foreach (string id in arrId)
                {
                    string name = CacheRef.GetBettype(id);
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    arrName.Add(name);
                }

                names = string.Join(", ", arrName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return names;
        }
    }
}