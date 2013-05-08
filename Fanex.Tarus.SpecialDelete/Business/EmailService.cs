// -----------------------------------------------------------------------
// <copyright file="EmailService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using DNAide.Core;
    using DNAide.Utils;
    using DNAide.Web;
    using Fanex.Data.DNAide;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.SpecialDelete.Business.NumberGame;
    using Fanex.Tarus.SpecialDelete.Entity.LiveCasino;
    using Sunplus.Accounting.Core.Configurations;
using Fanex.Tarus.SpecialDelete.Entity;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmailService : BaseService, IEmailService
    {
        INumberWheelService _INumberWheelService;

        public EmailService()
        {
            _INumberWheelService = new NumberWheelService();
        }


        public bool SendMailWhenDeleteBetToDBA(string ErrMsg, string ErrCode, string MatchID, string FromRefNo, string ToRefNo)
        {
            try
            {
                if (string.IsNullOrEmpty(ErrMsg)) ErrMsg = string.Empty;
                if (string.IsNullOrEmpty(ErrCode)) ErrMsg = string.Empty;

                SpecialDeleteElement mailling = SpecialDeleteSection.Instance.NotificationDBA["EmailNotification"];
                string ToMailCSD = mailling.To;
                string CC = mailling.CC;
                string BCC = mailling.BCC;
                string Subject = WebUtils.LoadTemplate("SpecialDelete/LiveCasino/DeleteBetsSubjectSendMailToDBA.htm");
                string Body = WebUtils.LoadTemplate("SpecialDelete/LiveCasino/DeleteBetsBodySendMailToDBA.htm");
                Body = Body.Replace("{ErrCode}", ErrCode).Replace("{ErrMsg}", ErrMsg);
                Body = Body.Replace("{MatchID}", MatchID);
                Body = Body.Replace("{FromRefno}", FromRefNo);
                Body = Body.Replace("{ToRefno}", ToRefNo);
                var isSuccess = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, Subject, Subject, Body, null);
                return isSuccess;
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
                return false;
            }
        }

        public void SendEmailWhenDeleteBetToInternalGroup(ResultModel result, DataTable dtMatch, DataTable dtUsers, DataTable dtChoice, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency)
        {
            try
            {
               
                if (dtMatch != null && dtMatch.Rows.Count> 0 && dtUsers != null && dtUsers.Rows.Count>0)
                {                    
                    var rows = dtUsers.AsEnumerable().Select(s => s).ToList();

                    string expression = "";

                    if (IsRealCurrency)
                    {
                        expression = "currency  <> 20";
                    }
                    else
                    {
                        expression = "currency = 20";
                    }

                    DataRow[] drTicketsDeleted = dtUsers.Select(expression, "");
                    rows = drTicketsDeleted.Select(s => s).ToList();
                    int cnt = drTicketsDeleted.Count();

                    #region Template
                    string tplpath = "";
                    if (IsNumberGame)
                        tplpath = "NumberGame/";
                    if (IsLiveCasino)
                        tplpath = "LiveCasino/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");

                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                    string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");
                   
                    #endregion

                    #region send mail

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        #region Template
                        string strHome = "";
                            
                        #endregion

                        #region Create Dataset dsUser

                        string ToMailCSD = "";
                        string CC = "", BCC = "";


                        SpecialDeleteElement mailling = new SpecialDeleteElement();

                        if (IsRealCurrency)
                        {
                            mailling = SpecialDeleteSection.Instance.NotificationRealCurrency["EmailNotification"];
                        }
                        else
                        {
                            mailling = SpecialDeleteSection.Instance.NotificationTestCurrency["EmailNotification"];
                        }


                        #region MatchInfo
                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;
                        string productType = "";
                        if (IsLiveCasino)
                            productType = "Live Casino";
                        else if (IsNumberGame)
                            productType = "Number Game";
                        else productType = "";

                        if (IsRealCurrency)
                        {
                            Subject = "Real Currency - Deleted " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                        }
                        else
                        {
                            Subject = "Test Currency - Deleted " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                        }


                        string tplMatch = tplMatchAll.Clone().ToString();

                        TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);

                        if (IsLiveCasino)
                        {
                            strHome = CastAide.AsString(dtMatch.Rows[0]["GameEvent"]);
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", string.Empty);
                            TemplateHandler.RemoveBlock(ref tplMatch, "vs");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                        }
                        TemplateHandler.Assign(ref tplMatch, "EventDate", CastAide.AsDateTime(drMatch["EventDate"]).ToShortDateString());
                        TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                        TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["DATETIME"]));

                        string site = string.Empty;
                        string bgColor = string.Empty;
                        string isMP = string.Empty;
                        #endregion

                        #region AssignRow
                        StringBuilder body = new StringBuilder();



                        var sites = drTicketsDeleted.AsEnumerable().Select(s => CastAide.AsString(s["site"])).Distinct().ToList();
                        int i = 0;
                        foreach (string s in sites)
                        {
                            i++;
                            if (i == sites.Count)
                                site += s;
                            else
                                site += s + ", ";
                        }

                        string betteam = string.Empty;

                        int index = 0;
                        foreach (DataRow drBet in drTicketsDeleted)
                        {
                            index++;
                            bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                            string row = tplUser.Clone().ToString();

                            TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                            TemplateHandler.Assign(ref row, "username", drBet["username"]);
                            TemplateHandler.Assign(ref row, "refno", drBet["refno"]);

                            bool IsLive = false;
                            var betType = CacheRef.GetBettype(drBet["bettype"]);
                            if (IsNumberGame)
                            {
                                IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                if (CastAide.AsInt32(drBet["bettype"]) == 90)
                                {
                                    betType = IsLive ? betType + " (Live Game)" : betType + " (Pre Game)";
                                }                                
                            }

                            TemplateHandler.Assign(ref row, "bettype", betType);

                            if (IsLiveCasino)
                            {
                                var refNo = CastAide.AsDecimal(drBet["refno"]);
                                var choice = string.Empty;
                                if (dtChoice != null && dtChoice.Rows.Count>0)                                
                                {
                                    var _transid = CastAide.AsDecimal(drBet["refno"]);
                                    choice = BuildChoiceColumnForLiveCasino(_transid, dtChoice);
                                }
                                TemplateHandler.Assign(ref row, "bet", choice);                                
                            }
                            else if (IsNumberGame && CastAide.AsInt32(drBet["bettype"]) == 90)
                            {

                                int hdp1 = CastAide.AsInt32(drBet["hdp1"]);
                                int beteam = CastAide.AsInt32(drBet["betteam"]);
                                string _choice = _INumberWheelService.GetChoiceName(hdp1, beteam);
                                TemplateHandler.Assign(ref row, "bet", _choice);
                            }
                            else
                            {
                                if (IsNumberGame)
                                {
                                    int bettype = CastAide.AsInt32(drBet["bettype"]);
                                    string _betteam = CastAide.AsString(drBet["betteam"]);
                                    TemplateHandler.Assign(ref row, "bet", ConvertBeteamOfNumberGame(bettype, _betteam)); 
                                }
                                else if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                }
                            }

                            TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                            TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                            TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                            TemplateHandler.Assign(ref row, "currency", SelCurrencyName(CastAide.AsString(drBet["currency"])));

                            TemplateHandler.Assign(ref row, "{hdp}", "--");

                            TemplateHandler.Assign(ref row, "transtime", Convert.ToDateTime(drBet["transdate"]).ToString());

                            string status = CastAide.AsString(drBet["status"]).ToLower();

                            TemplateHandler.Assign(ref row, "status", status.ToUpper());
                            TemplateHandler.Assign(ref row, "creator", drBet["creator"]);

                            if (IsLiveCasino)
                            {                               
                                
                                TemplateHandler.Assign(ref row, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"],2));
                                TemplateHandler.Assign(ref row, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"],2));
                                TemplateHandler.Assign(ref row, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"],2));
                            }

                            

                            TemplateHandler.Assign(ref row, "winloss", status == "running" ?  "-" : NumberFormat.DecFormat(drBet["winlost"], 2));
                            TemplateHandler.Assign(ref row, "comm", status == "running" ?  "-" : NumberFormat.DecFormat(drBet["comm"], 2));
                            TemplateHandler.Assign(ref row, "index", NumberFormat.DecFormat(index,0));

                            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", ListNegativebalanceCustomer(rows));
                            body.Append(row);
                        }
                        #endregion

                        TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                        ToMailCSD = mailling.To;
                        CC = mailling.CC;
                        BCC = mailling.BCC;

                        Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);

                        Subject = Subject.Replace("{site}", site);

                        //Send mail job
                        StringBuilder LogMail = new StringBuilder();
                        LogMail.Append("<!--Start-->Date: " + DateTime.Now.ToString() + "<!--/Start--><br>" + Environment.NewLine);
                        LogMail.Append("<!--To-->To: " + ToMailCSD + "<!--/To--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Subject-->Subject: " + Subject + "<!--/Subject--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Content-->" + Environment.NewLine + tplMatch + Environment.NewLine + "<!--/Content-->" + Environment.NewLine);
                        LogMail.Append("<!--End--><hr/><!--/End-->" + Environment.NewLine + Environment.NewLine);

                        // send mail via gmail system
                        bool f = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                        #endregion
                    }
                    #endregion


                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorHandler.Add(new Result() { 
                    Key = "100",
                    Value = ex.Message
                });
            }
        }

        StringBuilder AssignData(DataTable dtUsers, DataTable dtChoice, string licensee, string columnName, string tplUser, ref string tplMatch, ref string site, bool IsNumberGame, bool IsLiveCasino)
        {
            #region AssignRow
            StringBuilder body = new StringBuilder();

            string alias = licensee;
            if (alias.Equals("deposit"))
            {
                alias = "asiabet88";
            }
            var rows = dtUsers.AsEnumerable().Where(s => CastAide.AsString(s[columnName]).ToLower() == alias.ToLower()).Select(s => s).ToList();
            var sites = rows.AsEnumerable().Select(s => CastAide.AsString(s["site"])).Distinct().ToList();
            

            int i = 0;
            foreach (string s in sites)
            {
                i++;
                if (i == sites.Count)
                    site += s;
                else
                    site += s + ", ";
            }
         
            string betteam = string.Empty;

            int index = 0;
            string bgColor = "";
            foreach (DataRow drBet in rows)
            {
                index++;
                bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                string row = tplUser.Clone().ToString();

                TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                TemplateHandler.Assign(ref row, "username", drBet["username"]);
                TemplateHandler.Assign(ref row, "creator", drBet["creator"]);
                TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                bool IsLive = false;
                var betType = CacheRef.GetBettype(drBet["bettype"]);
                if (IsNumberGame)
                {
                    IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                    if (CastAide.AsInt32(drBet["bettype"]) == 90)
                    {
                        betType = IsLive ? betType + " (Live Game)" : betType + " (Pre Game)";
                    }
                }

                TemplateHandler.Assign(ref row, "bettype", betType);

                if (IsLiveCasino)
                {
                    var refNo = CastAide.AsDecimal(drBet["refno"]);
                    var choice = string.Empty;
                    if (dtChoice != null && dtChoice.Rows.Count > 0)
                    {
                        var _transid = CastAide.AsDecimal(drBet["refno"]);
                        choice = BuildChoiceColumnForLiveCasino(_transid, dtChoice);
                    }
                    TemplateHandler.Assign(ref row, "bet", choice);
                }
                else if (IsNumberGame && CastAide.AsInt32(drBet["bettype"]) == 90)
                {

                    int hdp1 = CastAide.AsInt32(drBet["hdp1"]);
                    int beteam = CastAide.AsInt32(drBet["betteam"]);
                    string _choice = _INumberWheelService.GetChoiceName(hdp1, beteam);
                    TemplateHandler.Assign(ref row, "bet", _choice);
                }
                else
                {
                    if (IsNumberGame)
                    {
                        int bettype = CastAide.AsInt32(drBet["bettype"]);
                        string _betteam = CastAide.AsString(drBet["betteam"]);
                        TemplateHandler.Assign(ref row, "bet", ConvertBeteamOfNumberGame(bettype, _betteam));
                    }
                    else if (IsLiveCasino)
                    {
                        TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                    }
                }

                TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                TemplateHandler.Assign(ref row, "currency", SelCurrencyName(CastAide.AsString(drBet["currency"])));

                TemplateHandler.Assign(ref row, "{hdp}", "--");

                TemplateHandler.Assign(ref row, "transtime", Convert.ToDateTime(drBet["transdate"]).ToString());

                string status = CastAide.AsString(drBet["status"]).ToLower();
                TemplateHandler.Assign(ref row, "status", status.ToUpper());

                if (IsLiveCasino)
                {

                    TemplateHandler.Assign(ref row, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"], 2));
                    TemplateHandler.Assign(ref row, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"], 2));
                    TemplateHandler.Assign(ref row, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"], 2));
                }


                TemplateHandler.Assign(ref row, "winloss", status == "running" ? "-" : NumberFormat.DecFormat(drBet["winlost"], 2));
                TemplateHandler.Assign(ref row, "comm", status == "running" ? "-" : NumberFormat.DecFormat(drBet["comm"], 2));
                TemplateHandler.Assign(ref row, "index", NumberFormat.DecFormat(index, 0));
                
                body.Append(row);
            }

            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", ListNegativebalanceCustomer(rows));

            #endregion
            return body;
        }

        public void SendEmailWhenDeleteBetToCSD(ResultModel result, DataTable dtMatch, DataTable dtUsers, DataTable dtChoice, bool IsNumberGame, bool IsLiveCasino)
        {
            try
            {

                if (dtMatch != null && dtMatch.Rows.Count>0 && dtUsers != null && dtUsers.Rows.Count>0 )
                {                   
                    var rows = dtUsers.AsEnumerable().Select(s => s).ToList();

                    string expression = "";

                    DataRow[] drTicketsDeleted = dtUsers.Select(expression, "");
                    int cnt = drTicketsDeleted.Count();

                    #region Template
                    string tplpath = "";
                    if (IsNumberGame)
                        tplpath = "NumberGame/";
                    if (IsLiveCasino)
                        tplpath = "LiveCasino/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");

                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                    string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");

                    #endregion

                    #region send mail

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        #region Template
                        string strHome = "";

                        #endregion


                        #region MatchInfo
                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;
                        string productType = "";
                        if (IsLiveCasino)
                            productType = "Live Casino";
                        else if (IsNumberGame)
                            productType = "Number Game";
                        else productType = "";

                        Subject = "Deleted " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";

                        string tplMatch = tplMatchAll.Clone().ToString();

                        TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);

                        if (IsLiveCasino)
                        {
                            strHome = CastAide.AsString(dtMatch.Rows[0]["GameEvent"]);
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", string.Empty);
                            TemplateHandler.RemoveBlock(ref tplMatch, "vs");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                        }
                        TemplateHandler.Assign(ref tplMatch, "EventDate", CastAide.AsDateTime(drMatch["EventDate"]).ToShortDateString());
                        TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                        TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["DATETIME"]));

                        string site = string.Empty;
                        string bgColor = string.Empty;
                        string isMP = string.Empty;
                        #endregion

                        #region Create Dataset dsUser

                        string ToMailCSD = "";
                        string CC = "", BCC = "";

                        var collection = SpecialDeleteSection.Instance.DeleteBet;

                        foreach (SpecialDeleteElement element in collection)
                        {
                            if (element.Alias.ToLower().Equals("deposit"))
                            {
                                #region send email

                                DataTable dtMail = SelMailingList();

                                foreach (DataRow dr in dtMail.Rows)
                                {
                                    site = string.Empty;
                                    var _rows = dtUsers.AsEnumerable().Where(s => s["site"].ToString().ToLower() == dr["site"].ToString().ToLower());

                                    if (_rows.Count() == 0)
                                        continue;

                                    var body = AssignData(dtUsers, dtChoice, dr["site"].ToString(), "site", tplUser, ref tplMatch, ref site, IsNumberGame, IsLiveCasino);

                                    TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                                    ToMailCSD = SplitMailList(dr["tolist"].ToString());
                                    CC = SplitMailList(dr["cclist"].ToString());
                                    BCC = SplitMailList(dr["bcclist"].ToString());

                                    var _subject = Subject.Clone().ToString().Replace("{site}", site);


                                    // send mail via gmail system
                                    bool f = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, _subject, _subject, tplMatch, null);
                                    string msg = "";
                                    if (f)
                                    {

                                        msg = "Details of deleted " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                        result.ErrorHandler.Add(new Result()
                                        {
                                            Key = "-100",
                                            Value = msg
                                        });
                                    }
                                    else
                                    {
                                        msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                                        result.ErrorHandler.Add(new Result()
                                        {
                                            Key = "100",
                                            Value = msg
                                        });
                                    }
                                }
                                
                                #endregion
                            }
                            else
                            {
                                var subCollections = element.Stores;
                                foreach (SpecialDeleteElement subElement in subCollections)
                                {
                                    site = string.Empty;
                                    var body = AssignData(dtUsers, dtChoice, subElement.Alias, subElement.Name, tplUser, ref tplMatch, ref site, IsNumberGame, IsLiveCasino);

                                    if (body.Length == 0)
                                        continue;

                                    TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                                    #region send email
                                    ToMailCSD = subElement.To;
                                    CC = subElement.CC;
                                    BCC = subElement.BCC;

                                    var _subject = Subject.Clone().ToString().Replace("{site}", site);


                                    // send mail via gmail system
                                    bool f = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, _subject, _subject, tplMatch, null);
                                    string msg = "";
                                    if (f)
                                    {

                                        msg = "Details of deleted " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                        result.ErrorHandler.Add(new Result()
                                        {
                                            Key = "-100",
                                            Value = msg
                                        });
                                    }
                                    else
                                    {
                                        msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                                        result.ErrorHandler.Add(new Result()
                                        {
                                            Key = "100",
                                            Value = msg
                                        });
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion


                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorHandler.Add(new Result()
                {
                    Key = "100",
                    Value = ex.Message
                });
            }
        }

        public bool SendMailWhenRecoverBetToDBA(string ErrMsg, string ErrCode, string TransID)
        {
            try
            {
                if (string.IsNullOrEmpty(ErrMsg)) ErrMsg = string.Empty;
                if (string.IsNullOrEmpty(ErrCode)) ErrMsg = string.Empty;

                SpecialDeleteElement mailling = SpecialDeleteSection.Instance.NotificationDBA["EmailNotification"];
                string ToMailCSD = mailling.To;
                string CC = mailling.CC;
                string BCC = mailling.BCC;
                string Subject = WebUtils.LoadTemplate("SpecialDelete/LiveCasino/RecoverBetsSubjectSendMailToDBA.htm");
                string Body = WebUtils.LoadTemplate("SpecialDelete/LiveCasino/RecoverBetsBodySendMailToDBA.htm");
                Body = Body.Replace("{ErrCode}", ErrCode).Replace("{ErrMsg}", ErrMsg);
                Body = Body.Replace("{TransID}", TransID);
                var isSuccess = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, Subject, Subject, Body, null);
                return isSuccess;
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
                return false;
            }
        }

        public void SendEmailWhenRecoverBetToInternalGroup(ResultModel result, DbResult rs, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string transid, bool IsNegativeBalance)
        {
            string ErrMsg = "";
            try
            {
                if (rs.ResultSet.Tables.Count >= 2)
                {
                    DataTable dtMatch = rs.ResultSet.Tables[0];
                    DataTable dtUsers = rs.ResultSet.Tables[1];

                    string tplpath = "";
                    if (IsNumberGame)
                        tplpath = "NumberGame/";
                    if (IsLiveCasino)
                        tplpath = "LiveCasino/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");
                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Recovered");

                    string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");
                                        
                    tplMatchAll = tplMatchAll.Replace("Bets", "Bet");
                    tplUser = tplUser.Replace("Bets", "Bet");

                    #region send mail

                    string expression = "";

                    if (IsRealCurrency)
                    {
                        expression = "currency  <> 20";
                    }
                    else
                    {
                        expression = "currency = 20";
                    }

                    DataRow[] drTicketsRecovered = dtUsers.Select(expression, "");

                    int cnt = drTicketsRecovered.Count();

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        #region Template
                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        int bettype = CastAide.AsInt32(dtUsers.Rows[0]["bettype"]);
                        if (IsNumberGame)
                        {
                            //TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                            //TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplUser, "score");
                            TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                            strHome = CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]) + " No " + CastAide.AsString(dtMatch.Rows[0]["matchcode"]);

                        }
                        #endregion

                        #region Create Dataset dsUser

                        #region MatchInfo
                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;

                        string productType = "";
                        if (IsNumberGame)
                            productType = "Number Game";
                        if(IsLiveCasino)
                            productType = "Live Casino";

                        if (IsRealCurrency)
                        {
                            Subject = "Real Currency - Recover " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                        }
                        else
                        {
                            Subject = "Test Currency - Recover " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                        }

                        string tplMatch = tplMatchAll.Clone().ToString();

                        //Match
                        if(IsNumberGame)
                            strHome = "Number Game No " + drMatch["matchcode"];

                        TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);

                        if (IsLiveCasino)
                        {
                            strHome = CastAide.AsString(dtMatch.Rows[0]["GameEvent"]);
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", string.Empty);
                            TemplateHandler.RemoveBlock(ref tplMatch, "vs");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                        }

                        TemplateHandler.Assign(ref tplMatch, "EventDate", CastAide.AsDateTime(drMatch["EventDate"]).ToShortDateString());
                        TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                        TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));

                        string site = string.Empty;
                        string bgColor = string.Empty;
                        string isMP = string.Empty;
                        #endregion

                        #region AssignRow
                        StringBuilder body = new StringBuilder();
                        StringBuilder body_mix = new StringBuilder();
                        string betteam = string.Empty;
                        int index = 0;
                        string username = "";
                        foreach (DataRow drBet in drTicketsRecovered)
                        {
                            index++;
                            site = drBet["site"].ToString();
                            bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                            string row = tplUser.Clone().ToString();

                            username = CastAide.AsString(drBet["username"]);
                            TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                            TemplateHandler.Assign(ref row, "username", username);                            

                            
                            string bettypeName = CacheRef.GetBettype(drBet["bettype"]);
                            TemplateHandler.Assign(ref row, "refno", drBet["transid"]);

                            if (IsLiveCasino)
                            {
                                var refNo = CastAide.AsDecimal(drBet["refno"]);
                                var choice = string.Empty;
                                if (rs.ResultSet.Tables.Count >= 3)
                                    choice = BuildChoiceColumnForLiveCasino(refNo, rs.ResultSet.Tables[2]);
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                TemplateHandler.Assign(ref row, "bet", choice);                                
                            }
                            else if (CastAide.AsInt32(drBet["bettype"]) == 90)
                            {
                                bool IsLive = false;
                                IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                bettypeName += IsLive ? " (Live Game)" : " (Pre Game)";

                                int hdp1 = CastAide.AsInt32(drBet["hdp1"]);
                                int beteam = CastAide.AsInt32(drBet["betteam"]);
                                string _choice = _INumberWheelService.GetChoiceName(hdp1, beteam);

                                TemplateHandler.Assign(ref row, "bet", _choice);
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);

                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                if (IsNumberGame)
                                {
                                    bettype = CastAide.AsInt32(drBet["bettype"]);
                                    string _betteam = CastAide.AsString(drBet["betteam"]);
                                    TemplateHandler.Assign(ref row, "bet", ConvertBeteamOfNumberGame(bettype, _betteam));
                                }
                                else if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                }
                            }
                            TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                            TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                            TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));

                            string status = CastAide.AsString(drBet["status"]).ToLower();

                            TemplateHandler.Assign(ref row, "winloss", status == "running" ? "-" : NumberFormat.DecFormat(drBet["winlost"], 2));
                            TemplateHandler.Assign(ref row, "comm", string.Empty);                                                        
                            TemplateHandler.Assign(ref row, "currency", SelCurrencyName(CastAide.AsString(drBet["currency"])));



                            TemplateHandler.Assign(ref row, "{hdp}", "--");
                            TemplateHandler.Assign(ref row, "index", index);
                            TemplateHandler.Assign(ref row, "transtime", Convert.ToDateTime(drBet["transdate"]).ToString());
                            TemplateHandler.Assign(ref row, "status", status.ToUpper());
                            TemplateHandler.Assign(ref row, "creator", drBet["creator"]);

                            if (IsLiveCasino)
                            {
                                TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));                                
                                TemplateHandler.Assign(ref row, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"],2));
                                TemplateHandler.Assign(ref row, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"],2));
                                TemplateHandler.Assign(ref row, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"],2));
                            }

                            body.Append(row);
                        }
                        #endregion

                        if(IsNegativeBalance)
                            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", "<b>Negative bet credit customer(s):</b> " + username);
                        else
                            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", "");

                        TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                        string ToMailCSD = "";
                        string CC = "", BCC = "";


                        var usernames = SpecialDeleteSection.Instance[SpecialDeleteSection.StoreCollection.NotificationRealCurrency]["usernames"];
                        bool IsDeveloperRecovered = false;

                        foreach (SpecialDeleteElement element in usernames.Stores)
                        {
                            IsDeveloperRecovered = AppSession.AccUserName.ToLower().Contains(element.Name.ToLower());
                            if (IsDeveloperRecovered)
                                break;
                        }

                        SpecialDeleteElement mailElement = new SpecialDeleteElement();

                        if (IsRealCurrency)
                        {                           
                            mailElement = SpecialDeleteSection.Instance.NotificationRealCurrency["EmailNotification"];
                            ToMailCSD = mailElement.To;
                            CC = mailElement.CC;
                            BCC = mailElement.BCC;
                        }
                        else // NotificationTestCurrency - for testing
                        {
                            mailElement = SpecialDeleteSection.Instance.NotificationTestCurrency["EmailNotification"];
                            ToMailCSD = mailElement.To;
                            CC = mailElement.CC;
                            BCC = mailElement.BCC;
                        }

                        Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);


                        Subject = Subject.Replace("{site}", site);


                        //Send mail job
                        StringBuilder LogMail = new StringBuilder();
                        LogMail.Append("<!--Start-->Date: " + DateTime.Now.ToString() + "<!--/Start--><br>" + Environment.NewLine);
                        LogMail.Append("<!--To-->To: " + ToMailCSD + "<!--/To--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Subject-->Subject: " + Subject + "<!--/Subject--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Content-->" + Environment.NewLine + tplMatch + Environment.NewLine + "<!--/Content-->" + Environment.NewLine);
                        LogMail.Append("<!--End--><hr/><!--/End-->" + Environment.NewLine + Environment.NewLine);

                        //var CCMail = SpecialDeleteSection.Instance.CC["AlwaysReceived"];


                        // send mail via gmail system
                        bool f = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                        string msg = "";
                        site = site.ToUpper();

                       

                        if (f)
                        {
                            msg = "Details of recover " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                            result.ErrorHandler.Add(new Result() { 
                                Key = "-100",
                                Value = msg
                            });
                        }
                        else
                        {
                            msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                            result.ErrorHandler.Add(new Result()
                            {
                                Key = "100",
                                Value = msg
                            });
                        }



                        #endregion

                        
                    }
                    #endregion


                }               

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorHandler.Add(new Result()
                {
                    Key = "100",
                    Value = ex.Message
                });
            }            
        }

        public void SendEmailWhenRecoverBetToCSD(ResultModel result, DbResult rs, bool IsNumberGame, bool IsLiveCasino, string transid, bool IsNegativeBalance)
        {
            string ErrMsg = "";
            try
            {
                if (rs.ResultSet.Tables.Count >= 2)
                {
                    DataTable dtMatch = rs.ResultSet.Tables[0];
                    DataTable dtUsers = rs.ResultSet.Tables[1];

                    string tplpath = "";
                    if (IsNumberGame)
                        tplpath = "NumberGame/";
                    if (IsLiveCasino)
                        tplpath = "LiveCasino/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");
                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Recovered");

                    string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");

                    tplMatchAll = tplMatchAll.Replace("Bets", "Bet");
                    tplUser = tplUser.Replace("Bets", "Bet");

                    #region send mail

                    string expression = "";
                    expression = "currency  <> 20";

                    DataRow[] drTicketsRecovered = dtUsers.Select(expression, "");

                    int cnt = drTicketsRecovered.Count();

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        #region Template
                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        int bettype = CastAide.AsInt32(dtUsers.Rows[0]["bettype"]);
                        if (IsNumberGame)
                        {
                            //TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                            //TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplUser, "score");
                            TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                            strHome = CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]) + " No " + CastAide.AsString(dtMatch.Rows[0]["matchcode"]);

                        }
                        #endregion

                        #region Create Dataset dsUser

                        #region MatchInfo
                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;

                        string productType = "";
                        if (IsNumberGame)
                            productType = "Number Game";
                        if (IsLiveCasino)
                            productType = "Live Casino";

                        Subject = "Recover " + productType + " Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";

                        string tplMatch = tplMatchAll.Clone().ToString();

                        //Match
                        if (IsNumberGame)
                            strHome = "Number Game No " + drMatch["matchcode"];

                        TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);

                        if (IsLiveCasino)
                        {
                            strHome = CastAide.AsString(dtMatch.Rows[0]["GameEvent"]);
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", string.Empty);
                            TemplateHandler.RemoveBlock(ref tplMatch, "vs");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                            TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                        }

                        TemplateHandler.Assign(ref tplMatch, "EventDate", CastAide.AsDateTime(drMatch["EventDate"]).ToShortDateString());
                        TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                        TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));

                        string site = string.Empty;
                        string bgColor = string.Empty;
                        string isMP = string.Empty;
                        #endregion

                        #region AssignRow
                        StringBuilder body = new StringBuilder();
                        StringBuilder body_mix = new StringBuilder();
                        string betteam = string.Empty;
                        int index = 0;
                        string username = "";
                        foreach (DataRow drBet in drTicketsRecovered)
                        {
                            index++;
                            site = drBet["site"].ToString();
                            bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                            string row = tplUser.Clone().ToString();

                            username = CastAide.AsString(drBet["username"]);
                            TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                            TemplateHandler.Assign(ref row, "username", username);

                            TemplateHandler.Assign(ref row, "refno", drBet["transid"]);
                            string bettypeName = CacheRef.GetBettype(drBet["bettype"]);

                            if (IsLiveCasino)
                            {
                                var refNo = CastAide.AsDecimal(drBet["refno"]);
                                var choice = string.Empty;
                                if (rs.ResultSet.Tables.Count >= 3)
                                    choice = BuildChoiceColumnForLiveCasino(refNo, rs.ResultSet.Tables[2]);
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                TemplateHandler.Assign(ref row, "bet", choice);
                            }
                            else if (CastAide.AsInt32(drBet["bettype"]) == 90)
                            {
                                bool IsLive = false;
                                IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                bettypeName += IsLive ? " (Live Game)" : " (Pre Game)";

                                int hdp1 = CastAide.AsInt32(drBet["hdp1"]);
                                int beteam = CastAide.AsInt32(drBet["betteam"]);
                                string _choice = _INumberWheelService.GetChoiceName(hdp1, beteam);

                                TemplateHandler.Assign(ref row, "bet", _choice);
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                if (IsNumberGame)
                                {
                                    bettype = CastAide.AsInt32(drBet["bettype"]);
                                    string _betteam = CastAide.AsString(drBet["betteam"]);
                                    TemplateHandler.Assign(ref row, "bet", ConvertBeteamOfNumberGame(bettype, _betteam));
                                }
                                else if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                }
                            }
                            TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                            TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                            TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));

                            string status = CastAide.AsString(drBet["status"]).ToLower();

                            TemplateHandler.Assign(ref row, "winloss", status == "running" ? "-" : NumberFormat.DecFormat(drBet["winlost"], 2));
                            TemplateHandler.Assign(ref row, "comm", string.Empty);
                            TemplateHandler.Assign(ref row, "currency", SelCurrencyName(CastAide.AsString(drBet["currency"])));



                            TemplateHandler.Assign(ref row, "{hdp}", "--");
                            TemplateHandler.Assign(ref row, "index", index);
                            TemplateHandler.Assign(ref row, "transtime", Convert.ToDateTime(drBet["transdate"]).ToString());
                            TemplateHandler.Assign(ref row, "status", status.ToUpper());
                            TemplateHandler.Assign(ref row, "creator", drBet["creator"]);

                            if (IsLiveCasino)
                            {
                                TemplateHandler.Assign(ref row, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                                TemplateHandler.Assign(ref row, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"], 2));
                                TemplateHandler.Assign(ref row, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"], 2));
                                TemplateHandler.Assign(ref row, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"], 2));
                            }

                            body.Append(row);
                        }
                        #endregion

                        if (IsNegativeBalance)
                            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", "<b>Negative bet credit customer(s):</b> " + username);
                        else
                            TemplateHandler.Assign(ref tplMatch, "ListNegativebalanceCustomer", "");

                        TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                        string ToMailCSD = "";
                        string CC = "", BCC = "";

                        SpecialDeleteElement mailElement = new SpecialDeleteElement();

                        // always send to csd
                        mailElement = SpecialDeleteSection.Instance.RecoverBet["MailForRecoverBet"];
                        ToMailCSD = mailElement.To;
                        CC = mailElement.CC;
                        BCC = mailElement.BCC;                         

                        Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);


                        Subject = Subject.Replace("{site}", site);


                        //Send mail job
                        StringBuilder LogMail = new StringBuilder();
                        LogMail.Append("<!--Start-->Date: " + DateTime.Now.ToString() + "<!--/Start--><br>" + Environment.NewLine);
                        LogMail.Append("<!--To-->To: " + ToMailCSD + "<!--/To--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Subject-->Subject: " + Subject + "<!--/Subject--><br>" + Environment.NewLine);
                        LogMail.Append("<!--Content-->" + Environment.NewLine + tplMatch + Environment.NewLine + "<!--/Content-->" + Environment.NewLine);
                        LogMail.Append("<!--End--><hr/><!--/End-->" + Environment.NewLine + Environment.NewLine);

                        //var CCMail = SpecialDeleteSection.Instance.CC["AlwaysReceived"];

                        // send mail via gmail system
                        bool f = SendMailExtension.AccSendMail(ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                        string msg = "";
                        site = site.ToUpper();
                                            

                        if (f)
                        {
                            msg = "Details of recover " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                            result.ErrorHandler.Add(new Result()
                            {
                                Key = "-100",
                                Value = msg
                            });
                        }
                        else
                        {
                            msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                            result.ErrorHandler.Add(new Result()
                            {
                                Key = "100",
                                Value = msg
                            });
                        }



                        #endregion


                    }
                    #endregion


                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorHandler.Add(new Result()
                {
                    Key = "100",
                    Value = ex.Message
                });
            }
        }



        string GetBetteam(int bettype, int betteam)
        {
            string rs = "";
            switch (bettype)
            {
                case 1101://Baccarat
                    rs = CacheRef.GetLiveCasinoResult(betteam);
                    break;
                case 1102://Live Roulette
                    rs = CacheRef.GetRouletteBetResult(betteam);
                    break;
            }
            return rs;
        }

        private string BuildChoiceColumnForLiveCasino(decimal refNo, DataTable table1)
        {

            var dataByRefNo = table1.AsEnumerable().Where(s => CastAide.AsDecimal(s["refNo"]) == refNo);
            
            var arrChoice = new List<string>();
            var choices = new List<Choice>();

            foreach (DataRow row1 in dataByRefNo)
            {
                int bettype = CastAide.AsInt32(row1["bettype"]);
                int betteam = CastAide.AsInt32(row1["betteam"]);

                choices.Add(new Choice
                {
                    Betteam = GetBetteam(bettype, betteam),
                    Stake = NumberFormat.DecFormat(row1["stake"],2),
                    Odds = NumberFormat.DecFormat(row1["odds"], 2)
                });
            }

            foreach (Choice c in choices)
            {
                arrChoice.Add("<span><span style='color:red'>" + c.Betteam + "</span> " + c.Stake + " <span style='color:#4F81BD'>@" + c.Odds + "</span></span>");
            }
         
            return string.Join("<br />", arrChoice.ToArray());
        }

        public string ListNegativebalanceCustomer(List<DataRow> rows)
        {
            var arrCustomers = new List<string>();
            var users = rows.AsEnumerable().Where(s => CastAide.AsBoolean(s["isNegative"]) == true).Select(s => CastAide.AsString(s["username"])).Distinct().ToList();
            string rs = "";

            foreach (string username in users)
            {
                arrCustomers.Add(username);
            }
            if (users.Count > 0)
            {
                rs = "<b>Negative bet credit customer(s):</b> " + string.Join(", ", users);
            }

            return rs;
        }


      
    }
}
