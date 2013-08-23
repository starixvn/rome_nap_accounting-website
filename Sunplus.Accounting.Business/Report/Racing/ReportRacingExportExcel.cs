using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Report.Racebook;

namespace Sunplus.Accounting.Business.Report.Racing
{
    public partial class ReportRacing
    {
        #region Export 2 excel

        public void ExportToExcelWinLossRacingReport(Enumerations.ReportType reportType, string winlossDate)
        {
            #region private variables

            var agencyGroupTitle = "Agency";
            var ibcGroupTitle = "IBC";
            var partnerGroupTitle = "Partner";
            var poolCurrencyTitle = "Pool-Currency";
            var currencyTile = "Currency";
            var winToTitle = "Win TO";
            var placeToTitle = "Place TO";
            var wpToTitle = "W/P TO";
            var totalToTitle = "Total TO";
            var commIncomTitle = "Comm Income";
            var ptWinLossTitle = "PT Win/Loss";
            var toteFeeTitle = "Tote Fee";
            var commExprenseTitle = "Comm Exprense";
            var netCommTitle = "Net Com";
            var dividendWonTitle = "Dividend Won";
            var amountDueTile = "Amount Due";
            var agencyTotalColumn = 5;
            var ibcTotalColumn = 9;
            var partnerTotalColumn = 8;

            var winLossSession = SessionExtension.GetSession(HttpContext.Current.Session, string.Format(_winLossRacingReportSessionKey, reportType));
            var reportResult = winLossSession != null
                                   ? (RacingWinlossBO)winLossSession
                                   : GetWinlossRacingReport(reportType, winlossDate);

            #endregion private variables

            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 1;
            var nextCol = 0;
            var reportName = string.Format("{0}-{1}", reportType, winlossDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportType.ToString());
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            #endregion excel format

            #region assign Header

            #region title

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + agencyTotalColumn + ibcTotalColumn + partnerTotalColumn + 2).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + agencyTotalColumn + ibcTotalColumn + partnerTotalColumn + 2).Value =
                string.Format("{0} - Date {1}", reportType.ToString().Replace("_", " "), winlossDate);

            #endregion title

