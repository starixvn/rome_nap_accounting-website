<%@ WebHandler Language="C#" Class="UpdateSuper_v2" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using System.Collections;
using DNAide.Core;


public class UpdateSuper_v2 : BaseHandler {
    
    public override  void ProcessRequest (HttpContext context) 
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/html";
        string errmsg="";
        //string custid = "0";
        if (context.Session["AccId"] == null)
        {
            //string parent = "";
            //if (context.Request["set"].Equals("1")) parent = "parent.";
            //else parent = "parent.parent.";
            errmsg = "Session timeout.";
            context.Response.Write("<script> parent.PTUnBlockUI();parent.scroll('0', '0');parent.alert_error('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "','err');parent.afterUpdate(false,0);</script>");
            return;
        }
        else if (Permissions.CheckPermission(CastAide.AsInt32(context.Session["AccId"]), Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            //string parent = "";
            //if (context.Request["set"].Equals("1")) parent = "parent.";
            //else parent = "parent.parent.";
            context.Response.Write("<script>parent.PTUnBlockUI(); parent.scroll('0', '0');parent.alert_error('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "','err');parent.afterUpdate(false,0);</script>");
            return;
        }
        else
        {
       
            try
            {
                System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentUICulture;
                Hashtable resultUpdated = new Hashtable();

                var custDetail = Sunplus.Accounting.Business.Customer.CustomerService.GetCustInfo(context.Request);
                
                var custId = CustId;

                if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                {
                    if (context.Request["chkClosed"] == "yes")
                        custDetail.Closed = true;    
                }
                else
                {

                    if (context.Request["chkClosed"] == "yes")
                        custDetail.Closed = true;    
                }

                
                if(context.Request["chkSuspended"] == "yes")
                    custDetail.Suspend = true;
                Hashtable hash = new Hashtable();
                hash.Add("custid", custId); //int
                hash.Add("userid", AppSession.AccId); //int    
                hash.Add("firstname", custDetail.FirstName); //nvarchar
                hash.Add("lastname", custDetail.LastName); //nvarchar
                hash.Add("address", custDetail.Address); //nvarchar
                hash.Add("postal", custDetail.Postal); //nvarchar
                hash.Add("city", custDetail.City); //nvarchar
                hash.Add("state", custDetail.State); //nvarchar
                hash.Add("country", custDetail.Country); //nvarchar
                hash.Add("phone", custDetail.Phone); //nvarchar
                hash.Add("mobilephone", custDetail.MobilePhone); //nvarchar
                hash.Add("fax", custDetail.Fax); //nvarchar
                hash.Add("closed", custDetail.Closed); //bit
                hash.Add("suspended", custDetail.Suspend); //bit
                
                //if (CastAide.AsInt32(context.Request["hisexternal"]) == 1)
                    //hash.Add("credit", context.Request["holdmaxcredit"]); //money
                //else
                    hash.Add("credit", context.Request["txtCredit"]); //money
                
                hash.Add("groupa", context.Request["listGroupA"]); //decimal
                hash.Add("groupb", context.Request["listGroupB"]); //decimal
                hash.Add("groupc", context.Request["listGroupC"]); //decimal
                hash.Add("groupd", context.Request["listGroupD"]); //decimal
                hash.Add("discountcs", context.Request["listOther"]); //decimal



                if (!WebSettings.Disable1x2Comm)
                {
                    hash.Add("discount1x2", context.Request["listGroup1x2"]); //decimal
                }
                
              
                AddParam1_1(ref hash, context);
                AddParam1_2(ref hash, context);
                AddParam1_3(ref hash, context);
                AddParam(ref hash, context, "2");
                AddParam(ref hash, context, "3");
                AddParam(ref hash, context, "5");
                AddParam(ref hash, context, "10");
                AddParam(ref hash, context, "99");
                AddParam8(ref hash, context, "8");
                AddParam11(ref hash, context, "11");
                //AddParam201(ref hash, context, "201");

                Sunplus.Accounting.Business.PT.SportBooks.NumberGameManager numbergame = new Sunplus.Accounting.Business.PT.SportBooks.NumberGameManager();
                numbergame.AddParamSuperPT(ref hash, context);
                //mixed sports
                Sunplus.Accounting.Business.PT.SportBooks.MixedSportsManager mixedSports = new Sunplus.Accounting.Business.PT.SportBooks.MixedSportsManager();
                mixedSports.AddParamSuperPT(ref hash, context);
                
                hash.Add("ip", context.Request.UserHostAddress.ToString());
                hash.Add("language", "E"); //varchar

                hash.Add("errcode", ""); //int
                hash.Add("errmsg", ""); //varchar
                IBCUtils.IbcError rs = UpdateCustomer.CustomerUpdEditCustomerCom_acc(hash);
                if (rs.IsSuccess())
                {
                    
                    // SynCS
                    if (Permissions.IsAllowCasino() && context.Request["hidSynCS"] == "True")
                    {
                        // Store data for casino section
                        CSCustomerDetail custInfo = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustInfoIBC(hash["custid"].ToString(), hash);
                        // Sync data to Casino
                        CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
                        csCustomer.SiteName = custInfo.SiteName;
                        bool status = csCustomer.UpdateCustInfo(custInfo.IBCCustID, custInfo.FirstName, custInfo.LastName, custInfo.Phone, custInfo.MobilePhone, custInfo.Fax, custInfo.Closed, custInfo.Suspend, context.Request.UserHostAddress, AppSession.CustID, AppSession.Username, 3);
                    }
                                                            
                    context.Response.Write("<script>parent.scroll('0','0');parent.showAlertMessage('err','Update Super successfully',0);parent.PTUnBlockUI();parent.afterUpdate(true,2000); </script>");
                   
                }
                else
                {
                    string msg = rs.ErrorMsg;
                    context.Response.Write("<script>parent.PTUnBlockUI();parent.scroll('0','0');parent.alert_error('" + msg + "','err');</script>");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("<script> parent.PTUnBlockUI(); parent.showAlertMessage('err','" + StringUtils.ParseToJSMsg(ex.Message) + "',1); </script>");
            }
        }
    }

    void AddParam1_1(ref Hashtable param, HttpContext context)
    {
        param.Add("minbet", context.Request["txtMinbet1"]) ;//money  
        param.Add("maxbet", context.Request["txtMaxbet1"]);//money   
        param.Add("maxpermatch", context.Request["txtMaxPerMatch1"]);//money  

        if (!string.IsNullOrEmpty(context.Request["chkCopyHdpD1"]) || !string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy from Handicap
        {
            //super
            param.Add("spositiontaking_1", context.Request["listSHdpD1"]); //smallmoney  
            param.Add("spositiontaking_3", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_7", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_8", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_2", context.Request["listSHdpD1"]); //smallmoney  

            //master
            param.Add("mpositiontaking_1", context.Request["listMHdpD1"]); //smallmoney  
            param.Add("mpositiontaking_3", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_7", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_8", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_2", context.Request["listMHdpD1"]); //smallmoney  

            //agent
            param.Add("apositiontaking_1", context.Request["listAHdpD1"]); //smallmoney  
            param.Add("apositiontaking_3", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_7", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_8", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_2", context.Request["listAHdpD1"]); //smallmoney  
        }
        else
        {
            //super
            param.Add("spositiontaking_1", context.Request["listSHdpD1"]); //smallmoney  
            param.Add("spositiontaking_3", context.Request["listSOUD1"]); //'smallmoney  
            param.Add("spositiontaking_7", context.Request["listS1stHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_8", context.Request["listS1stOUD1"]); //'smallmoney  
            param.Add("spositiontaking_2", context.Request["listSOE1"]); //smallmoney  

            //master
            param.Add("mpositiontaking_1", context.Request["listMHdpD1"]); //smallmoney  
            param.Add("mpositiontaking_3", context.Request["listMOUD1"]); //'smallmoney  
            param.Add("mpositiontaking_7", context.Request["listM1stHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_8", context.Request["listM1stOUD1"]); //'smallmoney  
            param.Add("mpositiontaking_2", context.Request["listMOE1"]); //smallmoney  

            //agent
            param.Add("apositiontaking_1", context.Request["listAHdpD1"]); //smallmoney  
            param.Add("apositiontaking_3", context.Request["listAOUD1"]); //'smallmoney  
            param.Add("apositiontaking_7", context.Request["listA1stHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_8", context.Request["listA1stOUD1"]); //'smallmoney  
            param.Add("apositiontaking_2", context.Request["listAOE1"]); //smallmoney  
        }
    }

    void AddParam1_2(ref Hashtable param, HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy all from handicap
        {
            //super
            param.Add("spositiontaking_5", context.Request["listSHdpD1"]); //smallmoney  
            param.Add("spositiontaking_4", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_6", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_9", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("spositiontaking_10", context.Request["listSHdpD1"]); //smallmoney  

            //master
            param.Add("mpositiontaking_5", context.Request["listMHdpD1"]); //smallmoney  
            param.Add("mpositiontaking_4", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_6", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_9", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mpositiontaking_10", context.Request["listMHdpD1"]); //smallmoney  

            //agent
            param.Add("apositiontaking_5", context.Request["listAHdpD1"]); //smallmoney  
            param.Add("apositiontaking_4", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_6", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_9", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("apositiontaking_10", context.Request["listAHdpD1"]); //smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopy1x21"])) //copy from 1X2
        {
            //super
            param.Add("spositiontaking_5", context.Request["listS1x21"]); //smallmoney  
            param.Add("spositiontaking_4", context.Request["listS1x21"]); //'smallmoney  
            param.Add("spositiontaking_6", context.Request["listS1x21"]); //'smallmoney  
            param.Add("spositiontaking_9", context.Request["listS1x21"]); //'smallmoney  
            param.Add("spositiontaking_10", context.Request["listS1x21"]); //smallmoney  

            //master
            param.Add("mpositiontaking_5", context.Request["listM1x21"]); //smallmoney  
            param.Add("mpositiontaking_4", context.Request["listM1x21"]); //'smallmoney  
            param.Add("mpositiontaking_6", context.Request["listM1x21"]); //'smallmoney  
            param.Add("mpositiontaking_9", context.Request["listM1x21"]); //'smallmoney  
            param.Add("mpositiontaking_10", context.Request["listM1x21"]); //smallmoney  

            //agent
            param.Add("apositiontaking_5", context.Request["listA1x21"]); //smallmoney  
            param.Add("apositiontaking_4", context.Request["listA1x21"]); //'smallmoney  
            param.Add("apositiontaking_6", context.Request["listA1x21"]); //'smallmoney  
            param.Add("apositiontaking_9", context.Request["listA1x21"]); //'smallmoney  
            param.Add("apositiontaking_10", context.Request["listA1x21"]); //smallmoney  
        }
        else
        {
            //super
            param.Add("spositiontaking_5", context.Request["listS1x21"]); //smallmoney  
            param.Add("spositiontaking_4", context.Request["listSCS1"]); //'smallmoney  
            param.Add("spositiontaking_6", context.Request["listSTG1"]); //'smallmoney  
            param.Add("spositiontaking_9", context.Request["listSMP1"]); //'smallmoney  
            param.Add("spositiontaking_10", context.Request["listSOutright1"]); //smallmoney  

            //master
            param.Add("mpositiontaking_5", context.Request["listM1x21"]); //smallmoney  
            param.Add("mpositiontaking_4", context.Request["listMCS1"]); //'smallmoney  
            param.Add("mpositiontaking_6", context.Request["listMTG1"]); //'smallmoney  
            param.Add("mpositiontaking_9", context.Request["listMMP1"]); //'smallmoney  
            param.Add("mpositiontaking_10", context.Request["listMOutright1"]); //smallmoney  

            //agent
            param.Add("apositiontaking_5", context.Request["listA1x21"]); //smallmoney  
            param.Add("apositiontaking_4", context.Request["listACS1"]); //'smallmoney  
            param.Add("apositiontaking_6", context.Request["listATG1"]); //'smallmoney  
            param.Add("apositiontaking_9", context.Request["listAMP1"]); //'smallmoney  
            param.Add("apositiontaking_10", context.Request["listAOutright1"]); //smallmoney  
        }    
    }

    void AddParam1_3(ref Hashtable param, HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy all from Handicap
        {
            //super
            param.Add("slivepositiontaking_1", context.Request["listSHdpD1"]); //smallmoney  
            param.Add("slivepositiontaking_3", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("slivepositiontaking_7", context.Request["listSHdpD1"]); //'smallmoney  
            param.Add("slivepositiontaking_8", context.Request["listSHdpD1"]); //'smallmoney  

            //master
            param.Add("mlivepositiontaking_1", context.Request["listMHdpD1"]); //smallmoney  
            param.Add("mlivepositiontaking_3", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mlivepositiontaking_7", context.Request["listMHdpD1"]); //'smallmoney  
            param.Add("mlivepositiontaking_8", context.Request["listMHdpD1"]); //'smallmoney  

            //agent
            param.Add("alivepositiontaking_1", context.Request["listAHdpD1"]); //smallmoney  
            param.Add("alivepositiontaking_3", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("alivepositiontaking_7", context.Request["listAHdpD1"]); //'smallmoney  
            param.Add("alivepositiontaking_8", context.Request["listAHdpD1"]); //'smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyHdpL1"])) //copy from Handicap
        {
            //super
            param.Add("slivepositiontaking_1", context.Request["listSHdpL1"]); //smallmoney  
            param.Add("slivepositiontaking_3", context.Request["listSHdpL1"]); //'smallmoney  
            param.Add("slivepositiontaking_7", context.Request["listSHdpL1"]); //'smallmoney  
            param.Add("slivepositiontaking_8", context.Request["listSHdpL1"]); //'smallmoney  

            //master
            param.Add("mlivepositiontaking_1", context.Request["listMHdpL1"]); //smallmoney  
            param.Add("mlivepositiontaking_3", context.Request["listMHdpL1"]); //'smallmoney  
            param.Add("mlivepositiontaking_7", context.Request["listMHdpL1"]); //'smallmoney  
            param.Add("mlivepositiontaking_8", context.Request["listMHdpL1"]); //'smallmoney  

            //agent
            param.Add("alivepositiontaking_1", context.Request["listAHdpL1"]); //smallmoney  
            param.Add("alivepositiontaking_3", context.Request["listAHdpL1"]); //'smallmoney  
            param.Add("alivepositiontaking_7", context.Request["listAHdpL1"]); //'smallmoney  
            param.Add("alivepositiontaking_8", context.Request["listAHdpL1"]); //'smallmoney  
        }
        else
        {
            //super
            param.Add("slivepositiontaking_1", context.Request["listSHdpL1"]); //smallmoney  
            param.Add("slivepositiontaking_3", context.Request["listSOUL1"]); //'smallmoney  
            param.Add("slivepositiontaking_7", context.Request["listS1stHdpL1"]); //'smallmoney  
            param.Add("slivepositiontaking_8", context.Request["listS1stOUL1"]); //'smallmoney  

            //master
            param.Add("mlivepositiontaking_1", context.Request["listMHdpL1"]); //smallmoney  
            param.Add("mlivepositiontaking_3", context.Request["listMOUL1"]); //'smallmoney  
            param.Add("mlivepositiontaking_7", context.Request["listM1stHdpL1"]); //'smallmoney  
            param.Add("mlivepositiontaking_8", context.Request["listM1stOUL1"]); //'smallmoney  

            //agent
            param.Add("alivepositiontaking_1", context.Request["listAHdpL1"]); //smallmoney  
            param.Add("alivepositiontaking_3", context.Request["listAOUL1"]); //'smallmoney  
            param.Add("alivepositiontaking_7", context.Request["listA1stHdpL1"]); //'smallmoney  
            param.Add("alivepositiontaking_8", context.Request["listA1stOUL1"]); //'smallmoney  
        }
    }

    /// <summary>
    /// this function apply for sport type : 2, 3, 5, 9, 10, 99
    /// </summary>
    /// <param name="param"></param>
    /// <param name="context"></param>
    /// <param name="sporttype"></param>
    void AddParam(ref Hashtable param, HttpContext context, string sporttype)
    {

        if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) // copy all from soccer handicap
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet1"]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet1"]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch1"]);//money  
            
            //super
            param.Add("v" + sporttype + "spositiontaking_1", context.Request["listSHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_2", context.Request["listSHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_3", context.Request["listSHdpD1"]);//smallmoney   
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "spositiontaking_7", context.Request["listSHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_8", context.Request["listSHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_9", context.Request["listSHdpD1"]);//smallmoney  
            }
            param.Add("v" + sporttype + "spositiontaking_20", context.Request["listSHdpD1"]);//smallmoney  

            //master
            param.Add("v" + sporttype + "mpositiontaking_1", context.Request["listMHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_2", context.Request["listMHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_3", context.Request["listMHdpD1"]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "mpositiontaking_7", context.Request["listMHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_8", context.Request["listMHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_9", context.Request["listMHdpD1"]);//smallmoney  
            }
            param.Add("v" + sporttype + "mpositiontaking_20", context.Request["listMHdpD1"]);//smallmoney  

            //agent
            param.Add("v" + sporttype + "apositiontaking_1", context.Request["listAHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_2", context.Request["listAHdpD1"]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_3", context.Request["listAHdpD1"]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "apositiontaking_7", context.Request["listAHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_8", context.Request["listAHdpD1"]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_9", context.Request["listAHdpD1"]);//smallmoney  
            }
            param.Add("v" + sporttype + "apositiontaking_20", context.Request["listAHdpD1"]);//smallmoney   
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"]))//copy from basketball
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet2"]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet2"]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch2"]);//money  

            //super
            param.Add("v" + sporttype + "spositiontaking_1", context.Request["listSHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_2", context.Request["listSHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_3", context.Request["listSHdpB2"]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "spositiontaking_7", context.Request["listSHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_8", context.Request["listSHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_9", context.Request["listSHdpB2"]);//smallmoney  
            }
            param.Add("v" + sporttype + "spositiontaking_20", context.Request["listSHdpB2"]);//smallmoney  

            //master
            param.Add("v" + sporttype + "mpositiontaking_1", context.Request["listMHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_2", context.Request["listMHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_3", context.Request["listMHdpB2"]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "mpositiontaking_7", context.Request["listMHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_8", context.Request["listMHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_9", context.Request["listMHdpB2"]);//smallmoney  
            }
            param.Add("v" + sporttype + "mpositiontaking_20", context.Request["listMHdpB2"]);//smallmoney  

            //agent
            param.Add("v" + sporttype + "apositiontaking_1", context.Request["listAHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_2", context.Request["listAHdpB2"]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_3", context.Request["listAHdpB2"]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "apositiontaking_7", context.Request["listAHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_8", context.Request["listAHdpB2"]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_9", context.Request["listAHdpB2"]);//smallmoney  
            }
            param.Add("v" + sporttype + "apositiontaking_20", context.Request["listAHdpB2"]);//smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyHdpB" + sporttype])) //copy from Handicap
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v" + sporttype + "spositiontaking_1", context.Request["listSHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_2", context.Request["listSHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_3", context.Request["listSHdpB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "spositiontaking_7", context.Request["listSHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_8", context.Request["listSHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_9", context.Request["listSHdpB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "spositiontaking_20", context.Request["listSHdpB" + sporttype]);//smallmoney  

            //master
            param.Add("v" + sporttype + "mpositiontaking_1", context.Request["listMHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_2", context.Request["listMHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_3", context.Request["listMHdpB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "mpositiontaking_7", context.Request["listMHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_8", context.Request["listMHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_9", context.Request["listMHdpB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "mpositiontaking_20", context.Request["listMHdpB" + sporttype]);//smallmoney  

            //agent
            param.Add("v" + sporttype + "apositiontaking_1", context.Request["listAHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_2", context.Request["listAHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_3", context.Request["listAHdpB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "apositiontaking_7", context.Request["listAHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_8", context.Request["listAHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_9", context.Request["listAHdpB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "apositiontaking_20", context.Request["listAHdpB" + sporttype]);//smallmoney  
        }
        else
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v" + sporttype + "spositiontaking_1", context.Request["listSHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_2", context.Request["listSOEB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "spositiontaking_3", context.Request["listSOUB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "spositiontaking_7", context.Request["listSHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_8", context.Request["listSOUB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "spositiontaking_9", context.Request["listSMPB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "spositiontaking_20", context.Request["listSMLB" + sporttype]);//smallmoney  

            //super
            param.Add("v" + sporttype + "mpositiontaking_1", context.Request["listMHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_2", context.Request["listMOEB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "mpositiontaking_3", context.Request["listMOUB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "mpositiontaking_7", context.Request["listMHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_8", context.Request["listMOUB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "mpositiontaking_9", context.Request["listMMPB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "mpositiontaking_20", context.Request["listMMLB" + sporttype]);//smallmoney  

            //agent
            param.Add("v" + sporttype + "apositiontaking_1", context.Request["listAHdpB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_2", context.Request["listAOEB" + sporttype]);//smallmoney  
            param.Add("v" + sporttype + "apositiontaking_3", context.Request["listAOUB" + sporttype]);//smallmoney  
            if (sporttype == "2")
            {
                param.Add("v" + sporttype + "apositiontaking_7", context.Request["listAHdpB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_8", context.Request["listAOUB" + sporttype]);//smallmoney  
                param.Add("v" + sporttype + "apositiontaking_9", context.Request["listAMPB" + sporttype]);//smallmoney  
            }
            param.Add("v" + sporttype + "apositiontaking_20", context.Request["listAMLB" + sporttype]);//smallmoney  
        }
    }

    void AddParam8(ref Hashtable param, HttpContext context, string sporttype)
    {
        if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy all from soccer handicap
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet1"]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet1"]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch1"]);//money  
            
            //super
            param.Add("v8spositiontaking_1", context.Request["listSHdpD1"]) ;//smallmoney  
            param.Add("v8spositiontaking_3", context.Request["listSHdpD1"]) ;//smallmoney  
            param.Add("v8spositiontaking_20", context.Request["listSHdpD1"]);//smallmoney  
            
            //master
            param.Add("v8mpositiontaking_1", context.Request["listMHdpD1"]);//smallmoney  
            param.Add("v8mpositiontaking_3", context.Request["listMHdpD1"]);//smallmoney  
            param.Add("v8mpositiontaking_20", context.Request["listMHdpD1"]);//smallmoney  

            //agent
            param.Add("v8apositiontaking_1", context.Request["listAHdpD1"]);//smallmoney  
            param.Add("v8apositiontaking_3", context.Request["listAHdpD1"]);//smallmoney  
            param.Add("v8apositiontaking_20", context.Request["listAHdpD1"]);//smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"])) //copy all from basketball handicap
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet2"]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet2"]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch2"]);//money  

            //super
            param.Add("v8spositiontaking_1", context.Request["listSHdpB2"]);//smallmoney  
            param.Add("v8spositiontaking_3", context.Request["listSHdpB2"]);//smallmoney  
            param.Add("v8spositiontaking_20", context.Request["listSHdpB2"]);//smallmoney  

            //master
            param.Add("v8mpositiontaking_1", context.Request["listMHdpB2"]);//smallmoney  
            param.Add("v8mpositiontaking_3", context.Request["listMHdpB2"]);//smallmoney  
            param.Add("v8mpositiontaking_20", context.Request["listMHdpB2"]);//smallmoney  

            //agent
            param.Add("v8apositiontaking_1", context.Request["listAHdpB2"]);//smallmoney  
            param.Add("v8apositiontaking_3", context.Request["listAHdpB2"]);//smallmoney  
            param.Add("v8apositiontaking_20", context.Request["listAHdpB2"]);//smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyHdpB8"])) //copy all from basketball handicap
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v8spositiontaking_1", context.Request["listSHdpB8"]);//smallmoney  
            param.Add("v8spositiontaking_3", context.Request["listSHdpB8"]);//smallmoney  
            param.Add("v8spositiontaking_20", context.Request["listSHdpB8"]);//smallmoney  

            //master
            param.Add("v8mpositiontaking_1", context.Request["listMHdpB8"]);//smallmoney  
            param.Add("v8mpositiontaking_3", context.Request["listMHdpB8"]);//smallmoney  
            param.Add("v8mpositiontaking_20", context.Request["listMHdpB8"]);//smallmoney  

            //agent
            param.Add("v8apositiontaking_1", context.Request["listAHdpB8"]);//smallmoney  
            param.Add("v8apositiontaking_3", context.Request["listAHdpB8"]);//smallmoney  
            param.Add("v8apositiontaking_20", context.Request["listAHdpB8"]);//smallmoney  
        }
        else
        {
            param.Add("minbetv" + sporttype, context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv" + sporttype, context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv" + sporttype, context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v8spositiontaking_1", context.Request["listSHdpB8"]);//smallmoney  
            param.Add("v8spositiontaking_3", context.Request["listSOUB8"]);//smallmoney  
            param.Add("v8spositiontaking_20", context.Request["listSMLB8"]);//smallmoney  

            //master
            param.Add("v8mpositiontaking_1", context.Request["listMHdpB8"]);//smallmoney  
            param.Add("v8mpositiontaking_3", context.Request["listMOUB8"]);//smallmoney  
            param.Add("v8mpositiontaking_20", context.Request["listMMLB8"]);//smallmoney  

            //agent
            param.Add("v8apositiontaking_1", context.Request["listAHdpB8"]);//smallmoney  
            param.Add("v8apositiontaking_3", context.Request["listAOUB8"]);//smallmoney  
            param.Add("v8apositiontaking_20", context.Request["listAMLB8"]);//smallmoney  
        }
    }

    void AddParam11(ref Hashtable param, HttpContext context, string sporttype)
    {
        if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) // copy all from soccer handicap
        { 
            param.Add("minbetv11", context.Request["txtMinbet1"]) ;//money  
            param.Add("maxbetv11", context.Request["txtMaxbet1"]);//money  
            param.Add("maxpermatchv11", context.Request["txtMaxPerMatch1"]);//money  
            
            //super
            param.Add("v11spositiontaking_1", context.Request["listSHdpD1"]) ;//smallmoney  
            param.Add("v11spositiontaking_20", context.Request["listSHdpD1"]);//smallmoney  

            //master
            param.Add("v11mpositiontaking_1", context.Request["listMHdpD1"]);//smallmoney  
            param.Add("v11mpositiontaking_20", context.Request["listMHdpD1"]);//smallmoney  

            //agent
            param.Add("v11apositiontaking_1", context.Request["listAHdpD1"]);//smallmoney  
            param.Add("v11apositiontaking_20", context.Request["listAHdpD1"]);//smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"])) //copy all from basketball handicap
        {
            param.Add("minbetv11", context.Request["txtMinbet2"]);//money  
            param.Add("maxbetv11", context.Request["txtMaxbet2"]);//money  
            param.Add("maxpermatchv11", context.Request["txtMaxPerMatch2"]);//money  

            //super
            param.Add("v11spositiontaking_1", context.Request["listSHdpB2"]);//smallmoney  
            param.Add("v11spositiontaking_20", context.Request["listSHdpB2"]);//smallmoney  

            //master
            param.Add("v11mpositiontaking_1", context.Request["listMHdpB2"]);//smallmoney  
            param.Add("v11mpositiontaking_20", context.Request["listMHdpB2"]);//smallmoney  

            //agent
            param.Add("v11apositiontaking_1", context.Request["listAHdpB2"]);//smallmoney  
            param.Add("v11apositiontaking_20", context.Request["listAHdpB2"]);//smallmoney  
        }
        else if (!string.IsNullOrEmpty(context.Request["chkCopyHdpB11"])) //copy from handicap
        {
            param.Add("minbetv11", context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv11", context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv11", context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v11spositiontaking_1", context.Request["listSHdpB11"]);//smallmoney  
            param.Add("v11spositiontaking_20", context.Request["listSHdpB11"]);//smallmoney  

            //master
            param.Add("v11mpositiontaking_1", context.Request["listMHdpB11"]);//smallmoney  
            param.Add("v11mpositiontaking_20", context.Request["listMHdpB11"]);//smallmoney  

            //agent
            param.Add("v11apositiontaking_1", context.Request["listAHdpB11"]);//smallmoney  
            param.Add("v11apositiontaking_20", context.Request["listAHdpB11"]);//smallmoney  
        }
        else
        {
            param.Add("minbetv11", context.Request["txtMinbet" + sporttype]);//money  
            param.Add("maxbetv11", context.Request["txtMaxbet" + sporttype]);//money  
            param.Add("maxpermatchv11", context.Request["txtMaxPerMatch" + sporttype]);//money  

            //super
            param.Add("v11spositiontaking_1", context.Request["listSHdpB11"]);//smallmoney  
            param.Add("v11spositiontaking_20", context.Request["listSMLB11"]);//smallmoney  

            //master
            param.Add("v11mpositiontaking_1", context.Request["listMHdpB11"]);//smallmoney  
            param.Add("v11mpositiontaking_20", context.Request["listMMLB11"]);//smallmoney  

            //agent
            param.Add("v11apositiontaking_1", context.Request["listAHdpB11"]);//smallmoney  
            param.Add("v11apositiontaking_20", context.Request["listAMLB11"]);//smallmoney  
        }
    }

    void AddParam201(ref Hashtable param, HttpContext context, string sporttype)
    {
        if (Sunplus.Accounting.Core.Configurations.WebSettings.disabledFI)
        {
            param.Add("minbetv201", context.Request["hiddenMinbet201"]);//money  
            param.Add("maxbetv201", context.Request["hiddenMaxbet201"]);//money  
            param.Add("maxpermatchv201", context.Request["hiddenMaxpermatch201"]);//money  

            //super
            param.Add("v201spositiontaking_3", context.Request["hiddenSOUB201"]);//smallmoney          

            //master
            param.Add("v201mpositiontaking_3", context.Request["hiddenMOUB201"]);//smallmoney                          

            //agent
            param.Add("v201apositiontaking_3", context.Request["hiddenAOUB201"]);//smallmoney
            
            //Odd/Even
            //super
            param.Add("v201spositiontaking_2", context.Request["hiddenSOEB201"]);//smallmoney          
            //master
            param.Add("v201mpositiontaking_2", context.Request["hiddenMOEB201"]);//smallmoney                          
            //agent
            param.Add("v201apositiontaking_2", context.Request["hiddenAOEB201"]);//smallmoney
            //END
        }
        else
        {
            if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy all from soccer handicap
            {
                param.Add("minbetv201", context.Request["txtMinbet1"]);//money  
                param.Add("maxbetv201", context.Request["txtMaxbet1"]);//money  
                param.Add("maxpermatchv201", context.Request["txtMaxPerMatch1"]);//money  

                //super
                param.Add("v201spositiontaking_3", context.Request["listSHdpD1"]);//smallmoney          

                //master
                param.Add("v201mpositiontaking_3", context.Request["listMHdpD1"]);//smallmoney                          

                //agent
                param.Add("v201apositiontaking_3", context.Request["listAHdpD1"]);//smallmoney  
                
                //Odd/Even
                //super
                param.Add("v201spositiontaking_2", context.Request["listSHdpD1"]);//smallmoney          
                //master
                param.Add("v201mpositiontaking_2", context.Request["listMHdpD1"]);//smallmoney                          
                //agent
                param.Add("v201apositiontaking_2", context.Request["listAHdpD1"]);//smallmoney  
                //END            
            }
            else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"])) //copy all from basketball handicap
            {
                
                param.Add("minbetv201", context.Request["txtMinbet3"]);//money  
                param.Add("maxbetv201", context.Request["txtMaxbet3"]);//money  
                param.Add("maxpermatchv201", context.Request["txtMaxPerMatch3"]);//money  

                //super
                param.Add("v201spositiontaking_3", context.Request["listSHdpB2"]);//smallmoney              

                //master
                param.Add("v201mpositiontaking_3", context.Request["listMHdpB2"]);//smallmoney              

                //agent
                param.Add("v201apositiontaking_3", context.Request["listAHdpB2"]);//smallmoney      
                
                //Odd/Even
                //super
                param.Add("v201spositiontaking_2", context.Request["listSHdpB2"]);//smallmoney              
                //master
                param.Add("v201mpositiontaking_2", context.Request["listMHdpB2"]);//smallmoney              
                //agent
                param.Add("v201apositiontaking_2", context.Request["listAHdpB2"]);//smallmoney      
                //END        
            }
            else
            {
                param.Add("minbetv201", context.Request["txtMinbet201"]);//money  
                param.Add("maxbetv201", context.Request["txtMaxbet201"]);//money  
                param.Add("maxpermatchv201", context.Request["txtMaxPerMatch201"]);//money  

                if (!string.IsNullOrEmpty(context.Request["chkCopyOU201"])) //copy all from basketball handicap
                {
                    //super
                    param.Add("v201spositiontaking_3", context.Request["listSOUB201"]);//smallmoney              

                    //master
                    param.Add("v201mpositiontaking_3", context.Request["listMOUB201"]);//smallmoney  

                    //agent
                    param.Add("v201apositiontaking_3", context.Request["listAOUB201"]);//smallmoney    
                    
                    //Odd/Even
                    //super
                    param.Add("v201spositiontaking_2", context.Request["listSOUB201"]);//smallmoney              
                    //master
                    param.Add("v201mpositiontaking_2", context.Request["listMOUB201"]);//smallmoney  
                    //agent
                    param.Add("v201apositiontaking_2", context.Request["listAOUB201"]);//smallmoney  
                    //END
                }   
                else
                {
                    //super
                    param.Add("v201spositiontaking_3", context.Request["listSOUB201"]);//smallmoney              

                    //master
                    param.Add("v201mpositiontaking_3", context.Request["listMOUB201"]);//smallmoney  

                    //agent
                    param.Add("v201apositiontaking_3", context.Request["listAOUB201"]);//smallmoney    

                    //Odd/Even
                    //super
                    param.Add("v201spositiontaking_2", context.Request["listSOEB201"]);//smallmoney              
                    //master
                    param.Add("v201mpositiontaking_2", context.Request["listMOEB201"]);//smallmoney  
                    //agent
                    param.Add("v201apositiontaking_2", context.Request["listAOEB201"]);//smallmoney  
                    //END
                }       
            }
        }
    }   
    public bool IsReusable {
        get {
            return false;
        }
    }

}
