using System;
using System.Collections;
using System.Data;
using System.Text;

using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for LeagueWLSporttype
/// </summary>

namespace BLL.Report.LeagueWL
{
    public class LeagueWLSporttype : ReportBase
    {
        private string tplBody = "", tplFooter = "";
        private string tplHeader = "";

        #region RenderHTML

        public override string RenderHTML(Hashtable hash)
        {
            var html = new StringBuilder();
            try
            {
                #region Declare variable

                DateTime wldate = CastAide.AsDateTime(hash["wldate"]);
                string db = CastAide.AsString(hash["db"]);
                decimal WinLost8RM;

                db = (string.IsNullOrEmpty(db)) ? "rep" : db;

                DbResult rs = null;

                tplHeader = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Sportype_Header.htm");
                tplBody = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Sportype_Body.htm");
                tplFooter = WebUtils.LoadTemplate("WinLost/LeagueWL/LeagueWL_Sportype_Footer.htm");

                DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
                if (wldate <= movebetsday)
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Sporttype14(wldate);
                //rs = SiteProvider.Report.LeagueWL().LeagueWL_Sporttype(wldate);
                else
                    rs = SiteProvider.Report.LeagueWL().LeagueWL_Sporttype(wldate);

                WinLost8RM = CastAide.AsDecimal(rs.Outputs["winlost_8rm"]);

                DataTable dtWL = rs.ResultSet.Tables[0];

                dtWL.Columns.Add("LeagueName", typeof(string));
                DataRow[] dtRowsLeague = dtWL.Select();
                foreach (DataRow dr in dtRowsLeague)
                {
                    dr["LeagueName"] = CacheRef.GetLeagueName(dr["LeagueID"]);
                }

                DataView dv = dtWL.DefaultView;
                dv.Sort = " sporttype asc, LeagueName asc";
                DataTable dt = dv.ToTable();

                DataRow[] dtRows = dt.Select();

                int Sporttype = 0, Sporttype_P = 0, Index = 0;
                bool isNoInfo = true;
                var SportTotal = new LeagueWLRow();
                var SportGrandTotal = new LeagueWLRow();

                html.Append(tplHeader);

                string strColspan = "<tr bgcolor='white'><td colspan='7'></td></tr>";
                string strHeaderSport =
                    "<tr class='limit'><td colspan='7' style='text-align:left;' class='rpt-header'><b>{sportname}</b></td></tr>";
                string strRow = "", strHeader = "";

                #endregion Declare variable

                #region foreach

                foreach (DataRow dr in dtRows)
                {
                    var row = new LeagueWLRow();

                    #region Mapping datarow into object LeagueWLRow

                    row.sporttype = CastAide.AsInt32(dr["sporttype"]);
                    row.leagueid = CastAide.AsInt32(dr["leagueid"]);

                    row.WinLost = CastAide.AsDecimal(dr["WinLost"]);
                    row.Position = CastAide.AsDecimal(dr["Position"]);
                    row.Commission = CastAide.AsDecimal(dr["Commission"]);
                    row.LineTotal = CastAide.AsDecimal(dr["LineTotal"]);
                    row.Turnover = CastAide.AsDecimal(dr["Turnover"]);

                    if (row.WinLost == 0 && row.Position == 0 && row.Commission == 0 &&
                        row.LineTotal == 0 & row.Turnover == 0)
                        continue;

                    #endregion Mapping datarow into object LeagueWLRow

                    #region Calculate and add row total at each Sport

                    strRow = tplBody;
                    Sporttype_P = row.sporttype;
                    if (Sporttype_P != Sporttype)
                    {
                        if (Index > 0)
                        {
                            if (Sporttype == 100)
                                SportTotal.Header = "Mix Parlay Total";
                            else
                                SportTotal.Header = CacheRef.GetSportName(Sporttype) + " Total";
                            AssignRow(ref strRow, SportTotal, true);
                            TemplateHandler.Assign(ref strRow, "sport", "sportLeg");
                            html.Append(strRow);
                            html.Append(strColspan);

                            strRow = tplBody;
                        }

                        if (Sporttype_P == 100)
                        {
                            Sporttype = Sporttype_P;
                            SportTotal = new LeagueWLRow();
                            SportTotal = row;

                            #region Calculate grand total

                            SportGrandTotal.WinLost += row.WinLost;
                            SportGrandTotal.Position += row.Position;
                            SportGrandTotal.Commission += row.Commission;
                            SportGrandTotal.LineTotal += row.LineTotal;
                            SportGrandTotal.Turnover += row.Turnover;

                            #endregion Calculate grand total

                            continue;
                        }

                        strHeader = strHeaderSport;
                        TemplateHandler.Assign(ref strHeader, "sportname", CacheRef.GetSportName(Sporttype_P));
                        html.Append(strHeader);

                        Sporttype = Sporttype_P;
                        Index++;
                        SportTotal = new LeagueWLRow();
                    }

                    #endregion Calculate and add row total at each Sport

                    #region Add row

                    AssignRow(ref strRow, row, false);
                    TemplateHandler.Assign(ref strRow, "sport", "");

                    if (
                        !(row.WinLost == 0 && row.Position == 0 && row.Commission == 0 && row.LineTotal == 0 &&
                          row.Turnover == 0))
                    {
                        html.Append(strRow);
                        isNoInfo = false;
                    }

                    #endregion Add row

                    #region Calculate total on each sport

                    SportTotal.WinLost += row.WinLost;
                    SportTotal.Position += row.Position;
                    SportTotal.Commission += row.Commission;
                    SportTotal.LineTotal += row.LineTotal;
                    SportTotal.Turnover += row.Turnover;

                    #endregion Calculate total on each sport

                    #region Calculate grand total

                    SportGrandTotal.WinLost += row.WinLost;
                    SportGrandTotal.Position += row.Position;
                    SportGrandTotal.Commission += row.Commission;
                    SportGrandTotal.LineTotal += row.LineTotal;
                    SportGrandTotal.Turnover += row.Turnover;

                    #endregion Calculate grand total
                }

                #endregion foreach

                //if (isNoInfo)
                //{
                //    html.Append(AccCommon.GenNoInformation("8") + "</table>");
                //    return html.ToString();
                //}

                #region Total of Sporttype

                if (Sporttype == 100)
                    SportTotal.Header = "Mix Parlay Total";
                else
                    SportTotal.Header = CacheRef.GetSportName(Sporttype) + " Total";

                strRow = tplBody;
                AssignRow(ref strRow, SportTotal, true);
                TemplateHandler.Assign(ref strRow, "sport", "sportLeg");
                html.Append(strColspan);

                if (
                    !(SportTotal.WinLost == 0 && SportTotal.Position == 0 && SportTotal.Commission == 0 &&
                      SportTotal.LineTotal == 0 && SportTotal.Turnover == 0))
                    html.Append(strRow);

                #endregion Total of Sporttype

                #region Grand Total of All Sporttypes

                html.Append(strColspan);
                SportGrandTotal.Header = "Grand Total ";

                strRow = tplFooter;
                AssignRow(ref strRow, SportGrandTotal, true);
                SportGrandTotal.LineTotal += WinLost8RM;
                if (WinLost8RM > 0)
                    TemplateHandler.Assign(ref strRow, "color-8rm", "rpt-red-color");
                else
                    TemplateHandler.Assign(ref strRow, "color-8rm", "");

                if (SportGrandTotal.LineTotal > 0)
                    TemplateHandler.Assign(ref strRow, "color-grand", "rpt-red-color");
                else
                    TemplateHandler.Assign(ref strRow, "color-grand", "");
                TemplateHandler.Assign(ref strRow, "8rm", NumberFormat.DecFormat(WinLost8RM, 2));
                TemplateHandler.Assign(ref strRow, "grandtotal", NumberFormat.DecFormat(SportGrandTotal.LineTotal, 2));
                html.Append(strRow + "</table>");

                #endregion Grand Total of All Sporttypes

                SetCacheReport(KeyCaching, html.ToString(), DateTime.Now.AddMinutes(30));
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                html = new StringBuilder();
                html.Append(tplHeader);
                html.Append("<tr><td bgcolor='white' class='rpt-red-color rpt-bold' colspan='7'>" + ex.Message +
                            "</td></tr>");
                html.Append("</table>");
            }
            return html.ToString();
        }

