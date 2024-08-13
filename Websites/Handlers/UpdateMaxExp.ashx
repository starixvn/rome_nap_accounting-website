<%@ WebHandler Language="C#" Class="UpdateMaxExp" %>

using System;
using System.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;


public class UpdateMaxExp : BaseHandler {

    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {
            return;
        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                string set = context.Request["set"];
                string custId = context.Request["custId"];
               
                if (!Permissions.GetHostPermission(Enumerations.Functions.Customer.MaxExposure))
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg( "You don\'t have permission to access.", AlertMessage.MsgType.Error) + "','errMsg');");
                }
                else
                {
                    string matchLimit = context.Request["matchLimit"];
                    IbcError error = UpdateCustomer.UpdateMaxExposure(custId, matchLimit, context.Session["AccId"]);
                    if (error.IsSuccess())
                    {
                        context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg("Update Max Exposure Successfully.", AlertMessage.MsgType.Success) + "','errMsg');parent.afterUpdate(false, 2000);");
                    }
                    else
                    {
                        if (error.ErrorCode == 500)
                        {
                            context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg'); parent.afterUpdate(false, 2000);");
                        }
                        else
                        {
                            context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                        }
                    }
                }
            }
            catch
            {
                GotoErroPage(context);
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
            }
        }

    }

    public override bool IsReusable {
        get {
            return false;
        }
    }

}
