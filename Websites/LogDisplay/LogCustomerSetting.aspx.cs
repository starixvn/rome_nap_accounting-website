using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;
using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Model;
using Sunplus.Shared.ViewLogLib.ServiceLayger;
using DNAide.Core;
public partial class _ViewLog_LogCustomerSetting : ViewLogPageBase
{
    private const string XsltPath = "~/LogDisplay/Resources/XsltTemplate";

    protected override void PageLoadEvent(object sender, EventArgs e)
    {
        DataControl = rptDataLog;
        NoInfoLabel = ltrNoInfo;
        CurrentUrl = "LogCustomerSetting.aspx";
        if (CastAide.AsString(Request["custname"]).Trim() == "")
            this.content.Style.Value = "display:none";
    }

    protected override IList GetLogData(out int totalPage)
    {
        totalPage = 0;
        //this.RequestViewLogContext.IsViewStatus = true; => wrong command why?
        var requestViewLogContext = this.RequestViewLogContext;
        var entityList = new List<LogRunSP>();
        try
        {
            if (requestViewLogContext != null)
            {
                requestViewLogContext.ActionTypeId = LogActionTypeId;

                entityList = (new LogRunSpService()).GetPaged(requestViewLogContext, this.PageIndex, this.PageSize,
                                                              out totalPage);
            }
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        

        return entityList;
    }

    private int LogActionTypeId
    {
        get
        {
            return int.Parse(GetQueryStringValue("actiontype", "-1"));
        }
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

            var actionId = CastAide.AsInt32(objActionId);
            var logId = CastAide.AsInt32(objLogId);
            var htmlContent = !string.IsNullOrEmpty(xml) ? AssignTemplateLanguage(Utility.GetHtmlContent(xml, actionId, XsltPath), actionId) : string.Empty;
            //try
            //{
                htmlContent = IpManagement.FormatIPs(htmlContent);
            //}catch
            //{
            //}
            return htmlContent.Replace(TbodyLogId, logId.ToString());
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return ex.Message + " " + HttpUtility.HtmlEncode(xml);
        }
    }
}
