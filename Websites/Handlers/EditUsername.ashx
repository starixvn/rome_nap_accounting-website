<%@ WebHandler Language="C#" Class="EditUsername" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;

using BLL;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Fanex.Tarus.Security.HttpModules.Services;

public class EditUsername : BaseHandler
{

    public override void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        base.ProcessRequest(context);
        if (context.Session["AccId"] != null)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                string oldUser = context.Request["txtOldUname"] as string ?? string.Empty;
                oldUser = oldUser.Trim();
                string newUser = context.Request["txtNewUname"] as string ?? string.Empty;
                newUser = newUser.Trim();
                string accId = context.Session["AccId"].ToString();

                int custid = 0;
                IBCUtils.IbcError err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdUserNameRpt(oldUser, newUser, accId, out custid);

                if (err.IsSuccess())
                {
                    context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg("Update CustId successfully.", AlertMessage.MsgType.Success) + "','errMsg');");
                }
                else
                {
                    context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg(err.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg("Not casino's customer", AlertMessage.MsgType.Error) + "','errMsg');");
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
        var hasPermission = Sunplus.Accounting.Core.Common.Functions.Customer.IsVisibleClearCredit;

        if (hasPermission)
        {
            var username = _context.Request["txtOldUname"];

            return CheckSpecialPermission(username)
                && CheckCN88SiteCreditSiteDepositSitePermission(username);
        }

        return false;
    }

    public override bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
