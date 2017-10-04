using DNAide.Core;
using Fanex.CustomerSetting.Module.Core.Criteria;
using Fanex.CustomerSetting.Module.Core.Services.Impl;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using System;
using System.Collections;
using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Domain;
using Fanex.Tarus.Security.Services;
using Sunplus.Accounting.Business.Security;

public partial class CustInfo : PageBase
{
    public bool isBA, isEnabled;
    public string arrAbnomalBet = "";
    public string username = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        username = System.Web.HttpUtility.UrlDecode(Request.QueryString["username"]);
        if (string.IsNullOrEmpty(username))
        {
            HtmlHolder.Text = string.Empty;
            return;
        }

        var serviceBase = new ServiceBase();
        var checkCustomerBelongToSpecialSiteResult = serviceBase.CheckCustomerBelongToSpecialSite(new CheckCustomerIsSpecialSiteCriteria() { UserName = Request["username"] });
        if (checkCustomerBelongToSpecialSiteResult.IsSpecialHeadline)
        {
            if (!Permissions.HasPermission(Enumerations.Functions.AccessSpecialSites))
            {
                HtmlHolder.Text = AlertMessage.GetMsg(GetGlobalResourceObject(Constants.Resources.LanguageResources, "DontHavePermission").ToString(), AlertMessage.MsgType.Error);
                return;
            }
        }

        var checkSpecialSitePermission = serviceBase.CheckSpecialSitePermission(
                   new CheckSpecialSitePermissionCriteria()
                   {
                       UserID = AppSession.AccId,
                       CustName = username
                   });

        if (checkSpecialSitePermission.ErrorCode > 0)
        {
            HtmlHolder.Text = AlertMessage.GetMsg(checkSpecialSitePermission.ErrorMessage, AlertMessage.MsgType.Error);
            return;
        }

        CustomerManager customer = new CustomerManager();
        try
        {
            Hashtable hash = customer.GetHtmlCustInfoContent(username.Trim());
            //HtmlHolder.Text = customer.GetHtmlCustInfoContent(username.Trim());
            HtmlHolder.Text = CastAide.AsString(hash["strcontent"]);
            isBA = hash.ContainsKey("isba") ? CastAide.AsBoolean(hash["isba"]) : false;
        }
        catch (Exception)
        {
            HtmlHolder.Text = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
        }
    }
}

