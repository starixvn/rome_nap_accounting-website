<%@ WebHandler Language="C#" Class="UpdateMember" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils.DAL;
using IBCUtils;


public class UpdateMember : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {

        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            string errMsg = "";
            bool ptupdsoccer;
            bool ptupdbasketball;
            try
            {
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                ht.Add("custid", context.Request.Form["custId"]);
                ht.Add("firstname", context.Request["txtFirstname"]);
                ht.Add("lastname", context.Request["txtLastname"]);
                ht.Add("phone", context.Request["txtPhone"]);
                ht.Add("mobilephone", context.Request["txtMobile"]);
                ht.Add("credit", context.Request["txtMaxCredit"]);
                //**************
                ht.Add("updatemultiple", false);
                ht.Add("updatecommission", 1);
                ht.Add("updateminmaxbet", 1);
                ht.Add("updateminmaxbetb", 1);
                ht.Add("updatepositiontaking",1);
                ptupdsoccer = Convert.ToBoolean(1);
                ht.Add("updatepositiontakingb",1);
                ptupdbasketball = Convert.ToBoolean(1);
                ht.Add("updateclosed", 1);
                //**************
                ht.Add("playerdiscount", context.Request.Form["listCommMem1"]);
                ht.Add("playerdiscountcs", context.Request.Form["listCommMem5"]);
                ht.Add("minbet", context.Request["txtMinBet"]);
                ht.Add("maxbet", context.Request["txtMaxBet"]);
                ht.Add("maxpermatch", context.Request["txtMaxPerMatch"]);
                ht.Add("minbetb", context.Request["txtMinBetb"]);
                ht.Add("maxbetb", context.Request["txtMaxBetb"]);
                ht.Add("maxpermatchb", context.Request["txtMaxPerMatchb"]);
                if (String.IsNullOrEmpty(context.Request.Form["chkClosed"]))
                {
                    ht.Add("closed", 0);
                }
                else
                {
                    ht.Add("closed", 1);
                }
                //**************
                //**************
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
                    ht.Add("apositiontaking_4", context.Request["listSCS"]);
                    ht.Add("apositiontaking_9", context.Request["listSMP"]);
                    ht.Add("apositiontaking_5", context.Request["listS1x2"]);
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

                ht.Add("userid", context.Session["AccId"]);
                ht.Add("discountproblem", 0);
                ht.Add("discountcsproblem", 0);
                ht.Add("agent_discount", 0);
                ht.Add("agent_discountcs", 0);
                ht.Add("ip", context.Request.UserHostAddress.ToString()); 
                ht.Add("returnerrcode", 1);
                ht.Add("errcode", 0);
                ht.Add("errmsg", "");                
                
                // ht.Add("username", "");
                var rs = UpdateCustomer.CustomerUpdEditCustomerAgRpt(ht);
                if (Convert.ToInt32(rs["errcode"])==0)
                {
                    if (Convert.ToBoolean(rs["discountproblem"]) == true)
                    {
                        errMsg += rs["username"] + ":: Comm" + context.Request.Form["listCommMem1"] + "Agent Comm" + rs["agent_discount"] + ". System set Comm:" + rs["agent_discount"] + "for this member.";
                    }
                    if (Convert.ToBoolean(rs["discountcsproblem"]) == true)
                    {
                        errMsg += rs["username"] + ":: Comm" + context.Request.Form["listCommMem5"] + " Agent Comm" + rs["agent_discountcs"] + ". System set Comm:" + rs["agent_discountcs"] + "for this member.";
                    }
                    bool position = false;
                    if (FormatHandler.GetDouble(rs["apositiontaking_1"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpD"]))
                    {
                        position = true;
                    }
                    if (!String.IsNullOrEmpty(context.Request["chkCopyHdpD"]))
                    {
                        if (FormatHandler.GetDouble(rs["apositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_7"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_8"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_2"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpD"]))
                            position = true;
                    }
                    else
                    {
                        if (FormatHandler.GetDouble(rs["apositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSOUD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_7"]) != FormatHandler.GetDouble(context.Request.Form["listS1stHdpD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_8"]) != FormatHandler.GetDouble(context.Request.Form["listS1stOUD"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_2"]) != FormatHandler.GetDouble(context.Request.Form["listSOE"]))
                            position = true;
                    }
                    if (FormatHandler.GetDouble(rs["apositiontaking_5"]) != FormatHandler.GetDouble(context.Request.Form["listS1x2"]))
                        position = true;

                    if (!String.IsNullOrEmpty(context.Request["chkCopy1x2"]))
                    {
                        if (FormatHandler.GetDouble(rs["apositiontaking_4"]) != FormatHandler.GetDouble(context.Request.Form["listS1x2"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_9"]) != FormatHandler.GetDouble(context.Request.Form["listS1x2"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_6"]) != FormatHandler.GetDouble(context.Request.Form["listS1x2"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_10"]) != FormatHandler.GetDouble(context.Request.Form["listS1x2"]))
                            position = true;

                    }
                    else
                    {
                        if (FormatHandler.GetDouble(rs["apositiontaking_4"]) != FormatHandler.GetDouble(context.Request.Form["listSCS"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_9"]) != FormatHandler.GetDouble(context.Request.Form["listSMP"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_6"]) != FormatHandler.GetDouble(context.Request.Form["listSTG"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["apositiontaking_10"]) != FormatHandler.GetDouble(context.Request.Form["listSOutright"]))
                            position = true;
                    }

                    if (FormatHandler.GetDouble(rs["alivepositiontaking_1"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpL"]))
                        position = true;

                    if (!String.IsNullOrEmpty(context.Request["chkCopyHdpL"]))
                    {
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpL"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_7"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpL"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_8"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpL"]))
                            position = true;
                    }
                    else
                    {
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSOUL"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_7"]) != FormatHandler.GetDouble(context.Request.Form["listS1stHdpL"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["alivepositiontaking_8"]) != FormatHandler.GetDouble(context.Request.Form["listS1stOUL"]))
                            position = true;
                    }
                    if (FormatHandler.GetDouble(rs["bapositiontaking_1"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpB"]))
                        position = true;
                    if (!String.IsNullOrEmpty(context.Request["chkCopyHdpB"]))
                    {
                        if (FormatHandler.GetDouble(rs["bapositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpB"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["bapositiontaking_2"]) != FormatHandler.GetDouble(context.Request.Form["listSHdpB"]))
                            position = true;
                    }
                    else
                    {
                        if (FormatHandler.GetDouble(rs["bapositiontaking_3"]) != FormatHandler.GetDouble(context.Request.Form["listSOUB"]))
                            position = true;
                        if (FormatHandler.GetDouble(rs["bapositiontaking_2"]) != FormatHandler.GetDouble(context.Request.Form["listSOEB"]))
                            position = true;
                    }

                    if (ptupdsoccer)
                    {
                        if (Convert.ToInt32(rs["updatepositiontaking"]) == 0)
                            errMsg += "Customer" + rs["username"] + " --> Update positiontaking is unsuccessful.";
                    }
                    else if (ptupdbasketball)
                    {
                        if (Convert.ToInt32(rs["updatepositiontakingb"]) == 0)
                            errMsg += "Customer" + rs["username"] + " --> Update positiontaking is unsuccessful.";
                    }
                    if (string.IsNullOrEmpty(errMsg))
                    {

                        context.Response.Write("parent.alert_error('Update member successfully!','errMsg');");
                        if (string.IsNullOrEmpty(context.Request["set"]))
                        {
                            context.Response.Write("parent.close();");
                            context.Response.Write("parent.opener.location.reload();");
                        }
                        else
                        {
                            context.Response.Write("parent.obj('btnUpdate').disabled=false;");
                        }
                    }
                    else
                    {
                        context.Response.Write("parent.alert_error(\"" + IBCUtils.Encryption.RemoveSpecialChars(errMsg) + "\",\"errMsg\");");
                        context.Response.Write("parent.obj('btnUpdate').disabled=false;");
                    }
                }
                else
                {
                    errMsg = rs["errmsg"] as string ?? string.Empty;
                    context.Response.Write("parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(errMsg) + "','errMsg');");
                    context.Response.Write("parent.obj('btnUpdate').disabled=false;");
                }
            }
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.alert_error('"+ IBCUtils.Encryption.RemoveSpecialChars(ex.Message) +"','errMsg');");
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
