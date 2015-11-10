<%@ WebHandler Language="C#" Class="ProcessLogin" %>

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BLL;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Services;
using IBCUtils;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Fanex.Nap.SSO.Repositories;
using Fanex.Nap.SSO.Criteria;
using log4net;



public class ProcessLogin : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string username = context.Request.Form["TxtUsername"];
            string password = context.Request.Form["TxtPassword"];

            context.Response.Write("<html><head><script type='text/javascript'>\n");
            int custId;
            string system;
            int roleId;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                context.Response.Write("parent.signIn.loginError('Username and Password must not be empty!');\n");
            
            string[] deniedUsersAccess = ConfigurationManager.AppSettings["DeniedUsersAccess"].Split(',');
            bool deniedAccess = false;

            for (int i = 0; i < deniedUsersAccess.Length; i++)
            {
                if (username == deniedUsersAccess[i].Trim())
                {
                    deniedAccess = true;
                    break;
                }
            }
            if (deniedAccess)
            {
                context.Session.Clear();
                FormsAuthentication.SignOut();
                context.Response.Write("parent.signIn.loginError('" + Resources.ErrorMsg.DenyLoginAccount + "');\n");
                goto end;
            }
                
            if (LoginFailedHandler.IsLocked(username))
            {
                context.Response.Write(LoginFailedHandler.WriteJS(username));
                goto end;
            }          

            AppSession.CurrentSystemName = "accounting";

            string browserName = context.Request.Browser.Browser + " " + context.Request.Browser.Version;
            IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdmin(ref username, password, context.Request.UserHostAddress, context.Request.UrlReferrer.AbsoluteUri, out custId, out roleId, out system, context.Request.Url.Host, browserName, context.Request.Browser.Platform, "");
            if (error.IsSuccess())
            {
                LoginFailedHandler.LoginSuccess(username);

                FormsAuthentication.SetAuthCookie(username, false);
                AppSession.Username = username;
                AppSession.AccUserName = username;
                AppSession.AccId = custId;

                AppSession.CurrentSystemName = system;

                if (string.IsNullOrEmpty(system))
                {
                    context.Session.Clear();
                    FormsAuthentication.SignOut();
                    context.Response.Write("parent.signIn.loginError('" + Resources.ErrorMsg.DenyLoginAccount + "');\n");
                    goto end;
                }

                #region checking - denied systems
                var deniedGroups = System.Configuration.ConfigurationManager.AppSettings["DeniedAccessGroup"].ToLower().Split(';');

                if (deniedGroups != null && deniedGroups.Length > 0)
                {
                    if(((IList<string>)deniedGroups).Contains(system.ToLower()))
                    {  
                            context.Session.Clear();
                            FormsAuthentication.SignOut();
                            context.Response.Write("parent.signIn.loginError('" + Resources.ErrorMsg.DenyLoginAccount + "');\n");
                            goto end;
                        
                    }
                }
                #endregion checking - denied systems

                if (system.ToUpper() != "ACCOUNTING")
                {
                    system = "EXTERNAL";
                    context.Session.Clear();
                    FormsAuthentication.SignOut();
					context.Response.Clear();
                    context.Response.Write("<script type='text/javascript'>top.location.href='https://www.726872.com/accext';</script>");
					return;
                }

                AppSession.CurrentSystemNAME = Enumerations.ToAccountingSystem(system);
                AppSession.OnAccId = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdminOnuser(custId, WebSettings.ServerId, context.Request.UserHostAddress);
                AppSession.RoleID = roleId;
                AppSession.AccPwd = IBCUtils.Encryption.CfsEnCode(password);

                if (!AuthManager.IsValidRequestUrl(context.Request.Url.Host, AppSession.CurrentSystemName))
                {
                    context.Session.Clear();
                    FormsAuthentication.SignOut();
                    context.Response.Write("parent.signIn.loginError('Invalid Login');\n");
                    goto end;
                }
                AuthManager.AssignRoles(custId);
                if (AppSession.RoleAccess.Count < 1) //limit user have no permission access 
                {
                    context.Session.Clear();
                    FormsAuthentication.SignOut();
                    context.Response.Write("parent.signIn.loginError('" + Resources.ErrorMsg.DenyLoginAccount + "');\n");
                    goto end;
                }

                context.Response.Write("top.location.href='" + FormsAuthentication.DefaultUrl.Replace("https", "http") + "';\n");
                IMenuManager menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
                menuService.GetUserMenu();
                
                bool isShowNap = (new LoginRepository()).CheckUserSiteAccess(new CheckUserSiteAccessCriteria());
                if (isShowNap)
                {
                    AppSession.UserMenu.Add(new Sunplus.Accounting.Entities.MenuBO { 
                        Id=0,
                        Name="Nap",
                        Url = System.Configuration.ConfigurationManager.AppSettings["NapHomeSSOController"],
                        System = "accounting",
                        ParentId = 0,
                        FunctionName = "",
                        Target = "_blank",
                        IsHide = false,
                        SubMenu= null
                    });
                }
				
				// Progressive Bonus
				Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminId = custId;
				Fanex.CustomerSetting.Module.Core.Web.AppSession.AdminUsername = username;
            }
            else
            {
                if (LoginFailedHandler.IsLocked(username))
                {
                    context.Response.Write(LoginFailedHandler.WriteJS(username));
                }
                else
                {
                    LoginFailedHandler.LoginFailed(username, error.ErrorCode);

                    //-----------------------------------------------------------------------

                    if (LoginFailedHandler.IsLocked(username))
                    {
                        context.Response.Write(LoginFailedHandler.WriteJS(username));
                    }
                    else
                    {
                        context.Response.Write("parent.signIn.loginError('" + IBCUtils.Encryption.RemoveSpecialChars(error.ErrorMsg) + "');\n");
                    }
                }
            }
        end:
            context.Response.Write("\n</script></head><body></body>");
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
