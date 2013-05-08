<%@ WebHandler Language="C#" Class="UpdateCustomerHR" %>

using System;
using System.Web;
using DNAide.Core;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using System.Collections;
using System.Web.SessionState;
using IBCUtils;


//phuong.nguyen
//update HR setting for customer
public class UpdateCustomerHR : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/html";
        if (context.Request["custid"] == null)
        {
            return;
        }
        else
        {
            try
            {
                System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentUICulture;                
                
                
                int userId = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
                if (userId == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
                int custid = Convert.ToInt32(context.Request["custid"]);
                string roleid = context.Request["roleid"];
                string ip = context.Request.UserHostAddress.ToString();
                //int roleId = Convert.ToInt16(context.Session["CustRoleId"]);
                double ukComm = Convert.ToDouble(context.Request["listUKMeeting"]);
                double otherComm = Convert.ToDouble(context.Request["listOthersMeeting"]);
                double minbet = Convert.ToDouble(context.Request["txtMinbet151"]);
                double maxbet = Convert.ToDouble(context.Request["txtMaxbet151"]);
                double maxpermatch = Convert.ToDouble(context.Request["txtMaxPerMatch151"]);
                string strDisabledHRValue = context.Request["newHRStatus"];
                string strIsUpdDownLine = context.Request["newHRStatusDL"];
                string set = context.Request["set"];
                bool isUpdDownline = true;
                bool disabledHR = false;
                if(!string.IsNullOrEmpty(strDisabledHRValue))
                    disabledHR = strDisabledHRValue.Equals("1") ? true : false;
                if (!string.IsNullOrEmpty(strIsUpdDownLine))
                    isUpdDownline = strIsUpdDownLine.Equals("1") ? true : false;
                string language = "E";
                string errmsg = "";
                if (Permissions.CheckPermission(userId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
                {
                    context.Response.Write("<script>parent.showProgressBar(false); parent.scroll('0', '0'); parent.document.getElementById('btnAdd').disabled = false; parent.document.getElementById('btnAdd').style.color = ''; parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(errmsg) + "','error151');</script>");
                    return;
                }
                Hashtable updHRResult = new Hashtable();
                updHRResult =
                    CustomerManager.UpdateCustomerHRSetting(custid, userId, disabledHR, language, ip, minbet, maxbet,
                                                            maxpermatch, ukComm, otherComm, isUpdDownline);
                string strDelayReload = string.Empty;
                string strUpdateHRStatus = string.Empty;
                if (set.Equals("1"))
                {
                    strDelayReload =
                        "<script>parent.DelayReloadPage('../v2/EditHorseracing.aspx?set=1&result=1&roleid=" + roleid + "&custId=" + custid.ToString() +
                        "', 2000);</script>";
                    strUpdateHRStatus = "<script type='text/javascript'>parent.parent.frames['leftPanel'].document.getElementById('disabledHR').innerHTML ='" + ((disabledHR) ? "Yes" : "No") + "';</script>";
                    strDelayReload += strUpdateHRStatus;
                }
                else if (set.Equals("2"))
                {
                    string imgLoading = "../App_Themes/SteelBlue/Images/loader3.gif";
                    strDelayReload = "<script>parent.alert_error('" + Resources.Language.lblUpdatedHRSuccessfull + "','error151');parent.window.opener.DelayLoadPage('', 2000, '" + imgLoading + "'); parent.close()</script>";
                    //strUpdateHRStatus = "<script type='text/javascript'>parent.window.opener.parent.parent.frames['leftPanel'].document.getElementById('disabledHR').innerHTML ='" + ((disabledHR) ? "Yes" : "No") + "';</script>";
                }
                
               
                if (!Convert.ToString(updHRResult["errCode"]).Equals("0"))
                {
                    string msg = updHRResult["errMsg"] as string ?? string.Empty;
                    if (105 != Convert.ToInt16(updHRResult["errCode"]))
                        context.Response.Write("<script>parent.showProgressBar(false); parent.scroll('0', '0'); parent.document.getElementById('btnAdd').disabled = false; parent.document.getElementById('btnAdd').style.color = ''; parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(msg) + "','error151');</script>");
                    else
                    {
                        context.Response.Write(
                            "<script>parent.showProgressBar(false); parent.scroll('0', '0'); parent.document.getElementById('btnAdd').disabled = false; parent.document.getElementById('btnAdd').style.color = '';</script>");
                        context.Response.Write(strDelayReload);
                    }
                }
                else
                {
                    context.Response.Write("<script>parent.showProgressBar(false); parent.scroll('0', '0'); parent.document.getElementById('btnAdd').disabled = false; parent.document.getElementById('btnAdd').style.color = '';</script>");
                    context.Response.Write(strDelayReload);
                }
            }
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("<script>parent.showProgressBar(false); parent.alert_error('" + ex.Message.Replace("'", "").Replace("\"", "") + "', 'error151'); </script>");
            }
        }
    }

   
    public bool IsReusable {
        get {
            return false;
        }
    }

}
