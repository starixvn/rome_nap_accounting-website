using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete.Mixparlay;

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public class SpecialDeleteMixparlay : SpecialDeleteBase
    {
        private string mpTplItem = string.Empty;

        public SpecialDeleteMixparlay()
        {
            mpTplItem = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/Mixpalay/TplMixparlayEvent.htm");
        }

        public List<MixParlayDTO> FetchData(DataTable table, string type)
        {
            List<MixParlayDTO> rows = new List<MixParlayDTO>();
            try
            {
                int mergeIndex = 1;
                foreach (DataRow dataRow in table.Rows)
                {
                    MixParlayDTO mpItem = new MixParlayDTO();

                    mpItem.Type = type;
                    mpItem.Order = mergeIndex;
                    mpItem.Account = CastAide.AsString(dataRow["username"]);
                    mpItem.Refno = CastAide.AsString(dataRow["refno"]);
                    mpItem.TransId = CastAide.AsString(dataRow["transid"]);
                    mpItem.SubTransId = CastAide.AsString(dataRow["subtransid"]);
                    mpItem.Odds = NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dataRow["odds"]), CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])), 3);
                    mpItem.OddType = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])));
                    mpItem.Amount = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["stake"]), 2);
                    mpItem.Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
                    mpItem.TransTime = CastAide.AsDateTime(dataRow["transdate"]).ToString();
                    mpItem.Status = CastAide.AsString(dataRow["status"]);
                    mpItem.WinLoss = mpItem.Status == "running" ? "" : NumberFormat.DecFormat(dataRow["winlost"], 2);
                    mpItem.SPT = NumberFormat.DecFormat(dataRow["spositiontaking"], 2);
                    mpItem.MPT = NumberFormat.DecFormat(dataRow["mpositiontaking"], 2);
                    mpItem.APT = NumberFormat.DecFormat(dataRow["apositiontaking"], 2);
                    mpItem.Bettype = CacheRef.GetBettype(dataRow["bettype"]);

                    mpItem.IsDelete = "1";
                    mpItem.Liveindicator = CastAide.AsBoolean(dataRow["choice_liveindicator"], false);

                    // format choice
                    mpItem.Choice = GenMPChoiceItem(mpTplItem, dataRow, "0");
                    rows.Add(mpItem);
                    mergeIndex++;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rows;
        }

        /// <summary>
        /// GenMPRowClient - Web UI
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="transid"></param>
        /// <returns>string</returns>
        public string GenMPRowClient(string refno, string matchid)
        {
            string strData = "";
            try
            {
                ISpecialDeleteDAO specialDeleteDAO = new SpecialDeleteDAO();
                DbResult ds = specialDeleteDAO.GetMPTicket(refno);
                string eventItems = string.Empty;
                if (ds != null && ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    int ind = 1;
                    foreach (DataRow row2 in ds.ResultSet.Tables[0].Select())
                    {
                        string mpItem = mpTplItem.Clone().ToString();
                        TemplateHandler.Assign(ref mpItem, "MoreBetInfo", "");
                        string mpHome = CacheRef.GetTeamName(row2["homeid"]) + (CastAide.AsBoolean(row2["isNeutral"]) ? "(N)" : string.Empty);
                        string mpAway = CacheRef.GetTeamName(row2["awayid"]);
                        betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row2["betteam"].ToString(), Convert.ToInt32(row2["bettype"]), "e");
                        sportypeName = CacheRef.GetSportName(row2["sporttype"]);
                        leagueName = CacheRef.GetLeagueName(row2["leagueid"]);
                        bettype = Convert.IsDBNull(row2["bettype"]) ? 0 : Convert.ToInt32(row2["bettype"]);
                        hdp1 = Convert.IsDBNull(row2["hdp1"]) ? 0 : Convert.ToDecimal(row2["hdp1"]);
                        hdp2 = Convert.IsDBNull(row2["hdp2"]) ? 0 : Convert.ToDecimal(row2["hdp2"]);
                        odds = Convert.IsDBNull(row2["odds"]) ? 0 : Convert.ToDecimal(row2["odds"]);
                        bettypeName = CacheRef.GetBettype(row2["bettype"]);

                        string tid = row2["transid"].ToString();
                        string mid = row2["matchid"].ToString();
                        string bid = row2["bettype"].ToString();

                        int statusId = CastAide.AsInt32(row2["statusid"]);
                        string borderTop = (ind > 1) ? " b-top" : "";
                        bool liveindicator = CastAide.AsBoolean(row2["liveindicator"], false);

                        if (mid == matchid || matchid == "0")
                        {
                            string clsZoom = matchid == "0" ? "in" : "out";
                            TemplateHandler.Assign(ref mpItem, "mp", "" + borderTop);
                            TemplateHandler.Assign(ref mpItem, "id", string.Format("id='match-{0}'", refno));
                            TemplateHandler.Assign(ref mpItem, "refno", refno);
                            TemplateHandler.Assign(ref mpItem, "zoom", "<span id='zoom-1-" + refno + "' class='zoom " + clsZoom + "' refno='" + refno + "' data='1'><img border='0' src='../../App_Themes/Images/zoom_out.png' align='top' /></span>");
                            TemplateHandler.Assign(ref mpItem, "sel", "sel");
                            string clsCur = liveindicator ? "live" : "cur";
                            TemplateHandler.Assign(ref mpItem, "cur", clsCur);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref mpItem, "mp", "mp" + borderTop);
                            TemplateHandler.Assign(ref mpItem, "id", "");
                            TemplateHandler.Assign(ref mpItem, "refno", refno);
                            TemplateHandler.Assign(ref mpItem, "zoom", "");
                            TemplateHandler.Assign(ref mpItem, "sel", "");
                            TemplateHandler.Assign(ref mpItem, "cur", "");
                        }

                        string formatlive = string.Empty;
                        if (liveindicator)
                        {
                            formatlive = string.Format("&nbsp;<b>[{0}-{1}]</b>", CastAide.AsInt32(row2["livehomescore"], 0), CastAide.AsInt32(row2["liveawayscore"], 0));
                        }
                        TemplateHandler.Assign(ref mpItem, "formatlive", formatlive);

                        switch (bettype)
                        {
                            case 1:
                            case 7:
                            case 20:
                            case 21:
                            case 22:
                            case 124:
                            case 125:
                                GenHdpEvent(ref mpItem, mpHome, mpAway, betteam, statusId, true);
                                break;

                            case 2:
                            case 12:
                                GenOEEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 3:
                            case 8:
                                GenOUEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 4:
                            case 14:
                            case 30:
                                GenCSEvent(ref mpItem, mpHome, mpAway, true);
                                break;

                            case 5:
                            case 15:
                                Gen1x2Event(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 6:
                            case 16:
                                GenTGEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 24:
                                GenDoubleChanceEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 25:
                                GenDrawNoBetEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 26:
                                GenBothOneNeitherEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 27:
                                GenToWinToNilEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 28:
                                GenThreeWayHandicap(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 13:
                                GenCleanSheetEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;

                            case 121:
                            case 122:
                            case 123:
                                GenHomeAwayDrawNoBet(ref mpItem, mpHome, mpAway, statusId, betteam, true);
                                break;

                            default:
                                mpItem = string.Empty;
                                break;
                        }
                        strData += mpItem;
                        ind++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return strData;
        }

        /// <summary>
        /// GenMPRowItem
        /// </summary>
        /// <param name="mpItem"></param>
        /// <param name="row2"></param>
        /// <returns></returns>
        public string GenMPChoiceItem(string mpItem, DataRow row2, string matchid)
        {
            try
            {
                TemplateHandler.Assign(ref mpItem, "MoreBetInfo", "");
                string mpHome = CacheRef.GetTeamName(row2["choice_HomeID"]) + (CastAide.AsBoolean(row2["choice_isNeutral"]) ? "(N)" : string.Empty);
                string mpAway = CacheRef.GetTeamName(row2["choice_AwayID"]);
                betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row2["choice_Betteam"].ToString(), CastAide.AsInt32(row2["choice_Bettype"]), "e");
                sportypeName = CacheRef.GetSportName(row2["choice_Sporttype"]);
                leagueName = CacheRef.GetLeagueName(row2["choice_LeagueID"]);
                bettype = Convert.IsDBNull(row2["choice_Bettype"]) ? 0 : Convert.ToInt32(row2["choice_Bettype"]);
                hdp1 = Convert.IsDBNull(row2["choice_hdp1"]) ? 0 : Convert.ToDecimal(row2["choice_hdp1"]);
                hdp2 = Convert.IsDBNull(row2["choice_hdp2"]) ? 0 : Convert.ToDecimal(row2["choice_hdp2"]);
                odds = Convert.IsDBNull(row2["choice_odds"]) ? 0 : Convert.ToDecimal(row2["choice_odds"]);
                bettypeName = CacheRef.GetBettype(bettype);

                string tid = row2["transid"].ToString();
                string mid = row2["choice_matchid"].ToString();
                string refno = row2["refno"].ToString();
                bool liveindicator = CastAide.AsBoolean(row2["choice_liveindicator"], false);

                TemplateHandler.Assign(ref mpItem, "refno", refno);
                TemplateHandler.Assign(ref mpItem, "cur", liveindicator ? "live" : "cur");
                if (mid == matchid || matchid == "0")
                {
                    string clsZoom = matchid == "0" ? "in" : "";
                    TemplateHandler.Assign(ref mpItem, "mp", "");
                    TemplateHandler.Assign(ref mpItem, "id", string.Format("id='match-{0}'", refno));
                    TemplateHandler.Assign(ref mpItem, "zoom", "<span id='zoom-0-" + refno + "' class='zoom " + clsZoom + "' refno='" + refno + "' data='0'><img border='0' src='../../App_Themes/Images/zoom_in.png' align='top' /></span>");
                    TemplateHandler.Assign(ref mpItem, "sel", "sel");
                }
                else
                {
                    TemplateHandler.Assign(ref mpItem, "mp", "mp");
                    TemplateHandler.Assign(ref mpItem, "id", refno);
                    TemplateHandler.Assign(ref mpItem, "zoom", "");
                    TemplateHandler.Assign(ref mpItem, "sel", "");
                }

                string formatlive = string.Empty;
                if (liveindicator)
                {
                    formatlive = string.Format("&nbsp;<b>[{0}-{1}]</b>", CastAide.AsInt32(row2["choice_livehomescore"], 0), CastAide.AsInt32(row2["choice_liveawayscore"], 0));
                }
                TemplateHandler.Assign(ref mpItem, "formatlive", formatlive);

                switch (bettype)
                {
                    case 1:
                    case 7:
                    case 20:
                    case 21:
                    case 22:
                    case 124:
                    case 125:
                        GenHdpEvent(ref mpItem, mpHome, mpAway, betteam, true);
                        break;

                    case 2:
                    case 12:
                        GenOEEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 3:
                    case 8:
                        GenOUEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 4:
                    case 14:
                    case 30:
                        GenCSEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 5:
                    case 15:
                        Gen1x2Event(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 6:
                    case 16:
                        GenTGEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 24:
                        GenDoubleChanceEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 25:
                        GenDrawNoBetEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 26:
                        GenBothOneNeitherEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 27:
                        GenToWinToNilEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 28:
                        GenThreeWayHandicap(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 13:
                        GenCleanSheetEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 121:
                    case 122:
                    case 123:
                        GenHomeAwayDrawNoBet(ref mpItem, mpHome, mpAway, betteam, true);
                        break;

                    default:
                        mpItem = string.Empty;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return mpItem;
        }

        /// <summary>
        /// SendPrivateMsg
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="matchId"></param>
        /// <param name="pmsg"></param>
        /// <returns></returns>
        public bool SendPrivateMsg(DbResult rs, int matchId, Message pmsg)
        {
            bool result = false;
            try
            {
                #region send private message to users

                if (rs.ResultSet.Tables.Count > 0)
                {
                    DataTable dtMsg = rs.ResultSet.Tables[2];
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
                            string uname = Key.Key.ToString().Split('^')[1];//This is a UserName of ticket that is deleted [TargetName Field]
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

                        string strTransID_Current = "";
                        if (dtBets.Rows.Count > 0)
                            strTransID_Current = CastAide.AsString(dtBets.Rows[0]["transids"]);
                        int CountBetsBelong1Member = CastAide.AsInt32(dtBets.Compute("count(transids)", "transids='" + strTransID_Current + "'"));

                        // send private message to users for cancel their transactions
                        int MessageId = 0;
                        foreach (DataRow dr in dtBets.Rows)
                        {
                            int[] custid = { CastAide.AsInt32(dr["custid"]) }; // This is a CustID of ticket that is deleted [TargetID Field]
                            string[] uname = { CastAide.AsString(dr["username"]) };//This is a UserName of ticket that is deleted [TargetName Field]
                            if (CastAide.AsInt32(dr["custid"]) < 100) continue;//SRecommend < 100 and roleid=3
                            string[] bettransid = { CastAide.AsString(dr["transids"]) };

                            if (MessageId == 0)
                            {
                                MessageId = Messenger.SetMessage(pmsg, custid, uname, CastAide.AsInt32(matchId), bettransid);
                            }
                            else
                            {
                                Messenger.AddTargt2Message(MessageId, custid, uname, bettransid);
                            }
                        }
                    }
                }

                #endregion send private message to users

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// SendEmailToCSD
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        /// <param name="dbResult"></param>
        /// <returns></returns>
        public string SendEmailToCSD(int errcode, string errmsg, DataSet rs, string reason)
        {
            string ErrMsg = "";
            try
            {
                ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Msg");
                ErrMsg = AlertMessage.GetMsg(ErrMsg, AlertMessage.MsgType.Success);

                if (rs != null && rs.Tables.Count >= 2)
                {
                    DataTable dtMatch = rs.Tables[0];
                    DataTable dtUsers = new DataTable();

                    dtUsers = rs.Tables[1];

                    string tplpath = "Mixpalay/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplSendMailMixparlayFull.htm");
                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                    string tplUserMP = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplMPSendMailRow.htm");

                    //For special bets - 12EU
                    string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail_SpecialBet.htm");
                    TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                    string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMailAllRow_SpecialBet.htm");

                    if (dtMatch.Rows.Count > 0 && dtUsers.Rows.Count > 0)
                    {
                        #region send mail to CSD

                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        if (CastAide.AsInt32(dtUsers.Rows[0]["choice_bettype"]) == 10)
                        {
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

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

                        var collections = SpecialDeleteSection.Instance.DeleteBet;

                        SpecialDeleteDataforMaillings maillings = new SpecialDeleteDataforMaillings();
                        maillings.Items = new List<SpecialDeleteDataforMailing>();
                        string sites = string.Empty;
                        string users = string.Empty;
                        foreach (SpecialDeleteElement collection in collections)
                        {
                            if (collection.Alias.ToLower().Equals("deposit"))
                            {
                                #region deposit

                                ISpecialDeleteDAO _specialDAO = new SpecialDeleteDAO();
                                DataTable dtMail = _specialDAO.SelMailingList();

                                foreach (DataRow dr in dtMail.Rows)
                                {
                                    DataTable dtUser = dtUsers.Clone();

                                    var _rows = dtUsers.AsEnumerable().Where(s => s["site"].ToString().ToLower() == dr["site"].ToString().ToLower());

                                    if (_rows.Count() == 0)
                                        continue;

                                    SpecialDeleteDataforMailing mailing = new SpecialDeleteDataforMailing()
                                    {
                                        To = SplitMailList(dr["tolist"].ToString()),
                                        Cc = SplitMailList(dr["cclist"].ToString()),
                                        Bcc = SplitMailList(dr["bcclist"].ToString()),
                                        Name = "site",
                                        Alias = dr["site"].ToString()
                                    };
                                    mailing.Items = new DataTable();

                                    foreach (DataRow row in _rows)
                                    {
                                        dtUser.ImportRow(row);
                                    }

                                    mailing.Items = dtUser;
                                    maillings.Items.Add(mailing);
                                }

                                #endregion deposit
                            }
                            else
                            {
                                #region others group

                                foreach (SpecialDeleteElement store in collection.Stores)
                                {
                                    SpecialDeleteDataforMailing mailing = new SpecialDeleteDataforMailing()
                                    {
                                        To = store.To,
                                        Cc = store.CC,
                                        Bcc = store.BCC,
                                        Name = store.Name,
                                        Alias = store.Alias
                                    };
                                    mailing.Items = new DataTable();

                                    DataTable dtUser = dtUsers.Clone();
                                    string expression = string.Empty;
                                    expression = "" + store.Name + "='" + store.Alias + "'";

                                    DataRow[] rows = dtUsers.Select(expression);
                                    foreach (DataRow row in rows)
                                    {
                                        dtUser.ImportRow(row);
                                    }

                                    mailing.Items = dtUser;
                                    maillings.Items.Add(mailing);
                                }

                                #endregion others group
                            }
                        }

                        #endregion Create Dataset dsUser

                        foreach (SpecialDeleteDataforMailing dataforMailing in maillings.Items)
                        {
                            if (dataforMailing.Items.Rows.Count > 0)
                            {
                                #region MatchInfo

                                DataRow drMatch = dtMatch.Rows[0];
                                string Subject = string.Empty;

                                Subject = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Subject");
                                Subject = string.Format(Subject, "{site}", "{site_name}", Convert.ToDateTime(drMatch["datetime"]));

                                string tplMatch = tplMatchAll.Clone().ToString();

                                //Match
                                if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
                                    strHome = "Number Game No " + drMatch["matchcode"];

                                TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);
                                TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                                TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                                TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                                TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                                TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));
                                TemplateHandler.Assign(ref tplMatch, "reason", reason);

                                string site = string.Empty;

                                #endregion MatchInfo

                                #region AssignRow

                                StringBuilder body_mix = new StringBuilder();
                                foreach (DataRow drBet in dataforMailing.Items.Rows)
                                {
                                    site = drBet["site"].ToString();
                                    string rowMP = tplUserMP.Clone().ToString();

                                    TemplateHandler.Assign(ref rowMP, "username", drBet["username"]);
                                    TemplateHandler.Assign(ref rowMP, "refno", drBet["transid"]);
                                    TemplateHandler.Assign(ref rowMP, "transdate", drBet["transdate"]);
                                    TemplateHandler.Assign(ref rowMP, "bettype", CacheRef.GetBettype(drBet["bettype"]));

                                    string status = CastAide.AsString(drBet["status"]);
                                    string winloss = (status == "running") ? "" : NumberFormat.DecFormat(drBet["winlost"], 2);

                                    TemplateHandler.Assign(ref rowMP, "winloss", winloss);
                                    TemplateHandler.Assign(ref rowMP, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                    TemplateHandler.Assign(ref rowMP, "oddstype", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));

                                    TemplateHandler.Assign(ref rowMP, "status", status);
                                    TemplateHandler.Assign(ref rowMP, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"], 2));
                                    TemplateHandler.Assign(ref rowMP, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"], 2));
                                    TemplateHandler.Assign(ref rowMP, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"], 2));

                                    TemplateHandler.Assign(ref rowMP, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                                    TemplateHandler.Assign(ref rowMP, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                    string choice = GenMPMailRow(drBet, CastAide.AsString(drBet["subtransid"]));
                                    TemplateHandler.Assign(ref rowMP, "choice", choice);

                                    body_mix.Append(rowMP);
                                }
                                TemplateHandler.Assign(ref tplMatch, "Rows_Mix", body_mix.ToString());

                                #endregion AssignRow

                                #region send mail

                                string ToMailCSD = "";
                                string CC = "", BCC = "";

                                ToMailCSD = dataforMailing.To;
                                CC = dataforMailing.Cc;
                                BCC = dataforMailing.Bcc;

                                Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);
                                site = regx.IsMatch(dataforMailing.Alias) ? dataforMailing.Alias.ToUpper() : site;

                                Subject = Subject.Replace("{site}", site);
                                Subject = Subject.Replace("{site_name}", dataforMailing.Name);

                                // send mail via gmail system
                                bool result = SendMailExtension.AccSendMail(MailSection.HostName.IBC, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                                string msg = "";
                                site = site.ToUpper() + " " + dataforMailing.Name;

                                if (result)
                                {
                                    msg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Success");
                                    ErrMsg += AlertMessage.GetMsg(string.Format(msg, site, ToMailCSD), AlertMessage.MsgType.Success);
                                }
                                else
                                {
                                    msg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_SendMail_Fail");
                                    ErrMsg += AlertMessage.GetMsg(string.Format(msg, site, ToMailCSD), AlertMessage.MsgType.Error);
                                }

                                #endregion send mail
                            }
                        }

                        #endregion send mail to CSD
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }

        public string SendEmailWhenDeleteBetToInternalGroup(int errcode, string errmsg, DataSet rs, bool IsRealCurrency, string reason)
        {
            string ErrMsg = "";
            try
            {
                ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Msg");
                ErrMsg = AlertMessage.GetMsg(ErrMsg, AlertMessage.MsgType.Success);

                if (rs != null && rs.Tables.Count >= 2)
                {
                    DataTable dtMatch = rs.Tables[0];
                    DataTable dtUsers = new DataTable();

                    dtUsers = rs.Tables[3];

                    string expression = "";

                    if (IsRealCurrency)
                    {
                        expression = "currency  <> 20";
                    }
                    else
                    {
                        expression = " currency  = 20";
                    }

                    DataRow[] rows = dtUsers.Select(expression);
                    int cnt = rows.Count();

                    string site = string.Empty;
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

                    string tplpath = "Mixpalay/";
                    string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplSendMailMixparlayFull.htm");
                    TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                    string tplUserMP = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplMPSendMailRow.htm");

                    //For special bets - 12EU
                    string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMail_SpecialBet.htm");
                    TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                    string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/DeleteBetReportSendMailAllRow_SpecialBet.htm");

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        #region send mail to CSD

                        #region Template

                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        if (CastAide.AsInt32(dtUsers.Rows[0]["choice_bettype"]) == 10)
                        {
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

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

                        #endregion Template

                        #region MatchInfo

                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;

                        Subject = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Subject");
                        Subject = string.Format(Subject, "{site}", "{site_name}", Convert.ToDateTime(drMatch["datetime"]));

                        if (IsRealCurrency)
                        {
                            Subject = "Real Currency - " + Subject;
                        }

                        TemplateHandler.Assign(ref Subject, "site_name", string.Empty);

                        string tplMatch = tplMatchAll.Clone().ToString();

                        //Match
                        if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
                            strHome = "Number Game No " + drMatch["matchcode"];

                        TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);
                        TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                        TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                        TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                        TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                        TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));
                        TemplateHandler.Assign(ref tplMatch, "reason", reason);

                        #endregion MatchInfo

                        #region AssignRow

                        StringBuilder body_mix = new StringBuilder();
                        foreach (DataRow drBet in rows)
                        {
                            string rowMP = tplUserMP.Clone().ToString();

                            TemplateHandler.Assign(ref rowMP, "username", drBet["username"]);
                            TemplateHandler.Assign(ref rowMP, "refno", drBet["transid"]);
                            TemplateHandler.Assign(ref rowMP, "transdate", drBet["transdate"]);

                            string status = CastAide.AsString(drBet["status"]);
                            string winloss = (status == "running") ? "" : NumberFormat.DecFormat(drBet["winlost"], 2);

                            TemplateHandler.Assign(ref rowMP, "winloss", winloss);
                            TemplateHandler.Assign(ref rowMP, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                            TemplateHandler.Assign(ref rowMP, "oddstype", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                            TemplateHandler.Assign(ref rowMP, "bettype", CacheRef.GetBettype(drBet["bettype"]));

                            TemplateHandler.Assign(ref rowMP, "status", status);
                            TemplateHandler.Assign(ref rowMP, "superpt", NumberFormat.DecFormat(drBet["spositiontaking"], 2));
                            TemplateHandler.Assign(ref rowMP, "masterpt", NumberFormat.DecFormat(drBet["mpositiontaking"], 2));
                            TemplateHandler.Assign(ref rowMP, "agentpt", NumberFormat.DecFormat(drBet["apositiontaking"], 2));

                            TemplateHandler.Assign(ref rowMP, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                            TemplateHandler.Assign(ref rowMP, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                            string choice = GenMPMailRow(drBet, CastAide.AsString(drBet["subtransid"]));
                            TemplateHandler.Assign(ref rowMP, "choice", choice);

                            body_mix.Append(rowMP);
                        }
                        TemplateHandler.Assign(ref tplMatch, "Rows_Mix", body_mix.ToString());

                        #endregion AssignRow

                        #region send mail

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

                        ToMailCSD = mailling.To;
                        CC = mailling.CC;
                        BCC = mailling.BCC;

                        Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);

                        Subject = Subject.Replace("{site}", site);

                        // send mail via gmail system
                        bool result = SendMailExtension.AccSendMail(MailSection.HostName.IBC, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                        string msg = "";
                        site = site.ToUpper();

                        if (result)
                        {
                            msg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_MPBet_Success");
                            ErrMsg += AlertMessage.GetMsg(string.Format(msg, site, ToMailCSD), AlertMessage.MsgType.Success);
                        }
                        else
                        {
                            msg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "specialDelete_SendMail_Fail");
                            ErrMsg += AlertMessage.GetMsg(string.Format(msg, site, ToMailCSD), AlertMessage.MsgType.Error);
                        }

                        #endregion send mail

                        #endregion send mail to CSD
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }

        /// <summary>
        /// GenMPMailRowsa
        /// </summary>
        /// <param name="row2"></param>
        /// <param name="subtransid"></param>
        /// <returns></returns>
        private string GenMPMailRow(DataRow row2, string subtransid)
        {
            string mpItem = string.Empty;
            try
            {
                mpItem = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/Mixpalay/TplMPSendMailChoice.htm");
                TemplateHandler.Assign(ref mpItem, "MoreBetInfo", "");
                string mpHome = CacheRef.GetTeamName(row2["homeid"]) + (CastAide.AsBoolean(row2["isNeutral"]) ? "(N)" : string.Empty);
                string mpAway = CacheRef.GetTeamName(row2["awayid"]);
                betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row2["betteam"].ToString(), CastAide.AsInt32(row2["choice_bettype"]), "e");
                sportypeName = CacheRef.GetSportName(row2["sporttype"]);
                leagueName = CacheRef.GetLeagueName(row2["leagueid"]);
                bettype = Convert.IsDBNull(row2["choice_bettype"]) ? 0 : Convert.ToInt32(row2["choice_bettype"]);
                hdp1 = Convert.IsDBNull(row2["choice_hdp1"]) ? 0 : Convert.ToDecimal(row2["choice_hdp1"]);
                hdp2 = Convert.IsDBNull(row2["choice_hdp2"]) ? 0 : Convert.ToDecimal(row2["choice_hdp2"]);
                odds = Convert.IsDBNull(row2["choice_odds"]) ? 0 : Convert.ToDecimal(row2["choice_odds"]);
                bettypeName = CacheRef.GetBettype(bettype);

                string subtid = row2["subtransid"].ToString();
                bool liveindicator = CastAide.AsBoolean(row2["liveindicator"], false);

                TemplateHandler.Assign(ref mpItem, "bgcolor", liveindicator ? "pink" : "#FFFFAA");

                if (subtid == subtransid)
                {
                    TemplateHandler.Assign(ref mpItem, "style_bettype", "font-weight:bold; color:Blue");
                }
                else
                {
                    TemplateHandler.Assign(ref mpItem, "style_bettype", "");
                }

                string formatlive = string.Empty;
                if (liveindicator)
                {
                    formatlive = string.Format("&nbsp;<b>[{0}-{1}]</b>", CastAide.AsInt32(row2["livehomescore"], 0), CastAide.AsInt32(row2["liveawayscore"], 0));
                }
                TemplateHandler.Assign(ref mpItem, "formatlive", formatlive);

                switch (bettype)
                {
                    case 1:
                    case 7:
                    case 20:
                    case 21:
                    case 22:
                    case 124:
                    case 125:
                        GenHdpEvent(ref mpItem, mpHome, mpAway, betteam, true);
                        break;

                    case 121:
                    case 122:
                    case 123:
                        GenHomeAwayDrawNoBet(ref mpItem, mpHome, mpAway, betteam, true);
                        break;

                    case 2:
                    case 12:
                        GenOEEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 3:
                    case 8:
                        GenOUEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 4:
                    case 14:
                    case 30:
                        GenCSEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 5:
                    case 15:
                        Gen1x2Event(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 6:
                    case 16:
                        GenTGEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 24:
                        GenDoubleChanceEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 25:
                        GenDrawNoBetEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 26:
                        GenBothOneNeitherEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 27:
                        GenToWinToNilEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 28:
                        GenThreeWayHandicap(ref mpItem, mpHome, mpAway, true);
                        break;

                    case 13:
                        GenCleanSheetEvent(ref mpItem, mpHome, mpAway, true);
                        break;

                    default:
                        mpItem = string.Empty;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return mpItem;
        }

        protected void GenDoubleChanceEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            string choice = betteam;
            if (betteam.ToLower().Equals("1x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("2x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("12"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP();
            string Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenDoubleChanceEvent(ref string eventItem, string home, string away, bool isMP)
        {
            string choice = betteam;
            if (betteam.ToLower().Equals("1x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("2x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("12"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP();

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenCleanSheetEvent(ref string eventItem, string home, string away, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("hy"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("hn"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");
            else if (betteam.ToLower().Equals("ay"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("an"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");

            if (isMP) choice += oddMP();

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenCleanSheetEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("hy"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("hn"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");
            else if (betteam.ToLower().Equals("ay"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("an"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");

            if (isMP) choice += oddMP();
            string Event = home + " -VS- " + away;

            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenDrawNoBetEvent(ref string eventItem, string home, string away, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) { choice += oddMP(); }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenDrawNoBetEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) { choice += oddMP(); }
            string Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenBothOneNeitherEvent(ref string eventItem, string home, string away, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("b"))
                choice = "Both";
            else if (betteam.ToLower().Equals("o"))
                choice = "One";
            else if (betteam.ToLower().Equals("n"))
                choice = "No Goal";

            if (isMP) { choice += oddMP(); }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenBothOneNeitherEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("b"))
                choice = "Both";
            else if (betteam.ToLower().Equals("o"))
                choice = "One";
            else if (betteam.ToLower().Equals("n"))
                choice = "No Goal";

            if (isMP) { choice += oddMP(); }
            string Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenToWinToNilEvent(ref string eventItem, string home, string away, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("h"))
            {
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            }
            else if (betteam.ToLower().Equals("a"))
            {
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            }
            if (isMP) { choice += oddMP(); }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenToWinToNilEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            var choice = string.Empty;
            if (betteam.ToLower().Equals("h"))
            {
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            }
            else if (betteam.ToLower().Equals("a"))
            {
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            }
            if (isMP) { choice += oddMP(); }
            string Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenThreeWayHandicap(ref string eventItem, string home, string away, bool isMP)
        {
            var choice = string.Empty;
            var hdpResult = 0;
            if (betteam.ToLower().Equals("1"))
            {
                hdpResult = (int)(hdp1 - hdp2 > 0 ? -(hdp1 - hdp2) : Math.Abs(hdp1 - hdp2));
                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (betteam.ToLower().Equals("2"))
            {
                hdpResult = (int)(hdp2 - hdp1 > 0 ? -(hdp2 - hdp1) : Math.Abs(hdp2 - hdp1));
                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (betteam.ToLower().Equals("x"))
            {
                choice = string.Format("Draw <span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>", (hdp1 - hdp2 > 0 ? "A" : "H"), (int)(Math.Abs(hdp1 - hdp2)));
            }
            if (isMP) { choice += oddMP(); }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenThreeWayHandicap(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            var choice = string.Empty;
            var hdpResult = 0;
            if (betteam.ToLower().Equals("1"))
            {
                hdpResult = (int)(hdp1 - hdp2 > 0 ? -(hdp1 - hdp2) : Math.Abs(hdp1 - hdp2));
                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (betteam.ToLower().Equals("2"))
            {
                hdpResult = (int)(hdp2 - hdp1 > 0 ? -(hdp2 - hdp1) : Math.Abs(hdp2 - hdp1));
                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (betteam.ToLower().Equals("x"))
            {
                /*choice = string.Format("Draw <span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>",
                    (hdp1 - hdp2 > 0 ? away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "") :
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "")),
                    (int)(Math.Abs(hdp1 - hdp2)));*/
                choice = string.Format("Draw <span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>", (hdp1 - hdp2 > 0 ? "A" : "H"), (int)(Math.Abs(hdp1 - hdp2)));
            }
            if (isMP) { choice += oddMP(); }
            string Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                Event = string.Format("<s>{0}</s>", Event);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
            }
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        public List<MixParlayDTO> FetchDataPager(List<DataRow> rows)
        {
            List<MixParlayDTO> listRow = new List<MixParlayDTO>();
            try
            {
                int index = 1;
                string curGroup = rows[0]["group"].ToString();
                foreach (DataRow dataRow in rows)
                {
                    MixParlayDTO mpItem = new MixParlayDTO();

                    mpItem.Type = CastAide.AsString(dataRow["group"]);
                    if (curGroup != mpItem.Type)
                    {
                        curGroup = mpItem.Type;
                        index = 1;
                    }
                    mpItem.Order = index;
                    index++;

                    mpItem.Account = CastAide.AsString(dataRow["username"]);
                    mpItem.Refno = CastAide.AsString(dataRow["refno"]);
                    mpItem.TransId = CastAide.AsString(dataRow["transid"]);
                    mpItem.SubTransId = CastAide.AsString(dataRow["subtransid"]);
                    mpItem.Odds = NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dataRow["odds"]), CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])), 3);
                    mpItem.OddType = CastAide.AsString(CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"]))));
                    mpItem.Amount = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["stake"]), 2);
                    mpItem.Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
                    mpItem.TransTime = CastAide.AsDateTime(dataRow["transdate"]).ToString();
                    mpItem.Status = CastAide.AsString(dataRow["status"]);
                    mpItem.WinLoss = mpItem.Status == "running" ? "" : NumberFormat.DecFormat(dataRow["winlost"], 2);
                    mpItem.SPT = NumberFormat.DecFormat(dataRow["spositiontaking"], 2);
                    mpItem.MPT = NumberFormat.DecFormat(dataRow["mpositiontaking"], 2);
                    mpItem.APT = NumberFormat.DecFormat(dataRow["apositiontaking"], 2);
                    mpItem.Bettype = CacheRef.GetBettype(dataRow["bettype"]);
                    mpItem.IsDelete = "1";
                    mpItem.Liveindicator = CastAide.AsBoolean(dataRow["choice_liveindicator"], false);

                    // format choice
                    mpItem.Choice = GenMPChoiceItem(mpTplItem, dataRow, "0");
                    listRow.Add(mpItem);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listRow;
        }
    }
}