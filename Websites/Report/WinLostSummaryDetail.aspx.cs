using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using DNAide.Core;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Cache;

/// <summary>
/// CITS
/// </summary>
public partial class Report_WinLostSummaryDetail : PageBase
{
    private string fileName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        content.Text = gencontent();

        string btnExport = Request["btnExport"];
        if (btnExport != null)
        {
            fileName = fileName.Replace(" ", "");
            if (fileName.Length == 0) fileName = "WLSummaryDetail";
            ExportToExcel(fileName);
        }
    }

    private string gencontent()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            DateTime fdate = DateTime.Today, tdate = DateTime.Today;
            int source = FormatHandler.GetInt(Request["D"]);
            int sType = FormatHandler.GetInt(Request["S"]);
            int bType = FormatHandler.GetInt(Request["B"]);
            int bLive = FormatHandler.GetInt(Request["Live"]);

            Hashtable hidden = new Hashtable();
            hidden.Add("D", source);
            hidden.Add("S", sType);
            hidden.Add("B", bType);
            hidden.Add("Live", bLive);

            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailAllRow.htm"); string row = "";
            string totalrows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailTotalRow.htm"); string totalrow = "";
            string totalBettyperows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailTotalBettypeRow.htm"); string totalBettyperow = "";
            string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
            string bettyperows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailBettype.htm"); string bettyperow = "";
            string headers = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailHeader.htm");
            string leaguerows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailLeague.htm"); string leaguerow = "";

            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            TemplateHandler.Assign(ref norow, "{tdno}", "8");
            string odd_even = "";

            string oType = "";
            string bTypeName = "", bt = "", color = "";

            double BTypeWin = 0, BTypePos = 0, BTypeCom = 0, BTypeAd2 = 0;
            double SportWin = 0, SportPos = 0, SportCom = 0, SportAd2 = 0;
            double LeagueWin = 0, LeaguePos = 0, LeagueCom = 0, LeagueAd2 = 0;
            double Win = 0, Pos = 0, Com = 0, Ad2 = 0;

            int pBetType = 0, BetType = 0, index = 0, League = 0, pLeague = 0;
            bool pLive = false, Live = false;

            if (!Validator.ValidDateMMDDYYYY(Request["fdate"], out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(Request["tdate"], out tdate).IsSuccess())
            {
                html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WLSummaryDetailAllHeader.htm", new string[] { "{msg}" }, new string[] { "Details report from " + fdate.ToShortDateString() + " to " + tdate.ToShortDateString() }));
                html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WLSummaryDetailHeader.htm"));
                TemplateHandler.Assign(ref norow, "{nodata}", (IbcError.DATE_INVALID_FORMAT_MSG));
                html.Append(norow);
                html.Append("<table>");
                return html.ToString();
            }

            //DataTable dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_WinLostDetails(fdate, tdate, source, sType, bType, bLive).ResultsSet.Tables[0];
            DataTable dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_WinLostDetails(fdate, tdate, source, sType, bType, bLive).ResultSet.Tables[0];
            //header
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WLSummaryDetailAllHeader.htm", new string[] { "{msg}" }, new string[] { "Details report from " + fdate.ToShortDateString() + " to " + tdate.ToShortDateString() }));

            string tplExport = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/ExportToExcel.htm");
            TemplateHandler.Assign(ref tplExport, "{fdate}", fdate.ToShortDateString());
            TemplateHandler.Assign(ref tplExport, "{tdate}", tdate.ToShortDateString());
            TemplateHandler.Assign(ref tplExport, "{hidden}", CustomerUtils.AssignMultipleHidden(hidden));
            lblExportToExcel.Text = tplExport;

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bTypeName = CacheRef.GetBettype(dr["bType"]);
                    if (bType == 0) fileName = "WLSummaryDetail-from-" + fdate.ToShortDateString() + "-to-" + tdate.ToShortDateString();
                    else fileName = bTypeName + "-from-" + fdate.ToShortDateString() + "-to-" + tdate.ToShortDateString();
                    BetType = FormatHandler.GetInt(dr["BType"]);
                    Live = FormatHandler.GetBool(dr["LiveIndicator"]);
                    League = FormatHandler.GetInt(dr["leagueid"]);

                    // title bettype and header
                    if (BetType != pBetType || Live != pLive)
                    {
                        if (pBetType > 0)
                        {
                            //total league
                            totalrow = totalrows.Clone().ToString();
                            genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueAd2);
                            html.Append(totalrow);

                            //total bettype
                            bt = (pLive) ? "<font color='red'>Live! </font>" : "";
                            bt += oType;
                            totalBettyperow = totalBettyperows.Clone().ToString();
                            //color = "#dddddd";
                            color = "class='footer1'";
                            TemplateHandler.Assign(ref totalBettyperow, "{color}", color);
                            genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeAd2);
                            TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                            html.Append(totalBettyperow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueAd2 = 0;
                        BTypeWin = BTypePos = BTypeCom = BTypeAd2 = 0;

                        //bettype
                        bt = (Live) ? "<font color='red'>Live! </font>" : "";
                        bt += bTypeName;
                        bettyperow = bettyperows.Clone().ToString();
                        TemplateHandler.Assign(ref bettyperow, "{bettype}", "<b>" + bt + "</b>");
                        html.Append(bettyperow);
                        pBetType = BetType; pLive = Live; pLeague = 0; index = 0;
                        oType = bTypeName;

                        //header
                        html.Append(headers);
                    }

                    //league
                    if (League != pLeague)
                    {
                        if (pLeague > 0 && index > 1)
                        {
                            totalrow = totalrows.Clone().ToString();
                            genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueAd2);
                            html.Append(totalrow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueAd2 = 0;
                        index = 0;

                        leaguerow = leaguerows.Clone().ToString();
                        TemplateHandler.Assign(ref leaguerow, "{league}", CacheRef.GetLeagueName(dr["LeagueId"]));
                        html.Append(leaguerow);
                        pLeague = League;
                    }
                    index++;
                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    Ad2 = FormatHandler.GetDouble(dr["Admin2"]);

                    //row
                    row = rows.Clone().ToString();

                    if (BetType == 10) // outright
                    {
                        TemplateHandler.RemoveBlock(ref row, "no-away");
                        TemplateHandler.Assign(ref row, "{col}", "3");
                        TemplateHandler.Assign(ref row, "{align}", "c");
                    }
                    else
                        TemplateHandler.Assign(ref row, "{align}", "r");

                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{matchcode}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref row, "{home}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
                    TemplateHandler.Assign(ref row, "{away}", CacheRef.GetTeamName(dr["awayid"]));
                    genrow(ref row, Win, Pos, Com, Ad2);
                    html.Append(row);

                    BTypeWin += Win; SportWin += Win; LeagueWin += Win;
                    BTypePos += Pos; SportPos += Pos; LeaguePos += Pos;
                    BTypeCom += Com; SportCom += Com; LeagueCom += Com;
                    BTypeAd2 += Ad2; SportAd2 += Ad2; LeagueAd2 += Ad2;
                }
                //total league
                totalrow = totalrows.Clone().ToString();
                genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueAd2);
                html.Append(totalrow);

                //total bettype
                bt = (pLive) ? "<font color='red'>Live! </font>" : "";
                bt += oType;
                totalBettyperow = totalBettyperows.Clone().ToString();
                //color = "#dddddd";
                color = "class='footer1'";
                TemplateHandler.Assign(ref totalBettyperow, "{color}", color);
                genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeAd2);
                TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                html.Append(totalBettyperow);

                //colspan row
                html.Append("<tr><td colspan='8'>&nbsp;</tr></tr>");

                //total all row
                totalBettyperow = totalBettyperows.Clone().ToString();
                //color = "#99cccc";
                color = "class='report-footer'";
                TemplateHandler.Assign(ref totalBettyperow, "{color}", color);
                genrow(ref totalBettyperow, SportWin, SportPos, SportCom, SportAd2);
                TemplateHandler.Assign(ref totalBettyperow, "{bettype}", "");
                html.Append(totalBettyperow);
            }
            else
            {
                TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
                html.Append(norow);
            }
            html.Append("</table>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        string tplTableBlank = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TableBlank.htm");
        html.Append(tplTableBlank);
        return html.ToString();
    }

    #region string colorvalue(object value)

    private static string colorvalue(object value)
    {
        string str = "";
        if (FormatHandler.GetDouble(value) > 0) str = "<font color ='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
        else
        {
            if (FormatHandler.GetDouble(value) == 0) str = FormatHandler.FormatNumber2(value);
            else str = FormatHandler.FormatNumber20(value);
        }
        return str;
    }

    #endregion string colorvalue(object value)

    #region void genrow(ref string row, double win, double pos, double com, double, double admin2)

    private static void genrow(ref string row, double win, double pos, double com, double admin2)
    {
        TemplateHandler.Assign(ref row, "{WinLost}", FormatHandler.FormatNumber2(colorvalue(win)));
        TemplateHandler.Assign(ref row, "{Position}", FormatHandler.FormatNumber2(colorvalue(pos)));
        TemplateHandler.Assign(ref row, "{Comm}", FormatHandler.FormatNumber2(colorvalue(com)));
        double l = win + pos + com;
        TemplateHandler.Assign(ref row, "{LineTotal}", FormatHandler.FormatNumber2(colorvalue(l)));
        TemplateHandler.Assign(ref row, "{Admin2}", FormatHandler.FormatNumber2(colorvalue(admin2)));
    }

    #endregion void genrow(ref string row, double win, double pos, double com, double, double admin2)

    #region ExportToExcel

    public void ExportToExcel(string FileName)
    {
        string strStyle = ReportHTML.CSSExcelReport();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        Response.Charset = "";

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        content.RenderControl(htmlWrite);

        Response.Write(strStyle);

        Response.Write(stringWrite.ToString());
        Response.End();
    }

    #endregion ExportToExcel
}