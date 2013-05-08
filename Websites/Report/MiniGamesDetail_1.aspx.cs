using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using DNAide.Data;
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_MiniGamesDetail : System.Web.UI.Page
{
    public StringBuilder html = new StringBuilder();
    public string displayExport = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;      
        DateTime  wldate = CastAide.AsDateTime(Request["wldate"]);
        html.Append(ReportWLMiniGames(wldate));

        if (Request["type"] == "Direct")
        {
            string script = "<script type ='text/javascript'>";
            script += "parent.showProgressBar(false);";
            script += "</script>";
            Response.Write(script);
            displayExport = "none";
        }
    }

    
    string ReportWLMiniGames(DateTime wldate)
    {
        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/MiniGamesWLDetail.htm");
        if (Request["type"] == "Direct")
            TemplateHandler.RemoveBlock(ref tpl, "");
        string tplContent = TemplateHandler.CopyBlock(tpl, "content");
        string tplTotal = TemplateHandler.CopyBlock(tpl, "Total");
        TemplateHandler.RemoveBlock(ref tpl, "Total");

        StringBuilder rows = new StringBuilder();

        try
        {
            Title = Sunplus.Accounting.Business.Report.Reports.Title;           

            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = Request["db"];
            string WL = Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            DbResult rs = null;

            DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
            if (wldate <= movebetsday)
                rs = MatchWinLostReport.WinlostMiniGames_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostMiniGames_Details(wldate, db);
           

           DataTable dt = rs.ResultSet.Tables[0];
           if (dt.Rows.Count == 0)
           {
               string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
               TemplateHandler.Assign(ref norow, "{tdno}", "10");
               TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
               TemplateHandler.ReplaceBlock(ref tpl, "content", norow);
           }
           else
           {
               #region Create Data for testing
               /*
            DataTable dt = new DataTable();
            DataRow drNewRow = dt.NewRow();
            dt.Columns.Add("Bettype", typeof(int));
            dt.Columns.Add("CurrencyName", typeof(string));
            dt.Columns.Add("winlost", typeof(decimal));
            dt.Columns.Add("pos", typeof(decimal));
            dt.Columns.Add("comm", typeof(decimal));
            dt.Columns.Add("linetotal", typeof(decimal));            

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 71;
            drNewRow["CurrencyName"] = "UUS";
            drNewRow["winlost"] = 300;
            drNewRow["pos"] = -270;
            drNewRow["comm"] = 0.3;
            drNewRow["linetotal"] = 30.3;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 71;
            drNewRow["CurrencyName"] = "HK$";
            drNewRow["winlost"] = 30;
            drNewRow["pos"] = -15;
            drNewRow["comm"] = 5.6;
            drNewRow["linetotal"] = 20.6;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 72;
            drNewRow["CurrencyName"] = "UUS";
            drNewRow["winlost"] = 100;
            drNewRow["pos"] = -90;
            drNewRow["comm"] = 1.5;
            drNewRow["linetotal"] = 11.5;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 72;
            drNewRow["CurrencyName"] = "HK$";
            drNewRow["winlost"] = -1500;
            drNewRow["pos"] = -1350;
            drNewRow["comm"] = 1.5;
            drNewRow["linetotal"] = -148.5;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 71;
            drNewRow["CurrencyName"] = "USD";
            drNewRow["winlost"] = -1500;
            drNewRow["pos"] = -1350;
            drNewRow["comm"] = 1.5;
            drNewRow["linetotal"] = -148.5;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 72;
            drNewRow["CurrencyName"] = "RM";
            drNewRow["winlost"] = -1500;
            drNewRow["pos"] = -1350;
            drNewRow["comm"] = 1.5;
            drNewRow["linetotal"] = -148.5;
            dt.Rows.Add(drNewRow);

            drNewRow = dt.NewRow();
            drNewRow["Bettype"] = 71;
            drNewRow["CurrencyName"] = "US$";
            drNewRow["winlost"] = -1500;
            drNewRow["pos"] = -1350;
            drNewRow["comm"] = 1.5;
            drNewRow["linetotal"] = -148.5;
            dt.Rows.Add(drNewRow);
             */
               #endregion

               DataView dv = dt.DefaultView;
               dv.Sort = "Currency asc";
               DataTable dtMiniGames = dv.ToTable();

               int index = 0;
               string oeBGColor = "";
               string condition = "", rowTotal = "";
               ArrayList arrCurency = new ArrayList();
  
               #region foreach
               foreach (DataRow dr in dtMiniGames.Rows)
               {
                   index++;
                   oeBGColor = (oeBGColor == "colGroup1") ? "" : "colGroup1";
                   string row = tplContent;
                   string currName = CacheUtils.GetExchangeName(CastAide.AsInt32(dr["currency"]));

                   if (!arrCurency.Contains(currName))
                       arrCurency.Add(currName);

                   TemplateHandler.Assign(ref row, "report-row", oeBGColor);
                   TemplateHandler.Assign(ref row, "align", "c");
                   TemplateHandler.Assign(ref row, "Currency", currName);
                   TemplateHandler.Assign(ref row, "turnover", NumberFormat.DecFormat(dr["turnover"], 2));
                   TemplateHandler.Assign(ref row, "winlost", NumberFormat.DecFormat(dr["winlost"], 2));
                   TemplateHandler.Assign(ref row, "pos", NumberFormat.DecFormat(dr["pos"], 2));
                   TemplateHandler.Assign(ref row, "comm", NumberFormat.DecFormat(dr["comm"], 2));
                   TemplateHandler.Assign(ref row, "linetotal", NumberFormat.DecFormat(dr["linetotal"], 2));
                   rows.Append(row);
               }
               #endregion

               #region Grand Total
               rowTotal = tplContent;
               TemplateHandler.Assign(ref rowTotal, "report-row", "report-footer");
               TemplateHandler.ReplaceBlock(ref rowTotal, "Currency", "<td align='center'>Grand Total</td>");
               condition = "";
               TotalRows(ref rowTotal, condition, dt);
               rows.Append(rowTotal);
               #endregion

               TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
           }
            if (Request["IsExport"] == "1") Sunplus.Accounting.Business.Report.Reports.ExportToExcel(tpl, "MiniGames_" + wldate.ToShortDateString());
        }
        catch (Exception ex)
        {
            tpl = "<font color='red'><b>" + ex.Message + "</b></font>";
        }
        return tpl;
    }

    #region TotalRows
    void TotalRows(ref string row, string condition, DataTable dt)
    {
        TemplateHandler.Assign(ref row, "align", "c");
        TemplateHandler.Assign(ref row, "Currency", "Mini Game Total");
        TemplateHandler.Assign(ref row, "turnover", NumberFormat.DecFormat(dt.Compute("sum(turnover)", condition), 2));
        TemplateHandler.Assign(ref row, "winlost", NumberFormat.DecFormat(dt.Compute("sum(winlost)", condition), 2));
        TemplateHandler.Assign(ref row, "pos", NumberFormat.DecFormat(dt.Compute("sum(pos)", condition), 2));
        TemplateHandler.Assign(ref row, "comm", NumberFormat.DecFormat(dt.Compute("sum(comm)", condition), 2));
        TemplateHandler.Assign(ref row, "linetotal", NumberFormat.DecFormat(dt.Compute("sum(linetotal)", condition), 2));
    }
    #endregion


}
