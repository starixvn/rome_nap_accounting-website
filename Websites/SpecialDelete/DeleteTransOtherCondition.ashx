<%@ WebHandler Language="C#" Class="DeleteTrans" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using System.IO;
using DNAide.Data;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;


public class DeleteTrans : IHttpHandler,IRequiresSessionState {
    string ErrMsg = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "text/html";
        
        StringBuilder html = new StringBuilder();
        bool IsSendMail = false;
        try
        {
            string matchId = context.Request["matchid"];

            if (context.Request["IsOutRight"] == "1")
                matchId = context.Request["ddlOutright"];
            
            {
                #region
                string username = AppSession.AccUserName;
                
                if (context.Session["AccId"] == null)
                {
                    ErrMsg = AlertMessage.GetMsg(IBCUtils.IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error);
                }
                DbResult rs = DbHelper.Execute("DBA_DelBet_Custom", Sunplus.Accounting.DataAccess.DBType.MAIN);


                #region send private message to users
                if (rs.ResultSet.Tables.Count > 0)
                {
                    DataTable dtMsg = rs.ResultSet.Tables[rs.ResultSet.Tables.Count - 1];
                    if (dtMsg.Rows.Count > 0)
                    {
                        Hashtable list = new Hashtable();

                        for (int i = 0; i < dtMsg.Rows.Count; i++)
                        {

                            string[] ArrColumnsNameDownLineID = { "custid", "recommend", "mrecommend", "srecommend" };
                            string[] ArrColumnsNameDownLineUserName = { "CustUserName", "AgUserName", "MagUserName", "SagUserName" };

                            for (int j = 0; j < ArrColumnsNameDownLineID.Length; j++)
                            {
                                string cnameID = ArrColumnsNameDownLineID[j];
                                string cnameUser = ArrColumnsNameDownLineUserName[j];

                                string custid = dtMsg.Rows[i][cnameID].ToString();
                                string uname = dtMsg.Rows[i][cnameUser].ToString();

                                string key = custid + "^" + uname;
                                string ticknum = dtMsg.Rows[i]["transid"].ToString();

                                if (list[key] != null)
                                {
                                    if (list[key].ToString().IndexOf(ticknum) == -1)
                                    {
                                        string _tmp = list[key].ToString();
                                        list.Remove(key);
                                        list.Add(key, _tmp + "," + ticknum);
                                    }
                                }
                                else
                                    list.Add(key, ticknum);
                            }
                        }

                        DataTable dtDeletedBet = new DataTable();
                        dtDeletedBet.Columns.Add("custid", typeof(int));
                        dtDeletedBet.Columns.Add("username", typeof(string));
                        dtDeletedBet.Columns.Add("transids", typeof(string));
                        dtDeletedBet.Columns.Add("betcount", typeof(int));

                        foreach (DictionaryEntry Key in list)
                        {
                            int custid = CastAide.AsInt32(Key.Key.ToString().Split('^')[0]); // This is a CustID of ticket that is deleted [TargetID Field]
                            string uname = Key.Key.ToString().Split('^')[1];//This is a CustId of ticket that is deleted [TargetName Field]

                            DataRow dr = dtDeletedBet.NewRow();
                            dr["custid"] = custid;
                            dr["username"] = uname;
                            string[] ArrTransids = Key.Value.ToString().Split(',');
                            Array.Sort(ArrTransids);
                            string strTransIDs = "";
                            for (int i = 0; i < ArrTransids.Length; i++)
                            {
                                strTransIDs += ArrTransids[i] + ", ";
                            }
                            strTransIDs = strTransIDs.Substring(0, strTransIDs.Length - 2);
                            dr["transids"] = strTransIDs;
                            dr["betcount"] = Key.Value.ToString().Split(',').Length;
                            dtDeletedBet.Rows.Add(dr);
                        }
                        DataView dv = dtDeletedBet.DefaultView;
                        dv.Sort = "betcount asc, transids asc";
                        DataTable dtBets = dv.ToTable();

                        //Set batch Message
                        Message pmsg = new Message();
                        pmsg.Content_en = context.Request["men"];
                        pmsg.Content_ch = context.Request["mch"];
                        pmsg.Content_cs = context.Request["mcs"];
                        pmsg.Content_it = context.Request["mit"];
                        pmsg.Content_jp = context.Request["mjp"];
                        pmsg.Content_th = context.Request["mth"];
                        pmsg.Content_es = context.Request["mes"];

                        pmsg.CreatorId = Convert.ToInt32(context.Session["AccId"]);
                        pmsg.CreatorName = username;
                        string strTransID_Current = "", strTransID_Previous = "";
                        if (dtBets.Rows.Count > 0)
                            strTransID_Current = CastAide.AsString(dtBets.Rows[0]["transids"]);
                        int CountBetsBelong1Member = CastAide.AsInt32(dtBets.Compute("count(transids)", "transids='" + strTransID_Current + "'"));
                        bool IsAllBetsBelong1Member = dtBets.Rows.Count == CountBetsBelong1Member;
                        int BatchId = 0;
                        //if(!IsAllBetsBelong1Member) BatchId = Messenger.SetBatchMessage(pmsg);

                        // send private message to users for cancel their transactions                                                                                                                
                        int MessageId = 0;
                        foreach (DataRow dr in dtBets.Rows)
                        {
                            int[] custid = { CastAide.AsInt32(dr["custid"]) }; // This is a CustID of ticket that is deleted [TargetID Field]
                            string[] uname = { CastAide.AsString(dr["username"]) };//This is a CustId of ticket that is deleted [TargetName Field]

                            if (CastAide.AsInt32(dr["custid"]) < 100) continue;//SRecommend < 100 and roleid=3 
                            string [] bettransid = {CastAide.AsString(dr["transids"])};
                            
                            if ( MessageId == 0)
                                MessageId = Messenger.SetMessage(pmsg, custid, uname, CastAide.AsInt32(matchId), bettransid);                                
                            else
                                Messenger.AddTargt2Message(MessageId, custid, uname, bettransid);                            
                        }
                    }
                }
                #endregion


                int errcode = CastAide.AsInt32(rs.Outputs["errcode"]);
                string errmsg = CastAide.AsString(rs.Outputs["errmsg"]);

                if (errcode == 0)
                {
                    ErrMsg = AlertMessage.GetMsg("Bet transactions deleted!", AlertMessage.MsgType.Success);
                    if (rs.ResultSet.Tables.Count >= 2)
                    {
                        DataTable dtMatch = rs.ResultSet.Tables[0];
                        DataTable dtUser = rs.ResultSet.Tables[1];

                        string tplMatchAll = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail.htm");
                        string tplUser = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMailAllRow.htm");

                        //For special bets - 12EU
                        string tplMatchAll_SpecialBet = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail_SpecialBet.htm");
                        string tplUser_SpecialBet = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMailAllRow_SpecialBet.htm");


                        #region send mail
                        if (dtMatch.Rows.Count > 0 && dtUser.Rows.Count > 0)
                        {


                            #region send mail to CSD
                            string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty);
                            //if (dtUser.Rows[0]["typenamee"].ToString().Equals("Outright"))
                            if (CacheRef.GetTeamName(dtUser.Rows[0]["bettype"]).Equals("Outright"))
                            {
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                                TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser, "score");
                                TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                                //For 12EU
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                                TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                                TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

                                strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " + CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);
                            }

                            #region Create Dataset dsUser
                            DataSet dsUser = new DataSet();

                            DataTable dt12Bet = dtUser.Clone();
                            dt12Bet.TableName = "12Bet";

                            DataTable dtAsiaBet = dtUser.Clone();
                            dtAsiaBet.TableName = "AsiaBet";

                            DataTable dt12EUBet = dtUser.Clone();
                            dt12EUBet.TableName = "12EU";

                            DataTable dtMansion88Bet = dtUser.Clone();
                            dtMansion88Bet.TableName = "Mansion88";

                            DataTable dtAlogBet = dtUser.Clone();
                            dtAlogBet.TableName = "Alog";

                            foreach (DataRow drAll in dtUser.Rows)
                            {
                                //Add 12EU to data table
                                if (DNAide.Core.CastAide.AsString(drAll["username"], "").ToLower().Equals("12eu"))
                                {
                                    DataRow dr12EU = dt12EUBet.NewRow();
                                    for (int i = 0; i < drAll.ItemArray.Length; i++)
                                    {
                                        dr12EU[i] = drAll[i];
                                    }
                                    dt12EUBet.Rows.Add(dr12EU);
                                }
                                else
                                {
                                    if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("12bet")) // for 12Bet
                                    {
                                        DataRow dr12Bet = dt12Bet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            dr12Bet[i] = drAll[i];
                                        }
                                        dt12Bet.Rows.Add(dr12Bet);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("mansion88")) // for mansion88 bet
                                    {
                                        DataRow drMansion88Bet = dtMansion88Bet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drMansion88Bet[i] = drAll[i];
                                        }
                                        dtMansion88Bet.Rows.Add(drMansion88Bet);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("alog")) // for alog site
                                    {
                                        DataRow drAlogBet = dtAlogBet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drAlogBet[i] = drAll[i];
                                        }
                                        dtAlogBet.Rows.Add(drAlogBet);
                                    }
                                    else // for AsiaBet
                                    {
                                        DataRow drAsiaBet = dtAsiaBet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drAsiaBet[i] = drAll[i];
                                        }
                                        dtAsiaBet.Rows.Add(drAsiaBet);
                                    }
                                }


                            }
                            dsUser.Tables.Add(dt12Bet);
                            dsUser.Tables.Add(dtAsiaBet);
                            dsUser.Tables.Add(dt12EUBet);
                            dsUser.Tables.Add(dtMansion88Bet);
                            dsUser.Tables.Add(dtAlogBet);
                            #endregion

                            foreach (DataTable dtBet in dsUser.Tables)
                            {
                                if (dtBet != null && dtBet.Rows.Count > 0)
                                {
                                    DataRow drMatch = dtMatch.Rows[0];
                                    string bgColor = "#F2F2F2";
                                    StringBuilder body = new StringBuilder();
                                    bool Is12Bet = false;
                                    bool Is12EU = false;
                                    bool IsMansionBet = false;
                                    bool IsAlogBet = false;
                                    string site = string.Empty;
                                    string siteName = string.Empty;
                                    string Subject = "Deleted Bets Report for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";

                                    //Match
                                    string tplMatch = string.Empty;
                                    if (dtBet.TableName == "12EU")
                                    {
                                        tplMatch = tplMatchAll_SpecialBet.Clone().ToString();
                                        siteName = "";
                                    }
                                    else if (dtBet.TableName == "Mansion88")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Mansion88 Site";
                                    }
                                    else if (dtBet.TableName == "Alog")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Alog Site";
                                    }
                                    else
                                    {
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Deposit Site";
                                    }


                                    TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);
                                    TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                                    TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                                    TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                                    TemplateHandler.Assign(ref tplMatch, "CustId", context.Session["Username"]);
                                    TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));


                                    foreach (DataRow drBet in dtBet.Rows)
                                    {
                                        site = drBet["site"].ToString();
                                        bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                        string row = string.Empty;

                                        //12EU does not have deposit username
                                        if (DNAide.Core.CastAide.AsString(drBet["username"], "").ToLower().Equals("12eu"))
                                        {
                                            Is12EU = true;
                                            row = tplUser_SpecialBet.Clone().ToString();
                                        }
                                        else
                                        {
                                            row = tplUser.Clone().ToString();
                                            if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("12bet"))
                                                Is12Bet = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("mansion88"))
                                                IsMansionBet = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("alog"))
                                                IsAlogBet = true;
                                            TemplateHandler.Assign(ref row, "username1", drBet["username2"]);
                                        }

                                        TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                        TemplateHandler.Assign(ref row, "username", drBet["username"]);



                                        TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                                        TemplateHandler.Assign(ref row, "bettype", CacheRef.GetBettype(drBet["bettype"]));//drBet["typenamee"]);
                                        TemplateHandler.Assign(ref row, "bet", drBet["betteam"]);
                                        TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(drBet["odds"], 3));
                                        TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(drBet["oddstype"]));
                                        TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["StakePositiontaking"], 2));
                                        TemplateHandler.Assign(ref row, "currency", CacheUtils.GetExchangeName( CastAide.AsInt32( drBet["currency"])));

                                        switch (CastAide.AsInt32(drBet["bettype"]))
                                        {
                                            case 1:
                                            case 3:
                                            case 7:
                                            case 8:
                                            case 22:
                                                Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                                Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                                                break;
                                            default:
                                                TemplateHandler.Assign(ref row, "{hdp}", "--");
                                                break;
                                        }
                                        
                                        TemplateHandler.Assign(ref row, "transtime", Convert.ToDateTime(drBet["transdate"]).ToString());
                                        TemplateHandler.Assign(ref row, "score1", drBet["livehomescore"]);
                                        TemplateHandler.Assign(ref row, "score2", drBet["liveawayscore"]);

                                        string BetForm = "";
                                        if (drBet["betfrom"].ToString().ToLower().Equals("m"))
                                            BetForm = "Wap";
                                        else
                                            BetForm = "Internet";

                                        TemplateHandler.Assign(ref row, "BetFrom", BetForm);
                                        body.Append(row);
                                    } 
                                    TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());
                                    string ToMailCSD = "";
                                    string CC = "", BCC = "";
                                    if (Is12EU)
                                    {
                                        ToMailCSD = MailSection.ToMail12EUCSD;
                                        CC = MailSection.CCMail12EUCSD;
                                        BCC = MailSection.BCCMail12EUCSD;
                                    }
                                    else
                                    {
                                        if (Is12Bet)
                                        {
                                            ToMailCSD = MailSection.ToMail12BetCSD;
                                            CC = MailSection.CCMail12BetCSD;
                                            BCC = MailSection.BCCMail12BetCSD;
                                        }
                                        else if (IsMansionBet)
                                        {
                                            ToMailCSD = MailSection.ToMailMansion88CSD;
                                            CC = MailSection.CCMailMansion88CSD;
                                            BCC = MailSection.BCCMailMansion88CSD;
                                            site = "";
                                        }
                                        else if (IsAlogBet)
                                        {
                                            ToMailCSD = MailSection.ToMailAlogCSD;
                                            CC = MailSection.CCMailAlogCSD;
                                            BCC = MailSection.BCCMailAlogCSD;
                                            site = "";
                                        }
                                        else
                                        {
                                            ToMailCSD = MailSection.ToMailAsiaBetCSD;
                                            CC = MailSection.CCMailAsiaBetCSD;
                                            BCC = MailSection.BCCMailAsiaBetCSD;
                                        }
                                    }

                                   
                                    Subject = Subject.Replace("{site}", site);
                                    Subject = Subject.Replace("{site_name}", siteName);

                                    //Send mail job
                                    StringBuilder LogMail = new StringBuilder();
                                    LogMail.Append("<!--Start-->Date: " + DateTime.Now.ToString() + "<!--/Start--><br>" + Environment.NewLine);
                                    LogMail.Append("<!--To-->To: " + ToMailCSD + "<!--/To--><br>" + Environment.NewLine);
                                    LogMail.Append("<!--Subject-->Subject: " + Subject + "<!--/Subject--><br>" + Environment.NewLine);
                                    LogMail.Append("<!--Content-->" + Environment.NewLine + tplMatch + Environment.NewLine + "<!--/Content-->" + Environment.NewLine);
                                    LogMail.Append("<!--End--><hr/><!--/End-->" + Environment.NewLine + Environment.NewLine);

                                    bool result = false;
                                    try
                                    {
                                        WriteLogMail(LogMail.ToString(), context);
                                        if (MailSection.CCMail.Length > 0)
                                        {
                                            if(CC.Length>0)
                                                CC += ";" + MailSection.CCMail;
                                            else
                                                CC = MailSection.CCMail;
                                        }
                                        if (MailSection.BCCMail.Length > 0)
                                        {
                                            if(BCC.Length>0)
                                                BCC += ";" + MailSection.BCCMail;
                                            else
                                                BCC = MailSection.BCCMail;
                                        }
                                        result = DNAide.Utils.SendMailService.SendMail2(MailSection.MailServer, ToMailCSD, MailSection.FromMail, CC, BCC, Subject, tplMatch, MailSection.MailPwd);
                                    }
                                    catch (Exception ex)
                                    {
                                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                                        ErrMsg += AlertMessage.GetMsg(ex.Message, Sunplus.Accounting.Core.Helper.AlertMessage.MsgType.Error);
                                    }

                                    string msg = "";
                                    site = "";

                                    if (Is12EU) site = "12EU";
                                    else if (Is12Bet) site = "12Bet";
                                    else if (IsMansionBet) site = "Mansion88";
                                    else if (IsAlogBet) site = "ALog";
                                    else site = "AsiaBet88";
                                    
                                    if (result)
                                    {
                                        IsSendMail = true;
                                        msg = "Details of deleted " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                        ErrMsg += AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success);
                                    }
                                    else
                                    {
                                        msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                                        ErrMsg += AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    ErrMsg = AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error);                    
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);        
        }
        context.Response.Write(ErrMsg);
    }

    void WriteLogMail(string content, HttpContext context)
    {
        try
        {
            string path = context.Server.MapPath("Logs");// +"/LogMailSpecialDelete.html";
            int index = path.IndexOf("SpecialDelete");
            string root = path.Substring(0, index - 1) + "\\Logs\\LogMailSpecialDelete.html";

            // Specify file, instructions, and privelegdes
            FileStream file = new FileStream(root, FileMode.Append, FileAccess.Write);

            // Create a new stream to write to the file
            StreamWriter sw = new StreamWriter(file);

            // Write a string to the file
            sw.WriteLine(content);

            // Close StreamWriter
            sw.Close();

            // Close file
            file.Close();
        }
        catch
        {
        
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}