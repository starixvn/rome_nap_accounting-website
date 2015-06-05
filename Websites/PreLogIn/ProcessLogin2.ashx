<%@ WebHandler Language="C#" Class="ProcessLogin2" %>
using System;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.SessionState;
using Fanex.Common;
using Fanex.Data.DNAide;
using Fanex.Nap.SSO.Repositories;
using Fanex.Nap.SSO.Criteria;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Accounting.Services;
using Sunplus.Core.LoggingLib;

public class ProcessLogin2 : IHttpHandler,IRequiresSessionState {    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";        
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Write("<html><head><script type='text/javascript'>\n");
        try 
        {
            var custId = Converter.ToInt(context.Request["uid"]);
            var logId = Converter.To<long>(context.Request["lid"]);
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

                ds = DbHelper.ExecuteDbResult("Acc_Con_AdminSelByCustID", param);

                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    string username = ds.ResultSet.Tables[0].Rows[0]["username"] + "";
                    int custIdInt = Convert.ToInt32(custId);

                    int roleId = Convert.ToInt32(ds.ResultSet.Tables[0].Rows[0]["roleid"]);
                    string pwd = ds.ResultSet.Tables[0].Rows[0]["userpwd"] + "";
                    int onUserId = UserController.AccLoginAdminOnuser(custIdInt, WebSettings.ServerId, context.Request.UserHostAddress);
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
                    menuService.GetReportMenu("ACC-REPORT");
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
                            IsHide = false,
                            SubMenu = null
                        });
                    }

                    // Progressive Bonus
                    Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminId = custIdInt;
                    Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminUsername = username;
                    Fanex.CustomerSetting.Module.Core.Web.AppSession.FromSystem = 3;
                }
            }
            
            if (succ)
            {
                context.Response.Write("top.location.href='" + FormsAuthentication.DefaultUrl + (!String.IsNullOrEmpty(context.Request.QueryString["uname"]) ? "?uname=" + Converter.ToString2(context.Request.QueryString["uname"]) : string.Empty) + "';\n");
            }
            else
            {
                context.Response.Write("top.location.href='" + FormsAuthentication.LoginUrl + "';\n");
            }           
        }
        catch (Exception ex)
        {
             Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
                
        context.Response.Write("\n</script></head><body></body>");        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}