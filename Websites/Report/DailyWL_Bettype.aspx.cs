using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Business.Report.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;

public partial class DailyWL_Bettype : PageBase
{
    private bool IsBlankCell = false;
    public string mode = "";
    public string jsfunction = "";
    public string today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    private RMReportUtils utilsBARM = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        int count = 0;
        try
        {
            utilsBARM = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);

            Title = Sunplus.Accounting.Business.Report.Reports.Title;
            string strfdate = Request["fdate"];
            string dateReport = Request["dateReport"];
            if (CastAide.AsInt16(dateReport) == 1)
                strfdate = today;
            else if (CastAide.AsInt16(dateReport) == 2)
                strfdate = yesterday;

            string str = "";
            string path = "";
            string header = "";
            mode = Request["mode"];
            bool IsWL = CastAide.AsDateTime(strfdate) > ReportUtils.BalanceUp.MoveBetsDay;
            string sportname = Request["sportname"];
            string sporttype = Request["sporttype"];

            string strURL = "DailyWL_Sporttype.aspx?fdate=" + strfdate + "&db=" + Request["db"] + "&mode=" + mode;
            path += "<span class='action font-small' onclick=\"open_dailyreport('" + strURL + "');\"><font color='blue'>Main > </font></span>";

            //soccer

            Hashtable hash = null;
            string type = (string.IsNullOrEmpty(Request["type"])) ? string.Empty : Request["type"];
            switch (type)
            {
                case "soccer":
                    hash = (Hashtable)Session["sitemap_soccer"];
                    header = "SOCCER";
                    if (Permissions.IsDenyView2HSoccer)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                case "hr":
                    hash = (Hashtable)Session["sitemap_hr"];
                    header = "Horse Racing";
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                case "fi":
                    hash = (Hashtable)Session["sitemap_fi"];
                    header = "Finance";
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                case "casino":
                    hash = (Hashtable)Session["sitemap_casino"];
                    header = "Casino";
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                case "bingo":
                    hash = (Hashtable)Session["sitemap_bingo"];
                    header = "Bingo";
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                case "p2p":
                    hash = (Hashtable)Session["sitemap_p2p"];
                    header = "P2P";
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;

                default:
                    hash = (Hashtable)Session["sitemap_other"];
                    header = Request["path"].ToUpper();
                    if (Permissions.IsDenyView2HOther)
                        Response.Redirect("~/AccessDenied.aspx");
                    break;
            }
            string sitemap = "";
            string lnksporttype = hash["sporttype"].ToString();
            string lnkpath = hash["path"].ToString();
            string lnkpage = hash["page"].ToString();
            string lnkclick = hash["linkclick"].ToString();

            if (type.Equals("otherbettype"))
            {
                strURL = lnkpage + "?sporttype=" + lnksporttype + "&fdate=" + strfdate + "&path=" + lnkpath;// +"&mode=" + lnkmode;
                strURL = "<span class='action font-small' onclick=\"open_dailyreport('" + strURL + "');\"><font color='blue'><b>" + lnkclick + "</b> </font></span>";
                path += strURL + " > <b class='font-small'>" + Request["path"].Split('|')[1] + "</b>";
            }
            else
                path += "<b class='font-small'>" + Request["path"] + "</b>";

            bool hashData = true;

            DateTime wldate;

            #region Header

            int displaydays = 0;
            TimeSpan sp = DateTime.Today - new DateTime(2011, 01, 01);
            displaydays = sp.Days;
            string evt = "SetEvtGetdate()";

            string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingleMaxDays(strfdate, "Date", string.Empty, displaydays, 7, "../", string.Empty, evt);

