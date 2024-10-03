<%@ WebHandler Language="C#" Class="UpdateChangeGroup" %>

using System;
using System.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities.Customer;

public class UpdateChangeGroup : BaseHandler
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
                IBCUtils.IbcError err = new IbcError();
                string strClear = context.Request["clearCr"] as string ?? string.Empty;
                string accId = context.Session["accId"].ToString();
                string uname = context.Request["txtUname"] as string ?? string.Empty;
                uname = uname.Trim();
                string strResult = "";
                string errmsg = "";
                if (strClear == "1")
                {
                    strResult = "Clear Credit successfully.";
                    var custInfo = UpdateCustomer.GetCustInfo(uname);
                    
                    if (Permissions.CheckPermission(Convert.ToInt32(context.Session["AccId"] as object ?? -1), Enumerations.Functions.Customer.Credit, ref errmsg) != 0)
                    {
                        err.ErrorMsg = errmsg;
                        err.ErrorCode = 101;
                    }
                    else
                    {
                        if (custInfo.RoleId == 1)
                        {
                            var totalAmt = UpdateCustomer.GetTotalAmount(custInfo.CustId);
                            err = UpdateCustomer.UpdClearMemberCreditRpt(uname, accId, totalAmt);
                        }
                        else
                        {
                            err = UpdateCustomer.UpdClearSMACreditRpt(uname, accId);
                        }
                    }                  

                }
                else
                {
                    string ugroup = context.Request["listGroup"] as string ?? string.Empty;
                    if (Permissions.CheckPermission(Convert.ToInt32(context.Session["AccId"] as object ?? -1), Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
                    {
                        err.ErrorMsg = errmsg;
                        err.ErrorCode = 101;
                    }
                    else
                    {
                        err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdUserGroupRpt(uname, ugroup, accId);
                        strResult = "Change User Group successfully.";
                    }
                }
                if (err.IsSuccess())
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(strResult, AlertMessage.MsgType.Success) + "','errMsg');");
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
        var hasPermission = Sunplus.Accounting.Core.Common.Functions.Customer.IsVisibleClearCredit;

        if (hasPermission)
        {
            var username = _context.Request["txtUname"];

            return CheckSpecialPermission(username)
                && CheckCreditSiteDepositSitePermission(username);
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
