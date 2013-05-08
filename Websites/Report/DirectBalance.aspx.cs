using System;
using System.Web;
using DNAide.Web;
using System.Text;
using DNAide.Core;
using Sunplus.Accounting.Core.Cache;

public partial class Report_DirectBalance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        
        int isExport = CastAide.AsInt32(Request["isexport"], 0);
        this.ReportTitle.Text = Resources.Language.DirectBalance;
        reportHeader.Text = getHeaderLink();

        if (isExport == 1)
        {
            string strContent = string.Empty;
            

            string display = CastAide.AsString(Request["display"]);
            string fileName = "DirectBalance";
            if (display == "member")
            {
                fileName = fileName + "_member";
            }
            else if (display == "agent")
            {
                fileName = fileName + "_agent";
            }
            else if (display == "master")
            {
                fileName = fileName + "_master";
            }


            //apply cache for direct balance
            if (!string.IsNullOrEmpty(ReportCache.REPORT_DIRECT_BAL))
                strContent = ReportCache.REPORT_DIRECT_BAL;
            else
            {
                IServiceDirectBalance direct = new ServiceDirectBalance();
                strContent = direct.GetDirectBalance(display);
            }
            ExportToExcel(Response, strContent, fileName);
        }
        else
        {
            string display = CastAide.AsString(Request["display"]);
            if  (display == "member" || display == "master" || display == "agent")
            {
                reportHeader.Text = reportHeader.Text + "<script>ViewBalance('" + display  + "');</script>";
            }
        }
    }
    private string getHeaderLink()
    {

        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("DirectBalance/Header.htm");
        //assign header name
        TemplateHandler.Assign(ref tpl, "{DirectBalance}", Resources.Language.DirectBalance);
        TemplateHandler.Assign(ref tpl, "{Refresh}", Resources.Language.refresh);
        TemplateHandler.Assign(ref tpl, "{DirectMaster}", Resources.Language.DirectMaster);
        TemplateHandler.Assign(ref tpl, "{DirectAgent}", Resources.Language.DirectAgent);
        TemplateHandler.Assign(ref tpl, "{DirectMember}", Resources.Language.DirectMember);
        return tpl;
    }
    
    #region ExportToExcel
    
    public void ExportToExcel(HttpResponse Response, string data, string filename)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.xls";

        StringBuilder content = new StringBuilder();
        content.AppendLine(ReportHTML.CSSExcelReport());
        content.AppendLine(data);
        content.Append("<table><tr><td class='tdhidden'></td></tr></table>");

        content.Replace("href=\"", "href=\"#");
        content.Replace("href='", "href='#");

        Response.Write(content.ToString());
        Response.End();
    }

    #endregion
}
