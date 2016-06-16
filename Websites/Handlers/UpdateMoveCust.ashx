<%@ WebHandler Language="C#" Class="UpdateMoveCust" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Fanex.Tarus.Security.Criteria;

public class UpdateMoveCust : BaseHandler {

    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        base.ProcessRequest(context);
        if (context.Session["AccId"] != null)
        {
            try
            {
                if (AppSession.AccId == 0)
                {
                    context.Response.Write(AlertMessage.GetMsg("Session Timeout", AlertMessage.MsgType.Error));
                    return;

                }
                string type = context.Request["agent"] as string ?? string.Empty;
                string strAgent = "";
                string recommendId = "";
                string custUname = "";

                var moveCustomer = new MoveCustomer();
                recommendId = context.Request["upline"] as string ?? string.Empty;
                custUname = context.Request["downline"] as string ?? string.Empty;
                var checkSpecialSitePermission = Permissions.CheckSpecialSitePermission(
                    new CheckSpecialSitePermissionCriteria()
                    {
                        UserID = AppSession.AccId,
                        CustName = recommendId
                    });

                if (checkSpecialSitePermission.ErrorCode > 0)
                {
                    context.Response.Write(AlertMessage.GetMsg(checkSpecialSitePermission.ErrorMessage, AlertMessage.MsgType.Error));
                }
                else
                {
                    MoveCustomer.MoveType moveType = type == "1" ? MoveCustomer.MoveType.Agent : MoveCustomer.MoveType.Master;

                    string resultMsg = moveCustomer.Move(moveType, custUname, recommendId);
                    if (resultMsg.IndexOf("Successfully", StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        CustomerService.ClearSystemUsers();
                        context.Response.Write(AlertMessage.GetMsg(resultMsg, AlertMessage.MsgType.Success));
                    }
                    else
                    {
                        context.Response.Write(AlertMessage.GetMsg(resultMsg, AlertMessage.MsgType.Error));
                    }
                }

            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write(StringUtils.ParseToJSMsg(ex.Message));
            }
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
