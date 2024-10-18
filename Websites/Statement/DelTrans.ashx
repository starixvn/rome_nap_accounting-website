<%@ WebHandler Language="C#" Class="DelTrans" %>

//using System;
//using System.Web;
//using System.Web.SessionState;
//using DNAide.Web;
//using Sunplus.Accounting.Business.Security;
//using Sunplus.Accounting.Core.Common;
//using Sunplus.Accounting.Core.Helper;


//public class DelTrans : IHttpHandler,IRequiresSessionState {

//    public void ProcessRequest (HttpContext context) {
//        context.Response.ContentType = "text/html";
//        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);


//        if (context.Session["CustId"] == null || context.Session["AccId"] == null)
//        {
//            context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error));
//        }
//        else
//        {
//            var custName = context.Request["custName"].ToString();
//            var userService = Sunplus.Accounting.Business.BootStrappers.CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.IUserService>();
//            int custId = userService.GetCustomerIdByUsername(custName);
//            long refno = 0;
//            if (!long.TryParse(context.Request["refno"], out refno))
//            {
//                context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INVALID_INPUT_VALUE_MSG, AlertMessage.MsgType.Error));
//            }
//            else
//            {
//                try
//                {
//                    if (Permissions.GetHostPermission(Enumerations.Functions.Transfer.DeleteTrans))
//                    {
//                        IBCUtils.IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountStatement.Acc_BettransDelTransaction(refno, custId , (int)context.Session["AccId"], context.Request.UserHostAddress);
//                        if (error.ErrorCode != 0)
//                        {
//                            context.Response.Write("1|" + error.ErrorMsg);
//                        }
//                        else
//                        {
//                            context.Response.Write("0|Reverse transaction successfully");
//                        }
//                    }
//                    else
//                    {
//                        context.Response.Write("1|You are not authorized on this function.");
//                    }


//                }
//                catch(Exception ex)
//                {
//                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
//                    context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INTERNAL_ERROR_MSG, AlertMessage.MsgType.Error));                    
//                }                
//            }
//        }
//    }

//    public bool IsReusable {
//        get {
//            return false;
//        }
//    }

//}
