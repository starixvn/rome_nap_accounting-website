using System;
using System.Xml;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web.Controls;

using System.Collections.Generic;
using Sunplus.Accounting.Core.Web;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

public partial class Report_ReportList : System.Web.UI.Page
{
    /* 1) SS33
    2) SSJOHN
    3) SSJOE 
    4) SSMICHAEL
    5) BChai
      */

    protected void Page_Load(object sender, EventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        string reportListPath = Path.Combine(WebUtils.GetTemplateFolder(), "Report/MainReportList.htm");
        xmlDoc.Load(Server.MapPath("~/"+reportListPath));        
        string Content = xmlDoc.InnerXml;
        TemplateHandler.Assign(ref Content, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", DateTime.Now.ToShortDateString(), false, false, 3, 0, "", "reportlist")); //Phuong.Nguyen <2009-01-03>, show others day of next month
        TemplateHandler.Assign(ref Content, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", DateTime.Now.ToShortDateString(), true, false, 3, 0, "", "reportlist")); //END
        XmlNodeList pageList = xmlDoc.GetElementsByTagName("a");
        var accessPermission = AppSession.PageAccess;

        bool IsActive;
        foreach (XmlNode page in pageList)
        {
            var filePath = page.Attributes["onclick"].Value.ToLower();
            Regex reg = new Regex("Report/.*?(.aspx)", RegexOptions.IgnoreCase);
            filePath = reg.Match(filePath).Value;
            IsActive = page.Attributes["IsActive"].Value == "1";
            if (!IsActive)
                TemplateHandler.RemoveBlock(ref Content, page.Attributes["id"].Value);
            else if (!accessPermission.ContainsKey(filePath)) TemplateHandler.RemoveBlock(ref Content, page.Attributes["id"].Value);           
           
        }
        ReportListHtml.Text = Content;
    }

    private string GetPath(string url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;
        return string.Empty;
    }
}
