<%@ WebHandler Language="C#" Class="UpdateChangeGroup" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class UpdateChangeGroup : BaseHandler {
    
    public override void ProcessRequest (HttpContext context) {        
        context.Response.ContentType = "text/html";
        base.ProcessRequest(context);
        if (context.Session["AccId"] != null)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                IBCUtils.IbcError err = null;
                string strClear = context.Request["clearCr"] as string ?? string.Empty;
                string accId = context.Session["accId"].ToString();
                string uname = context.Request["txtUname"] as string ?? string.Empty;
                uname = uname.Trim();
                string strResult = "";              
                if (strClear == "1")
                {
                    strResult = "Clear Credit successfully.";
                    err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdClearCreditRpt(uname, accId);
                }
                else
                {                    
                    string ugroup = context.Request["listGroup"] as string ?? string.Empty;
                    err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdUserGroupRpt(uname, ugroup, accId);
                    strResult = "Change User Group successfully.";
                }                
                if (err.IsSuccess())
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(strResult,AlertMessage.MsgType.Success) + "','errMsg');");
                }
                else
                {

                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(err.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
            }
        }     
    }
    protected override bool HasPermission()
    {
        return Sunplus.Accounting.Core.Common.Functions.Customer.IsVisibleClearCredit;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}
