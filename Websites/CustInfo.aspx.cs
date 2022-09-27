using DNAide.Core;
using Fanex.CustomerSetting.Module.Core.Criteria;
using Fanex.CustomerSetting.Module.Core.Services.Impl;
using Fanex.Tarus.Security.Services;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using System;
using System.Collections;
using System.Linq;

public partial class CustInfo : PageBase
{
    private const byte IsAllowSearchByCustomerId = 1;

    public bool isEnabled;
    public string arrAbnomalBet = string.Empty;
    public string username = string.Empty;
    private ICustomerAuthorizationService customerAuthorizationService = new CustomerAuthorizationService();

    protected void Page_Load(object sender, EventArgs e)
    {
        username = System.Web.HttpUtility.HtmlEncode(Request.QueryString["username"]);

        if (string.IsNullOrEmpty(username))
        {
            HtmlHolder.Text = string.Empty;
            return;
        }

        if (IsNotFinanceUserName(username))
        {
            var serviceBase = new ServiceBase();
            var checkCustomerBelongToSpecialSiteResult = serviceBase.CheckCustomerBelongToSpecialSite(new CheckCustomerIsSpecialSiteCriteria
            {
                UserName = username,
                CheckCustId = IsAllowSearchByCustomerId
            });

            if (checkCustomerBelongToSpecialSiteResult.IsSpecialHeadline &&
                !Permissions.HasPermission(Enumerations.Functions.AccessSpecialSites))
            {
                HtmlHolder.Text = AlertMessage.GetMsg(GetGlobalResourceObject(Constants.Resources.LanguageResources, "DontHavePermission").ToString(), AlertMessage.MsgType.Error);
                return;
            }

            var checkSpecialSitePermission = serviceBase.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria
            {
                UserID = AppSession.AccId,
                CustName = username,
                CheckCustId = IsAllowSearchByCustomerId
            });

            if (checkSpecialSitePermission.ErrorCode > 0)
            {
                HtmlHolder.Text = AlertMessage.GetMsg(checkSpecialSitePermission.ErrorMessage, AlertMessage.MsgType.Error);
                return;
            }
        }

        CustomerManager customer = new CustomerManager();

        try
        {
            Hashtable hash = customer.GetHtmlCustInfoContent(username.Trim(), IsAllowSearchByCustomerId);
            HtmlHolder.Text = CastAide.AsString(hash["strcontent"]);
        }
        catch (Exception)
        {
            HtmlHolder.Text = AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
        }
    }

    private bool IsNotFinanceUserName(string userName)
    {
        var customerType = customerAuthorizationService.GetCustomerTypes(userName);
        return !customerType.Any(c => c.IsInternal);
    }
}

