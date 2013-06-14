/// <summary>
/// Summary description for SpecialDeleteBase
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete.LiveCasino;
using Sunplus.Accounting.DataAccess.SpecialDelete;

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public class SpecialDeleteBase
    {

        #region SpecialBettype enum

        public enum SpecialBettype
        {
            BetNormal = 0,
            BetDeposit = 1,
            Betdaq = 2,
            Bet12EU = 3,
            BetMasterMHC = 4
        }

        #endregion
        #region GetSpecialBettype
        public string GetSpecialBettype(SpecialBettype type)
        {
            string str = "";
            switch (type)
            {
                case SpecialBettype.BetNormal:
                    str = "BetNormal";
                    break;
                case SpecialBettype.BetDeposit:
                    str = "Deposit";
                    break;
                case SpecialBettype.Betdaq:
                    str = "BD";
                    break;
                case SpecialBettype.Bet12EU:
                    str = "12EU";
                    break;
                case SpecialBettype.BetMasterMHC:
                    str = "MasterMHC";
                    break;
            }
            return str;
        }
        #endregion

        #region AssignData
        protected string getBettypeName(string bettypeId, bool liveindicator)
        {
            string name = "";
            return name = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(CastAide.AsInt32(bettypeId));
        }

        protected bool IsNumberGame(string bettypeId)
        {
            if (CastAide.AsInt32(bettypeId) >= 81 && CastAide.AsInt32(bettypeId) <= 87)
                return true;
            else
                return false;
        }

        protected void AssignData(DataTable dataTable, string type, ref List<SpecialDeleteBO> list)
        {
            int index = 1;
            string betteam = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                SpecialDeleteBO specialDeleteBo = new SpecialDeleteBO();
                bool liveindicator = false;
                if (dataTable.Columns.Contains("liveindicator"))
                    liveindicator = CastAide.AsBoolean(dataRow["liveindicator"]);
                string name = getBettypeName(dataRow["bettype"].ToString(), liveindicator);
                string Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dataRow["betteam"].ToString(), CastAide.AsInt32(dataRow["bettype"]), "e");
                if (CastAide.AsInt32(dataRow["bettype"]) == 90)
                {
                    name += liveindicator ? " (Live Game)" : " (Pre Game)";
                    Betteam = Fanex.Tarus.BetList.Helper.ChoiceHelper.GetNumberWheelChoice(CastAide.AsInt32(dataRow["hdp1"]), CastAide.AsInt32(Betteam));
                }
                specialDeleteBo.Type = type;
                specialDeleteBo.IsMP = "1";
                specialDeleteBo.Order = index;
                specialDeleteBo.Account = CastAide.AsString(dataRow["username"]);
                specialDeleteBo.Refno = CastAide.AsString(dataRow["transid"]);
                specialDeleteBo.Bettype = name;// Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBettype(CastAide.AsInt32(dataRow["bettype"]));
                specialDeleteBo.Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dataRow["betteam"].ToString(), CastAide.AsInt32(dataRow["bettype"]), "e");
                specialDeleteBo.Odds = NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dataRow["odds"]), CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])), 3);
                specialDeleteBo.OddType = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])));
                specialDeleteBo.Amount = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["stake"]), 2);
                specialDeleteBo.Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
                switch (CastAide.AsInt32(dataRow["bettype"]))
                {
                    case 1:                    
                    case 7:                    
                    case 22:
                        betteam = CastAide.AsString(dataRow["betteam"]).ToLower();
                        if (betteam == "h")
                        {
                            specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp2"]) - CastAide.AsDecimal(dataRow["hdp1"]), 2);
                        }
                        else 
                        {
                            specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp1"]) - CastAide.AsDecimal(dataRow["hdp2"]), 2);
                        }                        
                        break;
                    case 3:
                    case 8:
                        specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp1"]), 2);
                        break;
                    default:
                        specialDeleteBo.Hdp = "";
                        break;
                }

                specialDeleteBo.TransTime = CastAide.AsDateTime(dataRow["transdate"]).ToString();
                specialDeleteBo.Score = CastAide.AsString(dataRow["livehomescore"]) + " - " + CastAide.AsString(dataRow["liveawayscore"]);
                specialDeleteBo.Creator = CastAide.AsString(dataRow["creator"]);
                specialDeleteBo.Status = CastAide.AsString(dataRow["status"]);
                specialDeleteBo.SaFilter = DataUtils.IsExistKey(dataRow, "secacceptflag") ? CastAide.AsInt32(dataRow["secacceptflag"]) : 0;

                specialDeleteBo.IsDelete = "0";
                Regex reg = new Regex("running|refund", RegexOptions.IgnoreCase);
                if (AppSession.Username.ToLower() == "joe" || AppSession.Username.ToLower() == "steven")
                    specialDeleteBo.IsDelete = "1";
                else if (reg.IsMatch(specialDeleteBo.Status))
                    specialDeleteBo.IsDelete = "1";

                list.Add(specialDeleteBo);

                index++;
            }

        }

        protected void AssignDataEarlyKO(IEnumerable<DataRow> Rows, string type, ref List<SpecialDeleteBetEarlyKO> list, string isMP, ref int mergeIndex)
        {
            string betteam = string.Empty;
            foreach (DataRow dataRow in Rows)
            {
                SpecialDeleteBetEarlyKO specialDeleteBo = new SpecialDeleteBetEarlyKO();
                bool liveindicator = false;
                if (dataRow.Table.Columns.Contains("liveindicator"))
                    liveindicator = CastAide.AsBoolean(dataRow["liveindicator"]);
                string name = getBettypeName(dataRow["bettype"].ToString(), liveindicator);

                specialDeleteBo.Type = type;
                specialDeleteBo.IsMP = dataRow["ismp"].ToString();
                specialDeleteBo.Order = mergeIndex;
                specialDeleteBo.Account = CastAide.AsString(dataRow["username"]);
                specialDeleteBo.Refno = CastAide.AsString(dataRow["refno"]);
                specialDeleteBo.TransId = CastAide.AsString(dataRow["transid"]);
                specialDeleteBo.SubTransId = CastAide.AsString(dataRow["subtransid"]);
                specialDeleteBo.Bettype = name;
                specialDeleteBo.Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dataRow["betteam"].ToString(), CastAide.AsInt32(dataRow["bettype"]), "e");
                if (specialDeleteBo.IsMP == "1")
                {
                    specialDeleteBo.Odds = NumberFormat.DecFormat(dataRow["odds"], 3);
                    specialDeleteBo.OddType = "";
                }
                else
                {
                    specialDeleteBo.Odds = NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dataRow["odds"]), CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])), 3);
                    specialDeleteBo.OddType = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(dataRow["bettype"]), CastAide.AsInt32(dataRow["oddstype"])));
                }

                specialDeleteBo.Amount = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["stake"]), 2);
                specialDeleteBo.CurrencyId = CastAide.AsString(dataRow["currency"], string.Empty);
                specialDeleteBo.Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
                switch (CastAide.AsInt32(dataRow["bettype"]))
                {
                    case 1:
                    case 7:
                    case 22:                        
                        betteam = CastAide.AsString(dataRow["betteam"]).ToLower();
                        if (betteam == "h")
                        {
                            specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp2"]) - CastAide.AsDecimal(dataRow["hdp1"]), 2);
                        }
                        else 
                        {
                            specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp1"]) - CastAide.AsDecimal(dataRow["hdp2"]), 2);
                        }
                        break;
                    case 3:
                    case 8:
                        specialDeleteBo.Hdp = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["hdp1"]),2);
                        break;
                    default:
                        specialDeleteBo.Hdp = "";
                        break;
                }
                specialDeleteBo.ParlayType = CastAide.AsBoolean(dataRow["ismp"]) ? CastAide.AsInt32(dataRow["ParlayType"]) : 0;

                specialDeleteBo.TransTime = CastAide.AsDateTime(dataRow["transdate"]).ToString();
                specialDeleteBo.Score = CastAide.AsString(dataRow["livehomescore"]) + " - " + CastAide.AsString(dataRow["liveawayscore"]);
                specialDeleteBo.Creator = CastAide.AsString(dataRow["creator"]);
                specialDeleteBo.Status = CastAide.AsString(dataRow["status"]);
                specialDeleteBo.SaFilter = CastAide.AsInt32(dataRow["secacceptflag"]);
                specialDeleteBo.IsDelete = "0";
                Regex reg = new Regex("running|refund", RegexOptions.IgnoreCase);
                if (AppSession.Username.ToLower() == "joe" || AppSession.Username.ToLower() == "steven")
                    specialDeleteBo.IsDelete = "1";
                else if (reg.IsMatch(specialDeleteBo.Status))
                    specialDeleteBo.IsDelete = "1";

                list.Add(specialDeleteBo);

                mergeIndex++;
            }

        }
        #endregion

        #region SendPrivateMsgToUsers
        protected bool SendPrivateMsgToUsers(DataSet rs, int matchId, Message pmsg)
        {
            bool result = false;
            try
            {
                #region send private message to users
                if (rs.Tables.Count > 0)
                {
                    DataTable dtMsg = null;
                    if (rs.Tables.Count >= 3)
                        dtMsg = rs.Tables[2];
                    else
                        dtMsg = rs.Tables[0];

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
                                MessageId = Messenger.SetMessage(pmsg, custid, uname, CastAide.AsInt32(matchId), bettransid);
                            else
                                Messenger.AddTargt2Message(MessageId, custid, uname, bettransid);
                        }
                    }
                }
                #endregion

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        protected bool SendPrivateMsgEarlyKO(DataSet rs, int matchId, Message pmsg)
        {
            bool result = false;
            try
            {
                #region send private message to users

                int countDB = rs.Tables.Count;
                if (countDB > 0)
                {
                    DataTable dtMsg = null;
                    if (countDB >= 3)
                        dtMsg = rs.Tables[2];
                    else
                        dtMsg = rs.Tables[0];

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
                                MessageId = Messenger.SetMessage(pmsg, custid, uname, CastAide.AsInt32(matchId), bettransid);
                            else
                                Messenger.AddTargt2Message(MessageId, custid, uname, bettransid);
                        }
                    }
                }
                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region SendEmailToCSD_Old
        protected string SendEmailToCSD_Old(int errcode, string errmsg, DbResult rs, bool IsNumberGame, bool IsRecoverBets, string transid)
        {
            string ErrMsg = "";
            try
            {

                if (errcode == 0)
                {
                    if (IsRecoverBets)
                    {
                        ErrMsg = AlertMessage.GetMsg("Transaction " + transid + " was recovered.", AlertMessage.MsgType.Success);
                    }
                    else
                    {
                        ErrMsg = AlertMessage.GetMsg("Bet transactions deleted!", AlertMessage.MsgType.Success);
                    }

                    if (rs.ResultSet.Tables.Count >= 2)
                    {
                        DataTable dtMatch = rs.ResultSet.Tables[0];
                        DataTable dtUser = rs.ResultSet.Tables[1];

                        string tplpath = "";
                        if (IsNumberGame)
                            tplpath = "NumberGame/";
                        string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");
                        if (IsRecoverBets)
                            TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Recovered");
                        else
                            TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                        string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");

                        //For special bets - 12EU
                        string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");
                        if (IsNumberGame)
                            TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Recovered");
                        else
                            TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                        string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");

                        if (IsRecoverBets)
                        {
                            tplUser_SpecialBet = tplUser_SpecialBet.Replace("Bets", "Bet");
                            tplMatchAll_SpecialBet = tplMatchAll_SpecialBet.Replace("Bets", "Bet");
                            tplMatchAll = tplMatchAll.Replace("Bets", "Bet");
                            tplUser = tplUser.Replace("Bets", "Bet");
                        }



                        #region send mail

                        if (dtMatch.Rows.Count > 0 && dtUser.Rows.Count > 0)
                        {
                            #region send mail to CSD
                            string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                            if (CastAide.AsInt32(dtUser.Rows[0]["bettype"]) == 10)
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

                            DataTable dtTLCBet = dtUser.Clone();
                            dtTLCBet.TableName = "TLC";

                            DataTable dtDelaBet = dtUser.Clone();
                            dtDelaBet.TableName = "Dela";

                            DataTable dtSpondemoBet = dtUser.Clone();
                            dtSpondemoBet.TableName = "Spondemo";

                            DataTable dtZzun88 = dtUser.Clone();
                            dtZzun88.TableName = "Zzun88";

                            DataTable dtMayfair = dtUser.Clone();
                            dtMayfair.TableName = "Mayfair";

                            DataTable dtSuncity = dtUser.Clone();
                            dtSuncity.TableName = "Suncity";

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
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("tlc")) // for tlc site
                                    {
                                        DataRow drTLCBet = dtTLCBet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drTLCBet[i] = drAll[i];
                                        }
                                        dtTLCBet.Rows.Add(drTLCBet);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("dela")) // for Dela site
                                    {
                                        DataRow drDelaBet = dtDelaBet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drDelaBet[i] = drAll[i];
                                        }
                                        dtDelaBet.Rows.Add(drDelaBet);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("spondemo")) // for Spondemo site
                                    {
                                        DataRow drSpondemoBet = dtSpondemoBet.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drSpondemoBet[i] = drAll[i];
                                        }
                                        dtSpondemoBet.Rows.Add(drSpondemoBet);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("zzun88")) // for Zzun88 site
                                    {
                                        DataRow drZzun88 = dtZzun88.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drZzun88[i] = drAll[i];
                                        }
                                        dtZzun88.Rows.Add(drZzun88);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("mayfair")) // for Mayfair site
                                    {
                                        DataRow drMayfair = dtMayfair.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drMayfair[i] = drAll[i];
                                        }
                                        dtMayfair.Rows.Add(drMayfair);
                                    }
                                    else if (DNAide.Core.CastAide.AsString(drAll["site"], "").ToLower().Equals("suncity")) // for Suncity  site
                                    {
                                        DataRow drSuncity = dtSuncity.NewRow();
                                        for (int i = 0; i < drAll.ItemArray.Length; i++)
                                        {
                                            drSuncity[i] = drAll[i];
                                        }
                                        dtSuncity.Rows.Add(drSuncity);
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
                            dsUser.Tables.Add(dtTLCBet);
                            dsUser.Tables.Add(dtDelaBet);
                            dsUser.Tables.Add(dtSpondemoBet);
                            dsUser.Tables.Add(dtZzun88);
                            dsUser.Tables.Add(dtMayfair);
                            dsUser.Tables.Add(dtSuncity);
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
                                    bool IsTLCBet = false;
                                    bool IsDelaBet = false;
                                    bool IsSpondemoBet = false;
                                    bool IsZzun88 = false;
                                    bool IsMayfair = false;
                                    bool IsSuncity = false;
                                    string site = string.Empty;
                                    string siteName = string.Empty;
                                    string Subject = string.Empty;

                                    if (IsRecoverBets)
                                    {
                                        Subject = "Recovered Bet Report for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                                    }
                                    else
                                    {
                                        Subject = "Deleted Bets Report for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                                    }

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
                                    else if (dtBet.TableName == "TLC")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "TLC Site";
                                    }
                                    else if (dtBet.TableName == "Dela")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Dela Site";
                                    }
                                    else if (dtBet.TableName == "Spondemo")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Spondemo Site";
                                    }
                                    else if (dtBet.TableName == "Zzun88")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Zzun88 Site";
                                    }
                                    else if (dtBet.TableName == "Mayfair")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Mayfair Site";
                                    }
                                    else if (dtBet.TableName == "Suncity")
                                    {
                                        site = string.Empty;
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Suncity Site";
                                    }
                                    else
                                    {
                                        tplMatch = tplMatchAll.Clone().ToString();
                                        siteName = "Deposit Site";
                                        if (IsRecoverBets)
                                            siteName = "Site";
                                    }

                                    if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
                                        strHome = "Number Game No " + drMatch["matchcode"];

                                    TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);
                                    TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                                    TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                                    TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                                    TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
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
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("tlc"))
                                                IsTLCBet = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("dela"))
                                                IsDelaBet = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("spondemo"))
                                                IsSpondemoBet = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("zzun88"))
                                                IsZzun88 = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("mayfair"))
                                                IsMayfair = true;
                                            else if (DNAide.Core.CastAide.AsString(drBet["site"], "").ToLower().Equals("suncity"))
                                                IsSuncity = true;
                                            TemplateHandler.Assign(ref row, "username1", drBet["username2"]);
                                        }

                                        TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                        TemplateHandler.Assign(ref row, "username", drBet["username"]);



                                        TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                                        bool IsLive = false;
                                        if (IsNumberGame)
                                            IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                        TemplateHandler.Assign(ref row, "bettype", getBettypeName(drBet["bettype"].ToString(), IsLive));//CacheRef.GetBettype(drBet["bettype"]));//drBet["typenamee"]);
                                        TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                        TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                        TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                        TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["StakePositiontaking"], 2));
                                        TemplateHandler.Assign(ref row, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                        switch (CastAide.AsInt32(drBet["bettype"]))
                                        {
                                            case 1:
                                            case 7:
                                            case 22:
                                                Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                                Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                                if (drBet["betteam"].ToString() == "h")
                                                    TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                                                else
                                                    TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp1 - hdp2));
                                                break;
                                            case 3:
                                            case 8:
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(CastAide.AsDecimal(drBet["hdp1"])));
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

                                    if (IsRecoverBets)
                                    {
                                        ToMailCSD = MailSection.ToMailRecoverBetCSD;
                                        CC = MailSection.CCMailRecoverBetCSD;
                                        BCC = MailSection.BCCMailRecoverBetCSD;
                                    }
                                    else
                                    {
                                        #region delete bet
                                        if (Is12EU)
                                        {
                                            ToMailCSD = MailSection.ToMail12EUCSD;
                                            CC = MailSection.CCMail12EUCSD;
                                            BCC = MailSection.BCCMail12EUCSD;
                                        }
                                        else if (Is12Bet)
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
                                        else if (IsTLCBet)
                                        {
                                            ToMailCSD = MailSection.ToMailTLCCSD;
                                            CC = MailSection.CCMailTLCCSD;
                                            BCC = MailSection.BCCMailTLCCSD;
                                            site = "";
                                        }
                                        else if (IsDelaBet)
                                        {
                                            ToMailCSD = MailSection.ToMailDelaCSD;
                                            CC = MailSection.CCMailDelaCSD;
                                            BCC = MailSection.BCCMailDelaCSD;
                                            //site = "";
                                        }
                                        else if (IsSpondemoBet)
                                        {
                                            ToMailCSD = MailSection.ToMailSpondemoCSD;
                                            CC = MailSection.CCMailSpondemoCSD;
                                            BCC = MailSection.BCCMailSpondemoCSD;
                                            site = "";
                                        }
                                        else if (IsZzun88)
                                        {
                                            ToMailCSD = MailSection.ToMailZzun88CSD;
                                            CC = MailSection.CCMailZzun88CSD;
                                            BCC = MailSection.BCCMailZzun88CSD;
                                            //site = "";
                                            siteName = "";
                                        }
                                        else if (IsMayfair)
                                        {
                                            ToMailCSD = MailSection.ToMailMayfairCSD;
                                            CC = MailSection.CCMailMayfairCSD;
                                            BCC = MailSection.BCCMailMayfairCSD;
                                            //site = "";
                                            siteName = "";
                                        }
                                        else if (IsSuncity)
                                        {
                                            ToMailCSD = MailSection.ToMailSuncityCSD;
                                            CC = MailSection.CCMailSuncityCSD;
                                            BCC = MailSection.BCCMailSuncityCSD;
                                            // site = "";
                                            siteName = "";
                                        }
                                        else
                                        {
                                            ToMailCSD = MailSection.ToMailAsiaBetCSD;
                                            CC = MailSection.CCMailAsiaBetCSD;
                                            BCC = MailSection.BCCMailAsiaBetCSD;
                                        }
                                        #endregion
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

                                        WriteLogMail(LogMail.ToString());
                                        if (MailSection.CCMail.Length > 0)
                                        {
                                            if (CC.Length > 0)
                                                CC += ";" + MailSection.CCMail;
                                            else
                                                CC = MailSection.CCMail;
                                            if (IsRecoverBets)
                                                CC = CC.Replace(";blusmurf@gmail.com", string.Empty);
                                        }
                                        if (MailSection.BCCMail.Length > 0)
                                        {
                                            if (BCC.Length > 0)
                                                BCC += ";" + MailSection.BCCMail;
                                            else
                                                BCC = MailSection.BCCMail;
                                        }
                                        result = DNAide.Utils.SendMailService.SendMail2(MailSection.MailServer, ToMailCSD, MailSection.FromMail, CC, BCC, Subject, tplMatch, MailSection.MailPwd);

                                       

                                    }
                                    catch (Exception ex)
                                    {

                                        {
                                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                                        }
                                        ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
                                    }

                                    string msg = "";
                                    // site = "";

                                    if (Is12EU) site = "12EU";
                                    else if (Is12Bet) site = "12Bet";
                                    else if (IsMansionBet) site = "Mansion88";
                                    else if (IsAlogBet) site = "ALog";
                                    else if (IsTLCBet) site = "TLC";
                                    else if (IsDelaBet) site = "Dela";
                                    else if (IsSpondemoBet) site = "Spondemo";
                                    else if (IsZzun88) site = "Zzun88";
                                    else if (IsMayfair) site = "Mayfair";
                                    else if (IsSuncity) site = "Suncity";
                                    // else site = "AsiaBet88";

                                    if (result)
                                    {
                                        if (IsRecoverBets)
                                        {
                                            msg = "Details of recoverd " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                        }
                                        else
                                        {
                                            msg = "Details of deleted " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                        }

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

            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return ErrMsg;
        }
        #endregion



        #region GetSitesAndUsers
        protected string GetSitesAndUsers()
        {
            var collections = SpecialDeleteSection.Instance.DeleteBet;
            string expression = string.Empty;
            string sites = string.Empty;
            string users = string.Empty;
            foreach (SpecialDeleteElement element in collections)
            {
                foreach (SpecialDeleteElement store in element.Stores)
                {
                    if (store.Name.ToLower().Equals("site"))
                        sites += "'" + store.Alias + "',";
                    if (store.Name.ToLower().Equals("username"))
                        users += "'" + store.Alias + "',";
                }
            }


            if (sites.Length > 1)
                sites = sites.Substring(0, sites.Length - 1);
            if (users.Length > 1)
                users = users.Substring(0, users.Length - 1);

            expression = " site not in ( " + sites + " ) ";
            expression += " and username not in ( " + users + " ) ";

            return expression;
        }
        #endregion

        protected string SendEmailToCSD(int errcode, string errmsg, DataSet rs, bool IsNumberGame, string reason)
        {
            string ErrMsg = "";
            try
            {

                if (errcode == 0)
                {
                    ErrMsg = AlertMessage.GetMsg("Bet transactions deleted!", AlertMessage.MsgType.Success);

                    if (rs.Tables.Count >= 2)
                    {
                        DataTable dtMatch = rs.Tables[0];
                        DataTable dtUsers = rs.Tables[1];

                        string tplpath = "";
                        if (IsNumberGame)
                            tplpath = "NumberGame/";
                        string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");

                        TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                        string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");

                        ////For special bets - 12EU
                        //string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");

                        //TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                        //string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");



                        #region send mail

                        if (dtMatch.Rows.Count > 0 && dtUsers.Rows.Count > 0)
                        {
                            #region send mail to CSD

                            #region template
                            string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                            if (CastAide.AsInt32(dtUsers.Rows[0]["bettype"]) == 10)
                            {
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                                TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser, "score");
                                TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                                ////For 12EU
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

                                strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " + CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);

                            }
                            #endregion

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

                                        var _rows = dtUsers.AsEnumerable().Where(s=>s["site"].ToString().ToLower() == dr["site"].ToString().ToLower());

                                        if(_rows.Count() == 0)
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

                                    #endregion
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
                                            Alias = store.Alias,
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
                                    #endregion
                                }
                            }

                            #endregion

                            string betteam = string.Empty;
                            foreach (SpecialDeleteDataforMailing dataforMailing in maillings.Items)
                            {
                                if (dataforMailing.Items.Rows.Count > 0)
                                {
                                    #region MatchInfo
                                    DataRow drMatch = dtMatch.Rows[0];
                                    string Subject = string.Empty;

                                    Subject = "Deleted Bets Report for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";

                                    string tplMatch = tplMatchAll.Clone().ToString();

                                    //Match
                                    if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
                                        strHome = "Number Game No " + drMatch["matchcode"];

                                    TemplateHandler.Assign(ref tplMatch, "Code", drMatch["matchcode"]);
                                    TemplateHandler.Assign(ref tplMatch, "Home", strHome);
                                    TemplateHandler.Assign(ref tplMatch, "Away", CacheRef.GetTeamName(drMatch["awayid"]));
                                    TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                                    TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                                    TemplateHandler.Assign(ref tplMatch, "reason", reason);
                                    TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));

                                    string site = string.Empty;
                                    string bgColor = string.Empty;
                                    string isMP = string.Empty;
                                    #endregion

                                    #region AssignRow
                                    StringBuilder body = new StringBuilder();
                                    StringBuilder body_mix = new StringBuilder();
                                    foreach (DataRow drBet in dataforMailing.Items.Rows)
                                    {

                                        site = drBet["site"].ToString();
                                        bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                        string row = tplUser.Clone().ToString();

                                        TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                        TemplateHandler.Assign(ref row, "username", drBet["username"]);
                                        TemplateHandler.Assign(ref row, "username1", drBet["username2"]);



                                        TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                                        bool IsLive = false;
                                        if (IsNumberGame)
                                            IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                        TemplateHandler.Assign(ref row, "bettype", getBettypeName(drBet["bettype"].ToString(), IsLive));
                                        TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                        TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                        TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                        TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["stake"], 2));
                                        TemplateHandler.Assign(ref row, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                        switch (CastAide.AsInt32(drBet["bettype"]))
                                        {
                                            case 1:
                                            case 7:
                                            case 22:
                                                betteam = CastAide.AsString(drBet["betteam"]).ToLower();
                                                Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                                Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                                Decimal hdp = 0;
                                                if (betteam == "h")
                                                    hdp = hdp2 - hdp1;
                                                else
                                                    hdp = hdp1 - hdp2;
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp));
                                                break;
                                            case 3:
                                            case 8:
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(drBet["hdp1"]));
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
                                    #endregion

                                    TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());

                                    string ToMailCSD = "";
                                    string CC = "", BCC = "";

                                    string _ToMailCSD = "";

                                    bool isSendBoth = false;

                                    ToMailCSD = dataforMailing.To;
                                    CC = dataforMailing.Cc;
                                    BCC = dataforMailing.Bcc;

                                    Regex regx = new Regex("12eu", RegexOptions.IgnoreCase);
                                    site = regx.IsMatch(dataforMailing.Alias) ? dataforMailing.Alias.ToUpper() : site;

                                    Subject = Subject.Replace("{site}", site);
                                    Subject = Subject.Replace("{site_name}", dataforMailing.Name);


                                    // send mail via gmail system
                                    bool result = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                                    string msg = "";
                                    site = site.ToUpper() + " " + dataforMailing.Name;

                                    if (result)
                                    {

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

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }

        protected bool SendMailWhenDeleteBetToDBA(string ErrMsg, string ErrCode, string MatchID, string FromRefNo, string ToRefNo)
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
                var isSuccess = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, Body, null);
                return isSuccess;
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
                return false;
            }
        }

        protected bool SendMailWhenRecoverBetToDBA(string ErrMsg, string ErrCode, string TransID)
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
                var isSuccess = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, Body, null);
                return isSuccess;
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
                return false;
            }
        }

        protected string SendEmailWhenDeleteBetToInternalGroup(int errcode, string errmsg, DataSet rs, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string reason)
        {
            string ErrMsg = "";
            try
            {

                if (errcode == 0)
                {
                    ErrMsg = AlertMessage.GetMsg("Bet transactions deleted!", AlertMessage.MsgType.Success);

                    if (rs.Tables.Count >= 4)
                    {
                        DataTable dtMatch = rs.Tables[0];
                        DataTable dtUsers = rs.Tables[3];

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

                        ////For special bets - 12EU
                        //string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");

                        //TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                        //string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");
                        #endregion

                        #region send mail

                        if (dtMatch.Rows.Count > 0 && cnt > 0)
                        {
                            #region Template
                            string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                            if (CastAide.AsInt32(dtUsers.Rows[0]["bettype"]) == 10)
                            {
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                                TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser, "score");
                                TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                                ////For 12EU
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

                                strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " + CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);

                            }
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

                            if (IsRealCurrency)
                            {
                                Subject = "Real Currency - Deleted Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                            }
                            else
                            {
                                Subject = "Deleted Bets Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                            }


                            string tplMatch = tplMatchAll.Clone().ToString();

                            //Match
                            if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
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
                            TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
                            TemplateHandler.Assign(ref tplMatch, "UserName", AppSession.Username);
                            TemplateHandler.Assign(ref tplMatch, "reason", reason);
                            TemplateHandler.Assign(ref tplMatch, "time", Convert.ToDateTime(drMatch["datetime"]));

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
                            foreach (DataRow drBet in drTicketsDeleted)
                            {

                                bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                string row = tplUser.Clone().ToString();

                                TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                TemplateHandler.Assign(ref row, "username", drBet["username"]);
                                TemplateHandler.Assign(ref row, "username1", drBet["username2"]);



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
                                    if (rs.Tables.Count >= 4)
                                    {
                                        var _transid = CastAide.AsDecimal(drBet["refno"]);
                                        choice = BuildChoiceColumnForLiveCasino(_transid, rs.Tables[4]);
                                    }
                                    TemplateHandler.Assign(ref row, "bet", choice);
                                }
                                else if (IsNumberGame && CastAide.AsInt32(drBet["bettype"]) == 90)
                                {
                                    //string choice = "Group "+ NumberFormat.DecFormat(drBet["hdp1"], 0) + "-" + Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e");
                                    string choice = Fanex.Tarus.BetList.Helper.ChoiceHelper.GetNumberWheelChoice(CastAide.AsInt32(drBet["hdp1"]), CastAide.AsInt32(Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e")));
                                    TemplateHandler.Assign(ref row, "bet", choice);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                }

                                TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["stake"], 2));
                                TemplateHandler.Assign(ref row, "currency", GetCurrencyName(CastAide.AsInt32(drBet["currency"])));

                                switch (CastAide.AsInt32(drBet["bettype"]))
                                {
                                    case 1:
                                    case 7:
                                    case 22:                                     

                                        betteam = CastAide.AsString(drBet["betteam"]).ToLower();
                                        Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                        Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                        Decimal hdp = 0;
                                        if (betteam == "h")
                                            hdp = hdp2 - hdp1;
                                        else
                                            hdp = hdp1 - hdp2;
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp));
                                        break;
                                    case 3:
                                    case 8:
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(drBet["hdp1"]));
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

                                if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "stake", drBet["stake"]);
                                    TemplateHandler.Assign(ref row, "status", UppercaseFirst(CastAide.AsString(drBet["status"])));
                                    TemplateHandler.Assign(ref row, "superpt", drBet["spositiontaking"]);
                                    TemplateHandler.Assign(ref row, "masterpt", drBet["mpositiontaking"]);
                                    TemplateHandler.Assign(ref row, "agentpt", drBet["apositiontaking"]);
                                }

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
                            bool result = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                            #endregion
                        }
                        #endregion


                    }
                }
                else
                {
                    ErrMsg = AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }

        protected string SendEmailWhenRecoverBet(int errcode, string errmsg, DataSet rs, bool IsNumberGame, bool IsLiveCasino, bool IsRealCurrency, string transid)
        {
            string ErrMsg = "";
            try
            {

                if (errcode == 0)
                {
                    ErrMsg = AlertMessage.GetMsg("Transaction " + transid + " was recovered.", AlertMessage.MsgType.Success);

                    if (rs != null && rs.Tables.Count >= 2)
                    {
                        DataTable dtMatch = rs.Tables[0];
                        DataTable dtUsers = rs.Tables[1];

                        string tplpath = "";
                        if (IsNumberGame)
                            tplpath = "NumberGame/";
                        if (IsLiveCasino)
                            tplpath = "LiveCasino/";
                        string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail.htm");
                        TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Recovered");
                        TemplateHandler.RemoveBlock(ref tplMatchAll, "reason");

                        string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");

                        //For special bets - 12EU
                        string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");
                        TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Recovered");
                        TemplateHandler.RemoveBlock(ref tplMatchAll, "reason");

                        string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");

                        tplUser_SpecialBet = tplUser_SpecialBet.Replace("Bets", "Bet");
                        tplMatchAll_SpecialBet = tplMatchAll_SpecialBet.Replace("Bets", "Bet");
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
                            if (bettype == 10 || (bettype >= 81 && bettype <= 90))
                            {
                                //TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                                //TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser, "score");
                                TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                                //For 12EU
                                //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                                TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                                //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                                TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                                TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

                                if (bettype == 10) // outright
                                {
                                    strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " + CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);
                                }
                                else//Number game
                                {
                                    strHome = CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]) + " No " + CastAide.AsString(dtMatch.Rows[0]["matchcode"]);
                                }

                            }
                            #endregion

                            #region Create Dataset dsUser

                            #region MatchInfo
                            DataRow drMatch = dtMatch.Rows[0];
                            string Subject = string.Empty;


                            Subject = "Recovered Bet Report for {site} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                            if (IsRealCurrency)
                                Subject = "Real Currency - " + Subject;


                            string tplMatch = tplMatchAll.Clone().ToString();

                            //Match
                            if (strHome.Length == 0 && CacheRef.GetTeamName(drMatch["awayid"]).Length == 0)
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

                            TemplateHandler.Assign(ref tplMatch, "EventDate", FormatHandler.FormatDateEn(drMatch["EventDate"]));
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
                            foreach (DataRow drBet in drTicketsRecovered)
                            {

                                site = drBet["site"].ToString();
                                bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                string row = tplUser.Clone().ToString();

                                TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                TemplateHandler.Assign(ref row, "username", drBet["username"]);
                                TemplateHandler.Assign(ref row, "username1", drBet["username2"]);

                                if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "refno", drBet["transid"]);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref row, "refno", drBet["refno"]);
                                }
                                string bettypeName = CacheRef.GetBettype(drBet["bettype"]);
                               
                                if (IsLiveCasino)
                                {
                                    var refNo = CastAide.AsDecimal(drBet["refno"]);
                                    var choice = string.Empty;
                                    if (rs.Tables.Count >= 3)
                                        choice = BuildChoiceColumnForLiveCasino(refNo, rs.Tables[2]);
                                    TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                    TemplateHandler.Assign(ref row, "bet", choice);
                                }
                                else if (CastAide.AsInt32(drBet["bettype"]) == 90)
                                {
                                    bool IsLive = false;
                                    IsLive = CastAide.AsBoolean(drBet["liveindicator"]);
                                    bettypeName += IsLive ? " (Live Game)" : " (Pre Game)";
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.BetList.Helper.ChoiceHelper.GetNumberWheelChoice(CastAide.AsInt32(drBet["hdp1"]), CastAide.AsInt32(drBet["betteam"])));
                                    TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref row, "bettype", bettypeName);
                                    TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                }
                                TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["stake"], 2));
                                TemplateHandler.Assign(ref row, "currency", GetCurrencyName(CastAide.AsInt32(drBet["currency"])));

                                switch (CastAide.AsInt32(drBet["bettype"]))
                                {
                                    case 1:
                                    case 7:
                                    case 22:
                                        betteam = CastAide.AsString(drBet["betteam"]).ToLower();
                                        Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                        Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                        Decimal hdp = 0;
                                        if (betteam == "h")
                                            hdp = hdp2 - hdp1;
                                        else
                                            hdp = hdp1 - hdp2;
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp));
                                        break;
                                    case 3:
                                    case 8:
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(drBet["hdp1"]));
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

                                if (IsLiveCasino)
                                {
                                    TemplateHandler.Assign(ref row, "stake", drBet["stake"]);
                                    TemplateHandler.Assign(ref row, "status", UppercaseFirst(CastAide.AsString(drBet["status"])));
                                    TemplateHandler.Assign(ref row, "superpt", drBet["spositiontaking"]);
                                    TemplateHandler.Assign(ref row, "masterpt", drBet["mpositiontaking"]);
                                    TemplateHandler.Assign(ref row, "agentpt", drBet["apositiontaking"]);
                                }

                                body.Append(row);
                            }
                            #endregion

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
                                // always send to csd
                                mailElement = SpecialDeleteSection.Instance.RecoverBet["MailForRecoverBet"];
                                ToMailCSD = mailElement.To;
                                CC = mailElement.CC;
                                BCC = mailElement.BCC;

                                if (IsDeveloperRecovered) // mail to NotificationRealCurrency - internal group
                                {
                                    mailElement = SpecialDeleteSection.Instance.NotificationRealCurrency["EmailNotification"];

                                    if (ToMailCSD.Length > 0)
                                        ToMailCSD = ToMailCSD + ";" + mailElement.To;
                                    else
                                        ToMailCSD = mailElement.To;

                                    if (CC.Length > 0)
                                        CC = CC + ";" + mailElement.CC;
                                    else
                                        CC = mailElement.CC;

                                    if (BCC.Length > 0)
                                        BCC = BCC + ";" + mailElement.BCC;
                                    else
                                        BCC = mailElement.BCC;
                                }
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


                            // send mail via gmail system
                            bool result = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                            string msg = "";
                            site = site.ToUpper();

                            mailElement = SpecialDeleteSection.Instance.RecoverBet["MailForRecoverBet"];
                            ToMailCSD = mailElement.To;

                            if (result)
                            {
                                msg = "Details of recover " + site + " ticket(s) are sent successfully to " + ToMailCSD;
                                ErrMsg += AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success);
                            }
                            else
                            {
                                msg = "Email of " + site + " " + ToMailCSD + " failed. Please inform IT";
                                ErrMsg += AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error);
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

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }

        private string GetCurrencyName(int Currency)
        {
            DbResult ds = SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
            if (ds == null) return string.Empty;
            DataTable tbl = ds.ResultSet.Tables[0];
            EnumerableRowCollection<DataRow> currencies = tbl.AsEnumerable();
            foreach (DataRow dr in currencies)
            {
                if (DNAide.Core.CastAide.AsInt32(dr["exchangeid"]) == Currency)
                    return DNAide.Core.CastAide.AsString(dr["currency"]);
            }
            return string.Empty;
        }

        private string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            if (s.Length == 1) return char.ToUpper(s[0]).ToString();
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private string BuildChoiceColumnForLiveCasino(decimal refNo, DataTable table1)
        {
            var arrChoice = new List<string>();
            var choices = new List<Choice>();

            foreach (DataRow row1 in table1.Rows)
            {
                if (refNo == CastAide.AsDecimal(row1["refno"]))
                {
                    choices.Add(new Choice
                    {
                        Betteam = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(row1["betteam"])),
                        Stake = NumberFormat.DecFormat(row1["stake"], 2),
                        Odds = NumberFormat.DecFormat(row1["odds"], 2)
                    });
                }
            }

            foreach (Choice c in choices)
            {
                arrChoice.Add("<span><span style='color:red'>" + c.Betteam + "</span> " + c.Stake + " <span style='color:#4F81BD'>@" + c.Odds + "</span></span>");
            }
            return string.Join("<br />", arrChoice.ToArray());
        }

        /// <summary>
        /// SendEmailToCSDEarlyKO
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        /// <param name="rs"></param>
        /// <param name="transid"></param>
        /// <returns></returns>
        protected string SendEmailToCSDEarlyKO(DataSet rs, string transid, string reason)
        {
            string ErrMsg = "";
            try
            {
                if (rs.Tables.Count >= 2)
                {
                    DataTable dtMatch = rs.Tables[0];
                    DataTable dtUsers = rs.Tables[1];

                    if (dtMatch.Rows.Count > 0 && dtUsers.Rows.Count > 0)
                    {
                        string tplpath = "";
                        string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplSendMailEarlyKickOffTime.htm");
                        TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                        string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");
                        string tplUserMP = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRowMP.htm");

                        ////For special bets - 12EU
                        //string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");
                        //TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                        //string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");

                        #region send mail to CSD
                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        if (CastAide.AsInt32(dtUsers.Rows[0]["bettype"]) == 10)
                        {
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                            TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplUser, "score");
                            TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                            ////For 12EU
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

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

                                #endregion
                            }
                            else
                            {
                                foreach (SpecialDeleteElement store in collection.Stores)
                                {
                                    SpecialDeleteDataforMailing mailing = new SpecialDeleteDataforMailing()
                                    {
                                        To = store.To,
                                        Cc = store.CC,
                                        Bcc = store.BCC,
                                        Name = store.Name,
                                        Alias = store.Alias,
                                    };
                                    mailing.Items = new DataTable();

                                    DataTable dtUser = dtUsers.Clone();
                                    string expression = string.Empty;

                                    if (store.Alias.ToLower().Equals("deposit"))
                                    {
                                        expression = GetSitesAndUsers();
                                    }
                                    else
                                    {
                                        expression = "" + store.Name + "='" + store.Alias + "'";
                                    }

                                    DataRow[] rows = dtUsers.Select(expression);
                                    foreach (DataRow row in rows)
                                    {                                        
                                        dtUser.ImportRow(row);
                                    }

                                    mailing.Items = dtUser;
                                    maillings.Items.Add(mailing);
                                }
                            }
                        }

                        #endregion

                        foreach (SpecialDeleteDataforMailing dataforMailing in maillings.Items)
                        {
                            if (dataforMailing.Items.Rows.Count > 0)
                            {
                                #region MatchInfo
                                DataRow drMatch = dtMatch.Rows[0];
                                string Subject = string.Empty;


                                Subject = "Deleted Bets report (Early Kick-Off) for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";



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
                                string bgColor = string.Empty;
                                string isMP = string.Empty;
                                #endregion

                                #region AssignRow
                                StringBuilder body = new StringBuilder();
                                StringBuilder body_mix = new StringBuilder();
                                bool hasSingle = false; bool hasMP = false;
                                string betteam = string.Empty;
                                foreach (DataRow drBet in dataforMailing.Items.Rows)
                                {

                                    isMP = drBet["ismp"].ToString();
                                    if (isMP.Equals("1"))
                                    {
                                        hasMP = true;
                                        site = drBet["site"].ToString();
                                        string rowMP = tplUserMP.Clone().ToString();

                                        TemplateHandler.Assign(ref rowMP, "username", drBet["username"]);
                                        TemplateHandler.Assign(ref rowMP, "refno", drBet["transid"]);
                                        TemplateHandler.Assign(ref rowMP, "transdate", drBet["transdate"]);

                                        string choice = GenMPRow(CastAide.AsString(drBet["refno"]), CastAide.AsString(drBet["subtransid"]));
                                        TemplateHandler.Assign(ref rowMP, "choice", choice);
                                        TemplateHandler.Assign(ref rowMP, "bettype", Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(CastAide.AsInt32(drBet["parlaytype"])));
                                        TemplateHandler.Assign(ref rowMP, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                                        TemplateHandler.Assign(ref rowMP, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                        body_mix.Append(rowMP);
                                    }
                                    else
                                    {
                                        hasSingle = true;
                                        site = drBet["site"].ToString();
                                        bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                        string row = tplUser.Clone().ToString();

                                        TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                        TemplateHandler.Assign(ref row, "username", drBet["username"]);
                                        TemplateHandler.Assign(ref row, "username1", drBet["username2"]);



                                        TemplateHandler.Assign(ref row, "refno", drBet["transid"]);
                                        bool IsLive = false;
                                        TemplateHandler.Assign(ref row, "bettype", getBettypeName(drBet["bettype"].ToString(), IsLive));
                                        TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                        TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                        TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                        TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["stake"], 2));
                                        TemplateHandler.Assign(ref row, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                        switch (CastAide.AsInt32(drBet["bettype"]))
                                        {
                                            case 1:
                                            case 7:
                                            case 22:
                                                betteam = CastAide.AsString(drBet["betteam"]).ToLower();
                                                Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                                Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                                Decimal hdp = 0;
                                                if (betteam == "h")
                                                    hdp = hdp2 - hdp1;
                                                else
                                                    hdp = hdp1 - hdp2;
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp));                                             
                                                break;
                                            case 3:
                                            case 8:
                                                TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(drBet["hdp1"]));                                             
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
                                }
                                #endregion


                                if (hasSingle)
                                {
                                    TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());
                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref tplMatch, "single-bet");
                                }
                                if (hasMP)
                                {
                                    TemplateHandler.Assign(ref tplMatch, "Rows_Mix", body_mix.ToString());
                                }
                                else
                                {
                                    TemplateHandler.RemoveBlock(ref tplMatch, "mixparlay-bet");
                                }

                                string ToMailCSD = "";
                                string CC = "", BCC = "";

                                ToMailCSD = dataforMailing.To;
                                CC = dataforMailing.Cc;
                                BCC = dataforMailing.Bcc;

                                Regex regx = new Regex("12eu|best-odds", RegexOptions.IgnoreCase);
                                site = regx.IsMatch(dataforMailing.Alias) ? dataforMailing.Alias.ToUpper() : site;

                                Subject = Subject.Replace("{site}", site);
                                Subject = Subject.Replace("{site_name}", dataforMailing.Name);

                               
                                // send mail via gmail system
                                bool result = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);

                                string msg = "";
                                site = site.ToUpper() + " " + dataforMailing.Name;

                                if (result)
                                {
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
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                ErrMsg += AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
            return ErrMsg;
        }


        protected string SendEmailWhenDeleteBetToInternalGroupEarlyKO(DataSet rs, string transid, bool IsRealCurrency, string reason)
        {
            string ErrMsg = "";
            try
            {
                if (rs.Tables.Count >= 3)
                {
                    DataTable dtMatch = rs.Tables[0];
                    DataTable dtUsers = rs.Tables[3];

                    string expression = "";
                    if (IsRealCurrency)
                    {
                        expression = "currency  <> 20";
                    }
                    else
                    {
                        expression = " currency = 20";
                    }

                    DataRow[] rows = dtUsers.Select(expression);
                    int cnt = rows.Count();

                    if (dtMatch.Rows.Count > 0 && cnt > 0)
                    {
                        string tplpath = "";
                        string tplMatchAll = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "TplSendMailEarlyKickOffTime.htm");
                        TemplateHandler.Assign(ref tplMatchAll, "ActionType", "Deleted");
                        string tplUser = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow.htm");
                        string tplUserMP = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRowMP.htm");

                        ////For special bets - 12EU
                        //string tplMatchAll_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMail_SpecialBet.htm");
                        //TemplateHandler.Assign(ref tplMatchAll_SpecialBet, "ActionType", "Deleted");

                        //string tplUser_SpecialBet = WebUtils.LoadTemplate("SpecialDelete/" + tplpath + "DeleteBetReportSendMailAllRow_SpecialBet.htm");

                        #region send mail to CSD
                        string strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + (CastAide.AsBoolean(dtMatch.Rows[0]["isNeutral"]) ? "(N)" : string.Empty);
                        if (CastAide.AsInt32(dtUsers.Rows[0]["bettype"]) == 10)
                        {
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "score");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "hdp");
                            TemplateHandler.RemoveBlock(ref tplMatchAll, "vs");

                            TemplateHandler.RemoveBlock(ref tplUser, "betype-choice");
                            TemplateHandler.RemoveBlock(ref tplUser, "score");
                            TemplateHandler.RemoveBlock(ref tplUser, "hdp");

                            ////For 12EU
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "betype-choice");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "score");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "hdp");
                            //TemplateHandler.RemoveBlock(ref tplMatchAll_SpecialBet, "vs");

                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "betype-choice");
                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "score");
                            //TemplateHandler.RemoveBlock(ref tplUser_SpecialBet, "hdp");

                            strHome = CacheRef.GetTeamName(dtMatch.Rows[0]["homeid"]) + " - " + CacheRef.GetLeagueName(dtMatch.Rows[0]["leagueid"]);

                        }


                        #region MatchInfo
                        DataRow drMatch = dtMatch.Rows[0];
                        string Subject = string.Empty;


                        Subject = "Deleted Bets report (Early Kick-Off) for {site} {site_name} (" + Convert.ToDateTime(drMatch["datetime"]) + ")";
                        if (IsRealCurrency)
                            Subject = "Real Currency - " + Subject;


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
                        string bgColor = string.Empty;
                        string isMP = string.Empty;

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

                        #endregion

                        #region AssignRow
                        StringBuilder body = new StringBuilder();
                        StringBuilder body_mix = new StringBuilder();
                        bool hasSingle = false; bool hasMP = false;
                        string betteam = string.Empty;
                        foreach (DataRow drBet in rows)
                        {

                            isMP = drBet["ismp"].ToString();
                            if (isMP.Equals("1"))
                            {
                                hasMP = true;
                                // site = drBet["site"].ToString();
                                string rowMP = tplUserMP.Clone().ToString();

                                TemplateHandler.Assign(ref rowMP, "username", drBet["username"]);
                                TemplateHandler.Assign(ref rowMP, "refno", drBet["transid"]);
                                TemplateHandler.Assign(ref rowMP, "transdate", drBet["transdate"]);
                                TemplateHandler.Assign(ref rowMP, "bettype", Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(CastAide.AsInt32(drBet["parlaytype"])));

                                string choice = GenMPRow(CastAide.AsString(drBet["refno"]), CastAide.AsString(drBet["subtransid"]));
                                TemplateHandler.Assign(ref rowMP, "choice", choice);

                                TemplateHandler.Assign(ref rowMP, "stake", NumberFormat.DecFormat(drBet["stake"], 2));
                                TemplateHandler.Assign(ref rowMP, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                body_mix.Append(rowMP);
                            }
                            else
                            {
                                hasSingle = true;
                                //site = drBet["site"].ToString();
                                bgColor = (bgColor == "#FCFCFC") ? "#F2F2F2" : "#FCFCFC";
                                string row = tplUser.Clone().ToString();

                                TemplateHandler.Assign(ref row, "bgcolor", bgColor);
                                TemplateHandler.Assign(ref row, "username", drBet["username"]);
                                TemplateHandler.Assign(ref row, "username1", drBet["username2"]);



                                TemplateHandler.Assign(ref row, "refno", drBet["transid"]);
                                bool IsLive = false;
                                TemplateHandler.Assign(ref row, "bettype", getBettypeName(drBet["bettype"].ToString(), IsLive));
                                TemplateHandler.Assign(ref row, "bet", Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(drBet["betteam"].ToString(), CastAide.AsInt32(drBet["bettype"]), "e"));
                                TemplateHandler.Assign(ref row, "odds", NumberFormat.DecFormat(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(drBet["odds"]), CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"])), 3));
                                TemplateHandler.Assign(ref row, "OddsType", CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(CastAide.AsInt32(drBet["bettype"]), CastAide.AsInt32(drBet["oddstype"]))));
                                TemplateHandler.Assign(ref row, "amount", NumberFormat.DecFormat(drBet["stake"], 2));
                                TemplateHandler.Assign(ref row, "currency", CacheUtils.GetExchangeName(CastAide.AsInt32(drBet["currency"])));

                                switch (CastAide.AsInt32(drBet["bettype"]))
                                {
                                    case 1:
                                    case 7:
                                    case 22:
                                        betteam = CastAide.AsString(drBet["betteam"]).ToLower();
                                        Decimal hdp1 = Convert.IsDBNull(drBet["hdp1"]) ? 0 : Convert.ToDecimal(drBet["hdp1"]);
                                        Decimal hdp2 = Convert.IsDBNull(drBet["hdp2"]) ? 0 : Convert.ToDecimal(drBet["hdp2"]);
                                        Decimal hdp = 0;
                                        if (betteam == "h")
                                            hdp = hdp2 - hdp1;
                                        else
                                            hdp = hdp1 - hdp2;
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp));                                             
                                        break;
                                    case 3:
                                    case 8:
                                        TemplateHandler.Assign(ref row, "{hdp}", IBCUtils.FormatHandler.FormatNumber2(drBet["hdp1"]));                                             
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
                        }
                        #endregion


                        if (hasSingle)
                        {
                            TemplateHandler.Assign(ref tplMatch, "Rows", body.ToString());
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref tplMatch, "single-bet");
                        }
                        if (hasMP)
                        {
                            TemplateHandler.Assign(ref tplMatch, "Rows_Mix", body_mix.ToString());
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref tplMatch, "mixparlay-bet");
                        }

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


                        Subject = Subject.Replace("{site}", site);
                        Subject = Subject.Replace("{site_name}", "");

                        // send mail via gmail system
                        bool result = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, ToMailCSD, CC, BCC, Subject, Subject, tplMatch, null);


                        #endregion
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

        #region WriteLogMail
        protected void WriteLogMail(string content)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("Logs");// +"/LogMailSpecialDelete.html";
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
        #endregion

        #region MixParlay
        protected decimal odds = 0, hdp1 = 0, hdp2 = 0;
        protected string bettypeName = "", sportypeName = "", leagueName = "", betteam = "";
        protected int bettype = 0;
        protected bool IsSearchResult = false;
        protected string GenMPRow(string refno, string transid)
        {
            string tplEvent = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/SendMailRowMPEvent.htm");
            TemplateHandler.Assign(ref tplEvent, "MoreBetInfo", "");
            string strData = "";
            try
            {
                System.Threading.Thread.Sleep(5000);
                DbResult ds = AccountReport.AccBetsSelBetCheckM(refno);
                string eventItems = string.Empty;
                if (ds != null && ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    int ind = 1; int numC = ds.ResultSet.Tables[0].Rows.Count;
                    foreach (DataRow row2 in ds.ResultSet.Tables[0].Select())
                    {
                        string mpItem = (string)tplEvent.Clone();
                        string mpEvent = (string)mpItem.Clone();
                        string mpHome = CacheRef.GetTeamName(row2["homeid"]) + (CastAide.AsBoolean(row2["isNeutral"]) ? "(N)" : string.Empty);
                        string mpAway = CacheRef.GetTeamName(row2["awayid"]);
                        betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(row2["betteam"].ToString(), Convert.ToInt32(row2["bettype"]), "e");
                        sportypeName = CacheRef.GetSportName(row2["Sporttype"]);
                        leagueName = CacheRef.GetLeagueName(row2["leagueid"]);
                        bettype = Convert.IsDBNull(row2["bettype"]) ? 0 : Convert.ToInt32(row2["bettype"]);
                        decimal mpHdp1 = Convert.IsDBNull(row2["hdp1"]) ? 0 : Convert.ToDecimal(row2["hdp1"]);
                        decimal mpHdp2 = Convert.IsDBNull(row2["hdp1"]) ? 0 : Convert.ToDecimal(row2["hdp2"]);
                        decimal mpHdp = (betteam == "h") ? mpHdp2 - mpHdp1 : mpHdp1 - mpHdp2;
                        decimal mpOdds = Convert.IsDBNull(row2["odds"]) ? 0 : Convert.ToDecimal(row2["odds"]);
                        odds = mpOdds;
                        hdp1 = mpHdp1;
                        hdp2 = mpHdp2;

                        bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(CastAide.AsInt32(row2["bettype"]));
                        string moreBetInfo = string.Empty;

                        string tid = row2["transid"].ToString();
                        string mid = row2["matchid"].ToString();
                        string bid = row2["bettype"].ToString();
                        int statusId = CastAide.AsInt32(row2["statusid"]);

                        if (tid == transid)
                        {
                            TemplateHandler.Assign(ref mpItem, "MP", "");
                            TemplateHandler.Assign(ref mpItem, "style", "style=\"background-color:#FFFFAA;border:solid 1px #7C98A4\"");
                            IsSearchResult = true;
                        }
                        else
                        {
                            TemplateHandler.Assign(ref mpItem, "MP", "MP");
                            TemplateHandler.Assign(ref mpItem, "bgcolor", "");
                            IsSearchResult = false;
                        }

                        if (ind == numC)
                        {
                            TemplateHandler.Assign(ref mpItem, "hr", "");
                        }
                        switch (bettype)
                        {
                            case 1:
                            case 7:
                            case 20:
                            case 21:
                            case 124:
                            case 125:
                            case 1201:
                                GenHdpEvent(ref mpItem, mpHome, mpAway, betteam, statusId, true);
                                break;
                            case 2:
                            case 12:
                                GenOEEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;
                            case 3:
                            case 8:
                            case 1203:
                                GenOUEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;
                            case 4:
                            case 14:
                            case 30:
                            case 1204:
                                GenCSEvent(ref mpItem, mpHome, mpAway, statusId, true);
                                break;
                            case 5:
                            case 15:
                            case 1205:
                                Gen1x2Event(ref mpItem, mpHome, mpAway, statusId, true);
                                break;
                            case 6:
                            case 16:
                            case 1206:
                                GenTGEvent(ref mpItem, mpHome, mpAway, statusId, true);
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

                        strData += mpItem + ((ind == numC) ? "" : "<br>");
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
        protected string oddMP()
        {
            string cls = (IsSearchResult) ? "" : "MP";
            return "<span class='style_12BB" + cls + "'><b> @ " + NumberFormat.DecFormat(odds, 3) + "</b></span>";
        }
        protected void GenHdpEvent(ref string eventItem, string home, string away, string betteam, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (bettype == 20) //Money line
            {
                if (betteam == "h") choice = home;
                else choice = away;
            }
            else
            {
                if (betteam == "h") choice = home + " " + NumberFormat.DecFormat((hdp2 - hdp1), 2) + liveScore;
                else choice = away + " " + NumberFormat.DecFormat((hdp1 - hdp2), 2) + liveScore;
            }

            if (isMP) choice += oddMP();
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenHdpEvent(ref string eventItem, string home, string away, string betteam, int statusId, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (bettype == 20) //Money line
            {
                if (betteam == "h") choice = home;
                else choice = away;
            }
            else
            {
                if (betteam == "h") choice = home + " " + NumberFormat.DecFormat((hdp2 - hdp1), 2) + liveScore;
                else choice = away + " " + NumberFormat.DecFormat((hdp1 - hdp2), 2) + liveScore;
            }

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

        protected void GenHomeAwayDrawNoBet(ref string eventItem, string home, string away, string betteam, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (bettype == 121)
            {
                if (betteam == "x") choice = "Draw";
                else if(betteam == "a") choice = away;
            }
            else if (bettype == 122)
            {
                if (betteam == "h") choice = home;
                else if(betteam == "x") choice = "Draw";
            }
            else if (bettype == 123)
            {
                if (betteam == "h") choice = "Draw";
                else if(betteam == "a") choice = "No Draw";
            }
            if (isMP) choice += oddMP();
            string Event = home + " -VS- " + away;

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }
        protected void GenHomeAwayDrawNoBet(ref string eventItem, string home, string away, int statusId, string betteam, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (bettype == 121)
            {
                if (betteam == "x") choice = "Draw";
                else if(betteam == "a") choice = away;
            }
            else if (bettype == 122)
            {
                if (betteam == "h") choice = home;
                else if(betteam == "x") choice = "Draw";
            }
            else if (bettype == 123)
            {
                if (betteam == "h") choice = "Draw";
                else if(betteam == "a") choice = "No Draw";
            }
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
        protected void GenOEEvent(ref string eventItem, string home, string away, bool isMP)
        {
            string choice = "";
            if (betteam == "h") choice = "ODD";
            else choice = "EVEN";

            if (isMP) choice += oddMP();
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }


        protected void GenOEEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            string choice = "";
            if (betteam == "h") choice = "ODD";
            else choice = "EVEN";

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

        protected void GenOUEvent(ref string eventItem, string home, string away, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (betteam == "h") choice = "OVER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;
            else choice = "UNDER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;

            if (isMP) choice += oddMP();
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenOUEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            string liveScore = "";
            string choice = "";
            if (betteam == "h") choice = "OVER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;
            else choice = "UNDER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;

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

        protected void GenCSEvent(ref string eventItem, string home, string away, bool isMP)
        {
            string choice = "";
            choice = (betteam == "5:0" || betteam == "0:5") ? betteam + " UP"  : betteam;
            if (bettype == 30)
                choice = (betteam == "4:0" || betteam == "0:4") ? betteam + " UP" : betteam;
            
            if (isMP) choice += oddMP();

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenCSEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {

            string choice = "";
            choice = (betteam == "5:0" || betteam == "0:5") ? betteam +" UP" : betteam;
            if(bettype == 30)
                choice = (betteam == "4:0" || betteam == "0:4") ? betteam + " UP" : betteam;
            
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

        protected void Gen1x2Event(ref string eventItem, string home, string away, bool isMP)
        {
            string choice = "";
            if (bettype == 5) choice = "FT. " + betteam;
            if (bettype == 15) choice = "HT. " + betteam;
            if (isMP) choice += oddMP();

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void Gen1x2Event(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            string choice = "";
            if (bettype == 5) choice = "FT. " + betteam;
            if (bettype == 15) choice = "HT. " + betteam;
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

        protected void GenTGEvent(ref string eventItem, string home, string away, bool isMP)
        {
            string choice = betteam;
            if (isMP) choice += oddMP();
            string Event = home + "-VS-" + away;

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        protected void GenTGEvent(ref string eventItem, string home, string away, int statusId, bool isMP)
        {
            string choice = betteam;
            if (isMP) choice += oddMP();
            string Event = home + "-VS-" + away;
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

        #endregion

        protected string SplitMailList(string mailList)
        {
            if (mailList.Length > 0 && mailList.Substring(mailList.Length-1,1) == ";")
                mailList = mailList.Substring(0, mailList.Length - 1);

            return mailList;
        }
    }
}
