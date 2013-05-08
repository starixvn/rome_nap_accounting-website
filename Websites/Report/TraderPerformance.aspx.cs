using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils.DAL;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Text;
using DNAide.Web;
using IBCUtils;
using IBCUtils.Web;
using DNAide.Core;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_TraderPerformance : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //title
        ReportTitle.Text = Resources.Language.traderPerformance;
        //header 
        reportHeader.Text = loadHeader() + "<script>GetDataReport();</script>";

        string reportContent = string.Empty;
        int isSoccer = ((Request["IsSoccer"] == null) ? 1 : CastAide.AsInt32(Request["IsSoccer"], 1));

        if (CastAide.AsInt32(Request["IsExport"]) == 1)
        {

            if (isSoccer > 1 || isSoccer < 0) isSoccer = 1;
            if (!string.IsNullOrEmpty(ReportCache.REPORT_TRADER))
            {
                reportContent = ReportCache.REPORT_TRADER;
            }
            else
            {                
                reportContent = ReportHTML.GetTraderPerformance(Request["fdate"], Request["tdate"], CastAide.AsString(Request["sortCol"]), CastAide.AsString(Request["order"]), isSoccer);
            }            

            if (isSoccer == 1)
            {
                reportContent = "<b>" + Resources.Language.traderPerformance + " FOR SOCCER FROM " + Request["fdate"] + " TO " + Request["tdate"] + "</b>" + reportContent;
                ExportToExcel(Response, reportContent, "TraderPerformance_Soccer");
            }
            else
            {
                reportContent = "<b>" + Resources.Language.traderPerformance + " FOR OTHER SPORT FROM " + Request["fdate"] + " TO " + Request["tdate"] + "</b>" + reportContent;
                ExportToExcel(Response, reportContent, "TraderPerformance_OtherSport");
            }           

             
        }
       
    }

    private string loadHeader()
    {

        string template = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/TraderPerformancerHeader.htm");
        //template = TemplateHandler.CopyBlock(template, "Header");

        int IsSoccer = ((Request["IsSoccer"] == null) ? 1 : CastAide.AsInt32(Request["IsSoccer"], 1));
        if (IsSoccer < 0 || IsSoccer > 1)
            IsSoccer = 1;
        string clsSoccer = (IsSoccer == 1) ? "" : "lnk";
        string clsOther = (IsSoccer == 0) ? "" : "lnk";
        TemplateHandler.Assign(ref template, "clsSoccer_vl", clsSoccer);
        TemplateHandler.Assign(ref template, "clsOther_vl", clsOther);
        TemplateHandler.Assign(ref template, "fdate_vl", Request["fdate"]);
        TemplateHandler.Assign(ref template, "tdate_vl", Request["tdate"]);
        TemplateHandler.Assign(ref template, "IsSoccer_vl", IsSoccer.ToString());
        TemplateHandler.Assign(ref template, "{sortCol}", Request["sortCol"]);
        TemplateHandler.Assign(ref template, "{order}", Request["order"]);
        TimeSpan spanDay = DateTime.Today - ReportUtils.BalanceUp.MoveBetsDay;
        var totalDay = spanDay.Days;
        TemplateHandler.Assign(ref template, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", Request["fdate"], false, false, 0, totalDay, ""));
        TemplateHandler.Assign(ref template, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", Request["tdate"], true, false, 0, totalDay, ""));


        return template;
    }

    public static void ExportToExcel(HttpResponse Response, string data, string filename)
    {
        StringBuilder content = new StringBuilder();
        content.AppendLine("<html>");
        content.AppendLine("<head>");
        content.AppendLine("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");
        content.AppendLine("</head>");

        content.AppendLine(ReportHTML.CSSExcelReport());

        
        content.Append("</hmtl> ");

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";


        //string strRpt = TemplateHandler.CopyBlock(data, "excel-export");
        TemplateHandler.RemoveBlock(ref data, "not-excel-export");
        TemplateHandler.ReplaceBlock(ref data, "excel-border-fix", "<table border='1' cellspacing='0' cellpadding='0'> ");
        TemplateHandler.ReplaceBlock(ref data, "Excel-Total-Footer", "<tr align='right' class='report-footer'> ");

        content.AppendLine(data);
        content.Append("<table><tr><td class='tdhidden'></td></tr></table>");

        content.Replace("href=\"", "href=\"#");
        content.Replace("href='", "href='#");

        Response.Write(content.ToString());
        Response.End();
    }

    
}
