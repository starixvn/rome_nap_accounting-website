using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Report;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;

using System.Collections.Specialized;

/// <summary>
/// CITS
/// </summary>
public partial class NumberGameWLBettypeDetail : PageBase
{

    public string param = "";
    private string keyReport = "NumberGameDailyWLByBettypeDetail";
    protected void Page_Load(object sender, EventArgs e)
    {
        int sporttype = 161;
        //StringCollection strPermission = AppSession.RoleAccess;
        keyReport += AppSession.AccUserName;
        string btnExport = Request["btnExport"];
        if (btnExport != null)
        {            
            string fileName = Request["path"].Replace(" ", "-");
            var objData = AppSession.GetSession(Session, keyReport);
            var data = string.Empty;
            if (objData != null)
            {
                data = objData.ToString();
            }
            else
            {
                data = gencontent();
            }
            ExportToExcel(fileName, data);
        }
        else
        {
            content.Text = gencontent();
            content.Text += Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TableBlank.htm");
        }
    }

    string gencontent()
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
            header = "NUMBER GAME EVENT";

            param = "&B=" + bType + "&Live=" + bLive + "&site=" + Request["site"] + "&fdate=" + Request["fdate"] +
                    "&tdate=" + Request["tdate"] + "&sporttype=" + Request["sporttype"] + "&sportname=" +
                    Request["sportname"] + "&path=" + Request["path"] + "&mode=" + mode;


            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailAllRow.htm");
            string row = "";
           
                TemplateHandler.Assign(ref rows, "{align}", "r");
                TemplateHandler.Assign(ref rows, "{col}", "1");
            

            string totalrows =
                Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailTotalRow.htm");
            string totalrow = "";
            string totalBettyperows =
                Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailTotalBettypeRow.htm");
            string totalBettyperow = "";
            string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
            string bettyperows =
                Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailBettype.htm");
            string bettyperow = "";
            string headers = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailHeader.htm");
            string leaguerows =
                Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/NumberGame/BettypeWL/WinLostSummaryDetailLeague.htm");
            string leaguerow = "";

            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");



            TemplateHandler.Assign(ref totalrows, "{cols}", "1");
            TemplateHandler.Assign(ref norow, "{tdno}", "9");
            TemplateHandler.Assign(ref bettyperows, "{cols}", "9");
            TemplateHandler.Assign(ref totalBettyperows, "{cols}", "1");
            TemplateHandler.Assign(ref headers, "{matchcode}", "");



            string odd_even = "";

            string oType = "";
            string bTypeName = "", bt = "", color = "";

            double bTypeWin = 0, bTypePos = 0, bTypeCom = 0, bTypeTurnOver = 0, BTypeLineTotal = 0;
            double SportWin = 0, SportPos = 0, SportCom = 0, SportTurnOver = 0, SportLineTotal = 0;
            double LeagueWin = 0, LeaguePos = 0, LeagueCom = 0, LeagueTurnOver = 0, LeagueLineTotal = 0;
            double Win = 0, Pos = 0, Com = 0, TurnOver = 0, LineTotal = 0;
            double total8rm = 0;
            double virtualLT = 0d, virtualLTBettype = 0d, virtualLTSport = 0d, virtualLTLeague = 0;


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
            int colspanColumn = 9;

            int SiteID = CastAide.AsInt32(Request.QueryString["SiteID"]);
            int Currency = CastAide.AsInt32(Request.QueryString["Currency"]);

            if (CastAide.AsDateTime(Request["fdate"]) > ReportUtils.BalanceUp.MoveBetsDay)
                rs = NumberGameWinLoss.WinlossByBetTypeDetail(wldate, bLive, bType, SiteID, Currency);
            else if (CastAide.AsDateTime(Request["fdate"]) >= ReportUtils.BalanceUp.BalanceUpDay)
                rs = NumberGameWinLoss.WinlossByBetTypeDetail14(wldate, bLive, bType, SiteID, Currency);
            else
                rs = NumberGameWinLoss.WinlossByBetTypeDetail_Old(wldate, bLive, bType, SiteID, Currency);