            if (IsWL)
                TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "");
            else
                TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "none");

            string db = Request["db"];
            if (db == null) db = "Rep";
            if (db == "Rep")
            {
                TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "checked=checked");
                TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "");
            }
            else
            {
                TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "");
                TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "checked=checked");
            }
            TemplateHandler.Assign(ref strDateTimeSingle, "db", db);

            headerDatetime.Text = strDateTimeSingle;

            #endregion Header

            //headerDatetime.Text = Reports.GetHeader(strfdate, strtdate, path, true);
            DbResult rs;
            DataTable dtLive = new DataTable(), dtNoneLive = new DataTable(), dt8RM = new DataTable(), dtBARM = new DataTable();
            decimal _8RM_Live = 0, _8RM_NoneLive = 0, _BARM_Live = 0, _BARM_NoneLive = 0;

            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            if (!Validator.ValidDateMMDDYYYY(strfdate, out wldate).IsSuccess())
            {
                content.Text = "Invalid Date Format!!!";
                return;
            }

            switch (type)
            {
                case "soccer":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Soccer_Bettype(wldate);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Soccer_Bettype_14(wldate);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Soccer_Bettype_WH(wldate);

                    #region separate data Live and None Line

                    dtLive = rs.ResultSet.Tables[0].Clone();
                    dtNoneLive = rs.ResultSet.Tables[0].Clone();
                    dt8RM = rs.ResultSet.Tables[1];

                    if (rs.ResultSet.Tables.Count == 3)
                    {
                        dtBARM = rs.ResultSet.Tables[2];

                        _BARM_Live = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        _BARM_NoneLive = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }

                    _8RM_Live = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                    _8RM_NoneLive = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        if (CastAide.AsBoolean(dr["liveindicator"]))
                        {
                            DataRow drNewLive = dtLive.NewRow();
                            for (int i = 0; i < dtLive.Columns.Count; i++)
                            {
                                drNewLive[i] = dr[i];
                            }
                            dtLive.Rows.Add(drNewLive);
                        }
                        else
                        {
                            DataRow drNewNoneLive = dtNoneLive.NewRow();
                            for (int i = 0; i < dtNoneLive.Columns.Count; i++)
                            {
                                drNewNoneLive[i] = dr[i];
                            }
                            dtNoneLive.Rows.Add(drNewNoneLive);
                        }
                    }

                    #endregion separate data Live and None Line

                    break;

                case "numbergame":
                    IsBlankCell = false;
                    var dailyWL = CommonBootStrapper.Locator.GetInstance<IReportRacingDAO>();

                    rs = dailyWL.DailyWLDirect_NumberGame(wldate.ToShortDateString());

                    #region separate data Live and None Line

                    dtLive = rs.ResultSet.Tables[0].Clone();
                    dtNoneLive = rs.ResultSet.Tables[0].Clone();
                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        if (CastAide.AsBoolean(dr["liveindicator"]))
                        {
                            DataRow drNewLive = dtLive.NewRow();
                            for (int i = 0; i < dtLive.Columns.Count; i++)
                            {
                                drNewLive[i] = dr[i];
                            }
                            dtLive.Rows.Add(drNewLive);
                        }
                        else
                        {
                            DataRow drNewNoneLive = dtNoneLive.NewRow();
                            for (int i = 0; i < dtNoneLive.Columns.Count; i++)
                            {
                                drNewNoneLive[i] = dr[i];
                            }
                            dtNoneLive.Rows.Add(drNewNoneLive);
                        }
                    }

                    #endregion separate data Live and None Line

                    break;

                case "hr":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing(wldate);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing_14(wldate);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_HorseRacing_WH(wldate);
                    dtNoneLive = rs.ResultSet.Tables[0];
                    break;

                case "fi":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance(wldate);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance_14(wldate);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Finance_WH(wldate);
                    dtNoneLive = rs.ResultSet.Tables[0];
                    break;

                case "casino":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Casino(wldate);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Casino14(wldate);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Casino_WH(wldate);
                    dtNoneLive = rs.ResultSet.Tables[0];
                    break;

                case "bingo":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Bingo(wldate);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Bingo_14(wldate);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_Bingo_WH(wldate);
                    dtNoneLive = rs.ResultSet.Tables[0];
                    break;

                case "otherbettype":
                    IsBlankCell = false;
                    if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport_Bettype(wldate, sporttype);
                    else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport_Bettype_14(wldate, sporttype);
                    else
                        rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport_Bettype_WH(wldate, sporttype);

                    #region separate data Live and None Line

                    dtLive = rs.ResultSet.Tables[0].Clone();
                    dtNoneLive = rs.ResultSet.Tables[0].Clone();
                    dt8RM = rs.ResultSet.Tables[1];

                    if (rs.ResultSet.Tables.Count == 3)
                    {
                        dtBARM = rs.ResultSet.Tables[2];

                        _BARM_Live = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        _BARM_NoneLive = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }

                    _8RM_Live = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                    _8RM_NoneLive = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        if (CastAide.AsBoolean(dr["liveindicator"]))
                        {
                            DataRow drNewLive = dtLive.NewRow();
                            for (int i = 0; i < dtLive.Columns.Count; i++)
                            {
                                drNewLive[i] = dr[i];
                            }
                            dtLive.Rows.Add(drNewLive);
                        }
                        else
                        {
                            DataRow drNewNoneLive = dtNoneLive.NewRow();
                            for (int i = 0; i < dtNoneLive.Columns.Count; i++)
                            {
                                drNewNoneLive[i] = dr[i];
                            }
                            dtNoneLive.Rows.Add(drNewNoneLive);
                        }
                    }

                    #endregion separate data Live and None Line

                    break;
            }

            //str += "<table width='500px'><tr><td>";
            str += getcontent(wldate, 0, header, dtNoneLive, _8RM_NoneLive, _BARM_NoneLive);
            if (dtNoneLive.Rows.Count > 0)
            {
                count++;
                str += "<span style='height:15px;'  id='spSplit'><br/><br/></span>";
            }

            switch (type)
            {
                case "hr":
                case "fi":
                case "casino":
                case "bingo":
                    break;

                default:
                    str += getcontent(wldate, 1, header, dtLive, _8RM_Live, _BARM_Live);
                    if (dtLive.Rows.Count > 0) count++;
                    break;
            }

            str += "</td></tr></table>";

            Footer.Text = GetFooter(strfdate, strfdate, sportname, sporttype, Request["path"]);
            Footer.Text += AssignHidden();

            content.Text = str;

            //Export to Excel
            string btnExport = Request["IsExport"];
            if (btnExport == "1")
            {
                string FileName = "Main-" + Request["path"].Replace(" ", "");
                ExportToExcel(FileName);
            }
        }
        catch (Exception ex)
        {
            //string script = "<script type ='text/javascript'>";
            //script += "alert('" + IBCUtils.Encryption.RemoveSpecialChars(ex.Message) + "');";
            //script += "</script>";
            //Response.Write(script);
            LogExtensions.ExceptionHandler(ex);
        }
        finally
        {
            string script = "<script type ='text/javascript'>";
            script += "var count = " + count.ToString() + ";";
            script += "</script>";
            //Response.Write(script);
            jsfunction = script;
        }
    }

    private string type = "";

    #region getcontent

    private string getcontent(DateTime wldate, int liveindicator, string header, DataTable dt, decimal total8rm, decimal totalBARM)
    {
        StringBuilder html = new StringBuilder();

        DateTime fdate = DateTime.Today, tdate = DateTime.Today;
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        type = Request["type"];
        type = (string.IsNullOrEmpty(type)) ? "" : type;
        string sporttype = Request["sporttype"];
        sporttype = (string.IsNullOrEmpty(sporttype)) ? "0" : sporttype;
        string sportname = Request["sportname"];
        sportname = (string.IsNullOrEmpty(sportname)) ? "" : sportname;

        try
        {
            string isLive = "";
            if (liveindicator == 1) isLive = "LIVE";
            else isLive = "NON - LIVE";

            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            string strfdate;
            strfdate = Request["fdate"];
            strfdate = ReportHTML.ValidDate(strfdate);

            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            if (!Validator.ValidDateMMDDYYYY(strfdate, out fdate).IsSuccess())
            {
                return html.ToString();
            }

            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostSummaryAllHeader.htm", new string[] { "{msg}", "{width}", "{col}", "{style}" }, new string[] { header + " \"" + isLive + "\" PRODUCT WIN/LOSS REPORT", "", "7", "width='500'" }));
            string WLheader = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryHeader.htm");

            TemplateHandler.Assign(ref WLheader, "{TypeName}", "Bettype");

            if (dt.Rows.Count == 0)
            {
                #region no data

                TemplateHandler.RemoveBlock(ref WLheader, "no-link");
                html.Append(WLheader);
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                TemplateHandler.Assign(ref norow, "{tdno}", "7");
                html.Append(norow);
                return html.ToString();

                #endregion no data
            }

            #region declare variable

            string sp = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummarySportType.htm");
            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryAllRow.htm");
            string colspans = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryColspan.htm"), colspan = "";
            string totalrows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryTotalRow.htm");

            string odd_even = "";
            string color = "";

            TemplateHandler.RemoveBlock(ref WLheader, "no-link");//no details
            TemplateHandler.RemoveBlock(ref rows, "no-link");
            TemplateHandler.Assign(ref norow, "{tdno}", "7");
            TemplateHandler.Assign(ref colspans, "{col}", "7");
            TemplateHandler.Assign(ref totalrows, "{col}", "1");

            double AllWin = 0, AllPos = 0, AllCom = 0, AllTurnOver = 0;
            double IBCWin = 0, IBCPos = 0, IBCCom = 0, IBCTurnOver = 0;

            double Win = 0, Pos = 0, Com = 0, TurnOver = 0;
            string totalRowSportType = "";
            string BettypeName = "", Bettype = "";

            int IsHedge = 0;
            int index = 0;
            bool IsCalculatedTotalToteOdds = false, IsExistedTOdds = false, IsExistedFOdds = false;
            string condition = "", row = "";
            double l = 0;

            #endregion declare variable

            html.Append(WLheader);
            if (dt.Rows.Count > 0)
            {
                #region foreach

                foreach (DataRow dr in dt.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    row = rows.Clone().ToString();
                    var bettypeId = CastAide.AsInt32(dr["bettype"]);
                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    l = Win + Pos + Com;
                    if (type == "hr")
                    {
                        l = FormatHandler.GetDouble(dr["LineTotal"]);
                        switch (bettypeId)
                        {
                            case 31:
                            case 32:
                            case 33:
                                IsExistedTOdds = true;
                                break;

                            case 41:
                            case 42:
                            case 43:
                                IsExistedFOdds = true;
                                break;
                        }

                        #region Subtotal Tote Odds HR

                        if (!IsCalculatedTotalToteOdds && IsExistedTOdds && bettypeId > 34)
                        {
                            row = rows.Clone().ToString();
                            TemplateHandler.Assign(ref row, "style", "bgcolor='' style='font-weight:bold'");
                            BettypeName = "<b>Tote Odds Total:</b>";
                            condition = "bettype = 31 or bettype = 32 or bettype = 33";
                            Win = CastAide.AsDouble(dt.Compute("sum(WinLost)", condition));
                            Pos = CastAide.AsDouble(dt.Compute("sum(Position)", condition));
                            Com = CastAide.AsDouble(dt.Compute("sum(Commission)", condition));
                            TurnOver = CastAide.AsDouble(dt.Compute("sum(TurnOver)", condition));
                            l = CastAide.AsDouble(dt.Compute("sum(LineTotal)", condition));
                            Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, BettypeName, Win, Pos, Com, l, TurnOver, IsBlankCell);
                            html.Append(row);

                            Win = FormatHandler.GetDouble(dr["WinLost"]);
                            Pos = FormatHandler.GetDouble(dr["Position"]);
                            Com = FormatHandler.GetDouble(dr["Commission"]);
                            TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                            l = FormatHandler.GetDouble(dr["LineTotal"]);

                            row = rows.Clone().ToString();
                            colspan = colspans.Clone().ToString();
                            TemplateHandler.Assign(ref colspan, "{msg}", "");
                            html.Append(colspan);

                            IsCalculatedTotalToteOdds = true;
                        }

                        #endregion Subtotal Tote Odds HR
                    }

                    BettypeName = RemotingRef.RefData.GetBetTypeName(bettypeId.ToString(), "E");  //CastAide.AsString(dr["bettypename"]);
                    Bettype = bettypeId.ToString();

                    #region Set Bettype Name

                    if (string.IsNullOrEmpty(BettypeName))
                    {
                        switch (Bettype)
                        {
                            case "71": BettypeName = "Casino"; break;
                            case "72": BettypeName = "P2P"; break;
                            case "252":
                                BettypeName = Resources.Language.lblIBCbetBingo;
                                break;
                        }
                    }

                    #endregion Set Bettype Name

                    string path = "Main>" + Request["path"] + ">" + BettypeName;

                    TemplateHandler.Assign(ref row, "{style}", "");
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);

                    #region all betype

                    if (dt.Columns.Contains("IsHedge") && CastAide.AsString(dr["IsHedge"]).Contains("(hedge)"))
                    {
                        IsHedge = 1;
                        switch (bettypeId)
                        {
                            case 1:
                                BettypeName = "<b>HDP (hedge)</b>";
                                break;

                            case 3:
                                BettypeName = "<b>OU (hedge)</b>";
                                break;

                            case 7:
                                BettypeName = "<b>1st HDP (hedge)</b>";
                                break;

                            case 8:
                                BettypeName = "<b>1st OU (hedge)</b>";
                                break;
                        }
                    }
                    else
                    {
                        IsHedge = 0;

                        switch (bettypeId)
                        {
                            case 1:
                                BettypeName = "<b>HDP</b>";
                                break;

                            case 3:
                                BettypeName = "<b>OU</b>";
                                break;

                            case 4:
                                BettypeName = "<b>CS</b>";
                                break;

                            case 30:
                                BettypeName = "<b>1st CS</b>";
                                break;

                            case 6:
                                BettypeName = "<b>TG</b>";
                                break;

                            case 7:
                                BettypeName = "<b>1st HDP</b>";
                                break;

                            case 8:
                                BettypeName = "<b>1st OU</b>";
                                break;

                            case 14:
                                BettypeName = "<b>FG/LG</b>";
                                break;

                            case 1001:
                                BettypeName = "<b>Kick Off</b>";
                                break;

                            default:
                                BettypeName = "<b>" + BettypeName + "</b>";
                                break;
                        }
                    }

                    #endregion all betype

                    string db = (ReportUtils.IsRepDB) ? "Rep" : "Main";

                    path = path.Replace(">", "-");

                    string evt = "OpenDetails('" + Bettype + "','" + liveindicator + "','" + type + "','" + wldate.ToShortDateString() + "','" + wldate.ToShortDateString() + "','" + sporttype + "','" + sportname + "','" + path + "','" + db + "','" + IsHedge + "','" + Request["mode"] + "');";
                    BettypeName = "<span id='spTypeName' onclick=\"" + evt + "\"><b><a href='#'>" + BettypeName + "</a></span></b>";

                    if (Bettype == "71") BettypeName = "Casino";
                    if (Bettype == "72") BettypeName = "P2P";
                    //kiet mai: change Casino Mini Game to ibc bingo
                    if (Bettype == "73") BettypeName = Resources.Language.lblIBCbetBingo;

                    Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, BettypeName, Win, Pos, Com, l, TurnOver, IsBlankCell);
                    html.Append(row);
                    BettypeName = "";

                    AllWin += Win; IBCWin += Win;
                    AllPos += Pos; IBCPos += Pos;
                    AllCom += Com; IBCCom += Com;
                    AllTurnOver += TurnOver; IBCTurnOver += TurnOver;
                }

                #endregion foreach

                if (type == "hr")
                {
                    #region Subtotal Totle & Fixed Odds HR

                    row = rows.Clone().ToString();
                    TemplateHandler.Assign(ref row, "style", "bgcolor='' style='font-weight:bold'");
                    if (!IsCalculatedTotalToteOdds && IsExistedTOdds)
                    {
                        BettypeName = "<b>Tote Odds Total:</b>";
                        condition = "bettype = 31 or bettype = 32 or bettype = 33";
                    }
                    else if (IsExistedFOdds)
                    {
                        BettypeName = "<b>Fxied Odds Total:</b>";
                        condition = "bettype = 41 or bettype = 42 or bettype = 43";
                    }
                    Win = CastAide.AsDouble(dt.Compute("sum(WinLost)", condition));
                    Pos = CastAide.AsDouble(dt.Compute("sum(Position)", condition));
                    Com = CastAide.AsDouble(dt.Compute("sum(Commission)", condition));
                    TurnOver = CastAide.AsDouble(dt.Compute("sum(TurnOver)", condition));
                    l = CastAide.AsDouble(dt.Compute("sum(LineTotal)", condition));
                    Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, BettypeName, Win, Pos, Com, l, TurnOver, IsBlankCell);
                    html.Append(row);
                    row = rows.Clone().ToString();

                    #endregion Subtotal Totle & Fixed Odds HR
                }

                #region //total

                colspan = colspans.Clone().ToString();
                TemplateHandler.Assign(ref colspan, "{msg}", "");
                html.Append(colspan);

                //All Total
                totalRowSportType = totalrows.Clone().ToString();
                color = " bgcolor='#dddddd'";
                TemplateHandler.Assign(ref totalRowSportType, "{colorline}", color);
                TemplateHandler.Assign(ref totalRowSportType, "{coloradmin}", color);

                double line = AllWin + AllPos + AllCom;

                if (type == "hr")
                {
                    line = CastAide.AsDouble(dt.Compute("sum(LineTotal)", ""));
                }

                double Margin = (AllTurnOver == 0) ? 0 : ((line / AllTurnOver) * 100);
                Sunplus.Accounting.Business.Report.Reports.genrow(ref totalRowSportType, "", "<b>All Total:</b>", Reports.colorvalue(AllWin), FormatHandler.FormatNumber20(AllPos), FormatHandler.FormatNumber20(AllCom), "<b>" + FormatHandler.FormatNumber20(AllTurnOver) + "</b>", "<b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(line) + "</b>", Reports.colorvalue(Margin, "%"));
                html.Append(totalRowSportType);

                //Total IBC
                if (total8rm != 0 || totalBARM != 0)
                {
                    decimal totalRM = total8rm + (utilsBARM.IsActive ? totalBARM : 0);
                    string str8rm = "<tr><td align='right' nowrap><b>IBC(8RM" + (utilsBARM.IsActive ? " + " + utilsBARM.DisplayName : string.Empty) + ") Total:</b></td><td nowrap align='right'><b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(totalRM) + "</b></td><td colspan='5'></td></tr>";
                    html.Append(str8rm);
                }

                #endregion //total
            }
            else
            {
                #region no data

                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);

                #endregion no data
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            TemplateHandler.Assign(ref norow, "{nodata}", (ex.Message));
            html.Append("</table>");
            throw;
        }
        html.Append("</table>");

        return html.ToString();
    }

    #endregion getcontent

    #region footer

    private string GetFooter(string fdate, string tdate, string sportname, string sporttype, string path)
    {
        StringBuilder html = new StringBuilder();

        string footer = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DailyWLFooter.htm");

        TemplateHandler.Assign(ref footer, "{fdate}", fdate);
        TemplateHandler.Assign(ref footer, "{tdate}", tdate);
        TemplateHandler.Assign(ref footer, "{db}", ReportUtils.DBType);

        string tplmainmenu = TemplateHandler.CopyBlock(footer, "main");

        if (!string.IsNullOrEmpty(Request["mode"]))
        {
            tplmainmenu = tplmainmenu.Replace("DailyWL_Sporttype.aspx?", "DailyWL_Sporttype.aspx?mode=" + Request["mode"] + "&");
        }

        html.Append(tplmainmenu);

        return html.ToString();
    }

    #endregion footer

    #region AssignHidden

    private string AssignHidden()
    {
        Hashtable hash = new Hashtable();

        type = (string.IsNullOrEmpty(type)) ? "" : type;
        string sporttype = Request["sporttype"];
        sporttype = (string.IsNullOrEmpty(sporttype)) ? "0" : sporttype;
        string sportname = Request["sportname"];
        sportname = (string.IsNullOrEmpty(sportname)) ? "" : sportname;

        hash.Add("path", Request["path"]);
        hash.Add("type", type);
        hash.Add("sporttype", sporttype);
        hash.Add("sportname", sportname);

        return CustomerUtils.AssignMultipleHidden(hash);
    }

    #endregion AssignHidden

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
        Response.Write("<table><tr><td class='tdhidden'></td></tr></table>");
        Response.Write(stringWrite.ToString());
        Response.End();
    }

    #endregion ExportToExcel
}