using System.Collections;
using System.Collections.Generic;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.SpecialDelete;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;

/// <summary>
/// Summary description for SpecialDelete
/// </summary>

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public class SpecialDelete_Entity_ : SpecialDeleteBase
    {
        #region GetSpecialDelete

        public string GetSpecialDelete(Hashtable param)
        {
            string matchHtml = string.Empty;
            var _transListHtml = new StringBuilder();
            string ContentHtml = string.Empty;
            string transList_BD = string.Empty;
            string transList_12EU = string.Empty;
            string transList_Deposit = string.Empty;
            string transList_MHC = string.Empty;
            int matchid = 0;

            _transListHtml.Append(WebUtils.LoadTemplate("SpecialDelete/TransHeader.htm"));
            string transRowTpl = WebUtils.LoadTemplate("SpecialDelete/TransRow.htm");
            string tplNoInfo = TemplateHandler.CopyBlock(transRowTpl, "no-info");
            TemplateHandler.RemoveBlock(ref transRowTpl, "no-info");

            string transRowTpl_SB = WebUtils.LoadTemplate("SpecialDelete/TransRow_SpecialBet.htm");
            transList_BD = WebUtils.LoadTemplate("SpecialDelete/TransHeader_SpecialBet.htm");
            transList_12EU = transList_BD;
            transList_Deposit = transList_BD;
            transList_MHC = transList_BD;

            if (param.ContainsKey("matchid"))
                matchid = CastAide.AsInt32(param["matchid"]);

            ArrayList lst = SpecialDeleteProvider.Instance(DataAccess.DataAccess.DAOClient.Entity).GetSpecialDelete(param);
            int index = 0;
            int countTbl = 0;
            int cntNormalBet = 0, cntDeposit = 0, cntBetDaq = 0, cnt12EU = 0, cntMasterMHC = 0;
            foreach (List<SpecialDeleteRow> item in lst)
            {
                foreach (SpecialDeleteRow row in item)
                {
                    index++;
                    if (countTbl == 0) //NormalBet
                        _transListHtml.Append(AssignDatarow(row, transRowTpl, index));
                    else if (countTbl == 1) //Deposit
                        _transListHtml.Append(AssignDatarow(row, transRowTpl_SB, index, SpecialBettype.BetDeposit));
                    else if (countTbl == 2) //BetDaq
                        _transListHtml.Append(AssignDatarow(row, transRowTpl_SB, index, SpecialBettype.Betdaq));
                    else if (countTbl == 3) //12EU
                        _transListHtml.Append(AssignDatarow(row, transRowTpl_SB, index, SpecialBettype.Bet12EU));
                    else if (countTbl == 4) //member of master: MHCEU (105181), MHCRM (118473), MHCUS(105183)
                        _transListHtml.Append(AssignDatarow(row, transRowTpl_SB, index, SpecialBettype.BetMasterMHC));
                }

                #region Add title

                switch (countTbl)
                {
                    case 0: //Normal
                        cntNormalBet = index;
                        break;

                    case 1: //Deposit
                        if (index > 0)
                        {
                            TemplateHandler.Assign(ref transList_Deposit, "{SB_Name}", "Deposit");
                            TemplateHandler.Assign(ref transList_Deposit, "{SB_Color}", "#6495ed");
                            _transListHtml.Append(transList_Deposit);
                        }
                        cntDeposit = index;
                        break;

                    case 2: //BetDaq
                        if (index > 0)
                        {
                            TemplateHandler.Assign(ref transList_BD, "{SB_Name}", "Bet Daq");
                            TemplateHandler.Assign(ref transList_BD, "{SB_Color}", "#659E17");
                            _transListHtml.Append(transList_BD);
                        }
                        cntBetDaq = index;
                        break;

                    case 3: //12EU
                        if (index > 0)
                        {
                            TemplateHandler.Assign(ref transList_12EU, "{SB_Name}", "12EU");
                            TemplateHandler.Assign(ref transList_12EU, "{SB_Color}", "#cc9900");
                            _transListHtml.Append(transList_12EU);
                        }
                        cnt12EU = index;
                        break;

                    case 4: //member of master: MHCEU (105181), MHCRM (118473), MHCUS(105183)
                        if (index > 0)
                        {
                            TemplateHandler.Assign(ref transList_MHC, "{SB_Name}",
                                                   "Members belong to Master MHCEU, MHCRM and MHCUS");
                            TemplateHandler.Assign(ref transList_MHC, "{SB_Color}", "#ff8040");
                            _transListHtml.Append(transList_MHC);
                        }
                        cntMasterMHC = index;
                        break;
                }

                #endregion Add title

                index = 0;
                countTbl++;
            }

            if (cntNormalBet == 0 && cntDeposit == 0 && cntBetDaq == 0 && cnt12EU == 0 && cntMasterMHC == 0)
                _transListHtml.Append("<tr><td colspan='14' class='calign'>There is no information available.</td></tr>");

            ContentHtml = _transListHtml.ToString();
            if (cntDeposit == 0 && cntBetDaq == 0 && cnt12EU == 0 && cntMasterMHC == 0)
                TemplateHandler.RemoveBlock(ref ContentHtml, "DeleteOption");

            ContentHtml += "</table>";
            ContentHtml += "<input type='hidden' id='rowcount' name='rowcount' value='" + cntNormalBet + "'/>";
            ContentHtml += "<input type='hidden' id='rowcountDeposit' name='rowcountDeposit' value='" + cntDeposit +
                           "'/>";
            ContentHtml += "<input type='hidden' id='rowcountBD' name='rowcountBD' value='" + cntBetDaq + "'/>";
            ContentHtml += "<input type='hidden' id='rowcount12EU' name='rowcount12EU' value='" + cnt12EU + "'/>";
            ContentHtml += "<input type='hidden' id='rowcountMHC' name='rowcountMHC' value='" + cntMasterMHC + "'/>";

            bool IsFail = false;
            string matchinfo = GetMatchInfoById(matchid, ref IsFail);
            if (IsFail)
                ContentHtml = matchinfo;
            else
                ContentHtml = matchinfo + "<br>" + ContentHtml;

            return ContentHtml;
        }

        #endregion GetSpecialDelete

        #region GetMatchInfoById

        private string GetMatchInfoById(int matchid, ref bool IsFail)
        {
            IsFail = false;
            string tpl = "";
            List<MatchInfoRow> lst = SpecialDeleteProvider.Instance(DataAccess.DataAccess.DAOClient.Entity).MatchSeById(matchid);
            if (lst.Count == 0)
            {
                IsFail = true;
                tpl = AlertMessage.GetMsg("NO MATCH WITH SUCH MATCH ID", AlertMessage.MsgType.Error);
            }
            else
            {
                tpl = WebUtils.LoadTemplate("SpecialDelete/MatchInfo.htm");
                foreach (MatchInfoRow row in lst)
                {
                    TemplateHandler.Assign(ref tpl, "MatchCode", row.matchcode);
                    TemplateHandler.Assign(ref tpl, "Home",
                                           CacheRef.GetTeamName(row.homeid) +
                                           (row.IsNeutral ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref tpl, "Away", CacheRef.GetTeamName(row.awayid));
                    TemplateHandler.Assign(ref tpl, "EventDate", row.eventdate);
                }
            }

            return tpl;
        }

        #endregion GetMatchInfoById

        #region AssignDatarow

        private string AssignDatarow(SpecialDeleteRow row, string transRowTpl, int index)
        {
            string transRow = transRowTpl;
            TemplateHandler.Assign(ref transRow, "{no}", index);
            TemplateHandler.Assign(ref transRow, "{Account}", row.username);
            TemplateHandler.Assign(ref transRow, "{Refno}", row.transid);
            TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row.bettype));
            TemplateHandler.Assign(ref transRow, "{Bet}", row.betteam);
            TemplateHandler.Assign(ref transRow, "{Odds}", FormatHandler.FormatNumber3(row.odds));
            TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row.oddstype));
            TemplateHandler.Assign(ref transRow, "{Amount}", FormatHandler.FormatNumber2(row.StakePositiontaking));
            TemplateHandler.Assign(ref transRow, "{currency}", CacheUtils.GetExchangeName(row.currency));
            switch (row.bettype)
            {
                case 1:
                case 3:
                case 7:
                case 8:
                case 22:
                    TemplateHandler.Assign(ref transRow, "{Hdp}", FormatHandler.FormatNumber2(row.hdp2 - row.hdp1));
                    break;

                default:
                    TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                    break;
            }
            TemplateHandler.Assign(ref transRow, "{TransTime}", row.transdate);
            TemplateHandler.Assign(ref transRow, "{HomeScore}", row.livehomescore);
            TemplateHandler.Assign(ref transRow, "{AwayScore}", row.liveawayscore);
            TemplateHandler.Assign(ref transRow, "{Creator}", row.creator);
            TemplateHandler.Assign(ref transRow, "{Status}", row.status);

            // _transListHtml.Append(transRow + "\n");
            return transRow;
        }

        #endregion AssignDatarow

        #region AssignDatarow

        private string AssignDatarow(SpecialDeleteRow row, string transRowTpl_SB, int index, SpecialBettype type)
        {
            string transRow = transRowTpl_SB;
            TemplateHandler.Assign(ref transRow, "{no}", index);
            TemplateHandler.Assign(ref transRow, "{Account}", row.username);
            TemplateHandler.Assign(ref transRow, "{Refno}", row.transid);
            TemplateHandler.Assign(ref transRow, "{Bettype}", CacheRef.GetBettype(row.bettype));
            TemplateHandler.Assign(ref transRow, "{Bet}", row.betteam);
            TemplateHandler.Assign(ref transRow, "{Odds}", FormatHandler.FormatNumber3(row.odds));
            TemplateHandler.Assign(ref transRow, "{OddsType}", CacheRef.GetOddsTypeName(row.oddstype));
            TemplateHandler.Assign(ref transRow, "{Amount}", FormatHandler.FormatNumber2(row.StakePositiontaking));
            TemplateHandler.Assign(ref transRow, "{currency}", CacheUtils.GetExchangeName(row.currency));
            TemplateHandler.Assign(ref transRow, "{SB}", GetSpecialBettype(type));
            switch (row.bettype)
            {
                case 1:
                case 3:
                case 7:
                case 8:
                case 22:
                    TemplateHandler.Assign(ref transRow, "{Hdp}", FormatHandler.FormatNumber2(row.hdp2 - row.hdp1));
                    break;

                default:
                    TemplateHandler.Assign(ref transRow, "{Hdp}", "--");
                    break;
            }
            TemplateHandler.Assign(ref transRow, "{TransTime}", row.transdate);
            TemplateHandler.Assign(ref transRow, "{HomeScore}", row.livehomescore);
            TemplateHandler.Assign(ref transRow, "{AwayScore}", row.liveawayscore);
            TemplateHandler.Assign(ref transRow, "{Creator}", row.creator);
            TemplateHandler.Assign(ref transRow, "{Status}", row.status);

            string IsDelete = "0";
            if (AppSession.Username == "joe" || AppSession.Username == "steven")
                IsDelete = "1";
            else if (row.status == "running")
                IsDelete = "1";
            TemplateHandler.Assign(ref transRow, "{IsDelete}", IsDelete);

            return transRow;
        }

        #endregion AssignDatarow
    }
}