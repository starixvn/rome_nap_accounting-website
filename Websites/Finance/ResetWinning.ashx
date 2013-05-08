<%@ WebHandler Language="C#" Class="ResetWinning" %>

using System;
using System.Web;
//using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Core.LoggingLib;
using System.Web.SessionState;

using DNAide.Core;
using Sunplus.Accounting.Core.Common;

public class ResetWinning : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";

        if (!Permissions.GetHostPermission<Enumerations.Functions.FinancialManagement>(Enumerations.Functions.FinancialManagement.ResetWinning))
        {
            context.Response.Write(AlertMessage.GetMsg("You are not authorized on this function.", AlertMessage.MsgType.Error));
            return;
        }
        //context.Response.Write("OK");
        //return;
        string strResult = string.Empty;
        try
        {
            DateTime dtmDate = CastAide.AsDateTime(context.Request["date"]);

            IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            string[] arr = new string[4];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = context.Request["txt" + (i + 1).ToString()];
            
            //Get Data here
            string strErrMsg = string.Empty;
            bool isSuccess = userService.InsResetWinning(dtmDate, AppSession.AccId, arr, context.Request.UserHostAddress, ref strErrMsg);

            if (!isSuccess)
                strResult = AlertMessage.GetMsg(strErrMsg, AlertMessage.MsgType.Error);
            else
                strResult = "OK";
        }

        catch (Exception ex)
        {
            strResult = AlertMessage.GetMsg("Cannot update data! Please contact Adminstrator", AlertMessage.MsgType.Error);
            LogExtensions.ExceptionHandler(ex);           
        }
        context.Response.Write(strResult);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}