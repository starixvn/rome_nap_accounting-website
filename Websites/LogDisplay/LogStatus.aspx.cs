using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Model;
using Sunplus.Shared.ViewLogLib.ServiceLayger;
using DNAide.Core;
using System.Web;


public partial class _ViewLog_LogStatus : ViewLogPageBase
{
    private const string XsltPath = "~/LogDisplay/Resources/XsltTemplate";

    protected override void PageLoadEvent(object sender, EventArgs e)
    {
        DataControl = rptDataLog;
        NoInfoLabel = ltrNoInfo;
        CurrentUrl = "LogStatus.aspx";
        if (CastAide.AsString(Request["custname"]).Trim() == "")
            this.content.Style.Value = "display:none";
    }

    protected override IList GetLogData(out int totalPage)
    {
        totalPage = 0;
        var entityList = new List<LogRunSP>();
        try
        {
            var requestViewLogContext = this.RequestViewLogContext;
            if (requestViewLogContext != null)
            {
                requestViewLogContext.IsViewStatus = true;
                entityList = (new LogRunSpService()).GetPaged(requestViewLogContext, this.PageIndex,
                                                                             this.PageSize, out totalPage);
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        

        return entityList;
    }

   


    /// <summary>
    /// Transform xml to html using xslt
    /// </summary>
    /// <param name="xml">xml content</param>
    /// <param name="actionId">actionid to get approriated xslt file</param>
    /// <returns>html string</returns>
    protected string GetContentHtml(object objXml, object objActionId, object objLogId)
    {
        string xml = CastAide.AsString(objXml);
        try
        {
            int actionId = CastAide.AsInt32(objActionId);
            int logId = CastAide.AsInt32(objLogId);
            string htmlContent = !string.IsNullOrEmpty(xml) ? AssignTemplateLanguage(Utility.GetHtmlContent(xml, actionId, XsltPath), actionId) : string.Empty;
            htmlContent = IpManagement.FormatIPs(htmlContent);
            return htmlContent.Replace(TbodyLogId, logId.ToString());
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return ex.Message + " " + HttpUtility.HtmlEncode(xml);
        }
    }
}

