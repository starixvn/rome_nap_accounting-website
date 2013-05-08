using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.Report.MatchAnalysis;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.Business.Report.MatchAnalysis
{
    public class MatchAnalysisService:IMatchAnalysisService
    {
        private IMatchAnalysisDAO _analysisDao;
        public MatchAnalysisService(IMatchAnalysisDAO iAnalysisDao)
        {
            _analysisDao = iAnalysisDao;
        }

        #region GetKeyForSession
        string GetKeyForSession(ReportType reportType)
        {
            string key = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    key = "MatchAnalysisByCurrency";
                    break;
                case ReportType.Bettype:
                    key = "MatchAnalysisByBettype";
                    break;
                case ReportType.Super:
                    key = "MatchAnalysisBySuper";
                    break;
                case ReportType.Member:
                    key = "MatchAnalysisByMember";
                    break;
                case ReportType.Country:
                    key = "MatchAnalysisByCountry";
                    break;
            }
            return key;
        }
        #endregion

        #region GetNameSheet
        string GetNameSheet(ReportType reportType)
        {
            string key = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    key = "MatchAnalysisByCurrency";
                    break;
                case ReportType.Bettype:
                    key = "MatchAnalysisByBettype";
                    break;
                case ReportType.Super:
                    key = "MatchAnalysisBySuper";
                    break;
                case ReportType.Member:
                    key = "MatchAnalysisByMember";
                    break;
                case ReportType.Country:
                    key = "MatchAnalysisByCountry";
                    break;
            }
            return key;
        }
        #endregion 

        #region GetColumnNameForExcel
        string GetColumnNameForExcel(ReportType reportType, int statusId)
        {
            string name = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    name = "Currency";
                    break;
                case ReportType.Bettype:
                    name = "Bet type";
                    break;
                case ReportType.Super:
                    name = "Super";
                    break;
                case ReportType.Member:
                    switch (statusId)
                    {
                        case 112:
                            name = "Top 100 (WON) Member";
                            break;
                        case 113:
                            name = "Top 100 (LOSE) Member";
                            break;
                    }
                    
                    break;
                case ReportType.Country:
                    name = "MatchAnalysisByCountry";
                    break;
            }
            return name;
        }
        #endregion

        #region SelMatchAnalysisByType
        public MatchAnalysisBO SelMatchAnalysisByType(int matchId, int statusId, Entities.Report.MatchAnalysis.ReportType reportType)
        {
            MatchAnalysisBO match = new MatchAnalysisBO();
            match.Items = new List<MatchAnalysisDTO>();
            match.Total = new MatchAnalysisDTO();

            try
            {
                DataTable dataTable = _analysisDao.SelMatchAnalysisByType(matchId, statusId, reportType);
                if(dataTable != null)
                {
                    DataRow[] rows = dataTable.Select();
                    string itemName = "", currency = "";

                    #region Assign Data
                    foreach (DataRow row in rows)
                    {
                        switch (reportType)
                        {
                            case ReportType.Currency:
                                itemName = CastAide.AsString(row["currencyname"]);
                                break;
                            case ReportType.Bettype:
                                itemName = CacheRef.GetBettype(row["bettype"]);
                                break;
                            case ReportType.Super:
                                itemName = CastAide.AsString(row["username"]);
                                break;
                            case ReportType.Member:
                                itemName = CastAide.AsString(row["username"]);
                                currency = CastAide.AsString(row["currencyname"]);
                                break;
                        }

                        match.Items.Add(new MatchAnalysisDTO()
                                            {
                                                ItemName = itemName,
                                                Currency = currency,
                                                BetCount = NumberFormat.DecFormat( (row["betcount"]),0),
                                                MemberTO = NumberFormat.DecFormat((row["MemberTurnover"]), 2),
                                                MemberWL = NumberFormat.DecFormat((row["MemberWinLost"]), 2),
                                                HouseTO = NumberFormat.DecFormat((row["HouseTurnover"]), 2),
                                                HouseWL = NumberFormat.DecFormat((row["HouseWinloss"]), 2),
                                                SMATO = NumberFormat.DecFormat((row["SMATurnover"]), 2),
                                                SMAWL = NumberFormat.DecFormat((row["SMAWinLost"]), 2),
                                            });
                      
                    }

                    match.Total = new MatchAnalysisDTO()
                                      {
                                          ItemName = "TOTAL",
                                          BetCount = NumberFormat.DecFormat( match.Items.Sum(s=>CastAide.AsDecimal(s.BetCount)),0),
                                          MemberTO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.MemberTO)), 2),
                                          MemberWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.MemberWL)), 2),
                                          HouseTO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.HouseTO)), 2),
                                          HouseWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.HouseWL)), 2),
                                          SMATO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.SMATO)), 2),
                                          SMAWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.SMAWL)), 2),
                                          Currency = string.Empty,
                                      };
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions .ExceptionHandler(ex);
            }

            string key = GetKeyForSession(reportType);
            AppSession.AddSession(HttpContext.Current.Session, key, match);

            return match;
        }
        #endregion

        #region GetStatusById
        string GetStatusById(int statusId)
        {
            string status = string.Empty;
            switch (statusId)
            {
                case 0:
                    status = "All";
                    break;
                case 112:
                    status = "Won";
                    break;
                case 113:
                    status = "Lose";
                    break;
                case 111:
                    status = "Draw";
                    break;
            }
            return status;
        }
        #endregion

        #region ExportMatchAnalysis
        public ExcelFile ExportMatchAnalysis(int matchId, int statusId, ReportType reportType)
        {
            ExcelFile excelFile = new ExcelFile();
            var exlStyle = new ExcelGemboxUtils();
            
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            ExcelWorksheet ws = null;
            string tag = GetNameSheet(reportType);
            ws = worksheets.Add(tag);
            int indexRow = 4;
            try
            {
                string key = GetKeyForSession(reportType);
                MatchAnalysisBO match = AppSession.GetSession(HttpContext.Current.Session, key) as MatchAnalysisBO ??
                                        SelMatchAnalysisByType(matchId, statusId, reportType);

                int colspan = 7;
                if (reportType == ReportType.Member)
                    colspan = 8;
                int r = 142, g = 179, b = 231;

                #region Title
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Value = "Special Match Analysis (Base on RM) ";
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Style = exlStyle.GetStyleTitle(252, 213, 180);
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Merged = true;

                ws.Cells.GetSubrangeAbsolute(2, 0, 2, colspan).Value = "Report Type: " + reportType.ToString() + " - Match ID: " + matchId + " - Status: " + GetStatusById(statusId);
                ws.Cells.GetSubrangeAbsolute(2, 0, 2, colspan).Style = exlStyle.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
                ws.Cells.GetSubrangeAbsolute(2, 0, 2, colspan).Merged = true;
                #endregion

                #region Header

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Value = GetColumnNameForExcel(reportType, statusId);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Value = "Member Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Value = "Member Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Value = "Bet Count";
                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Value = "House Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Value = "House Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Value = "SMA Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Value = "SMA Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Merged = true;

                if (reportType == ReportType.Member)
                {
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Value = "Currency";
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Style = exlStyle.GetStyleTitle(r, g, b);
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Merged = true;
                }

                #endregion

                indexRow++;
                #region Content

                r = g = b = 255;
                foreach (MatchAnalysisDTO item in match.Items)
                {
                    indexRow++;

                    ws.Cells[indexRow, 0].Value = item.ItemName;
                    ws.Cells[indexRow, 0].Style = exlStyle.GetStyleText(r, g, b);

                    ws.Cells[indexRow, 1].Value = CastAide.AsDecimal( item.MemberTO);
                    ws.Cells[indexRow, 1].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(item.MemberWL);
                    ws.Cells[indexRow, 2].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(item.BetCount);
                    ws.Cells[indexRow, 3].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(item.HouseTO);
                    ws.Cells[indexRow, 4].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(item.HouseWL);
                    ws.Cells[indexRow, 5].Style = exlStyle.GetStyleNumber(true, CastAide.AsDecimal(item.HouseWL), "", r, g, b);

                    ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(item.SMATO);
                    ws.Cells[indexRow, 6].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(item.SMAWL);
                    ws.Cells[indexRow, 7].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);


                    if (reportType == ReportType.Member)
                    {
                        ws.Cells[indexRow, 8].Value = item.Currency;
                        ws.Cells[indexRow, 8].Style = exlStyle.GetStyleText(r, g, b);
                    }
                }
                #endregion

                #region Total

                r = 196;
                g = 217;
                b = 238;
                indexRow++;

                ws.Cells[indexRow, 0].Value = "TOTAL";
                ws.Cells[indexRow, 0].Style = exlStyle.GetStyleTextTotal(r, g, b);

                ws.Cells[indexRow, 1].Value = CastAide.AsDecimal(match.Total.MemberTO);
                ws.Cells[indexRow, 1].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(match.Total.MemberWL);
                ws.Cells[indexRow, 2].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(match.Total.BetCount);
                ws.Cells[indexRow, 3].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(match.Total.HouseTO);
                ws.Cells[indexRow, 4].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(match.Total.HouseWL);
                ws.Cells[indexRow, 5].Style = exlStyle.GetStyleNumberTotal(true, CastAide.AsDecimal(match.Total.HouseWL), "", r, g, b);

                ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(match.Total.SMATO);
                ws.Cells[indexRow, 6].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(match.Total.SMAWL);
                ws.Cells[indexRow, 7].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);


                if (reportType == ReportType.Member)
                {
                    ws.Cells[indexRow, 8].Value = "";
                    ws.Cells[indexRow, 8].Style = exlStyle.GetStyleTextTotal(r, g, b);
                }
                #endregion

                for (int i = 0; i <= colspan; i++)
                {
                    ws.Columns[i].Width = 120 * 40;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }
        #endregion
    }
}
