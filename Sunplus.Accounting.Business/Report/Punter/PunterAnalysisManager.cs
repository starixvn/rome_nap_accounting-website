using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;

namespace BLL.Report.Punter
{
    public class PunterAnalysisManager : ReportBase
    {
        private readonly string keySelBetCountByCustomer = "selbetcountbycustomer." + AppSession.Username;
        private readonly string keySelIpByCustomer = "selipbycustomer." + AppSession.Username;
        private readonly string keySelTopPunterByBettype = "seltoppunterbyBettype." + AppSession.Username;
        private readonly string keySelTopPunterByCustId = "seltoppunterbycustid." + AppSession.Username;
        private readonly string keySelTopPunterByLeagueCustId = "seltoppunterbyleaguecustid." + AppSession.Username;
        private readonly string keySelTopPunterBySporttype = "seltoppunterbysporttype." + AppSession.Username;
        private readonly string keySelTopPunterSummary = "seltoppuntersummary." + AppSession.Username;
        private readonly string keySelTopPunterSummaryByLeague = "seltoppuntersummarybyleague." + AppSession.Username;
        private readonly string user = "[" + AppSession.Username + "]";
        private bool hashError;
        private string msg = "<span class='error-message'>{content}</span>";

        public string LoadHeader()
        {
            string header = WebUtils.LoadTemplate("Report/Punter/PunterHeaderForm.htm");
            string month = WebUtils.LoadTemplate("Report/Punter/PeriodMonthly.htm");

            string currYear = DateTime.Today.Year.ToString();
            string currMonth = (DateTime.Today.Month - 1).ToString();

            TemplateHandler.Assign(ref header, "month", GenMonthCalendar("ddlMonth", currMonth));
            TemplateHandler.Assign(ref header, "Year", GenYearCalendar("ddlYear", currYear));
            TemplateHandler.Assign(ref header, "Carlendar", month);
            return header;
        }

        public string GenMonthCalendar(string name, string currvalue)
        {
            string[] arr = {
                               "January", "February", "March", "April", "May", "June", "July", "August", "September",
                               "October", "November", "December"
                           };
            var html = new StringBuilder();
            html.Append("<select id='" + name + "' name='" + name + "'>");
            for (int i = 0; i < arr.Length; i++)
            {
                if (currvalue == (i + 1).ToString())
                    html.Append("<option selected='selected' value='" + (i + 1) + "'>" + arr[i] + "</option>");
                else
                    html.Append("<option value='" + (i + 1) + "'>" + arr[i] + "</option>");
            }
            html.Append("</select>");
            return html.ToString();
        }

        public string GenYearCalendar(string name, string currvalue)
        {
            int start = 2007;
            int end = DateTime.Today.Year;
            var html = new StringBuilder();
            html.Append("<select id='" + name + "' name='" + name + "'>");
            for (int i = start; i <= end; i++)
            {
                if (currvalue == i.ToString())
                    html.Append("<option selected='selected' value='" + i + "'>" + i + "</option>");
                else
                    html.Append("<option value='" + i + "'>" + i + "</option>");
            }
            html.Append("</select>");
            return html.ToString();
        }

        private string negativenumber(object num, string spec)
        {
            decimal n = CastAide.AsDecimal(num);
            string str = "";
            if (n > 0)
                str = NumberFormat.DecFormat(n, 2) + spec;
            else
            {
                n = n * (-1);
                str = "<span class='negnum'>(" + NumberFormat.DecFormat(n, 2) + spec + ")</span>";
            }
            return str;
        }

        private string negativenumber(object num)
        {
            decimal n = CastAide.AsDecimal(num);
            string str = "";
            if (n > 0)
                str = NumberFormat.DecFormat(n, 2);
            else
            {
                n = n * (-1);
                str = "<span class='negnum'>(" + NumberFormat.DecFormat(n, 2) + ")</span>";
            }
            return str;
        }

        public override string RenderHTML(Hashtable input)
        {
            throw new NotImplementedException();
        }

        #region SelTopPunterSummary

