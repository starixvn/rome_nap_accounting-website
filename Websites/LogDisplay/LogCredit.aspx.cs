using System;
using System.Collections;
using System.Collections.Generic;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;
using Sunplus.Shared.ViewLogLib.Model;
using Sunplus.Shared.ViewLogLib.ServiceLayger;
using System.Configuration;
using DNAide.Core;


public partial class _ViewLog_LogCredit : ViewLogPageBase
{
    List<IPVNBean> ipVNList = null;
    protected override void PageLoadEvent(object sender, EventArgs e)
    {
        DataControl = rptDataLog;
        NoInfoLabel = ltrNoInfo;
        CurrentUrl = "LogCredit.aspx";
        this.PageSize = 70;
        ipVNList = IpManagement.IPVNList;
        if (CastAide.AsString(Request["custname"]).Trim() == "")
            this.content.Style.Value = "display:none";
    }

    protected override IList GetLogData(out int totalPage)
    {
        totalPage = 0;
        List<LogCredit> entityList = new List<LogCredit>();
        try
        {
            entityList = (new LogCreditService()).GetPaged(this.RequestViewLogContext, this.PageIndex, this.PageSize, out totalPage);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        

        return entityList;


    }

    /// <summary>
    /// IPs vietnam cannot show detail info by popup
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    protected string ShowIp(object ip)
    {
        if (!ReferenceEquals(ip, null))
        {

            string ipReplaced = IpManagement.ReplaceVNIP(CastAide.AsString(ip), ipVNList);
            if (string.IsNullOrEmpty(ipReplaced))
                return "";
            else if (ipReplaced.Trim() == WebSettings.IPVNReplace.Trim())
                return WebSettings.IPVNReplace;
            else
                return string.Format("<a href='{0}' target=_blank>{1}</a>", string.Format(ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", CastAide.AsString(ip))), CastAide.AsString(ip));

            
            

        }

        return string.Empty;
    }


   
}
