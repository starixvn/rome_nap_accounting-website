using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.AdoNet.ReportDAO;
using Sunplus.Accounting.DataAccess.IBCDLL.AgentReport;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.Report.DailyWL;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Report.Mixparlay;

namespace Sunplus.Accounting.Business.Report.DailyWL
{
    public class ReportDailyWL : IReportDailyWL
    {
        private static string _dailyWlDirectRacingSessionKey = "dailywldirectracingsessionkey";
        private static string _dailyWlDirectRacingBySportSessionKey = "dailywldirectracingbysportsessionkey{0}";
        private static string _dailyWlDirectRacingBySportDetails = "dailywlDirectracingbysportdetails{0}";
        private static string _dailyWlDirectMixParlayAdvance = "dailywldirectmixparlayadvance";
        private static IReportRacingDAO _reportRacingDao;

        public ReportDailyWL(IReportRacingDAO reportRacingDao)
        {
            _reportRacingDao = reportRacingDao;
        }


        #region DailyWLDirectRacing

        public DailyWLBO DailyWLDirectRacing(string winlossdate)
        {
            DailyWLBO dailyWlbo = new DailyWLBO();
            dailyWlbo.Items = new List<DailyWLRowBO>();
            dailyWlbo.Totals = new List<DailyWLRowBO>();

            DataTable dataTable = _reportRacingDao.DailyWLDirectRacing(winlossdate);
            if (dataTable == null)
                return dailyWlbo;

            DataRow[] rows = dataTable.Select();
            foreach (DataRow row in rows)
            {
                dailyWlbo.Items.Add(new DailyWLRowBO()
                                        {
                                            Id = CastAide.AsString(row["sporttype"]),
                                            Name = CacheRef.GetSportName(row["sporttype"]),
                                            WinLoss = NumberFormat.DecFormat(row["winlost"], 2),
                                            Position = NumberFormat.DecFormat(row["position"], 2),
                                            Comm = NumberFormat.DecFormat(row["commission"], 2),
                                            LineTotal = NumberFormat.DecFormat(row["LineTotal"], 2),
                                            Turnover = NumberFormat.DecFormat(row["Turnover"], 2),

                                            Margin = CastAide.AsDecimal(row["Turnover"]) == 0
                                                         ? "N/A"
                                                         : NumberFormat.DecFormat(
                                                             CastAide.AsDecimal(row["LineTotal"])/
                                                             CastAide.AsDecimal(row["Turnover"])*100, 2) + "%",
                                        });
            }

            #region Total

            DailyWLRowBO total = new DailyWLRowBO();
            total.Name = "All Total";
            total.WinLoss = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2);
            total.Position = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Position)), 2);
            total.Comm = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2);
            total.LineTotal = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.LineTotal)), 2);
            total.Turnover = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2);
            total.Margin = CastAide.AsDecimal(total.Turnover) == 0
                               ? "N/A"
                               : NumberFormat.DecFormat(
                                   CastAide.AsDecimal(total.LineTotal)/CastAide.AsDecimal(total.Turnover)*100, 2) + "%";

            dailyWlbo.Totals.Add(total);

            #endregion

            SessionExtension.AddSession(HttpContext.Current.Session, _dailyWlDirectRacingSessionKey, dailyWlbo);
            return dailyWlbo;
        }

        #endregion


        #region DailyWLDirectRacingBySport

        public DailyWLBO DailyWLDirectRacingBySport(string sporttype, string winlossdate)
        {
            DailyWLBO dailyWlbo = new DailyWLBO();
            dailyWlbo.Items = new List<DailyWLRowBO>();
            dailyWlbo.Totals = new List<DailyWLRowBO>();

            DataTable dataTable = _reportRacingDao.DailyWLDirectRacingBySport(sporttype, winlossdate);
            if (dataTable == null)
                return dailyWlbo;

            DataRow[] rows = dataTable.Select();
            foreach (DataRow row in rows)
            {
                dailyWlbo.Items.Add(new DailyWLRowBO()
                                        {
                                            Id = CastAide.AsString(row["bettype"]),
                                            Name = CacheRef.GetBettype(row["bettype"]),
                                            WinLoss = NumberFormat.DecFormat(row["winlost"], 2),
                                            Position = NumberFormat.DecFormat(row["position"], 2),
                                            Comm = NumberFormat.DecFormat(row["commission"], 2),
                                            LineTotal = NumberFormat.DecFormat(row["LineTotal"], 2),
                                            Turnover = NumberFormat.DecFormat(row["Turnover"], 2),

                                            Margin = CastAide.AsDecimal(row["Turnover"]) == 0
                                                         ? "N/A"
                                                         : NumberFormat.DecFormat(
                                                             CastAide.AsDecimal(row["LineTotal"])/
                                                             CastAide.AsDecimal(row["Turnover"])*100, 2) + "%",
                                        });
            }

            #region Total

            DailyWLRowBO total = new DailyWLRowBO();
            total.Name = "All Total";
            total.WinLoss = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2);
            total.Position = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Position)), 2);
            total.Comm = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2);
            total.LineTotal = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.LineTotal)), 2);
            total.Turnover = NumberFormat.DecFormat(dailyWlbo.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2);
            total.Margin = CastAide.AsDecimal(total.Turnover) == 0
                               ? "N/A"
                               : NumberFormat.DecFormat(
                                   CastAide.AsDecimal(total.LineTotal)/CastAide.AsDecimal(total.Turnover)*100, 2) + "%";

            dailyWlbo.Totals.Add(total);

            #endregion

            SessionExtension.AddSession(HttpContext.Current.Session,
                                        string.Format(_dailyWlDirectRacingBySportSessionKey, sporttype), dailyWlbo);
            return dailyWlbo;
        }

        #endregion

        #region DailyWLDirectRacingBySportDetails

        public DailyWLDTO DailyWLDirectRacingBySportDetails(string winlossdate, string sporttype, string bettype)
        {
            DailyWLDTO dailyWlbo = new DailyWLDTO();
            dailyWlbo.Items = new List<DailyWLDetails>();
            dailyWlbo.Totals = new List<DailyWLRowBO>();

            DataTable dataTable = _reportRacingDao.DailyWLDirectRacingBySportDetails(winlossdate, sporttype, bettype);
            if (dataTable == null)
                return dailyWlbo;

            var leagueIds = dataTable.AsEnumerable().Select(s => s["leagueid"]).Distinct().ToList();
            var AllLeague = new List<DailyWLRowBO>();

            #region League

            foreach (object leagueId in leagueIds)
            {
                DailyWLDetails League = new DailyWLDetails();
                League.Items = new List<DailyWLRowBO>();
                League.Totals = new List<DailyWLRowBO>();

                DataRow[] rows = dataTable.Select("leagueid = " + leagueId);
                foreach (DataRow row in rows)
                {
                    #region row

                    DailyWLRowBO rowBo = new DailyWLRowBO();

                    rowBo.Name = CastAide.AsString(row["race"]);
                    rowBo.WinLoss = NumberFormat.DecFormat(row["winlost"], 2);
                    rowBo.Position = NumberFormat.DecFormat(row["position"], 2);
                    rowBo.Comm = NumberFormat.DecFormat(row["commission"], 2);
                    rowBo.LineTotal = NumberFormat.DecFormat(row["LineTotal"], 2);
                    rowBo.Turnover = NumberFormat.DecFormat(row["Turnover"], 2);
                    rowBo.Margin = CastAide.AsDecimal(row["Turnover"]) == 0
                                       ? "N/A"
                                       : NumberFormat.DecFormat(
                                           CastAide.AsDecimal(row["LineTotal"])/CastAide.AsDecimal(row["Turnover"])*100,
                                           2) + "%";

                    League.Items.Add(rowBo);
                    AllLeague.Add(rowBo);

                    #endregion
                }

                #region total

                League.Totals.Add(new DailyWLRowBO()
                                      {
                                          Name = CacheRef.GetLeagueName(leagueId),

                                          WinLoss =
                                              NumberFormat.DecFormat(
                                                  League.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                          Position =
                                              NumberFormat.DecFormat(
                                                  League.Items.Sum(s => CastAide.AsDecimal(s.Position)), 2),
                                          Comm =
                                              NumberFormat.DecFormat(League.Items.Sum(s => CastAide.AsDecimal(s.Comm)),
                                                                     2),
                                          LineTotal =
                                              NumberFormat.DecFormat(
                                                  League.Items.Sum(s => CastAide.AsDecimal(s.LineTotal)), 2),
                                          Turnover =
                                              NumberFormat.DecFormat(
                                                  League.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                                          Margin = League.Items.Sum(s => CastAide.AsDecimal(s.Turnover)) == 0
                                                       ? "N/A"
                                                       : NumberFormat.DecFormat(
                                                           League.Items.Sum(s => CastAide.AsDecimal(s.LineTotal))/
                                                           League.Items.Sum(s => CastAide.AsDecimal(s.Turnover))*100, 2) +
                                                         "%"
                                      });

                dailyWlbo.Items.Add(League);

                #endregion

            }

            #endregion

            #region grand Total

            dailyWlbo.Totals.Add(new DailyWLRowBO()
                                     {
                                         Name = "Grand Total",

                                         WinLoss =
                                             NumberFormat.DecFormat(AllLeague.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                         Position =
                                             NumberFormat.DecFormat(AllLeague.Sum(s => CastAide.AsDecimal(s.Position)),
                                                                    2),
                                         Comm =
                                             NumberFormat.DecFormat(AllLeague.Sum(s => CastAide.AsDecimal(s.Comm)), 2),
                                         LineTotal =
                                             NumberFormat.DecFormat(
                                                 AllLeague.Sum(s => CastAide.AsDecimal(s.LineTotal)), 2),
                                         Turnover =
                                             NumberFormat.DecFormat(AllLeague.Sum(s => CastAide.AsDecimal(s.Turnover)),
                                                                    2),

                                         Margin = AllLeague.Sum(s => CastAide.AsDecimal(s.Turnover)) == 0
                                                      ? "N/A"
                                                      : NumberFormat.DecFormat(
                                                          AllLeague.Sum(s => CastAide.AsDecimal(s.LineTotal))/
                                                          AllLeague.Sum(s => CastAide.AsDecimal(s.Turnover))*100, 2) +
                                                        "%"
                                     });

            #endregion

            SessionExtension.AddSession(HttpContext.Current.Session,
                                        string.Format(_dailyWlDirectRacingBySportDetails, bettype), dailyWlbo);
            return dailyWlbo;
        }

        #endregion

        #region export 2 excel

        public void Export2ExcelDailyWLDirectRacing(string winlossDate)
        {
            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var reportName = string.Format("DailyWinLossRacing-{0}", winlossDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add("DailyWinLossRacing");
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _dailyWlDirectRacingSessionKey);
            var wlData = getSession != null
                             ? (DailyWLBO) getSession
                             : DailyWLDirectRacing(winlossDate);

            #endregion

            #region Header

            var reportHeaderTitle = "Daily WinLoss Racing";
            RacingReportHeader(ref wsReport, startCol, startRow, true, false, false, reportHeaderTitle);

            #endregion

            #region Body

            if (wlData != null && wlData.Items.Count > 0)
            {
                RacingReporBody(ref wsReport, wlData.Items, startCol, startRow, ref nextRow, false, false, false);
                RacingReporBody(ref wsReport, wlData.Totals, startCol, startRow, ref nextRow, true, false, false);
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

        public void Export2ExcelDailyWLDirectRacingBySport(string sporttype, string winlossDate)
        {
            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var sportName = GetRacebookName(CastAide.AsInt32(sporttype));
            var reportName = string.Format("DailyWinLossRacing-{0}-{1}", sportName.Replace(" ", ""), winlossDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("DailyWinLossRacing-{0}", sportName.Replace(" ", "")));
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session,
                                                         string.Format(_dailyWlDirectRacingBySportSessionKey, sporttype));
            var wlData = getSession != null
                             ? (DailyWLBO) getSession
                             : DailyWLDirectRacingBySport(sporttype, winlossDate);

            #endregion

            #region Header

            var reportHeaderTitle = string.Format("Daily WinLoss Racing - {0}", sportName);
            RacingReportHeader(ref wsReport, startCol, startRow, false, true, false, reportHeaderTitle);

            #endregion

            #region Body

            if (wlData != null && wlData.Items.Count > 0)
            {
                RacingReporBody(ref wsReport, wlData.Items, startCol, startRow, ref nextRow, false, false, false);
                RacingReporBody(ref wsReport, wlData.Totals, startCol, startRow, ref nextRow, true, false, false);
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

        public void Export2ExcelDailyWLDirectRacingBySportDetails(string sporttype, string bettype, string winlossDate)
        {
            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var sportName = GetRacebookName(CastAide.AsInt32(sporttype));
            var bettypeName = HRPositionTakingManager.GetBettypeName(CastAide.AsInt32(bettype));
            var reportName = string.Format("DailyWinLossRacing-{0}-{1}-{2}", sportName.Replace(" ", ""),
                                           bettypeName.Replace(" ", ""), winlossDate);
            var excelFile = new ExcelFile();
            var wsReport =
                excelFile.Worksheets.Add(string.Format("DailyWinLossRacing-{0}-{1}", sportName.Replace(" ", ""),
                                                       bettypeName.Replace(" ", "")));
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session,
                                                         string.Format(_dailyWlDirectRacingBySportDetails, bettype));
            var wlData = getSession != null
                             ? (DailyWLDTO) getSession
                             : DailyWLDirectRacingBySportDetails(winlossDate, sportName, bettype);

            #endregion

            #region Header

            var reportHeaderTitle = string.Format("Daily WinLoss Racing - {0} - {1}", sportName, bettypeName);
            RacingReportHeader(ref wsReport, startCol, startRow, false, false, true, reportHeaderTitle);

            #endregion

            #region Body

            if (wlData != null && wlData.Items.Count > 0)
            {
                var bgTitleForEachGroup = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(255, 255, 255));
                foreach (var report in wlData.Items)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Style = bgTitleForEachGroup;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).
                        Value = report.Totals.ToList()[0].Name;

                    nextRow++;
                    RacingReporBody(ref wsReport, report.Items, startCol, startRow, ref nextRow, false, false, true);
                    RacingReporBody(ref wsReport, report.Totals, startCol, startRow, ref nextRow, false, true, false);
                }
                RacingReporBody(ref wsReport, wlData.Totals, startCol, startRow, ref nextRow, true, false, true);
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

        private static void RacingReportHeader(ref ExcelWorksheet wsReport, int startCol, int startRow,
                                               bool isDirect, bool isRacingBySport, bool isRacingDetail,
                                               string reportHeaderTitle)
        {
            //var typeTilte = isDirect ? "Sporttype" : (isRacingBySport ? "Bettype" : "Race");
            const string typeTilte = "Bettype";

            const string winLossTitle = "WinLoss";
            const string positionTitle = "Position";
            const string commTitle = "Comm";
            const string lineTotalTitle = "Line Total";
            const string turnoverTitle = "Turnover";
            const string marginTitle = "Margin";
            var nextCol = 1;
            var bgHeader = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Value =
                reportHeaderTitle;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 6).Style.Font.Weight =
                ExcelFont.BoldWeight;

            wsReport.Cells[startRow - 1, startCol].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol].Value = typeTilte;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = winLossTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = positionTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = commTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = lineTotalTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = turnoverTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeader;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = marginTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeader;
        }

        private static void RacingReporBody(ref ExcelWorksheet wsReport, List<DailyWLRowBO> reportData, int startCol,
                                            int startRow,
                                            ref int nextRow, bool isFooter, bool isTotal, bool isDetail)
        {
            var bgColor = isFooter
                              ? Color.FromArgb(205, 222, 255)
                              : isTotal ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor,
                                                                                isDetail && isFooter ? true : false,
                                                                                true);
            var bgFormatPercent = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", bgColor,
                                                                                       isDetail && isFooter
                                                                                           ? true
                                                                                           : false, true);
            var bgLineTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00",
                                                                                         Color.FromArgb(130, 199, 255),
                                                                                         true, true);
            var bgTurnoverFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, true, true);
            var rowIndex = 1;

            foreach (var dailyWlRowRow in reportData)
            {
                var nextCol = 1;
                wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol].Value = isTotal
                                                                         ? "Total"
                                                                         : dailyWlRowRow.Name;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(dailyWlRowRow.WinLoss);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(dailyWlRowRow.Position);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(dailyWlRowRow.Comm);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = bgFormat;

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value =
                    CastAide.AsDecimal(dailyWlRowRow.LineTotal);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = isFooter || isTotal
                                                                                       ? bgLineTotalFormat
                                                                                       : bgFormat;

                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDecimal(dailyWlRowRow.Turnover);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = isFooter || isTotal
                                                                                       ? bgTurnoverFormat
                                                                                       : bgFormat;

                if (CastAide.AsDouble(dailyWlRowRow.Turnover) != 0)
                {
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value =
                        CastAide.AsDouble(dailyWlRowRow.Margin.Replace("%", ""))/100;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormatPercent;
                }
                else
                {
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = dailyWlRowRow.Margin;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Right;
                }


                nextRow++;
                rowIndex++;
            }
        }


        private static string GetRacebookName(int sportId)
        {
            var sportName = string.Empty;
            switch (sportId)
            {
                case Constants.Sport.HorseRacing:
                    sportName = "Horse Racing";
                    break;
                case Constants.Sport.Greyhound:
                    sportName = ResourceExtension.GetResourceValue(
                        ResourceExtension.Resources.Language, "Greyhound");
                    break;
                case Constants.Sport.Harness:
                    sportName = ResourceExtension.GetResourceValue(
                        ResourceExtension.Resources.Language, "Harness");
                    break;
                default:
                    break;
            }

            return sportName;
        }

        #endregion


        public DNAide.Data.DbResult DailyWLDirect_Sporttype(string winlossdate)
        {
            return _reportRacingDao.DailyWLDirect_Sporttype(winlossdate);
        }

        #region IReportDailyWL Members


        public DNAide.Data.DbResult DailyWLDirect_MixParlay(string winlossdate, int bettype)
        {
            return _reportRacingDao.DailyWLDirect_MixParlay(winlossdate, bettype);
        }

        #endregion


        public MixParlayView DailyWLDirect_MixParlayAdvance(string winlossdate, int bettype)
        {
            var mixparlayView = new MixParlayView();
            mixparlayView.ListLiveType = new List<LiveTypeDetail>();
            var mainReportDAO = new ReportMainDAO();
            try
            {
                DbResult rs = mainReportDAO.SelectMixparlayDetail(winlossdate, bettype);
                DataTable tb = null;
                if (rs != null && rs.ResultSet.Tables.Count > 0)
                    tb = rs.ResultSet.Tables[0];
                if (tb != null)
                {
                    DataRow[] liveTypeRows = tb.DefaultView.ToTable(true, "LiveType").Select();
                    //create object MixParlayView 
                    double GrandtotalComm = 0;
                    double GrandtotalWinloss = 0;
                    double GrandtotalPosition = 0;
                    double GrandtotalLineTotal = 0;
                    double GrandtotalTurnover = 0;
                    foreach (DataRow row in liveTypeRows)
                    {
                        var liveType = new LiveTypeDetail
                        {
                            LiveName =
                                Sunplus.Accounting.Core.Common.AccCommon.GetLiveName(
                                    CastAide.AsInt32(row["Livetype"])),
                            ListSportType = new List<SportTypeDetail>(),
                            Total = new WinLossDetail()
                        };

                        var sports =
                            tb.AsEnumerable().Where(
                                s => CastAide.AsInt32(s["LiveType"]) == CastAide.AsInt32(row["LiveType"])).Select(s => s);
                        DataTable tbsport = sports.CopyToDataTable();
                        DataRow[] sportTypeRows = tbsport.DefaultView.ToTable(true, "SportType").Select();

                        double totalComm = 0;
                        double totalWinloss = 0;
                        double totalPosition = 0;
                        double totalLineTotal = 0;
                        double totalTurnover = 0;
                        int TotalSport = 0;
                        foreach (DataRow rowsport in sportTypeRows)
                        {
                            var sportType = new SportTypeDetail();
                            sportType.SportName =
                                Sunplus.Accounting.Core.Common.AccCommon.GetSportName(CastAide.AsInt32(rowsport["SportType"]));
                            sportType.ListWlDetail = new List<WinLossDetail>();
                            string SubTotal = "";
                            var WLDetails = tbsport.AsEnumerable().Where(
                                s => CastAide.AsInt32(s["SportType"]) == CastAide.AsInt32(rowsport["SportType"])).Select(
                                    s => s);
                            double SubtotalComm = 0;
                            double SubtotalWinloss = 0;
                            double SubtotalPosition = 0;
                            double SubtotalLineTotal = 0;
                            double SubtotalTurnover = 0;
                            foreach (DataRow wlDetail in WLDetails)
                            {
                                string Margin = "N/A";
                                if (CastAide.AsDouble(wlDetail["Turnover"]) != 0)
                                    Margin = (CastAide.AsDouble(wlDetail["LineTotal"]) /
                                              CastAide.AsDouble(wlDetail["Turnover"].ToString())).ToString();

                                sportType.ListWlDetail.Add(new WinLossDetail()
                                {
                                    Status =
                                        "Mix Parlay(" + wlDetail["Status"].ToString() + ")",
                                    WinLoss =
                                        NumberFormat.DecFormat(
                                            wlDetail["winlost"].ToString(), 2),
                                    Position =
                                        NumberFormat.DecFormat(
                                            wlDetail["Position"].ToString(), 2),
                                    Comm =
                                        NumberFormat.DecFormat(
                                            wlDetail["Commission"].ToString(), 2),
                                    LineTotal =
                                        NumberFormat.DecFormat(
                                            wlDetail["LineTotal"].ToString(), 2),
                                    Turnover =
                                        NumberFormat.DecFormat(
                                            wlDetail["Turnover"].ToString(), 2),
                                    Margin = CastAide.AsDecimal(wlDetail["Turnover"]) == 0
                                                 ? "N/A"
                                                 : NumberFormat.DecFormat(
                                                     CastAide.AsDecimal(wlDetail["LineTotal"]) /
                                                     CastAide.AsDecimal(wlDetail["Turnover"]) *
                                                     100, 2)
                                });

                                SubtotalComm += CastAide.AsDouble(wlDetail["Commission"]);
                                SubtotalWinloss += CastAide.AsDouble(wlDetail["winlost"]);
                                SubtotalPosition += CastAide.AsDouble(wlDetail["Position"]);
                                SubtotalTurnover += CastAide.AsDouble(wlDetail["Turnover"]);
                                SubtotalLineTotal += CastAide.AsDouble(wlDetail["LineTotal"]);



                            }
                            sportType.TotalItems = WLDetails.Count() + 1;
                            sportType.SubTotal = new WinLossDetail
                            {
                                Comm = NumberFormat.DecFormat(SubtotalComm.ToString(), 2),
                                Position = NumberFormat.DecFormat(SubtotalPosition.ToString(), 2),
                                WinLoss = NumberFormat.DecFormat(SubtotalWinloss.ToString(), 2),
                                LineTotal = NumberFormat.DecFormat(SubtotalLineTotal.ToString(), 2),
                                Turnover = NumberFormat.DecFormat(SubtotalTurnover.ToString(), 2),
                                Margin = SubtotalTurnover == 0
                                             ? "N/A"
                                             : NumberFormat.DecFormat(
                                                 SubtotalLineTotal / SubtotalTurnover * 100, 2)
                            };
                            liveType.ListSportType.Add(sportType);
                            TotalSport += (sportType.TotalItems);
                            totalComm += SubtotalComm;
                            totalPosition += SubtotalPosition;
                            totalWinloss += SubtotalWinloss;
                            totalLineTotal += SubtotalLineTotal;
                            totalTurnover += SubtotalTurnover;

                        }
                        liveType.TotalItems = TotalSport;
                        liveType.Total = new WinLossDetail
                        {
                            Comm = NumberFormat.DecFormat(totalComm.ToString(), 2),
                            Position = NumberFormat.DecFormat(totalPosition.ToString(), 2),
                            WinLoss = NumberFormat.DecFormat(totalWinloss.ToString(), 2),
                            LineTotal = NumberFormat.DecFormat(totalLineTotal.ToString(), 2),
                            Turnover = NumberFormat.DecFormat(totalTurnover.ToString(), 2),
                            Margin = totalTurnover == 0
                                         ? "N/A"
                                         : NumberFormat.DecFormat(totalLineTotal / totalTurnover * 100, 2)

                        };
                        GrandtotalComm += totalComm;
                        GrandtotalTurnover += totalTurnover;
                        GrandtotalWinloss += totalWinloss;
                        GrandtotalPosition += totalPosition;
                        GrandtotalLineTotal += totalLineTotal;
                        mixparlayView.ListLiveType.Add(liveType);

                    }
                    mixparlayView.GrandTotal = new WinLossDetail
                    {
                        Comm = NumberFormat.DecFormat(GrandtotalComm.ToString(), 2),
                        Position = NumberFormat.DecFormat(GrandtotalPosition.ToString(), 2),
                        WinLoss = NumberFormat.DecFormat(GrandtotalWinloss.ToString(), 2),
                        LineTotal = NumberFormat.DecFormat(GrandtotalLineTotal.ToString(), 2),
                        Turnover = NumberFormat.DecFormat(GrandtotalTurnover.ToString(), 2),
                        Margin = GrandtotalTurnover == 0
                                     ? "N/A"
                                     : NumberFormat.DecFormat(
                                         GrandtotalLineTotal / GrandtotalTurnover * 100, 2)
                    };
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _dailyWlDirectMixParlayAdvance, mixparlayView);
            }
            catch (Exception ex)
            {
                
               LogExtensions.ExceptionHandler(ex);
            }
            
            return mixparlayView;
        }


        public void Export2ExcelDailyWLDirectMixParlayAdvance(string winlossdate, int bettype, string reportTitle)
        {
            var getReportData =
                SessionExtension.GetSession(HttpContext.Current.Session, _dailyWlDirectMixParlayAdvance) != null
                    ? (MixParlayView)
                      SessionExtension.GetSession(HttpContext.Current.Session, _dailyWlDirectMixParlayAdvance)
                    : DailyWLDirect_MixParlayAdvance(winlossdate, bettype);

            #region excel format

            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var reportName = reportTitle.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);

            #endregion

            #region header

            wsReport.Columns[0].Width = 20 * 256;
            wsReport.Columns[1].Width = 15*256;
            wsReport.Columns[2].Width = 15*256;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle;
            Export2ExcelDailyWLDirectMixParlayAdvanceHeader(ref wsReport, startRow, startCol);
            #endregion

            #region content
            if(getReportData!=null && getReportData.ListLiveType.Count > 0)
            {
                /*foreach (LiveTypeDetail liveTypeDetail in getReportData.ListLiveType)
                {
                    Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                     liveTypeDetail);
                }*/
                if(getReportData.ListLiveType.Count > 2)
                {
                    Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                   getReportData.ListLiveType[1], 221, 217, 196);
                    Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                        getReportData.ListLiveType[0], 202, 240, 145);
                    Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                        getReportData.ListLiveType[2], 197, 217, 241);
                }
                else if (getReportData.ListLiveType.Count > 1)
                {
                    if(getReportData.ListLiveType[0].LiveName == "Live" && getReportData.ListLiveType[1].LiveName == "Non Live")
                    {
                        Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                 getReportData.ListLiveType[1], 221, 217, 196);
                        Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                            getReportData.ListLiveType[0], 202, 240, 145);
                       
                    }
                    else
                    {
                        Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                getReportData.ListLiveType[0], 221, 217, 196);
                        Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                            getReportData.ListLiveType[1], 202, 240, 145);
                       
                    }
                    
                }
                else if (getReportData.ListLiveType.Count > 0)
                {
                    Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                getReportData.ListLiveType[0], 221, 217, 196);
                }
               
                #region footer

                Export2ExcelDailyWLDirectMixParlayAdvanceFooter(ref wsReport, startRow, startCol, nextRow, getReportData.GrandTotal);

                #endregion
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

        private void Export2ExcelDailyWLDirectMixParlayAdvanceHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = string.Empty;
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Sporttype";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Bettype";
            wsReport.Cells[startRow - 1, startCol + (nextCol+=1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "WinLoss";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Position";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Comm";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Line Total";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Margin";

        }

        private void Export2ExcelDailyWLDirectMixParlayAdvanceContent(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, LiveTypeDetail liveTypeDetail, int r, int g, int b)
        {
            var numberDefaultFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var numberDefaultSubFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(222, 210, 190), true, false, false);
            var numberDefaultTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumberMixparlayAdvance(".00", Color.FromArgb(33, 89, 103), true, false, true);

            var numberFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, true);
            var numberSubFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(222, 210, 190), true, true);
            var numberTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumberTotal(".00", Color.FromArgb(33, 89, 103), true, true);

            var numberSportFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), true, true);
            var numberLiveFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(r, g, b), true, true);

            var percentFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", Color.FromArgb(253, 252, 252), false, true);
            var percentSubFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", Color.FromArgb(222, 210, 190), true, true);
            var percentTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumberTotal(".00%", Color.FromArgb(33, 89, 103), true, true);

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + liveTypeDetail.TotalItems -1,
                                               startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + liveTypeDetail.TotalItems - 1,
                                               startCol).Style = numberLiveFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + liveTypeDetail.TotalItems - 1,
                                               startCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + liveTypeDetail.TotalItems - 1,
                                               startCol).Value = liveTypeDetail.LiveName;
            foreach (SportTypeDetail sportTypeDetail in liveTypeDetail.ListSportType)
            {

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 1,
                                                   startRow + nextRow + sportTypeDetail.TotalItems - 2, startCol + 1).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 1,
                                                   startRow + nextRow + sportTypeDetail.TotalItems - 2, startCol + 1).
                    Style = numberSportFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 1,
                                                   startRow + nextRow + sportTypeDetail.TotalItems - 2, startCol + 1).
                    Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + 1,
                                                   startRow + nextRow + sportTypeDetail.TotalItems - 2, startCol + 1).
                    Value = string.Format("{0}" , sportTypeDetail.SportName);
                #region detail
                foreach (WinLossDetail winLossDetail in sportTypeDetail.ListWlDetail)
                {
                    var nextCol = 1;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winLossDetail.Status;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.WinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.Position);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.Comm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol+=1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.LineTotal);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = percentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(winLossDetail.Margin.Replace("%", ""))/100;
                    nextRow++;
                }
                #endregion

                #region sub total
                var subNextCol = 0;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = string.Empty;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Center;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = "Sub Total";
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.WinLoss);
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberDefaultSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.Position);
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberDefaultSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.Comm);
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.LineTotal);
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = numberDefaultSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.Turnover);
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol += 1)].Style = percentSubFormat;
                wsReport.Cells[startRow + nextRow, startCol + (subNextCol)].Value = CastAide.AsDouble(sportTypeDetail.SubTotal.Margin.Replace("%", ""))/100;
                nextRow++;
                #endregion
            }
            #region total
            var totalNextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Style = numberSportFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Style.HorizontalAlignment =
                HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Style.HorizontalAlignment =
               HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = "Total";
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberTotalFormat;
           // wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style.Font.Color = Color.White;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.Position);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.LineTotal);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.Turnover);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = percentTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(liveTypeDetail.Total.Margin.Replace("%", ""))/100;
            nextRow++;
            #endregion
        }

        private void Export2ExcelDailyWLDirectMixParlayAdvanceFooter(ref ExcelWorksheet wsReport, int startRow, int startCol, int nextRow, WinLossDetail grandTotal)
        {
            var numberTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, true);
            var percentTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", Color.FromArgb(218, 241, 187), true, true);

            var numberDefaultTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(218, 241, 187), true, false);
            var numberDefaultTotalFormatLineTotal = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(181, 196, 208), true, true);

            var totalNextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 2).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 2).Style =
                numberTotalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (totalNextCol+=2)).Value =
                "Grand Total";

            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.Position);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormatLineTotal;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.LineTotal);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = numberDefaultTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.Turnover);
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol += 1)].Style = percentTotalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (totalNextCol)].Value = CastAide.AsDouble(grandTotal.Margin.Replace("%", "")) / 100;
        }
    }
}
