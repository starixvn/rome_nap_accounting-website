<%@ WebHandler Language="C#" Class="UpdateCustomerStatus" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;

public class UpdateCustomerStatus : BaseHandler
{

    public override void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        base.ProcessRequest(context);
        try
        {
            int _accid = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            int _custid = Convert.ToInt32(context.Request.QueryString["custid"]);
            if (_custid == -1) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);

            bool status = false;
            bool isUpdDownline = false;
            if (!Boolean.TryParse(context.Request["status"] as string ?? string.Empty, out status))
                throw new AccException("Invalid Session");
            string s = string.Empty;
            string type = context.Request["type"] as string ?? string.Empty;
            string errmsg = "";
            if (Permissions.CheckPermission(_accid, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
            {
                context.Response.Write("<script type='text/javascript'>" + StringUtils.JS_Alert(errmsg) + "</script>");
                return;
            }

            switch (type)
            {
                case "suspended":
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateSuspend(_accid, _custid, status, context.Request.UserHostAddress);
                    s = "parent.changeDisplayStatus('suspended','" + status + "');";
                    s += "var chksuspended = parent.parent.frames['centerFr'].document.getElementById('chkSuspended');";
                    s += "if (chksuspended!=null) chksuspended.checked=" + ((status) ? "true;" : "false;");
                    break;
                case "Outright":
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDisableOutright(_accid, _custid, status, context.Request.UserHostAddress);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "special":
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateSpecial(_accid, _custid, status);
                    s = "parent.changeDisplayStatus('special','" + status + "');";
                    break;
                case "autompt":
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateAutoMPT(_accid, _custid, status, context.Request.UserHostAddress);
                    s = "parent.document.getElementById('AllowAutoPT').innerHTML ='" + ((status) ? "Yes" : "No") + "';";
                    s += "parent.changeDisplayStatus();";
                    break;
                case "closed":
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateClosed(_accid, _custid, status, context.Request.UserHostAddress);
                    if (context.Request["isClearCredit"] != null)
                    {
                        bool isClearCredit = bool.Parse(context.Request["isClearCredit"].ToString());
                        if (isClearCredit)
                        {
                            var customerManager = new Sunplus.Accounting.Business.Customer.CustomerManager();
                            string CustUserName = customerManager.GetCustName(AppSession.CustRoleId);
                            string accId = AppSession.AccId.ToString();
                            IbcError error = new IbcError();
                            if (Permissions.CheckPermission(_accid, Enumerations.Functions.Customer.Credit, ref errmsg) != 0)
                            {
                                error.ErrorMsg = errmsg;
                                error.ErrorCode = 101;
                            }
                            else
                            {
                                if(AppSession.CustRoleId == 1) // member
                                {
                                    var totalAmt = UpdateCustomer.GetTotalAmount(AppSession.CustID);
                                    error = UpdateCustomer.UpdClearMemberCreditRpt(CustUserName, accId, totalAmt);
                                }
                                else
                                {
                                    error = UpdateCustomer.UpdClearSMACreditRpt(CustUserName, accId);
                                }
                            }

                            if (error.ErrorCode > 0)
                            {
                                context.Response.Write("<script type='text/javascript'>" +
                                                       StringUtils.JS_Alert(StringUtils.ParseToJSMsg(error.ErrorMsg)) + "</script>");
                            }

                        }
                        s += "parent.HideClosedPop('ClosedStatusPop');";

                    }
                    s += "parent.changeDisplayStatus();";
                    break;
                case "AutoSuspend":
                    int isSuspend = (status) ? 1 : 0;
                    if (Permissions.CheckPermission(_accid, Enumerations.Functions.Customer.AutoSuspend, ref errmsg) != 0)
                    {
                        s = StringUtils.JS_Alert(errmsg);
                        break;
                    }
                    IbcError err = UpdateCustomer.CustomerUpdAutoSuspendRpt(_custid, isSuspend, _accid);
                    if (err.ErrorCode > 0)
                        throw new AccException(err.ErrorMsg);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "Internal":
                    if (status.Equals(true))
                    {
                        if (!Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.CreateFinanceAccount))
                        {
                            s = StringUtils.JS_Alert(Resources.ErrorMsg.error_CreateFinanceAccountPermission);
                            break;
                        }
                    }
                    CustomerUpdateStatus.CustomerUpdInternal(_custid, status, _accid, context.Request.UserHostAddress);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "SelectedBetting"://Selected Betting
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");
                    CustomerUpdateStatus.CustomerUpdateSelectedBetting(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "limitperip":
                    isUpdDownline = false;
                    int limitperid = 0;

                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");

                    if (!Int32.TryParse(context.Request["limitperid"] as string ?? string.Empty, out limitperid))
                        throw new AccException("Invalid Session");

                    CustomerUpdateStatus.CustomerUpdateLimitPerIP(_accid, _custid, limitperid, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                //End
                default: throw new AccException("Invalid Session!");
            }
            context.Response.Write("<script type='text/javascript'>" + s + "</script>");
        }
        catch (AccException ex)
        {
            context.Response.Write("<script type='text/javascript'>" + StringUtils.JS_Alert(ex.Message) +
                                        "</script>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    public override bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