        #endregion RenderHTML

        #region AssignRow

        private void AssignRow(ref string strRow, LeagueWLRow row, bool IsSetHeader)
        {
            if (IsSetHeader)
            {
                TemplateHandler.Assign(ref strRow, "League", row.Header);
                TemplateHandler.Assign(ref strRow, "link", "");
                TemplateHandler.Assign(ref strRow, "evt", "");
            }
            else
            {
                TemplateHandler.Assign(ref strRow, "League", CacheRef.GetLeagueName(row.leagueid));
                TemplateHandler.Assign(ref strRow, "link", "rpt-link");
                string evt = "onclick=\" GetLeagueWLBettype('{0}','{1}','{2}'); \"";
                // {0}: sporttype, {1}:leagueid, {2} sport and league name

                string name = "";
                name = (CacheRef.GetLeagueName(row.leagueid).Contains(CacheRef.GetSportName(row.sporttype)))
                           ? CacheRef.GetLeagueName(row.leagueid)
                           : CacheRef.GetSportName(row.sporttype) + " - " + CacheRef.GetLeagueName(row.leagueid);

                evt = string.Format(evt, row.sporttype, row.leagueid, name);
                TemplateHandler.Assign(ref strRow, "evt", evt);
            }
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
        }

        #endregion AssignRow

        #region Export2Excel

        public override void Export2Excel(Hashtable input)
        {
            string wldate = CastAide.AsString(input["wldate"]);
            input.Add(FileName, "LeagueWinlost_" + wldate);

            if (GetCachedReport(KeyCaching) == null)
            {
                ExcelContent = RenderHTML(input).Replace("{border}", "1");
                base.Export2Excel(input);
            }
            else
            {
                ExcelContent = GetCachedReport(KeyCaching).Replace("{border}", "1");
                ;
                base.Export2Excel(input);
            }
        }

        #endregion Export2Excel

        public LeagueWLSporttype()
        {
            KeyCaching = "accreportleaguewinlostsport." + AppSession.Username;
        }

        public override string CreateHeader(Hashtable input)
        {
            string db = CastAide.AsString(input["db"]);
            string path = CastAide.AsString(input["path"]);
            string fdate = CastAide.AsString(input["wldate"]);

            int displaydays = 0;
            TimeSpan sp = DateTime.Today - ReportUtils.BalanceUp.BalanceUpDay;
            displaydays = sp.Days;
            string evt = "";
            string strDateTimeSingle = Reports.GetDateTimeSingle(fdate, "Date", string.Empty, displaydays, "../../",
                                                                 path, evt);

            TemplateHandler.RemoveBlock(ref strDateTimeSingle, "getdata");

            return strDateTimeSingle;
        }
    }
}