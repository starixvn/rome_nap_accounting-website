<%@ WebHandler Language="C#" Class="UpdateMaster" %>

using System;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;


public class UpdateMaster : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/html";
        context.Response.CacheControl = "no-cache";
        if (context.Session["AccId"] == null)
        {
            FormsAuthentication.SignOut();
            context.Session.Clear();
            context.Response.Write("<script>parent.alert_error('"+IBCUtils.IbcError.SESSION_EXPIRED_MSG+"','errMsg');parent.close();</script>");
            context.Response.End();
        }
        else
        {
            try
            {
                
                Hashtable hash = new Hashtable();
                hash.Add("custid", context.Request["custId"]);//int
                hash.Add("firstname", context.Request["txtFirstname"]); //nvarchar
                hash.Add("lastname", context.Request["txtLastname"]); //nvarchar
                hash.Add("phone", context.Request["txtPhone"]); //nvarchar
                hash.Add("mobilephone", context.Request["txtMobile"]); //nvarchar
                hash.Add("srecommend", context.Request["superId"]);//int
                hash.Add("credit", context.Request["txtMaxCredit"]);//money
                hash.Add("minbet", context.Request["txtMinBet"]);//money
                hash.Add("maxbet", context.Request["txtMaxBet"]);//money
                hash.Add("maxpermatch", context.Request["txtMaxPerMatch"]);//money
                hash.Add("minbetb", context.Request["txtMinBetb"]);//money
                hash.Add("maxbetb", context.Request["txtMaxBetb"]);//money
                hash.Add("maxpermatchb", context.Request["txtMaxPerMatchb"]);//money
                
                hash.Add("language", "E");//varchar
                hash.Add("updatemultiple", 0);//bit

                if (context.Request["chkClosed"]=="yes")
                {
                    hash.Add("closed", true);//bit
                }
                else
                {
                    hash.Add("closed", false);//bit
                }



                hash.Add("mpositiontaking_1", context.Request["listSHdpD"]);// smallmoney = 0.7,
                if (String.IsNullOrEmpty(context.Request["chkCopyHdpD"]))
                {
                    hash.Add("mpositiontaking_3", context.Request["listSOUD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_7", context.Request["listS1stHdpD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_8", context.Request["listS1stOUD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_2", context.Request["listSOE"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSPHdpD"]))
                    {
                        hash.Add("spositiontaking_1", context.Request["listMHdpD"]);//smallmoney
                        hash.Add("isauto_spositiontaking_1", 0);//bit
                        hash.Add("auto_spositiontaking_1", 0);//smallmoney                        
                    }
                    else
                    {
                        hash.Add("spositiontaking_1", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_1", 1);//bit
                        hash.Add("auto_spositiontaking_1", context.Request["listSPHdpD"]);//smallmoney                  
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOUD"]))
                    {
                        hash.Add("spositiontaking_3", context.Request["listMOUD"]);//smallmoney
                        hash.Add("isauto_spositiontaking_3", 0);//bit
                        hash.Add("auto_spositiontaking_3", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_3", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_3", 1);//bit
                        hash.Add("auto_spositiontaking_3", context.Request["listSPOUD"]);//smallmoney
                    }

                    if (String.IsNullOrEmpty(context.Request["chkSP1stHdpD"]))
                    {
                        hash.Add("spositiontaking_7", context.Request["listM1stHdpD"]);//smallmoney
                        hash.Add("isauto_spositiontaking_7", 0);//bit
                        hash.Add("auto_spositiontaking_7", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_7", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_7", 1);//bit
                        hash.Add("auto_spositiontaking_7", context.Request["listSP1stHdpD"]);//smallmoney
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSP1stOUD"]))
                    {
                        hash.Add("spositiontaking_8", context.Request["listM1stOUD"]);//smallmoney
                        hash.Add("isauto_spositiontaking_8", 0);//bit
                        hash.Add("auto_spositiontaking_8", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_8", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_8", 1);//bit
                        hash.Add("auto_spositiontaking_8", context.Request["listSP1stOUD"]);//smallmoney
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOE"]))
                    {
                        hash.Add("spositiontaking_2", context.Request["listMOE"]);//smallmoney  
                        hash.Add("isauto_spositiontaking_2", 0);//bit
                        hash.Add("auto_spositiontaking_2", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_2", 0);//smallmoney  
                        hash.Add("isauto_spositiontaking_2", 1);//bit
                        hash.Add("auto_spositiontaking_2", context.Request["listSPOE"]);//smallmoney
                    }
                }
                else
                {
                    hash.Add("mpositiontaking_3", context.Request["listSHdpD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_7", context.Request["listSHdpD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_8", context.Request["listSHdpD"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_2", context.Request["listSHdpD"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSPHdpD"]))
                    {
                        hash.Add("spositiontaking_1", context.Request["listMHdpD"]);//smallmoney
                        hash.Add("spositiontaking_3", context.Request["listMHdpD"]);//smallmoney
                        hash.Add("spositiontaking_7", context.Request["listMHdpD"]);//smallmoney
                        hash.Add("spositiontaking_8", context.Request["listMHdpD"]);//smallmoney
                        hash.Add("spositiontaking_2", context.Request["listMHdpD"]);//smallmoney                        

                        hash.Add("isauto_spositiontaking_1", 0);//bit
                        hash.Add("isauto_spositiontaking_3", 0);//bit
                        hash.Add("isauto_spositiontaking_7", 0);//bit
                        hash.Add("isauto_spositiontaking_8", 0);//bit
                        hash.Add("isauto_spositiontaking_2", 0);//bit

                        hash.Add("auto_spositiontaking_1", 0);//smallmoney
                        hash.Add("auto_spositiontaking_3", 0);//smallmoney
                        hash.Add("auto_spositiontaking_7", 0);//smallmoney
                        hash.Add("auto_spositiontaking_8", 0);//smallmoney
                        hash.Add("auto_spositiontaking_2", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_1", 0);//smallmoney
                        hash.Add("spositiontaking_3", 0);//smallmoney
                        hash.Add("spositiontaking_7", 0);//smallmoney
                        hash.Add("spositiontaking_8", 0);//smallmoney
                        hash.Add("spositiontaking_2", 0);//smallmoney

                        hash.Add("isauto_spositiontaking_1", 1);//bit
                        hash.Add("isauto_spositiontaking_3", 1);//bit
                        hash.Add("isauto_spositiontaking_7", 1);//bit
                        hash.Add("isauto_spositiontaking_8", 1);//bit
                        hash.Add("isauto_spositiontaking_2", 1);//bit

                        hash.Add("auto_spositiontaking_1", context.Request["listSPHdpD"]);//smallmoney
                        hash.Add("auto_spositiontaking_3", context.Request["listSPHdpD"]);//smallmoney
                        hash.Add("auto_spositiontaking_7", context.Request["listSPHdpD"]);//smallmoney
                        hash.Add("auto_spositiontaking_8", context.Request["listSPHdpD"]);//smallmoney
                        hash.Add("auto_spositiontaking_2", context.Request["listSPHdpD"]);//smallmoney
                    }
                }

                hash.Add("mpositiontaking_5", context.Request["listS1x2"]);// smallmoney = 0.7,
                if (String.IsNullOrEmpty(context.Request["chkCopy1x2"]))
                {
                    hash.Add("mpositiontaking_4", context.Request["listSCS"]);// smallmoney = 0.7,                
                    hash.Add("mpositiontaking_6", context.Request["listSTG"]);// smallmoney = 0.7,                
                    hash.Add("mpositiontaking_9", context.Request["listSMP"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_10", context.Request["listSOutright"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSP1x2"]))
                    {
                        hash.Add("spositiontaking_5", context.Request["listM1x2"]);//smallmoney
                        hash.Add("isauto_spositiontaking_5", 0);//bit
                        hash.Add("auto_spositiontaking_5", 0);//smallmoney

                    }
                    else
                    {
                        hash.Add("spositiontaking_5", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_5", 1);//bit
                        hash.Add("auto_spositiontaking_5", context.Request["listSP1x2"]);//smallmoney
                    }

                    if (String.IsNullOrEmpty(context.Request["chkSPCS"]))
                    {
                        hash.Add("spositiontaking_4", context.Request["listMCS"]);//smallmoney                
                        hash.Add("isauto_spositiontaking_4", 0);//bit 
                        hash.Add("auto_spositiontaking_4", 0);//smallmoney              
                    }
                    else
                    {
                        hash.Add("spositiontaking_4", 0);//smallmoney                
                        hash.Add("isauto_spositiontaking_4", 1);//bit                
                        hash.Add("auto_spositiontaking_4", context.Request["listSPCS"]);//smallmoney

                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPTG"]))
                    {
                        hash.Add("spositiontaking_6", context.Request["listMTG"]);//smallmoney                
                        hash.Add("isauto_spositiontaking_6", 0);//bit        
                        hash.Add("auto_spositiontaking_6", 0);//smallmoney           

                    }
                    else
                    {
                        hash.Add("spositiontaking_6", 0);//smallmoney                
                        hash.Add("isauto_spositiontaking_6", 1);//bit         
                        hash.Add("auto_spositiontaking_6", context.Request["listSPTG"]);//smallmoney          

                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPMP"]))
                    {
                        hash.Add("spositiontaking_9", context.Request["listMMP"]);//smallmoney
                        hash.Add("isauto_spositiontaking_9", 0);//bit
                        hash.Add("auto_spositiontaking_9", 0);//smallmoney 
                    }
                    else
                    {
                        hash.Add("spositiontaking_9", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_9", 1);//bit
                        hash.Add("auto_spositiontaking_9", context.Request["listSPMP"]);//smallmoney 
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOutright"]))
                    {
                        hash.Add("spositiontaking_10", context.Request["listMOutright"]);//smallmoney
                        hash.Add("isauto_spositiontaking_10", 0);//bit
                        hash.Add("auto_spositiontaking_10", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("spositiontaking_10", 0);//smallmoney
                        hash.Add("isauto_spositiontaking_10", 1);//bit
                        hash.Add("auto_spositiontaking_10", context.Request["listSPOutright"]);//smallmoney
                    }
                }
                else
                {
                    hash.Add("mpositiontaking_4", context.Request["listS1x2"]);// smallmoney = 0.7,                
                    hash.Add("mpositiontaking_6", context.Request["listS1x2"]);// smallmoney = 0.7,                
                    hash.Add("mpositiontaking_9", context.Request["listS1x2"]);// smallmoney = 0.7,
                    hash.Add("mpositiontaking_10", context.Request["listS1x2"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSP1x2"]))
                    {
                        hash.Add("spositiontaking_5", context.Request["listM1x2"]);//smallmoney
                        hash.Add("spositiontaking_4", context.Request["listM1x2"]);//smallmoney                
                        hash.Add("spositiontaking_6", context.Request["listM1x2"]);//smallmoney                
                        hash.Add("spositiontaking_9", context.Request["listM1x2"]);//smallmoney
                        hash.Add("spositiontaking_10", context.Request["listM1x2"]);//smallmoney

                        hash.Add("isauto_spositiontaking_5", 0);//bit
                        hash.Add("isauto_spositiontaking_4", 0);//bit
                        hash.Add("isauto_spositiontaking_6", 0);//bit
                        hash.Add("isauto_spositiontaking_9", 0);//bit
                        hash.Add("isauto_spositiontaking_10", 0);//bit

                        hash.Add("auto_spositiontaking_5", 0);//smallmoney
                        hash.Add("auto_spositiontaking_4", 0);//smallmoney                
                        hash.Add("auto_spositiontaking_6", 0);//smallmoney                
                        hash.Add("auto_spositiontaking_9", 0);//smallmoney                
                        hash.Add("auto_spositiontaking_10", 0);//smallmoney

                    }
                    else
                    {
                        hash.Add("spositiontaking_5", 0);//smallmoney
                        hash.Add("spositiontaking_4", 0);//smallmoney                
                        hash.Add("spositiontaking_6", 0);//smallmoney                
                        hash.Add("spositiontaking_9", 0);//smallmoney
                        hash.Add("spositiontaking_10", 0);//smallmoney

                        hash.Add("isauto_spositiontaking_5", 1);//bit
                        hash.Add("isauto_spositiontaking_4", 1);//bit
                        hash.Add("isauto_spositiontaking_6", 1);//bit
                        hash.Add("isauto_spositiontaking_9", 1);//bit
                        hash.Add("isauto_spositiontaking_10", 1);//bit

                        hash.Add("auto_spositiontaking_5", context.Request["listSP1x2"]);//smallmoney
                        hash.Add("auto_spositiontaking_4", context.Request["listSP1x2"]);//smallmoney                
                        hash.Add("auto_spositiontaking_6", context.Request["listSP1x2"]);//smallmoney                
                        hash.Add("auto_spositiontaking_9", context.Request["listSP1x2"]);//smallmoney                
                        hash.Add("auto_spositiontaking_10", context.Request["listSP1x2"]);//smallmoney
                    }
                }


                hash.Add("mlivepositiontaking_1", context.Request["listSHdpL"]);// smallmoney = 0.7,
                if (String.IsNullOrEmpty(context.Request["chkCopyHdpL"]))
                {
                    hash.Add("mlivepositiontaking_3", context.Request["listSOUL"]);// smallmoney = 0.7,
                    hash.Add("mlivepositiontaking_7", context.Request["listS1stHdpL"]);// smallmoney = 0.7,
                    hash.Add("mlivepositiontaking_8", context.Request["listS1stOUL"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSPHdpL"]))
                    {
                        hash.Add("slivepositiontaking_1", context.Request["listMHdpL"]);//smallmoney
                        hash.Add("isauto_slivepositiontaking_1", 0);//bit
                        hash.Add("auto_slivepositiontaking_1", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("slivepositiontaking_1", 0);//smallmoney
                        hash.Add("isauto_slivepositiontaking_1", 1);//bit
                        hash.Add("auto_slivepositiontaking_1", context.Request["listSPHdpL"]);//smallmoney
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOUL"]))
                    {
                        hash.Add("slivepositiontaking_3", context.Request["listMOUL"]);//smallmoney
                        hash.Add("isauto_slivepositiontaking_3", 0);//bit    
                        hash.Add("auto_slivepositiontaking_3", 0);//smallmoney                
                    }
                    else
                    {
                        hash.Add("slivepositiontaking_3", 0);//smallmoney
                        hash.Add("isauto_slivepositiontaking_3", 1);//bit
                        hash.Add("auto_slivepositiontaking_3", context.Request["listSPOUL"]);//smallmoney
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSP1stHdpL"]))
                    {
                        hash.Add("slivepositiontaking_7", context.Request["listM1stHdpL"]);//smallmoney
                        hash.Add("isauto_slivepositiontaking_7", 0);//bit     
                        hash.Add("auto_slivepositiontaking_7", 0);//smallmoney               
                    }
                    else
                    {
                        hash.Add("slivepositiontaking_7", 0);//smallmoney
                        hash.Add("isauto_slivepositiontaking_7", 1);//bit
                        hash.Add("auto_slivepositiontaking_7", context.Request["listSP1stHdpL"]);//smallmoney
                    }
                    if (String.IsNullOrEmpty(context.Request["chkSP1stOUL"]))
                    {
                        hash.Add("slivepositiontaking_8", context.Request["listM1stOUL"]);//smallmoney
                        hash.Add("isauto_slivepositiontaking_8", 0);//bit
                        hash.Add("auto_slivepositiontaking_8", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("slivepositiontaking_8", 0);//smallmoney
                        hash.Add("isauto_slivepositiontaking_8", 1);//bit
                        hash.Add("auto_slivepositiontaking_8", context.Request["listSP1stOUL"]);//smallmoney
                    }
                }
                else
                {
                    hash.Add("mlivepositiontaking_3", context.Request["listSHdpL"]);// smallmoney = 0.7,
                    hash.Add("mlivepositiontaking_7", context.Request["listSHdpL"]);// smallmoney = 0.7,
                    hash.Add("mlivepositiontaking_8", context.Request["listSHdpL"]);// smallmoney = 0.7,
                    if (String.IsNullOrEmpty(context.Request["chkSPHdpL"]))
                    {
                        hash.Add("slivepositiontaking_1", context.Request["listMHdpL"]);//smallmoney
                        hash.Add("slivepositiontaking_3", context.Request["listMHdpL"]);//smallmoney
                        hash.Add("slivepositiontaking_7", context.Request["listMHdpL"]);//smallmoney
                        hash.Add("slivepositiontaking_8", context.Request["listMHdpL"]);//smallmoney

                        hash.Add("isauto_slivepositiontaking_1", 0);//bit
                        hash.Add("isauto_slivepositiontaking_3", 0);//bit
                        hash.Add("isauto_slivepositiontaking_7", 0);//bit
                        hash.Add("isauto_slivepositiontaking_8", 0);//bit

                        hash.Add("auto_slivepositiontaking_1", 0);//smallmoney
                        hash.Add("auto_slivepositiontaking_3", 0);//smallmoney
                        hash.Add("auto_slivepositiontaking_7", 0);//smallmoney
                        hash.Add("auto_slivepositiontaking_8", 0);//smallmoney

                    }
                    else
                    {
                        hash.Add("slivepositiontaking_1", 0);//smallmoney
                        hash.Add("slivepositiontaking_3", 0);//smallmoney
                        hash.Add("slivepositiontaking_7", 0);//smallmoney
                        hash.Add("slivepositiontaking_8", 0);//smallmoney

                        hash.Add("isauto_slivepositiontaking_1", 1);//bit
                        hash.Add("isauto_slivepositiontaking_3", 1);//bit
                        hash.Add("isauto_slivepositiontaking_7", 1);//bit
                        hash.Add("isauto_slivepositiontaking_8", 1);//bit

                        hash.Add("auto_slivepositiontaking_1", context.Request["listSPHdpL"]);//smallmoney
                        hash.Add("auto_slivepositiontaking_3", context.Request["listSPHdpL"]);//smallmoney
                        hash.Add("auto_slivepositiontaking_7", context.Request["listSPHdpL"]);//smallmoney
                        hash.Add("auto_slivepositiontaking_8", context.Request["listSPHdpL"]);//smallmoney

                    }
                }


                hash.Add("bmpositiontaking_1", context.Request["listSHdpB"]);// smallmoney = 0.7,
                if (String.IsNullOrEmpty(context.Request["chkCopyHdpB"]))
                {
                    hash.Add("bmpositiontaking_3", context.Request["listSOUB"]);// smallmoney = 0.7,
                    hash.Add("bmpositiontaking_7", context.Request["listSHdpB"]);// smallmoney = 0.7,
                    hash.Add("bmpositiontaking_8", context.Request["listSOUB"]);// smallmoney = 0.7, 
                    hash.Add("bmpositiontaking_2", context.Request["listSOEB"]);// smallmoney = 0.7,          

                    if (String.IsNullOrEmpty(context.Request["chkSPHdpB"]))
                    {
                        hash.Add("bspositiontaking_1", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("bspositiontaking_7", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("isauto_bspositiontaking_1", 0);//bit
                        hash.Add("isauto_bspositiontaking_7", 0);//bit
                        hash.Add("auto_bspositiontaking_1", 0);//smallmoney
                        hash.Add("auto_bspositiontaking_7", 0);//smallmoney                                            
                    }
                    else
                    {
                        hash.Add("bspositiontaking_1", 0);//smallmoney
                        hash.Add("bspositiontaking_7", 0);//smallmoney
                        hash.Add("isauto_bspositiontaking_1", 1);//bit
                        hash.Add("isauto_bspositiontaking_7", 1);//bit
                        hash.Add("auto_bspositiontaking_1", context.Request["listSPHdpB"]);//smallmoney
                        hash.Add("auto_bspositiontaking_7", context.Request["listSPHdpB"]);//smallmoney

                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOUB"]))
                    {
                        hash.Add("bspositiontaking_3", context.Request["listMOUB"]);//smallmoney                        
                        hash.Add("bspositiontaking_8", context.Request["listMOUB"]);//smallmoney
                        hash.Add("isauto_bspositiontaking_3", 0);//bit   
                        hash.Add("isauto_bspositiontaking_8", 0);//bit
                        hash.Add("auto_bspositiontaking_3", 0);//smallmoney    
                        hash.Add("auto_bspositiontaking_8", 0);//smallmoney
                    }
                    else
                    {
                        hash.Add("bspositiontaking_3", 0);//smallmoney                        
                        hash.Add("bspositiontaking_8", 0);//smallmoney
                        hash.Add("isauto_bspositiontaking_3", 1);//bit   
                        hash.Add("isauto_bspositiontaking_8", 1);//bit
                        hash.Add("auto_bspositiontaking_3", context.Request["listSPOUB"]);//smallmoney    
                        hash.Add("auto_bspositiontaking_8", context.Request["listSPOUB"]);//smallmoney

                    }
                    if (String.IsNullOrEmpty(context.Request["chkSPOEB"]))
                    {
                        hash.Add("bspositiontaking_2", context.Request["listMOEB"]);//smallmoney                
                        hash.Add("isauto_bspositiontaking_2", 0);//bit     
                        hash.Add("auto_bspositiontaking_2", 0);//smallmoney                
                    }
                    else
                    {
                        hash.Add("bspositiontaking_2", 0);//smallmoney                
                        hash.Add("isauto_bspositiontaking_2", 1);//bit           
                        hash.Add("auto_bspositiontaking_2", context.Request["listSPOEB"]);//smallmoney           
                    }
                }
                else
                {
                    hash.Add("bmpositiontaking_3", context.Request["listSHdpB"]);// smallmoney = 0.7,
                    hash.Add("bmpositiontaking_7", context.Request["listSHdpB"]);// smallmoney = 0.7,
                    hash.Add("bmpositiontaking_8", context.Request["listSHdpB"]);// smallmoney = 0.7, 
                    hash.Add("bmpositiontaking_2", context.Request["listSHdpB"]);// smallmoney = 0.7,          

                    if (String.IsNullOrEmpty(context.Request["chkSPHdpB"]))
                    {
                        hash.Add("bspositiontaking_1", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("bspositiontaking_3", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("bspositiontaking_7", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("bspositiontaking_8", context.Request["listMHdpB"]);//smallmoney
                        hash.Add("bspositiontaking_2", context.Request["listMHdpB"]);//smallmoney                

                        hash.Add("isauto_bspositiontaking_1", 0);//bit                        
                        hash.Add("isauto_bspositiontaking_3", 0);//bit
                        hash.Add("isauto_bspositiontaking_7", 0);//bit
                        hash.Add("isauto_bspositiontaking_8", 0);//bit                                
                        hash.Add("isauto_bspositiontaking_2", 0);//bit                                                        

                        hash.Add("auto_bspositiontaking_1", 0);//smallmoney
                        hash.Add("auto_bspositiontaking_3", 0);//smallmoney
                        hash.Add("auto_bspositiontaking_7", 0);//smallmoney
                        hash.Add("auto_bspositiontaking_8", 0);//smallmoney
                        hash.Add("auto_bspositiontaking_2", 0);//smallmoney                
                    }
                    else
                    {
                        hash.Add("bspositiontaking_1", 0);//smallmoney
                        hash.Add("bspositiontaking_3", 0);//smallmoney
                        hash.Add("bspositiontaking_7", 0);//smallmoney
                        hash.Add("bspositiontaking_8", 0);//smallmoney
                        hash.Add("bspositiontaking_2", 0);//smallmoney                

                        hash.Add("isauto_bspositiontaking_1", 1);//bit
                        hash.Add("isauto_bspositiontaking_3", 1);//bit
                        hash.Add("isauto_bspositiontaking_7", 1);//bit
                        hash.Add("isauto_bspositiontaking_8", 1);//bit
                        hash.Add("isauto_bspositiontaking_2", 1);//bit

                        hash.Add("auto_bspositiontaking_1", context.Request["listSPHdpB"]);//smallmoney
                        hash.Add("auto_bspositiontaking_3", context.Request["listSPHdpB"]);//smallmoney
                        hash.Add("auto_bspositiontaking_7", context.Request["listSPHdpB"]);//smallmoney
                        hash.Add("auto_bspositiontaking_8", context.Request["listSPHdpB"]);//smallmoney
                        hash.Add("auto_bspositiontaking_2", context.Request["listSPHdpB"]);//smallmoney                
                    }
                }
                hash.Add("groupa", context.Request["listGroupA"]);//decimal
                hash.Add("groupb", context.Request["listGroupB"]);//decimal
                hash.Add("groupc", context.Request["listGroupC"]);//decimal
                hash.Add("groupd", context.Request["listGroupD"]);//decimal
                hash.Add("discountcs", context.Request["listOther"]);//decimal
                hash.Add("updatecommission", 1);//bit
                hash.Add("updateminmaxbet", 1);//bit
                hash.Add("updateminmaxbetb", 1);//bit
                hash.Add("updatepositiontaking", 1);//bit
                hash.Add("updatepositiontakingb", 1);//bit
                hash.Add("updateclosed", 1);//bit              
                //************************************
                if (context.Request["isUpdateMember"] == "0")
                {
                    hash.Add("updateallmembers", 0);//bit
                }
                else
                {
                    hash.Add("updateallmembers", 1);//bit
                }

                hash.Add("newspositiontaking", Convert.ToDouble(context.Request["memberPT"], System.Globalization.CultureInfo.CurrentUICulture));//smallmoney
                //************************************                
                hash.Add("subaccid", 0);//int       
                hash.Add("ip", context.Request.UserHostAddress.ToString());                       

                IBCUtils.IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdEditCustomerSAgRpt(hash);
                if (error.IsSuccess())
                {
                    //context.Response.Write("<script type='text/javascript'>parent.location.href='../SuperAgentList.aspx?type=" + context.Request["type"] + "&custid=" + context.Request["superid"] + "';</script>");
                    context.Response.Write("<script type='text/javascript'>parent.alert_error('Update Master Successfully','errMsg');");
                    if (string.IsNullOrEmpty(context.Request["set"]))
                    {
                        context.Response.Write("parent.opener.location.reload(true);parent.close();</script>");
                    }
                    else
                    {
                        context.Response.Write("parent.obj('btnUpdate').disabled=false;</script>");
                    }
                }
                else
                {
                    context.Response.Write("<script>parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(error.ErrorMsg) + "','errMsg');");
                    context.Response.Write("parent.obj('btnUpdate').disabled=false;</script>");
                }
            }

            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("<script>alert('"+IBCUtils.IbcError.INTERNAL_ERROR_MSG+"');</script>");
                
                context.Response.Write("<script>alert('" + IBCUtils.Encryption.RemoveSpecialChars(ex.Message) + "');</script>");
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
