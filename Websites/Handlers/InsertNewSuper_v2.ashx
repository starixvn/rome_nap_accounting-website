<%@ WebHandler Language="C#" Class="InsertNewSuper_v2" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Bingo.Services;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using System.Collections.Generic;
using System.Collections;
using System.Web.SessionState;
using DNAide.Core;


public class InsertNewSuper_v2 : BaseHandler {

    private CustDetail _custDetail =
        Sunplus.Accounting.Business.Customer.CustomerService.GetCustInfo(HttpContext.Current.Request);
    protected override int CurrencyId
    {
        get
        {
            return _custDetail.CurrencyId;
        }
    }
    public override void ProcessRequest (HttpContext context) 
    {            
          
        context.Response.ContentType = "text/html";
        base.ProcessRequest(context);
        string errmsg="";
        if (context.Session["AccId"] == null)
        {
            return;
        }
        else if (Permissions.CheckPermission(CastAide.AsInt32(context.Session["AccId"]), Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            context.Response.Write("<script>parent.scroll('0', '0');parent.alert_error('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "');parent.PTUnBlockUI();</script>");
            return;
        }
        else
        {
            try
            {
                System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentUICulture;
                Hashtable resultUpdated = new Hashtable();
                Hashtable hash = new Hashtable();

                var groupa = context.Request["listGroupA"];
                var custId = 0;
                var ip = context.Request.UserHostAddress;
                var isInternal = CastAide.AsString(context.Request["chkInternal"], "off") == "on";
                
                hash.Add("custid", custId); //int
                hash.Add("firstname", _custDetail.FirstName); //nvarchar
                hash.Add("lastname", _custDetail.LastName); //nvarchar
                hash.Add("address", _custDetail.Address); //nvarchar
                hash.Add("postal", _custDetail.Postal); //nvarchar
                hash.Add("city", _custDetail.City); //nvarchar
                hash.Add("state", _custDetail.State); //nvarchar
                hash.Add("country", _custDetail.Country); //nvarchar
                hash.Add("phone", _custDetail.Phone); //nvarchar
                hash.Add("mobilephone", _custDetail.MobilePhone); //nvarchar
                hash.Add("fax", _custDetail.Fax); //nvarchar
                hash.Add("email", _custDetail.Email); //nvarchar
                hash.Add("refcode1", ""); //nvarchar
                hash.Add("creator", AppSession.AccId); //int
                hash.Add("username", _custDetail.UserName); //varchar
                hash.Add("userpwd", _custDetail.Password); //varchar
                hash.Add("site", context.Request["listSite"]); //nvarchar                
                hash.Add("currency", _custDetail.CurrencyId); //int              
                hash.Add("credit", context.Request["txtCredit"]); //money
                hash.Add("groupa", groupa); //decimal
                hash.Add("groupb", context.Request["listGroupB"]); //decimal
                hash.Add("groupc", context.Request["listGroupC"]); //decimal
                hash.Add("groupd", context.Request["listGroupD"]); //decimal
                hash.Add("discountcs", context.Request["listOther"]); //decimal
                hash.Add("isinternal", isInternal); //decimal

                //Check if login Admin got Create finance Account

                if (AppSession.SYSTEM.ToLower() != Enumerations.AccountingSystem.Accounting.ToString().ToLower())
                {
                        context.Response.Write("<script>parent.scroll('0', '0');parent.alert_error('" + AlertMessage.GetMsg(Resources.ErrorMsg._500, AlertMessage.MsgType.Error) + "');parent.PTUnBlockUI();</script>");
                        return;
                }
                
                if(isInternal)
                {
                    if (!Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.CreateFinanceAccount))
                    {
                        context.Response.Write("<script>parent.scroll('0', '0');parent.alert_error('" + AlertMessage.GetMsg(Resources.ErrorMsg.error_CreateFinanceAccountPermission, AlertMessage.MsgType.Error) + "');parent.PTUnBlockUI();</script>");
                        return;
                    }
                }

                if (!WebSettings.Disable1x2Comm)
                {
                    hash.Add("discount1x2", context.Request["listGroup1x2"]); //decimal
                }
               
                if (WebSettings.disabledFI)
                {
                    hash.Add("v201discount", 0); //decimal
                }
                else
                {
                    hash.Add("v201discount", context.Request["listFCComm"]); //decimal
                }
                hash.Add("ip", ip);
                if (WebSettings.disabledHR == true)
                {
                    hash.Add("disablehorseracing", 1);
                    hash.Add("v151discount_31", 0); //decimal
                    hash.Add("v151discount_32", 0); //decimal                
                    AddParam151(ref hash, context, "151", false);
                }
                else
                {
                    hash.Add("disablehorseracing", Convert.ToInt16(context.Request["hdAllowCurr"])==1? 0:1);
                    hash.Add("v151discount_31", context.Request["listUKHRComm"]); //decimal
                    hash.Add("v151discount_32", context.Request["listOtherHRComm"]); //decimal 
                    AddParam151(ref hash, context, "151", true);
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
                AddParam201(ref hash, context, "201");

                Sunplus.Accounting.Business.PT.SportBooks.NumberGameManager numbergame = new Sunplus.Accounting.Business.PT.SportBooks.NumberGameManager();
                numbergame.AddParamSuperPT(ref hash, context);
                //mixed sports
                Sunplus.Accounting.Business.PT.SportBooks.MixedSportsManager mixedSports = new Sunplus.Accounting.Business.PT.SportBooks.MixedSportsManager();
                mixedSports.AddParamSuperPT(ref hash, context);
                
                hash.Add("language", "E"); //varchar              
                hash.Add("errcode", 0); //int
                hash.Add("errmsg", ""); //varchar
                resultUpdated = UpdateCustomer.Acc_CustomerInsMember_Com(hash);
                
                if (Convert.ToString(resultUpdated["errCode"]) == "0")
                {
                    var superId = CastAide.AsInt32(resultUpdated["superId"]);
                    //Insert this super to Cust Group if currency system is not Accounting
                    if(!AppSession.SYSTEM.ToLower().Equals(Enumerations.AccountingSystem.Accounting.ToString().ToLower()))
                    {
                        var customerService = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.IUserService>();
                        var result = customerService.InsertCustomerToGroup(superId, 4, AppSession.SYSTEM);
                    }
                        
                    //if (context.Request["hidCS"] != null))))
                    if (!WebSettings.disabledFI )
                    {
                        string strUrl = "../Customer/V2/UpdatePT.aspx?custid=" + superId + "&insert=true&roleid=4&uplineid=0&exchangeId=" + _custDetail.CurrencyId;
                        context.Response.Write("<script>setTimeout('ReLoad()', 3000); function ReLoad() { parent.PTUnBlockUI();if (parent.name=='centerFr') { parent.location.replace('" + strUrl + "'); } else {parent.parent.location.replace('" + strUrl + "')} }; </script>");
                    }
                    else
                    {
                        string strQuery = "strSearch=search&txtSearch=" + context.Request["txtUsername"];
                        context.Response.Write("<script>parent.DelayReloadPage('../SuperAgentList.aspx?" + strQuery + "&hr=" + (!WebSettings.disabledHR) + "', 2000);</script>");
                    }
                }
                else
                {
                    string msg = resultUpdated["errMsg"] as string ?? string.Empty;
                    if (msg != "")
                    {
                        msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error);
                    }
                    context.Response.Write("<script>parent.scroll('0', '0');parent.alert_error('" + msg + "','err'); parent.PTUnBlockUI();</script>");

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                context.Response.Write("<script>parent.ShowErrorInsert('" + ex.Message.Replace("'", "").Replace("\"", "") + "');parent.PTUnBlockUI();</script>");
            }
        }
    }

    void AddParam1_1(ref Hashtable param, HttpContext context)
    {
        
        param.Add("minbet", context.Request["txtMinbet1"]) ;//money  
        param.Add("maxbet", context.Request["txtMaxbet1"]);//money  
        param.Add("maxpermatch", context.Request["txtMaxPerMatch1"]);//money  
        param.Add("disablefinancial", (context.Request["chkDisabledFI"]) == "yes");
        
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
        if (WebSettings.disabledFI)
        {
            //param.Add("minbetv201", 0);//money  
            //param.Add("maxbetv201", 0);//money  
            //param.Add("maxpermatchv201", 0);//money  

            ////super
            //param.Add("v201spositiontaking_3", 0);//smallmoney              

            ////master
            //param.Add("v201mpositiontaking_3", 0);//smallmoney  

            ////agent
            //param.Add("v201apositiontaking_3", 0);//smallmoney  
            
            ////Odd/Even
            //param.Add("v201spositiontaking_2", 0);//smallmoney              

            ////master
            //param.Add("v201mpositiontaking_2", 0);//smallmoney  

            ////agent
            //param.Add("v201apositiontaking_2", 0);//smallmoney  
            ////END   
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
                param.Add("minbetv201", context.Request["txtMinbet2"]);//money  
                param.Add("maxbetv201", context.Request["txtMaxbet2"]);//money  
                param.Add("maxpermatchv201", context.Request["txtMaxPerMatch2"]);//money  

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
                //End          
            }
            else
            {
                param.Add("minbetv201", context.Request["txtMinbet201"]);//money  
                param.Add("maxbetv201", context.Request["txtMaxbet201"]);//money  
                param.Add("maxpermatchv201", context.Request["txtMaxPerMatch201"]);//money  
                if (!string.IsNullOrEmpty(context.Request["chkCopyOU201"])) //copy all from FI Over/Under
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
                    //End
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
                    //End
                }
                       
            }
        }
    }
    void AddParam151(ref Hashtable param, HttpContext context, string sporttype, bool IsAllowCurr)
    {
        if (IsAllowCurr==true || WebSettings.disabledHR ==false )
        {
            if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) //copy all from soccer handicap
            {
                param.Add("minbetv151", context.Request["txtMinbet1"]);//money  
                param.Add("maxbetv151", context.Request["txtMaxbet1"]);//money  
                param.Add("maxpermatchv151", context.Request["txtMaxPerMatch1"]);//money  

            }
            else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"])) //copy all from basketball handicap
            {
                param.Add("minbetv151", context.Request["txtMinbet2"]);//money  
                param.Add("maxbetv151", context.Request["txtMaxbet2"]);//money  
                param.Add("maxpermatchv151", context.Request["txtMaxPerMatch2"]);//money  

            }
            else
            {
                param.Add("minbetv151", context.Request["txtMinbet151"]);//money  
                param.Add("maxbetv151", context.Request["txtMaxbet151"]);//money  
                param.Add("maxpermatchv151", context.Request["txtMaxPerMatch151"]);//money  
            }
        }
        else
        {
            param.Add("minbetv151", 0);//money  
            param.Add("maxbetv151", 0);//money  
            param.Add("maxpermatchv151", 0);//money  
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}
