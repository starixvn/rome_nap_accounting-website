<%@ WebHandler Language="C#" Class="UpdateVirtualRate" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;


public class UpdateVirtualRate : BaseHandler{
    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {
        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                string custId = context.Request["custId"];
                decimal virRate = (IBCUtils.FormatHandler.GetDecimal(context.Request["hidExRate"]) + 100) / 100;
                string userId=Convert.ToString(context.Session["AccId"]);
                IBCUtils.IbcError error = UpdateCustomer.UpdVirtualRate(custId,virRate,userId);
                if (error.IsSuccess())
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg("Update Virtual Exchange Rate Successfully.", AlertMessage.MsgType.Success) + "','errMsg'); parent.afterUpdate(true, 2000);");                    
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
