using System;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using IBCUtils;

using Sunplus.Accounting.Business.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;

//using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for DeleteBet
/// </summary>
namespace Sunplus.Accounting.Business.BetList
{
    public class DeleteBet
    {
        public static string LoadContent()
        {
            string betId = CastAide.AsString(HttpContext.Current.Request["betid"]);
            string betFrom = CastAide.AsString(HttpContext.Current.Request["betfrom"]);
            string matchID = CastAide.AsString(HttpContext.Current.Request["matchid"]);
            string bettype = CastAide.AsString(HttpContext.Current.Request["bettype"]);
            string html = string.Empty;
            html = WebUtils.LoadTemplate("BetList\\DeleteBet.htm");
            TemplateHandler.Assign(ref html, "{BetId}", betId);
            TemplateHandler.Assign(ref html, "{betfrom_vl}", betFrom);
            TemplateHandler.Assign(ref html, "{MatchId}", matchID);
            TemplateHandler.Assign(ref html, "{Bettype}", bettype);
            return html;
        }

        public static string DelBet(string betId, string betfrom, int MatchId, int bettype)
        {
            string result = string.Empty;
            try
            {
                long betIdInt = 0;
                if (!long.TryParse(betId, out betIdInt))
                {
                    //TemplateHandler.Assign(ref html, "{ErrMsg}", "Invalid Bet Id");
                    //result = "Error|Invalid Bet Id";
                    throw new Exception(ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "error_InvalidRequest"));
                }
                else
                {
                    IbcError error = null;
                    if (bettype == 9)
                        error = AccountReport.BetTransDelParlay(betIdInt,
                                                                CastAide.AsInt32(HttpContext.Current.Session["AccId"]),
                                                                CastAide.AsString(
                                                                    HttpContext.Current.Request["username"]),
                                                                Encryption.CfsEnCode(
                                                                    CastAide.AsString(
                                                                        HttpContext.Current.Request["password"])),
                                                                CastAide.AsInt32(HttpContext.Current.Session["CustId"]));
                    else
                        error = AccountReport.BetTransDel(betIdInt,
                                                          CastAide.AsString(HttpContext.Current.Request["username"]),
                                                          Encryption.CfsEnCode(
                                                              CastAide.AsString(HttpContext.Current.Request["password"])),
                                                          CastAide.AsInt32(HttpContext.Current.Session["CustId"]),
                                                          CastAide.AsInt32(HttpContext.Current.Session["AccId"]));

                    if (error.IsSuccess())
                    {
                        #region send private message to users
                        // send private message to users for cancel their transactions                                               

                        Message pmsg = new Message();
                        int[] targetid = new int[1];//This is a CustID of ticket that is deleted [TargetID Field]
                        string[] targetname = new string[1];//This is a UserName of ticket that is deleted [TargetName Field]                
                        string[] transid = { betId };

                        pmsg.Content_en = CastAide.AsString(HttpContext.Current.Request["men"]);
                        pmsg.Content_ch = CastAide.AsString(HttpContext.Current.Request["mch"]);
                        pmsg.Content_cs = CastAide.AsString(HttpContext.Current.Request["mcs"]);
                        pmsg.Content_it = CastAide.AsString(HttpContext.Current.Request["mit"]);
                        pmsg.Content_jp = CastAide.AsString(HttpContext.Current.Request["mjp"]);
                        pmsg.Content_th = CastAide.AsString(HttpContext.Current.Request["mth"]);
                        pmsg.Content_es = CastAide.AsString(HttpContext.Current.Request["mes"]);

                        pmsg.CreatorId = CastAide.AsInt32(HttpContext.Current.Session["AccId"]);
                        pmsg.CreatorName = CastAide.AsString(HttpContext.Current.Request["username"]);

                        int[] ArrDownlineID = { AppSession.MemberID, AppSession.AgentID, AppSession.MasterID, AppSession.SuperID };
                        string[] ArrDownlineName = { AppSession.MemberUserName, AppSession.AgentUserName, AppSession.MasterUserName, AppSession.SusperUserName };

                        int MessageId = 0, BatchId = 0;
                        if (betfrom == "p")
                        {
                            string str = pmsg.Content_en.Trim() + pmsg.Content_ch.Trim() + pmsg.Content_cs.Trim()
                                + pmsg.Content_it.Trim() + pmsg.Content_jp.Trim() + pmsg.Content_th.Trim();
                            if (str.Length > 0)
                            {
                                BatchId = 0;// Messenger.SetBatchMessage(pmsg);
                                for (int i = 0; i < ArrDownlineID.Length; i++)
                                {
                                    targetid[0] = ArrDownlineID[i];
                                    targetname[0] = ArrDownlineName[i];

                                    if (targetid[0] < 100) continue;//SRecommend < 100 and roleid=3 

                                    if (MessageId == 0)
                                        MessageId = Messenger.SetMessage(pmsg, targetid, targetname, MatchId, transid);
                                    else
                                        Messenger.AddTargt2Message(MessageId, targetid, targetname, transid);
                                }
                            }
                        }
                        else
                        {
                            BatchId = 0;// Messenger.SetBatchMessage(pmsg);
                            for (int i = 0; i < ArrDownlineID.Length; i++)
                            {
                                targetid[0] = ArrDownlineID[i];
                                targetname[0] = ArrDownlineName[i];

                                if (targetid[0] < 100) continue; //SRecommend < 100 and roleid=3 

                                if (MessageId == 0)
                                    MessageId = Messenger.SetMessage(pmsg, targetid, targetname, MatchId, transid);
                                else
                                    Messenger.AddTargt2Message(MessageId, targetid, targetname, transid);
                            }
                        }

                        #endregion

                        result = "Success|You have deleted successfully BetID : " + betId;

                        //Response.Write("<script type='text/javascript'>alert('You have deleted successfully BetID : " + betId + " ');  opener.location.reload();close();</script>");
                    }
                    else if(error.ErrorCode == 103)
                    {
                        result = "Error|" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "error_OnlyDeleteTheUnsettledTicket");
                    }
                    else
                    {
                        //TemplateHandler.Assign(ref html, "{ErrMsg}", error.ErrorMsg);
                        result = "Error|" + error.ErrorMsg;
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
               
            }
            return result;
        }
    }
}
