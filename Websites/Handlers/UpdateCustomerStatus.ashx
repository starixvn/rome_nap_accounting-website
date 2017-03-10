<%@ WebHandler Language="C#" Class="UpdateCustomerStatus" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using IBCUtils.Web;
using System.Text;
using CasinoLibrary.SBAgent;
using CasinoLibrary.Model.UserException;

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

            var customerRngSite = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(_custid);
            string lockUpdate = "";
            switch (type)
            {
                case "suspended":
                    lockUpdate = string.Format(Resources.ErrorMsg.error_LockUpdateStatus, "SUSPENDED");
                    CSCustomerDetail custcs = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetBasicCustInfoIBC(_custid);
                    try
                    {

                        if (WebSettings.disableCS && custcs.CasinoSync)
                        {
                            context.Response.Write("<script type='text/javascript'>" +
                                                   StringUtils.JS_Alert(StringUtils.ParseToJSMsg(lockUpdate)) + "</script>");
                            return;

                        }
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateSuspend(_accid, _custid, status, context.Request.UserHostAddress);
                    s = "parent.changeDisplayStatus('suspended','" + status + "');";
                    s += "var chksuspended = parent.parent.frames['centerFr'].document.getElementById('chkSuspended');";
                    s += "if (chksuspended!=null) chksuspended.checked=" + ((status) ? "true;" : "false;");


                    // Notes: please put this into a transaction with updating of IBC
                    try // Casino cheat
                    {
                        if (custcs.CasinoSync)
                        {
                            bool success = CasinoManager.UpdateCustSuspend(_custid, status,
                                                                               context.Request.UserHostAddress, customerRngSite);
                        }
                    }
                    catch (ErrorException casinoException)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(casinoException);
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
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

                    custcs = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetBasicCustInfoIBC(_custid);
                    if (custcs.CasinoSync)
                    {
                        var casinoSync = new Customer();
                        try
                        {
                            casinoSync.UpdateMasterDisableAutoMPT(_custid, status, context.Request.UserHostAddress, AppSession.AccId, AppSession.AccUserName, 3);
                        }
                        catch (ErrorException casinoException)
                        {
                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(casinoException);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    s = "parent.document.getElementById('AllowAutoPT').innerHTML ='" + ((status) ? "Yes" : "No") + "';";
                    s = "parent.changeDisplayStatus();";
                    break;
                case "closed":
                    lockUpdate = string.Format(Resources.ErrorMsg.error_LockUpdateStatus, "CLOSED");
                    custcs = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetBasicCustInfoIBC(_custid);
                    try
                    {

                        if (WebSettings.disableCS && custcs.CasinoSync)
                        {
                            context.Response.Write("<script type='text/javascript'>" +
                                                   StringUtils.JS_Alert(StringUtils.ParseToJSMsg(lockUpdate)) + "</script>");
                            return;

                        }
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateClosed(_accid, _custid, status, context.Request.UserHostAddress, custcs.RoleId);
                    if (context.Request["isClearCredit"] != null)
                    {
                        bool isClearCredit = bool.Parse(context.Request["isClearCredit"].ToString());
                        if (isClearCredit)
                        {
                            string CustUserName = Sunplus.Accounting.Business.Customer.CustomerManager.GetCustName(AppSession.CustRoleId);
                            string accId = AppSession.AccId.ToString();
                            IBCUtils.IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdClearCreditRpt(CustUserName, accId);

                            if (error.ErrorCode > 0)
                            {
                                context.Response.Write("<script type='text/javascript'>" +
                                                       StringUtils.JS_Alert(StringUtils.ParseToJSMsg(error.ErrorMsg)) + "</script>");
                            }

                        }
                        s += "parent.HideClosedPop('ClosedStatusPop');";

                    }
                    s += "parent.changeDisplayStatus();";

                    try // Casino cheat
                    {
                        if (custcs.CasinoSync)
                        {
                            bool success1 = CasinoManager.UpdateCustClosed(_custid, status,
                                                                               context.Request.UserHostAddress, customerRngSite);
                        }
                    }
                    catch (ErrorException casinoException)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(casinoException);
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    break;
                case "dangerous":
                    //HuyNguyen Modify
                    int level;
                    string name = context.Request["name"].ToString();
                    if (!Int32.TryParse(context.Request["level"] as string ?? string.Empty, out level))
                        throw new AccException("Invalid Session");
                    int dangertype;
                    if (name == "Original")
                    { dangertype = 1; }
                    else if (name == "ABT")
                    { dangertype = 2; }
                    else
                    { dangertype = 3; }
                    StringBuilder result = new StringBuilder();
                    DangerousSetting danger = new DangerousSetting();
                    DangerousDetail dangerDetail = danger.CustomerUpdateDangerous(_accid, _custid, level, dangertype);
                    result.Append("parent.document.getElementById('spanDangerous" + name + "').innerHTML ='" + dangerDetail.name + "';");
                    result.Append("parent.document.getElementById('tdvalue" + name + "').style.backgroundColor ='" + dangerDetail.color + "';");
                    result.Append("parent.document.getElementById('currentLevel" + name + "').value ='" + level + "';");

                    s = result.ToString();

                    break;
                /*Phuong.Nguyen
                 *Add new case for HR request changing status
                 */
                //Start
                case "disabledHR":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddonwline parameters");
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDisabledHR(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledHROpenTote":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddonwline parameters");
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDisabledHROpenTote(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledCS":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddonwline parameters");
                    CasinoManager.UpdateCSStatus(_custid, true, status, !isUpdDownline, customerRngSite);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "AutoSuspend":
                    int isSuspend = (status) ? 1 : 0;
                    IBCUtils.IbcError err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdAutoSuspendRpt(_custid, isSuspend, _accid);
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
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdInternal(_custid, status, _accid, context.Request.UserHostAddress);
                    //s = "parent.document.getElementById('internalUser').innerHTML ='" + ((status) ? Resources.Language.userInternal : Resources.Language.userBetting) + "';";
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledBingoGame"://BingoGame
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDisabledBingoGame(_accid, _custid, status, context.Request.UserHostAddress, !isUpdDownline, AppSession.IsSyncBingoCurrentCust);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledNumberGame"://NumberGame
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDisabledNumberGame(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "SelectedBetting"://Selected Betting
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateSelectedBetting(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "limitperip":
                    isUpdDownline = false;
                    int limitperid = 0;

                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddownline parameters");

                    if (!Int32.TryParse(context.Request["limitperid"] as string ?? string.Empty, out limitperid))
                        throw new AccException("Invalid Session");

                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateLimitPerIP(_accid, _custid, limitperid, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledLCS":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddonwline parameters");
                    new CustomerUpdateStatus().CustomerUpdateDisabledLiveCasino(_accid, _custid, status, context.Request.UserHostAddress, !isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledVS":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                        throw new AccException("Invalid isupddonwline parameters");
                    new CustomerUpdateStatus().CustomerUpdateDisabledVirtualSports(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledEsports":
                    if (Permissions.GetHostPermission(Enumerations.Functions.Customer.EnableEsports))
                    {
                        isUpdDownline = false;
                        if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                            throw new AccException("Invalid isupddonwline parameters");
                        new CustomerUpdateStatus().CustomerUpdateDisabledEsport(_accid, _custid, status, context.Request.UserHostAddress, isUpdDownline);
                        s = "parent.changeDisplayStatus();";
                    }
                    else
                    {
                        context.Response.Write("<script type='text/javascript'>" + StringUtils.JS_Alert("You are not authorized on this function.") +
                                   "</script>");
                    }
                    break;

                case "enableHorseRacingFixedOdds":
                    // Louis.Nguyen: Enable Horse Racing Fixed Odds
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                    {
                        throw new AccException("Invalid isupddonwline parameters");
                    }

                    new CustomerUpdateStatus().CustomerUpdateDisableHorseRacingFixedOdds(
                        _accid,
                        _custid,
                        status,
                        context.Request.UserHostAddress,
                        isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                case "disabledGoldDeluxe":
                    isUpdDownline = false;
                    if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdDownline))
                    {
                        throw new AccException("Invalid isupddonwline parameters");
                    }

                    new CustomerUpdateStatus().CustomerUpdateDisabledGoldDeluxe(_accid, _custid, status, context.Request.UserHostAddress, !isUpdDownline);
                    s = "parent.changeDisplayStatus();";
                    break;
                //End
                default: throw new AccException("Invalid Session!");
            }
            //s += "parent.parent.BlockUI();";            
            context.Response.Write("<script type='text/javascript'>" + s + "</script>");
        }
        catch (AccException ex)
        {
            string type = context.Request["type"] as string ?? string.Empty;
            if (type != "dangerous")
            {
                context.Response.Write("<script type='text/javascript'>" + StringUtils.JS_Alert(ex.Message) +
                                       "</script>");
            }
            else
            {
                context.Response.Write("<script type='text/javascript'>parent.$.msg({ content: '" + ex.Message +
                                       "', timeOut: 5000, fadeIn: 500, fadeOut: 200 });</script>");
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
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