            #region header - first row

            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol++)].Value = string.Empty;
            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol++)].Value = string.Empty;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1, startCol + nextCol + agencyTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1,
                                               startCol + nextCol + agencyTotalColumn).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1,
                                               startCol + (nextCol += agencyTotalColumn)).Value = agencyGroupTitle;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1, startCol + nextCol + ibcTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + nextCol + ibcTotalColumn).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + (nextCol += ibcTotalColumn)).Value = ibcGroupTitle;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1, startCol + nextCol + partnerTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + nextCol + partnerTotalColumn).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + (nextCol + partnerTotalColumn)).Value = partnerGroupTitle;

            #endregion header - first row

            #region header - second row

            nextCol = 0;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = poolCurrencyTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = currencyTile;

            #region Agency

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commIncomTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = ptWinLossTitle;

            #endregion Agency

            #region IBC

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commIncomTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = toteFeeTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commExprenseTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = ptWinLossTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = netCommTitle;

            #endregion IBC

            #region Parner

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = dividendWonTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commExprenseTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = toteFeeTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = amountDueTile;

            #endregion Parner

            #endregion header - second row

            #endregion assign Header

            #region Body

            if (reportResult != null && reportResult.Items.Count > 0)
            {
                foreach (var currentPool in reportResult.Items)
                {
                    HorseRacingDailyAssignDataForEachRow(true, ref wsReport, currentPool.Items, ref nextRow, startCol,
                                                         startRow, false, false, false, false);
                    //nextRow++;
                    HorseRacingDailyAssignDataForEachRow(true, ref wsReport, currentPool.Totals, ref nextRow, startCol,
                                                         startRow, true, false, false, false);
                    nextRow++;
                }

                #region comparision

                var isToteMatched = true;
                var isDividendMatched = true;
                if (reportResult.Comparision != null && reportResult.Comparision.Count > 0)
                {
                    foreach (RacingWinlossDumpFileBO racingWinlossDumpFileBo in reportResult.Comparision)
                    {
                        if (racingWinlossDumpFileBo.IsMatchTO == false)
                            isToteMatched = false;
                        if (racingWinlossDumpFileBo.IsMatchDividend == false)
                            isDividendMatched = false;
                    }
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 4,
                                                       startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 5).Merged =
                        true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 4,
                                                       startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 5).Style.
                        FillPattern.SetSolid(Color.Yellow);
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 4,
                                                       startRow + nextRow,
                                                       startCol + agencyTotalColumn + ibcTotalColumn + 2 + 5).Value =
                        "Tote Comparision";
                    nextRow++;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 2 + 4].Style.Font
                        .Color = isToteMatched ? Color.Blue : Color.Red;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 2 + 4].Value =
                        isToteMatched ? "OK" : "Not Matched";

                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 2 + 5].Style.Font
                        .Color = isDividendMatched ? Color.Blue : Color.Red;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 2 + 5].Value =
                        isDividendMatched ? "OK" : "Not Matched";
                    nextRow++;
                }

                #endregion comparision

                HorseRacingDailyAssignDataForEachRow(true, ref wsReport, reportResult.Totals, ref nextRow, startCol,
                                                         startRow, true, true, false, false);

                #region comparision data

                if (reportResult.Comparision != null && reportResult.Comparision.Count > 0)
                {
                    if (isDividendMatched == false || isToteMatched == false)
                    {
                        nextRow += 2;
                        ExportToExcelAssignComparisionData(ref wsReport, reportResult.Comparision, startRow, startCol,
                                                           ref nextRow, agencyTotalColumn, ibcTotalColumn);
                    }
                }

                #endregion comparision data
            }

            #endregion Body

            #region response file

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion response file
        }

        public void ExportToExcelWinLossRacingReportDetail(string winlossDate, string poolCurrency, string partnerId, string meetingName, Enumerations.ReportType reportType)
        {
            #region private variables

            var agencyGroupTitle = "Agency";
            var ibcGroupTitle = "IBC";
            var partnerGroupTitle = "Partner";
            var poolCurrencyTitle = "Pool-Currency";
            var currencyTile = "Currency";
            var RaceTitle = "Race";
            var winToTitle = "Win TO";
            var placeToTitle = "Place TO";
            var wpToTitle = "W/P TO";
            var totalToTitle = "Total TO";
            var commIncomTitle = "Comm Income";
            var ptWinLossTitle = "PT Win/Loss";
            var toteFeeTitle = "Tote Fee";
            var commExprenseTitle = "Comm Exprense";
            var netCommTitle = "Net Com";
            var dividendWonTitle = "Dividend Won";
            var amountDueTile = "Amount Due";
            var agencyTotalColumn = 5;
            var ibcTotalColumn = 9;
            var partnerTotalColumn = 8;

            var winLossSession = SessionExtension.GetSession(HttpContext.Current.Session, string.Format(_winLossRacingReportDetailSessionKey, poolCurrency, partnerId));
            var reportResult = winLossSession != null
                                   ? (RacingWinlossDetailsBO)winLossSession
                                   : GetWinlossRacingReportDetails(winlossDate, poolCurrency, partnerId);

            #endregion private variables

            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 1;
            var nextCol = 0;
            var reportName = string.Format("{0}-{1}", reportType, winlossDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}-{1}", reportType, winlossDate));
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            #endregion excel format

            #region assign Header

            #region title

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + agencyTotalColumn + ibcTotalColumn + partnerTotalColumn + 2).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + agencyTotalColumn + ibcTotalColumn + partnerTotalColumn + 2).Value =
                string.Format("{0} - {1} - Date {2}", reportType.ToString().Replace("_", " "), meetingName, winlossDate);

            #endregion title

            #region header - first row

            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol++)].Value = string.Empty;
            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol++)].Value = string.Empty;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1, startCol + nextCol + agencyTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1,
                                               startCol + nextCol + agencyTotalColumn).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow - 1,
                                               startCol + (nextCol += agencyTotalColumn)).Value = agencyGroupTitle;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1, startCol + nextCol + ibcTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + nextCol + ibcTotalColumn).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + (nextCol += ibcTotalColumn)).Value = ibcGroupTitle;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1, startCol + nextCol + partnerTotalColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + nextCol + partnerTotalColumn).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol + 1, startRow - 1,
                                               startCol + (nextCol + partnerTotalColumn)).Value = partnerGroupTitle;

            #endregion header - first row

            #region header - second row

            nextCol = 0;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = poolCurrencyTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = RaceTitle;

            #region Agency

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commIncomTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = ptWinLossTitle;

            #endregion Agency

            #region IBC

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commIncomTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = toteFeeTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commExprenseTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = ptWinLossTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = netCommTitle;

            #endregion IBC

            #region Parner

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = winToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = placeToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = wpToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = totalToTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = dividendWonTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = commExprenseTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = toteFeeTitle;
            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + (nextCol++)].Value = amountDueTile;

            #endregion Parner

            #endregion header - second row

            #endregion assign Header

            #region Body

            if (reportType == Enumerations.ReportType.Horse_Racing_Daily_Report_Detail_For_AU)
            {
                if (reportResult != null && reportResult.Items.Count > 0)
                {
                    foreach (var currentPool in reportResult.Items)
                    {
                        var curList = currentPool.Items.ToList();
                        foreach (RacingWinlossMeetingBO t in curList)
                        {
                            HorseRacingDailyAssignDataForEachRow_ForAust(false, ref wsReport, t.Items, ref nextRow, startCol,
                                                                 startRow, false, false, true, false);
                            HorseRacingDailyAssignDataForEachRow_ForAust(false, ref wsReport, t.Totals, ref nextRow, startCol,
                                                                 startRow, true, false, true, true);
                        }
                        //nextRow++;
                        HorseRacingDailyAssignDataForEachRow_ForAust(false, ref wsReport, currentPool.Totals, ref nextRow, startCol,
                                                             startRow, true, false, true, false);
                        nextRow++;
                    }
                    HorseRacingDailyAssignDataForEachRow_ForAust(false, ref wsReport, reportResult.Totals, ref nextRow, startCol,
                                                             startRow, true, true, true, false);
                    HorseRacingDailyAssignDataForEachRow_ForAust(false, ref wsReport, reportResult.GrandTotals, ref nextRow, startCol,
                                                             startRow, true, true, true, false);
                }
            }
            else
            {
                if (reportResult != null && reportResult.Items.Count > 0)
                {
                    foreach (var currentPool in reportResult.Items)
                    {
                        var curList = currentPool.Items.ToList();
                        foreach (RacingWinlossMeetingBO t in curList)
                        {
                            HorseRacingDailyAssignDataForEachRow(false, ref wsReport, t.Items, ref nextRow, startCol,
                                                                 startRow, false, false, true, false);
                            HorseRacingDailyAssignDataForEachRow(false, ref wsReport, t.Totals, ref nextRow, startCol,
                                                                 startRow, true, false, true, true);
                        }
                        //nextRow++;
                        HorseRacingDailyAssignDataForEachRow(false, ref wsReport, currentPool.Totals, ref nextRow, startCol,
                                                             startRow, true, false, true, false);
                        nextRow++;
                    }
                    HorseRacingDailyAssignDataForEachRow(false, ref wsReport, reportResult.Totals, ref nextRow, startCol,
                                                             startRow, true, true, true, false);
                    HorseRacingDailyAssignDataForEachRow(false, ref wsReport, reportResult.GrandTotals, ref nextRow, startCol,
                                                             startRow, true, true, true, false);
                }
            }
            wsReport.Columns[0].Width = 6000;

            #endregion Body

            #region response file

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));

            excelFile.SaveXls(response.OutputStream);

            #endregion response file
        }

        private static void ExportToExcelAssignComparisionData(ref ExcelWorksheet wsReport, List<RacingWinlossDumpFileBO> comparisionData,
            int startRow, int startCol, ref int nextRow, int agencyTotalColumn, int ibcTotalColumn)
        {
            if (comparisionData != null)
            {
                var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
                var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false);
                var bgNotMatched = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), Color.Red);
                var bgOk = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), Color.Blue);

                #region header

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 3,
                                                   startRow + nextRow + 1,
                                                   startCol + agencyTotalColumn + ibcTotalColumn + 3).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 3,
                                                   startRow + nextRow + 1,
                                                   startCol + agencyTotalColumn + ibcTotalColumn + 3).Style =
                    bgHeaderNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 3,
                                                   startRow + nextRow + 1,
                                                   startCol + agencyTotalColumn + ibcTotalColumn + 3).Value = "Currency";

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5)
                    .Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5)
                    .Style = bgHeaderNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5)
                    .Value = "Partner";

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7)
                    .Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7)
                    .Style = bgHeaderNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6,
                                                   startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7)
                    .Value = "Company";

                nextRow++;

                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4].Style =
                    bgHeaderNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4].Value =
                    "TO";
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5].Style =
                    bgHeaderNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5].Value =
                    "Dividend WON ";
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6].Style =
                    bgHeaderNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6].Value =
                    "TO";
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7].Style =
                    bgHeaderNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7].Value =
                    "Dividend WON ";
                nextRow++;

                #endregion header

                #region assign data

                foreach (var comparision in comparisionData)
                {
                    var to = CastAide.AsDouble(comparision.ibcTO) == CastAide.AsDouble(comparision.partnerTO) ? true : false;
                    var devidend = CastAide.AsDouble(comparision.ibcDividendWon) == CastAide.AsDouble(comparision.partnerDividendWon) ? true : false;

                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 3].Style =
                        bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 3].Value =
                        comparision.currency;

                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4].Style =
                        to ? bgOk : bgNotMatched;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 4].Value =
                        CastAide.AsDouble(comparision.partnerTO);
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5].Style =
                        devidend ? bgOk : bgNotMatched;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 5].Value =
                        CastAide.AsDouble(comparision.partnerDividendWon);
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6].Style =
                        to ? bgOk : bgNotMatched;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 6].Value =
                        CastAide.AsDouble(comparision.ibcTO);
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7].Style =
                        devidend ? bgOk : bgNotMatched;
                    wsReport.Cells[startRow + nextRow, startCol + agencyTotalColumn + ibcTotalColumn + 7].Value =
                        CastAide.AsDouble(comparision.ibcDividendWon);

                    nextRow++;
                }

                #endregion assign data
            }
        }

        #region Horse Racing Daily - assign for each row

        private static List<string> GetDistinctCurrency(List<RacingWinlossRowBO> pool)
        {
            var listDistinctMeeting = new List<string>();
            foreach (RacingWinlossRowBO racingWinlossRowBo in pool)
            {
                if (!CheckItemExistedInArray(listDistinctMeeting, racingWinlossRowBo.Currency))
                    listDistinctMeeting.Add(racingWinlossRowBo.Currency);
            }
            return listDistinctMeeting;
        }

        private static void HorseRacingDailyAssignDataForEachRow(bool isMaster, ref ExcelWorksheet wsReport, List<RacingWinlossRowBO> pool, ref int nextRow, int startCol, int startRow, bool isTotal, bool isFooter, bool isReportDetail, bool isTotalDetail)
        {
            var bgColor = isFooter ? Color.FromArgb(205, 222, 255)
                : isTotalDetail ? Color.FromArgb(255, 238, 221)
                : isTotal ? Color.FromArgb(211, 225, 200)
                 : Color.FromArgb(255, 255, 255);

            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal, false);
            var normalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor, isTotal, false);
            int _nextRow = 0;
            var listDistinctMeeting = GetDistinctMeeting(pool);
            if (!isFooter)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).
                    Style = bgFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).Value = pool.ToList()[0].Name;
            }
            else
            {
                int currentRow = startRow + nextRow;
                _nextRow = nextRow;
                for (int i = 0; i < listDistinctMeeting.Count; i++)
                {
                    if (listDistinctMeeting[i].Trim() == "Grand Total")
                    {
                        if (isMaster)
                        {
                            var listCurrency = GetDistinctCurrency(pool);
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow - 1 + listCurrency.Count, 0).
                                Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow - 1 + listCurrency.Count, 0).
                                Style = bgFormat;
                        }
                        else
                        {
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow + 1, 0).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow + 1, 0).Style = bgFormat;
                        }
                    }
                    else
                    {
                        var _bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(221, 235, 255), isTotal, false);
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 0).Style = _bgFormat;
                    }
                    wsReport.Rows[currentRow].Cells[0].Value = listDistinctMeeting[i];
                    ++currentRow;
                }
            }
            int _nextCol = 0;
            foreach (var currentRow in pool.ToList())
            {
                var nextCol = 1;

                #region assign data for each currency

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = normalFormat;
                if (isReportDetail && !isTotal && !isFooter)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;

                    if (string.IsNullOrEmpty(currentRow.RaceNo))
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = string.Empty;
                    else
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsInt32(currentRow.RaceNo);
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = currentRow.Currency;
                }

                //wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = isReportDetail ? currentRow.RaceNo : currentRow.Currency;

                #region Agency

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyPTWL);

                #endregion Agency

                #region IBC

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCToteFee);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCPTWL);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCNetIncome);

                #endregion IBC

                #region Parner

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerDividendWon);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerToteFee);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(currentRow.PartnerAmountDue);

                #endregion Parner

                #endregion assign data for each currency

                _nextCol = nextCol;
                nextRow++;
            }
            if (isFooter)
            {
                int currentRow = startRow + _nextRow;
                for (int i = 0; i < listDistinctMeeting.Count; i++)
                {
                    if (listDistinctMeeting[i].Trim() != "Grand Total")
                    {
                        var _bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(221, 235, 255), isTotal, false);
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, _nextCol).Style = _bgFormat;
                    }
                    ++currentRow;
                }
            }
        }

        private static void HorseRacingDailyAssignDataForEachRow_ForAust(bool isMaster, ref ExcelWorksheet wsReport, List<RacingWinlossRowBO> pool, ref int nextRow, int startCol, int startRow, bool isTotal, bool isFooter, bool isReportDetail, bool isTotalDetail)
        {
            if (isFooter)
            {
                List<RacingWinlossRowBO> listPool = new List<RacingWinlossRowBO>();
                foreach (var p in pool)
                {
                    if (!p.Currency.Contains("Base"))
                    {
                        listPool.Add(p);
                    }
                }
                foreach (var p in pool)
                {
                    if (!p.Currency.Contains("Base"))
                    {
                        continue;
                    }
                    listPool.Add(p);
                    break;
                }
                pool = listPool;
            }
            var bgColor = isFooter ? Color.FromArgb(205, 222, 255)
                : isTotalDetail ? Color.FromArgb(255, 238, 221)
                : isTotal ? Color.FromArgb(211, 225, 200)
                 : Color.FromArgb(255, 255, 255);

            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal, false);
            var normalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor, isTotal, false);
            int _nextRow = 0;
            var listDistinctMeeting = GetDistinctMeeting(pool);
            if (!isFooter)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).
                    Style = bgFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + pool.Count - 1, startCol).Value = pool.ToList()[0].Name;
            }
            else
            {
                int currentRow = startRow + nextRow;
                _nextRow = nextRow;
                for (int i = 0; i < listDistinctMeeting.Count; i++)
                {
                    if (listDistinctMeeting[i].Trim() == "Grand Total")
                    {
                        if (isMaster)
                        {
                            var listCurrency = GetDistinctCurrency(pool);
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow - 1 + listCurrency.Count, 0).
                                Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow - 1 + listCurrency.Count, 0).
                                Style = bgFormat;
                        }
                        else
                        {
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow + 1, 0).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow + 1, 0).Style = bgFormat;
                        }
                    }
                    else
                    {
                        var _bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(221, 235, 255), isTotal, false);
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 0).Style = _bgFormat;
                    }
                    wsReport.Rows[currentRow].Cells[0].Value = listDistinctMeeting[i];
                    ++currentRow;
                }
            }
            int _nextCol = 0;
            foreach (var currentRow in pool.ToList())
            {
                var nextCol = 1;

                #region assign data for each currency

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = normalFormat;
                if (isReportDetail && !isTotal && !isFooter)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;

                    if (string.IsNullOrEmpty(currentRow.RaceNo))
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = string.Empty;
                    else
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsInt32(currentRow.RaceNo);
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = currentRow.Currency;
                }

                //wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = isReportDetail ? currentRow.RaceNo : currentRow.Currency;

                #region Agency

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.AgencyPTWL);

                #endregion Agency

                #region IBC

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCToteFee);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCPTWL);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.IBCNetIncome);

                #endregion IBC

                #region Parner

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerWinTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerWPTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerDividendWon);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Value = CastAide.AsDouble(currentRow.PartnerToteFee);
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(currentRow.PartnerAmountDue);

                #endregion Parner

                #endregion assign data for each currency

                _nextCol = nextCol;
                nextRow++;
            }
            if (isFooter)
            {
                int currentRow = startRow + _nextRow;
                for (int i = 0; i < listDistinctMeeting.Count; i++)
                {
                    if (listDistinctMeeting[i].Trim() != "Grand Total")
                    {
                        var _bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(221, 235, 255), isTotal, false);
                        wsReport.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, _nextCol).Style = _bgFormat;
                    }
                    ++currentRow;
                }
            }
        }

        #endregion Horse Racing Daily - assign for each row

        #region Racing Group

        public void ExportToExcelWinLossRacingGroup(string fromDate, string toDate)
        {
            try
            {
                RacingGroupMain(fromDate, toDate, true, false);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        private void RacingGroupMain(string fromDate, string toDate, bool isExport, bool isSendMail, string To, string Cc, string Subject, string Content)
        {
            #region local variables

            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var reportName = string.Format("{0}-From-{1}-To-{2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily").Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"));

            #endregion local variables

            #region Assign Header

            #region report title

            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"), fromDate, toDate);

            #endregion report title

            RacingGroupAssignHeader(ref wsReport, startRow, startCol);

            #endregion Assign Header

            #region Assign Content

            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDTO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey) :
                GetRacingWinlossGroup(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {
                foreach (RacingWinlossGroupDayBO item in getSession.Items)
                {
                    var totalList = new List<RacingWinlossGroupRowBO>();
                    totalList.Add(item.Total);

                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        item.Items, false, false);
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        totalList, true, false);
                }
                if (getSession.Total != null)
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        getSession.Total, false, true);
            }

            #endregion Assign Content

            #region response

            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);

                #region mail setting

                var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily");
                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDaily"];

                #endregion mail setting

                //SendMailExtension.SendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
                //                           emailSubject, emailContent, memoryStream, string.Format("{0}.xls", reportName));

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                {
                    Content = memoryStream,
                    ContentType = "application/ms-excel",
                    FileName = string.Format("{0}.xls", reportName)
                };

                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, To, Cc, title, Subject, Content, mailAttachment);
            }

            #endregion response
        }

        //private void RacingGroupMain(string fromDate, string toDate, bool isExport, bool isSendMail)
        //{
        //    #region local variables
        //    var startCol = 0;
        //    var startRow = 3;
        //    var nextRow = 0;
        //    var reportName = string.Format("{0}-From-{1}-To-{2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily").Replace(" ", ""), fromDate, toDate);
        //    var excelFile = new ExcelFile();
        //    var wsReport = excelFile.Worksheets.Add(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"));
        //    #endregion

        //    #region Assign Header
        //    #region report title
        //    wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
        //    wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"), fromDate, toDate);
        //    #endregion
        //    RacingGroupAssignHeader(ref wsReport, startRow, startCol);
        //    #endregion
        //    #region Assign Content
        //    var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
        //        != null ? (RacingWinlossGroupDTO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey) :
        //        GetRacingWinlossGroup(fromDate, toDate);
        //    if (getSession != null && getSession.Items.Count > 0)
        //    {
        //        foreach (RacingWinlossGroupDayBO item in getSession.Items)
        //        {
        //            var totalList = new List<RacingWinlossGroupRowBO>();
        //            totalList.Add(item.Total);

        //            RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
        //                item.Items, false, false);
        //            RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
        //                totalList, true, false);
        //        }
        //        if (getSession.Total != null)
        //            RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
        //                getSession.Total, false, true);
        //    }
        //    #endregion

        //    #region response
        //    if (isExport)
        //    {
        //        var response = HttpContext.Current.Response;
        //        response.Clear();
        //        response.ContentType = "application/vnd.ms-excel";
        //        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
        //        excelFile.SaveXls(response.OutputStream);
        //    }
        //    else if (isSendMail)
        //    {
        //        var memoryStream = new MemoryStream();
        //        excelFile.SaveXls(memoryStream);
        //        #region mail setting
        //        var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily");
        //        var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
        //        var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
        //        var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDaily"];
        //        #endregion

        //        //SendMailExtension.SendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
        //        //                           emailSubject, emailContent, memoryStream, string.Format("{0}.xls", reportName));

        //        var mailAttachment = new MailAttachment()
        //        {
        //            Content = memoryStream,
        //            ContentType = "application/ms-excel",
        //            FileName = string.Format("{0}.xls", reportName)
        //        };
        //        SendMailExtension.AccSendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title, emailSubject,
        //                                      emailContent, mailAttachment);
        //    }
        //    #endregion
        //}
        //private void RacingGroupAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        //{
        //    #region local variables
        //    var dateTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date");
        //    var partnerTypeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PartnerType");
        //    var totalInComeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalIncome");
        //    var groupTotalInCome = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupTotalInCome");
        //    var turnOverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
        //    var ibcTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIbc");
        //    var onetwoBetTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lbl12Bet");
        //    var partnerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPartner");
        //    var agencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgency");
        //    var totalToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
        //    var ptTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPT");
        //    var netCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNetComm");
        //    var devidendWonTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDividendWon");
        //    var commExenseTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommExpense");
        //    var feeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFee");
        //    var amountDueTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAmountDue");
        //    var commTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
        //    var nextCol = 1;
        //    var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
        //    var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
        //    #endregion

        //    #region first row
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = string.Empty;
        //    nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", turnOverTitle);
        //    //nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", ibcTitle);
        //    //nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", onetwoBetTitle);

        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + (nextCol += 1)).Value = string.Format("{0}", groupTotalInCome);

        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 5).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 5).Style = bgHeaderNormalFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol + 5)).Value = string.Format("{0} (RM)", partnerTitle);
        //    #endregion

        //    #region second row
        //    nextCol = 1;
        //    wsReport.Cells[startRow - 1, startCol].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol].Value = dateTitle;
        //    wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + nextCol].Value = partnerTypeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = agencyTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ibcTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = partnerTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalToTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = netCommTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 2)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = turnOverTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = devidendWonTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commExenseTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = feeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = amountDueTitle;
        //    #endregion

        //}
        //private void RacingGroupAssignContent(ref ExcelWorksheet wsReport, int startRow,
        //    int startCol, ref int nextRow, List<RacingWinlossGroupRowBO> reportContent, bool isTotal, bool isFooter)
        //{
        //    #region local variables
        //    //var bgColor = isFooter ? Color.FromArgb(205, 222, 255) : isTotal ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
        //    var bgColor = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
        //    var bgGrandTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(205, 222, 255), true, false);
        //    var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal || isFooter ? true : false, false);
        //    var nextCol = 1;
        //    var currentRow = 0;
        //    #endregion

        //    foreach (var item in reportContent)
        //    {
        //        nextCol = 1;
        //        bgFormat = (isFooter && currentRow == reportContent.Count - 1) ? bgGrandTotalFormat : bgFormat;

        //        #region left columns
        //        if (isFooter || isTotal)
        //        {
        //            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Merged = true;
        //            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Style = bgFormat;
        //            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Value = item.Name;
        //        }
        //        else
        //        {
        //            if (currentRow == 0)
        //            {
        //                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + reportContent.Count - 1, startCol).Merged = true;
        //                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + reportContent.Count - 1, startCol).Style = bgFormat;
        //                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + (nextRow + reportContent.Count - 1), startCol).Value = item.WinlostDate;
        //            }
        //            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
        //            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = item.Name;
        //        }
        //        #endregion

        //        #region right columns
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverAgency);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverIBC);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.Turnover3rd);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverTotalTO);

        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCPT);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCNetComm);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCTotalIncome);

        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetPT);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetComm);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetTotalIncome);

        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.GroupTotalIncome);

        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerTurnover);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerDividendWon);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerCommExpense);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerFees);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
        //        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerAmountDueto);
        //        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
        //        #endregion

        //        currentRow++;
        //        nextRow++;
        //    }
        //}

        #endregion Racing Group

        #region Racing Group For Tote

        //public void ExportToExcelWinLossRacingGroupForTote(string fromDate, string toDate)
        //{
        //    RacingGroupForToteMain(fromDate, toDate, true, false);
        //}

        //private void RacingGroupForToteMain(string fromDate, string toDate, bool isExport, bool isSendMail)
        //{
        //    #region local variables
        //    var startCol = 0;
        //    var startRow = 3;
        //    var nextRow = 0;
        //    var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyForTote");
        //    var reportName = string.Format("{0}-From-{1}-To-{2}", title.Replace(" ", ""), fromDate, toDate);
        //    var excelFile = new ExcelFile();
        //    var wsReport = excelFile.Worksheets.Add(title);
        //    #endregion

        //    #region Assign Header
        //    #region report title
        //    wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
        //    wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", title, fromDate, toDate);
        //    #endregion
        //    RacingGroupForToteAssignHeader(ref wsReport, startRow, startCol);
        //    #endregion
        //    #region Assign Content
        //    var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
        //        != null ? (RacingWinlossGroupDayBO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupForToteReportSessionKey) :
        //        GetRacingWinlossGroupForTote(fromDate, toDate);
        //    if (getSession != null && getSession.Items.Count > 0)
        //    {
        //        RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow, getSession.Items
        //                , false, false);

        //        if (getSession.Total != null)
        //        {
        //            var total = new List<RacingWinlossGroupRowBO>();
        //            total.Add(getSession.Total);

        //            RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
        //                                            total, false, true);
        //        }
        //    }
        //    #endregion

        //    #region response
        //    if (isExport)
        //    {
        //        var response = HttpContext.Current.Response;
        //        response.Clear();
        //        response.ContentType = "application/vnd.ms-excel";
        //        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
        //        excelFile.SaveXls(response.OutputStream);
        //    }
        //    else if (isSendMail)
        //    {
        //        var memoryStream = new MemoryStream();
        //        excelFile.SaveXls(memoryStream);
        //        #region mail setting

        //        var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
        //        var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
        //        var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDailyForTote"];
        //        #endregion
        //        //SendMailExtension.SendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
        //        //                           emailSubject, emailContent, memoryStream, string.Format("{0}.xls", reportName));

        //        var mailAttachment = new MailAttachment()
        //        {
        //            Content = memoryStream,
        //            ContentType = "application/ms-excel",
        //            FileName = string.Format("{0}.xls", reportName)
        //        };
        //        SendMailExtension.AccSendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
        //                                      emailSubject, emailContent, mailAttachment);
        //    }
        //    #endregion
        //}

        private void RacingGroupForToteMain(string fromDate, string toDate, bool isExport, bool isSendMail, string To, string Cc, string Subject, string Content)
        {
            #region local variables

            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyForTote");
            var reportName = string.Format("{0}-From-{1}-To-{2}", title.Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(title);

            #endregion local variables

            #region Assign Header

            #region report title

            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", title, fromDate, toDate);

            #endregion report title

            RacingGroupForToteAssignHeader(ref wsReport, startRow, startCol);

            #endregion Assign Header

            #region Assign Content

            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDayBO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupForToteReportSessionKey) :
                GetRacingWinlossGroupForTote(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {
                RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow, getSession.Items
                        , false, false);

                if (getSession.Total != null)
                {
                    var total = new List<RacingWinlossGroupRowBO>();
                    total.Add(getSession.Total);

                    RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                    total, false, true);
                }
            }

            #endregion Assign Content

            #region response

            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);

                #region mail setting

                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDailyForTote"];

                #endregion mail setting

                //SendMailExtension.SendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
                //                           emailSubject, emailContent, memoryStream, string.Format("{0}.xls", reportName));

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                {
                    Content = memoryStream,
                    ContentType = "application/ms-excel",
                    FileName = string.Format("{0}.xls", reportName)
                };
                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, To, Cc, title,
                                              Subject, Content, mailAttachment);
            }

            #endregion response
        }

        //private void RacingGroupForToteAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        //{
        //    #region local variables
        //    var dateTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date");
        //    var totalInComeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalIncome");
        //    var groupTotalInCome = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupTotalInCome");
        //    var turnOverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
        //    var ibcTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIbc");
        //    var onetwoBetTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lbl12Bet");
        //    var agencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgency");
        //    var totalToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
        //    var ptTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPT");
        //    var netCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNetComm");
        //    var commTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
        //    var bettypeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Bettype");
        //    var partnerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPartner");
        //    var nextCol = 1;
        //    var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
        //    var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
        //    #endregion

        //    #region first row
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = string.Empty;
        //    nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", turnOverTitle);
        //    //nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", ibcTitle);
        //    //nextCol++;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", onetwoBetTitle);

        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Merged = true;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Style = bgHeaderIbcFormat;
        //    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + (nextCol += 1)).Value = string.Format("{0}", groupTotalInCome);
        //    #endregion

        //    #region second row
        //    nextCol = 1;
        //    wsReport.Cells[startRow - 1, startCol].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol].Value = dateTitle;
        //    wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + nextCol].Value = bettypeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = agencyTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ibcTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = partnerTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalToTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = netCommTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commTitle;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
        //    wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
        //    #endregion

        //}
        //private void RacingGroupForToteAssignContent(ref ExcelWorksheet wsReport, int startRow,
        //    int startCol, ref int nextRow, List<RacingWinlossGroupRowBO> reportContent, bool isTotal, bool isFooter)
        //{
        //    #region local variables
        //    var bgColor = isFooter ? Color.FromArgb(205, 222, 255) : Color.FromArgb(255, 255, 255);
        //    var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal || isFooter ? true : false, false);
        //    var nextCol = 1;
        //    var currentRow = 0;
        //    ExcelWorksheet wsWinLoss = wsReport;
        //    var nextrow = nextRow;
        //    #endregion

        //    foreach (var item in reportContent)
        //    {
        //        nextCol = 1;
        //        #region left columns
        //        if (isFooter || isTotal)
        //        {
        //            wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Merged = true;
        //            wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Style = bgFormat;
        //            wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Value = item.Name;
        //        }
        //        else
        //        {
        //            wsWinLoss.Cells[startRow + nextrow, startCol].Style = bgFormat;
        //            wsWinLoss.Cells[startRow + nextrow, startCol].Value = item.WinlostDate;
        //            wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol)].Style = bgFormat;
        //            wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = item.Name;
        //        }

        //        #endregion

        //        #region right columns
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverAgency);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverIBC);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.Turnover3rd);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverTotalTO);

        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCPT);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCNetComm);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCTotalIncome);

        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetPT);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetComm);
        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetTotalIncome);

        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
        //        wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.GroupTotalIncome);

        //        wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol)].Style = bgFormat;
        //        #endregion

        //        currentRow++;
        //        nextrow++;
        //    }

        //    nextRow = nextrow;
        //    wsReport = wsWinLoss;
        //}

        #endregion Racing Group For Tote

        #region Racing daily report for tote

        public void ExportToExcelHeaderWinlossRacingLicenseeDetailsPt(ref ExcelWorksheet wsReport, int startRow, int startCol, string site)
        {
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderNormalFormatMember = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(224, 244, 170));
            var bgHeaderNormalFormatLicensee = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(204, 255, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var nextCol = 0;

            #region width of cols

            wsReport.Columns[0].Width = 25 * 256;
            wsReport.Columns[1].Width = 10 * 256;
            wsReport.Columns[2].Width = 10 * 256;
            wsReport.Columns[3].Width = 10 * 256;
            wsReport.Columns[4].Width = 10 * 256;
            wsReport.Columns[5].Width = 10 * 256;
            wsReport.Columns[6].Width = 15 * 256;
            wsReport.Columns[7].Width = 10 * 256;
            wsReport.Columns[8].Width = 10 * 256;
            wsReport.Columns[9].Width = 10 * 256;
            wsReport.Columns[10].Width = 10 * 256;
            wsReport.Columns[11].Width = 15 * 256;
            wsReport.Columns[12].Width = 15 * 256;
            wsReport.Columns[13].Width = 15 * 256;
            wsReport.Columns[14].Width = 15 * 256;
            wsReport.Columns[15].Width = 15 * 256;

            #endregion width of cols

            #region Assign header

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMeetingCurrency");

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Race"); ;

            #region Member

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 4)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 4)).Style = bgHeaderNormalFormatMember;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 4)).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MemberOverall");

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormatMember;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWinTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlaceTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWPTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDividendWon");

            #endregion Member

            #region Licensee

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 5)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 5)).Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 5)).Value = "Licensee (" + site + ")";

            //Begin IBC
            if (WebSettings.IsApply50PercentRacingReport)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 6), startRow - 1, startCol + (nextCol + 7)).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 6), startRow - 1, startCol + (nextCol + 7)).Style = bgHeaderNormalFormatMember;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol + 6), startRow - 1, startCol + (nextCol + 7)).Value = "IBC";
            }
            //End IBC

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWinTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlaceTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWPTO");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");

            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PTWinloss");
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatLicensee;
            wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommIncome");

            //begin IBC
            if (WebSettings.IsApply50PercentRacingReport)
            {
                wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
                wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PTWinloss");
                wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormatMember;
                wsReport.Cells[startRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommIncome");
            }
            //End IBC

            #endregion Licensee

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAmountduetoIBC");

            #endregion Assign header
        }

        public void ExportToExcelContentWinlossRacingLicenseeDetailsPt(ref ExcelWorksheet wsReport, int startRow, int startCol,
            ref int nextRow, bool isMeetingTotal, bool isPoolCurrencyTotal, bool isFooter, List<RacingDailyBySiteRow> racingDailyBySiteRows)
        {
            var bgColor = isFooter ? Color.FromArgb(205, 222, 255) : (isMeetingTotal ? Color.FromArgb(255, 238, 221) : (isPoolCurrencyTotal ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255)));
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isFooter || isMeetingTotal || isPoolCurrencyTotal ? true : false, false);
            var normalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor, isFooter || isMeetingTotal || isPoolCurrencyTotal ? true : false, false);

            var currentRow = 0;

            foreach (RacingDailyBySiteRow item in racingDailyBySiteRows)
            {
                var nextCol = 0;

                #region left cols

                if (isMeetingTotal || isFooter)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                       startCol + (nextCol + 1)).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                       startCol + (nextCol + 1)).Style = bgFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                       startCol + (nextCol + 1)).Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                       startCol + (nextCol += 1)).Value = string.Format("{0} {1}",
                                                                                                        item.Name,
                                                                                                        isMeetingTotal
                                                                                                            ? "Total"
                                                                                                            : string.
                                                                                                                  Empty);
                }
                else
                {
                    if (currentRow == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + racingDailyBySiteRows.Count - 1, startCol).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + racingDailyBySiteRows.Count - 1, startCol).
                            Style
                            = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + racingDailyBySiteRows.Count - 1,
                                                           startCol).
                            Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + racingDailyBySiteRows.Count - 1,
                                                           startCol).
                            Value
                            = item.Name + (!isPoolCurrencyTotal ? string.Format(" - {0}", item.CurrencyName) : string.Empty);
                    }

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = normalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    if (isPoolCurrencyTotal)
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = item.CurrencyName;
                    else
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsInt32(item.Race);
                }

                #endregion left cols

                #region right cols

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.WinTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.PlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.WinPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.TotalTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.DividendWon);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.AgentWinTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.AgentPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.AgentWinPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.AgentTotalTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.AgentPtWinLoss);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.CommIncome);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;

                //Begin IBC
                if (WebSettings.IsApply50PercentRacingReport)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.IBCPTWL);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.IBCCommIncomm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgFormat;
                }
                //End iBC

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.Amountdueto);

                #endregion right cols

                currentRow++;
                nextRow++;
            }
        }

        public void ExportToExcelWinlossRacingLicenseeDetailsPt(string winlossDate, string site, string reportTitle)
        {
            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 1;
            var nextCol = 0;

            var reportName = reportTitle.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                "lblRacingWinlossBySite").Replace(" ", "")));

            #endregion excel format

            #region header

            wsReport.Cells[startRow - 2, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 2, startCol].Value = reportTitle;
            ExportToExcelHeaderWinlossRacingLicenseeDetailsPt(ref wsReport, startRow, startCol, site);

            #endregion header

            #region content

            var getReportData = SessionExtension.GetSession(HttpContext.Current.Session,
                                                                            string.Format(
                                                                                _winLossRacingReportLicensseSessionKey,
                                                                                site)) != null
                                                    ? (RacingDailyBySiteBO)
                                                      SessionExtension.GetSession(HttpContext.Current.Session,
                                                                                  string.Format(
                                                                                      _winLossRacingReportLicensseSessionKey,
                                                                                      site))
                                                    : WinlostRacingLicensee_Details(winlossDate, site);

            if (getReportData != null && getReportData.Items != null && getReportData.Items.Count > 0)
            {
                //all pool currencies
                foreach (RacingDailyBySiteCurrency poolCurrencyGroup in getReportData.Items)
                {
                    //all meeting for each pool currency
                    foreach (RacingDailyBySiteMeeting meetingGroup in poolCurrencyGroup.Items)
                    {
                        //all of race - detail
                        ExportToExcelContentWinlossRacingLicenseeDetailsPt(ref wsReport, startRow, startCol, ref nextRow,
                                                                         false, false, false,
                                                                         meetingGroup.Items);
                        //all of race - total
                        var listTotalForEachMeeting = new List<RacingDailyBySiteRow>();
                        listTotalForEachMeeting.Add(meetingGroup.Total);
                        ExportToExcelContentWinlossRacingLicenseeDetailsPt(ref wsReport, startRow, startCol, ref nextRow,
                                                                         true, false, false,
                                                                         listTotalForEachMeeting);
                    }
                    //all metting - total
                    ExportToExcelContentWinlossRacingLicenseeDetailsPt(ref wsReport, startRow, startCol, ref nextRow,
                                                                         false, true, false,
                                                                         poolCurrencyGroup.Totals);
                    nextRow++;
                }
                //footer

                var listFooter = new List<RacingDailyBySiteRow>();
                listFooter.Add(getReportData.Total);
                ExportToExcelContentWinlossRacingLicenseeDetailsPt(ref wsReport, startRow, startCol, ref nextRow,
                                                                         false, false, true,
                                                                         listFooter);
            }

            #endregion content

            #region response file

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion response file
        }

        public void ExportToExcelRacingDailyReportForTote(string reportTitle, string winlostDate)
        {
            #region local variables

            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var reportName = "RacingDailyReportForTote";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add("RacingDailyReportForTote");

            #endregion local variables

            #region Assign Header

            #region report title

            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 24).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 24).Style.Font.Weight =
                ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 24).Value = reportTitle;

            #endregion report title

            ExportToExcelRacingDailyReportForToteAssignHeader(ref wsReport, startRow, startCol);

            #endregion Assign Header

            #region content

            var getData =
                SessionExtension.GetSession(HttpContext.Current.Session, _RacingDailyWinLossForToteSessionKey) != null
                    ? (RacingDailyWinLossDTO)
                      SessionExtension.GetSession(HttpContext.Current.Session, _RacingDailyWinLossForToteSessionKey)
                    : SelWinlostHR_Details(winlostDate);

            if (getData != null && getData.Items.Count > 0)
            {
                foreach (RacingDailyWinLossByCurrency racingByCurrency in getData.Items)
                {
                    foreach (RacingDailyWinLossByMeetingCurrency racingByMeeting in racingByCurrency.Items)
                    {
                        ExportToExcelRacingDailyReportForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                           racingByMeeting.Items, false, false,
                                                                           false, false, false);
                        ExportToExcelRacingDailyReportForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                           racingByMeeting.Totals, true, false,
                                                                           false, false, true);
                    }

                    if (racingByCurrency.Totals != null && racingByCurrency.Totals.Count > 0)
                        ExportToExcelRacingDailyReportForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                               racingByCurrency.Totals, false, true,
                                                                               false, false, true);
                    if (racingByCurrency.GroupTotals != null && racingByCurrency.GroupTotals.Count > 0)
                        ExportToExcelRacingDailyReportForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                               racingByCurrency.GroupTotals, false, false,
                                                                               true, false, true);
                }
                //nextRow++;
                ExportToExcelRacingDailyReportForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                           getData.Totals, false, false,
                                                                           false, true, true);
            }

            #endregion content

            #region response

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion response
        }

        private static void ExportToExcelRacingDailyReportForToteAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            #region local variables

            var meetingCurrencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMeetingCurrency");
            var agencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgency");
            var ibcTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIbc");
            var plumelelaTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "plumelelaTitle");

            var raceTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Race");

            var winToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWinTO");
            var placeToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlaceTO");
            var wpToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWPTO");
            var totalToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
            var comincomeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommIncome");
            var ptWinLossTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPTWinLoss");

            var commExpenseTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommExpense");
            var pgiAmtoteFeeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPGIAmtoteFee");
            var netCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNetComm");

            var dividendWonTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDividendWon");
            var amountDueTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAmountDue");

            var nextCol = 1;
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var agencyMergeCols = 5;
            var ibcMergeCols = 8;
            var plumelelaMergeCols = 7;

            #endregion local variables

            #region set with for each col

            wsReport.Columns[startCol].Width = 25 * 256;
            wsReport.Columns[startCol + 1].Width = 10 * 256;
            wsReport.Columns[startCol + 2].Width = 14 * 256;
            wsReport.Columns[startCol + 3].Width = 14 * 256;
            wsReport.Columns[startCol + 4].Width = 14 * 256;
            wsReport.Columns[startCol + 5].Width = 14 * 256;
            wsReport.Columns[startCol + 6].Width = 14 * 256;
            wsReport.Columns[startCol + 7].Width = 14 * 256;
            wsReport.Columns[startCol + 8].Width = 14 * 256;
            wsReport.Columns[startCol + 9].Width = 14 * 256;
            wsReport.Columns[startCol + 10].Width = 14 * 256;
            wsReport.Columns[startCol + 11].Width = 14 * 256;
            wsReport.Columns[startCol + 12].Width = 14 * 256;
            wsReport.Columns[startCol + 13].Width = 14 * 256;
            wsReport.Columns[startCol + 14].Width = 14 * 256;
            wsReport.Columns[startCol + 15].Width = 14 * 256;
            wsReport.Columns[startCol + 16].Width = 14 * 256;
            wsReport.Columns[startCol + 17].Width = 14 * 256;
            wsReport.Columns[startCol + 18].Width = 14 * 256;
            wsReport.Columns[startCol + 19].Width = 14 * 256;
            wsReport.Columns[startCol + 20].Width = 14 * 256;
            wsReport.Columns[startCol + 21].Width = 14 * 256;
            wsReport.Columns[startCol + 22].Width = 14 * 256;
            wsReport.Columns[startCol + 23].Width = 14 * 256;
            wsReport.Columns[startCol + 24].Width = 14 * 256;

            #endregion set with for each col

            #region first row

            wsReport.Cells[startRow - 2, startCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 2, startCol].Value = string.Empty;
            wsReport.Cells[startRow - 2, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 2, startCol + nextCol].Value = string.Empty;
            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + agencyMergeCols)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + agencyMergeCols)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol += agencyMergeCols)).Value = agencyTitle;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + ibcMergeCols)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + ibcMergeCols)).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol += ibcMergeCols)).Value = ibcTitle;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + plumelelaMergeCols)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + plumelelaMergeCols)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2,
                                               startCol + (nextCol + plumelelaMergeCols)).Value = plumelelaTitle;

            #endregion first row

            #region second row

            nextCol = 1;
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol].Value = meetingCurrencyTitle;
            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = raceTitle;

            #region Agency

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = winToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = placeToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = wpToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = totalToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = comincomeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ptWinLossTitle;

            #endregion Agency

            #region ibc

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = winToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = placeToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = wpToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = totalToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = comincomeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = commExpenseTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = pgiAmtoteFeeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ptWinLossTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = netCommTitle;

            #endregion ibc

            #region plumelela

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = winToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = placeToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = wpToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = totalToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = dividendWonTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = commExpenseTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = pgiAmtoteFeeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = amountDueTitle;

            #endregion plumelela

            #endregion second row
        }

        private static void ExportToExcelRacingDailyReportForToteAssignContent(ref ExcelWorksheet wsReport, int startRow, int startCol,
            ref int nextRow, List<RacingDailyWinLossRow> racingDailyWinLossRows, bool isMeetingTotal, bool isPoolTotal, bool isGroupTotal, bool isGrandTotal, bool isBold)
        {
            var bgColor = isGrandTotal
                              ? Color.FromArgb(205, 222, 255)
                              : isMeetingTotal
                                    ? Color.FromArgb(255, 238, 221)
                                    : isPoolTotal
                                          ? Color.FromArgb(211, 225, 200)
                                          : isGroupTotal ? Color.FromArgb(255, 255, 196) : Color.FromArgb(255, 255, 255);

            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isBold, false);
            var normalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor, isBold, false);
            var currentRow = 0;
            foreach (RacingDailyWinLossRow racingDailyWinLossRow in racingDailyWinLossRows)
            {
                var nextCol = 1;

                #region left cols

                if (isMeetingTotal)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + nextCol).Style =
                        normalFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                       startCol + nextCol).Value =
                        string.Format("{0} - {1} {2}", racingDailyWinLossRow.Meeting, racingDailyWinLossRow.Currency, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total"));
                }
                else
                {
                    if (currentRow == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + racingDailyWinLossRows.Count - 1,
                                                       startCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + racingDailyWinLossRows.Count - 1,
                                                           startCol).Style = normalFormat;

                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + racingDailyWinLossRows.Count - 1,
                                                           startCol).Value = isGrandTotal ? "Grand Total" : (isPoolTotal)
                                                                                 ? ResourceExtension.GetResourceValue(
                                                                                     ResourceExtension.Resources.
                                                                                         Language, "Total")
                                                                                         : isGroupTotal ? string.Format("{0} Group Total", racingDailyWinLossRow.Currency) : string.Format("{0}-{1}",
                                                                                                 racingDailyWinLossRow.
                                                                                                     Meeting,
                                                                                                 racingDailyWinLossRow.
                                                                                                     Currency);
                    }

                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = normalFormat;
                    if (isGrandTotal || isGroupTotal || isPoolTotal)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = racingDailyWinLossRow.Currency;
                    }
                    else
                    {
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsInt32(racingDailyWinLossRow.Race);
                    }
                }

                #endregion left cols

                #region Agency

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgWinTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgWPTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.AgPTWL);

                #endregion Agency

                #region IBC

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCWinTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCWPTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCCommIncome);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCAmountFee);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCPT);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.IBCNetIncome);

                #endregion IBC

                #region Plumelela

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuWinTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuPlaceTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuWPTO);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuDividendWon);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuAmountFee);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(racingDailyWinLossRow.PhuAmountDue);

                #endregion Plumelela

                currentRow++;
                nextRow++;
            }
            if (isPoolTotal || isGroupTotal)
                nextRow++;
        }

        #endregion Racing daily report for tote

        #endregion Export 2 excel
    }
}