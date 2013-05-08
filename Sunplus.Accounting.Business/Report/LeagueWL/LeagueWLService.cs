using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities.Report.LeagueWL;
using Sunplus.Accounting.Business.Report.Common;

namespace Sunplus.Accounting.Business.Report.LeagueWL
{
    public class LeagueWLService : ILeagueWLService
    {
        private readonly IReportWinLossDAO _ireportWinLossDAO;
        private static string _SelLeagueWLBySport = "selleaguewlbysportnew";
        private static string _SelLeagueWLByBettype = "selleaguewlbybettypenew";
        private static string _SelLeagueWLByBettypeDetail = "selleaguewlbybettypedetailnew";
        private RMReportUtils utilsBARM =   new RMReportUtils(RMReportUtils.ReportName.ReportLeagueWL, RMReportUtils.RMType.BARM);
        public LeagueWLService()
        {
        }

        public LeagueWLService(IReportWinLossDAO ireportWinLossDAO)
        {
            _ireportWinLossDAO = ireportWinLossDAO;
          
        }

        public LeagueWLBySportsDTO SelLeagueWLBySports(string winlostdate, bool isGetNew)
        {
            if(!isGetNew)
            {
                var leagueWLSession = SessionExtension.GetSession(HttpContext.Current.Session, _SelLeagueWLBySport);
                if (leagueWLSession != null)
                    return (LeagueWLBySportsDTO)leagueWLSession;
            }

            var league = new LeagueWLBySportsDTO
                { Items = new List<LeagueWLBySportsBO>(), GrandTotal = new LeagueWLRow() };
            try
            {
                var rs = _ireportWinLossDAO.SelLeagueWLbySports(winlostdate);
                if (rs == null || rs.ResultSet.Tables.Count == 0)
                    return league;

                var dataTable = rs.ResultSet.Tables[0];
                
                List<int> sports = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["sporttype"])).Distinct().ToList();
                #region assing items
                foreach (int sport in sports)
                {
                    var rows = dataTable.AsEnumerable().Where(s => CastAide.AsInt32(s["sporttype"]) == sport).Select(s => s);

                    var leagueWlBySports = new LeagueWLBySportsBO
                        {
                            Items = new List<LeagueWLRow>(),
                            Total = new LeagueWLRow(),
                            SportId = sport.ToString(),
                            SportName = sport == 100 ? "Mix Parlay" : CacheRef.GetSportName(sport)
                        };

                    leagueWlBySports.Total.LeagueName = string.Format("{0} Total", sport==100 ? "Mix Parlay" : CacheRef.GetSportName(sport));
                    double winlossTotal = 0, positionTotal = 0, commTotal = 0, lineTotal = 0, turnoverTotal = 0, marginTotal = 0;
                    foreach (DataRow row in rows)
                    {
                        leagueWlBySports.Items.Add(new LeagueWLRow()
                                                       {
                                                           LeagueId = CastAide.AsString(row["leagueid"]),
                                                           LeagueName = CacheRef.GetLeagueName(CastAide.AsInt32(row["leagueid"])),
                                                           WinLoss = NumberFormat.DecFormat(CastAide.AsDouble(row["WinLost"]), 2),
                                                           Position = NumberFormat.DecFormat(CastAide.AsDouble(row["Position"]), 2),
                                                           Comm = NumberFormat.DecFormat(CastAide.AsDouble(row["Commission"]), 2),
                                                           LineTotal = NumberFormat.DecFormat(CastAide.AsDouble(row["LineTotal"]), 2),
                                                           Turnover = NumberFormat.DecFormat(CastAide.AsDouble(row["Turnover"]), 2),
                                                           Margin = CastAide.AsDouble(row["Turnover"]) == 0 ? "N/A" : NumberFormat.DecFormat(CastAide.AsDouble(((CastAide.AsDouble(row["LineTotal"]) / CastAide.AsDouble(row["Turnover"])) * 100)), 2)
                                                       });
                        #region total for each sport
                        
                        winlossTotal += CastAide.AsDouble(row["WinLost"]);
                        positionTotal += CastAide.AsDouble(row["Position"]);
                        commTotal += CastAide.AsDouble(row["Commission"]);
                        lineTotal += CastAide.AsDouble(row["LineTotal"]);
                        turnoverTotal += CastAide.AsDouble(row["Turnover"]);
                        marginTotal += (CastAide.AsDouble(row["Turnover"]) == 0
                                            ? 0
                                            : (((CastAide.AsDouble(row["LineTotal"])/CastAide.AsDouble(row["Turnover"]))*
                                                100)));

                        #endregion
                    }
                    leagueWlBySports.Items = leagueWlBySports.Items.OrderBy(e => e.LeagueName).ToList();

                    #region format for total row
                    leagueWlBySports.Total.WinLoss = NumberFormat.DecFormat(winlossTotal, 2);
                    leagueWlBySports.Total.Position = NumberFormat.DecFormat(positionTotal, 2);
                    leagueWlBySports.Total.Comm = NumberFormat.DecFormat(commTotal, 2);
                    leagueWlBySports.Total.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                    leagueWlBySports.Total.Turnover = NumberFormat.DecFormat(turnoverTotal, 2);
                    leagueWlBySports.Total.Margin = NumberFormat.DecFormat(marginTotal, 2);
                    #endregion

                    league.Items.Add(leagueWlBySports);

                    #region grand total
                    league.GrandTotal.LeagueName = "Grand Total";
                    league.GrandTotal.WinLoss =
                        NumberFormat.DecFormat((CastAide.AsDouble(league.GrandTotal.WinLoss) +
                                                CastAide.AsDouble(leagueWlBySports.Total.WinLoss)), 2);
                    league.GrandTotal.Position =
                        NumberFormat.DecFormat((CastAide.AsDouble(league.GrandTotal.Position) +
                                                CastAide.AsDouble(leagueWlBySports.Total.Position)), 2);
                    league.GrandTotal.Comm =
                        NumberFormat.DecFormat((CastAide.AsDouble(league.GrandTotal.Comm) +
                                                CastAide.AsDouble(leagueWlBySports.Total.Comm)), 2);
                    league.GrandTotal.LineTotal =
                        NumberFormat.DecFormat((CastAide.AsDouble(league.GrandTotal.LineTotal) +
                                                CastAide.AsDouble(leagueWlBySports.Total.LineTotal)), 2);
                    league.GrandTotal.Turnover =
                        NumberFormat.DecFormat((CastAide.AsDouble(league.GrandTotal.Turnover) +
                                                CastAide.AsDouble(leagueWlBySports.Total.Turnover)), 2);
                    #endregion

                }
                #endregion

