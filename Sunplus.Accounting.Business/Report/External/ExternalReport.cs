using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.Report.External;

namespace Sunplus.Accounting.Business.Report.External
{
    public class ExternalReport : IExternalReport
    {
        private IExternalReportDAO _externalReportDao;
        private string _ExternalWinlossDailySuperSessionKey = "externalwinlossdailyv2sessionkeysuper";
        private string _ExternalWinlossDailyMasterSessionKey = "externalwinlossdailyv2sessionkeymaster";
        private string _ExternalWinlossDailyAgentSessionKey = "externalwinlossdailyv2sessionkeyagent";
        private string _ExternalWinlossDailyMemberSessionKey = "externalwinlossdailyv2sessionkeymember";

        private string _ExternalWinlossDailyDetailSuperSessionKey = "externalwinlossdailydetailv2sessionkeysuper";
        private string _ExternalWinlossDailyDetailMasterSessionKey = "externalwinlossdailydetailv2sessionkeymaster";
        private string _ExternalWinlossDailyDetailAgentSessionKey = "externalwinlossdailydetailv2sessionkeyagent";
        private string _ExternalWinlossDailyDetailMemberSessionKey = "externalwinlossdailydetailv2sessionkeymember";

        public ExternalReport()
        {
        }

        public ExternalReport(IExternalReportDAO iExternalReportDao)
        {
            _externalReportDao = iExternalReportDao;
        }

