using System;
using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for LeagueWLBettype
/// </summary>
namespace BLL.Report.LeagueWL
{
    public class LeagueWLBettype : ReportBase
    {
        public LeagueWLBettype()
        {
            KeyCaching = "accreportleaguewinlostbettype." + AppSession.Username;
        }

        private string tplHeader = "", tplBody = "", tplFooter = "";

        #region RenderHTML

        public override string RenderHTML(Hashtable input)
        {
            StringBuilder html = new StringBuilder();

            try
            {
                int sporttype = CastAide.AsInt32(input["sporttype"]);
                int leagueid = CastAide.AsInt32(input["leagueid"]);
                DateTime wldate = CastAide.AsDateTime(input["wldate"]);
                string db = CastAide.AsString(input["db"], "rep");

                tplHeader = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Bettype_Header.htm");
                tplBody = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Bettype_Body.htm");
                tplFooter = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Bettype_Footer.htm");

                string tplHeaderLive = tplHeader;
                string tplHeaderNoneLine = tplHeader;
                string evt = "", title = "";

                title = "";
                title = (CacheRef.GetLeagueName(leagueid).Contains(CacheRef.GetSportName(sporttype)))
                    ? CacheRef.GetLeagueName(leagueid)
                    : CacheRef.GetSportName(sporttype) + " - " + CacheRef.GetLeagueName(leagueid);
                TemplateHandler.Assign(ref tplHeaderLive, "title", title + " - LIVE");
                TemplateHandler.Assign(ref tplHeaderNoneLine, "title", title + " - NON LIVE");

                DbResult rs;
                DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
                if (wldate <= movebetsday)
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Bettype14(wldate, sporttype, leagueid);
                //rs = SiteProvider.Report.LeagueWL().LeagueWL_Bettype(wldate, sporttype, leagueid);
                else
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Bettype(wldate, sporttype, leagueid);

                ReportBean bean = new ReportBean(rs);

                DataRow[] drRows1 = bean.lsRows[0];
                DataRow[] drRows2 = bean.lsRows[1];//WinLost_8RM

                decimal WinLost_8RM_Live = 0, WinLost_8RM_NoneLive = 0;

                foreach (DataRow dr in drRows2)
                {
                    if (CastAide.AsInt32(dr["liveindicator"]) == 1)
                        WinLost_8RM_Live = CastAide.AsDecimal(dr["winlost8rm"]);
                    if (CastAide.AsInt32(dr["liveindicator"]) == 0)
                        WinLost_8RM_NoneLive = CastAide.AsDecimal(dr["winlost8rm"]);
                }

                LeagueWLRow GrandTotal = new LeagueWLRow();
                LeagueWLRow TotalLive = new LeagueWLRow();
                LeagueWLRow TotalNoneLive = new LeagueWLRow();

                StringBuilder htmlLive = new StringBuilder();
                StringBuilder htmlNoneLive = new StringBuilder();

                bool IsLiveHasData = false, IsNoneLineHasData = false;

                string strRow = "";
                htmlLive.Append(tplHeaderLive);
                htmlNoneLive.Append(tplHeaderNoneLine);

                foreach (DataRow dr in drRows1)
                {
                    strRow = tplBody;

                    LeagueWLRow row = new LeagueWLRow();

                    #region map data from Datarow into object

                    row.liveindicator = CastAide.AsBoolean(dr["liveindicator"]);
                    row.Bettype = CastAide.AsInt32(dr["bettype"]);
                    row.isHedge = CastAide.AsString(dr["isHedge"]);
                    row.WinLost = CastAide.AsDecimal(dr["Winlost"]);
                    row.Position = CastAide.AsDecimal(dr["Position"]);
                    row.Commission = CastAide.AsDecimal(dr["Commission"]);
                    row.LineTotal = CastAide.AsDecimal(dr["LineTotal"]);
                    row.Turnover = CastAide.AsDecimal(dr["Turnover"]);

                    #endregion map data from Datarow into object

                    row.Header = CacheRef.GetBettype(row.Bettype);
                    row.Header += " " + row.isHedge;

                    //leagueid, sporttype, bettype, liveindicator, IsHedge, db, wldate
                    evt = "onclick=\"GetLeagueWLDetails('" + leagueid + "','" + sporttype + "','" + row.Bettype + "','" + CastAide.AsInt32(row.liveindicator) + "','" + ((row.isHedge.Length > 0) ? 1 : 0) + "','" + db + "','" + wldate.ToShortDateString() + "');\"";

                    TemplateHandler.Assign(ref strRow, "bold", "");
                    TemplateHandler.Assign(ref strRow, "link", "rpt-link");
                    TemplateHandler.Assign(ref strRow, "evt", evt);

                    if (row.liveindicator)
                    {
                        AssignRow(ref strRow, row, ref TotalLive);
                        htmlLive.Append(strRow);
                        IsLiveHasData = true;
                    }
                    else
                    {
                        AssignRow(ref strRow, row, ref TotalNoneLive);
                        htmlNoneLive.Append(strRow);
                        IsNoneLineHasData = true;
                    }
                }

                if (IsLiveHasData)
                {
                    strRow = tplFooter;
                    TotalLive.Header = "All Total";
                    GrandTotal = TotalLive;
                    TemplateHandler.Assign(ref strRow, "bold", "rpt-bold");

                    if (WinLost_8RM_Live > 0)
                        TemplateHandler.Assign(ref strRow, "color-8rm", "rpt-red-color");
                    else
                        TemplateHandler.Assign(ref strRow, "color-8rm", "");

                    if (TotalLive.LineTotal + WinLost_8RM_Live > 0)
                        TemplateHandler.Assign(ref strRow, "color-grand", "rpt-red-color");
                    else
                        TemplateHandler.Assign(ref strRow, "color-grand", "");

                    TemplateHandler.Assign(ref strRow, "8rm", NumberFormat.DecFormat(WinLost_8RM_Live, 2));
                    TemplateHandler.Assign(ref strRow, "total", NumberFormat.DecFormat(TotalLive.LineTotal + WinLost_8RM_Live, 2));
                    AssignRow(ref strRow, TotalLive, ref GrandTotal);
                    htmlLive.Append(strRow);

                    html.Append(htmlLive.ToString());
                }
                if (IsNoneLineHasData)
                {
                    if (IsLiveHasData) html.Append("<p></p><br/>");
                    strRow = tplFooter;
                    TotalLive.Header = "All Total";
                    TemplateHandler.Assign(ref strRow, "bold", "rpt-bold");

                    if (WinLost_8RM_NoneLive > 0)
                        TemplateHandler.Assign(ref strRow, "color-8rm", "rpt-red-color");
                    else
                        TemplateHandler.Assign(ref strRow, "color-8rm", "");

                    if (TotalNoneLive.LineTotal + WinLost_8RM_NoneLive > 0)
                        TemplateHandler.Assign(ref strRow, "color-grand", "rpt-red-color");
                    else
                        TemplateHandler.Assign(ref strRow, "color-grand", "");

                    TemplateHandler.Assign(ref strRow, "8rm", NumberFormat.DecFormat(WinLost_8RM_NoneLive, 2));
                    TemplateHandler.Assign(ref strRow, "total", NumberFormat.DecFormat(TotalNoneLive.LineTotal + WinLost_8RM_NoneLive, 2));
                    AssignRow(ref strRow, TotalNoneLive, ref GrandTotal);
                    htmlNoneLive.Append(strRow);

                    html.Append(htmlNoneLive.ToString());
                }

                if (!IsLiveHasData && !IsNoneLineHasData)
                {
                    html.Append(htmlLive.ToString());
                    html.Append("<tr><td bgcolor='white' colspan='7'>There is no information</td></tr>");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                html = new StringBuilder();
                html.Append(tplHeader);
                html.Append("<tr><td bgcolor='white' class='rpt-red-color rpt-bold' colspan='7'>" + ex.Message + "</td></tr>");
                html.Append("</table>");
            }

            SetCacheReport(KeyCaching, html.ToString(), DateTime.Now.AddMinutes(30));

            return html.ToString();
        }

        #endregion RenderHTML

        #region AssignRow

        private void AssignRow(ref string strRow, LeagueWLRow row, ref LeagueWLRow Total)
        {
            TemplateHandler.Assign(ref strRow, "Bettype", row.Header);

            TemplateHandler.Assign(ref strRow, "Winloss", NumberFormat.DecFormat(row.WinLost, 2));
            TemplateHandler.Assign(ref strRow, "Position", NumberFormat.DecFormat(row.Position, 2));
            TemplateHandler.Assign(ref strRow, "Comm", NumberFormat.DecFormat(row.Commission, 2));
            TemplateHandler.Assign(ref strRow, "LineTotal", NumberFormat.DecFormat(row.LineTotal, 2));
            TemplateHandler.Assign(ref strRow, "Turnover", NumberFormat.DecFormat(row.Turnover, 2));
            TemplateHandler.Assign(ref strRow, "Margin", NumberFormat.DecFormat(row.Margin, 2) + "%");

            if (row.WinLost > 0) TemplateHandler.Assign(ref strRow, "color-wl", "rpt-red-color");
            else TemplateHandler.Assign(ref strRow, "color-wl", "");

            if (row.LineTotal > 0) TemplateHandler.Assign(ref strRow, "color-line", "rpt-red-color");
            else TemplateHandler.Assign(ref strRow, "color-line", "");

            if (row.Margin > 0) TemplateHandler.Assign(ref strRow, "color-margin", "rpt-red-color");
            else TemplateHandler.Assign(ref strRow, "color-margin", "");

            Total.WinLost += row.WinLost;
            Total.Position += row.Position;
            Total.Commission += row.Commission;
            Total.LineTotal += row.LineTotal;
            Total.Turnover += row.Turnover;
        }

        #endregion AssignRow

        #region Export2Excel

        public override void Export2Excel(Hashtable input)
        {
            string wldate = CastAide.AsString(input["wldate"]);
            input.Add(FileName, "LeagueWLBettype_" + wldate);

            if (GetCachedReport(KeyCaching) == null)
            {
                ExcelContent = RenderHTML(input).Replace("{border}", "1");
                base.Export2Excel(input);
            }
            else
            {
                ExcelContent = GetCachedReport(KeyCaching).Replace("{border}", "1"); ;
                base.Export2Excel(input);
            }
        }

        #endregion Export2Excel
    }
}