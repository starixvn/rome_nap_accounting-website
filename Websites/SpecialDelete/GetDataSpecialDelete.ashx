<%@ WebHandler Language="C#" Class="GetDataSpecialDelete" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using IBCUtils;
using System.Web.SessionState;
using DNAide.Data;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Collections;
using System.Text;
using System.Data;



public class GetDataSpecialDelete : IHttpHandler, IRequiresSessionState
{
    string script = "";
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        int matchId = 0;
        long frefNo = 0;
        long trefNo = 0;
        string betTypes = context.Request.Form["bettype"];
        string typename = context.Request.Form["bettypename"];
        
        StringBuilder html = new StringBuilder();

        int type = 0;
        int.TryParse(context.Request["type"], out type);
        if (!int.TryParse(context.Request.Form["matchid"], out matchId) || !long.TryParse(context.Request.Form["frefno"], out frefNo) ||
            !long.TryParse(context.Request.Form["trefno"], out trefNo))
        {
            html.Append(AlertMessage.GetMsg("Invalid Input Data!", AlertMessage.MsgType.Error));
            context.Response.Write(html.ToString());
            return;
        }

        int liveindicator = CastAide.AsInt32(context.Request["ddlLiveIndicator"], 2);        
        string betteam1378 = CastAide.AsString(context.Request["ddlBetTeam1378"], "All");
        string betteam515 = CastAide.AsString(context.Request["ddlBetTeam515"], "All");
        string betteam4 = context.Request.Form["betteam4"];
        
