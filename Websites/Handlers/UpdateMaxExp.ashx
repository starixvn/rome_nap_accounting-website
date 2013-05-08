<%@ WebHandler Language="C#" Class="UpdateMaxExp" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
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
                string matchLimit = context.Request["matchLimit"];
                IBCUtils.IbcError error = UpdateCustomer.UpdateMaxExposure(custId, matchLimit, context.Session["AccId"]);
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
            catch(Exception ex)
            {
               //context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
                GotoErroPage(context);
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
            }
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
