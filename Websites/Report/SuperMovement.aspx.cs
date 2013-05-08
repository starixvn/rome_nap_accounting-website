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
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils;

public partial class Report_SuperMovement : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isExportExcel = false;
        string reportContent = string.Empty;
        //title
        this.ReportTitle.Text = Resources.Language.SuperBalanceMovement;

        if (CastAide.AsInt32(Request["isexport"], 0) == 1)
            isExportExcel = true;


        this.reportHeader.Text = loadHeader();
        

        //reportContent = buildReport();

        //this.reportContent.Text = reportContent;

        if (isExportExcel)
        {
            int balanceType = CastAide.AsInt32(Request["balancetype"], 0);
            if (balanceType == 0)
            {
                if (!string.IsNullOrEmpty(ReportCache.REPORT_SUPER_MOVEMENT_BASE))
                    reportContent = ReportCache.REPORT_SUPER_MOVEMENT_BASE;
                else
                    reportContent = ReportHTML.GetSuperMovement(balanceType);

                ExportToExcel(Response, reportContent, "SuperBaseBalanceMovement");
            }
            else
            {
                if (!string.IsNullOrEmpty(ReportCache.REPORT_SUPER_MOVEMENT_ORIGINAL))
                    reportContent = ReportCache.REPORT_SUPER_MOVEMENT_ORIGINAL;
                else
                    reportContent = ReportHTML.GetSuperMovement(balanceType);

                ExportToExcel(Response, reportContent, "SuperOriginalBalanceMovement");
            }
        }

    }

    string loadHeader()
    {
        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(@"DirectBalance\BalanceMovement.htm");
        tpl = TemplateHandler.CopyBlock(tpl, "ReportHead");
        TemplateHandler.Assign(ref tpl, "{BalanceMovement}", Resources.Language.SuperBalanceMovement);
        TemplateHandler.Assign(ref tpl, "{OriginalBal}", Resources.Language.OriginalBalance);
        TemplateHandler.Assign(ref tpl, "{BaseBal}", Resources.Language.BaseBalance);
        TemplateHandler.Assign(ref tpl, "{ExcelExport}", Resources.Language.ExcelExport);
        TemplateHandler.Assign(ref tpl, "{Refresh}", Resources.Language.refresh);

        tpl = tpl + "<script>GetDataReport();</script>" ;

        return tpl;
    }

    
    public static void ExportToExcel(HttpResponse Response, string data, string filename)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.xls";

        StringBuilder content = new StringBuilder();

        content.AppendLine("<style>");
        content.AppendLine("td,table{border-color:gray;}");
        content.AppendLine(".report-header { background:#536b81; color:#ffffff; font-weight:bold; text-align:center; }");
        content.AppendLine(".report-footer{background:#ccffcc;font-weight:bold;font-size:11px;}");
        content.AppendLine("tr.bg-even{background:#DFEAFF;}");
        content.AppendLine("tr.bg-odd{background:#F9F9F7;}");
        content.AppendLine("tr.bg-total{background:#357EC7;color:#fff;font-weight:bold;text-align:center;}");
        content.AppendLine("td{font-family:Tahoma; font-size:11px;}");
        content.AppendLine(".ralign{text-align:right;font-size:11px;}");
        content.AppendLine(".lalign{text-align:left;font-size:11px;}");
        content.AppendLine(".calign{text-align:center;font-size:11px;}");
        content.AppendLine(".cl1{background-color: #FFDDFF;}");
        content.AppendLine(".cl2{background-color: #ffffee;}");
        


        content.AppendLine("</style>");

        string strRpt = data;
        TemplateHandler.RemoveBlock(ref strRpt, "not-excel-export");
        TemplateHandler.ReplaceBlock(ref strRpt, "excel-replace", "<table border='1' cellspacing='0' cellpadding='0' ");

        content.AppendLine(strRpt);
        content.Append("<table><tr><td></td></tr></table>");

        Response.Write(content.ToString());
        Response.End();
    }
}