        private DataTable GetSelTopPunterSummary(string username, string fromdate, string todate, string toprow,
                                                 string orderby, string ordertype)
        {
            var dt = new DataTable();

            Hashtable hash;
            hash = new Hashtable();

            if (username.Length != 0)
            {
                hash.Add("username", username);
            }

            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("toprow", toprow);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));

            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterSummary(hash);
                dt = rs.ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return dt;
        }

        public string SelTopPunterSummary(string username, string fromdate, string todate, string toprow, string orderby,
                                          string ordertype)
        {
            DataTable dt = GetSelTopPunterSummary(username, fromdate, todate, toprow, orderby, ordertype);
            DataRow[] Rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.UserName = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/TopPunterSummaryContent.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            TemplateHandler.Assign(ref content, "OrderBy", orderby);
            TemplateHandler.Assign(ref content, "OrderType", ordertype);
            TemplateHandler.Assign(ref content, "LeagueId", "");
            string row = "";
            var htmlContent = new StringBuilder();

            foreach (DataRow dataRow in Rows)
            {
                row = content;
                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "UserName", dataRow["username"]);
                TemplateHandler.Assign(ref row, "CustId", dataRow["custid"]);
                TemplateHandler.Assign(ref row, "fromdate", fromdate);
                TemplateHandler.Assign(ref row, "todate", todate);
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"]));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));
                TemplateHandler.Assign(ref row, "BetCount", NumberFormat.DecFormat(dataRow["Betcount"], 0));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIBC"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["TurnOver"]);
                total.BetCount += CastAide.AsDecimal(dataRow["BetCount"]);

                htmlContent.Append(row);
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "BetCount", NumberFormat.DecFormat(total.BetCount, 0));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (Rows.Length == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='6' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='6' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";

                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Punter Summary from " + fromdate + " to " + todate;

            //CacheData(keyPunterSummary, rows, WebSettings.CacheExpire);

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            CacheData(keySelTopPunterSummary, hashCache, WebSettings.CacheExpire);

            //Export2ExcelSelTopPunterSummary(rows, title , ref fullpath, "");
            var h = new Hashtable();
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterSummary

        #region SelTopPunterSummaryExportOnly

        public string SelTopPunterSummaryExportOnly(string username, string fromdate, string todate, string toprow,
                                                    string orderby, string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();

            if (username.Length != 0)
            {
                hash.Add("username", username);
            }

            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("toprow", toprow);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));
            DataRow[] rows = { };

            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterSummary(hash);
                DataTable dt = rs.ResultSet.Tables[0];
                rows = dt.Select();
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            string fullpath = "";
            string title = "Punter Summary from " + fromdate + " to " + todate;
            //Export2ExcelSelTopPunterSummary(rows, title, ref fullpath, "");
            var h = new Hashtable();
            h.Add("data", "");
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterSummaryExportOnly

        #region SelTopPunterByCustId

        private DataTable GetSelTopPunterByCustId(string custId, string username, string fromdate, string todate,
                                                  string orderby, string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();
            hash.Add("custid", custId);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));

            DataRow[] rows = { };
            DbResult rs = null;
            var dt = new DataTable();
            try
            {
                rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterByCustId(hash);
                dt = rs.ResultSet.Tables[0];
                rows = dt.Select();
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelTopPunterByCustId(string custId, string username, string fromdate, string todate,
                                           string orderby, string ordertype)
        {
            var dt = new DataTable();

            dt = GetSelTopPunterByCustId(custId, username, fromdate, todate, orderby, ordertype);
            DataRow[] rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.Sportmame = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/SelTopPunterByCustID.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            TemplateHandler.Assign(ref content, "OrderBy", orderby);
            TemplateHandler.Assign(ref content, "OrderType", ordertype);
            string header = TemplateHandler.CopyBlock(tpl, "Header");
            TemplateHandler.RemoveBlock(ref tpl, "Header");
            TemplateHandler.Assign(ref header, "UserName", username);
            string row = "";
            var htmlContent = new StringBuilder();

            TemplateHandler.Assign(ref content, "CustId", custId);
            TemplateHandler.Assign(ref content, "UserName", username);
            TemplateHandler.Assign(ref content, "fdate", fromdate);
            TemplateHandler.Assign(ref content, "tdate", todate);

            int count = 0;
            foreach (DataRow dataRow in rows)
            {
                row = content;

                if (dataRow["sporttype"].ToString() == "100" || dataRow["sporttype"].ToString() == "251")
                //Mix parlay, CASINO
                {
                    string str = "<td class='l'>" + dataRow["sportname"] + "</td>";
                    TemplateHandler.ReplaceBlock(ref row, "Sport", str);
                }

                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "SportName", dataRow["SportName"]);
                TemplateHandler.Assign(ref row, "Sporttype", dataRow["Sporttype"]);
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"], ""));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIbc"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["Turnover"]);

                if (CastAide.AsDecimal(dataRow["WinlostIbc"]) == 0 && CastAide.AsDecimal(dataRow["Turnover"]) == 0)
                    continue;

                htmlContent.Append(row);
                count++;
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "SportName", "Total");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC, ""));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (count == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Puner Summary by " + username + " from " + fromdate + " to " + todate;

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            hashCache.Add("filename", "PunterByUserName-" + username);
            CacheData(keySelTopPunterByCustId, hashCache, WebSettings.CacheExpire);
            //Export2ExcelSelTopPunterBy(rows, title, "PunterByUserName-" + username, "SelTopPunterByCustId", ref fullpath);

            var h = new Hashtable();
            h.Add("header", header);
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterByCustId

        #region SelTopPunterBySporttype

        private DataTable GetSelTopPunterBySporttype(string custId, string username, string sporttype, string fromdate,
                                                     string todate, string orderby, string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();
            hash.Add("custid", custId);
            hash.Add("sporttype", sporttype);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));

            DataRow[] rows = { };
            var dt = new DataTable();
            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterBySporttype(hash);
                dt = rs.ResultSet.Tables[0];
                rows = dt.Select();
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelTopPunterBySporttype(string custId, string username, string sporttype, string fromdate,
                                              string todate, string orderby, string ordertype)
        {
            DataRow[] rows = { };
            DataTable dt = GetSelTopPunterBySporttype(custId, username, sporttype, fromdate, todate, orderby, ordertype);
            rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.Sportmame = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/SelTopPunterBySporttype.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            TemplateHandler.Assign(ref content, "OrderBy", orderby);
            TemplateHandler.Assign(ref content, "OrderType", ordertype);

            string header = TemplateHandler.CopyBlock(tpl, "Header");
            TemplateHandler.RemoveBlock(ref tpl, "Header");
            TemplateHandler.Assign(ref header, "UserName", username);
            TemplateHandler.Assign(ref header, "SportName", CacheRef.GetSportName(sporttype));

            TemplateHandler.Assign(ref content, "CustId", custId);
            TemplateHandler.Assign(ref content, "UserName", username);
            TemplateHandler.Assign(ref content, "fdate", fromdate);
            TemplateHandler.Assign(ref content, "tdate", todate);

            string row = "";
            var htmlContent = new StringBuilder();

            int count = 0;
            int sport_p = 0, sport = 0;
            foreach (DataRow dataRow in rows)
            {
                row = content;

                sport_p = CastAide.AsInt32(dataRow["sporttype"]);
                if (sport != sport_p)
                {
                    TemplateHandler.Assign(ref row, "Sportname", CacheRef.GetSportName(dataRow["sporttype"]));
                    int rowspan = 1;
                    string exp = "sporttype = " + sport_p;
                    rowspan = dt.Select(exp, "").Length;
                    TemplateHandler.Assign(ref row, "rowspan", rowspan);
                    sport = sport_p;
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref row, "sportname");
                }

                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "Sporttype", sport_p);
                TemplateHandler.Assign(ref row, "BettypeName", CacheRef.GetBettype(dataRow["Bettype"]));
                TemplateHandler.Assign(ref row, "Bettype", dataRow["Bettype"]);
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"], ""));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIbc"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["Turnover"]);

                if (CastAide.AsDecimal(dataRow["WinlostIbc"]) == 0 && CastAide.AsDecimal(dataRow["Turnover"]) == 0)
                    continue;

                htmlContent.Append(row);
                count++;
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "BettypeName", "Total");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC, ""));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (count == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Punter summary by " + username + " " + CacheRef.GetSportName(sporttype) + " from " +
                           fromdate + " to " + todate;

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            hashCache.Add("filename", "PunterBySportType-" + username);
            CacheData(keySelTopPunterBySporttype, hashCache, WebSettings.CacheExpire);

            var h = new Hashtable();
            h.Add("header", header);
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterBySporttype

        #region SelTopPunterByBettype

        private DataTable GetSelTopPunterByBettype(string custId, string username, string sporttype, string bettype,
                                                   string fromdate, string todate, string orderby, string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();
            hash.Add("custid", custId);
            hash.Add("sporttype", sporttype);
            hash.Add("bettype", bettype);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));

            DataRow[] rows = { };
            var dt = new DataTable();
            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterByBettype(hash);
                dt = rs.ResultSet.Tables[0];
                rows = dt.Select();
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelTopPunterByBettype(string custId, string username, string sporttype, string bettype,
                                            string fromdate, string todate, string orderby, string ordertype)
        {
            DataTable dt = GetSelTopPunterByBettype(custId, user, sporttype, bettype, fromdate, todate, orderby,
                                                    ordertype);
            DataRow[] rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.Sportmame = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/SelTopPunterByBettype.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            string header = TemplateHandler.CopyBlock(tpl, "Header");
            TemplateHandler.RemoveBlock(ref tpl, "Header");
            TemplateHandler.Assign(ref header, "UserName", username);
            TemplateHandler.Assign(ref header, "SportName", CacheRef.GetSportName(sporttype));
            TemplateHandler.Assign(ref header, "Bettype", CacheRef.GetBettype(bettype));
            string row = "";
            var htmlContent = new StringBuilder();

            int count = 0;
            foreach (DataRow dataRow in rows)
            {
                row = content;
                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "League", CacheRef.GetLeagueName(dataRow["leagueid"]));
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"], ""));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIbc"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["Turnover"]);

                if (CastAide.AsDecimal(dataRow["WinlostIbc"]) == 0 && CastAide.AsDecimal(dataRow["Turnover"]) == 0)
                    continue;

                htmlContent.Append(row);
                count++;
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "League", "Total");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC, ""));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (count == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Punter summary by " + username + " " + CacheRef.GetSportName(sporttype) + " - " +
                           CacheRef.GetBettype(bettype) + " from " + fromdate + " to " + todate;

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            hashCache.Add("filename", "PunterByBettype-" + username);
            CacheData(keySelTopPunterByBettype, hashCache, WebSettings.CacheExpire);

            var h = new Hashtable();
            h.Add("header", header);
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterByBettype

        #region SelIpByCustomer

        private DataTable GetSelIpByCustomer(string custid, string fromdate, string todate)
        {
            var dt = new DataTable();
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                hash.Add("fromdate", fromdate);
                hash.Add("todate", todate);

                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelIPByCustomer(hash);
                dt = rs.ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelIpByCustomer(string custid, string username, string fromdate, string todate,
                                      ref string fullpath)
        {
            string tpl = WebUtils.LoadTemplate("Report/Punter/SelIPByCustomer.htm");
            TemplateHandler.Assign(ref tpl, "UserName", username);
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            var Rows = new StringBuilder();

            TemplateHandler.Assign(ref tpl, "fromdate", fromdate);
            TemplateHandler.Assign(ref tpl, "todate", todate);

            var dt = new DataTable();
            dt = GetSelIpByCustomer(custid, fromdate, todate);
            DataRow[] dataRows = dt.Select();

            foreach (DataRow dataRow in dataRows)
            {
                string row = content;
                TemplateHandler.Assign(ref row, "Month", dataRow["rptmonth"]);
                TemplateHandler.Assign(ref row, "Year", dataRow["rptyear"]);
                TemplateHandler.Assign(ref row, "IP", dataRow["IP"]);
                TemplateHandler.Assign(ref row, "Country", dataRow["Country"]);

                Rows.Append(row);
            }

            if (dataRows.Length == 0)
                TemplateHandler.ReplaceBlock(ref tpl, "Content",
                                             "<tr bgcolor='white'><td class='c' colspan='4'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") +
                                             "</td></tr>");
            else
                TemplateHandler.ReplaceBlock(ref tpl, "Content", Rows.ToString());
            string title = "Punter view IP by " + username + " from " + fromdate + " to " + todate;

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            hashCache.Add("filename", "PunterByIP-" + username);
            CacheData(keySelIpByCustomer, hashCache, WebSettings.CacheExpire);

            var h = new Hashtable();
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return tpl;
        }

        #endregion SelIpByCustomer

        #region SelBetCountByCustomer

        private DataTable GetSelBetCountByCustomer(string custid, string username, string fromdate, string todate,
                                                   string leagueid)
        {
            Hashtable hash;
            hash = new Hashtable();
            hash.Add("custid", custid);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            if (CastAide.AsInt32(leagueid) > 0)
                hash.Add("leagueid", leagueid);

            DataRow[] rows = { };
            var dt = new DataTable();
            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelBetCountByCustomer(hash);
                dt = rs.ResultSet.Tables[0];
                rows = dt.Select();
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelBetCountByCustomer(string custid, string username, string fromdate, string todate,
                                            ref string fullpath, string leagueid)
        {
            DataRow[] rows = { };
            DataTable dt = GetSelBetCountByCustomer(custid, username, fromdate, todate, leagueid);
            rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.UserName = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/SelBetCountByCustomer.htm");

            TemplateHandler.Assign(ref tpl, "fromdate", fromdate);
            TemplateHandler.Assign(ref tpl, "todate", todate);
            TemplateHandler.Assign(ref tpl, "UserName", username);
            string content = TemplateHandler.CopyBlock(tpl, "Content");

            string row = "";
            var htmlContent = new StringBuilder();

            foreach (DataRow dataRow in rows)
            {
                row = content;
                TemplateHandler.Assign(ref row, "Date", dataRow["rptmonth"] + "/" + dataRow["rptyear"]);
                TemplateHandler.Assign(ref row, "BetCount", NumberFormat.DecFormat(dataRow["BetCount"], 0));
                TemplateHandler.Assign(ref row, "WinCount", NumberFormat.DecFormat(dataRow["WinCount"], 0));
                TemplateHandler.Assign(ref row, "DrawCount", NumberFormat.DecFormat(dataRow["DrawCount"], 0));
                TemplateHandler.Assign(ref row, "WinLoss", negativenumber(dataRow["WinLostIBC"], ""));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIBC"]);
                total.Wincount += CastAide.AsDecimal(dataRow["Wincount"]);
                total.Drawcount += CastAide.AsDecimal(dataRow["Drawcount"]);
                total.BetCount += CastAide.AsDecimal(dataRow["BetCount"]);

                htmlContent.Append(row);
            }

            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "BetCount", NumberFormat.DecFormat(total.BetCount, 0));
            TemplateHandler.Assign(ref footer, "WinCount", NumberFormat.DecFormat(total.Wincount, 0));
            TemplateHandler.Assign(ref footer, "DrawCount", NumberFormat.DecFormat(total.Drawcount, 0));
            TemplateHandler.Assign(ref footer, "WinLoss", negativenumber(total.WinlostIBC, ""));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (rows.Length == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='5' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='5' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }

            string title = "Punter " + username + " by betcount from " + fromdate + " to " + todate;
            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            CacheData(keySelBetCountByCustomer, hashCache, WebSettings.CacheExpire);
            //Export2ExcelSelBetCountByCustomer(rows, title, ref fullpath);

            return tpl;
        }

        #endregion SelBetCountByCustomer

        #region SelSports

        public string SelSports(string fdate, string tdate, string sporttype)
        {
            var html = new StringBuilder();
            html.Append("<select id='ddlSports' name='ddlSports' onchange='LoadLeagues();'>");
            DataTable dataTable = IBCProvider.ReportProvider.PunterAnalysis().SelSports().ResultSet.Tables[0];
            DataRow[] dataRows = dataTable.Select();
            foreach (DataRow dataRow in dataRows)
            {
                html.Append("<option value='" + dataRow["sporttype"] + "'>" + dataRow["sportname"] + "</option>");
            }

            html.Append("</select>");

            var hashtable = new Hashtable();
            hashtable.Add("sports", html.ToString());
            hashtable.Add("leagues", SelLeagues(fdate, tdate, sporttype));

            return JsonUtil.ToJson(hashtable);
        }

        #endregion SelSports

        #region SelLeagues

        public string SelLeagues(string fdate, string tdate, string sporttype)
        {
            var html = new StringBuilder();
            var param = new Hashtable();
            param.Add("fromdate", fdate);
            param.Add("todate", tdate);
            param.Add("sporttype", sporttype);
            DataTable dataTable = IBCProvider.ReportProvider.PunterAnalysis().SelLeagues(param).ResultSet.Tables[0];
            DataRow[] dataRows = dataTable.Select();
            html.Append("<select id='ddlLeagues' name='ddlLeagues'>");

            foreach (DataRow dataRow in dataRows)
            {
                html.Append("<option value='" + dataRow["leagueid"] + "'>" + dataRow["leaguename"] + "</option>");
            }

            if (dataRows.Length == 0)
                html.Append("<option value='0'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</option>");

            html.Append("</select>");

            return html.ToString();
        }

        #endregion SelLeagues

        #region SelTopPunterSummaryByLeague

        private DataTable GetSelTopPunterSummaryByLeague(string leagueid, string leaguename, string fromdate,
                                                         string todate, string toprow, string orderby, string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();

            hash.Add("leagueid", leagueid);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("toprow", CastAide.AsInt32(toprow));
            hash.Add("orderby", CastAide.AsInt32(orderby));
            hash.Add("ordertype", CastAide.AsInt32(ordertype));
            var dt = new DataTable();
            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterSummaryByLeague(hash);
                dt = rs.ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelTopPunterSummaryByLeague(string leagueid, string leaguename, string fromdate, string todate,
                                                  string toprow, string orderby, string ordertype)
        {
            DataRow[] rows = { };
            DataTable dt = GetSelTopPunterSummaryByLeague(leagueid, leaguename, fromdate, todate, toprow, orderby,
                                                          ordertype);
            rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.UserName = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/TopPunterSummaryContent.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            TemplateHandler.Assign(ref content, "OrderBy", orderby);
            TemplateHandler.Assign(ref content, "OrderType", ordertype);
            TemplateHandler.Assign(ref content, "LeagueId", leagueid);
            string row = "";
            var htmlContent = new StringBuilder();

            foreach (DataRow dataRow in rows)
            {
                row = content;
                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "UserName", dataRow["username"]);
                TemplateHandler.Assign(ref row, "CustId", dataRow["custid"]);
                TemplateHandler.Assign(ref row, "fromdate", fromdate);
                TemplateHandler.Assign(ref row, "todate", todate);
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"]));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));
                TemplateHandler.Assign(ref row, "BetCount", NumberFormat.DecFormat(dataRow["Betcount"], 0));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIBC"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["TurnOver"]);
                total.BetCount += CastAide.AsDecimal(dataRow["BetCount"]);

                htmlContent.Append(row);
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "BetCount", NumberFormat.DecFormat(total.BetCount, 0));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (rows.Length == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='6' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='6' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";

                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Punter Summary by League: " + leaguename + " from " + fromdate + " to " + todate;
            //Export2ExcelSelTopPunterSummary(rows, title, ref fullpath, "Leagues");

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            CacheData(keySelTopPunterSummaryByLeague, hashCache, WebSettings.CacheExpire);

            var h = new Hashtable();
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterSummaryByLeague

        #region SelTopPunterByLeagueCustId

        private DataTable GetSelTopPunterByLeagueCustId(string custId, string leagueid, string username,
                                                        string sporttype, string fromdate, string todate, string orderby,
                                                        string ordertype)
        {
            Hashtable hash;
            hash = new Hashtable();
            hash.Add("custid", custId);
            hash.Add("leagueid", leagueid);
            hash.Add("fromdate", fromdate);
            hash.Add("todate", todate);
            hash.Add("orderby", orderby);
            hash.Add("ordertype", CastAide.AsInt32(ordertype));
            var dt = new DataTable();
            try
            {
                DbResult rs = IBCProvider.ReportProvider.PunterAnalysis().SelTopPunterByLeagueCustId(hash);
                dt = rs.ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                hashError = true;
                TemplateHandler.Assign(ref msg, "content", ex.Message);

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public string SelTopPunterByLeagueCustId(string custId, string leagueid, string username, string sporttype,
                                                 string fromdate, string todate, string orderby, string ordertype)
        {
            DataRow[] rows = { };
            DataTable dt = GetSelTopPunterByLeagueCustId(custId, leagueid, username, sporttype, fromdate, todate,
                                                         orderby, ordertype);
            rows = dt.Select();

            var total = new PunterAnalysisDetails();
            var lst = new List<PunterAnalysisDetails>();
            total.Sportmame = "Total";

            string tpl = WebUtils.LoadTemplate("Report/Punter/SelTopPunterByLeagueCustId.htm");
            string content = TemplateHandler.CopyBlock(tpl, "Content");
            string header = TemplateHandler.CopyBlock(tpl, "Header");
            TemplateHandler.RemoveBlock(ref tpl, "Header");
            string strtltie = "User: " + username + ", Sport: " + CacheRef.GetSportName(sporttype) + ", League: " +
                              CacheRef.GetLeagueName(leagueid);
            TemplateHandler.Assign(ref header, "UserName", strtltie);
            string row = "";
            var htmlContent = new StringBuilder();

            int count = 0;
            foreach (DataRow dataRow in rows)
            {
                row = content;
                TemplateHandler.Assign(ref row, "class_Total", "");
                TemplateHandler.Assign(ref row, "Bettype", CacheRef.GetBettype(dataRow["bettype"]));
                TemplateHandler.Assign(ref row, "WinlostIBC", negativenumber(dataRow["WinlostIbc"], ""));
                TemplateHandler.Assign(ref row, "TurnOver", NumberFormat.DecFormat(dataRow["Turnover"], 2));
                TemplateHandler.Assign(ref row, "Margin", negativenumber(CastAide.AsDecimal(dataRow["Margin"]) * 100, "%"));

                total.WinlostIBC += CastAide.AsDecimal(dataRow["WinlostIbc"]);
                total.TurnOver += CastAide.AsDecimal(dataRow["Turnover"]);

                if (CastAide.AsDecimal(dataRow["WinlostIbc"]) == 0 && CastAide.AsDecimal(dataRow["Turnover"]) == 0)
                    continue;

                htmlContent.Append(row);
                count++;
            }

            total.Margin =
                CastAide.AsDecimal(
                    NumberFormat.DecFormat((total.WinlostIBC / (total.TurnOver == 0 ? 1 : total.TurnOver)) * 100, 2));
            string footer = TemplateHandler.CopyBlock(tpl, "Footer");
            TemplateHandler.Assign(ref footer, "Bettype", "Total");
            TemplateHandler.Assign(ref footer, "WinlostIBC", negativenumber(total.WinlostIBC, ""));
            TemplateHandler.Assign(ref footer, "TurnOver", NumberFormat.DecFormat(total.TurnOver, 2));
            TemplateHandler.Assign(ref footer, "Margin", negativenumber(total.Margin, "%"));
            TemplateHandler.Assign(ref footer, "class_Total", "rpt-bold total");

            if (count == 0 || hashError)
            {
                string noinfo = "";
                if (hashError)
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + msg + "</td></tr>";
                else
                    noinfo = "<tr bgcolor='white'><td colspan='4' class='c'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") + "</td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "Content", noinfo);
                TemplateHandler.RemoveBlock(ref tpl, "Footer");
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref tpl, "Content", htmlContent.ToString());
                TemplateHandler.ReplaceBlock(ref tpl, "Footer", footer);
            }
            string fullpath = "";
            string title = "Punter summary by " + username + " - " + CacheRef.GetSportName(sporttype) + " - " +
                           CacheRef.GetLeagueName(leagueid) + " from " + fromdate + " to " + todate;

            var hashCache = new Hashtable();
            hashCache.Add("data", dt);
            hashCache.Add("title", title);
            CacheData(keySelTopPunterByLeagueCustId, hashCache, WebSettings.CacheExpire);

            //Export2ExcelSelTopPunterBy(rows, title, "PunterByLeage-" + username, "SelTopPunterByLeagueCustId", ref fullpath);

            var h = new Hashtable();
            h.Add("header", header);
            h.Add("data", tpl);
            h.Add("fullpath", fullpath);
            return JsonUtil.ToJson(h);
        }

        #endregion SelTopPunterByLeagueCustId

        #region Export2ExcelSelTopPunterSummary

        public void Export2ExcelSelTopPunterSummary(string username, string leagueid, string leaguename, string fromdate,
                                                    string todate, string toprow, string orderby, string ordertype,
                                                    string type)
        {
            string key = "";
            switch (type)
            {
                case "SelTopPunterSummary":
                    key = keySelTopPunterSummary;
                    break;

                case "SelTopPunterSummaryByLeagues":
                    key = keySelTopPunterSummaryByLeague;
                    break;
            }

            var hashCach = (Hashtable)GetCachedObjData(key);
            var dt = new DataTable();
            DataRow[] Rows = { };
            string title = "";
            if (hashCach != null)
            {
                dt = (DataTable)hashCach["data"];
                title = (string)hashCach["title"];
            }
            else
            {
                switch (type)
                {
                    case "SelTopPunterSummary":
                        dt = GetSelTopPunterSummary(username, fromdate, todate, toprow, orderby, ordertype);
                        title = "Punter Summary from " + fromdate + " to " + todate;
                        break;

                    case "SelTopPunterSummaryByLeagues":
                        dt = GetSelTopPunterSummaryByLeague(leagueid, leaguename, fromdate, todate, toprow, orderby,
                                                            ordertype);
                        title = "Punter Summary by League: " + leaguename + " from " + fromdate + " to " + todate;
                        break;
                }
            }

            Rows = dt.Select();

            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            string filename = type + "-" + DateTime.Today.ToShortDateString().Replace("/", "-");

            ExcelWorksheet ws = worksheets.Add(filename);
            var gem = new ExcelGemboxUtils();

            #region declare format cell

            //CellStyle celltitle = new CellStyle();
            // celltitle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            // celltitle.VerticalAlignment = VerticalAlignmentStyle.Center;
            // celltitle.FillPattern.SetSolid(Color.FromArgb(82, 106, 128));
            // celltitle.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // celltitle.Font.Weight = ExcelFont.BoldWeight;
            // celltitle.Font.Color = Color.White;
            // celltitle.WrapText = true;

            // CellStyle bgcolorText = new CellStyle();
            // CellStyle bgcolorNumber = new CellStyle();

            // CellStyle cellNumberNormalEven = new CellStyle();
            // cellNumberNormalEven.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNormalEven.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNormalEven.NumberFormat = "#,##0.00";
            // cellNumberNormalEven.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));

            // CellStyle cellTextNormalEven = new CellStyle();
            // cellTextNormalEven.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellTextNormalEven.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            // cellTextNormalEven.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellTextNormalEven.WrapText = true;
            // cellTextNormalEven.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));

            // CellStyle cellNumberNormalPercentEven = new CellStyle();
            // cellNumberNormalPercentEven.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNormalPercentEven.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNormalPercentEven.NumberFormat = "#,##0.00%_);(#,##0.00)%";

            // cellNumberNormalPercentEven.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));

            // CellStyle cellNumberNormal = new CellStyle();
            // cellNumberNormal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNormal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNormal.NumberFormat = "#,##0.00_);(#,##0.00)";

            // CellStyle cellNumberNegNormal = new CellStyle();
            // cellNumberNegNormal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNegNormal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNegNormal.Font.Color = Color.Red;
            // cellNumberNegNormal.NumberFormat = "#,##0.00_);(#,##0.00)";

            // CellStyle cellNumberNormalPercent = new CellStyle();
            // cellNumberNormalPercent.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNormalPercent.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNormalPercent.NumberFormat = "#,##0.00%";

            // CellStyle cellNumberNegNormalPercent = new CellStyle();
            // cellNumberNegNormalPercent.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNegNormalPercent.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNegNormalPercent.Font.Color = Color.Red;
            // cellNumberNegNormalPercent.NumberFormat = "#,##0.00%_);(#,##0.00)%";

            // CellStyle cellTextNormal = new CellStyle();
            // cellTextNormal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellTextNormal.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            // cellTextNormal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellTextNormal.WrapText = true;

            // CellStyle cellNumberTotal= new CellStyle();
            // cellNumberTotal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberTotal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberTotal.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            // cellNumberTotal.FillPattern.SetSolid(Color.FromArgb(218, 238, 243));
            // cellNumberTotal.Font.Weight = ExcelFont.BoldWeight;
            // cellNumberTotal.Font.Color = Color.Black;
            // cellNumberTotal.NumberFormat = "#,##0.00_);(#,##0.00)";

            // CellStyle cellNumberNegTotal = new CellStyle();
            // cellNumberNegTotal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNegTotal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNegTotal.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            // cellNumberNegTotal.FillPattern.SetSolid(Color.FromArgb(218, 238, 243));
            // cellNumberNegTotal.Font.Weight = ExcelFont.BoldWeight;
            // cellNumberNegTotal.Font.Color = Color.Red;
            // cellNumberNegTotal.NumberFormat = "#,##0.00_);(#,##0.00)";

            // CellStyle cellNumberTotalPercent = new CellStyle();
            // cellNumberTotalPercent.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberTotalPercent.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberTotalPercent.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            // cellNumberTotalPercent.FillPattern.SetSolid(Color.FromArgb(218, 238, 243));
            // cellNumberTotalPercent.Font.Weight = ExcelFont.BoldWeight;
            // cellNumberTotalPercent.Font.Color = Color.Black;
            // cellNumberTotalPercent.NumberFormat = "#,##0.00%_);(#,##0.00%)";

            // CellStyle cellNumberNegTotalPercent = new CellStyle();
            // cellNumberNegTotalPercent.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellNumberNegTotalPercent.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellNumberNegTotalPercent.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            // cellNumberNegTotalPercent.FillPattern.SetSolid(Color.FromArgb(218, 238, 243));
            // cellNumberNegTotalPercent.Font.Weight = ExcelFont.BoldWeight;
            // cellNumberNegTotalPercent.Font.Color = Color.Red;
            // cellNumberNegTotalPercent.NumberFormat = "#,##0.00%_);(#,##0.00)%";

            // CellStyle cellTextTotal = new CellStyle();
            // cellTextTotal.VerticalAlignment = VerticalAlignmentStyle.Center;
            // cellTextTotal.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            // cellTextTotal.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            // cellTextTotal.Font.Weight = ExcelFont.BoldWeight;
            // cellTextTotal.FillPattern.SetSolid(Color.FromArgb(218, 238, 243));
            // cellTextTotal.Font.Color = Color.Black;
            // cellTextTotal.WrapText = true;

            #endregion declare format cell

            #region columnwidth

            ws.Columns[0].Width = 20 * 256;
            ws.Columns[1].Width = 15 * 256;
            ws.Columns[2].Width = 15 * 256;
            ws.Columns[3].Width = 15 * 256;
            ws.Columns[4].Width = 15 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 4].Value = title;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Style = gem.GetStyleTitle();

            ws.Cells[1, 0].Value = "Username";
            ws.Cells[1, 1].Value = "Winlost";
            ws.Cells[1, 2].Value = "Turnover";
            ws.Cells[1, 3].Value = "Margin";
            ws.Cells[1, 4].Value = "BetCount";

            ws.Cells.GetSubrangeAbsolute(1, 0, 1, 4).Style = gem.GetStyleTitle();

            #endregion Header

            #region Content

            int i = 2;

            decimal winlost = 0, turnover = 0, margin = 0, betcount = 0;
            decimal totalwinlost = 0, totalturnover = 0, totalmargin = 0, totalbetcount = 0;

            bool IsEven = false;
            int index = 0;
            foreach (DataRow dr in Rows)
            {
                index++;

                IsEven = index % 2 == 0;

                winlost = CastAide.AsDecimal(dr["WinlostIbc"]);
                turnover = CastAide.AsDecimal(dr["turnover"]);
                margin = CastAide.AsDecimal(dr["margin"]);
                betcount = CastAide.AsDecimal(dr["betcount"]);

                totalwinlost += winlost;
                totalturnover += turnover;
                totalbetcount += betcount;

                ws.Cells[i, 0].Value = CastAide.AsString(dr["username"]);
                ws.Cells[i, 0].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 1].Value = winlost;
                ws.Cells[i, 1].Style = gem.GetStyleNumber(winlost, IsEven, "");

                ws.Cells[i, 2].Value = turnover;
                ws.Cells[i, 2].Style = gem.GetStyleNumber(turnover, IsEven, "");

                ws.Cells[i, 3].Value = margin;
                ws.Cells[i, 3].Style = gem.GetStyleNumber(margin, IsEven, "%");

                ws.Cells[i, 4].Value = betcount;
                ws.Cells[i, 4].Style = gem.GetStyleIntNumber(betcount, IsEven, "");

                i++;
            }

            totalmargin = (totalwinlost / ((totalturnover == 0) ? 1 : totalturnover));
            ws.Cells[i, 0].Value = "Total";
            ws.Cells[i, 0].Style = gem.GetStyleTextTotal();

            ws.Cells[i, 1].Value = totalwinlost;
            ws.Cells[i, 1].Style = gem.GetStyleNumberTotal(totalwinlost, "");

            ws.Cells[i, 2].Value = totalturnover;
            ws.Cells[i, 2].Style = gem.GetStyleNumberTotal(totalturnover, "");

            ws.Cells[i, 3].Value = totalmargin;
            ws.Cells[i, 3].Style = gem.GetStyleNumberTotal(totalmargin, "%");

            ws.Cells[i, 4].Value = totalbetcount;
            ws.Cells[i, 4].Style = gem.GetStyleIntNumberTotal(totalbetcount, "");

            base.excelFile = excelFile;
            ExportExcelFile(type + "-" + DateTime.Today.ToShortDateString().Replace("/", "-"));

            #endregion Content
        }

        #endregion Export2ExcelSelTopPunterSummary

        #region Export2ExcelSelTopPunterBy

        public void Export2ExcelSelTopPunterBy(string custId, string username, string leagueid, string sporttype,
                                               string bettype, string fromdate, string todate, string orderby,
                                               string ordertype, string type)
        {
            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            var gem = new ExcelGemboxUtils();
            string columnname = "Sporttype";

            var dt = new DataTable();
            DataRow[] Rows = { };
            string title = "", FileName = "";

            var hashCache = new Hashtable();

            switch (type)
            {
                case "SelTopPunterByCustId":
                    columnname = "Sporttype";
                    hashCache = (Hashtable)GetCachedObjData(keySelTopPunterByCustId);
                    if (hashCache != null)
                    {
                        dt = (DataTable)hashCache["data"];
                        title = (string)hashCache["title"];
                        FileName = (string)hashCache["filename"];
                    }
                    else
                    {
                        dt = GetSelTopPunterByCustId(custId, username, fromdate, todate, orderby, ordertype);

                        title = "Puner Summary by " + username + " from " + fromdate + " to " + todate;
                        FileName = "PunterByUserName-" + username;
                    }
                    break;

                case "SelTopPunterByLeagueCustId":
                    columnname = "Bettype";
                    hashCache = (Hashtable)GetCachedObjData(keySelTopPunterByLeagueCustId);
                    if (hashCache != null)
                    {
                        dt = (DataTable)hashCache["data"];
                        title = (string)hashCache["title"];
                    }
                    else
                    {
                        dt = GetSelTopPunterByLeagueCustId(custId, leagueid, username, sporttype, fromdate, todate,
                                                           orderby, ordertype);
                        title = "Punter summary by " + username + " - " + CacheRef.GetSportName(sporttype) + " - " +
                                CacheRef.GetLeagueName(leagueid) + " from " + fromdate + " to " + todate;
                    }
                    FileName = "PunterByUserName-" + username;
                    break;

                case "SelTopPunterByBettype":
                    columnname = "leagueid";
                    hashCache = (Hashtable)GetCachedObjData(keySelTopPunterByBettype);
                    if (hashCache != null)
                    {
                        dt = (DataTable)hashCache["data"];
                        title = (string)hashCache["title"];
                        FileName = (string)hashCache["filename"];
                    }
                    else
                    {
                        dt = GetSelTopPunterByBettype(custId, username, sporttype, bettype, fromdate, todate, orderby,
                                                      ordertype);

                        title = "Punter summary by " + username + " " + CacheRef.GetSportName(sporttype) + " - " +
                                CacheRef.GetBettype(bettype) + " from " + fromdate + " to " + todate;
                        FileName = "PunterByBettype-" + username;
                    }
                    break;
            }

            Rows = dt.Select();
            FileName = FileName.Replace("/", "");
            FileName = FileName + "-" + DateTime.Today.ToShortDateString().Replace("/", "-");
            ExcelWorksheet ws = worksheets.Add(FileName);

            #region columnwidth

            ws.Columns[0].Width = 20 * 256;
            ws.Columns[1].Width = 15 * 256;
            ws.Columns[2].Width = 15 * 256;
            ws.Columns[3].Width = 15 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 3].Value = title;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Style = gem.GetStyleTitle();
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Merged = true;

            if (columnname == "leagueid")
                ws.Cells[1, 0].Value = "LeagueName";
            else
                ws.Cells[1, 0].Value = columnname;
            ws.Cells[1, 1].Value = "Winlost";
            ws.Cells[1, 2].Value = "Turnover";
            ws.Cells[1, 3].Value = "Margin";

            ws.Cells.GetSubrangeAbsolute(1, 0, 1, 3).Style = gem.GetStyleTitle();

            #endregion Header

            #region Content

            int i = 2;

            decimal winlost = 0, turnover = 0, margin = 0;
            decimal totalwinlost = 0, totalturnover = 0, totalmargin = 0;

            bool IsEven = false;
            int index = 0;

            foreach (DataRow dr in Rows)
            {
                index++;

                IsEven = index % 2 == 0;

                winlost = CastAide.AsDecimal(dr["WinlostIbc"]);
                turnover = CastAide.AsDecimal(dr["turnover"]);
                margin = CastAide.AsDecimal(dr["margin"]);

                if (winlost == 0 && turnover == 0) continue;

                totalwinlost += winlost;
                totalturnover += turnover;

                switch (type)
                {
                    case "SelTopPunterByCustId":
                        if (dr[columnname].ToString() == "100")
                            ws.Cells[i, 0].Value = "Mix Parlay";
                        else
                            ws.Cells[i, 0].Value = CacheRef.GetSportName(CastAide.AsString(dr[columnname]));
                        break;

                    case "SelTopPunterBySporttype":
                    case "SelTopPunterByLeagueCustId":
                        ws.Cells[i, 0].Value = CacheRef.GetBettype(CastAide.AsString(dr[columnname]));
                        break;

                    case "SelTopPunterByBettype":
                        ws.Cells[i, 0].Value = CacheRef.GetLeagueName(CastAide.AsString(dr[columnname]));
                        break;
                }

                ws.Cells[i, 0].Style = gem.GetStyleText(IsEven);
                ws.Cells[i, 1].Value = winlost;
                ws.Cells[i, 1].Style = gem.GetStyleNumber(winlost, IsEven, "");

                ws.Cells[i, 2].Value = turnover;
                ws.Cells[i, 2].Style = gem.GetStyleNumber(turnover, IsEven, "");

                ws.Cells[i, 3].Value = margin;
                ws.Cells[i, 3].Style = gem.GetStyleNumber(margin, IsEven, "%");

                i++;
            }

            totalmargin = (totalwinlost / ((totalturnover == 0) ? 1 : totalturnover));
            ws.Cells[i, 0].Value = "Total";
            ws.Cells[i, 0].Style = gem.GetStyleTextTotal();

            ws.Cells[i, 1].Value = totalwinlost;
            ws.Cells[i, 1].Style = gem.GetStyleNumberTotal(totalwinlost, "");

            ws.Cells[i, 2].Value = totalturnover;
            ws.Cells[i, 2].Style = gem.GetStyleNumberTotal(totalturnover, "");

            ws.Cells[i, 3].Value = totalmargin;
            ws.Cells[i, 3].Style = gem.GetStyleNumberTotal(totalmargin, "%");

            base.excelFile = excelFile;
            ExportExcelFile(FileName);

            #endregion Content
        }

        #endregion Export2ExcelSelTopPunterBy

        #region Export2ExcelSelIPByCustomer

        public void Export2ExcelSelIPByCustomer(string custid, string username, string fromdate, string todate)
        {
            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;

            ExcelWorksheet ws = worksheets.Add(FileName);
            var gem = new ExcelGemboxUtils();

            var hashCache = new Hashtable();
            hashCache = (Hashtable)GetCachedObjData(keySelIpByCustomer);
            var dt = new DataTable();
            string title = "";
            if (hashCache != null)
            {
                dt = (DataTable)hashCache["data"];
                title = (string)hashCache["title"];
                FileName = (string)hashCache["filename"];
            }
            else
            {
                dt = GetSelIpByCustomer(custid, fromdate, todate);
                title = "Punter view IP by " + username + " from " + fromdate + " to " + todate;
                FileName = "PunterByIP-" + username;
            }
            DataRow[] Rows = dt.Select();
            FileName = FileName.Replace("/", "");
            FileName = FileName + "-" + DateTime.Today.ToShortDateString().Replace("/", "-");

            #region columnwidth

            ws.Columns[0].Width = 20 * 256;
            ws.Columns[1].Width = 15 * 256;
            ws.Columns[2].Width = 15 * 256;
            ws.Columns[3].Width = 15 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 3].Value = title;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Style = gem.GetStyleTitle();
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Merged = true;

            ws.Cells[1, 0].Value = "Month";
            ws.Cells[1, 1].Value = "Year";
            ws.Cells[1, 2].Value = "IP";
            ws.Cells[1, 3].Value = "Country";

            ws.Cells.GetSubrangeAbsolute(1, 0, 1, 3).Style = gem.GetStyleTitle();

            #endregion Header

            #region Content

            int i = 2;
            bool IsEven = false;
            foreach (DataRow dr in Rows)
            {
                IsEven = i % 2 == 0;
                ws.Cells[i, 0].Value = dr["rptmonth"];
                ws.Cells[i, 0].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 1].Value = dr["rptyear"];
                ws.Cells[i, 1].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 2].Value = dr["ip"];
                ws.Cells[i, 2].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 3].Value = dr["country"];
                ws.Cells[i, 3].Style = gem.GetStyleText(IsEven);

                i++;
            }

            base.excelFile = excelFile;
            ExportExcelFile(FileName);

            #endregion Content
        }

        #endregion Export2ExcelSelIPByCustomer

        #region Export2ExcelSelBetCountByCustomer

        public void Export2ExcelSelBetCountByCustomer(string custid, string username, string fromdate, string todate,
                                                      string leagueid)
        {
            var gem = new ExcelGemboxUtils();
            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;

            var hashCache = (Hashtable)GetCachedObjData(keySelBetCountByCustomer);
            var dt = new DataTable();
            DataRow[] Rows = { };
            string title = "";
            if (hashCache != null)
            {
                dt = (DataTable)hashCache["data"];
                title = (string)hashCache["title"];
            }
            else
            {
                dt = GetSelBetCountByCustomer(custid, username, fromdate, todate, leagueid);
                title = "Punter " + username + " by betcount from " + fromdate + " to " + todate;
            }

            Rows = dt.Select();

            string filename = "PunterByBetCount-" + DateTime.Today.ToShortDateString().Replace("/", "-");
            ExcelWorksheet ws = worksheets.Add(filename);

            #region columnwidth

            ws.Columns[0].Width = 20 * 256;
            ws.Columns[1].Width = 15 * 256;
            ws.Columns[2].Width = 15 * 256;
            ws.Columns[3].Width = 15 * 256;
            ws.Columns[4].Width = 15 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 4].Value = title;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Style = gem.GetStyleTitle();

            ws.Cells[1, 0].Value = "Date";
            ws.Cells[1, 1].Value = "Bet Count";
            ws.Cells[1, 2].Value = "Win Count";
            ws.Cells[1, 3].Value = "Draw Count";
            ws.Cells[1, 4].Value = "Winlost";

            ws.Cells.GetSubrangeAbsolute(1, 0, 1, 4).Style = gem.GetStyleTitle();

            #endregion Header

            #region Content

            int i = 2;

            decimal winlost = 0, wincount = 0, drawcount = 0, betcount = 0;
            decimal totalwinlost = 0, totalwincount = 0, totaldrawcount = 0, totalbetcount = 0;
            bool IsEven = false;
            int index = 0;
            foreach (DataRow dr in Rows)
            {
                index++;
                IsEven = index % 2 == 0;

                winlost = CastAide.AsDecimal(dr["WinlostIbc"]);
                wincount = CastAide.AsDecimal(dr["wincount"]);
                drawcount = CastAide.AsDecimal(dr["drawcount"]);
                betcount = CastAide.AsDecimal(dr["betcount"]);

                totalwinlost += winlost;
                totalwincount += wincount;
                totalbetcount += betcount;
                totaldrawcount += drawcount;

                ws.Cells[i, 0].Value = dr["rptmonth"] + "/" + dr["rptyear"];
                ws.Cells[i, 0].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 1].Value = betcount;
                ws.Cells[i, 1].Style = gem.GetStyleIntNumber(betcount, IsEven, "");

                ws.Cells[i, 2].Value = wincount;
                ws.Cells[i, 2].Style = gem.GetStyleIntNumber(wincount, IsEven, "");

                ws.Cells[i, 3].Value = drawcount;
                ws.Cells[i, 3].Style = gem.GetStyleIntNumber(drawcount, IsEven, "");

                ws.Cells[i, 4].Value = winlost;
                ws.Cells[i, 4].Style = gem.GetStyleNumber(winlost, IsEven, "");

                i++;
            }

            ws.Cells[i, 0].Value = "Total";
            ws.Cells[i, 0].Style = gem.GetStyleTextTotal();

            ws.Cells[i, 1].Value = totalbetcount;
            ws.Cells[i, 1].Style = gem.GetStyleIntNumberTotal(totalbetcount, "");

            ws.Cells[i, 2].Value = totalwincount;
            ws.Cells[i, 2].Style = gem.GetStyleIntNumberTotal(totalwincount, "");

            ws.Cells[i, 3].Value = totaldrawcount;
            ws.Cells[i, 3].Style = gem.GetStyleIntNumberTotal(totaldrawcount, "");

            ws.Cells[i, 4].Value = totalwinlost;
            ws.Cells[i, 4].Style = gem.GetStyleNumberTotal(totalwinlost, "");

            base.excelFile = excelFile;
            ExportExcelFile(filename);

            #endregion Content
        }

        #endregion Export2ExcelSelBetCountByCustomer

        #region Export2ExcelSelTopPunterBySport

        public void Export2ExcelSelTopPunterBySport(string custId, string username, string sporttype, string fromdate,
                                                    string todate, string orderby, string ordertype)
        {
            var dt = new DataTable();
            DataRow[] Rows = { };
            string title = "", FileName = "";
            var hashCache = (Hashtable)GetCachedObjData(keySelTopPunterBySporttype);
            if (hashCache != null)
            {
                dt = (DataTable)hashCache["data"];
                title = (string)hashCache["title"];
                FileName = (string)hashCache["filename"];
            }
            else
            {
                dt = GetSelTopPunterBySporttype(custId, username, sporttype, fromdate, todate, orderby, ordertype);
                title = "Punter summary by " + username + " " + CacheRef.GetSportName(sporttype) + " from " + fromdate +
                        " to " + todate;
                FileName = "PunterBySportType-" + username;
            }

            Rows = dt.Select();

            var excelFile = new ExcelFile();
            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            string columnname = "Sporttype";
            FileName = FileName.Replace("/", "");

            FileName = FileName + "-" + DateTime.Today.ToShortDateString().Replace("/", "-");
            ExcelWorksheet ws = worksheets.Add(FileName);
            var gem = new ExcelGemboxUtils();

            #region columnwidth

            ws.Columns[0].Width = 15 * 256;
            ws.Columns[1].Width = 20 * 256;
            ws.Columns[2].Width = 15 * 256;
            ws.Columns[3].Width = 15 * 256;
            ws.Columns[4].Width = 15 * 256;

            #endregion columnwidth

            #region Header

            ws.Cells[0, 4].Value = title;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Style = gem.GetStyleTitle();
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;

            ws.Cells[1, 0].Value = "Sporttype";
            ws.Cells[1, 1].Value = "Bettype";
            ws.Cells[1, 2].Value = "Winlost";
            ws.Cells[1, 3].Value = "Turnover";
            ws.Cells[1, 4].Value = "Margin";

            ws.Cells.GetSubrangeAbsolute(1, 0, 1, 4).Style = gem.GetStyleTitle();

            #endregion Header

            #region Content

            int i = 2;

            decimal winlost = 0, turnover = 0, margin = 0;
            decimal totalwinlost = 0, totalturnover = 0, totalmargin = 0;

            int sport_p = 0, sport = 0;
            bool IsEven = false;
            int index = 0;
            foreach (DataRow dr in Rows)
            {
                index++;

                IsEven = index % 2 == 0;

                winlost = CastAide.AsDecimal(dr["WinlostIbc"]);
                turnover = CastAide.AsDecimal(dr["turnover"]);
                margin = CastAide.AsDecimal(dr["margin"]);

                sport_p = CastAide.AsInt32(dr["sporttype"]);

                if (sport != sport_p)
                {
                    ws.Cells[i, 0].Value = CacheRef.GetSportName(dr["sporttype"]);
                    ws.Cells[i, 0].Style = gem.GetStyleText(IsEven);
                    sport = sport_p;
                    int rowspan = 1;
                    string exp = "sporttype =" + sport;
                    rowspan = dt.Select(exp, "").Length;

                    ws.Cells.GetSubrangeAbsolute(i, 0, i + rowspan - 1, 0).Merged = true;
                }

                if (winlost == 0 && turnover == 0) continue;

                totalwinlost += winlost;
                totalturnover += turnover;

                ws.Cells[i, 1].Value = CacheRef.GetBettype(dr["bettype"]);
                ws.Cells[i, 1].Style = gem.GetStyleText(IsEven);

                ws.Cells[i, 2].Value = winlost;
                ws.Cells[i, 2].Style = gem.GetStyleNumber(winlost, IsEven, "");

                ws.Cells[i, 3].Value = turnover;
                ws.Cells[i, 3].Style = gem.GetStyleNumber(turnover, IsEven, "");

                ws.Cells[i, 4].Value = margin;
                ws.Cells[i, 4].Style = gem.GetStyleNumber(margin, IsEven, "%");

                i++;
            }

            totalmargin = (totalwinlost / ((totalturnover == 0) ? 1 : totalturnover));

            ws.Cells.GetSubrangeAbsolute(i, 0, i, 1).Merged = true;
            ws.Cells[i, 0].Value = "Total";
            ws.Cells[i, 0].Style = gem.GetStyleTextTotal();

            ws.Cells[i, 2].Value = totalwinlost;
            ws.Cells[i, 2].Style = gem.GetStyleNumberTotal(totalwinlost, "");

            ws.Cells[i, 3].Value = totalturnover;
            ws.Cells[i, 3].Style = gem.GetStyleNumberTotal(totalturnover, "");

            ws.Cells[i, 4].Value = totalmargin;
            ws.Cells[i, 4].Style = gem.GetStyleNumberTotal(totalmargin, "%");

            base.excelFile = excelFile;
            ExportExcelFile(FileName);

            #endregion Content
        }

        #endregion Export2ExcelSelTopPunterBySport
    }
}