            colspanColumn = 9;
            DataTable dt = rs.ResultSet.Tables[0];
            //header
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Report/NumberGame/BettypeWL/WinLostSummaryDetailAllHeader.htm",
                                                                      new string[] {"{msg}", "{cols}"},
                                                                      new string[]
                                                                          {
                                                                              header + " WIN/LOSS REPORT",
                                                                              colspanColumn.ToString()
                                                                          }));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    //for Soccer and other sport
                    BetType = FormatHandler.GetInt(dr["BType"]);
                    Live = FormatHandler.GetBool(dr["LiveIndicator"]);

                     
                    bTypeName = CacheRef.GetBettype(BetType);
                    League = FormatHandler.GetInt(dr["leagueid"]);

                    // title bettype and header
                    if (BetType != pBetType || Live != pLive)
                    {
                        if (pBetType > 0)
                        {
                            //total league
                            totalrow = totalrows.Clone().ToString();
                            
                            genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver, virtualLTLeague);

                            html.Append(totalrow);

                            //total bettype
                            bt = (pLive) ? "<font color='red'>Live! </font>" : "";
                            bt += oType;
                            totalBettyperow = totalBettyperows.Clone().ToString();
                            color = "class=bgTotal";
                            TemplateHandler.Assign(ref totalBettyperow, "{color}", color);



                            genrow(ref totalBettyperow, bTypeWin, bTypePos, bTypeCom, bTypeTurnOver, virtualLTBettype);
                            TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                            html.Append(totalBettyperow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueTurnOver = LeagueLineTotal = 0;
                        bTypeWin = bTypePos = bTypeCom = bTypeTurnOver = BTypeLineTotal = 0;

                        //bettype                        
                        bt = (Live) ? "<font color='red'>Live! </font>" : "";
                        bt += bTypeName;
                        bettyperow = bettyperows.Clone().ToString();
                        TemplateHandler.Assign(ref bettyperow, "{bettype}", "<b>" + path + "</b>");
                        html.Append(bettyperow);
                        pBetType = BetType;
                        pLive = Live;
                        pLeague = 0;
                        index = 0;
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

                            genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver, virtualLTLeague);
                            html.Append(totalrow);
                        }
                        LeagueWin = LeaguePos = LeagueCom = LeagueTurnOver = LeagueLineTotal = 0;
                        index = 0;

                        leaguerow = leaguerows.Clone().ToString();

                        TemplateHandler.Assign(ref leaguerow, "{cols}", "8");


                        TemplateHandler.Assign(ref leaguerow, "{league}", CacheRef.GetLeagueName(dr["LeagueId"]));
                        html.Append(leaguerow);
                        pLeague = League;
                    }
                    index++;
                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    virtualLT = FormatHandler.GetDouble(dr["VirtualLineTotal"]);


                    //row
                    row = rows.Clone().ToString();
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);


                    //for Soccer and Other sport
                    TemplateHandler.Assign(ref row, "{matchcode}", dr["matchcode"].ToString());

                    genrow(ref row, Win, Pos, Com, TurnOver, virtualLT);
                    html.Append(row);

                    bTypeWin += Win;
                    SportWin += Win;
                    LeagueWin += Win;
                    bTypePos += Pos;
                    SportPos += Pos;
                    LeaguePos += Pos;
                    bTypeCom += Com;
                    SportCom += Com;
                    LeagueCom += Com;
                    bTypeTurnOver += TurnOver;
                    SportTurnOver += TurnOver;
                    LeagueTurnOver += TurnOver;
                    BTypeLineTotal += LineTotal;
                    SportLineTotal += LineTotal;
                    LeagueLineTotal += LineTotal;

                    virtualLTBettype += virtualLT;
                    virtualLTSport += virtualLT;
                    virtualLTLeague += virtualLT;
                }
                //total league
                if (index > 1)
                {
                    totalrow = totalrows.Clone().ToString();

                    genrow(ref totalrow, LeagueWin, LeaguePos, LeagueCom, LeagueTurnOver, virtualLTLeague);
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


                genrow(ref totalBettyperow, bTypeWin, bTypePos, bTypeCom, bTypeTurnOver, virtualLTBettype);

                TemplateHandler.Assign(ref totalBettyperow, "{bettype}", bt);
                html.Append(totalBettyperow);

                //total 8rm
                if (total8rm != 0)
                    html.Append("<tr><td colspan='2' align='right'><b>IBC(8RM) Total:</b></td><td align='right'><b>" +
                                colorvalue(total8rm) + "</b></td><td colspan='5'></td></tr>");
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

        AppSession.AddSession(Session, keyReport, html.ToString());
        return html.ToString();
    }

    #region string colorvalue(object value)
    static string colorvalue(object value)
    {
        string str = "";
        if (FormatHandler.GetDouble(value) > 0) str = "<font color ='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
        else
        {
            str = FormatHandler.FormatNumber20(value);
        }
        return str;
    }
    #endregion 

    #region void genrow(ref string row, double win, double pos, double com, double, double admin2)
    static void genrow(ref string row, double win, double pos, double com, double turnOver, double virtualLt)
    {
        TemplateHandler.Assign(ref row, "{WinLost}", FormatHandler.FormatNumber20(colorvalue(win)));
        TemplateHandler.Assign(ref row, "{Position}", FormatHandler.FormatNumber20(pos));
        TemplateHandler.Assign(ref row, "{Comm}", FormatHandler.FormatNumber20(com));
        double l = win + pos + com;
        double variance = l - virtualLt;
        double Margin = (turnOver == 0) ? 0 : ((l / turnOver) * 100);
        TemplateHandler.Assign(ref row, "{LineTotal}", FormatHandler.FormatNumber20(colorvalue(l)));
        TemplateHandler.Assign(ref row, "{VirtualLineTotal}", FormatHandler.FormatNumber20(colorvalue(virtualLt)));
        TemplateHandler.Assign(ref row, "{TurnOver}", FormatHandler.FormatNumber20(turnOver));
        TemplateHandler.Assign(ref row, "{Margin}", Reports.colorvalue(Margin, "%"));
        TemplateHandler.Assign(ref row, "{Variance}", FormatHandler.FormatNumber20(variance));
    }
    
    #endregion

    #region ExportToExcel
    public void ExportToExcel(string FileName, string data)
    {
        string strStyle = "";

        strStyle = ReportHTML.CSSExcelReport();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.xls";
        Response.Write(strStyle);
        Response.Write(data);
        Response.End();
    }
    #endregion
}
 
