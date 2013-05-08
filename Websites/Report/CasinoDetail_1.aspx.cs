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
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_CasinoDetail : PageBase
{

    public StringBuilder html = new StringBuilder();
    public string displayExport = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;      
        string bettype = Request["bettype"];
        DateTime  wldate = CastAide.AsDateTime(Request["wldate"]);
        html.Append(ReportWLCasino(bettype, wldate));

        if (Request["type"] == "Direct")
        {
            string script = "<script type ='text/javascript'>";
            script += "parent.showProgressBar(false);";
            script += "</script>";
            Response.Write(script);
            displayExport = "none";
        }
    }

    
    string ReportWLCasino(string bettype, DateTime wldate)
    {
        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/CasinoWLDetail.htm");
        if (Request["type"] == "Direct")
            TemplateHandler.RemoveBlock(ref tpl, "");
        string tplContent = TemplateHandler.CopyBlock(tpl, "content");
        string tplTotal = TemplateHandler.CopyBlock(tpl, "Total");
        TemplateHandler.RemoveBlock(ref tpl, "Total");

        StringBuilder rows = new StringBuilder();

        try
        {
            Title = Sunplus.Accounting.Business.Report.Reports.Title;           

            TemplateHandler.Assign(ref tpl, "bettype", bettype);
            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = Request["db"];
            string WL = Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            DbResult rs = null; 

            DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;

            if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                rs = MatchWinLostReport.WinlostCasino_Details(wldate, db);
            else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                rs = MatchWinLostReport.WinlostCasino_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostCasino_Details_WH(wldate);

            //if (wldate <= movebetsday)
            //    rs = MatchWinLostReport.WinlostCasino_Details_14(wldate);
            //else
            //    rs = MatchWinLostReport.WinlostCasino_Details(wldate, db);
           

           DataTable dt = rs.ResultSet.Tables[0];
           //if (dt.Rows.Count == 0)
           //{
           //    string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
           //    TemplateHandler.Assign(ref norow, "{tdno}", "10");
           //    TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
           //    TemplateHandler.ReplaceBlock(ref tpl, "content", norow);
           //}
           //else
           //{
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
           dv.Sort = "Bettype asc, Currency asc";
           DataTable dtCasino = dv.ToTable();

           int Bettype_current = 0, Bettype_previous = 0;
           int index = 0;
           string condition = "", rowTotal = "", row_even_odd = "#dfeaff";
           ArrayList arrCurency = new ArrayList();
           //   for (int j = 0; j <= 15; j++)
           {
               #region foreach
               foreach (DataRow dr in dtCasino.Rows)
               {
                   index++;
                   string row = tplContent;
                   Bettype_current = CastAide.AsInt32(dr["Bettype"]);

                   if (!arrCurency.Contains(dr["CurrencyName"]))
                       arrCurency.Add(CastAide.AsString(dr["CurrencyName"]));

                   if (Bettype_current != Bettype_previous)
                   {
                       if (index > 1)
                       {
                           rowTotal = tplContent;
                           TemplateHandler.RemoveBlock(ref rowTotal, "Bettype");
                           TemplateHandler.Assign(ref rowTotal, "bgcolor", "bgcolor='" + row_even_odd + "'");
                           rows.Replace("{rowspan}", index.ToString());
                           condition = "bettype='" + Bettype_previous + "'";
                           TemplateHandler.Assign(ref rowTotal, "report-row", " report-footer");
                           TotalRows(ref rowTotal, condition, Bettype_previous, dt);
                           rows.Append(rowTotal);
                       }

                       index = 1;
                       row_even_odd = (row_even_odd == "#dfeaff") ? "" : "#dfeaff";
                       Bettype_previous = Bettype_current;
                   }

                   if (index > 1) TemplateHandler.RemoveBlock(ref row, "Bettype");

                   TemplateHandler.Assign(ref row, "bgcolor", "bgcolor='" + row_even_odd + "'");
                   TemplateHandler.Assign(ref row, "report-row", row_even_odd);
                   TemplateHandler.Assign(ref row, "align", "c");
                   TemplateHandler.Assign(ref row, "Bettype", GetBetypeName(Bettype_current));
                   TemplateHandler.Assign(ref row, "Currency", dr["CurrencyName"]);
                   TemplateHandler.Assign(ref row, "turnover", NumberFormat.DecFormat(dr["turnover"], 2));
                   TemplateHandler.Assign(ref row, "winlost", NumberFormat.DecFormat(dr["winlost"], 2));
                   TemplateHandler.Assign(ref row, "pos", NumberFormat.DecFormat(dr["pos"], 2));
                   TemplateHandler.Assign(ref row, "comm", NumberFormat.DecFormat(dr["comm"], 2));
                   TemplateHandler.Assign(ref row, "linetotal", NumberFormat.DecFormat(dr["linetotal"], 2));
                   rows.Append(row);
               }
               #endregion
           }

           #region Toltal of bettype
           if (Bettype_previous > 0)
           {
               rowTotal = tplContent;
               TemplateHandler.Assign(ref rowTotal, "bgcolor", "");
               TemplateHandler.Assign(ref rowTotal, "report-row", "b");
               TemplateHandler.RemoveBlock(ref rowTotal, "Bettype");
               rows.Replace("{rowspan}", (index + 1).ToString());
               condition = "bettype='" + Bettype_previous + "'";
               TotalRows(ref rowTotal, condition, Bettype_previous, dt);
               rows.Append(rowTotal);
           }
           #endregion

           #region Total by Currency
           index = 0;
           rows.Append("<tr><td colspan='7'>&nbsp;</td></tr>");
           row_even_odd = "";
           foreach (string currency in arrCurency)
           {
               index++;
               rowTotal = tplContent;
               TemplateHandler.Assign(ref rowTotal, "Bettype", "Total");

               if (index > 1) TemplateHandler.RemoveBlock(ref rowTotal, "Bettype");

               TemplateHandler.Assign(ref rowTotal, "report-row", "b");

               if (row_even_odd.Equals("#e8ebd3"))
               {
                   row_even_odd = "#dfeaff";
               }
               else
               {
                   row_even_odd = "#e8ebd3";
               }
               TemplateHandler.Assign(ref rowTotal, "bgcolor", "style='background-color:" + row_even_odd + "'");
               TemplateHandler.Assign(ref rowTotal, "Currency", currency);
               condition = "currencyname='" + currency + "'";
               TotalRows(ref rowTotal, condition, Bettype_previous, dt);
               rows.Append(rowTotal);
           }
           rows.Replace("{rowspan}", index.ToString());
           #endregion

           #region Grand Total
           rowTotal = tplContent;
           row_even_odd = "#d6efba";
           TemplateHandler.Assign(ref rowTotal, "report-row", "footer1");
           TemplateHandler.Assign(ref rowTotal, "bgcolor", "bgcolor='" + row_even_odd + "'");
           TemplateHandler.RemoveBlock(ref rowTotal, "Bettype");
           TemplateHandler.ReplaceBlock(ref rowTotal, "Currency", "<td colspan='2' align='center'>Grand Total</td>");
           condition = "";
           TotalRows(ref rowTotal, condition, Bettype_previous, dt);
           rows.Append(rowTotal);
           #endregion

           TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());

           if (Request["IsExport"] == "1") Sunplus.Accounting.Business.Report.Reports.ExportToExcel(tpl, bettype + "_" + wldate.ToShortDateString());
        }
        catch (Exception ex)
        {
            tpl = "<font color='red'><b>" + ex.Message + "</b></font>";
        }
        return tpl;
    }

    #region GetBetypeName
    string GetBetypeName(int Bettype)
    {
        switch (Bettype)
        {
            case 71: return "Casino";
            case 72: return "P2P";
            default: return "Unknow";
        }
    }
    #endregion

    #region TotalRows
    void TotalRows(ref string row, string condition, int Bettype, DataTable dt)
    {
        TemplateHandler.Assign(ref row, "align", "c");
        TemplateHandler.Assign(ref row, "Currency", GetBetypeName(Bettype)+ " Total");
        TemplateHandler.Assign(ref row, "turnover", NumberFormat.DecFormat(dt.Compute("sum(turnover)", condition), 2));
        TemplateHandler.Assign(ref row, "winlost", NumberFormat.DecFormat(dt.Compute("sum(winlost)", condition), 2));
        TemplateHandler.Assign(ref row, "pos", NumberFormat.DecFormat(dt.Compute("sum(pos)", condition), 2));
        TemplateHandler.Assign(ref row, "comm", NumberFormat.DecFormat(dt.Compute("sum(comm)", condition), 2));
        TemplateHandler.Assign(ref row, "linetotal", NumberFormat.DecFormat(dt.Compute("sum(linetotal)", condition), 2));
    }
    #endregion


}
