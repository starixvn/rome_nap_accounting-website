using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Data;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Business.Report.Common;



public partial class DailyWL_OtherSport : PageBase
{

    public string mode = "";
    public string today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    RMReportUtils utilsBARM;
    protected void Page_Load(object sender, EventArgs e)
    {

        utilsBARM = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);

        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        if (Permissions.IsDenyView2HOther)
            Response.Redirect("~/AccessDenied.aspx");
        else
        {
            #region LoadData
            string str = "";

            string strfdate = Request["fdate"];
            string dateReport = Request["dateReport"];
            if (CastAide.AsInt16(dateReport) == 1)
                strfdate = today;
            else if (CastAide.AsInt16(dateReport) == 2)
                strfdate = yesterday;
            mode = Request["mode"];

            strfdate = (strfdate);

            bool IsWL = CastAide.AsDateTime(strfdate) > ReportUtils.BalanceUp.MoveBetsDay;

            string FullPhath = "<b> <a href= 'DailyWL_Sporttype.aspx?fdate=" + strfdate + "&db=" + Request["db"] + "&mode=" + mode + "'> Main </a> > " + Request["path"] + "</b>";

            #region Header
            int displaydays = 0;
            TimeSpan sp = DateTime.Today - new DateTime(2011, 01, 01);
            displaydays = sp.Days;
            string evt = "SetEvtGetdate()";
            string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingle(strfdate, "Date", string.Empty, displaydays, "../", FullPhath, evt);

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

            #endregion

            //headerDatetime.Text = Reports.GetHeader(strfdate, strtdate, FullPhath, true);
            int count = 0;

            ///str += "<table width='550px'><tr><td>";
            bool IsHasData = true;

            DbResult rs = null;
            DataTable dtLive = new DataTable(), dtNoneLive = new DataTable(), dt8RM = new DataTable(), dtBARM = new DataTable();
            decimal _8RM_Live = 0, _8RM_NoneLive = 0;
            decimal _BARM_Live = 0, _BARM_NoneLive = 0;
            DateTime wldate = DateTime.Today;
            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;

            if (!Validator.ValidDateMMDDYYYY(strfdate, out wldate).IsSuccess())
            {
                content.Text = "Invalid Date Format!!!";
            }


            if (wldate>ReportUtils.BalanceUp.MoveBetsDay)
                rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport(wldate);
            else if(wldate>=ReportUtils.BalanceUp.BalanceUpDay)
                rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport_14(wldate);
            else
                rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_OtherSport_WH(wldate);

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
            #endregion


            str += getcontent(wldate, 0, false, dtNoneLive, _8RM_NoneLive, _BARM_NoneLive);
            if (IsHasData) count++;

            str += "<span id='spSplit'><br><br></span>";

            str += getcontent(wldate, 1, true, dtLive, _8RM_Live, _BARM_Live);
            if (IsHasData) count++;

            //str += "</td></tr></table>";
            content.Text = str;
            content.Text += AssignHidden();

            string script = "<script type ='text/javascript'>";
            script += "var count = " + count.ToString() + ";";
            script += "</script>";
            Response.Write(script);

            string btnExport = Request["IsExport"];
            if (btnExport == "1")
            {
                string FileName = "Main-" + Request["path"].Replace(" ", "");
                ExportToExcel(FileName);
            }

            #endregion
        }
    }

    #region getcontent
    string getcontent(DateTime wldate, int liveindicator, bool isFooter, DataTable dt, decimal total8rm, decimal totalBARM)
    {
        StringBuilder html = new StringBuilder();
        DateTime fdate = wldate, tdate = wldate;
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        string sporttype = Request["sporttype"];
        sporttype = (string.IsNullOrEmpty(sporttype)) ? "" : sporttype;
        string db = ReportUtils.DBType;

        string title = "OTHER SPORTS";

        try
        {
            string isLive = "";
            if (liveindicator == 1) isLive = "LIVE";
            else isLive = "NON - LIVE";


            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
          

            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostSummaryAllHeader.htm", new string[] { "{msg}", "{col}", "{width}" }, new string[] { "OTHER SPORTS \"" + isLive + "\" PRODUCT WIN/LOSS REPORT", "7", "w100pc" }));
            string WLheader = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryHeader.htm");
            TemplateHandler.Assign(ref WLheader, "{TypeName}", "Bettype");

            if (dt.Rows.Count == 0)
            {
                #region no data
                html.Append(WLheader);
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                TemplateHandler.Assign(ref norow, "{tdno}", "8");
                html.Append(norow);
                return html.ToString();
                #endregion
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
            #endregion declare

            html.Append(WLheader);
            if (dt.Rows.Count > 0)
            {
                string btype = "";

                #region foreach

                foreach (DataRow dr in dt.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    string stype = "";
                    string SportName = RemotingRef.RefData.GetSportName(dr["SportType"].ToString(), "E");

                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    double l = Win + Pos + Com;

                    btype = "<b>" + SportName + "</b>";
                    stype = dr["SportType"].ToString();

                    string path = Request["path"] + "|" + SportName;
                    btype = "<a class='a' href='DailyWL_Frames.aspx?type=otherbettype&sportname=" + SportName + "&sporttype=" + stype + "&fdate=" + wldate.ToShortDateString() + "&tdate=" + wldate.ToShortDateString() + "&path=" + path + "&db=" + db + "&mode=" + Request["mode"] + "'>" + btype + "</a>";

                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, btype, Win, Pos, Com, TurnOver);
                    html.Append(row);
                    btype = "";

                    AllWin += Win; IBCWin += Win;
                    AllPos += Pos; IBCPos += Pos;
                    AllCom += Com; IBCCom += Com;
                    AllTurnOver += TurnOver; IBCTurnOver += TurnOver;
                }
                #endregion //end foreach

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
                double Margin = (AllTurnOver == 0) ? 0 : ((line / AllTurnOver) * 100);
                Sunplus.Accounting.Business.Report.Reports.genrow(ref totalRowSportType, "", "<b>All Total:</b>", Reports.colorvalue(AllWin), FormatHandler.FormatNumber20(AllPos), FormatHandler.FormatNumber20(AllCom), "<b>" + FormatHandler.FormatNumber20(AllTurnOver) + "</b>", "<b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(line) + "</b>", Reports.colorvalue(Margin, "%"));
                html.Append(totalRowSportType);

                //Total IBC
                if (total8rm != 0 || totalBARM != 0)
                {
                    var total = total8rm + (utilsBARM.IsActive ? totalBARM : 0);
                    string str8rm = "<tr><td nowrap align='right'><b>IBC(8RM" + (utilsBARM.IsActive ? " + " + utilsBARM.DisplayName : string.Empty) + ") Total:</b></td><td align='right'><b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(total) + "</b></td><td colspan='5'></td></tr>";
                    html.Append(str8rm);
                }
                #endregion end total
            }
            else
            {
                #region no data
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);
                #endregion
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            TemplateHandler.Assign(ref norow, "{nodata}", (ex.Message));
            html.Append("</table>");
            throw;
        }
        html.Append("</table>");

        if (isFooter) Footer.Text = (GetFooter(fdate, tdate, title));
        return html.ToString();
    }
    #endregion

    #region footer
    string GetFooter(DateTime fdate, DateTime tdate, string sportname)
    {
        StringBuilder html = new StringBuilder();
        string footer = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DailyWLFooter.htm");
        TemplateHandler.Assign(ref footer, "{fdate}", fdate.ToShortDateString());
        TemplateHandler.Assign(ref footer, "{tdate}", tdate.ToShortDateString());
        TemplateHandler.Assign(ref footer, "{db}", ReportUtils.DBType);

        string tplmainmenu = TemplateHandler.CopyBlock(footer, "main");

        if (!string.IsNullOrEmpty(Request["mode"]))
        {
            tplmainmenu = tplmainmenu.Replace("DailyWL_Sporttype.aspx?", "DailyWL_Sporttype.aspx?mode=" + Request["mode"] + "&");
        }

        html.Append(tplmainmenu);
        return html.ToString();
    }
    #endregion end footer

    #region AssignHidden
    string AssignHidden()
    {
        string sporttype = Request["sporttype"];
        sporttype = (string.IsNullOrEmpty(sporttype)) ? "" : sporttype;
        string type = Request["type"];

        Hashtable hash = new Hashtable();
        hash.Add("path", Request["path"]);
        hash.Add("sporttype", sporttype);
        // hash.Add("type", type);
        return CustomerUtils.AssignMultipleHidden(hash);
    }
    #endregion

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
    #endregion
}
