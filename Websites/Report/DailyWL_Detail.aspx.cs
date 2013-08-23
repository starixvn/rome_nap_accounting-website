using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Business.Report.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// CITS
/// </summary>
public partial class DailyWL_Detail : PageBase
{
    public string param = "";
    private RMReportUtils utilsBARM = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sporttype = Request["sporttype"];
        sporttype = (string.IsNullOrEmpty(sporttype)) ? "1" : sporttype;

        utilsBARM = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);

        switch (sporttype)
        {
            case "1":
                if (Permissions.IsDenyView2HSoccer)
                    Response.Redirect("~/AccessDenied.aspx");
                break;

            default:
                if (Permissions.IsDenyView2HOther)
                    Response.Redirect("~/AccessDenied.aspx");
                break;
        }

        content.Text = gencontent();
        content.Text += Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TableBlank.htm");

        string btnExport = Request["btnExport"];
        if (btnExport != null)
        {
            string fileName = Request["path"].Replace(" ", "-");
            ExportToExcel(fileName);
        }
    }

    private string gencontent()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            string path = Request["path"];
            path = path.Replace("-", ">");
            DateTime wldate = DateTime.Today;
            int bType = FormatHandler.GetInt(Request["B"]);
            int bLive = FormatHandler.GetInt(Request["Live"]);
            string sporttype = Request["sporttype"];
            sporttype = (string.IsNullOrEmpty(sporttype)) ? "1" : sporttype;
            string site = Request["site"];
            string header = Request["site"];
            if (site.Equals("rbs")) site = "RBS";
            else site = "Admin1";
            string mode = Request["mode"];
            bool IsWL = CastAide.AsDateTime(Request["fdate"]) > ReportUtils.BalanceUp.MoveBetsDay;

            int IsHedge = CastAide.AsInt32(Request["IsHedge"], 0);

            if (header.Equals("otherbettype")) header = (string.IsNullOrEmpty(Request["sportname"]) ? "" : Request["sportname"].ToUpper()) + " EVENT";
            else if (header.Equals("hr")) header = "HORSE RACING";
            else if (header.Equals("fi")) header = "FINANCE RACING";
            else if (header.Equals("numbergame")) header = "NUMBER GAME";
            else header = "SOCCER EVENT";

            param = "&B=" + bType + "&Live=" + bLive + "&site=" + Request["site"] + "&fdate=" + Request["fdate"] + "&tdate=" + Request["tdate"] + "&sporttype=" + Request["sporttype"] + "&sportname=" + Request["sportname"] + "&path=" + Request["path"] + "&mode=" + mode;

            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailAllRow.htm"); string row = "";
            if (bType == 10 || sporttype == "201") // Outright , FI
            {
                TemplateHandler.RemoveBlock(ref rows, "no-away");
                TemplateHandler.Assign(ref rows, "{col}", "3");
                TemplateHandler.Assign(ref rows, "{align}", "c");
            }
            else
            {
                TemplateHandler.Assign(ref rows, "{align}", "r");
                TemplateHandler.Assign(ref rows, "{col}", "1");
            }

            string totalrows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailTotalRow.htm"); string totalrow = "";
            string totalBettyperows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailTotalBettypeRow.htm"); string totalBettyperow = "";
            string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
            string bettyperows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailBettype.htm"); string bettyperow = "";
            string headers = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailHeader.htm");
            string leaguerows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryDetailLeague.htm"); string leaguerow = "";

            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");

            switch (sporttype)
            {
                case "151"://HR
                case "201"://FI
                    TemplateHandler.RemoveBlock(ref rows, "event");
                    TemplateHandler.RemoveBlock(ref headers, "event");
                    TemplateHandler.Assign(ref totalrows, "{cols}", "1");
                    TemplateHandler.Assign(ref norow, "{tdno}", "7");
                    TemplateHandler.Assign(ref bettyperows, "{cols}", "7");
                    TemplateHandler.Assign(ref totalBettyperows, "{cols}", "1");
                    if (sporttype == "151")
                        TemplateHandler.Assign(ref headers, "{matchcode}", "Race");
                    else
                        TemplateHandler.Assign(ref headers, "{matchcode}", "Event");
                    break;

                default:
                    TemplateHandler.Assign(ref totalrows, "{cols}", "4");
                    TemplateHandler.Assign(ref norow, "{tdno}", "10");
                    TemplateHandler.Assign(ref bettyperows, "{cols}", "10");
                    TemplateHandler.Assign(ref totalBettyperows, "{cols}", "4");
                    TemplateHandler.Assign(ref headers, "{matchcode}", "");
                    break;
            }

            string odd_even = "";

            string oType = "";
            string bTypeName = "", bt = "", color = "";

            double BTypeWin = 0, BTypePos = 0, BTypeCom = 0, BTypeTurnOver = 0, BTypeLineTotal = 0;
            double SportWin = 0, SportPos = 0, SportCom = 0, SportTurnOver = 0, SportLineTotal = 0;
            double LeagueWin = 0, LeaguePos = 0, LeagueCom = 0, LeagueTurnOver = 0, LeagueLineTotal = 0;
            double Win = 0, Pos = 0, Com = 0, TurnOver = 0, LineTotal = 0;
            double total8rm = 0, totalBARM = 0;

            int pBetType = 0, BetType = 0, index = 0, League = 0, pLeague = 0;
            bool pLive = false, Live = false;

            string strfdate, strtdate;
            strfdate = Request["fdate"];
            strtdate = Request["tdate"];

            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            strtdate = (string.IsNullOrEmpty(strtdate)) ? DateTime.Today.ToShortDateString() : strtdate;
            if (!Validator.ValidDateMMDDYYYY(strfdate, out wldate).IsSuccess())
            {
                return html.ToString();
            }

            DbResult rs;
            int ColspanColumn = 10;
            if (sporttype == "151") //for HR
            {
                if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing_Details(wldate, bType);
                else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing_Details_14(wldate, bType);
                else
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing_Details_WH(wldate, bType);
                ColspanColumn = 7;
            }
            else if (sporttype == "201") //for FI
            {
                if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance_Details(wldate, bType);
                else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance_Details_14(wldate, bType);
                else
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance_Details_WH(wldate, bType);
                ColspanColumn = 7;
            }
            else // for soccer and other sport
            {
                if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Details(wldate, bLive, bType, sporttype, IsHedge);
                else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Details_14(wldate, bLive, bType, sporttype, IsHedge);
                else
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.MatchWinLostReport.Acc_Rpt_DailyWLDirect_Details_WH(wldate, bLive, bType, sporttype, IsHedge);
                ColspanColumn = 10;
            }

            DataTable dt = rs.ResultSet.Tables[0];
            total8rm = (Convert.IsDBNull(rs.Outputs["winlost_8rm"])) ? 0 : Convert.ToDouble(rs.Outputs["winlost_8rm"]);
            totalBARM = CastAide.AsDouble(rs.Outputs["winlost_barm"]);

            //header
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostSummaryDetailAllHeader.htm", new string[] { "{msg}", "{cols}" }, new string[] { header + " WIN/LOSS REPORT", ColspanColumn.ToString() }));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (sporttype)
                    {
                        case "151": //hr
                        case "201": //fi
                            BetType = bType;
                            break;

                        default: //for Soccer and other sport
                            BetType = FormatHandler.GetInt(dr["BType"]);
                            Live = FormatHandler.GetBool(dr["LiveIndicator"]);
                            break;
                    }

                    bTypeName = CacheRef.GetBettype(BetType);
                    League = FormatHandler.GetInt(dr["leagueid"]);

                    // title bettype and header
                    if (BetType != pBetType || Live != pLive)
                    {
                        if (pBetType > 0)
                        {
                            //total league
                            totalrow = totalrows.Clone().ToString();

                            if (sporttype == "151") //HR
                                genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueLineTotal, LeagueTurnOver);
                            else
                                genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver);

                            html.Append(totalrow);

                            //total bettype
                            bt = (pLive) ? "<font color='red'>Live! </font>" : "";
                            bt += oType;
                            totalBettyperow = totalBettyperows.Clone().ToString();
                            color = "class=bgTotal";
                            TemplateHandler.Assign(ref totalBettyperow, "{color}", color);

                            if (sporttype == "151") //HR
                                genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeLineTotal, BTypeTurnOver);
                            else
                                genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeTurnOver);
                            TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                            html.Append(totalBettyperow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueTurnOver = LeagueLineTotal = 0;
                        BTypeWin = BTypePos = BTypeCom = BTypeTurnOver = BTypeLineTotal = 0;

                        //bettype
                        bt = (Live) ? "<font color='red'>Live! </font>" : "";
                        bt += bTypeName;
                        bettyperow = bettyperows.Clone().ToString();
                        TemplateHandler.Assign(ref bettyperow, "{bettype}", "<b>" + path + "</b>");
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
                            if (sporttype == "151")
                                genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueLineTotal, LeagueTurnOver);
                            else
                                genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver);
                            html.Append(totalrow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueTurnOver = LeagueLineTotal = 0;
                        index = 0;

                        leaguerow = leaguerows.Clone().ToString();
                        switch (sporttype)
                        {
                            case "151":
                            case "201":
                                TemplateHandler.RemoveBlock(ref leaguerow, "col1");
                                TemplateHandler.Assign(ref leaguerow, "{cols}", "7");
                                break;

                            default:
                                TemplateHandler.Assign(ref leaguerow, "{cols}", "9");
                                break;
                        }

                        TemplateHandler.Assign(ref leaguerow, "{league}", CacheRef.GetLeagueName(dr["LeagueId"]));
                        html.Append(leaguerow);
                        pLeague = League;
                    }
                    index++;
                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    if (sporttype == "151") //HR
                        LineTotal = FormatHandler.GetDouble(dr["LineTotal"]);

                    //row
                    row = rows.Clone().ToString();
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);

                    switch (sporttype)
                    {
                        case "151"://HR
                            TemplateHandler.Assign(ref row, "{matchcode}", "<b>" + dr["race"].ToString() + "</b>");
                            break;

                        case "201"://FI
                            TemplateHandler.Assign(ref row, "{matchcode}", CacheRef.GetFncIndexName(dr["homeid"]));
                            break;

                        default: //for Soccer and Other sport
                            TemplateHandler.Assign(ref row, "{matchcode}", dr["matchcode"].ToString());
                            TemplateHandler.Assign(ref row, "{home}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
                            TemplateHandler.Assign(ref row, "{away}", CacheRef.GetTeamName(dr["awayid"]));
                            break;
                    }
                    if (sporttype == "151")
                        genrow(ref row, Win, Pos, Com, LineTotal, TurnOver);
                    else
                        genrow(ref row, Win, Pos, Com, TurnOver);
                    html.Append(row);

                    BTypeWin += Win; SportWin += Win; LeagueWin += Win;
                    BTypePos += Pos; SportPos += Pos; LeaguePos += Pos;
                    BTypeCom += Com; SportCom += Com; LeagueCom += Com;
                    BTypeTurnOver += TurnOver; SportTurnOver += TurnOver; LeagueTurnOver += TurnOver;
                    BTypeLineTotal += LineTotal; SportLineTotal += LineTotal; LeagueLineTotal += LineTotal;
                }
                //total league
                if (index > 1)
                {
                    totalrow = totalrows.Clone().ToString();
                    if (sporttype == "151")//HR
                        genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueLineTotal, LeagueTurnOver);
                    else
                        genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver);
                    html.Append(totalrow);
                }

                //total bettype
                bt = (pLive) ? "<font color='red'>Live! </font>" : "";
                bt += oType;

                if (IsHedge == 1)
                    bt += " (Hegde)";

                totalBettyperow = totalBettyperows.Clone().ToString();
                color = "class=bgTotal";
                TemplateHandler.Assign(ref totalBettyperow, "{color}", color);

                if (sporttype == "151")//HR
                    genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeLineTotal, BTypeTurnOver);
                else
                    genrow(ref totalBettyperow, BTypeWin, BTypePos, BTypeCom, BTypeTurnOver);

                TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                html.Append(totalBettyperow);

                //total 8rm
                if (total8rm != 0 || totalBARM != 0)
                {
                    double totalRM = total8rm + (utilsBARM.IsActive ? totalBARM : 0);
                    html.Append("<tr><td colspan='4' align='right'><b>IBC(8RM" + (utilsBARM.IsActive ? " + " + utilsBARM.DisplayName : string.Empty) + ") Total:</b></td><td align='right'><b>" + colorvalue(totalRM) + "</b></td><td colspan='5'></td></tr>");
                }
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
        return html.ToString();
    }

    #region string colorvalue(object value)

    private static string colorvalue(object value)
    {
        string str = "";
        if (FormatHandler.GetDouble(value) > 0) str = "<font color ='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
        else
        {
            str = FormatHandler.FormatNumber20(value);
        }
        return str;
    }

    #endregion string colorvalue(object value)

    #region void genrow(ref string row, double win, double pos, double com, double, double admin2)

    private static void genrow(ref string row, double win, double pos, double com, double TurnOver)
    {
        TemplateHandler.Assign(ref row, "{WinLost}", FormatHandler.FormatNumber20(colorvalue(win)));
        TemplateHandler.Assign(ref row, "{Position}", FormatHandler.FormatNumber20(pos));
        TemplateHandler.Assign(ref row, "{Comm}", FormatHandler.FormatNumber20(com));
        double l = win + pos + com;
        double Margin = (TurnOver == 0) ? 0 : ((l / TurnOver) * 100);
        TemplateHandler.Assign(ref row, "{LineTotal}", FormatHandler.FormatNumber20(colorvalue(l)));
        TemplateHandler.Assign(ref row, "{TurnOver}", FormatHandler.FormatNumber20(TurnOver));
        TemplateHandler.Assign(ref row, "{Margin}", Reports.colorvalue(Margin, "%"));
    }

    private static void genrow(ref string row, double win, double pos, double com, double linetotal, double TurnOver)
    {
        TemplateHandler.Assign(ref row, "{WinLost}", FormatHandler.FormatNumber20(colorvalue(win)));
        TemplateHandler.Assign(ref row, "{Position}", FormatHandler.FormatNumber20(pos));
        TemplateHandler.Assign(ref row, "{Comm}", FormatHandler.FormatNumber20(com));
        double l = linetotal;
        double Margin = (TurnOver == 0) ? 0 : ((l / TurnOver) * 100);
        TemplateHandler.Assign(ref row, "{LineTotal}", FormatHandler.FormatNumber20(colorvalue(l)));
        TemplateHandler.Assign(ref row, "{TurnOver}", FormatHandler.FormatNumber20(TurnOver));
        TemplateHandler.Assign(ref row, "{Margin}", Reports.colorvalue(Margin, "%"));
    }

    #endregion void genrow(ref string row, double win, double pos, double com, double, double admin2)

    #region ExportToExcel

    public void ExportToExcel(string FileName)
    {
        string strStyle = "";

        strStyle = ReportHTML.CSSExcelReport();

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