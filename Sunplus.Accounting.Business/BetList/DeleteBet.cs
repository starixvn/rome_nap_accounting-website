using System;

//using Fanex.Tarus.SpecialDelete.Entity;
using System.Collections.Generic;

//using Fanex.Tarus.SpecialDelete.Business;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using Fanex.Tarus.SpecialDelete.SendEmail.Business;
using Fanex.Tarus.SpecialDelete.SendEmail.Entity;
using IBCUtils;
using Sunplus.Accounting.Business.SpecialDelete;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.Entities.SpecialDelete;



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

            if (CastAide.AsInt32(bettype) >= 1201 && CastAide.AsInt32(bettype) <= 1220)
            {
                TemplateHandler.RemoveBlock(ref html, "reason");
            }
            return html;
        }

        public static string DelBet(string betId, string betfrom, int MatchId, int bettype, string reason)
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
                    var delResult = new DeleteResult();
                    if (bettype == 9)
                    {
                        delResult = AccountReport.BetTransDelParlay(betIdInt,
                                                                CastAide.AsInt32(HttpContext.Current.Session["AccId"]),
                                                                CastAide.AsString(
                                                                    HttpContext.Current.Request["username"]),
                                                                Encryption.CfsEnCode(
                                                                    CastAide.AsString(
                                                                        HttpContext.Current.Request["password"])),
                                                                CastAide.AsInt32(HttpContext.Current.Session["CustId"]),
                                                                reason);
                    }
                    else
                    {
                        delResult = AccountReport.BetTransDel(betIdInt,
                                                          CastAide.AsString(HttpContext.Current.Request["username"]),
                                                          Encryption.CfsEnCode(
                                                              CastAide.AsString(HttpContext.Current.Request["password"])),
                                                          CastAide.AsInt32(HttpContext.Current.Session["CustId"]),
                                                          CastAide.AsInt32(HttpContext.Current.Session["AccId"]),
                                                          reason);
                    }
                    var errcode = delResult.Error.ErrorCode;
                    var errmsg = delResult.Error.ErrorMessage;

                    if (errcode == 0)
                    {
                        #region send private message to users

                        // send private message to users for cancel their transactions

                        Sunplus.Accounting.Entities.SpecialDelete.Message pmsg = new Sunplus.Accounting.Entities.SpecialDelete.Message();
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

                        #endregion send private message to users

                        result = "Success|You have deleted successfully BetID : " + betId;

                        //Response.Write("<script type='text/javascript'>alert('You have deleted successfully BetID : " + betId + " ');  opener.location.reload();close();</script>");

                        #region Send CSD, Real Currency, Test Currency

                        IEmailService _emailService = new EmailService();

                        DataTable dtMatch = delResult.PrivateMessage.Tables[0];
                        DataTable dtUsers = delResult.PrivateMessage.Tables[1];
                        DataTable dtChoice = new DataTable();

                        ResultModel resultModel = new ResultModel();
                        resultModel.ErrorHandler = new List<Result>();
                        resultModel.IsSuccess = true;

                        if (bettype == 9)
                        {
                            //2-MixParlay
                            _emailService.SendEmailWhenDeleteMixParlayBetFromBetListToCSD(errcode, errmsg, delResult.PrivateMessage, reason);
                            _emailService.SendEmailWhenDeleteMixParlayBetFromBetListToInternalGroup(errcode, errmsg, delResult.PrivateMessage, false, reason);
                            _emailService.SendEmailWhenDeleteMixParlayBetFromBetListToInternalGroup(errcode, errmsg, delResult.PrivateMessage, true, reason);
                        }
                        else if (bettype >= 80 && bettype <= 120)
                        {
                            ////4- Number Game
                            _emailService.SendEmailWhenDeleteBetToCSD(resultModel, dtMatch, dtUsers, dtChoice, true, false, reason);
                            _emailService.SendEmailWhenDeleteBetToInternalGroup(resultModel, dtMatch, dtUsers, dtChoice, true, false, false, reason);
                            _emailService.SendEmailWhenDeleteBetToInternalGroup(resultModel, dtMatch, dtUsers, dtChoice, true, false, true, reason);
                        }

                        else if (bettype == 1101 || bettype == 1102)
                        {
                            //6-LiveCasino
                            DataRow[] dr = delResult.PrivateMessage.Tables[2].Select("isDeposit = 1");
                            if (dr.Length > 0)
                            {
                                dtChoice = dr.CopyToDataTable();
                            }
                            _emailService.SendEmailWhenDeleteBetToCSD(resultModel, dtMatch, dtUsers, dtChoice, false, true, reason);

                            dtChoice = new DataTable();
                            DataRow[] dr2 = delResult.PrivateMessage.Tables[2].Select("isRealCust = 1");
                            if (dr2.Length > 0)
                            {
                                dtChoice = dr2.CopyToDataTable();
                            }
                            _emailService.SendEmailWhenDeleteBetToInternalGroup(resultModel, dtMatch, dtUsers, dtChoice, false, true, true, reason); // real currency
                            _emailService.SendEmailWhenDeleteBetToInternalGroup(resultModel, dtMatch, dtUsers, dtChoice, false, true, false, reason);//  for test currency
                        }
                        else if (bettype >= 1201 && bettype <= 1220)
                        {
                            //7-Virtual Sport
                            _emailService.SendEmailWhenDeleteVirtualSportToCSD(errcode, errmsg, delResult.PrivateMessage);
                            _emailService.SendEmailWhenDeleteVirtualSportToInternalGroup(errcode, errmsg, delResult.PrivateMessage, true); // for real currency
                            _emailService.SendEmailWhenDeleteVirtualSportToInternalGroup(errcode, errmsg, delResult.PrivateMessage, false); // for test currency
                        }
                        else
                        {
                            //1- DeleteSpecialTransaction
                            //3-Outright
                            _emailService.SendEmailWhenDeleteBetFromBetListToCSD(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, reason);
                            _emailService.SendEmailWhenDeleteBetFromBetListToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, false, false, reason);
                            _emailService.SendEmailWhenDeleteBetFromBetListToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, false, true, reason);
                        }

                        ////5- Early Kick-off
                        //_emailService.SendEmailToCSDEarlyKO(delResult.PrivateMessage, string.Empty, reason);
                        //_emailService.SendEmailWhenDeleteBetToInternalGroupEarlyKO(delResult.PrivateMessage, string.Empty, true, reason); // real currency
                        //_emailService.SendEmailWhenDeleteBetToInternalGroupEarlyKO(delResult.PrivateMessage, string.Empty, false, reason); // test currency

                        #endregion Send CSD, Real Currency, Test Currency

                        #region analysis code temp

                        //1- DeleteSpecialTransaction
                        //3-Outright
                        //result.ErrorMessage += SendEmailToCSD(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, message.Reason);

                        //SendEmailWhenDeleteBetToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, false, true, message.Reason); // for real currency
                        //SendEmailWhenDeleteBetToInternalGroup(delResult.Error.ErrorCode, delResult.Error.ErrorMessage, delResult.PrivateMessage, false, false, false, message.Reason); // for test currency

                        //2-MixParlay
                        //result.ErrorMessage += mixparlay.SendEmailToCSD(errcode, errmsg, delResult.PrivateMessage, message.Reason);

                        //mixparlay.SendEmailWhenDeleteBetToInternalGroup(errcode, errmsg, delResult.PrivateMessage, true, message.Reason); // real currency
                        //mixparlay.SendEmailWhenDeleteBetToInternalGroup(errcode, errmsg, delResult.PrivateMessage, false, message.Reason); // test currency

                        //4-NumberGame
                        //_emailService.SendEmailWhenDeleteBetToCSD(result, dtMatch, dtUsers, dtChoice, true, false, message.Reason);

                        ////Send email with real and test currency
                        //dtUsers = dbResult.ResultSet.Tables[3];
                        //_emailService.SendEmailWhenDeleteBetToInternalGroup(result, dtMatch, dtUsers, dtChoice, true, false, true, message.Reason);
                        //_emailService.SendEmailWhenDeleteBetToInternalGroup(result, dtMatch, dtUsers, dtChoice, true, false, false, message.Reason);

                        //5- Early Kick-off
                        //result.ErrorMessage += SendEmailToCSDEarlyKO(delResult.PrivateMessage, string.Empty, message.Reason);

                        //SendEmailWhenDeleteBetToInternalGroupEarlyKO(delResult.PrivateMessage, string.Empty, true, message.Reason); // real currency
                        //SendEmailWhenDeleteBetToInternalGroupEarlyKO(delResult.PrivateMessage, string.Empty, false, message.Reason); // test currency

                        //6-LiveCasino
                        //_emailService.SendEmailWhenDeleteBetToCSD(result, dtMatch, dtUsers, dtChoice, false, true, message.Reason);

                        ////Send email cho real với test currency
                        //dtUsers = dbResult.ResultSet.Tables[4];
                        //dtChoice = dbResult.ResultSet.Tables[5];
                        //_emailService.SendEmailWhenDeleteBetToInternalGroup(result, dtMatch, dtUsers, dtChoice, false, true, true, message.Reason);
                        //_emailService.SendEmailWhenDeleteBetToInternalGroup(result, dtMatch, dtUsers, dtChoice, false, true, false, message.Reason);

                        //7-Virtual Sport
                        //result.ErrorMessage += SendEmailToCSD(errcode, errmsg, dbResult, VirtualSportsName);

                        //result.ErrorMessage += SendEmailWhenDeleteBetToInternalGroup(errcode, errmsg, dbResult, VirtualSportsName, true); // for real currency
                        //SendEmailWhenDeleteBetToInternalGroup(errcode, errmsg, dbResult, VirtualSportsName, false); // for test currency

                        #endregion analysis code temp
                    }
                    else if (errcode == 103)
                    {
                        result = "Error|" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "error_OnlyDeleteTheUnsettledTicket");
                    }
                    else
                    {
                        //TemplateHandler.Assign(ref html, "{ErrMsg}", error.ErrorMsg);
                        result = "Error|" + errmsg;
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