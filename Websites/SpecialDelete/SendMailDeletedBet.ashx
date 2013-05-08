<%@ WebHandler Language="C#" Class="SendMailDeletedBet" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using System.IO;
using DNAide.Data;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;


public class SendMailDeletedBet : IHttpHandler, IRequiresSessionState
{
    string ErrMsg = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "text/html";
        
        StringBuilder html = new StringBuilder();
        bool IsSendMail = false;
        try
        {


            #region

            string username = AppSession.AccUserName;

            if (context.Session["AccId"] == null)
            {
                ErrMsg = AlertMessage.GetMsg(IBCUtils.IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error);
            }
            DbResult rs = DbHelper.Execute("Acc_SpecialDelete_DepositBetSel", DBType.WAREHOUSE);


            int errcode = CastAide.AsInt32(rs.Outputs["errcode"]);
            string errmsg = CastAide.AsString(rs.Outputs["errmsg"]);

            if (errcode == 0)
            {
                ErrMsg = AlertMessage.GetMsg("Message sent!", AlertMessage.MsgType.Success);
                DataTable dtMatchInfo = new DataTable();
                dtMatchInfo.Columns.Add("matchid", typeof(int));
                dtMatchInfo.Columns.Add("matchcode", typeof(string));
                dtMatchInfo.Columns.Add("homeid", typeof(int));
                dtMatchInfo.Columns.Add("awayid", typeof(int));
                dtMatchInfo.Columns.Add("eventdate", typeof(string));
                dtMatchInfo.Columns.Add("datetime", typeof(string));

                DataRow drMatchInfo = dtMatchInfo.NewRow();
                drMatchInfo["matchid"] = 886919;
                drMatchInfo["matchcode"] = "0781";
                drMatchInfo["homeid"] = 66186;
                drMatchInfo["awayid"] = 68174;
                drMatchInfo["eventdate"] = "2010-10-29";
                drMatchInfo["datetime"] = "2010-10-29 03:59:00.000";
                dtMatchInfo.Rows.Add(drMatchInfo);
                
                
                if (rs.ResultSet.Tables.Count >= 1)
                {
                    DataTable dtMatch = dtMatchInfo;
                    DataTable dtUser = rs.ResultSet.Tables[0];

                    string tplMatchAll =
                        Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail.htm");
                    string tplUser =
                        Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMailAllRow.htm");

                    //For special bets - 12EU
                    string tplMatchAll_SpecialBet =
                        Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail_SpecialBet.htm");
                    string tplUser_SpecialBet =
                        Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(
                            "SpecialDelete/DeleteBetReportSendMailAllRow_SpecialBet.htm");


                    #region send mail

                    if (dtMatch.Rows.Count > 0 && dtUser.Rows.Count > 0)
                    {


                        #region send mail to CSD

                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]);// +
                                       //  (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"])
                                        //      ? Resources.Language.IsNeutral
                                          //    : string.Empty);
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

                            strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " +
                                      CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);
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
                                if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("12bet"))
                                    // for 12Bet
                                {
                                    DataRow dr12Bet = dt12Bet.NewRow();
                                    for (int i = 0; i < drAll.ItemArray.Length; i++)
                                    {
                                        dr12Bet[i] = drAll[i];
                                    }
                                    dt12Bet.Rows.Add(dr12Bet);
                                }
                                else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("mansion88"))
                                    // for mansion88 bet
                                {
                                    DataRow drMansion88Bet = dtMansion88Bet.NewRow();
                                    for (int i = 0; i < drAll.ItemArray.Length; i++)
                                    {
                                        drMansion88Bet[i] = drAll[i];
                                    }
                                    dtMansion88Bet.Rows.Add(drMansion88Bet);
                                }
                                else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("alog"))
                                    // for alog site
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
                                string Subject = "Deleted Bets Report for {site} {site_name} (" +
                                                 Convert.ToDateTime(drMatch["datetime"]) + ")";

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
                                TemplateHandler.Assign(ref tplMatch, "EventDate",
                                                       FormatHandler.FormatDateEn(drMatch["EventDate"]));
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
                                        else if (
                                            DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals(
                                                "mansion88"))
                                            IsMansionBet = true;
                                        else if (
                                            DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals(
                                                "alog"))
                                            IsAlogBet = true;
                                        TemplateHandler.Assign(ref row, "username1", drBet["username2"]);
                                    }

                                    TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                    TemplateHandler.Assign(ref row, "username", drBet["username"]);



                                    TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                                    TemplateHandler.Assign(ref row, "bettype", CacheRef.GetBettype(drBet["bettype"]));
                                        //drBet["typenamee"]);
                                    TemplateHandler.Assign(ref row, "bet", drBet["betteam"]);
                                    TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(drBet["odds"], 3));
                                    TemplateHandler.Assign(ref row, "OddsType",
                                                           CacheRef.GetOddsTypeName(drBet["oddstype"]));
                                    TemplateHandler.Assign(ref row, "amount",
                                                           NumberFormat.DecFormat(drBet["StakePositiontaking"], 2));
                                    TemplateHandler.Assign(ref row, "currency",
                                                           CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                    switch (CastAide.AsInt32(drBet["bettype"]))
                                    {
                                        case 1:
                                        case 3:
                                        case 7:
                                        case 8:
                                        case 22:
                                            Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"])
                                                               ? 0
                                                               : Convert.ToDecimal(drBet["hdp1"]);
                                            Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"])
                                                               ? 0
                                                               : Convert.ToDecimal(drBet["hdp2"]);
                                            TemplateHandler.Assign(ref row, "{hdp}",
                                                                   IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                                            break;
                                        default:
                                            TemplateHandler.Assign(ref row, "{hdp}", "--");
                                            break;
                                    }

                                    TemplateHandler.Assign(ref row, "transtime",
                                                           Convert.ToDateTime(drBet["transdate"]).ToString());
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
                                LogMail.Append("<!--Start-->Date: " + DateTime.Now.ToString() + "<!--/Start--><br>" +
                                               Environment.NewLine);
                                LogMail.Append("<!--To-->To: " + ToMailCSD + "<!--/To--><br>" + Environment.NewLine);
                                LogMail.Append("<!--Subject-->Subject: " + Subject + "<!--/Subject--><br>" +
                                               Environment.NewLine);
                                LogMail.Append("<!--Content-->" + Environment.NewLine + tplMatch + Environment.NewLine +
                                               "<!--/Content-->" + Environment.NewLine);
                                LogMail.Append("<!--End--><hr/><!--/End-->" + Environment.NewLine + Environment.NewLine);

                                bool result = false;
                                try
                                {
                                    WriteLogMail(LogMail.ToString(), context);
                                    if (MailSection.CCMail.Length > 0)
                                    {
                                        if (CC.Length > 0)
                                            CC += ";" + MailSection.CCMail;
                                        else
                                            CC = MailSection.CCMail;
                                    }
                                    if (MailSection.BCCMail.Length > 0)
                                    {
                                        if (BCC.Length > 0)
                                            BCC += ";" + MailSection.BCCMail;
                                        else
                                            BCC = MailSection.BCCMail;
                                    }
                                    result = DNAide.Utils.SendMailService.SendMail2(MailSection.MailServer, ToMailCSD,
                                                                                    MailSection.FromMail, CC, BCC,
                                                                                    Subject, tplMatch,
                                                                                    MailSection.MailPwd);
                                }
                                catch (Exception ex)
                                {
                                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                                    ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
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
                                    msg = "Details of deleted " + site + " ticket(s) are sent successfully to " +
                                          ToMailCSD;
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