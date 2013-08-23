<%@ WebHandler Language="C#" Class="UpdateSuspendMember" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;


public class UpdateSuspendMember : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] != null)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                int custId = Convert.ToInt32(context.Request["custId"]);
                int isSuspend = Convert.ToInt32(context.Request["autoSuspend"]);
                int accId = Convert.ToInt32(context.Session["AccId"]);
                IBCUtils.IbcError err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdAutoSuspendRpt(custId,isSuspend,accId);
                if (err.IsSuccess())
                {

                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg("Update Auto Suspend successfully.", AlertMessage.MsgType.Success) + "','errMsg');");
                }
                else
                {

                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(err.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
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