        string hdp = context.Request.Form["hdp"];
        hdp = (string.IsNullOrEmpty(hdp)) ? "9999" : hdp;
        DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountTransaction.BetTransSelSpecialDel(matchId, frefNo, trefNo, betTypes, betteam1378, betteam515,betteam4, hdp, liveindicator, AppSession.Username.ToLower());
        if (ds == null)
        {
            html.Append(AlertMessage.GetMsg(IBCUtils.IbcError.INTERNAL_ERROR_MSG + "!", AlertMessage.MsgType.Error));
            context.Response.Write(html.ToString());
            return;
        }
        bool IsFail = false;
        html.Append(BuildTransList(ds.ResultSet, matchId, betTypes, ref IsFail));
        if (ds.ResultSet.Tables[0].Rows.Count > 0 || ds.ResultSet.Tables[1].Rows.Count > 0 || ds.ResultSet.Tables[2].Rows.Count > 0 || ds.ResultSet.Tables[3].Rows.Count > 0 || ds.ResultSet.Tables[4].Rows.Count > 0)
        {
            if (!IsFail)
                html.Append("<br/>" + BuildDeleteFormNew(matchId.ToString(), frefNo.ToString(), trefNo.ToString(), typename, betteam1378, betteam515, context.Request.Form["hdp"], betTypes, liveindicator ));
            html.Append("<br/>" + script);
            context.Response.Write(html.ToString());
        }
        else
            context.Response.Write("<span class='error-message'>There is no information available.</span>");
    }

    string BuildDeleteFormNew(string matchId, string frefno, string trefno, string bettypename, string betteam1378, string betteam515, string hdp, string betTypes, int liveindicator)
    {
        try
        {

            string html = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete\\DeleteFormNew.htm");
            hdp = (string.IsNullOrEmpty(hdp)) ? "N/A" : hdp;

            string BettypeStr = "";
            foreach (string btname in bettypename.Split('^'))
            {
                BettypeStr += btname + ", ";
            }

            BettypeStr = BettypeStr.Substring(0, BettypeStr.Trim().LastIndexOf(",") - 2);

            TemplateHandler.Assign(ref html, "{FromRefno}", frefno);
            TemplateHandler.Assign(ref html, "{ToRefno}", trefno);
            TemplateHandler.Assign(ref html, "{MatchId}", matchId);
            TemplateHandler.Assign(ref html, "{Bettype}", betTypes);
            TemplateHandler.Assign(ref html, "{BettypeStr}", BettypeStr);
            TemplateHandler.Assign(ref html, "{Betteam1378}", betteam1378);
            TemplateHandler.Assign(ref html, "{Betteam515}", betteam515);
            TemplateHandler.Assign(ref html, "{Hdp}", hdp);
            TemplateHandler.Assign(ref html, "{liveindicator}", liveindicator);            
            return html;
        }
        catch (Exception ex)
        {
           Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    string BuildTransList(DataSet dataset, int matchId, string bettypes, ref bool IsFail)
    {
        IsFail = false;
        string matchHtml = string.Empty;
        StringBuilder _transListHtml = new StringBuilder();
        string ContentHtml = string.Empty;
        string transList_BD = string.Empty;
        string transList_12EU = string.Empty;
        string transList_Deposit = string.Empty;
        string transList_MHC = string.Empty;
        int rowcount = 0, rowcountDeposit = 0, rowcountBD = 0, rowcount12EU = 0, rowcountMHC = 0;
        try
        {
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountTransaction.MatchSeById(matchId);
            if (ds == null) return AlertMessage.GetMsg(IBCUtils.IbcError.INTERNAL_ERROR_MSG, AlertMessage.MsgType.Error);
            if (ds.ResultSet.Tables[0].Rows.Count == 0)
            {
                IsFail = true;
                return AlertMessage.GetMsg("NO MATCH WITH SUCH MATCH ID", AlertMessage.MsgType.Error);
            }
            DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
            matchHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("SpecialDelete\\MatchInfo.htm", new string[] { "{MatchCode}", "{Home}", "{Away}", "{EventDate}" },
                new string[] { row1["matchcode"] + "", CacheRef.GetTeamName(row1["homeid"]) + (CastAide.AsBoolean(row1["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty), CacheRef.GetTeamName(row1["awayid"]) + "", (Convert.IsDBNull(row1["eventdate"]) ? "" : Convert.ToDateTime(row1["eventdate"]).ToShortDateString()) });
            _transListHtml.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete\\TransHeader.htm"));
            string transRowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete\\TransRow.htm");
            string tplNoInfo = TemplateHandler.CopyBlock(transRowTpl, "no-info");
            TemplateHandler.RemoveBlock(ref transRowTpl, "no-info");

            string transRowTpl_SB = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete\\TransRow_SpecialBet.htm");            
            transList_BD = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete\\TransHeader_SpecialBet.htm");
            transList_12EU = transList_BD;
            transList_Deposit = transList_BD;
            transList_MHC = transList_BD;

            //For normal bets
            DataTable table = dataset.Tables[0];
            
            int count = 0;

            int countTbl = 0;
            foreach (DataRow row in table.Rows)
            {
                countTbl++;
                string transRow = (string)transRowTpl.Clone();
                TemplateHandler.Assign(ref transRow, "{no}", countTbl.ToString());
                TemplateHandler.Assign(ref transRow, "{Account}", row["username"].ToString());
                TemplateHandler.Assign(ref transRow, "{Refno}", row["transid"].ToString());
                TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row["bettype"]));
                TemplateHandler.Assign(ref transRow, "{Bet}", row["betteam"].ToString());
                TemplateHandler.Assign(ref transRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(row["odds"]));
                TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row["oddstype"]));
                TemplateHandler.Assign(ref transRow, "{Amount}", IBCUtils.FormatHandler.FormatNumber2(row["StakePositiontaking"]));
                TemplateHandler.Assign(ref transRow, "{currency}", row["currency"].ToString());
                switch (CastAide.AsInt32(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                    case 22:
                        Decimal hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                        Decimal hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                        TemplateHandler.Assign(ref transRow, "{Hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                        break;
                    default:
                        TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                        break;
                }
                string transDate = Convert.IsDBNull(row["transdate"]) ? "Error" : (Convert.ToDateTime(row["transdate"]).ToString());
                TemplateHandler.Assign(ref transRow, "{TransTime}", transDate);
                TemplateHandler.Assign(ref transRow, "{HomeScore}", row["livehomescore"] + "");
                TemplateHandler.Assign(ref transRow, "{AwayScore}", row["liveawayscore"] + "");
                TemplateHandler.Assign(ref transRow, "{Creator}", row["creator"] + "");
                TemplateHandler.Assign(ref transRow, "{Status}", row["status"] + "");
                
                _transListHtml.Append(transRow + "\n");
            }
            

            //For Deposit bets
            DataTable table2 = dataset.Tables[1];
            int count2 = 0;
            foreach (DataRow row in table2.Rows)
            {
                count2++;
                string transRow = (string)transRowTpl_SB.Clone();
                TemplateHandler.Assign(ref transRow, "{no}", count2.ToString());
                TemplateHandler.Assign(ref transRow, "{Account}", row["username"].ToString());
                TemplateHandler.Assign(ref transRow, "{Refno}", row["transid"].ToString());
                TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row["bettype"]));
                TemplateHandler.Assign(ref transRow, "{Bet}", row["betteam"].ToString());
                TemplateHandler.Assign(ref transRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(row["odds"]));
                TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row["oddstype"]));
                TemplateHandler.Assign(ref transRow, "{Amount}", IBCUtils.FormatHandler.FormatNumber2(row["StakePositiontaking"]));
                TemplateHandler.Assign(ref transRow, "{currency}", row["currency"].ToString());
                TemplateHandler.Assign(ref transRow, "{SB}", "Deposit");
                switch (CastAide.AsInt32(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                    case 22:
                        Decimal hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                        Decimal hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                        TemplateHandler.Assign(ref transRow, "{Hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                        break;
                    default:
                        TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                        break;
                }
                string transDate = Convert.IsDBNull(row["transdate"]) ? "Error" : (Convert.ToDateTime(row["transdate"]).ToString());
                TemplateHandler.Assign(ref transRow, "{TransTime}", transDate);
                TemplateHandler.Assign(ref transRow, "{HomeScore}", row["livehomescore"] + "");
                TemplateHandler.Assign(ref transRow, "{AwayScore}", row["liveawayscore"] + "");
                TemplateHandler.Assign(ref transRow, "{Creator}", row["creator"] + "");
                TemplateHandler.Assign(ref transRow, "{Status}", row["status"] + "");
                string IsDelete = "0";
                if (AppSession.Username == "joe" || AppSession.Username == "steven")
                    IsDelete = "1";
                else if (CastAide.AsString(row["status"]) == "running")
                    IsDelete = "1";
                TemplateHandler.Assign(ref transRow, "{IsDelete}", IsDelete);
                transList_Deposit += transRow + "\n";
            }
            //replace SP(Special Bet to Deposit )
            if (count2 == 0)
                transList_Deposit = "";
            else
            {
                TemplateHandler.Assign(ref transList_Deposit, "{SB}", "Deposit");
                TemplateHandler.Assign(ref transList_Deposit, "{SB_Name}", "Deposit");
                TemplateHandler.Assign(ref transList_Deposit, "{SB_Color}", "#6495ed");
            }
            _transListHtml.Append(transList_Deposit);

            //For Special bets - BetDaq
            DataTable table3 = dataset.Tables[2];
            int count3 = 0;
            foreach (DataRow row in table3.Rows)
            {
                count3++;
                string transRow = (string)transRowTpl_SB.Clone();
                TemplateHandler.Assign(ref transRow, "{no}", count3.ToString());
                TemplateHandler.Assign(ref transRow, "{Account}", row["username"].ToString());
                TemplateHandler.Assign(ref transRow, "{Refno}", row["transid"].ToString());
                TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row["bettype"]));
                TemplateHandler.Assign(ref transRow, "{Bet}", row["betteam"].ToString());
                TemplateHandler.Assign(ref transRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(row["odds"]));
                TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row["oddstype"]));
                TemplateHandler.Assign(ref transRow, "{Amount}", IBCUtils.FormatHandler.FormatNumber2(row["StakePositiontaking"]));
                TemplateHandler.Assign(ref transRow, "{currency}", row["currency"].ToString());
                TemplateHandler.Assign(ref transRow, "{SB}", "BD");
                switch (CastAide.AsInt32(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                    case 22:
                        Decimal hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                        Decimal hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                        TemplateHandler.Assign(ref transRow, "{Hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                        break;
                    default:
                        TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                        break;
                }
                string transDate = Convert.IsDBNull(row["transdate"]) ? "Error" : (Convert.ToDateTime(row["transdate"]).ToString());
                TemplateHandler.Assign(ref transRow, "{TransTime}", transDate);
                TemplateHandler.Assign(ref transRow, "{HomeScore}", row["livehomescore"] + "");
                TemplateHandler.Assign(ref transRow, "{AwayScore}", row["liveawayscore"] + "");
                TemplateHandler.Assign(ref transRow, "{Creator}", row["creator"] + "");
                TemplateHandler.Assign(ref transRow, "{Status}", row["status"] + "");
                string IsDelete = "0";
                if (AppSession.Username == "joe" || AppSession.Username == "steven")
                    IsDelete = "1";
                else if (CastAide.AsString(row["status"]) == "running")
                    IsDelete = "1";
                TemplateHandler.Assign(ref transRow, "{IsDelete}", IsDelete);
                transList_BD += transRow + "\n";
            }
            //replace SP(Special Bet to BD - Betdaq)
            if (count3 == 0)
                transList_BD = "";
            else
            {
                TemplateHandler.Assign(ref transList_BD, "{SB}", "BD");
                TemplateHandler.Assign(ref transList_BD, "{SB_Name}", "Bet Daq");
                TemplateHandler.Assign(ref transList_BD, "{SB_Color}", "#659E17");
            }
            _transListHtml.Append(transList_BD);

            //For Special bets - 12EU
            DataTable table4 = dataset.Tables[3];
            int count4 = 0;
            foreach (DataRow row in table4.Rows)
            {
                count4++;
                string transRow = (string)transRowTpl_SB.Clone();
                TemplateHandler.Assign(ref transRow, "{no}", count4.ToString());
                TemplateHandler.Assign(ref transRow, "{Account}", row["username"].ToString());
                TemplateHandler.Assign(ref transRow, "{Refno}", row["transid"].ToString());
                TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row["bettype"]));
                TemplateHandler.Assign(ref transRow, "{Bet}", row["betteam"].ToString());
                TemplateHandler.Assign(ref transRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(row["odds"]));
                TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row["oddstype"]));
                TemplateHandler.Assign(ref transRow, "{Amount}", IBCUtils.FormatHandler.FormatNumber2(row["StakePositiontaking"]));
                TemplateHandler.Assign(ref transRow, "{currency}", row["currency"].ToString());
                TemplateHandler.Assign(ref transRow, "{SB}", "12EU");
                switch (CastAide.AsInt32(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                    case 22:
                        Decimal hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                        Decimal hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                        TemplateHandler.Assign(ref transRow, "{Hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                        break;
                    default:
                        TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                        break;
                }
                string transDate = Convert.IsDBNull(row["transdate"]) ? "Error" : (Convert.ToDateTime(row["transdate"]).ToString());
                TemplateHandler.Assign(ref transRow, "{TransTime}", transDate);
                TemplateHandler.Assign(ref transRow, "{HomeScore}", row["livehomescore"] + "");
                TemplateHandler.Assign(ref transRow, "{AwayScore}", row["liveawayscore"] + "");
                TemplateHandler.Assign(ref transRow, "{Creator}", row["creator"] + "");
                TemplateHandler.Assign(ref transRow, "{Status}", row["status"] + "");
                string IsDelete = "0";
                if (AppSession.Username == "joe" || AppSession.Username == "steven")
                    IsDelete = "1";
                else if (CastAide.AsString(row["status"]) == "running")
                    IsDelete = "1";
                TemplateHandler.Assign(ref transRow, "{IsDelete}", IsDelete);
                transList_12EU += transRow + "\n";
            }
            //replace SP(Special Bet to 12EU)
            if (count4 == 0)
                transList_12EU = "";
            else
            {
                TemplateHandler.Assign(ref transList_12EU, "{SB}", "12EU");
                TemplateHandler.Assign(ref transList_12EU, "{SB_Name}", "12EU");
                TemplateHandler.Assign(ref transList_12EU, "{SB_Color}", "#cc9900");
            }

            _transListHtml.Append(transList_12EU);

            //For member of master: MHCEU (105181), MHCRM (118473), MHCUS(105183)
            DataTable table5 = dataset.Tables[4];
            int count5 = 0;
            foreach (DataRow row in table5.Rows)
            {
                count5++;
                string transRow = (string)transRowTpl_SB.Clone();
                TemplateHandler.Assign(ref transRow, "{no}", count5.ToString());
                TemplateHandler.Assign(ref transRow, "{Account}", row["username"].ToString());
                TemplateHandler.Assign(ref transRow, "{Refno}", row["transid"].ToString());
                TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row["bettype"]));
                TemplateHandler.Assign(ref transRow, "{Bet}", row["betteam"].ToString());
                TemplateHandler.Assign(ref transRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(row["odds"]));
                TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row["oddstype"]));
                TemplateHandler.Assign(ref transRow, "{Amount}", IBCUtils.FormatHandler.FormatNumber2(row["StakePositiontaking"]));
                TemplateHandler.Assign(ref transRow, "{currency}", row["currency"].ToString());
                TemplateHandler.Assign(ref transRow, "{SB}", "MasterMHC");
                switch (CastAide.AsInt32(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                    case 22:
                        Decimal hdp1 = Convert.IsDBNull(row["hdp1"]) ? 0 : Convert.ToDecimal(row["hdp1"]);
                        Decimal hdp2 = Convert.IsDBNull(row["hdp2"]) ? 0 : Convert.ToDecimal(row["hdp2"]);
                        TemplateHandler.Assign(ref transRow, "{Hdp}", IBCUtils.FormatHandler.FormatNumber2(hdp2 - hdp1));
                        break;
                    default:
                        TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                        break;
                }
                string transDate = Convert.IsDBNull(row["transdate"]) ? "Error" : (Convert.ToDateTime(row["transdate"]).ToString());
                TemplateHandler.Assign(ref transRow, "{TransTime}", transDate);
                TemplateHandler.Assign(ref transRow, "{HomeScore}", row["livehomescore"] + "");
                TemplateHandler.Assign(ref transRow, "{AwayScore}", row["liveawayscore"] + "");
                TemplateHandler.Assign(ref transRow, "{Creator}", row["creator"] + "");
                TemplateHandler.Assign(ref transRow, "{Status}", row["status"] + "");
                string IsDelete = "0";
                if (AppSession.Username == "joe" || AppSession.Username == "steven")
                    IsDelete = "1";
                else if (CastAide.AsString(row["status"]) == "running")
                    IsDelete = "1";
                TemplateHandler.Assign(ref transRow, "{IsDelete}", IsDelete);
                transList_MHC += transRow + "\n";
            }
            //replace SP(Special Bet to MHC )
            if (count5 == 0)
                transList_MHC = "";
            else
            {
                TemplateHandler.Assign(ref transList_MHC, "{SB}", "MasterMHC");
                TemplateHandler.Assign(ref transList_MHC, "{SB_Name}", "Members belong to Master MHCEU, MHCRM and MHCUS");
                TemplateHandler.Assign(ref transList_MHC, "{SB_Color}", "#ff8040");
            }
            _transListHtml.Append(transList_MHC);

            if (countTbl == 0 && count2 == 0 && count3 == 0 && count4 == 0 && count5 == 0)
                _transListHtml.Append("<tr><td colspan='14' class='calign'>There is no information available.</td></tr>");

            script += "<script type='text/javascript'>";
            script += "var rowcount = " + table.Rows.Count.ToString() + ";";
            script += "var rowcountDeposit = " + table2.Rows.Count.ToString() + ";";            
            script += "var rowcountBD = " + table3.Rows.Count.ToString() + ";";
            script += "var rowcount12EU = " + table4.Rows.Count.ToString() + ";";
            script += "var rowcount_MasterMHC = " + table5.Rows.Count.ToString() + ";";            
            script += "</script>";
            
            rowcount = table.Rows.Count;
            rowcountDeposit = table2.Rows.Count;
            rowcountBD = table3.Rows.Count;
            rowcount12EU = table4.Rows.Count;
            rowcountMHC = table5.Rows.Count;

            ContentHtml = _transListHtml.ToString();
            if (count2 == 0 && count3 == 0 && count4 == 0 && count5 == 0)
                TemplateHandler.RemoveBlock(ref ContentHtml, "DeleteOption");
        }
        catch (Exception ex)
        {
            ContentHtml+="</table>ERROR WHILE LOADING DATA!";
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }

        ContentHtml+="</table>";
        ContentHtml+="<input type='hidden' id='rowcount' name='rowcount' value='" + rowcount + "'/>";
        ContentHtml+="<input type='hidden' id='rowcountDeposit' name='rowcountDeposit' value='" + rowcountDeposit + "'/>";
        ContentHtml+="<input type='hidden' id='rowcountBD' name='rowcountBD' value='" + rowcountBD + "'/>";
        ContentHtml+="<input type='hidden' id='rowcount12EU' name='rowcount12EU' value='" + rowcount12EU + "'/>";
        ContentHtml += "<input type='hidden' id='rowcountMHC' name='rowcountMHC' value='" + rowcountMHC + "'/>";

        return matchHtml + "<br />\n" + ContentHtml;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
