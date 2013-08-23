<%@ WebHandler Language="C#" Class="InsertNewMember" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using System.Collections;


public class InsertNewMember : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {
            return;
        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type='text/javascript'>\n");
            Hashtable resultUpdated = new Hashtable();
            try
            {
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                ht.Add("custid", 0);
                ht.Add("firstname", context.Request["txtFirstname"]);
                ht.Add("lastname", context.Request["txtLastname"]);
                ht.Add("creator", context.Session["AccId"]);
                ht.Add("username", context.Request["txtUsername"].Trim());
                ht.Add("userpwd", IBCUtils.Encryption.CfsEnCode(context.Request["txtPwd"].ToString().Trim()));
                ht.Add("phone", context.Request["txtPhone"]);
                ht.Add("mobilephone", context.Request["txtMobile"]);
                ht.Add("discount", context.Request["txtCommAgent1"]);
                ht.Add("discountcs", context.Request["txtCommAgent5"]);
                ht.Add("playerdiscount", context.Request["listCommMem1"]);
                ht.Add("playerdiscountcs", context.Request["listCommMem5"]);
                ht.Add("minbet", context.Request["txtMinBet"]);
                ht.Add("maxbet", context.Request["txtMaxBet"]);
                ht.Add("maxpermatch", context.Request["txtMaxPerMatch"]);
                ht.Add("minbetb", context.Request["txtMinBetb"]);
                ht.Add("maxbetb", context.Request["txtMaxBetb"]);
                ht.Add("maxpermatchb", context.Request["txtMaxPerMatchb"]);
                ht.Add("credit", context.Request["txtMaxCredit"]);
                ht.Add("recommend", context.Request["agentId"]);
                ht.Add("ugroup", context.Request["listGroupMember"]);
                if (String.IsNullOrEmpty(context.Request["chkClosed"]))
                {
                    ht.Add("closed", 0);
                }
                else
                {
                    ht.Add("closed", 1);
                }
                if (!String.IsNullOrEmpty(context.Request["chkCopyHdpD"]))
                {
                    ht.Add("apositiontaking_1", context.Request["listSHdpD"]);
                    ht.Add("apositiontaking_3", context.Request["listSHdpD"]);
                    ht.Add("apositiontaking_7", context.Request["listSHdpD"]);
                    ht.Add("apositiontaking_8", context.Request["listSHdpD"]);
                    ht.Add("apositiontaking_2", context.Request["listSHdpD"]);
                }
                else
                {
                    ht.Add("apositiontaking_1", context.Request["listSHdpD"]);
                    ht.Add("apositiontaking_3", context.Request["listSOUD"]);
                    ht.Add("apositiontaking_7", context.Request["listS1stHdpD"]);
                    ht.Add("apositiontaking_8", context.Request["listS1stOUD"]);
                    ht.Add("apositiontaking_2", context.Request["listSOE"]);
                }// end if (!String.IsNullOrEmpty(context.Request["chkCopyHdp"]))
                if (!String.IsNullOrEmpty(context.Request["chkCopy1x2"]))
                {
                    ht.Add("apositiontaking_4", context.Request["listS1x2"]);
                    ht.Add("apositiontaking_9", context.Request["listS1x2"]);
                    ht.Add("apositiontaking_5", context.Request["listS1x2"]);
                    ht.Add("apositiontaking_6", context.Request["listS1x2"]);
                    ht.Add("apositiontaking_10", context.Request["listS1x2"]);
                }
                else
                {
                    ht.Add("apositiontaking_5", context.Request["listS1x2"]);
                    ht.Add("apositiontaking_4", context.Request["listSCS"]);
                    ht.Add("apositiontaking_9", context.Request["listSMP"]);
                    ht.Add("apositiontaking_6", context.Request["listSTG"]);
                    ht.Add("apositiontaking_10", context.Request["listSOutright"]);
                }//end if (!String.IsNullOrEmpty(context.Request["chkCopy1x2"]))

                if (!String.IsNullOrEmpty(context.Request["chkCopyHdpL"]))
                {
                    ht.Add("alivepositiontaking_1", context.Request["listSHdpL"]);
                    ht.Add("alivepositiontaking_3", context.Request["listSHdpL"]);
                    ht.Add("alivepositiontaking_7", context.Request["listSHdpL"]);
                    ht.Add("alivepositiontaking_8", context.Request["listSHdpL"]);
                }
                else
                {
                    ht.Add("alivepositiontaking_1", context.Request["listSHdpL"]);
                    ht.Add("alivepositiontaking_3", context.Request["listSOUL"]);
                    ht.Add("alivepositiontaking_7", context.Request["listS1stHdpL"]);
                    ht.Add("alivepositiontaking_8", context.Request["listS1stOUL"]);
                }// end if (!String.IsNullOrEmpty(context.Request["chkLiveCopyHdp"]))

                if (!String.IsNullOrEmpty(context.Request["chkCopyHdpB"]))
                {
                    ht.Add("bapositiontaking_1", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_3", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_7", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_8", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_2", context.Request["listSHdpB"]);
                }
                else
                {
                    ht.Add("bapositiontaking_1", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_3", context.Request["listSOUB"]);
                    ht.Add("bapositiontaking_7", context.Request["listSHdpB"]);
                    ht.Add("bapositiontaking_8", context.Request["listSOUB"]);
                    ht.Add("bapositiontaking_2", context.Request["listSOEB"]);
                }// end if (!String.IsNullOrEmpty(context.Request["chkBsCopyHdp"]))                
                ht.Add("ip", context.Request.UserHostAddress.ToString());
                ht.Add("success", 1);
                ht.Add("language", "E");
                ht.Add("returnerrcode", 1);
                ht.Add("errcode", 0);
                ht.Add("errmsg", "");
                resultUpdated = UpdateCustomer.InsertMember(ht);
                if (Convert.ToString(resultUpdated["errCode"])=="0")
                {
                    //context.Response.Write("parent.alert_error('Add New Member successfully!','errMsg');");
                    context.Response.Write("alert('Add New Member successfully!');");
                    context.Response.Write("parent.parent.location.href='../Customer/MemberList.aspx?agentId=" + context.Request["agentId"] + "';");
                }
                else
                {
                    string errMsg = resultUpdated["errMsg"] as string ?? string.Empty;
                    context.Response.Write("parent.alert_error('"+ IBCUtils.Encryption.RemoveSpecialChars(errMsg) +"','errMsg');");
                }
            }
           catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
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
