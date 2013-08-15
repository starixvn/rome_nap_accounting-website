// -----------------------------------------------------------------------
// <copyright file="NumberGameWinLossByBettypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.NumberGameWinLoss.NumberGameWinLossByBettype
{
    using System;
    using Fanex.Tarus.Reports.DataAccess.NumberGameWinLoss.NumberGameWinLossByBettype;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss.NumberGameWinLossByBettype;
    using Fanex.Tarus.Infrastructure.Extensions;
    using DNAide.Core;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Utils;
    using System.Drawing;
    using Fanex.Tarus.Infrastructure.Helper;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Common;
    using System.Web;
    using Fanex.Tarus.Infrastructure.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameWinLossByBettypeService: INumberGameWinLossByBettypeService
    {
        private static string _NumberGameWinLossByBettypeSessionKey = "NumberGameWinLossByBettypeSessionKeyV2012";
        private static string _NumberGameWinLossByBettypeDetailSessionKey = "NumberGameWinLossByBettypeDetailSessionKeyV2012";
        INumberGameWinLossByBettypeAccess _numberGameAccess = null;
        public NumberGameWinLossByBettypeService() : this(null)
        { 
        }
        public NumberGameWinLossByBettypeService(INumberGameWinLossByBettypeAccess numberGameAccess)
        {
            _numberGameAccess = numberGameAccess ?? new NumberGameWinLossByBettypeAccess();
        }

        public WinLossReport SelNumberGameWinLosByBettype(string wlDate, string currencies, string companies, int liveStatus)
        {
            var reseult = new WinLossReport();
            try
            {
                AppSession.AddSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey, null);
                var dsWinLoss = _numberGameAccess.SelNumberGameWinLossByBettype(CastAide.AsDateTime(wlDate), currencies, companies, liveStatus);
                if (dsWinLoss != null && dsWinLoss.Tables.Count > 0 && dsWinLoss.Tables[0].Rows.Count > 0)
                {
                    var dtWinLoss = dsWinLoss.Tables[0];
                    reseult.WinLossDetails = new List<WinLossReportDetail>();
                    double totalWL = 0,
                           totalPosition = 0,
                           totalComm = 0,
                           totalTurnOver = 0,
                           totalVirtual = 0,
                           totalReal = 0,
                           totalVariance = 0;
                    foreach (DataRow dr in dtWinLoss.Rows)
                    {
                        var realLineReal = CastAide.AsDouble(dr["WinLost"]) + CastAide.AsDouble(dr["Position"]) +
                                           CastAide.AsDouble(dr["Commission"]);
                        var margin = (Math.Round(CastAide.AsDouble(dr["TurnOver"]), 2) == 0) ? 0 : ((realLineReal / CastAide.AsDouble(dr["TurnOver"])) * 100);
                        var bettypeName = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(dr["bettype"]), "E");

                        var wlDetail = new WinLossReportDetail()
                        {
                            BettypeId = CastAide.AsString(dr["bettype"]),
                            Bettype = bettypeName,
                            WinLoss = NumberFormat.DecFormat(dr["WinLost"], 2),
                            Position = NumberFormat.DecFormat(dr["Position"], 2),
                            Comm = NumberFormat.DecFormat(dr["Commission"], 2),
                            Turnover = NumberFormat.DecFormat(dr["TurnOver"], 2),
                            LineTotalVirtual = NumberFormat.DecFormat(dr["VirtualLineTotal"], 2),
                            LineTotalReal = NumberFormat.DecFormat(realLineReal, 2),
                            Variance = NumberFormat.DecFormat(realLineReal - CastAide.AsDouble(dr["VirtualLineTotal"]), 2),
                            Margin = NumberFormat.DecFormat(margin, 2)

                        };
                        reseult.WinLossDetails.Add(wlDetail);
                        totalWL += CastAide.AsDouble(wlDetail.WinLoss);
                        totalPosition += CastAide.AsDouble(wlDetail.Position);
                        totalComm += CastAide.AsDouble(wlDetail.Comm);
                        totalTurnOver += CastAide.AsDouble(wlDetail.Turnover);
                        totalReal += CastAide.AsDouble(wlDetail.LineTotalReal);
                        totalVirtual += CastAide.AsDouble(wlDetail.LineTotalVirtual);
                        totalVariance += CastAide.AsDouble(wlDetail.Variance);
                    }

                    reseult.GrandTotal = new WinLossReportDetail()
                    {
                        Comm = NumberFormat.DecFormat(totalComm, 2),
                        LineTotalReal = NumberFormat.DecFormat(totalReal, 2),
                        LineTotalVirtual = NumberFormat.DecFormat(totalVirtual, 2),
                        Margin =
                            (Math.Round(totalTurnOver, 2) != 0 ? NumberFormat.DecFormat((totalReal / totalTurnOver) * 100, 2) : "0.00"),
                        Position = NumberFormat.DecFormat(totalPosition, 2),
                        Turnover = NumberFormat.DecFormat(totalTurnOver, 2),
                        Variance = NumberFormat.DecFormat(totalVariance, 2),
                        WinLoss = NumberFormat.DecFormat(totalWL, 2)
                    };

                    AppSession.AddSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey, reseult);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return reseult;
        }
        
        public void ExportExcelNumberGameWinLossByBettype(string wlDate, string currencies, string companies, int liveStatus, string title)
        {
            var startRow = 2;
            var startCol = 0;
            var reportName = "NumberGameWinLossByBettype";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header
            var headerMergeCols = 6;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Value = "Number Game Win Loss by Bet Type";
            #endregion

            #region content

            var getData =
                AppSession.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey) != null
                    ? (WinLossReport)AppSession.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey)
                    : SelNumberGameWinLosByBettype(wlDate, currencies, companies, liveStatus);
            if (getData != null)
            {
                #region assign data

                ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref wsReport, startCol, startRow, ref nextRow, title);
                if (getData != null && getData.WinLossDetails != null && getData.WinLossDetails.Count > 0)
                {
                    ExportToExcelNumberGameWinLossByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                                         getData.WinLossDetails);

                    ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                        getData.GrandTotal, false);
                }
                else
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Value =
                    "No information is available.";
                }
                nextRow++;
                #endregion

            }
            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", string.Format("{0}_{1}", reportName, wlDate)));
            excelFile.SaveXls(response.OutputStream);

            #endregion
        }

        #region private functions - export excel
        private void ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, string title)
        {
            var headerFormart = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Value
                = title;
            nextRow += 1;

            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Bettype";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "WinLoss";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Position";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Comm";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Line Total";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Turnover";

            startCol++;
            wsReport.Cells[startRow + nextRow, startCol].Style = headerFormart;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Margin";
            nextRow++;

        }

        private void ExportToExcelNumberGameWinLossByBettypeAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, List<WinLossReportDetail> rows)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgOtherFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, true);
            var bgPercentFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, true);
            var bgPercentNormalFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, false);

            foreach (WinLossReportDetail row in rows)
            {
                var nextCol = 0;
                wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells[startRow + nextRow, startCol].Value = row.Bettype;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.WinLoss) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalReal) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalReal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Margin) != 0 ? bgPercentFormat : bgPercentNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

                nextRow++;
            }
        }

        private void ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, WinLossReportDetail row, bool isLeagueTotal)
        {
            var color = isLeagueTotal ? Color.FromArgb(221, 246, 207) : Color.FromArgb(211, 225, 200);
            var bgFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);
            var bgOtherFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, true);
            var bgPercentFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00%", color, true, true);
            var bgPercentNormalFormat = new ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Total";

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.WinLoss) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalReal) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalReal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Margin) != 0 ? bgPercentFormat : bgPercentNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

            nextRow++;
        }
        #endregion
        
        public WinLossReportDetailGroupByLeague SelNumberGameWinLosByBettypeDetail(string wlDate, string currencies, string companies, int liveStatus, int bettype)
        {
            var result = new WinLossReportDetailGroupByLeague();
            try
            {
                var rsWinLoss = _numberGameAccess.SelNumberGameWinLossByBettypeDetail(CastAide.AsDateTime(wlDate), currencies, companies, liveStatus, bettype);
                if (rsWinLoss != null && rsWinLoss.Tables.Count > 0 && rsWinLoss.Tables[0].Rows.Count > 0)
                {
                    AppSession.AddSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeDetailSessionKey, null);
                    result.WinLoss = new List<WinLossReport>();
                    #region private variables
                    double bWinlossTotal = 0,
                           bPositionTotal = 0,
                           bCommTotal = 0,
                           bLRealTotal = 0,
                           bLVirtualTotal = 0,
                           bTurnoverTotal = 0,
                           bVarianceTotal = 0;
                    double winlossTotal = 0,
                           positionTotal = 0,
                           commTotal = 0,
                           lRealTotal = 0,
                           lVirtualTotal = 0,
                           turnoverTotal = 0,
                           varianceTotal = 0;
                    double leagueWinlossTotal = 0,
                           leaguePositionTotal = 0,
                           leagueCommTotal = 0,
                           leagueLRealTotal = 0,
                           leagueLVirtualTotal = 0,
                           leagueTurnoverTotal = 0,
                           leagueVarianceTotal = 0;
                    int leagueId = 0, pLeagueId = 0;

                    var leagueWinLoss = new WinLossReport();
                    leagueWinLoss.WinLossDetails = new List<WinLossReportDetail>();
                    leagueWinLoss.GrandTotal = new WinLossReportDetail();
                    #endregion
                    var tbWinLoss = rsWinLoss.Tables[0];
                    foreach (DataRow dr in tbWinLoss.Rows)
                    {
                        var realLineReal = CastAide.AsDouble(dr["WinLost"]) + CastAide.AsDouble(dr["Position"]) +
                                           CastAide.AsDouble(dr["Commission"]);
                        var margin = (Math.Round(CastAide.AsDouble(dr["TurnOver"]), 2) == 0) ? 0 : ((realLineReal / CastAide.AsDouble(dr["TurnOver"])) * 100);
                        leagueId = CastAide.AsInt32(dr["leagueid"]);

                        #region each row
                        bWinlossTotal = CastAide.AsDouble(dr["WinLost"]);
                        bPositionTotal = CastAide.AsDouble(dr["Position"]);
                        bCommTotal = CastAide.AsDouble(dr["Commission"]);
                        bLRealTotal = realLineReal;
                        bLVirtualTotal = CastAide.AsDouble(dr["VirtualLineTotal"]);
                        bTurnoverTotal = CastAide.AsDouble(dr["TurnOver"]);
                        bVarianceTotal = realLineReal - CastAide.AsDouble(dr["VirtualLineTotal"]);
                        #endregion
                        if (leagueId != pLeagueId && pLeagueId > 0)
                        {
                            leagueWinLoss.Name = CacheRef.GetLeagueName(pLeagueId);
                            leagueWinLoss.GrandTotal = new WinLossReportDetail()
                            {
                                Bettype = string.Empty,
                                Comm = NumberFormat.DecFormat(leagueCommTotal, 2),
                                LineTotalReal = NumberFormat.DecFormat(leagueLRealTotal, 2),
                                LineTotalVirtual = NumberFormat.DecFormat(leagueLVirtualTotal, 2),
                                Margin = NumberFormat.DecFormat(Math.Round(leagueTurnoverTotal, 2) != 0 ? ((leagueLRealTotal / leagueTurnoverTotal) * 100) : 0, 2),
                                Position = NumberFormat.DecFormat(leaguePositionTotal, 2),
                                Turnover = NumberFormat.DecFormat(leagueTurnoverTotal, 2),
                                Variance = NumberFormat.DecFormat(leagueVarianceTotal, 2),
                                WinLoss = NumberFormat.DecFormat(leagueWinlossTotal, 2)

                            };
                            result.WinLoss.Add(leagueWinLoss);
                            leagueWinLoss = new WinLossReport();
                            leagueWinLoss.WinLossDetails = new List<WinLossReportDetail>();
                            leagueWinLoss.GrandTotal = new WinLossReportDetail();
                            leagueWinlossTotal = 0;
                            leaguePositionTotal = 0;
                            leagueCommTotal = 0;
                            leagueLRealTotal = 0;
                            leagueLVirtualTotal = 0;
                            leagueTurnoverTotal = 0;
                            leagueVarianceTotal = 0;
                        }

                        #region assign league total
                        leagueWinlossTotal += bWinlossTotal;
                        leaguePositionTotal += bPositionTotal;
                        leagueCommTotal += bCommTotal;
                        leagueLRealTotal += realLineReal;
                        leagueLVirtualTotal += bLVirtualTotal;
                        leagueTurnoverTotal += bTurnoverTotal;
                        leagueVarianceTotal += bVarianceTotal;
                        #endregion

                        #region assign bettype total
                        winlossTotal += bWinlossTotal;
                        positionTotal += bPositionTotal;
                        commTotal += bCommTotal;
                        lRealTotal += realLineReal;
                        lVirtualTotal += bLVirtualTotal;
                        turnoverTotal += bTurnoverTotal;
                        varianceTotal += bVarianceTotal;
                        #endregion

                        #region assign each row
                        var item = new WinLossReportDetail()
                        {
                            BettypeId = CastAide.AsString(dr["matchcode"]),
                            Bettype = CastAide.AsString(dr["matchcode"]),
                            WinLoss = NumberFormat.DecFormat(bWinlossTotal, 2),
                            Position = NumberFormat.DecFormat(bPositionTotal, 2),
                            Comm = NumberFormat.DecFormat(bCommTotal, 2),
                            Turnover = NumberFormat.DecFormat(bTurnoverTotal, 2),
                            LineTotalVirtual = NumberFormat.DecFormat(bLVirtualTotal, 2),
                            LineTotalReal = NumberFormat.DecFormat(bLRealTotal, 2),
                            Variance = NumberFormat.DecFormat(bVarianceTotal, 2),
                            Margin = NumberFormat.DecFormat(margin, 2)
                        };
                        leagueWinLoss.WinLossDetails.Add(item);
                        #endregion

                        pLeagueId = leagueId;
                    }
                    #region assign to last league
                    leagueWinLoss.Name = CacheRef.GetLeagueName(pLeagueId);
                    leagueWinLoss.GrandTotal = new WinLossReportDetail()
                    {
                        Bettype = string.Empty,
                        Comm = NumberFormat.DecFormat(leagueCommTotal, 2),
                        LineTotalReal = NumberFormat.DecFormat(leagueLRealTotal, 2),
                        LineTotalVirtual = NumberFormat.DecFormat(leagueLVirtualTotal, 2),
                        Margin = NumberFormat.DecFormat(Math.Round(leagueTurnoverTotal, 2) != 0 ? (leagueLRealTotal / leagueTurnoverTotal) * 100 : 0, 2),
                        Position = NumberFormat.DecFormat(leaguePositionTotal, 2),
                        Turnover = NumberFormat.DecFormat(leagueTurnoverTotal, 2),
                        Variance = NumberFormat.DecFormat(leagueVarianceTotal, 2),
                        WinLoss = NumberFormat.DecFormat(leagueWinlossTotal, 2)

                    };
                    result.WinLoss.Add(leagueWinLoss);
                    #endregion
                    result.GrandTotal = new WinLossReportDetail()
                    {
                        Bettype = string.Format("{0}{1}", liveStatus == 1 ? "Live! " : "", "Total"),
                        WinLoss = NumberFormat.DecFormat(winlossTotal, 2),
                        Position = NumberFormat.DecFormat(positionTotal, 2),
                        Comm = NumberFormat.DecFormat(commTotal, 2),
                        Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                        LineTotalVirtual = NumberFormat.DecFormat(lVirtualTotal, 2),
                        LineTotalReal = NumberFormat.DecFormat(lRealTotal, 2),
                        Variance = NumberFormat.DecFormat(varianceTotal, 2),
                        Margin = NumberFormat.DecFormat((Math.Round(turnoverTotal, 2) != 0 ? (lRealTotal / turnoverTotal) : 0) * 100, 2)
                    };

                    AppSession.AddSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeDetailSessionKey, result);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
        
        public void ExportExcelNumberGameWinLossByBettypeDetail(string wlDate, int liveStatus, int bettype, string bettypeName, string companies, string currencies, string title)
        {
            var startRow = 2;
            var startCol = 0;
            var reportName = bettypeName.Replace(" ", "").Replace("/", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header
            var headerMergeCols = 6;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Value = "Number Game Event Win/Loss Report";
            #endregion

            #region content

            var getData =
                AppSession.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeDetailSessionKey) != null
                    ? (WinLossReportDetailGroupByLeague)AppSession.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeDetailSessionKey)
                    : SelNumberGameWinLosByBettypeDetail(wlDate, currencies, companies, liveStatus, bettype);
            if (getData != null)
            {
                ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref wsReport, startCol, startRow, ref nextRow, bettypeName);
                foreach (var items in getData.WinLoss)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Value = items.Name;
                    nextRow++;
                    ExportToExcelNumberGameWinLossByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                                     items.WinLossDetails);
                    ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                    items.GrandTotal, true);
                }
                nextRow++;

                ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                     getData.GrandTotal, false);

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion
        }
    }
}
