<%@ WebHandler Language="C#" Class="UpdateMinMaxPT" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;
using Sunplus.Accounting.Business.PT.SportBooks;


public class UpdateMinMaxPT : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {
        
        
        base.ProcessRequest(context);        
        //context.Response.ContentType = "text/html";
        string errmsg = "";
        if (context.Session["AccId"] == null)
        {
        }
        else if (Permissions.CheckPermission(CastAide.AsInt32(context.Session["AccId"]), Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            context.Response.Write(" parent.PTUnBlockUI(); parent.alert_error('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "','errMsg');parent.afterUpdate(false,0); ");
            context.Response.Write("</script>");
            context.Response.Write("</body></html>");
            return;
        }
        else
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                Hashtable param=new Hashtable();
                param.Add("custid", CustId); //int

                string type = context.Request["type"];
                AddParamSoccer(ref param, context, type);
                AddParamBasketball(ref param, context, "2", type);
                AddParamBasketball(ref param, context, "3", type);
                AddParamBasketball(ref param, context, "5", type);
                AddParamBasketball(ref param, context, "10", type);
                AddParamBasketball(ref param, context, "99", type);
                AddParamBaseball(ref param, context, type);
                AddParamFinancial(ref param, context, type);
                AddParamMotosports(ref param, context, type);

                NumberGameManager numbergame = new NumberGameManager();
                numbergame.AddParamMinMaxPT(ref param, context, type);
                //mixed sports
                AddParamMixedSports(ref param, context, type);

                param.Add("updatemaxpos", (type == "super") ? 0 : 1); //bit                
                param.Add("updateminpos", 1); //bit  
                param.Add("ip", context.Request.UserHostAddress.ToString());              
                param.Add("language","E" ); //varchar
                param.Add("userid", context.Session["AccId"]); //int
                param.Add("subaccid", 0); //int              
                param.Add("errcode", 0); //int
                param.Add("errmsg","" ); //nvarchar
                IbcError error;
                //if (type == "super")
                //{
                //    error = UpdateCustomer.UpdMaxMinPT_acc1(param);
                //}
                //else
                //{
                    error = UpdateCustomer.UpdMaxMinPT_acc(param);
                //}
                if (error.IsSuccess())
                {
                    string Max = (type == "super") ? "" : ", max";  
                    string msg = "Update min" + Max +" position taking successfully.";
                    msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success);
                    context.Response.Write("parent.scroll(0,0);parent.PTUnBlockUI(); parent.alert_error('" + msg + "','errMsg');parent.afterUpdate(true,2000);");
                }
                else
                {
                    string msg = Encryption.RemoveSpecialChars(error.ErrorMsg);
                    msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error);
                    context.Response.Write("parent.scroll(0,0);parent.PTUnBlockUI(); parent.alert_error('" + msg + "','errMsg'); ");
                }
            }
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.PTUnBlockUI(); parent.scroll(0,0);  parent.alert_error('" +  AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
                
            }
            
        }        
    }
    

    //'this function is used to add parameters
    //'in Single Edit case get all parameters for Bet Setting and Positiontaking
    //'in Multiple Edit case: 
    //'       if Bet Setting checkbox is checked , value is updated db that is requested from Form 
    //'       if Bet Setting checkbox is unchecked , value is updated db that is 0
    //'       if Position Taking checkbox is checked , value is updated db that is requested from Form 
    //'       if Position Taking checkbox is unchecked , value is updated db that is 0
    void AddParamSoccer(ref Hashtable param, HttpContext context, string type)
    {
        try
        {
            string pre1 = "max", pre2 = "min";
            if (type == "super")
            {
                pre1 = "min";
                pre2 = "max"; 
            }

            if (context.Request["chkCopyAll"] != null) // Copy all Bet Setting and Position Taking from Soccer Handicap
            {
                //Max Position
                param.Add(pre1 + "positiontaking_1", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_2", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_3", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_4", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_5", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_6", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_7", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_8", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_9", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "positiontaking_10", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "livepositiontaking_1", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "livepositiontaking_3", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "livepositiontaking_7", getPT(context.Request["listSHdpD1"])); //smallmoney  
                param.Add(pre1 + "livepositiontaking_8", getPT(context.Request["listSHdpD1"])); //smallmoney              

                //Min Position
                param.Add(pre2 + "positiontaking_1", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_2", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_3", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_4", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_5", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_6", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_7", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_8", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_9", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "positiontaking_10", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "livepositiontaking_1", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "livepositiontaking_3", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "livepositiontaking_7", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "livepositiontaking_8", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "positiontaking_1"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_2"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_3"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_4"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_5"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_6"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_7"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_8"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_9"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "positiontaking_10"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "livepositiontaking_1"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "livepositiontaking_3"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "livepositiontaking_7"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "livepositiontaking_8"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney     
                }
            }
            else
            {
                if (context.Request["chkCopyHdpD1"] != null) //Copy From Handicap
                {
                    //Max Position
                    param.Add(pre1 + "positiontaking_1", getPT(context.Request["listSHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_3", getPT(context.Request["listSHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_7", getPT(context.Request["listSHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_8", getPT(context.Request["listSHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_2", getPT(context.Request["listSHdpD1"])); //smallmoney  

                    //Min Position
                    param.Add(pre2 + "positiontaking_1", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_3", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_7", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_8", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_2", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    
                    // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "positiontaking_1"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_3"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_7"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_8"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_2"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    }
                }
                else
                {
                    //Max Position
                    param.Add(pre1 + "positiontaking_1", getPT(context.Request["listSHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_3", getPT(context.Request["listSOUD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_7", getPT(context.Request["listS1stHdpD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_8", getPT(context.Request["listS1stOUD1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_2", getPT(context.Request["listSOE1"])); //smallmoney  

                    //Min Position
                    param.Add(pre2 + "positiontaking_1", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_3", getPT(context.Request["listMOUD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_7", getPT(context.Request["listM1stHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_8", getPT(context.Request["listM1stOUD1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_2", getPT(context.Request["listMOE1"])); // 'smallmoney  
                    
                    // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "positiontaking_1"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_3"] = getPT(context.Request["listMOUD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_7"] = getPT(context.Request["listM1stHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_8"] = getPT(context.Request["listM1stOUD1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_2"] = getPT(context.Request["listMOE1_Max"]); // 'smallmoney  
                    }
                }

                if (context.Request["chkCopy1x21"] != null) //Copy From 1 X 2
                {
                    //Max Position
                    param.Add(pre1 + "positiontaking_5", getPT(context.Request["listS1x21"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_4", getPT(context.Request["listS1x21"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_6", getPT(context.Request["listS1x21"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_9", getPT(context.Request["listS1x21"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_10", getPT(context.Request["listS1x21"])); //smallmoney  

                    //Min Position
                    param.Add(pre2 + "positiontaking_5", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_4", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_6", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_9", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_10", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    
                    // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "positiontaking_5"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_4"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_6"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_9"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_10"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney 
                    }
                }
                else
                {
                    //Max Position
                    param.Add(pre1 + "positiontaking_5", getPT(context.Request["listS1x21"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_4", getPT(context.Request["listSCS1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_6", getPT(context.Request["listSTG1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_9", getPT(context.Request["listSMP1"])); //smallmoney  
                    param.Add(pre1 + "positiontaking_10", getPT(context.Request["listSOutright1"])); //smallmoney  

                    //Min Position
                    param.Add(pre2 + "positiontaking_5", getPT(context.Request["listM1x21"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_4", getPT(context.Request["listMCS1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_6", getPT(context.Request["listMTG1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_9", getPT(context.Request["listMMP1"])); // 'smallmoney  
                    param.Add(pre2 + "positiontaking_10", getPT(context.Request["listMOutright1"])); // 'smallmoney  
                    
                    // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "positiontaking_5"] = getPT(context.Request["listM1x21_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_4"] = getPT(context.Request["listMCS1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_6"] = getPT(context.Request["listMTG1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_9"] = getPT(context.Request["listMMP1_Max"]); // 'smallmoney  
                        param[pre2 + "positiontaking_10"] = getPT(context.Request["listMOutright1_Max"]); // 'smallmoney  
                    }
                }
                
                if (context.Request["chkCopyHdpL1"] != null) //Copy From Handicap Live
                {
                    //Max Position
                    param.Add(pre1 + "livepositiontaking_1", getPT(context.Request["listSHdpL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_3", getPT(context.Request["listSHdpL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_7", getPT(context.Request["listSHdpL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_8", getPT(context.Request["listSHdpL1"])); //smallmoney              

                    //Min Position
                    param.Add(pre2 + "livepositiontaking_1", getPT(context.Request["listMHdpL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_3", getPT(context.Request["listMHdpL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_7", getPT(context.Request["listMHdpL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_8", getPT(context.Request["listMHdpL1"])); // 'smallmoney  
                    
                     // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "livepositiontaking_1"] = getPT(context.Request["listMHdpL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_3"] = getPT(context.Request["listMHdpL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_7"] = getPT(context.Request["listMHdpL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_8"] = getPT(context.Request["listMHdpL1_Max"]); // 'smallmoney  
                    }
                }
                else
                {
                    //Max Position
                    param.Add(pre1 + "livepositiontaking_1", getPT(context.Request["listSHdpL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_3", getPT(context.Request["listSOUL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_7", getPT(context.Request["listS1stHdpL1"])); //smallmoney  
                    param.Add(pre1 + "livepositiontaking_8", getPT(context.Request["listS1stOUL1"])); //smallmoney              

                    //Min Position
                    param.Add(pre2 + "livepositiontaking_1", getPT(context.Request["listMHdpL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_3", getPT(context.Request["listMOUL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_7", getPT(context.Request["listM1stHdpL1"])); // 'smallmoney  
                    param.Add(pre2 + "livepositiontaking_8", getPT(context.Request["listM1stOUL1"])); // 'smallmoney          
                    
                    // override max PT value
                    if (type == "super")
                    {
                        param[pre2 + "livepositiontaking_1"] = getPT(context.Request["listMHdpL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_3"] = getPT(context.Request["listMOUL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_7"] = getPT(context.Request["listM1stHdpL1_Max"]); // 'smallmoney  
                        param[pre2 + "livepositiontaking_8"] = getPT(context.Request["listM1stOUL1_Max"]); // 'smallmoney
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
    }

    void AddParamBasketball(ref Hashtable param, HttpContext context, string sporttype, string type)
    {
        try
        {
            string pre1 = "max", pre2 = "min";
            if (type == "super")
            {
                pre1 = "min";
                pre2 = "max";
            }
            if (context.Request["chkCopyAll"] != null) //Copy all Bet Setting and Position Taking from Soccer Handicap
            { 
                //Max Position
                param.Add(pre1 + "v" + sporttype +"positiontaking_1", getPT(context.Request["listSHdpD1"])) ; // 'smallmoney  
                param.Add(pre1 + "v" + sporttype +"positiontaking_2", getPT(context.Request["listSHdpD1"])) ; // 'smallmoney  
                param.Add(pre1 + "v" + sporttype +"positiontaking_3", getPT(context.Request["listSHdpD1"])) ; // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre1 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listSHdpD1"])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listSHdpD1"])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listSHdpD1"])); // 'smallmoney  
                }
                param.Add(pre1 + "v" + sporttype +"positiontaking_20", getPT(context.Request["listSHdpD1"])) ; // 'smallmoney  
                
                //Min Position
                param.Add(pre2 + "v" + sporttype +"positiontaking_1", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype +"positiontaking_2", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype +"positiontaking_3", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre2 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listMHdpD1"])); // 'smallmoney                      
                }
                param.Add(pre2 + "v" + sporttype +"positiontaking_20", getPT(context.Request["listMHdpD1"])); // 'smallmoney  
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v" + sporttype + "positiontaking_1"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_2"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_3"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                    if (sporttype == "2")
                    {
                        param[pre2 + "v" + sporttype + "positiontaking_7"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_8"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_9"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney                      
                    }
                    param[pre2 + "v" + sporttype + "positiontaking_20"] = getPT(context.Request["listMHdpD1_Max"]); // 'smallmoney  
                }
            }
            else if (context.Request["chkCopyBasket"] != null ) // Copy all Bet Setting and Position Taking from Basketball Handicap 
            {
                //Max Position
                param.Add(pre1 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listSHdpB2"])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listSHdpB2"])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listSHdpB2"])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre1 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listSHdpB2"])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listSHdpB2"])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listSHdpB2"])); // 'smallmoney                      
                }
                param.Add(pre1 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listSHdpB2"])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre2 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                }
                param.Add(pre2 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listMHdpB2"])); // 'smallmoney  
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v" + sporttype + "positiontaking_1"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_2"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_3"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                    if (sporttype == "2")
                    {
                        param[pre2 + "v" + sporttype + "positiontaking_7"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_8"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_9"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney  
                    }
                    param[pre2 + "v" + sporttype + "positiontaking_20"] = getPT(context.Request["listMHdpB2_Max"]); // 'smallmoney 
                }
            }
            else if (context.Request["chkCopyHdpB" + sporttype] != null) // Copy From Handicap
            {
                //Max Position
                param.Add(pre1 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre1 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                }
                param.Add(pre1 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre2 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                }
                param.Add(pre2 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v" + sporttype + "positiontaking_1"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_2"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_3"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                    if (sporttype == "2")
                    {
                        param[pre2 + "v" + sporttype + "positiontaking_7"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_8"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_9"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                    }
                    param[pre2 + "v" + sporttype + "positiontaking_20"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                }
            }
            else
            {
                //Max Position
                param.Add(pre1 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listSOEB" + sporttype])); // 'smallmoney  
                param.Add(pre1 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listSOUB" + sporttype])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre1 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listSHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listSOUB" + sporttype])); // 'smallmoney  
                    param.Add(pre1 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listSMPB" + sporttype])); // 'smallmoney  
                }
                param.Add(pre1 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listSMLB" + sporttype])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v" + sporttype + "positiontaking_1", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_2", getPT(context.Request["listMOEB" + sporttype])); // 'smallmoney  
                param.Add(pre2 + "v" + sporttype + "positiontaking_3", getPT(context.Request["listMOUB" + sporttype])); // 'smallmoney  
                if (sporttype == "2")
                {
                    param.Add(pre2 + "v" + sporttype + "positiontaking_7", getPT(context.Request["listMHdpB" + sporttype])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_8", getPT(context.Request["listMOUB" + sporttype])); // 'smallmoney  
                    param.Add(pre2 + "v" + sporttype + "positiontaking_9", getPT(context.Request["listMMPB" + sporttype])); // 'smallmoney  
                }
                param.Add(pre2 + "v" + sporttype + "positiontaking_20", getPT(context.Request["listMMLB" + sporttype])); // 'smallmoney              
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v" + sporttype + "positiontaking_1"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_2"] = getPT(context.Request["listMOEB" + sporttype + "_Max"]); // 'smallmoney  
                    param[pre2 + "v" + sporttype + "positiontaking_3"] = getPT(context.Request["listMOUB" + sporttype + "_Max"]); // 'smallmoney  
                    if (sporttype == "2")
                    {
                        param[pre2 + "v" + sporttype + "positiontaking_7"] = getPT(context.Request["listMHdpB" + sporttype + "_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_8"] = getPT(context.Request["listMOUB" + sporttype + "_Max"]); // 'smallmoney  
                        param[pre2 + "v" + sporttype + "positiontaking_9"] = getPT(context.Request["listMMPB" + sporttype + "_Max"]); // 'smallmoney  
                    }
                    param[pre2 + "v" + sporttype + "positiontaking_20"] = getPT(context.Request["listMMLB" + sporttype + "_Max"]); // 'smallmoney              
                }
            }
            
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
    }

    void AddParamBaseball(ref Hashtable param, HttpContext content, string type)
    {
        try
        {
            string pre1 = "max", pre2 = "min";
            if (type == "super")
            {
                pre1 = "min";
                pre2 = "max";
            }

            if (content.Request["chkCopyAll"] != null) //Copy all Bet Setting and Position Taking from Soccer Handicap
            { 
                //Max Position
                param.Add(pre1 + "v8positiontaking_1", getPT(content.Request["listSHdpD1"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_3", getPT(content.Request["listSHdpD1"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_20", getPT(content.Request["listSHdpD1"])); // 'smallmoney  
                
                //Min Position
                param.Add(pre2 + "v8positiontaking_1", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_3", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_20", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v8positiontaking_1"] = getPT(content.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_3"] = getPT(content.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_20"] = getPT(content.Request["listMHdpD1_Max"]); // 'smallmoney 
                }
            }
            else if (content.Request["chkCopyBasket"] != null) //Copy all Bet Setting and Position Taking from Basketball Handicap
            {
                //Max Position
                param.Add(pre1 + "v8positiontaking_1", getPT(content.Request["listSHdpB2"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_3", getPT(content.Request["listSHdpB2"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_20", getPT(content.Request["listSHdpB2"])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v8positiontaking_1", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_3", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_20", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v8positiontaking_1"] = getPT(content.Request["listMHdpB2_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_3"] = getPT(content.Request["listMHdpB2_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_20"] = getPT(content.Request["listMHdpB2_Max"]); // 'smallmoney 
                }
            }
            else if (content.Request["chkCopyHdpB8"] != null) //Copy From Handicap
            {
                //Max Position
                param.Add(pre1 + "v8positiontaking_1", getPT(content.Request["listSHdpB8"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_3", getPT(content.Request["listSHdpB8"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_20", getPT(content.Request["listSHdpB8"])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v8positiontaking_1", getPT(content.Request["listMHdpB8"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_3", getPT(content.Request["listMHdpB8"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_20", getPT(content.Request["listMHdpB8"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v8positiontaking_1"] = getPT(content.Request["listMHdpB8_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_3"] = getPT(content.Request["listMHdpB8_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_20"] = getPT(content.Request["listMHdpB8_Max"]); // 'smallmoney 
                }
            }
            else
            {
                //Max Position
                param.Add(pre1 + "v8positiontaking_1", getPT(content.Request["listSHdpB8"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_3", getPT(content.Request["listSOUB8"])); // 'smallmoney  
                param.Add(pre1 + "v8positiontaking_20", getPT(content.Request["listSMLB8"])); // 'smallmoney  

                //Min Position
                param.Add(pre2 + "v8positiontaking_1", getPT(content.Request["listMHdpB8"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_3", getPT(content.Request["listMOUB8"])); // 'smallmoney  
                param.Add(pre2 + "v8positiontaking_20", getPT(content.Request["listMMLB8"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v8positiontaking_1"] = getPT(content.Request["listMHdpB8_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_3"] = getPT(content.Request["listMOUB8_Max"]); // 'smallmoney  
                    param[pre2 + "v8positiontaking_20"] = getPT(content.Request["listMMLB8_Max"]); // 'smallmoney  
                }
            }          
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
    }

    void AddParamFinancial (ref Hashtable param, HttpContext content, string type)
    {
        try
        {
            string pre1 = "max", pre2 = "min";
            if (type == "super")
            {
                pre1 = "min";
                pre2 = "max";
            }
            if (WebSettings.disabledFI)
            {
            
                
                // Max Position
                param.Add(pre1 + "v201positiontaking_3", content.Request["hiddenSOUB201"]); // 'smallmoney

                //Min Postion
                param.Add(pre2 + "v201positiontaking_3", content.Request["hiddenMOUB201"]); // 'smallmoney
                
                //Odd/Even
                // Max Position
                param.Add(pre1 + "v201positiontaking_2", content.Request["hiddenSOEB201"]); // 'smallmoney

                //Min Postion
                param.Add(pre2 + "v201positiontaking_2", content.Request["hiddenMOEB201"]); // 'smallmoney
                //END
            }
            else
            {
                if (content.Request["chkCopyAll"] != null) //Copy all Bet Setting and Position Taking from Soccer Handicap
                {
                    // Max Position
                    param.Add(pre1 + "v201positiontaking_3", getPT(content.Request["listSHdpD1"])); // 'smallmoney

                    //Min Postion
                    param.Add(pre2 + "v201positiontaking_3", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                    
                    //Odd/Even
                    // Max Position
                    param.Add(pre1 + "v201positiontaking_2", getPT(content.Request["listSHdpD1"])); // 'smallmoney

                    //Min Postion
                    param.Add(pre2 + "v201positiontaking_2", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                    //END

                }
                else if (content.Request["chkCopyBasket"] != null) //Copy all Bet Setting and Position Taking from Basketball Handicap
                {
                    // Max Position
                    param.Add(pre1 + "v201positiontaking_3", getPT(content.Request["listSHdpB2"])); // 'smallmoney

                    //Min Postion
                    param.Add(pre2 + "v201positiontaking_3", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                    
                    //Odd/Even
                    // Max Position
                    param.Add(pre1 + "v201positiontaking_2", getPT(content.Request["listSHdpB2"])); // 'smallmoney

                    //Min Postion
                    param.Add(pre2 + "v201positiontaking_2", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                    //END

                }
                else
                {
                    if (content.Request["chkCopyOU201"] != null)
                    {
                        // Max Position
                        param.Add(pre1 + "v201positiontaking_3", getPT(content.Request["listSOUB201"])); // 'smallmoney

                        //Min Postion
                        param.Add(pre2 + "v201positiontaking_3", getPT(content.Request["listMOUB201"])); // 'smallmoney  
                        
                        //Odd/Even
                        // Max Position
                        param.Add(pre1 + "v201positiontaking_2", getPT(content.Request["listSOUB201"])); // 'smallmoney

                        //Min Postion
                        param.Add(pre2 + "v201positiontaking_2", getPT(content.Request["listMOUB201"])); // 'smallmoney
                        //END
                    }
                    else
                    {
                        // Max Position
                        param.Add(pre1 + "v201positiontaking_3", getPT(content.Request["listSOUB201"])); // 'smallmoney

                        //Min Postion
                        param.Add(pre2 + "v201positiontaking_3", getPT(content.Request["listMOUB201"])); // 'smallmoney  

                        //Odd/Even
                        // Max Position
                        param.Add(pre1 + "v201positiontaking_2", getPT(content.Request["listSOEB201"])); // 'smallmoney

                        //Min Postion
                        param.Add(pre2 + "v201positiontaking_2", getPT(content.Request["listMOEB201"])); // 'smallmoney
                        //END
                    }
                   

                }
            }
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
    }

    void AddParamMotosports(ref Hashtable param, HttpContext content, string type)
    {
        try
        {
            string pre1 = "max", pre2 = "min";
            if (type == "super")
            {
                pre1 = "min";
                pre2 = "max";
            }
            if (content.Request["chkCopyAll"] != null)//Copy all Bet Setting and Position Taking from Soccer Handicap
            { 
                // Max Postion
                param.Add(pre1 + "v11positiontaking_1", getPT(content.Request["listSHdpD1"])); // 'smallmoney  
                param.Add(pre1 + "v11positiontaking_20", getPT(content.Request["listSHdpD1"])); // 'smallmoney  
                
                //Min Postion
                param.Add(pre2 + "v11positiontaking_1", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                param.Add(pre2 + "v11positiontaking_20", getPT(content.Request["listMHdpD1"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v11positiontaking_1"] = getPT(content.Request["listMHdpD1_Max"]); // 'smallmoney  
                    param[pre2 + "v11positiontaking_20"] = getPT(content.Request["listMHdpD1_Max"]); // 'smallmoney 
                }
            }
            else if (content.Request["chkCopyBasket"] != null)// Copy all Bet Setting and Position Taking from Basketball Handicap
            {
                // Max Postion
                param.Add(pre1 + "v11positiontaking_1", getPT(content.Request["listSHdpB2"])); // 'smallmoney
                param.Add(pre1 + "v11positiontaking_20", getPT(content.Request["listSHdpB2"])); // 'smallmoney

                //Min Postion
                param.Add(pre2 + "v11positiontaking_1", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                param.Add(pre2 + "v11positiontaking_20", getPT(content.Request["listMHdpB2"])); // 'smallmoney  
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v11positiontaking_1"] = getPT(content.Request["listMHdpB2_Max"]); // 'smallmoney  
                    param[pre2 + "v11positiontaking_20"] = getPT(content.Request["listMHdpB2_Max"]); // 'smallmoney 
                }
            }
            else if (content.Request["chkCopyHdpB11"] != null)// Copy From Handicap
            {
                // Max Postion
                param.Add(pre1 + "v11positiontaking_1", getPT(content.Request["listSHdpB11"])); // 'smallmoney
                param.Add(pre1 + "v11positiontaking_20", getPT(content.Request["listSHdpB11"])); // 'smallmoney

                //Min Postion
                param.Add(pre2 + "v11positiontaking_1", getPT(content.Request["listMHdpB11"])); // 'smallmoney  
                param.Add(pre2 + "v11positiontaking_20", getPT(content.Request["listMHdpB11"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v11positiontaking_1"] = getPT(content.Request["listMHdpB11_Max"]); // 'smallmoney  
                    param[pre2 + "v11positiontaking_20"] = getPT(content.Request["listMHdpB11_Max"]); // 'smallmoney 
                }
            }
            else
            {
                // Max Postion
                param.Add(pre1 + "v11positiontaking_1", getPT(content.Request["listSHdpB11"])); // 'smallmoney
                param.Add(pre1 + "v11positiontaking_20", getPT(content.Request["listSMLB11"])); // 'smallmoney

                //Min Postion
                param.Add(pre2 + "v11positiontaking_1", getPT(content.Request["listMHdpB11"])); // 'smallmoney  
                param.Add(pre2 + "v11positiontaking_20", getPT(content.Request["listMMLB11"])); // 'smallmoney  
                
                // override max PT value
                if (type == "super")
                {
                    param[pre2 + "v11positiontaking_1"] = getPT(content.Request["listMHdpB11_Max"]); // 'smallmoney  
                    param[pre2 + "v11positiontaking_20"] = getPT(content.Request["listMMLB11_Max"]); // 'smallmoney  
                }
            }
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
    }
    
    void AddParamMixedSports(ref Hashtable param, HttpContext context, string type)
    {
        var mixedSports = new MixedSportsManager();
        string super, master;
        
        if (context.Request["chkCopyAll"] != null)
        {
            super = getPT(context.Request["listSHdpD1"]);
            master = getPT(context.Request["listMHdpD1"]);
            // override max PT value
            if (type.Equals("super"))
            {
                master = getPT(context.Request["listMHdpD1_Max"]);
            }
        }
        else if (context.Request["chkCopyBasket"] != null)
        {
            super = getPT(context.Request["listSHdpB2"]);
            master = getPT(context.Request["listMHdpB2"]);
            // override max PT value
            if (type.Equals("super"))
            {
                master = getPT(context.Request["listMHdpB2_Max"]);
            }
        }
        else
        {
            super = getPT(context.Request["listSMixParlay"]);
            master = getPT(context.Request["listMMixParlay"]);
            // override max PT value
            if (type.Equals("super"))
            {
                master = getPT(context.Request["listMMixParlays"]);
            }
            
        }
        
        mixedSports.AddParamMinMaxPT(ref param, type, super, master);
    }

    string getPT(string pt)
    {
        if (pt == null) return "0";
        return pt;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
