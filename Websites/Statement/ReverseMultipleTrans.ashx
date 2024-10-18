<%@ WebHandler Language="C#" Class="ReverseMultipleTrans" %>

//using System;
//using System.Web;
//using System.Web.SessionState;
//using DNAide.Web;
//using Sunplus.Accounting.Core.Common;
//using Sunplus.Accounting.Core.Helper;
//using System.Linq;
//using System.Collections.Generic;
//using Sunplus.Accounting.Business.Security;

//public class ReverseMultipleTrans : IHttpHandler, IRequiresSessionState
//{

//    public void ProcessRequest(HttpContext context)
//    {
//        context.Response.ContentType = "text/html";
//        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);


//        if (context.Session["CustId"] == null || context.Session["AccId"] == null)
//        {
//            context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error));
//        }
//        else
//        {
//            if (Permissions.GetHostPermission(Enumerations.Functions.Transfer.DeleteTrans))
//            {
//                var custName = context.Request["custName"].ToString();
//                var userService = Sunplus.Accounting.Business.BootStrappers.CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.IUserService>();
//                int custId = userService.GetCustomerIdByUsername(custName);
//                var refsno = context.Request["refsno"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//                if (refsno.Length == 0)
//                {
//                    context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INVALID_INPUT_VALUE_MSG, AlertMessage.MsgType.Error));
//                    return;
//                }

//                List<ReverseError> reverseErrors = new List<ReverseError>();
//                List<string> successRefs = new List<string>();
//                foreach (var refno in refsno)
//                {
//                    long parsedRefno = 0;
//                    if (!long.TryParse(refno, out parsedRefno))
//                    {
//                        context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INVALID_INPUT_VALUE_MSG, AlertMessage.MsgType.Error));
//                        return;
//                    }
//                    else
//                    {
//                        try
//                        {

//                            IBCUtils.IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountStatement.Acc_BettransDelTransaction(parsedRefno, custId, (int)context.Session["AccId"], context.Request.UserHostAddress);
//                            if (error.ErrorCode != 0)
//                            {
//                                reverseErrors.Add(new ReverseError() { Refno = parsedRefno.ToString(), Message = error.ErrorMsg });

//                            }
//                            else
//                            {
//                                successRefs.Add(parsedRefno.ToString());
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
//                            context.Response.Write("1|" + AlertMessage.GetMsg(IBCUtils.IbcError.INTERNAL_ERROR_MSG, AlertMessage.MsgType.Error));
//                            return;
//                        }
//                    }

//                }

//                var response = "0";
//                if (successRefs.Any())
//                {
//                    response += "|<div style='color:green'>Reverse successfully: " + string.Join(", ", successRefs) + "</div>";

//                }

//                if (reverseErrors.Any())
//                {
//                    response += "|<div style='color:red'>Reverse unsuccessfully:";
//                    reverseErrors.ForEach(error => { response += "</br>Ref No " + error.Refno + ": " + error.Message; });
//                    response += "</div>";
//                }

//                context.Response.Write(response);
//            }
//            else
//            {
//                context.Response.Write("1|" + AlertMessage.GetMsg("You are not authorized on this function.", AlertMessage.MsgType.Error));
//            }
//        }
//    }

//    public class ReverseError
//    {
//        public string Refno { get; set; }
//        public string Message { get; set; }
//    }

//    public bool IsReusable
//    {
//        get
//        {
//            return false;
//        }
//    }

//}