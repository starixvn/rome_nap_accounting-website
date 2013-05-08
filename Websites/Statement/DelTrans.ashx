<%@ WebHandler Language="C#" Class="DelTrans" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class DelTrans : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);


        if (context.Session["CustId"] == null || context.Session["AccId"] == null)
        {
            context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error));
        }
        else
        {
            var custName = context.Request["custName"].ToString();
            var userService = Sunplus.Accounting.Business.BootStrappers.CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.IUserService>();
            int custId = userService.GetCustomerIdByUsername(custName);
            long refno = 0;
            if (!long.TryParse(context.Request["refno"], out refno))
            {
                context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INVALID_INPUT_VALUE_MSG, AlertMessage.MsgType.Error));
            }
            else
            {
                try
                {
                    IBCUtils.IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountStatement.Acc_BettransDelTransaction(refno, custId , (int)context.Session["AccId"], context.Request.UserHostAddress);
                    if (error.ErrorCode != 0)
                    {
                        context.Response.Write("1|" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error));
                    }
                    else
                    {
                        context.Response.Write("0|Delete transaction successfully");
                    }
                }
                catch(Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INTERNAL_ERROR_MSG, AlertMessage.MsgType.Error));                    
                }                
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
