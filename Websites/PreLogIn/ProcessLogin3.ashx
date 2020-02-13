<%@ WebHandler Language="C#" Class="ProcessLogin3" %>
using System;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.SessionState;
using Fanex.Common;
using Fanex.CustomerSetting.Module.Core;
using Fanex.Data.DNAide;
using Fanex.Nap.SSO.Repositories;
using Fanex.Nap.SSO.Criteria;
using Fanex.Tarus.Infrastructure.Configurations;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IBCDLL;

public class ProcessLogin3 : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        try
        {
            var custId = Converter.ToInt(context.Request["uid"]);
            var logId = Converter.To<long>(context.Request["lid"]);
            var targetCustId = Converter.ToInt(context.Request["targetCustId"]);
            //Set CustID=0 to select by username

            var sessionId = Converter.ToInt(context.Request["sid"]);
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("sessionid", sessionId);
            param.Add("logid", logId);
            param.Add("result", false);
            bool succ = false;

            AppSession.SYSTEM = "accounting";

            DbResult ds = DbHelper.ExecuteDbResult("UserLoginClaimed_out", param);
            if (Convert.ToBoolean(ds.Outputs["result"]))
            {
                param.Clear();
                param.Add("custid", custId);

                ds = DbHelper.ExecuteDbResult("Con_AdminSelByCustID", param);

                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    string username = ds.ResultSet.Tables[0].Rows[0]["username"] + "";
                    int custIdInt = Convert.ToInt32(custId);

                    int roleId = Convert.ToInt32(ds.ResultSet.Tables[0].Rows[0]["roleid"]);
                    string pwd = ds.ResultSet.Tables[0].Rows[0]["userpwd"] + "";
                    int onUserId = UserController.AccLoginAdminOnuser(custIdInt, Sunplus.Accounting.Core.Configurations.WebSettings.ServerId, context.Request.UserHostAddress);
                    FormsAuthentication.SetAuthCookie(username, false);
                    AppSession.Username = username;//context.Session["Username"] = username;
                    AppSession.AccUserName = username;
                    AppSession.AccId = custIdInt;//context.Session["AccId"] = custIdInt;
                    AppSession.CurrentSystemNAME = Enumerations.AccountingSystem.Accounting;
                    AppSession.SYSTEM = Enumerations.AccountingSystem.Accounting.ToString();
                    AppSession.OnAccId = onUserId;//context.Session["OnAccId"] = onUserId;
                    AppSession.RoleID = roleId;//context.Session["roleId"] = roleId;
                    AppSession.AccPwd = pwd;
                    AuthManager.AssignRoles(custIdInt);
                    succ = true;

                    IMenuManager menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
                    menuService.GetUserMenu();

                    bool isShowNap = (new LoginRepository()).CheckUserSiteAccess(new CheckUserSiteAccessCriteria());
                    if (isShowNap)
                    {
                        AppSession.UserMenu.Add(new Sunplus.Accounting.Entities.MenuBO
                        {
                            Id = 0,
                            Name = "Nap",
                            Url = System.Configuration.ConfigurationManager.AppSettings["NapHomeSSOController"],
                            System = "accounting",
                            ParentId = 0,
                            FunctionName = "",
                            Target = "_blank",
                            IsHide = false
                        });
                    }

                    // Progressive Bonus
                    Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminId = custIdInt;
                    Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminUsername = username;
                }
            }

            if (succ)
            {
                CustomerService customerService = new CustomerService();
                var dsCustAllInfo = customerService.GetCustomerInfo(targetCustId.ToString(), AppSession.SYSTEM, 1);

                int tableCount = dsCustAllInfo != null ? dsCustAllInfo.ResultSet.Tables.Count : 0;
                if (dsCustAllInfo == null || tableCount < 5)
                {
                    context.Response.Write("Target Cust Id is not existed.");
                }
                else
                {
                    var isCredit = Utils.ToBoolean(dsCustAllInfo.ResultSet.Tables[0].Rows[0]["credit"]);
                    var roleid = Utils.ToInt32(dsCustAllInfo.ResultSet.Tables[0].Rows[0]["roleid"]);
                    var site = roleid != 1 ? Utils.ToString(dsCustAllInfo.ResultSet.Tables[0].Rows[0]["site"]) : Utils.ToString(dsCustAllInfo.ResultSet.Tables[0].Rows[0]["membersite"]);
                    var isCN88 = AppSettings.IsCN88SiteName(site);
                    context.Response.Write("<html><head><script type='text/javascript'>\n");
                    context.Response.Write("window.location.href='../TarusBetList/Portal_BetListByCustInfo?custId=" + Converter.ToInt(context.Request["targetCustId"]) + "&isDeposit=" + !isCredit + "&isCN88=" + isCN88 + "';\n");
                    context.Response.Write("\n</script></head><body></body>");
                }
            }
            else
            {
                context.Response.Write("<html><head><script type='text/javascript'>\n");
                context.Response.Write("window.location.href='../ErrorPage.aspx';\n");
                context.Response.Write("\n</script></head><body></body>");
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}