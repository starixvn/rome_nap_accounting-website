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
    public class LeagueWLDetails : ReportBase
    {
        private string tplBody = "", tplFooter = "";
        private string tplHeader = "";

        #region RenderHTML

        public override string RenderHTML(Hashtable input)
        {
            var html = new StringBuilder();

            tplHeader = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Details_Header.htm");
            tplBody = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Details_Body.htm");
            tplFooter = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Details_Footer.htm");

            try
            {
                int sporttype = CastAide.AsInt32(input["sporttype"]);
                int leagueid = CastAide.AsInt32(input["leagueid"]);
                DateTime wldate = CastAide.AsDateTime(input["wldate"]);
                int bettype = CastAide.AsInt32(input["bettype"]);
                bool liveindicator = CastAide.AsString(input["liveindicator"]) == "1";
                bool IsHedge = CastAide.AsString(input["IsHedge"]) == "1";
                string db = CastAide.AsString(input["db"], "rep");
                decimal WinLost_8RM = 0;
                string title = "";
                title = (CacheRef.GetLeagueName(leagueid).Contains(CacheRef.GetSportName(sporttype)))
                            ? CacheRef.GetLeagueName(leagueid)
                            : CacheRef.GetSportName(sporttype) + " - " + CacheRef.GetLeagueName(leagueid);

                title += liveindicator
                             ? " - LIVE - " + CacheRef.GetBettype(bettype)
                             : " - NONE LIVE- " + CacheRef.GetBettype(bettype);
                TemplateHandler.Assign(ref tplHeader, "title", title);

                DbResult rs;
                DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
                if (wldate <= movebetsday)
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Details14(wldate, sporttype, leagueid, bettype,
                                                                           liveindicator, IsHedge);
                //rs = SiteProvider.Report.LeagueWL().LeagueWL_Details(wldate, sporttype, leagueid, bettype, liveindicator, IsHedge);
                else
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Details(wldate, sporttype, leagueid, bettype,
                                                                         liveindicator, IsHedge);

                var bean = new ReportBean(rs);

                DataRow[] drRows = bean.lsRows[0];
                WinLost_8RM = CastAide.AsDecimal(bean.Outputs["winlost_8rm"]);

                var GrandTotal = new LeagueWLRow();

                string strRow = "";
                html.Append(tplHeader);
                string strHeader = "";
                int count = 0;
                foreach (DataRow dr in drRows)
                {
                    count++;
                    strRow = tplBody;

                    #region Mapping Data

                    var row = new LeagueWLRow();
                    row.matchcode = CastAide.AsString(dr["matchcode"]);
                    row.HomeId = CastAide.AsInt32(dr["HomeId"]);
                    row.AwayId = CastAide.AsInt32(dr["AwayId"]);
                    row.liveindicator = CastAide.AsBoolean(dr["liveindicator"]);
                    row.Bettype = CastAide.AsInt32(dr["Bettype"]);
                    row.leagueid = CastAide.AsInt32(dr["leagueid"]);
                    row.WinLost = CastAide.AsDecimal(dr["WinLost"]);
                    row.Position = CastAide.AsDecimal(dr["Position"]);
                    row.Commission = CastAide.AsDecimal(dr["Commission"]);
                    row.Turnover = CastAide.AsDecimal(dr["Turnover"]);
                    row.LineTotal = CastAide.AsDecimal(dr["LineTotal"]);
                    row.IsNeutral = CastAide.AsBoolean(dr["IsNeutral"]);

                    if (row.WinLost == 0 && row.Position == 0 && row.Commission == 0 && row.LineTotal == 0 &&
                        row.Turnover == 0)
                        continue;

                    #endregion Mapping Data

                    strHeader = CacheRef.GetBettype(row.Bettype);
                    strHeader += " " + ((row.IsNeutral) ? "(N)" : "");

                    TemplateHandler.Assign(ref strRow, "bold", "");
                    AssignRow(ref strRow, row, strHeader, ref GrandTotal);
                    html.Append(strRow);
                }

                strRow = tplFooter;
                strHeader = "All Total";
                TemplateHandler.Assign(ref strRow, "bold", "rpt-bold");
                if (WinLost_8RM > 0)
                    TemplateHandler.Assign(ref strRow, "color-8rm", "rpt-red-color");
                else
                    TemplateHandler.Assign(ref strRow, "color-8rm", "");

                if (GrandTotal.LineTotal + WinLost_8RM > 0)
                    TemplateHandler.Assign(ref strRow, "color-grand", "rpt-red-color");
                else
                    TemplateHandler.Assign(ref strRow, "color-grand", "");

                TemplateHandler.Assign(ref strRow, "8rm", NumberFormat.DecFormat(WinLost_8RM, 2));
                TemplateHandler.Assign(ref strRow, "total",
                                       NumberFormat.DecFormat(GrandTotal.LineTotal + WinLost_8RM, 2));
                AssignRow(ref strRow, GrandTotal, strHeader, ref GrandTotal);

                if (count == 0)
                {
                    html.Append("<tr><td bgcolor='white'  colspan='8'>There is no information</td></tr>");
                }
                else
                {
                    html.Append(strRow);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

                html = new StringBuilder();
                html.Append(tplHeader);
                html.Append("<tr><td bgcolor='white' class='rpt-red-color rpt-bold' colspan='8'>" + ex.Message +
                            "</td></tr>");
                html.Append("</table>");
            }

            SetCacheReport(KeyCaching, html.ToString(), DateTime.Now.AddMinutes(30));

            return html.ToString();
        }

        #endregion RenderHTML

        #region AssignRow

        private void AssignRow(ref string strRow, LeagueWLRow row, string strHeader, ref LeagueWLRow Total)
        {
            TemplateHandler.Assign(ref strRow, "code", row.matchcode);
            TemplateHandler.Assign(ref strRow, "homeid", CacheRef.GetTeamName(row.HomeId));
            TemplateHandler.Assign(ref strRow, "awayid", CacheRef.GetTeamName(row.AwayId));

            if (row.Bettype == 10)
                strRow = strRow.Replace("-vs-", string.Empty);

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
            input.Add(FileName, "LeagueWLDetails_" + wldate);

            if (GetCachedReport(KeyCaching) == null)
            {
                ExcelContent = RenderHTML(input).Replace("{border}", "1");
                TemplateHandler.RemoveBlock(ref ExcelContent, "excel");
                base.Export2Excel(input);
            }
            else
            {
                ExcelContent = GetCachedReport(KeyCaching).Replace("{border}", "1");
                ;
                TemplateHandler.RemoveBlock(ref ExcelContent, "excel");
                base.Export2Excel(input);
            }
        }

        #endregion Export2Excel

        public LeagueWLDetails()
        {
            KeyCaching = "accreportleaguewinlostdetails." + AppSession.Username;
        }
    }
}