                #region grand total - margin
                league.GrandTotal.Margin =
                    NumberFormat.DecFormat(
                        ((CastAide.AsDouble(league.GrandTotal.LineTotal)/CastAide.AsDouble(league.GrandTotal.Turnover))*
                        100), 2);
                #endregion
                #region 8RM
                league.GrandTotal8RM = NumberFormat.DecFormat(CastAide.AsDouble(rs.Outputs["winlost_8rm"]), 2);
                league.GrandTotalBARM = NumberFormat.DecFormat(CastAide.AsDouble(rs.Outputs["winlost_barm"]), 2);

                #endregion

                #region Line Grand Total

                league.LineTotal =
                    NumberFormat.DecFormat(
                        CastAide.AsDouble(league.GrandTotal.LineTotal) +
                        (CastAide.AsDouble(league.GrandTotal8RM) + (utilsBARM.IsActive ? CastAide.AsDouble(league.GrandTotalBARM) : 0)), 2);

                #endregion

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _SelLeagueWLBySport, league);
            return league;
        }
        
        public LeagueWLBettypeDTO SelLeagueWLByBettypes(int sportId, int leagueId, string winlostDate)
        {
            var result = new LeagueWLBettypeDTO();
            result.Items = new List<LeagueWLBettypeBO>();
            try
            {
                var rs = _ireportWinLossDAO.SelLeagueWLByBettype(sportId, leagueId, winlostDate);
                if (rs == null || rs.ResultSet.Tables.Count == 0)
                    return result;

                var dtDetail = rs.ResultSet.Tables[0];
                var dt8RM = rs.ResultSet.Tables[1];
                var dtBARM = rs.ResultSet.Tables.Count == 3 ? rs.ResultSet.Tables[2] : new DataTable();

                var liveRows = dtDetail.AsEnumerable().Where(s=> CastAide.AsBoolean(s["liveindicator"])).Select(s => s);
                var deadRows = dtDetail.AsEnumerable().Where(s => !CastAide.AsBoolean(s["liveindicator"])).Select(s => s);

                result.SportId = sportId.ToString();
                result.LeagueId = leagueId.ToString();

                double live8RM = 0, dead8RM = 0;
                double liveBARM = 0, deadBARM = 0;
                foreach (DataRow dr in dt8RM.Rows)
                {
                    if (CastAide.AsInt32(dr["liveindicator"]) == 1)
                    {
                        live8RM = CastAide.AsDouble(dr["winlost8rm"]);
                    }
                    if (CastAide.AsInt32(dr["liveindicator"]) == 0)
                    {
                        dead8RM = CastAide.AsDouble(dr["winlost8rm"]);
                    }
                }

                foreach (DataRow dr in dtBARM.Rows)
                {
                    if (CastAide.AsInt32(dr["liveindicator"]) == 1)
                    {
                        liveBARM = CastAide.AsDouble(dr["winlostbarm"]);
                    }
                    if (CastAide.AsInt32(dr["liveindicator"]) == 0)
                    {
                        deadBARM = CastAide.AsDouble(dr["winlostbarm"]);
                    }
                }

                if(liveRows!= null && liveRows.ToList().Count > 0)
                {
                    var liveData = AssignBettypeData(liveRows.ToList(), true);

                    liveData.GrandTotal8RM = NumberFormat.DecFormat(live8RM, 2);
                    liveData.GrandTotalBARM = NumberFormat.DecFormat(utilsBARM.IsActive ? liveBARM : 0, 2);
                    liveData.LineTotal = NumberFormat.DecFormat(CastAide.AsDouble(liveData.GrandTotal.LineTotal) + live8RM + (utilsBARM.IsActive ? liveBARM : 0), 2);
                    result.Items.Add(liveData);
                    
                }

                if (deadRows != null && deadRows.ToList().Count > 0)
                {
                    var deadData = AssignBettypeData(deadRows.ToList(), false);
                    deadData.GrandTotal8RM = NumberFormat.DecFormat(dead8RM, 2);
                    deadData.GrandTotalBARM = NumberFormat.DecFormat(utilsBARM.IsActive ? deadBARM : 0, 2);
                    deadData.LineTotal = NumberFormat.DecFormat(CastAide.AsDouble(deadData.GrandTotal.LineTotal) + dead8RM + (utilsBARM.IsActive ? deadBARM : 0), 2);
                    result.Items.Add(deadData);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _SelLeagueWLByBettype, result);
            return result;
        }

        private LeagueWLBettypeBO AssignBettypeData(List<DataRow> rows, bool isLive)
        {
            var result = new LeagueWLBettypeBO();
            result.IsLive = isLive;
            result.Items = new List<LeagueWLBettypeRow>();
            double winlossTotal = 0, positionTotal = 0, commTotal = 0, lineTotal = 0, turnoverTotal = 0, marginTotal = 0;
            foreach (DataRow liveRow in rows)
            {
                result.Items.Add(new LeagueWLBettypeRow()
                {
                    BettypeId = CastAide.AsString(liveRow["bettype"]),
                    BettypeName = CacheRef.GetBettype(CastAide.AsInt32(liveRow["bettype"])),
                    Comm = NumberFormat.DecFormat(CastAide.AsDouble(liveRow["Commission"]), 2),
                    IsHedge = CastAide.AsString(liveRow["isHedge"]),
                    LineTotal = NumberFormat.DecFormat(CastAide.AsDouble(liveRow["LineTotal"]), 2),
                    Position = NumberFormat.DecFormat(CastAide.AsDouble(liveRow["Position"]), 2),
                    Turnover = NumberFormat.DecFormat(CastAide.AsDouble(liveRow["Turnover"]), 2),
                    WinLoss = NumberFormat.DecFormat(CastAide.AsDouble(liveRow["Winlost"]), 2),
                    Margin = CastAide.AsDouble(liveRow["Turnover"]) == 0 ? "N/A" : NumberFormat.DecFormat((CastAide.AsDouble(liveRow["LineTotal"]) / CastAide.AsDouble(liveRow["Turnover"])) * 100, 2)
                });

                winlossTotal += CastAide.AsDouble(liveRow["Winlost"]);
                positionTotal += CastAide.AsDouble(liveRow["Position"]);
                commTotal += CastAide.AsDouble(liveRow["Commission"]);
                lineTotal += CastAide.AsDouble(liveRow["LineTotal"]);
                turnoverTotal += CastAide.AsDouble(liveRow["Turnover"]);
            }

            marginTotal = CastAide.AsDouble(lineTotal/turnoverTotal)*100;
            result.GrandTotal = new LeagueWLBettypeRow()
                                    {
                                        BettypeName = "Total",
                                        Comm = NumberFormat.DecFormat(commTotal, 2),
                                        LineTotal = NumberFormat.DecFormat(lineTotal, 2),
                                        Margin = NumberFormat.DecFormat(marginTotal, 2),
                                        Position = NumberFormat.DecFormat(positionTotal, 2),
                                        Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                                        WinLoss = NumberFormat.DecFormat(winlossTotal, 2)
                                    };

            return result;
        }
        
        public LeagueWLBettypeDetailBO SelLeagueWLDetailByBettypes(int sportId, int leagueId, int bettypeId, bool isLive, bool isHedge, string winlostDate)
        {
            var result = new LeagueWLBettypeDetailBO();
            result.Items = new List<LeagueWLBettypeDetailRow>();
            try
            {
                var rs = _ireportWinLossDAO.SeleLeagueWLDetailByBettype(winlostDate, sportId, leagueId, bettypeId, isLive, isHedge);
                if (rs == null || rs.ResultSet.Tables.Count == 0)
                    return result;
                var tbReport = rs.ResultSet.Tables[0];
                double winlossTotal = 0, positionTotal = 0, commTotal = 0, lineTotal = 0, turnoverTotal = 0, marginTotal = 0, grandTotal8RM = 0, grandTotalBARM = 0;
                grandTotal8RM = CastAide.AsDouble(rs.Outputs["winlost_8rm"]);
                grandTotalBARM = CastAide.AsDouble(rs.Outputs["winlost_barm"]);
                foreach (DataRow dr in tbReport.Rows)
                {
                    result.Items.Add(new LeagueWLBettypeDetailRow()
                                         {
                                             Code = CastAide.AsString(dr["matchcode"]),
                                             Event =
                                                 string.Format("{0} {1} {2}",
                                                               CacheRef.GetTeamName(CastAide.AsInt32(dr["HomeId"])),
                                                               CastAide.AsInt32(dr["Bettype"]) != 10
                                                                   ? "-vs-"
                                                                   : string.Empty,
                                                               CacheRef.GetTeamName(CastAide.AsInt32(dr["AwayId"]))),
                                             Comm = NumberFormat.DecFormat(CastAide.AsDouble(dr["Commission"]), 2),
                                             LineTotal = NumberFormat.DecFormat(CastAide.AsDouble(dr["LineTotal"]), 2),
                                             Position = NumberFormat.DecFormat(CastAide.AsDouble(dr["Position"]), 2),
                                             Turnover = NumberFormat.DecFormat(CastAide.AsDouble(dr["Turnover"]), 2),
                                             WinLoss = NumberFormat.DecFormat(CastAide.AsDouble(dr["Winlost"]), 2),
                                             Margin =
                                                 CastAide.AsDouble(dr["Turnover"]) == 0
                                                     ? "N/A"
                                                     : NumberFormat.DecFormat(
                                                         (CastAide.AsDouble(dr["LineTotal"])/
                                                          CastAide.AsDouble(dr["Turnover"]))*100, 2)
                                         });

                    winlossTotal += CastAide.AsDouble(dr["Winlost"]);
                    positionTotal += CastAide.AsDouble(dr["Position"]);
                    commTotal += CastAide.AsDouble(dr["Commission"]);
                    lineTotal += CastAide.AsDouble(dr["LineTotal"]);
                    turnoverTotal += CastAide.AsDouble(dr["Turnover"]);
                }
                marginTotal = CastAide.AsDouble(lineTotal / turnoverTotal) * 100;
                result.GrandTotal = new LeagueWLBettypeDetailRow()
                                        {
                                            Event = "Grand Total",
                                            Comm = NumberFormat.DecFormat(commTotal, 2),
                                            LineTotal = NumberFormat.DecFormat(lineTotal, 2),
                                            Margin = NumberFormat.DecFormat(marginTotal, 2),
                                            Position = NumberFormat.DecFormat(positionTotal, 2),
                                            Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                                            WinLoss = NumberFormat.DecFormat(winlossTotal, 2)
                                        };
                result.GrandTotal8RM = NumberFormat.DecFormat(grandTotal8RM , 2);
                result.GrandTotalBARM = NumberFormat.DecFormat(utilsBARM.IsActive ? grandTotalBARM : 0, 2);
                result.LineTotal = NumberFormat.DecFormat((lineTotal + grandTotal8RM + (utilsBARM.IsActive ? grandTotalBARM : 0)), 2);
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _SelLeagueWLByBettypeDetail, result);
            return result;
        }

        #region Export 2 excel - by sport
        public void ExportExcelLeagueWLBySports(string winlostDate, string reportTitle)
        {
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var reportName = string.Format("{0}-{1}", reportTitle, winlostDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportTitle);
            
            #region header

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Style.Font.Weight =
                ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Value =
                string.Format("{0}", reportTitle);
            ExportExcelLeagueWLBySportsAssignHeader(ref wsReport, startRow, startCol);
            #endregion

            #region content
            try
            {
                var getData =
                    (LeagueWLBySportsDTO) SessionExtension.GetSession(HttpContext.Current.Session, _SelLeagueWLBySport);
                if (getData != null)
                {
                    var bgTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(250, 229, 208), true, false);
                    foreach (LeagueWLBySportsBO leagueWl in getData.Items)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                           startCol + 6).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                           startCol + 6).Style = bgTotalFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow,
                                                           startCol + 6).Value = leagueWl.SportName;
                        nextRow++;
                        foreach (var row in leagueWl.Items)
                        {
                            ExportExcelLeagueWLBySportsAssignContent(ref wsReport, startCol, startRow, ref nextRow, row, false, false);
                        }
                        ExportExcelLeagueWLBySportsAssignContent(ref wsReport, startCol, startRow, ref nextRow, leagueWl.Total, true, false);
                        nextRow++;
                    }
                    ExportExcelLeagueWLBySportsAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.GrandTotal, false, true);
                    //8rm
                    var bgFooterFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Value = "8RM Total";

                    wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormat;
                    wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(getData.GrandTotal8RM);

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Value = string.Empty;

                    //BARM
                    if (utilsBARM.IsActive)
                    {
                        nextRow++;

                        var bgFooterFormatBARM = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Style = bgFooterFormatBARM;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Value = utilsBARM.DisplayName + " Total";

                        wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormatBARM;
                        wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(getData.GrandTotalBARM);

                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Style = bgFooterFormatBARM;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Value = string.Empty;
                    }

                    //grand line total
                    nextRow++;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Value = "Grand Total";

                    wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormat;
                    wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(getData.LineTotal);

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Value = string.Empty;
                }
            }
            catch
            {
            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName.Replace(" ", "")));

            excelFile.SaveXls(response.OutputStream);
            #endregion
        }
        private void ExportExcelLeagueWLBySportsAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            var bgHeaderFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "league");
            wsReport.Columns[startCol].Width = 255 * 45;

            wsReport.Cells[startRow - 1, startCol+ (nextCol+=1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol+ nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Winlost");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPosition");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLineTotal");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMargin");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;
        }
        private void ExportExcelLeagueWLBySportsAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, LeagueWLRow row, bool isTotal, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : (isTotal ? Color.FromArgb(250, 229, 208) : Color.FromArgb(255, 255, 255));
            var isBold = isTotal || isFooter ? true : false;
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, false);
            var bgFormatOther = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, true);
            var bgFormatPercent = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, isBold, true);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.LeagueName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatPercent;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin)/100;

            nextRow++;
        }

        #endregion

        #region Export 2 excel - by bettype
        public void ExportExcelLeagueWLByBettypes(string winlostDate, int sportId, int leagueId, string reportTitle)
        {
            const int startCol = 0;
            const int startRow = 1;
            var nextRow = 0;
            var reportName = string.Format("{0}-{1}", reportTitle, winlostDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportTitle);
            #region content

            var getDate =
                (LeagueWLBettypeDTO) SessionExtension.GetSession(HttpContext.Current.Session, _SelLeagueWLByBettype);
            if(getDate!= null)
            {

                foreach (LeagueWLBettypeBO leagueWL in getDate.Items)
                {
                    #region header
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style.Font.Weight =
                        ExcelFont.BoldWeight;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Value =
                        string.Format("{0} - {1}", reportTitle, (leagueWL.IsLive ? "LIVE" : "NONE LIVE") );
                    nextRow++;
                    ExportExcelLeagueWLByBettypesAssignHeader(ref wsReport, startRow, startCol, ref nextRow);
                    nextRow++;
                    #endregion
                    
                    #region detail

                    foreach (LeagueWLBettypeRow row in leagueWL.Items)
                    {
                        ExportExcelLeagueWLByBettypesAssignContent(ref wsReport, startCol, startRow, ref nextRow, row,
                                                                   false, false);
                    }
                    
                    #endregion
                    ExportExcelLeagueWLByBettypesAssignContent(ref wsReport, startCol, startRow, ref nextRow, leagueWL.GrandTotal,
                                                                   false, true);

                    //8rm
                    var bgFooterFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Value = "8RM Total";

                    wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormat;
                    wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(leagueWL.GrandTotal8RM);

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Value = string.Empty;

                    //BARM
                    if (utilsBARM.IsActive)
                    {
                        nextRow++;
                        var bgFooterFormatBARM = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Style = bgFooterFormatBARM;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                            Value = utilsBARM.DisplayName + " Total";

                        wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormatBARM;
                        wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(leagueWL.GrandTotalBARM);

                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Style = bgFooterFormatBARM;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                           startCol + 6).Value = string.Empty;
                    }

                    //grand line total
                    nextRow++;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 3).
                        Value = "Grand Total";

                    wsReport.Cells[startRow + nextRow, startCol + 4].Style = bgFooterFormat;
                    wsReport.Cells[startRow + nextRow, startCol + 4].Value = CastAide.AsDouble(leagueWL.LineTotal);

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Style = bgFooterFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 5, startRow + nextRow,
                                                       startCol + 6).Value = string.Empty;
                    nextRow++;
                }

                
            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName.Replace(" ", "")));

            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        private void ExportExcelLeagueWLByBettypesAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow)
        {
            var bgHeaderFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells[startRow +nextRow, startCol].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Bettype");
            wsReport.Columns[startCol].Width = 255 * 25;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Winlost");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPosition");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLineTotal");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMargin");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;
        }

        private void ExportExcelLeagueWLByBettypesAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, LeagueWLBettypeRow row, bool isTotal, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : (isTotal ? Color.FromArgb(250, 229, 208) : Color.FromArgb(255, 255, 255));
            var isBold = isTotal || isFooter ? true : false;
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, false);
            var bgFormatOther = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, true);
            var bgFormatPercent = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, isBold, true);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.BettypeName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatPercent;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

            nextRow++;
        }
        #endregion

        #region Export 2 excel - by bettype detail
        public void ExportExcelLeagueWLDetailByBettype(int sportId, int leagueId, int bettypeId, bool isLive, bool isHedge, string winlostDate, string reportTitle)
        {
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var reportName = string.Format("{0}-{1}", reportTitle, winlostDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportTitle);

            #region header
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Style.Font.Weight =
                ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Value =
                string.Format("{0}", reportTitle);
            ExportExcelLeagueWLByDetailBettypeAssignHeader(ref wsReport, startRow, startCol);
            #endregion

            #region content

            var getData =
                (LeagueWLBettypeDetailBO)
                SessionExtension.GetSession(HttpContext.Current.Session, _SelLeagueWLByBettypeDetail);
            if(getData!= null)
            {
                foreach (LeagueWLBettypeDetailRow row in getData.Items)
                {
                    ExportExcelLeagueWLDetailByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow, row,
                                                                    false, false);
                }
                ExportExcelLeagueWLDetailByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.GrandTotal,
                                                                    false, true);

                //8rm
                var bgFooterFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Style = bgFooterFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Value = "8RM Total";

                wsReport.Cells[startRow + nextRow, startCol + 5].Style = bgFooterFormat;
                wsReport.Cells[startRow + nextRow, startCol + 5].Value = CastAide.AsDouble(getData.GrandTotal8RM);

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Style = bgFooterFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Value = string.Empty;

                //BARM
                if (utilsBARM.IsActive)
                {
                    nextRow++;
                    var bgFooterFormatBARM = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, true);
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                        Style = bgFooterFormatBARM;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                        Value = utilsBARM.DisplayName + " Total";

                    wsReport.Cells[startRow + nextRow, startCol + 5].Style = bgFooterFormatBARM;
                    wsReport.Cells[startRow + nextRow, startCol + 5].Value = CastAide.AsDouble(getData.GrandTotalBARM);

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                       startCol + 7).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                       startCol + 7).Style = bgFooterFormatBARM;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                       startCol + 7).Value = string.Empty;
                }

                //grand line total
                nextRow++;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Style = bgFooterFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).
                    Value = "Grand Total";

                wsReport.Cells[startRow + nextRow, startCol + 5].Style = bgFooterFormat;
                wsReport.Cells[startRow + nextRow, startCol + 5].Value = CastAide.AsDouble(getData.LineTotal);

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Style = bgFooterFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 6, startRow + nextRow,
                                                   startCol + 7).Value = string.Empty;
            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName.Replace(" ", "")));

            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        private void ExportExcelLeagueWLByDetailBettypeAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            var bgHeaderFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells[startRow -1, startCol].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCode");
            wsReport.Columns[startCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "events");
            wsReport.Columns[startCol + nextCol].Width = 255 * 30;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Winlost");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPosition");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLineTotal");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMargin");
            wsReport.Columns[startCol + nextCol].Width = 255 * 15;
        }

        private void ExportExcelLeagueWLDetailByBettypeAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, LeagueWLBettypeDetailRow row, bool isTotal, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : (isTotal ? Color.FromArgb(250, 229, 208) : Color.FromArgb(255, 255, 255));
            var isBold = isTotal || isFooter ? true : false;
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, false);
            var bgFormatOther = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isBold, true);
            var bgFormatPercent = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, isBold, true);

            var nextCol = 0;
            if(isFooter)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style
                    = bgFormat;

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value
                    = row.Event;
                nextCol += 1;
            }
            else
            {
                wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells[startRow + nextRow, startCol].Value = row.Code;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = row.Event;    
            }
            

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatOther;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormatPercent;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

            nextRow++;
        }
        #endregion
    }
}