        public ExternalWLDailyBO SelWinLossDailyByCom(string winlostdate)
        {
            ExternalWLDailyBO external = new ExternalWLDailyBO();
            external.Items = new List<ExternalWLDailyRow>();
            external.Total = new ExternalWLDailyRow();

            try
            {
                DataTable dataTable = new DataTable();
                DbResult dbResult = _externalReportDao.SelWinLossDailyByCom(winlostdate);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                DataRow[] rows = dataTable.Select();

                #region foreach

                foreach (DataRow row in rows)
                {
                    ExternalWLDailyRow e = new ExternalWLDailyRow();
                    e.UserName = CastAide.AsString(row["username"]);
                    e.CustId = CastAide.AsInt32(row["custid"]);

                    e.TotalTO = NumberFormat.DecFormat(row["totalTO"], 2);
                    e.CompanyTO = NumberFormat.DecFormat(row["ComTO"], 2);
                    e.CompanyPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                      ? "0"
                                      : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.SuperTO = NumberFormat.DecFormat(row["SAgTO"], 2);
                    e.SuperPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.MasterTO = NumberFormat.DecFormat(row["MAgTO"], 2);
                    e.MasterPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.AgentTO = NumberFormat.DecFormat(row["AgTO"], 2);
                    e.AgentPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.CompanyWL = NumberFormat.DecFormat(row["ComWinLost"], 2);
                    e.CompanyMargin = (CastAide.AsDecimal(e.CompanyTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyWL) / CastAide.AsDecimal(e.CompanyTO) * 100, 2);

                    e.SuperWL = NumberFormat.DecFormat(row["SAgWinLost"], 2);
                    e.SuperMargin = (CastAide.AsDecimal(e.SuperTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperWL) / CastAide.AsDecimal(e.SuperTO) * 100, 2);

                    e.MasterWL = NumberFormat.DecFormat(row["MAgWinLost"], 2);
                    e.MasterMargin = (CastAide.AsDecimal(e.MasterTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterWL) / CastAide.AsDecimal(e.MasterTO) * 100, 2);

                    e.AgentWL = NumberFormat.DecFormat(row["AgWinLost"], 2);
                    e.AgentMargin = (CastAide.AsDecimal(e.AgentTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentWL) / CastAide.AsDecimal(e.AgentTO) * 100, 2);

                    external.Items.Add(e);
                }

                #endregion foreach

                #region Total

                external.Total = new ExternalWLDailyRow();
                external.Total.UserName = "Grand Total";

                external.Total.TotalTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2);

                external.Total.CompanyTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyTO)), 2);
                external.Total.CompanyPT = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.SuperTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperTO)), 2);
                external.Total.SuperPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.MasterTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterTO)), 2);
                external.Total.MasterPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.AgentTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentTO)), 2);
                external.Total.AgentPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.CompanyWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyWL)), 2);
                external.Total.CompanyMargin = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyWL) / CastAide.AsDecimal(external.Total.CompanyTO) * 100, 2);

                external.Total.SuperWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperWL)), 2);
                external.Total.SuperMargin = (CastAide.AsDecimal(external.Total.SuperTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperWL) / CastAide.AsDecimal(external.Total.SuperTO) * 100, 2);

                external.Total.MasterWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterWL)), 2);
                external.Total.MasterMargin = (CastAide.AsDecimal(external.Total.MasterTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterWL) / CastAide.AsDecimal(external.Total.MasterTO) * 100, 2);

                external.Total.AgentWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentWL)), 2);
                external.Total.AgentMargin = (CastAide.AsDecimal(external.Total.AgentTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentWL) / CastAide.AsDecimal(external.Total.AgentTO) * 100, 2);

                #endregion Total
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailySuperSessionKey, external);
            return external;
        }

        #region export 2 excel - Winloss daily by com

        public void Export2ExcelWinLossDailyByCom(string wlDate, string reportTitle, bool isExcludeMargin)
        {
            if (isExcludeMargin)
                Export2ExcelWinLossDailyByComExcludeMargin(wlDate, reportTitle);
            else
                Export2ExcelWinLossDailyByComIncludeMargin(wlDate, reportTitle);
        }

        private void Export2ExcelWinLossDailyByComIncludeMargin(string wlDate, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "SuperDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 17;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByComAssignHeader(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailySuperSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByComAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByComAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByComExcludeMargin(string wlDate, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "SuperDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 14;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByComAssignHeaderExcludeMargin(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailySuperSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByComAssignContentExcludeMargin(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByComAssignFooterExcludeMargin(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByComAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 8;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss & Margin";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByComAssignHeaderExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 4;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByComAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByComAssignContentExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        private void Export2ExcelWinLossDailyByComAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByComAssignFooterExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        #endregion export 2 excel - Winloss daily by com

        #region export 2 excel - Winloss daily by Super

        public void Export2ExcelWinLossDailyBySuper(string wlDate, int uplineId, string reportTitle, bool isExcludeMargin)
        {
            if (isExcludeMargin)
                Export2ExcelWinLossDailyBySuperExcludeMargin(wlDate, uplineId, reportTitle);
            else
                Export2ExcelWinLossDailyBySuperIncludeMargin(wlDate, uplineId, reportTitle);
        }

        private void Export2ExcelWinLossDailyBySuperIncludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "MasterDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 17;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyBySuperAssignHeader(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyMasterSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyBySuperAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyBySuperAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyBySuperExcludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "MasterDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 14;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyBySuperAssignHeaderExcludeMargin(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyMasterSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyBySuperAssignContentExcludeMargin(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyBySuperAssignFooterExcludeMargin(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyBySuperAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 8;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss & Margin";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyBySuperAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyBySuperAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyBySuperAssignHeaderExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 4;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyBySuperAssignContentExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        private void Export2ExcelWinLossDailyBySuperAssignFooterExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        #endregion export 2 excel - Winloss daily by Super

        #region export 2 excel - Winloss daily by Master

        public void Export2ExcelWinLossDailyByMaster(string wlDate, int uplineId, string reportTitle, bool isExcludeMargin)
        {
            if (isExcludeMargin)
                Export2ExcelWinLossDailyByMasterExcludeMargin(wlDate, uplineId, reportTitle);
            else
                Export2ExcelWinLossDailyByMasterIncludeMargin(wlDate, uplineId, reportTitle);
        }

        private void Export2ExcelWinLossDailyByMasterIncludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "AgentDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 17;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByMasterAssignHeader(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyAgentSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByMasterAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByMasterAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByMasterExcludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "AgentDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 14;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByMasterAssignHeaderExcludeMargin(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyAgentSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByMasterAssignContentExcludeMargin(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByMasterAssignFooterExcludeMargin(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByMasterAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 8;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss & Margin";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByMasterAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByMasterAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByMasterAssignHeaderExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 4;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByMasterAssignContentExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        private void Export2ExcelWinLossDailyByMasterAssignFooterExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        #endregion export 2 excel - Winloss daily by Master

        #region export 2 excel - Winloss daily by Agent

        public void Export2ExcelWinLossDailyByAgent(string wlDate, int uplineId, string reportTitle, bool isExcludeMargin)
        {
            if (isExcludeMargin)
                Export2ExcelWinLossDailyByAgentExcludeMargin(wlDate, uplineId, reportTitle);
            else
                Export2ExcelWinLossDailyByAgentIncludeMargin(wlDate, uplineId, reportTitle);
        }

        private void Export2ExcelWinLossDailyByAgentIncludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "MemberDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 17;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByAgentAssignHeader(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyMemberSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByAgentAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByAgentAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByAgentExcludeMargin(string wlDate, int uplineId, string reportTitle)
        {
            try
            {
                var startRow = 3;
                var startCol = 0;
                var reportName = "MemberDailyWinLoss";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                #region Header

                var headerMergeCols = 14;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Style.
                    HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                    Value =
                    reportTitle;
                Export2ExcelWinLossDailyByAgentAssignHeaderExcludeMargin(ref wsReport, startCol, startRow);

                #endregion Header

                #region content

                var getData =
                    (ExternalWLDailyBO)
                    SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyMemberSessionKey);
                if (getData != null && getData.Items.Count > 0)
                {
                    var index = 1;
                    foreach (var item in getData.Items)
                    {
                        Export2ExcelWinLossDailyByAgentAssignContentExcludeMargin(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                   index);
                        nextRow += 1;
                        index += 1;
                    }
                    Export2ExcelWinLossDailyByAgentAssignFooterExcludeMargin(ref wsReport, startCol, startRow, ref nextRow,
                                                              getData.Total);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void Export2ExcelWinLossDailyByAgentAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 8;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss & Margin";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByAgentAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByAgentAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterMargin);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentMargin);
        }

        private void Export2ExcelWinLossDailyByAgentAssignHeaderExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var groupColTurnover = 10;
            var groupColWinloss = 4;
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region first row

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + groupColTurnover).Value
                = "Turnover & Position Taking";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Merged
                = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + groupColTurnover + 1, startRow - 2,
                                               startCol + groupColTurnover + groupColWinloss).Value
                = "Win/Loss";

            #endregion first row

            #region second row

            var nextCol = 0;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value = "No.";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Username";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total T/O";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "%";
            //-------------------------
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Company";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Master";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Agent";

            #endregion second row
        }

        private void Export2ExcelWinLossDailyByAgentAssignContentExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row, int index)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgIndex = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
            wsReport.Cells[startRow + nextRow, startCol].Value = index;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        private void Export2ExcelWinLossDailyByAgentAssignFooterExcludeMargin(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDailyRow row)
        {
            var color = Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);

            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style =
                bgFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value =
                "Grand Total";
            nextCol += 1;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.TotalTO);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterPT);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentTO);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentPT);
            //-------------------------
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.CompanyWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.SuperWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.MasterWL);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(row.AgentWL);
        }

        #endregion export 2 excel - Winloss daily by Agent

        public ExternalWLDailyBO SelWinLossDailyBySuper(string winlostdate, int custid)
        {
            ExternalWLDailyBO external = new ExternalWLDailyBO();
            external.Items = new List<ExternalWLDailyRow>();
            external.Total = new ExternalWLDailyRow();

            try
            {
                DataTable dataTable = new DataTable();
                DbResult dbResult = _externalReportDao.SelWinLossDailyBySuper(winlostdate, custid);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                DataRow[] rows = dataTable.Select();

                #region foreach

                foreach (DataRow row in rows)
                {
                    ExternalWLDailyRow e = new ExternalWLDailyRow();
                    e.UserName = CastAide.AsString(row["username"]);
                    e.CustId = CastAide.AsInt32(row["custid"]);

                    e.TotalTO = NumberFormat.DecFormat(row["totalTO"], 2);
                    e.CompanyTO = NumberFormat.DecFormat(row["ComTO"], 2);
                    e.CompanyPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                      ? "0"
                                      : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.SuperTO = NumberFormat.DecFormat(row["SAgTO"], 2);
                    e.SuperPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.MasterTO = NumberFormat.DecFormat(row["MAgTO"], 2);
                    e.MasterPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.AgentTO = NumberFormat.DecFormat(row["AgTO"], 2);
                    e.AgentPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.CompanyWL = NumberFormat.DecFormat(row["ComWinLost"], 2);
                    e.CompanyMargin = (CastAide.AsDecimal(e.CompanyTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyWL) / CastAide.AsDecimal(e.CompanyTO) * 100, 2);

                    e.SuperWL = NumberFormat.DecFormat(row["SAgWinLost"], 2);
                    e.SuperMargin = (CastAide.AsDecimal(e.SuperTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperWL) / CastAide.AsDecimal(e.SuperTO) * 100, 2);

                    e.MasterWL = NumberFormat.DecFormat(row["MAgWinLost"], 2);
                    e.MasterMargin = (CastAide.AsDecimal(e.MasterTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterWL) / CastAide.AsDecimal(e.MasterTO) * 100, 2);

                    e.AgentWL = NumberFormat.DecFormat(row["AgWinLost"], 2);
                    e.AgentMargin = (CastAide.AsDecimal(e.AgentTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentWL) / CastAide.AsDecimal(e.AgentTO) * 100, 2);

                    external.Items.Add(e);
                }

                #endregion foreach

                #region Total

                external.Total = new ExternalWLDailyRow();
                external.Total.UserName = "Grand Total";

                external.Total.TotalTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2);

                external.Total.CompanyTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyTO)), 2);
                external.Total.CompanyPT = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.SuperTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperTO)), 2);
                external.Total.SuperPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.MasterTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterTO)), 2);
                external.Total.MasterPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.AgentTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentTO)), 2);
                external.Total.AgentPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.CompanyWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyWL)), 2);
                external.Total.CompanyMargin = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyWL) / CastAide.AsDecimal(external.Total.CompanyTO) * 100, 2);

                external.Total.SuperWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperWL)), 2);
                external.Total.SuperMargin = (CastAide.AsDecimal(external.Total.SuperTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperWL) / CastAide.AsDecimal(external.Total.SuperTO) * 100, 2);

                external.Total.MasterWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterWL)), 2);
                external.Total.MasterMargin = (CastAide.AsDecimal(external.Total.MasterTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterWL) / CastAide.AsDecimal(external.Total.MasterTO) * 100, 2);

                external.Total.AgentWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentWL)), 2);
                external.Total.AgentMargin = (CastAide.AsDecimal(external.Total.AgentTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentWL) / CastAide.AsDecimal(external.Total.AgentTO) * 100, 2);

                #endregion Total
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyMasterSessionKey, external);
            return external;
        }

        public ExternalWLDailyBO SelWinLossDailyByMaster(string winlostdate, int custid)
        {
            ExternalWLDailyBO external = new ExternalWLDailyBO();
            external.Items = new List<ExternalWLDailyRow>();
            external.Total = new ExternalWLDailyRow();

            try
            {
                DataTable dataTable = new DataTable();
                DbResult dbResult = _externalReportDao.SelWinLossDailyByMaster(winlostdate, custid);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                DataRow[] rows = dataTable.Select();

                #region foreach

                foreach (DataRow row in rows)
                {
                    ExternalWLDailyRow e = new ExternalWLDailyRow();
                    e.UserName = CastAide.AsString(row["username"]);
                    e.CustId = CastAide.AsInt32(row["custid"]);

                    e.TotalTO = NumberFormat.DecFormat(row["totalTO"], 2);
                    e.CompanyTO = NumberFormat.DecFormat(row["ComTO"], 2);
                    e.CompanyPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                      ? "0"
                                      : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.SuperTO = NumberFormat.DecFormat(row["SAgTO"], 2);
                    e.SuperPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.MasterTO = NumberFormat.DecFormat(row["MAgTO"], 2);
                    e.MasterPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.AgentTO = NumberFormat.DecFormat(row["AgTO"], 2);
                    e.AgentPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.CompanyWL = NumberFormat.DecFormat(row["ComWinLost"], 2);
                    e.CompanyMargin = (CastAide.AsDecimal(e.CompanyTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyWL) / CastAide.AsDecimal(e.CompanyTO) * 100, 2);

                    e.SuperWL = NumberFormat.DecFormat(row["SAgWinLost"], 2);
                    e.SuperMargin = (CastAide.AsDecimal(e.SuperTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperWL) / CastAide.AsDecimal(e.SuperTO) * 100, 2);

                    e.MasterWL = NumberFormat.DecFormat(row["MAgWinLost"], 2);
                    e.MasterMargin = (CastAide.AsDecimal(e.MasterTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterWL) / CastAide.AsDecimal(e.MasterTO) * 100, 2);

                    e.AgentWL = NumberFormat.DecFormat(row["AgWinLost"], 2);
                    e.AgentMargin = (CastAide.AsDecimal(e.AgentTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentWL) / CastAide.AsDecimal(e.AgentTO) * 100, 2);

                    external.Items.Add(e);
                }

                #endregion foreach

                #region Total

                external.Total = new ExternalWLDailyRow();
                external.Total.UserName = "Grand Total";

                external.Total.TotalTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2);

                external.Total.CompanyTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyTO)), 2);
                external.Total.CompanyPT = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.SuperTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperTO)), 2);
                external.Total.SuperPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.MasterTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterTO)), 2);
                external.Total.MasterPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.AgentTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentTO)), 2);
                external.Total.AgentPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.CompanyWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyWL)), 2);
                external.Total.CompanyMargin = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyWL) / CastAide.AsDecimal(external.Total.CompanyTO) * 100, 2);

                external.Total.SuperWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperWL)), 2);
                external.Total.SuperMargin = (CastAide.AsDecimal(external.Total.SuperTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperWL) / CastAide.AsDecimal(external.Total.SuperTO) * 100, 2);

                external.Total.MasterWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterWL)), 2);
                external.Total.MasterMargin = (CastAide.AsDecimal(external.Total.MasterTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterWL) / CastAide.AsDecimal(external.Total.MasterTO) * 100, 2);

                external.Total.AgentWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentWL)), 2);
                external.Total.AgentMargin = (CastAide.AsDecimal(external.Total.AgentTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentWL) / CastAide.AsDecimal(external.Total.AgentTO) * 100, 2);

                #endregion Total
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyAgentSessionKey, external);
            return external;
        }

        public ExternalWLDailyBO SelWinLossDailyByAgent(string winlostdate, int custid)
        {
            ExternalWLDailyBO external = new ExternalWLDailyBO();
            external.Items = new List<ExternalWLDailyRow>();
            external.Total = new ExternalWLDailyRow();

            try
            {
                DataTable dataTable = new DataTable();
                DbResult dbResult = _externalReportDao.SelWinLossDailyByAgent(winlostdate, custid);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                DataRow[] rows = dataTable.Select();

                #region foreach

                foreach (DataRow row in rows)
                {
                    ExternalWLDailyRow e = new ExternalWLDailyRow();
                    e.UserName = CastAide.AsString(row["username"]);
                    e.CustId = CastAide.AsInt32(row["custid"]);

                    e.TotalTO = NumberFormat.DecFormat(row["totalTO"], 2);
                    e.CompanyTO = NumberFormat.DecFormat(row["ComTO"], 2);
                    e.CompanyPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                      ? "0"
                                      : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.SuperTO = NumberFormat.DecFormat(row["SAgTO"], 2);
                    e.SuperPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.MasterTO = NumberFormat.DecFormat(row["MAgTO"], 2);
                    e.MasterPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.AgentTO = NumberFormat.DecFormat(row["AgTO"], 2);
                    e.AgentPT = (CastAide.AsDecimal(e.TotalTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentTO) / CastAide.AsDecimal(e.TotalTO) * 100, 2);

                    e.CompanyWL = NumberFormat.DecFormat(row["ComWinLost"], 2);
                    e.CompanyMargin = (CastAide.AsDecimal(e.CompanyTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.CompanyWL) / CastAide.AsDecimal(e.CompanyTO) * 100, 2);

                    e.SuperWL = NumberFormat.DecFormat(row["SAgWinLost"], 2);
                    e.SuperMargin = (CastAide.AsDecimal(e.SuperTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.SuperWL) / CastAide.AsDecimal(e.SuperTO) * 100, 2);

                    e.MasterWL = NumberFormat.DecFormat(row["MAgWinLost"], 2);
                    e.MasterMargin = (CastAide.AsDecimal(e.MasterTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.MasterWL) / CastAide.AsDecimal(e.MasterTO) * 100, 2);

                    e.AgentWL = NumberFormat.DecFormat(row["AgWinLost"], 2);
                    e.AgentMargin = (CastAide.AsDecimal(e.AgentTO) == 0)
                                    ? "0"
                                    : NumberFormat.DecFormat(CastAide.AsDecimal(e.AgentWL) / CastAide.AsDecimal(e.AgentTO) * 100, 2);

                    external.Items.Add(e);
                }

                #endregion foreach

                #region Total

                external.Total = new ExternalWLDailyRow();
                external.Total.UserName = "Grand Total";

                external.Total.TotalTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2);

                external.Total.CompanyTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyTO)), 2);
                external.Total.CompanyPT = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.SuperTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperTO)), 2);
                external.Total.SuperPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.MasterTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterTO)), 2);
                external.Total.MasterPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.AgentTO = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentTO)), 2);
                external.Total.AgentPT = (CastAide.AsDecimal(external.Total.TotalTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentTO) / CastAide.AsDecimal(external.Total.TotalTO) * 100, 2);

                external.Total.CompanyWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.CompanyWL)), 2);
                external.Total.CompanyMargin = (CastAide.AsDecimal(external.Total.CompanyTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.CompanyWL) / CastAide.AsDecimal(external.Total.CompanyTO) * 100, 2);

                external.Total.SuperWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.SuperWL)), 2);
                external.Total.SuperMargin = (CastAide.AsDecimal(external.Total.SuperTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.SuperWL) / CastAide.AsDecimal(external.Total.SuperTO) * 100, 2);

                external.Total.MasterWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.MasterWL)), 2);
                external.Total.MasterMargin = (CastAide.AsDecimal(external.Total.MasterTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.MasterWL) / CastAide.AsDecimal(external.Total.MasterTO) * 100, 2);

                external.Total.AgentWL = NumberFormat.DecFormat(external.Items.Sum(s => CastAide.AsDecimal(s.AgentWL)), 2);
                external.Total.AgentMargin = (CastAide.AsDecimal(external.Total.AgentTO) == 0)
                                               ? "0"
                                               : NumberFormat.DecFormat(CastAide.AsDecimal(external.Total.AgentWL) / CastAide.AsDecimal(external.Total.AgentTO) * 100, 2);

                #endregion Total
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyMemberSessionKey, external);
            return external;
        }

        public ExternalWLDetail SelWinLossDetailByCom(string param, Sorting sorting)
        {
            ExternalWLDetail external = new ExternalWLDetail();
            external.Items = new List<ExternalWLDetailRow>();
            external.Total = new ExternalWLDetailRow();

            try
            {
                external = SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailSuperSessionKey) as ExternalWLDetail;

                if (external == null || sorting == null)
                {
                    external = new ExternalWLDetail();
                    external.Items = new List<ExternalWLDetailRow>();
                    external.Total = new ExternalWLDetailRow();

                    DataTable dataTable = new DataTable();
                    DbResult dbResult = _externalReportDao.SelWinLossDetailByCom(param);
                    if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                        dataTable = dbResult.ResultSet.Tables[0];

                    DataRow[] rows = dataTable.Select();

                    #region foreach

                    foreach (DataRow row in rows)
                    {
                        external.Items.Add(new ExternalWLDetailRow()
                                               {
                                                   CustId = CastAide.AsInt32(row["custid"]),
                                                   UserName = CastAide.AsString(row["username"]),

                                                   Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                                                   GrossComm = NumberFormat.DecFormat(row["grosscomm"], 2),

                                                   WinLoss = NumberFormat.DecFormat(row["memberwinlost"], 2),
                                                   Comm = NumberFormat.DecFormat(row["playercomm"], 2),
                                                   Total = NumberFormat.DecFormat(row["membertotal"], 2),

                                                   AWinLoss = NumberFormat.DecFormat(row["agwinlost"], 2),
                                                   AComm = NumberFormat.DecFormat(row["agcomm"], 2),
                                                   ATotal = NumberFormat.DecFormat(row["agtotal"], 2),

                                                   MWinLoss = NumberFormat.DecFormat(row["magwinlost"], 2),
                                                   MComm = NumberFormat.DecFormat(row["magcomm"], 2),
                                                   MTotal = NumberFormat.DecFormat(row["magtotal"], 2),

                                                   SWinLoss = NumberFormat.DecFormat(row["sagwinlost"], 2),
                                                   SComm = NumberFormat.DecFormat(row["sagcomm"], 2),
                                                   STotal = NumberFormat.DecFormat(row["sagtotal"], 2),

                                                   Company = NumberFormat.DecFormat(row["company"], 2),
                                               });
                    }

                    #endregion foreach

                    #region total

                    external.Total = new ExternalWLDetailRow()
                                         {
                                             UserName = "Total",
                                             Turnover =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                                             GrossComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.GrossComm)), 2),

                                             WinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                             Comm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2),
                                             Total =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Total)), 2),

                                             AWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AWinLoss)), 2),
                                             AComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AComm)), 2),
                                             ATotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.ATotal)), 2),

                                             MWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MWinLoss)), 2),
                                             MComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MComm)), 2),
                                             MTotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MTotal)), 2),

                                             SWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.SWinLoss)), 2),
                                             SComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.SComm)), 2),
                                             STotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.STotal)), 2),

                                             Company =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Company)), 2),
                                         };

                    #endregion total

                    SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailSuperSessionKey, external);
                }

                SortList(external.Items, sorting);
                SortByIndicatorUser(external.Items, sorting);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return external;
        }

        private void SortByIndicatorUser(List<ExternalWLDetailRow> dataSource, Sorting sorting)
        {
            if (sorting == null)
                return;
            if (sorting.IsDecimal)
                return;

            var external = ExternalSection.Instance[ExternalSection.ExternalCollections.Sorting];
            foreach (ExternalElement element in external)
            {
                string[] usernames = element.Users.Split(',');
                foreach (string username in usernames)
                {
                    var lst = dataSource.Select(s => s).Where(s => s.UserName.ToLower() == username.ToLower()).ToList();
                    foreach (ExternalWLDetailRow row in lst)
                    {
                        dataSource.Remove(row);
                        dataSource.Insert(element.Index - 1, row);
                    }
                }
            }
        }

        private void SortList<T>(List<T> dataSource, Sorting sorting)
        {
            if (sorting == null)
                return;
            if (sorting.sortType == 0)
                return;

            PropertyInfo propInfo = typeof(T).GetProperty(sorting.fieldName);

            if (propInfo == null)
                return;

            bool IsAsc = false;
            if (sorting.sortType == 1)
                IsAsc = true;
            else if (sorting.sortType == 2)
                IsAsc = false;
            if (!sorting.IsDecimal)
            {
                Comparison<T> compare = delegate(T a, T b)
                {
                    object valueA = IsAsc ? (propInfo.GetValue(a, null)) : (propInfo.GetValue(b, null));
                    object valueB = IsAsc ? (propInfo.GetValue(b, null)) : (propInfo.GetValue(a, null));

                    return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                };
                dataSource.Sort(compare);
            }
            else
            {
                Comparison<T> compare = delegate(T a, T b)
                {
                    object valueA = IsAsc ? CastAide.AsDecimal(propInfo.GetValue(a, null)) : CastAide.AsDecimal(propInfo.GetValue(b, null));
                    object valueB = IsAsc ? CastAide.AsDecimal(propInfo.GetValue(b, null)) : CastAide.AsDecimal(propInfo.GetValue(a, null));

                    return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                };
                dataSource.Sort(compare);
            }
        }

        public ExternalWLDetail SelWinLossDetailBySuper(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            ExternalWLDetail external = new ExternalWLDetail();
            external.Items = new List<ExternalWLDetailRow>();
            external.Total = new ExternalWLDetailRow();

            try
            {
                external = SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMasterSessionKey) as ExternalWLDetail;

                if (external == null || sorting == null)
                {
                    external = new ExternalWLDetail();
                    external.Items = new List<ExternalWLDetailRow>();
                    external.Total = new ExternalWLDetailRow();

                    DataTable dataTable = new DataTable();
                    DbResult dbResult = _externalReportDao.SelWinLossDetailBySuper(custid, param, pageIndex, pageSize);
                    if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                        dataTable = dbResult.ResultSet.Tables[0];

                    DataRow[] rows = dataTable.Select();

                    #region foreach

                    foreach (DataRow row in rows)
                    {
                        external.Items.Add(new ExternalWLDetailRow()
                                               {
                                                   CustId = CastAide.AsInt32(row["custid"]),
                                                   UserName = CastAide.AsString(row["username"]),

                                                   Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                                                   GrossComm = NumberFormat.DecFormat(row["grosscomm"], 2),

                                                   WinLoss = NumberFormat.DecFormat(row["memberwinlost"], 2),
                                                   Comm = NumberFormat.DecFormat(row["playercomm"], 2),
                                                   Total = NumberFormat.DecFormat(row["membertotal"], 2),

                                                   AWinLoss = NumberFormat.DecFormat(row["agwinlost"], 2),
                                                   AComm = NumberFormat.DecFormat(row["agcomm"], 2),
                                                   ATotal = NumberFormat.DecFormat(row["agtotal"], 2),

                                                   MWinLoss = NumberFormat.DecFormat(row["magwinlost"], 2),
                                                   MComm = NumberFormat.DecFormat(row["magcomm"], 2),
                                                   MTotal = NumberFormat.DecFormat(row["magtotal"], 2),

                                                   SWinLoss = NumberFormat.DecFormat(row["sagwinlost"], 2),
                                                   SComm = NumberFormat.DecFormat(row["sagcomm"], 2),
                                                   STotal = NumberFormat.DecFormat(row["sagtotal"], 2),

                                                   Company = NumberFormat.DecFormat(row["company"], 2),
                                               });
                    }

                    #endregion foreach

                    #region total

                    external.Total = new ExternalWLDetailRow()
                                         {
                                             UserName = "Total",
                                             Turnover =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                                             GrossComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.GrossComm)), 2),

                                             WinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                             Comm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2),
                                             Total =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Total)), 2),

                                             AWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AWinLoss)), 2),
                                             AComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AComm)), 2),
                                             ATotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.ATotal)), 2),

                                             MWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MWinLoss)), 2),
                                             MComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MComm)), 2),
                                             MTotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MTotal)), 2),

                                             SWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.SWinLoss)), 2),
                                             SComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.SComm)), 2),
                                             STotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.STotal)), 2),

                                             Company =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Company)), 2),
                                         };

                    #endregion total

                    SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMasterSessionKey, external);
                }

                SortList(external.Items, sorting);
                SortByIndicatorUser(external.Items, sorting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return external;
        }

        public ExternalWLDetail SelWinLossDetailByMaster(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            ExternalWLDetail external = new ExternalWLDetail();
            external.Items = new List<ExternalWLDetailRow>();
            external.Total = new ExternalWLDetailRow();

            try
            {
                external = SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailAgentSessionKey) as ExternalWLDetail;

                if (external == null || sorting == null)
                {
                    external = new ExternalWLDetail();
                    external.Items = new List<ExternalWLDetailRow>();
                    external.Total = new ExternalWLDetailRow();

                    DataTable dataTable = new DataTable();
                    DbResult dbResult = _externalReportDao.SelWinLossDetailByMaster(custid, param, pageIndex, pageSize);
                    if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                        dataTable = dbResult.ResultSet.Tables[0];

                    DataRow[] rows = dataTable.Select();

                    #region foreach

                    foreach (DataRow row in rows)
                    {
                        external.Items.Add(new ExternalWLDetailRow()
                                               {
                                                   CustId = CastAide.AsInt32(row["custid"]),
                                                   UserName = CastAide.AsString(row["username"]),

                                                   Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                                                   GrossComm = NumberFormat.DecFormat(row["grosscomm"], 2),

                                                   WinLoss = NumberFormat.DecFormat(row["memberwinlost"], 2),
                                                   Comm = NumberFormat.DecFormat(row["playercomm"], 2),
                                                   Total = NumberFormat.DecFormat(row["membertotal"], 2),

                                                   AWinLoss = NumberFormat.DecFormat(row["agwinlost"], 2),
                                                   AComm = NumberFormat.DecFormat(row["agcomm"], 2),
                                                   ATotal = NumberFormat.DecFormat(row["agtotal"], 2),

                                                   MWinLoss = NumberFormat.DecFormat(row["magwinlost"], 2),
                                                   MComm = NumberFormat.DecFormat(row["magcomm"], 2),
                                                   MTotal = NumberFormat.DecFormat(row["magtotal"], 2),

                                                   Company = NumberFormat.DecFormat(row["company"], 2),
                                               });
                    }

                    #endregion foreach

                    #region total

                    external.Total = new ExternalWLDetailRow()
                                         {
                                             UserName = "Total",
                                             Turnover =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                                             GrossComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.GrossComm)), 2),

                                             WinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                             Comm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2),
                                             Total =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Total)), 2),

                                             AWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AWinLoss)), 2),
                                             AComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AComm)), 2),
                                             ATotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.ATotal)), 2),

                                             MWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MWinLoss)), 2),
                                             MComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MComm)), 2),
                                             MTotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.MTotal)), 2),

                                             Company =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Company)), 2),
                                         };

                    #endregion total

                    SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailAgentSessionKey, external);
                }

                SortList(external.Items, sorting);
                SortByIndicatorUser(external.Items, sorting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return external;
        }

        public ExternalWLDetail SelWinLossDetailByAgent(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            ExternalWLDetail external = new ExternalWLDetail();
            external.Items = new List<ExternalWLDetailRow>();
            external.Total = new ExternalWLDetailRow();

            try
            {
                external = SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMemberSessionKey) as ExternalWLDetail;

                if (external == null || sorting == null)
                {
                    external = new ExternalWLDetail();
                    external.Items = new List<ExternalWLDetailRow>();
                    external.Total = new ExternalWLDetailRow();

                    DataTable dataTable = new DataTable();
                    DbResult dbResult = _externalReportDao.SelWinLossDetailByAgent(custid, param, pageIndex, pageSize);
                    if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                        dataTable = dbResult.ResultSet.Tables[0];

                    DataRow[] rows = dataTable.Select();

                    #region foreach

                    foreach (DataRow row in rows)
                    {
                        external.Items.Add(new ExternalWLDetailRow()
                                               {
                                                   CustId = CastAide.AsInt32(row["custid"]),
                                                   UserName = CastAide.AsString(row["username"]),

                                                   Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                                                   GrossComm = NumberFormat.DecFormat(row["grosscomm"], 2),

                                                   WinLoss = NumberFormat.DecFormat(row["memberwinlost"], 2),
                                                   Comm = NumberFormat.DecFormat(row["playercomm"], 2),
                                                   Total = NumberFormat.DecFormat(row["membertotal"], 2),

                                                   AWinLoss = NumberFormat.DecFormat(row["agwinlost"], 2),
                                                   AComm = NumberFormat.DecFormat(row["agcomm"], 2),
                                                   ATotal = NumberFormat.DecFormat(row["agtotal"], 2),

                                                   Company = NumberFormat.DecFormat(row["company"], 2),
                                               });
                    }

                    #endregion foreach

                    #region total

                    external.Total = new ExternalWLDetailRow()
                                         {
                                             UserName = "Total",
                                             Turnover =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                                             GrossComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.GrossComm)), 2),

                                             WinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.WinLoss)), 2),
                                             Comm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Comm)), 2),
                                             Total =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Total)), 2),

                                             AWinLoss =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AWinLoss)), 2),
                                             AComm =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.AComm)), 2),
                                             ATotal =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.ATotal)), 2),

                                             Company =
                                                 NumberFormat.DecFormat(
                                                     external.Items.Sum(s => CastAide.AsDecimal(s.Company)), 2),
                                         };

                    #endregion total

                    SessionExtension.AddSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMemberSessionKey, external);
                }

                SortList(external.Items, sorting);
                SortByIndicatorUser(external.Items, sorting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return external;
        }

        #region export 2 excel - Winloss detail by com

        public void Export2ExcelWinLossDetailByCom(string reportTitle, Param param)
        {
            var startRow = 3;
            var startCol = 0;
            var reportName = "SuperDailyWinLossDetail";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header

            var headerMergeCols = 15;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Value =
                reportTitle;
            Export2ExcelWinLossDetailByComAssignHeader(ref wsReport, startCol, startRow);

            #endregion Header

            #region content

            var getData =
                (ExternalWLDetail)
                SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailSuperSessionKey);

            if (getData != null && getData.Items != null && getData.Items.Count > 0)
            {
                foreach (var item in getData.Items)
                {
                    Export2ExcelWinLossDetailByComAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                false);
                    nextRow++;
                }
                Export2ExcelWinLossDetailByComAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.Total,
                                                                true);
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

        private void Export2ExcelWinLossDetailByComAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = "Username";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "GrossComm";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            //Member group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Member";

            //Agent group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Agent";

            //Master group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Master";

            //Super group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Super";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Company";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            var nextCol2 = 2;
            //group detail - member
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - agent
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - master
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - super
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
        }

        private void Export2ExcelWinLossDetailByComAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDetailRow row, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isFooter, false, false);
            var nextCol = 0;

            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.GrossComm);
            //member
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Total);
            //agent
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.ATotal);
            //master
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MTotal);
            //super
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.SWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.SComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.STotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Company);
        }

        #endregion export 2 excel - Winloss detail by com

        #region export 2 excel - Winloss detail by super

        public void Export2ExcelWinLossDetailBySuper(string reportTitle, Param param)
        {
            var startRow = 3;
            var startCol = 0;
            var reportName = "MasterDailyWinLossDetail";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header

            var headerMergeCols = 15;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Value =
                reportTitle;
            Export2ExcelWinLossDetailBySuperAssignHeader(ref wsReport, startCol, startRow);

            #endregion Header

            #region content

            var getData =
                (ExternalWLDetail)
                SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMasterSessionKey);

            if (getData != null && getData.Items != null && getData.Items.Count > 0)
            {
                foreach (var item in getData.Items)
                {
                    Export2ExcelWinLossDetailBySuperAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                false);
                    nextRow++;
                }
                Export2ExcelWinLossDetailBySuperAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.Total,
                                                                true);
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

        private void Export2ExcelWinLossDetailBySuperAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = "Username";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "GrossComm";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            //Member group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Member";

            //Agent group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Agent";

            //Master group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Master";

            //Super group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Super";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Company";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            var nextCol2 = 2;
            //group detail - member
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - agent
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - master
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - super
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
        }

        private void Export2ExcelWinLossDetailBySuperAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDetailRow row, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isFooter, false, false);
            var nextCol = 0;

            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.GrossComm);
            //member
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Total);
            //agent
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.ATotal);
            //master
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MTotal);
            //super
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.SWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.SComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.STotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Company);
        }

        #endregion export 2 excel - Winloss detail by super

        #region export 2 excel - Winloss detail by master

        public void Export2ExcelWinLossDetailByMaster(string reportTitle, Param param)
        {
            var startRow = 3;
            var startCol = 0;
            var reportName = "AgentDailyWinLossDetail";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header

            var headerMergeCols = 12;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Value =
                reportTitle;
            Export2ExcelWinLossDetailByMasterAssignHeader(ref wsReport, startCol, startRow);

            #endregion Header

            #region content

            var getData =
                (ExternalWLDetail)
                SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailAgentSessionKey);

            if (getData != null && getData.Items != null && getData.Items.Count > 0)
            {
                foreach (var item in getData.Items)
                {
                    Export2ExcelWinLossDetailByMasterAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                false);
                    nextRow++;
                }
                Export2ExcelWinLossDetailByMasterAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.Total,
                                                                true);
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

        private void Export2ExcelWinLossDetailByMasterAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = "Username";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "GrossComm";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            //Member group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Member";

            //Agent group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Agent";

            //Master group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Master";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Company";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            var nextCol2 = 2;
            //group detail - member
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - agent
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - master
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
        }

        private void Export2ExcelWinLossDetailByMasterAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDetailRow row, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isFooter, false, false);
            var nextCol = 0;

            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.GrossComm);
            //member
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Total);
            //agent
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.ATotal);
            //master
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.MTotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Company);
        }

        #endregion export 2 excel - Winloss detail by master

        #region export 2 excel - Winloss detail by agent

        public void Export2ExcelWinLossDetailByAgent(string reportTitle, Param param)
        {
            var startRow = 3;
            var startCol = 0;
            var reportName = "MemberDailyWinLossDetail";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header

            var headerMergeCols = 9;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + headerMergeCols).
                Value =
                reportTitle;
            Export2ExcelWinLossDetailByAgentAssignHeader(ref wsReport, startCol, startRow);

            #endregion Header

            #region content

            var getData =
                (ExternalWLDetail)
                SessionExtension.GetSession(HttpContext.Current.Session, _ExternalWinlossDailyDetailMemberSessionKey);

            if (getData != null && getData.Items != null && getData.Items.Count > 0)
            {
                foreach (var item in getData.Items)
                {
                    Export2ExcelWinLossDetailByAgentAssignContent(ref wsReport, startCol, startRow, ref nextRow, item,
                                                                false);
                    nextRow++;
                }
                Export2ExcelWinLossDetailByAgentAssignContent(ref wsReport, startCol, startRow, ref nextRow, getData.Total,
                                                                true);
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

        private void Export2ExcelWinLossDetailByAgentAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow)
        {
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = "Username";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "GrossComm";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            //Member group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Member";

            //Agent group title
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol + 3)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + (nextCol += 3)).Value = "Agent";

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 1, startCol + nextCol).Value = "Company";
            wsReport.Columns[startCol + nextCol].Width = 120 * 40;

            var nextCol2 = 2;
            //group detail - member
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            //group detail - agent
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Win/Loss";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Comm";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
            wsReport.Cells[startRow - 1, startCol + (nextCol2 += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + (nextCol2)].Value = "Total";
            wsReport.Columns[startCol + nextCol2].Width = 120 * 40;
        }

        private void Export2ExcelWinLossDetailByAgentAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, ExternalWLDetailRow row, bool isFooter)
        {
            var color = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, isFooter, false, false);
            var nextCol = 0;

            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.UserName;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.GrossComm);
            //member
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Total);
            //agent
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AWinLoss);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.AComm);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.ATotal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Company);
        }

        #endregion export 2 excel - Winloss detail by agent
    }

}