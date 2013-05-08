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
using Sunplus.Accounting.Core.Extensions;
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


using ExcelUtil.Spreadsheet;
using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Core.Cache;

public partial class DailyWL_CombinationMixParlay : PageBase
{

    public string mode = "";
    public string title = "";
    private DataTable dataTable = null;
    public string today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["bettype"] != null)
        {
            hiddenBettype.Value = Request.QueryString["bettype"];
        }
        Title = Sunplus.Accounting.Business.Report.Reports.Title;

        if (Permissions.IsDenyView2HOther)
            Response.Redirect("~/AccessDenied.aspx");
        else
        {
            string fdate = Request["fdate"];
            string dateReport = Request["dateReport"];
            if (CastAide.AsInt16(dateReport) == 1)
                fdate = today;
            else if (CastAide.AsInt16(dateReport) == 2)
                fdate = yesterday;

            fdate = (fdate);

            string path = Request["path"];
            title = path;
            mode = Request["mode"];
            bool IsWL = CastAide.AsDateTime(fdate) > ReportUtils.BalanceUp.MoveBetsDay;

            //content.Text += "Main>" + path;
            string fullpath = "<b> <a href= 'DailyWL_Sporttype.aspx?fdate=" + fdate + "&db=" + Request["db"] + "&mode=" + mode + "'> Main </a> > " + path + "</b>";
            #region Header
            int displaydays = 0;
            TimeSpan sp = DateTime.Today - new DateTime(2011, 01, 01);
            displaydays = sp.Days;
            string evt = "SetEvtGetdate()";
            string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingle(fdate, "Date", string.Empty, displaydays, "../", fullpath, evt);

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
            //headerDatetime.Text = Reports.GetHeader(fdate, tdate, fullpath, true);
            string btnExport = Request["IsExport"];
            if (btnExport != "1")
            {
                this.GetDataTable(IsWL,fdate);
                Session["dataTable_SystemParlay2h_Report"] = this.dataTable;

                content.Text += getcontent(IsWL);
                string tableBlank = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TableBlank.htm");
                content.Text += tableBlank;

                Footer.Text = GetFooter(fdate, fdate);
                Footer.Text += AssignHidden();
            }
            if (btnExport == "1")
            {
                this.dataTable = (DataTable)Session["dataTable_SystemParlay2h_Report"];
                string FileName = "Main-" + Request["path"].Replace(" ", "");
                ExportToExcel2(FileName);
            }
        }
    }

    #region getcontent

    private void GetDataTable(bool IsWL, string strfdate)
    {
        DateTime wldate = DateTime.Today;
        //string strfdate = Request["fdate"];
        Validator.ValidDateMMDDYYYY(strfdate, out wldate);
        int bettype = -1;

        if (Request.QueryString["bettype"] != null)
        {
            bettype = CastAide.AsInt32(Request.QueryString["bettype"]);
        }
        else
        {
            bettype = CastAide.AsInt32(Request.Form["hiddenBettype"]);
            hiddenBettype.Value = Request.Form["hiddenBettype"];
        }

        DbResult rs = MatchWinLostReport.Acc_Rpt_DailyWLDirect_MixParlay(wldate, bettype);

        if (rs != null && rs.ResultSet.Tables.Count > 0)
        {
            this.dataTable = rs.ResultSet.Tables[0];
        }
    }

    private DataTable GetDataTableByLiveIndicator(int liveindicator)
    {
        if (this.dataTable != null)
        {
            var dataTable = this.dataTable.Clone();
            var live = CastAide.AsBoolean(liveindicator);
            DataRow newRow = dataTable.NewRow();
            foreach (DataRow row in this.dataTable.Rows)
            {
                if (CastAide.AsBoolean(row["liveindicator"]) == live)
                {
                    newRow = dataTable.NewRow();
                    for (int i = 0; i < this.dataTable.Columns.Count; ++i)
                    {
                        newRow[i] = row[i];
                    }
                    dataTable.Rows.Add(newRow);
                }
            }
            return dataTable;
        }
        return null;
    }

    string getcontent(bool IsWL)
    {
        StringBuilder html = new StringBuilder();
        DateTime wldate = DateTime.Today;
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        try
        {
            TemplateHandler.Assign(ref norow, "{tdno}", "7");
            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            string strfdate, strtdate;
            strfdate = Request["fdate"];

            strfdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            strtdate = (string.IsNullOrEmpty(strfdate)) ? DateTime.Today.ToShortDateString() : strfdate;
            if (!Validator.ValidDateMMDDYYYY(strfdate, out wldate).IsSuccess())
            {
                return html.ToString();
            }

            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostSummaryAllHeader.htm", new string[] { "{msg}", "{col}" }, new string[] { CacheRef.GetBettype(29).ToUpper() + " PRODUCT WIN/LOSS REPORT", "7" }));

            string WLheader = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryHeader.htm");
            TemplateHandler.RemoveBlock(ref WLheader, "no-link");
           // DbResult rs = null;
            int bettype = CastAide.AsInt32(Request["bettype"], 0);
            //var dailyWL = CommonBootStrapper.Locator.GetInstance<IReportRacingDAO>();
            //rs = dailyWL.DailyWLDirect_MixParlay(wldate.ToShortDateString(), bettype);


            TemplateHandler.Assign(ref WLheader, "{TypeName}", "Bettype");

            DataTable dtMP = this.dataTable;
            DataView dvMP = dtMP.DefaultView;
            dvMP.Sort = "sporttype asc";

            DataTable dt = dvMP.ToTable();

            #region declare variable
            string sp = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummarySportType.htm");
            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryAllRow.htm");
            TemplateHandler.RemoveBlock(ref rows, "no-link");

            string colspans = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryColspan.htm"), colspan = "";
            TemplateHandler.Assign(ref colspans, "{col}", "7");

            string totalrows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostSummaryTotalRow.htm");
            TemplateHandler.Assign(ref totalrows, "{col}", "1");
            string odd_even = "";
            string color = "";

            int SportType = 0;
            double AllWin = 0, AllPos = 0, AllCom = 0, AllTurnOver = 0;

            double Win = 0, Pos = 0, Com = 0, TurnOver = 0;
            string totalRowSportType = "";
            int sporttype_p = 0, sporttype_c = 0, index = 0;

            #endregion declare

            html.Append(WLheader);

            if (dt.Rows.Count > 0)
            {
                string sportname = "";
                string btype = "";
                string condition = "";
                string row = "";
                odd_even = "odd";
                #region foreach

                foreach (DataRow dr in dt.Rows)
                {

                    sporttype_c = CastAide.AsInt32(dr["sporttype"]);
                    if (sporttype_p != sporttype_c)
                    {
                        odd_even = "odd";

                        if (index > 0)
                        {
                            #region Sub total on each sport type
                            condition = "sporttype='" + sporttype_p + "'";
                            Win = CastAide.AsDouble(dt.Compute("sum(Winlost)", condition));
                            Pos = CastAide.AsDouble(dt.Compute("sum(Position)", condition));
                            Com = CastAide.AsDouble(dt.Compute("sum(Commission)", condition));
                            TurnOver = CastAide.AsDouble(dt.Compute("sum(TurnOver)", condition));
                            row = rows.Clone().ToString();
                            if (sporttype_p == 1)
                                btype = "<b>Soccer Sport Total</b>";
                            else if (sporttype_p == 98)
                                btype = "<b>Multi-Sport Total</b>";
                            else
                                btype = "<b>Others Sport Total</b>";
                            Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, btype, Win, Pos, Com, TurnOver);
                            TemplateHandler.Assign(ref row, "style", "style='font-weight:bold;background-color:#d0e8fb'");
                            TemplateHandler.Assign(ref row, "odd_even", "total2");
                            html.Append(row);
                            colspan = colspans.Clone().ToString();
                            TemplateHandler.Assign(ref colspan, "{msg}", "");
                            html.Append(colspan);
                            #endregion
                        }
                        sporttype_p = sporttype_c;
                    }


                    row = rows.Clone().ToString();

                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    double l = Win + Pos + Com;
                    btype = "<b>" + RemotingRef.RefData.GetBetTypeName(dr["bettype"].ToString(), "EN") + "(" + dr["status"].ToString() + ")</b>";
                    sportname = "<b>" + RemotingRef.RefData.GetBetTypeName(dr["bettype"].ToString(), "EN") + "</b>";

                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, btype, Win, Pos, Com, TurnOver);
                    TemplateHandler.Assign(ref row, "style", "");
                    html.Append(row);

                    AllWin += Win;
                    AllPos += Pos;
                    AllCom += Com;
                    AllTurnOver += TurnOver;
                    index++;
                }
                #endregion //end foreach

                #region Sub total on each sport type
                condition = "sporttype='" + sporttype_p + "'";
                Win = CastAide.AsDouble(dt.Compute("sum(Winlost)", condition));
                Pos = CastAide.AsDouble(dt.Compute("sum(Position)", condition));
                Com = CastAide.AsDouble(dt.Compute("sum(Commission)", condition));
                TurnOver = CastAide.AsDouble(dt.Compute("sum(TurnOver)", condition));
                row = rows.Clone().ToString();
                if (sporttype_p == 1)
                    btype = "<b>Soccer Sport Total</b>";
                else if (sporttype_p == 98)
                    btype = "<b>Multi-Sport Total</b>";
                else
                    btype = "<b>Others Sport Total</b>";
                Sunplus.Accounting.Business.Report.Reports.genrow(ref row, color, btype, Win, Pos, Com, TurnOver);
                TemplateHandler.Assign(ref row, "style", "style='font-weight:bold;background-color:#d0e8fb'");
                TemplateHandler.Assign(ref row, "odd_even", "total2");
                html.Append(row);
                colspan = colspans.Clone().ToString();
                TemplateHandler.Assign(ref colspan, "{msg}", "");
                #endregion

                #region //grand total

                colspan = colspans.Clone().ToString();
                TemplateHandler.Assign(ref colspan, "{msg}", "");
                html.Append(colspan);

                //All Total
                totalRowSportType = totalrows.Clone().ToString();
                color = " bgcolor='#8ac9db'";
                TemplateHandler.Assign(ref totalRowSportType, "{colorline}", color);
                TemplateHandler.Assign(ref totalRowSportType, "{coloradmin}", color);

                double line = AllWin + AllPos + AllCom;
                double Margin = (AllTurnOver == 0) ? 0 : ((line / AllTurnOver) * 100);
                Sunplus.Accounting.Business.Report.Reports.genrow(ref totalRowSportType, "", "<b>All Total:</b>", Reports.colorvalue(AllWin), FormatHandler.FormatNumber20(AllPos), FormatHandler.FormatNumber20(AllCom), "<b>" + FormatHandler.FormatNumber20(AllTurnOver) + "</b>", "<b>" + Sunplus.Accounting.Business.Report.Reports.colorvalue(line) + "</b>", Reports.colorvalue(Margin, "%"));
                html.Append(totalRowSportType);
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
            LogExtensions.ExceptionHandler(ex);
            TemplateHandler.Assign(ref norow, "{nodata}", (ex.Message));
            html.Append("</table>");

        }
        html.Append("</table>");

        return html.ToString();
    }
    #endregion

    #region AssignHidden
    string AssignHidden()
    {
        Hashtable hash = new Hashtable();
        hash.Add("path", Request["path"]);
        return CustomerUtils.AssignMultipleHidden(hash);
    }
    #endregion

    #region GetFooter
    string GetFooter(string fdate, string tdate)
    {
        StringBuilder html = new StringBuilder();
        string footer = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DailyWLFooter.htm");
        string url_today = "DailyWL_CombinationMixParlay.aspx?F=RECAL&fdate=" + fdate + "&tdate=" + tdate + "&Date=" + DateTime.Today.ToShortDateString() + "&path=" + Request["path"] + "&mode=" + Request["mode"]; // - today
        string url_yesterday = "DailyWL_CombinationMixParlay.aspx?F=RECAL&fdate=" + fdate + "&tdate=" + tdate + "&Date=" + DateTime.Today.AddDays(-1).ToShortDateString() + "&path=" + Request["path"] + "&mode=" + Request["mode"];//- yesterday


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

    public void ExportToExcel2(string FileName)
    {
        var excelFile = new ExcelFile();
        var wsReport = excelFile.Worksheets.Add(Request["path"].Replace(" ", ""));

        int currencRow = 0;
        DataTable dtMP = this.dataTable;
        ExportLiveOrNoneLive(ref wsReport, ref currencRow, dtMP);

        //respone-------------------

        var response = HttpContext.Current.Response;
        response.Clear();
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        excelFile.SaveXls(response.OutputStream);
    }

    private void ExportLiveOrNoneLive(ref ExcelWorksheet wsReport, ref int currencRow, DataTable dtMP)
    {
        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Merged = true;
        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Style.Font.Name = "Times New Roman";

        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Value = CacheRef.GetBettype(29).ToUpper() + " PRODUCT WIN/LOSS REPORT";
        //header
        ++currencRow;
        AssignHeaderExportExcel(ref wsReport, ref currencRow);
        //body
        string btype = "";
        double Win, Pos, Com, TurnOver, lineTotal, margin;
        int currencSportType = -1, previousSportType = -1;

        double total_Win = 0, total_Pos = 0, total_Com = 0, total_TurnOver = 0, total_lineTotal = 0, total_margin = 0;
        double all_total_Win = 0, all_total_Pos = 0, all_total_Com = 0, all_total_TurnOver = 0, all_total_lineTotal = 0, all_total_margin = 0;

        var totalSportTypeCellStyle = new Sunplus.Accounting
            .Core.Helper.ExcelGemboxUtils().GetStyleText(System.Drawing.Color.Black, 211, 225, 200, HorizontalAlignmentStyle.Center);
        if (dtMP.Rows.Count <= 0)
        {
            ++currencRow;
            wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Value = "There is no information available.";
            wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Style.Font.Name = "Times New Roman";
            return;
        }
        foreach (DataRow dr in dtMP.Rows)
        {
            ++currencRow;
            if (currencSportType == -1 && previousSportType == -1)
            {
                btype = RemotingRef.RefData.GetBetTypeName(dr["bettype"].ToString(), "EN") + "(" + dr["status"].ToString() + ")";
                wsReport.Cells[currencRow, 0].Value = btype;
                wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";

                Win = FormatHandler.GetDouble(dr["WinLost"]);
                total_Win += Win;
                all_total_Win += Win;
                wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(Win);
                if (colorvalue(Win) == "red")
                {
                    wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
                }

                Pos = FormatHandler.GetDouble(dr["Position"]);
                total_Pos += Pos;
                all_total_Pos += Pos;
                wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(Pos);

                Com = FormatHandler.GetDouble(dr["Commission"]);
                total_Com += Com;
                all_total_Com += Com;
                wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(Com);

                lineTotal = Win + Pos + Com;
                total_lineTotal += lineTotal;
                all_total_lineTotal += lineTotal;
                wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(lineTotal);
                if (colorvalue(lineTotal) == "red")
                {
                    wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
                }

                TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                total_TurnOver += TurnOver;
                all_total_TurnOver += TurnOver;
                wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(TurnOver);

                margin = (TurnOver == 0) ? 0 : ((lineTotal / TurnOver) * 100);
                wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
                wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(margin) + "%";
                if (colorvalue(margin) == "red")
                {
                    wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
                }

                currencSportType = previousSportType = CastAide.AsInt32(dr["sporttype"]);
            }
            else
            {
                currencSportType = CastAide.AsInt32(dr["sporttype"]);
                if (currencSportType == previousSportType)
                {
                    btype = RemotingRef.RefData.GetBetTypeName(dr["bettype"].ToString(), "EN") + "(" + dr["status"].ToString() + ")";
                    wsReport.Cells[currencRow, 0].Value = btype;
                    wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    total_Win += Win;
                    all_total_Win += Win;
                    wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(Win);
                    if (colorvalue(Win) == "red")
                    {
                        wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    total_Pos += Pos;
                    all_total_Pos += Pos;
                    wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(Pos);

                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    total_Com += Com;
                    all_total_Com += Com;
                    wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(Com);

                    lineTotal = Win + Pos + Com;
                    total_lineTotal += lineTotal;
                    all_total_lineTotal += lineTotal;
                    wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(lineTotal);
                    if (colorvalue(lineTotal) == "red")
                    {
                        wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    total_TurnOver += TurnOver;
                    all_total_TurnOver += TurnOver;
                    wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(TurnOver);

                    margin = (TurnOver == 0) ? 0 : ((lineTotal / TurnOver) * 100);
                    wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(margin) + "%";
                    if (colorvalue(margin) == "red")
                    {
                        wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    currencSportType = CastAide.AsInt32(dr["sporttype"]);
                }
                else if (currencSportType != previousSportType)
                {
                    //Total previous sporttype
                    if (previousSportType == 1)
                        btype = "Soccer Sport Total";
                    else if (previousSportType == 98)
                        btype = "Multi-Sport Total";
                    else
                        btype = "Others Sport Total";

                    wsReport.Cells[currencRow, 0].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 0].Value = btype;
                    wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 0].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                    wsReport.Cells[currencRow, 1].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 1].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(total_Win);
                    if (colorvalue(total_Win) == "red")
                    {
                        wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    wsReport.Cells[currencRow, 2].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 2].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(total_Pos);

                    wsReport.Cells[currencRow, 3].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 3].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 3].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(total_Com);

                    wsReport.Cells[currencRow, 4].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 4].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 4].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(total_lineTotal);
                    if (colorvalue(total_lineTotal) == "red")
                    {
                        wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    wsReport.Cells[currencRow, 5].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 5].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 5].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(total_TurnOver);

                    wsReport.Cells[currencRow, 6].Style.Font.Size = 200;
                    wsReport.Cells[currencRow, 6].Style = totalSportTypeCellStyle;
                    wsReport.Cells[currencRow, 6].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    total_margin = (total_TurnOver == 0) ? 0 : ((total_lineTotal / total_TurnOver) * 100);
                    wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(total_margin) + "%";
                    if (colorvalue(total_margin) == "red")
                    {
                        wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
                    }
                    total_Win = 0; total_Pos = 0; total_Com = 0; total_TurnOver = 0; total_lineTotal = 0; total_margin = 0;

                    //space row
                    ++currencRow;
                    wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Merged = true;
                    //------------------------
                    //new sporttype
                    ++currencRow;
                    btype = RemotingRef.RefData.GetBetTypeName(dr["bettype"].ToString(), "EN") + "(" + dr["status"].ToString() + ")";
                    wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 0].Value = btype;
                    wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    Win = FormatHandler.GetDouble(dr["WinLost"]);
                    total_Win += Win;
                    all_total_Win += Win;
                    wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(Win);
                    if (colorvalue(Win) == "red")
                    {
                        wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    Pos = FormatHandler.GetDouble(dr["Position"]);
                    total_Pos += Pos;
                    all_total_Pos += Pos;
                    wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(Pos);

                    Com = FormatHandler.GetDouble(dr["Commission"]);
                    total_Com += Com;
                    all_total_Com += Com;
                    wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(Com);

                    lineTotal = Win + Pos + Com;
                    total_lineTotal += lineTotal;
                    all_total_lineTotal += lineTotal;
                    wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(lineTotal);
                    if (colorvalue(lineTotal) == "red")
                    {
                        wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    TurnOver = FormatHandler.GetDouble(dr["TurnOver"]);
                    total_TurnOver += TurnOver;
                    all_total_TurnOver += TurnOver;
                    wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(TurnOver);

                    margin = (TurnOver == 0) ? 0 : ((lineTotal / TurnOver) * 100);
                    wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
                    wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(margin) + "%";
                    if (colorvalue(margin) == "red")
                    {
                        wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
                    }

                    currencSportType = previousSportType = CastAide.AsInt32(dr["sporttype"]);
                }
            }
        }
        //total last sport
        ++currencRow;
        if (previousSportType == 1)
            btype = "Soccer Sport Total";
        else if (previousSportType == 98)
            btype = "Multi-Sport Total";
        else
            btype = "Others Sport Total";

        wsReport.Cells[currencRow, 0].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 0].Value = btype;
        wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 0].Style.Font.Size = 200;
        wsReport.Cells[currencRow, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

        wsReport.Cells[currencRow, 1].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 1].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(total_Win);
        if (colorvalue(total_Win) == "red")
        {
            wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
        }

        wsReport.Cells[currencRow, 2].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 2].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(total_Pos);

        wsReport.Cells[currencRow, 3].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 3].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(total_Com);

        wsReport.Cells[currencRow, 4].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 4].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(total_lineTotal);
        if (colorvalue(total_lineTotal) == "red")
        {
            wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
        }

        wsReport.Cells[currencRow, 5].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 5].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(total_TurnOver);

        wsReport.Cells[currencRow, 6].Style = totalSportTypeCellStyle;
        wsReport.Cells[currencRow, 6].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        total_margin = (total_TurnOver == 0) ? 0 : ((total_lineTotal / total_TurnOver) * 100);
        wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(total_margin) + "%";
        if (colorvalue(total_margin) == "red")
        {
            wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
        }

        //space row
        ++currencRow;
        wsReport.Cells.GetSubrangeAbsolute(currencRow, 0, currencRow, 6).Merged = true;

        //All Total
        ++currencRow;

        wsReport.Cells[currencRow, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 0].Value = "All Total:";
        wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;

        wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 1].Value = FormatHandler.FormatNumber20(all_total_Win);
        if (colorvalue(all_total_Win) == "red")
        {
            wsReport.Cells[currencRow, 1].Style.Font.Color = System.Drawing.Color.Red;
        }

        wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 2].Value = FormatHandler.FormatNumber20(all_total_Pos);

        wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 3].Value = FormatHandler.FormatNumber20(all_total_Com);

        var allTotalSportTypeCellStyle = new Sunplus.Accounting
            .Core.Helper.ExcelGemboxUtils().GetStyleText(System.Drawing.Color.Black, 181, 196, 208, HorizontalAlignmentStyle.Center);
        wsReport.Cells[currencRow, 4].Style = allTotalSportTypeCellStyle;
        wsReport.Cells[currencRow, 4].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 4].Value = FormatHandler.FormatNumber20(all_total_lineTotal);
        if (colorvalue(all_total_lineTotal) == "red")
        {
            wsReport.Cells[currencRow, 4].Style.Font.Color = System.Drawing.Color.Red;
        }

        wsReport.Cells[currencRow, 5].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        wsReport.Cells[currencRow, 5].Value = FormatHandler.FormatNumber20(all_total_TurnOver);

        wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
        all_total_margin = (all_total_TurnOver == 0) ? 0 : ((all_total_lineTotal / all_total_TurnOver) * 100);
        wsReport.Cells[currencRow, 6].Value = FormatHandler.FormatNumber20(all_total_margin) + "%";
        if (colorvalue(all_total_margin) == "red")
        {
            wsReport.Cells[currencRow, 6].Style.Font.Color = System.Drawing.Color.Red;
        }
    }

    private string colorvalue(object value)
    {
        if (FormatHandler.GetDouble(value) > 0) return "red";
        return "";
    }

    private void AssignHeaderExportExcel(ref ExcelWorksheet wsReport, ref int currencRow)
    {
        var cellStyle = new Sunplus.Accounting
            .Core.Helper.ExcelGemboxUtils().GetStyleText(System.Drawing.Color.Black, 181, 196, 208, HorizontalAlignmentStyle.Center);
        wsReport.Cells[currencRow, 0].Value = "Bettype";
        wsReport.Cells[currencRow, 0].Style = cellStyle;
        wsReport.Cells[currencRow, 0].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 0].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 0].Style.Font.Size = 200;
        wsReport.Columns[0].Width = 6000;

        wsReport.Cells[currencRow, 1].Value = "WinLoss";
        wsReport.Cells[currencRow, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 1].Style = cellStyle;
        wsReport.Cells[currencRow, 1].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 1].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 1].Style.Font.Size = 200;
        wsReport.Columns[1].Width = 4000;

        wsReport.Cells[currencRow, 2].Value = "Position";
        wsReport.Cells[currencRow, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 2].Style = cellStyle;
        wsReport.Cells[currencRow, 2].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 2].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 2].Style.Font.Size = 200;
        wsReport.Columns[2].Width = 4000;

        wsReport.Cells[currencRow, 3].Value = "Comm";
        wsReport.Cells[currencRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 3].Style = cellStyle;
        wsReport.Cells[currencRow, 3].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 3].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 3].Style.Font.Size = 200;
        wsReport.Columns[3].Width = 4000;

        wsReport.Cells[currencRow, 4].Value = "Line Total";
        wsReport.Cells[currencRow, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 4].Style = cellStyle;
        wsReport.Cells[currencRow, 4].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 4].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 4].Style.Font.Size = 200;
        wsReport.Columns[4].Width = 4000;

        wsReport.Cells[currencRow, 5].Value = "Turnover";
        wsReport.Cells[currencRow, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 5].Style = cellStyle;
        wsReport.Cells[currencRow, 5].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 5].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 5].Style.Font.Size = 200;
        wsReport.Columns[5].Width = 4000;

        wsReport.Cells[currencRow, 6].Value = "Margin";
        wsReport.Cells[currencRow, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[currencRow, 6].Style = cellStyle;
        wsReport.Cells[currencRow, 6].Style.Font.Weight = ExcelFont.BoldWeight;
        wsReport.Cells[currencRow, 6].Style.Font.Name = "Times New Roman";
        wsReport.Cells[currencRow, 6].Style.Font.Size = 200;
        wsReport.Columns[6].Width = 4000;
    }
    #endregion
}
