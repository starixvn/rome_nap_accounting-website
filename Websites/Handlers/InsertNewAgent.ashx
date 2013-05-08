<%@ WebHandler Language="C#" Class="InsertNewAgent" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;


public class InsertNewAgent : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {
        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type='text/javascript'>\n");
            Hashtable resultUpdated = new Hashtable();
            try
            {
                System.Collections.Hashtable hash = new System.Collections.Hashtable();
                hash.Add("custid", 0);
                hash.Add("firstname", context.Request["txtFirstname"]);
                hash.Add("lastname", context.Request["txtLastname"]);
                hash.Add("creator", context.Session["AccId"]);
                hash.Add("SubAccID", 0);
                hash.Add("username", context.Request["txtUsername"].Trim());
                hash.Add("userpwd", IBCUtils.Encryption.CfsEnCode(context.Request["txtPwd"].Trim()));
                hash.Add("phone", context.Request["txtPhone"]);
                hash.Add("mobilephone", context.Request["txtMobile"]);
                hash.Add("fax", "");
                hash.Add("email", "");
                hash.Add("address", "");
                hash.Add("city", "");
                hash.Add("state", "");
                hash.Add("country", "");
                hash.Add("postal", "");
                hash.Add("groupa", context.Request["listGroupA"]);
                hash.Add("groupb", context.Request["listGroupB"]);
                hash.Add("groupc", context.Request["listGroupC"]);
                hash.Add("groupd", context.Request["listGroupD"]);
                hash.Add("discountcs", context.Request["listOther"]);
                hash.Add("minbet", context.Request["txtMinBet"]);
                hash.Add("maxbet", context.Request["txtMaxBet"]);
                hash.Add("maxpermatch", context.Request["txtMaxPerMatch"]);
                hash.Add("minbetb", context.Request["txtMinBetb"]);
                hash.Add("maxbetb", context.Request["txtMaxBetb"]);
                hash.Add("maxpermatchb", context.Request["txtMaxPerMatchb"]);
                hash.Add("credit", context.Request["txtMaxCredit"]);
                hash.Add("mrecommend", context.Request["masterId"]);

                hash.Add("apositiontaking_1", context.Request["listSHdpD"]);
                hash.Add("mpositiontaking_1", context.Request["listMHdpD"]);
                if (context.Request["chkCopyHdpD"] != null)
                {
                    hash.Add("apositiontaking_3", context.Request["listSHdpD"]);
                    hash.Add("apositiontaking_7", context.Request["listSHdpD"]);
                    hash.Add("apositiontaking_8", context.Request["listSHdpD"]);
                    hash.Add("apositiontaking_2", context.Request["listSHdpD"]);

                    hash.Add("mpositiontaking_3", context.Request["listMHdpD"]);
                    hash.Add("mpositiontaking_7", context.Request["listMHdpD"]);
                    hash.Add("mpositiontaking_8", context.Request["listMHdpD"]);
                    hash.Add("mpositiontaking_2", context.Request["listMHdpD"]);
                }
                else
                {
                    hash.Add("apositiontaking_3", context.Request["listSOUD"]);
                    hash.Add("apositiontaking_7", context.Request["listS1stHdpD"]);
                    hash.Add("apositiontaking_8", context.Request["listS1stOUD"]);
                    hash.Add("apositiontaking_2", context.Request["listSOE"]);

                    hash.Add("mpositiontaking_3", context.Request["listMOUD"]);
                    hash.Add("mpositiontaking_7", context.Request["listM1stHdpD"]);
                    hash.Add("mpositiontaking_8", context.Request["listM1stOUD"]);
                    hash.Add("mpositiontaking_2", context.Request["listMOE"]);
                }
                hash.Add("apositiontaking_5", context.Request["listS1x2"]);
                hash.Add("mpositiontaking_5", context.Request["listM1x2"]);
                if (context.Request["chkCopy1x2"] != null)
                {
                    hash.Add("apositiontaking_4", context.Request["listS1x2"]);
                    hash.Add("apositiontaking_6", context.Request["listS1x2"]);
                    hash.Add("apositiontaking_9", context.Request["listS1x2"]);
                    hash.Add("apositiontaking_10", context.Request["listS1x2"]);

                    hash.Add("mpositiontaking_4", context.Request["listM1x2"]);
                    hash.Add("mpositiontaking_6", context.Request["listM1x2"]);
                    hash.Add("mpositiontaking_9", context.Request["listM1x2"]);
                    hash.Add("mpositiontaking_10", context.Request["listM1x2"]);
                }
                else
                {
                    hash.Add("apositiontaking_4", context.Request["listSCS"]);
                    hash.Add("apositiontaking_6", context.Request["listSTG"]);
                    hash.Add("apositiontaking_9", context.Request["listSMP"]);
                    hash.Add("apositiontaking_10", context.Request["listSOutright"]);

                    hash.Add("mpositiontaking_4", context.Request["listMCS"]);
                    hash.Add("mpositiontaking_6", context.Request["listMTG"]);
                    hash.Add("mpositiontaking_9", context.Request["listMMP"]);
                    hash.Add("mpositiontaking_10", context.Request["listMOutright"]);
                }
                hash.Add("alivepositiontaking_1", context.Request["listSHdpL"]);
                hash.Add("mlivepositiontaking_1", context.Request["listMHdpL"]);
                if (context.Request["chkCopyHdpL"] != null)
                {
                    hash.Add("alivepositiontaking_3", context.Request["listSHdpL"]);
                    hash.Add("alivepositiontaking_7", context.Request["listSHdpL"]);
                    hash.Add("alivepositiontaking_8", context.Request["listSHdpL"]);

                    hash.Add("mlivepositiontaking_3", context.Request["listMHdpL"]);
                    hash.Add("mlivepositiontaking_7", context.Request["listMHdpL"]);
                    hash.Add("mlivepositiontaking_8", context.Request["listMHdpL"]);
                }
                else
                {
                    hash.Add("alivepositiontaking_3", context.Request["listSOUL"]);
                    hash.Add("alivepositiontaking_7", context.Request["listS1stHdpL"]);
                    hash.Add("alivepositiontaking_8", context.Request["listS1stOUL"]);

                    hash.Add("mlivepositiontaking_3", context.Request["listMOUL"]);
                    hash.Add("mlivepositiontaking_7", context.Request["listM1stHdpL"]);
                    hash.Add("mlivepositiontaking_8", context.Request["listM1stOUL"]);
                }
                hash.Add("bapositiontaking_1", context.Request["listSHdpB"]);
                hash.Add("bmpositiontaking_1", context.Request["listMHdpB"]);
                if (context.Request["chkCopyHdpB"] != null)
                {
                    hash.Add("bapositiontaking_3", context.Request["listSHdpB"]);
                    hash.Add("bapositiontaking_2", context.Request["listSHdpB"]);

                    hash.Add("bmpositiontaking_3", context.Request["listMHdpB"]);
                    hash.Add("bmpositiontaking_2", context.Request["listMHdpB"]);
                }
                else
                {
                    hash.Add("bapositiontaking_3", context.Request["listSOUB"]);
                    hash.Add("bapositiontaking_2", context.Request["listSOEB"]);

                    hash.Add("bmpositiontaking_3", context.Request["listMOUB"]);
                    hash.Add("bmpositiontaking_2", context.Request["listMOEB"]);
                }
                hash.Add("bapositiontaking_7", hash["bapositiontaking_1"]);
                hash.Add("bapositiontaking_8", hash["bapositiontaking_3"]);
                hash.Add("bmpositiontaking_7", hash["bmpositiontaking_1"]);
                hash.Add("bmpositiontaking_8", hash["bmpositiontaking_3"]);
                hash.Add("ip", context.Request.UserHostAddress.ToString());
                hash.Add("language", "E");
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0); //int
                hash.Add("errmsg", ""); //varchar
                resultUpdated = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.AddNewAgent(hash);
                if (Convert.ToString(resultUpdated["errCode"]) == "0")
                {
                    context.Response.Write("alert('Add New Agent Successfully');");
                    context.Response.Write("parent.parent.location.href='../Customer/AgentList.aspx?masterId=" + context.Request["masterId"] + "';");
                }
                else
                {
                    string msg = resultUpdated["errMsg"] as string ?? string.Empty;
                    context.Response.Write("parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(msg) + "','errMsg');");
                }
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(ex.Message) + "','errMsg');");
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
