<%@ WebHandler Language="C#" Class="UpdateCustInfo" %>

using System;
using System.Web;
using System.Collections;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;

public class UpdateCustInfo : BaseHandler
{
    protected override void RedirectError()
    {
        _context.Response.Write("<html><head></head><body>");
        _context.Response.Write("<script type=\"text/javascript\">");
        _context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error) + "','errMsg'); parent.afterUpdate(false, 2000);");
        _context.Response.Write("</script></body></html>");
    }
    public override void ProcessRequest(HttpContext context)
    {
        try
        {
            base.ProcessRequest(context);
            context.Response.ContentType = "text/html";
            string errmsg = "";
            if (context.Session["AccId"] == null)
            {
                return;
            }
            else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
            {
                context.Response.Write("<html><head></head><body>");
                context.Response.Write("<script type=\"text/javascript\">");
                context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "','errMsg'); parent.afterUpdate(false, 2000);");
                context.Response.Write("</script></body></html>");
                return;
            }
            else
            {
                context.Response.Write("<html><head></head><body>");
                context.Response.Write("<script type=\"text/javascript\">");
                try
                {
                    if (context.Request["custId"] == null) return;
                   
                    int custId = CastAide.AsInt32(context.Request["custId"]);

                    Hashtable ht = new Hashtable();
                    ht.Add("custid", custId);
                    ht.Add("processedby", context.Request["txtPersonInCharge"]);
                    ht.Add("boss", context.Request["txtBoss"]);
                    ht.Add("requestby", context.Request["txtRequestor"]);
                    ht.Add("approvedby ", context.Request["txtApprover"]);
                    ht.Add("mobilephone ", context.Request["txtPhone"]);
                    ht.Add("maxLoss ", context.Request["txtMaxLoss"]);
                    ht.Add("ip", context.Request.UserHostAddress);
                    ht.Add("userid", context.Session["AccId"]);
                    IbcError error = UpdateCustomer.CustomerUpdDirectInfomation(ht);
                    if (error.IsSuccess())
                    {
                        context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg("Update Direct Customer Information Successfully.", AlertMessage.MsgType.Success) + "','errMsg');");
                        context.Response.Write("parent.afterUpdate(true, 2000);");
                    }
                    else
                    {
                        context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                    }

                }
                catch (Exception ex)
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
                }
                finally
                {
                    context.Response.Write("</script></body></html>");
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
            context.Response.Write("</script></body></html>");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
