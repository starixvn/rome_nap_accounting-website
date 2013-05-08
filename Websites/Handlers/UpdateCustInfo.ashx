<%@ WebHandler Language="C#" Class="UpdateCustInfo" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;



public class UpdateCustInfo : BaseHandler {
    protected override void RedirectError()
    {
        _context.Response.Write("<html><head></head><body>");
        _context.Response.Write("<script type=\"text/javascript\">");
        _context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error) + "','errMsg'); parent.afterUpdate(false, 2000);");
        _context.Response.Write("</script></body></html>");
    }
    public override void ProcessRequest (HttpContext context) {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/html";
        string errmsg="";
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

                var custDetail = Sunplus.Accounting.Business.Customer.CustomerService.GetCustInfo(context.Request);
                var custId = CastAide.AsInt32(context.Request["custId"]);
                
                Hashtable ht = new Hashtable();
                ht.Add("custid", custId);
                ht.Add("firstname", custDetail.FirstName);
                ht.Add("lastname", custDetail.LastName);
                ht.Add("address", custDetail.Address);
                ht.Add("postal", custDetail.Postal);
                ht.Add("city", custDetail.City);
                ht.Add("state", custDetail.State);
                ht.Add("country", custDetail.Country);
                ht.Add("phone", custDetail.Phone);
                ht.Add("mobilephone", custDetail.MobilePhone);
                ht.Add("fax", custDetail.Fax);
                ht.Add("email", custDetail.Email);
                if (context.Request["txtBirthday"] == "")
                {
                    ht.Add("birthday", System.DBNull.Value);
                }
                else
                {
                    ht.Add("birthday", context.Request["txtBirthday"]);
                }
                ht.Add("refcode1", context.Request["txtRefcode1"]);
                ht.Add("refcode2", context.Request["txtRefcode2"]);
                ht.Add("remark", custDetail.Remark);
                if (String.IsNullOrEmpty(context.Request.Form["chkImportant"]))
                {
                    ht.Add("important", 0);
                }
                else
                {
                    ht.Add("important", 1);
                }
                ht.Add("userid", context.Session["AccId"]);
                ht.Add("ip", context.Request.UserHostAddress);
                IbcError error = UpdateCustomer.CustomerUpdUserInfoRpt(ht);
                if (error.IsSuccess())
                {

                    // Update for casino
                    if (Permissions.IsAllowCasino() && context.Request["hidSynCS"] != "False") 
                    {
                        try //Cheat casino
                        { // Store data for casino section
                            CSCustomerDetail custInfo = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustInfoIBC(ht["custid"].ToString(), ht);
                            custInfo.Currency = CastAide.AsInt16(context.Request["hidCur"]);
                            // Sync data to Casino
                            CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
                            //csCustomer.SiteName = new Sunplus.Accounting.Business.Customer.CustomerManager().GetRNGSiteNameByCustId(custId);
                            csCustomer.SiteName = custInfo.SiteName;
                            
                            bool status = csCustomer.UpdateCustInfoNoStatus(custInfo.IBCCustID, custInfo.FirstName, custInfo.LastName,
                                                               custInfo.Phone, custInfo.MobilePhone, custInfo.Fax,
                                                               custInfo.Email, ht["address"].ToString(), CastAide.AsDateTime(ht["birthday"]),
                                                               ht["postal"].ToString(), ht["state"].ToString(), ht["remark"].ToString(),
                                                               context.Request.UserHostAddress, AppSession.CustID, AppSession.Username, 3);
                        }
                        catch (Exception ex)
                        {
                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                        }
                        
                    }
                    
                    
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg("Update Customer Information Successfully.", AlertMessage.MsgType.Success ) + "','errMsg');");
                    //if (string.IsNullOrEmpty(context.Request["fcustinfo"]))
                    //{
                        context.Response.Write("parent.afterUpdate(true, 2000);");
                    //}
                }
                else
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("parent.alert_error('"+ AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) +"','errMsg');");
            }
            finally
            {
                context.Response.Write("</script></body></html>");
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
