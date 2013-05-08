<%@ WebHandler Language="C#" Class="UpdateAgent" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using System.Collections;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;


public class UpdateAgent : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
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
                Hashtable hash = new Hashtable();
                hash.Add("custid", context.Request["custId"]); //int
                hash.Add("firstname", context.Request["txtFirstname"]);
                hash.Add("lastname", context.Request["txtLastname"]);
                hash.Add("phone", context.Request["txtPhone"]);
                hash.Add("mobilephone", context.Request["txtMobile"]);
                hash.Add("groupa", context.Request["listGroupA"]);
                hash.Add("groupb", context.Request["listGroupB"]);
                hash.Add("groupc", context.Request["listGroupC"]);
                hash.Add("groupd", context.Request["listGroupD"]);
                hash.Add("discountcs", context.Request["listOther"]);
                if (String.IsNullOrEmpty(context.Request["chkClosed"]))
                {
                    hash.Add("closed", 0); //bit
                }
                else
                {
                    hash.Add("closed", 1); //bit
                }
                hash.Add("mrecommend", context.Request["masterId"]);
                hash.Add("minbet", context.Request["txtMinBet"]);
                hash.Add("maxbet", context.Request["txtMaxBet"]);
                hash.Add("maxpermatch", context.Request["txtMaxPerMatch"]);
                hash.Add("minbetb", context.Request["txtMinBetb"]);
                hash.Add("maxbetb", context.Request["txtMaxBetb"]);
                hash.Add("maxpermatchb", context.Request["txtMaxPerMatchb"]);
                hash.Add("credit", context.Request["txtMaxCredit"]);
                hash.Add("language", "E"); //varchar
                hash.Add("updatemultiple", 0); //bit
                
                //add parameters from Hdp --> O/E of Deadball
                hash.Add("apositiontaking_1", context.Request["listSHdpD"]);
                hash.Add("mpositiontaking_1", context.Request["listMHdpD"]);
                if (context.Request["chkCopyHdpD"] != null) // when Copy from Handicap checkbox is checked
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
                else  // when Copy from Handicap checkbox isn't checked
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
                //add parameters from 1x2 --> Outright of Deadball
                hash.Add("apositiontaking_5", context.Request["listS1x2"]);
                hash.Add("mpositiontaking_5", context.Request["listM1x2"]);
                if (context.Request["chkCopy1x2"] != null)  // when Copy from 1x2 checkbox is checked
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
                else  // when Copy from 1x2 checkbox isn't checked
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
                //add parameters from Hdp --> 1st O/U of Live
                hash.Add("alivepositiontaking_1", context.Request["listSHdpL"]);
                hash.Add("mlivepositiontaking_1", context.Request["listMHdpL"]);
                if (context.Request["chkCopyHdpL"] != null)  // when Copy from Handicap checkbox is checked
                {
                    hash.Add("alivepositiontaking_3", context.Request["listSHdpL"]);
                    hash.Add("alivepositiontaking_7", context.Request["listSHdpL"]);
                    hash.Add("alivepositiontaking_8", context.Request["listSHdpL"]);

                    hash.Add("mlivepositiontaking_3", context.Request["listMHdpL"]);
                    hash.Add("mlivepositiontaking_7", context.Request["listMHdpL"]);
                    hash.Add("mlivepositiontaking_8", context.Request["listMHdpL"]);
                }
                else // when Copy from Handicap checkbox isn't checked
                {
                    hash.Add("alivepositiontaking_3", context.Request["listSOUL"]);
                    hash.Add("alivepositiontaking_7", context.Request["listS1stHdpL"]);
                    hash.Add("alivepositiontaking_8", context.Request["listS1stOUL"]);

                    hash.Add("mlivepositiontaking_3", context.Request["listMOUL"]);
                    hash.Add("mlivepositiontaking_7", context.Request["listM1stHdpL"]);
                    hash.Add("mlivepositiontaking_8", context.Request["listM1stOUL"]);
                }
                //add parameters from Hdp --> O/E of Basketball
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

                hash.Add("updatecommission", 1); //bit
                hash.Add("updateminmaxbet", 1); //bit
                hash.Add("updateminmaxbetb", 1); //bit
                hash.Add("updateclosed", 1); //bit

                if (context.Request["isUpdatePt"] == "1")
                {
                    hash.Add("updatepositiontaking", true); //bit
                    hash.Add("updatepositiontakingb", true); //bit
                }
                else
                {
                    hash.Add("updatepositiontaking", false); //bit
                    hash.Add("updatepositiontakingb", false); //bit
                }
                hash.Add("subaccid", 0); //int
                hash.Add("ip", context.Request.UserHostAddress.ToString());
                hash.Add("returnerrcode", 1); //int
                hash.Add("errcode", 0); //int
                hash.Add("errmsg", "");
                IbcError error = UpdateCustomer.UpdateAgent(hash);
                if (error.IsSuccess())
                {
                    context.Response.Write("parent.alert_error('Update Agent successfully!','errMsg');");
                    if (string.IsNullOrEmpty(context.Request["set"]))
                    {
                        context.Response.Write("parent.opener.location.reload();parent.close();");
                    }
                    else
                    {
                        context.Response.Write("parent.obj('btnUpdate').disabled=false;");
                    }                    
                }
                else
                {
                    context.Response.Write("parent.alert_error(\"" + Encryption.RemoveSpecialChars(error.ErrorMsg) + "\",\"errMsg\");");
                    context.Response.Write("parent.obj('btnUpdate').disabled=false;");
                }
            }
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.alert_error('"+ Encryption.RemoveSpecialChars(ex.Message) +"','errMsg');");
                context.Response.Write("parent.obj('btnUpdate').disabled=false;");
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
