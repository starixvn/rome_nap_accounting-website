<%@ WebHandler Language="C#" Class="OnlineUpd" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL;



public class OnlineUpd : IHttpHandler , IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            int onlineId = AppSession.OnAccId;
            int onlineStatus = 0;
            context.Response.Write("<html><head></head><body>");
            string appPath = context.Request.ApplicationPath;
            if (appPath != "/") appPath += "/";
            if (onlineId != 0 && AccCommon.IsEnabledRecheckin)
                onlineStatus = UserController.OnlineUserUpdate(onlineId);
            if (AccCommon.IsLocalTest)
            {
                context.Response.Write("</body></html>");
            }

            var alertMessage = string.Empty;
            //Status 1: Kick out
            if (onlineStatus == 1)
            {
                alertMessage = "Another session has been logged in or your session is expired. Forced to logout!";
            }
            // Status 2 Permission revolked
            else if(onlineStatus == 2)
            {
                alertMessage = "Your Account was Expired, Please contact your supervisor";
            }
            
            context.Response.Write("<script type='text/javascript'>alert('" + alertMessage + "');top.location.href='" + appPath + "PreLogIn/Logout.ashx';</script>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);

            context.Response.Write("<html><head></head><body><script type='text/javascript'>top.location.href='../PreLogIn/LogOut.ashx';</script></body></html>");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
