using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.SpecialDelete.App_Templates.SpecialDelete;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.SpecialDelete;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;

/// <summary>
/// Summary description for SpecialDelete_Outright
/// </summary>

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public class SpecialDelete_Outright : SpecialDeleteBase
    {
        #region GetOutrightByLeagueID

        public string GetOutrightByLeagueID(int LeagueID)
        {
            DbResult rs =
                SpecialDeleteProvider.Instance(DataAccess.DataAccess.DAOClient.AdoNet).BettransDelSpecial_OutrightSel(LeagueID);
            DataRow[] Rows = rs.ResultSet.Tables[0].Select();
            string op = string.Empty;
            var Options = new StringBuilder();
            Options.Append("<select  id='ddlOutright' name='ddlOutright'>");
            foreach (DataRow dr in Rows)
            {
                op = "<option value='" + dr["orid"] + "'>" + CacheRef.GetTeamName(dr["TeamID"]) + "</option>";
                Options.Append(op);
            }
            Options.Append("</select>");
            return Options.ToString();
        }

        #endregion GetOutrightByLeagueID

        #region GetSpecialDelete

        public string GetSpecialDelete(Hashtable param)
        {
            return GeneralBets(param);
        }

        #endregion GetSpecialDelete

        #region GeneralBets

        private string GeneralBets(Hashtable param)
        {
            var orlist = new OutrightList_Template();
            orlist.delete.Username = AppSession.AccUserName;
            orlist.bets.NormalContent.Clear();
            orlist.bets.SpecialContent.Header.Clear();
            orlist.bets.SpecialContent.Content.Clear();

            var transrow_normal = new OutrightList_bets_NormalContent_Block();
            var transRowTpl_SB = new OutrightList_bets_SpecialContent_Content_Block();
            var header = new OutrightList_bets_SpecialContent_Header_Block();

            DbResult rs;
            rs = SpecialDeleteProvider.Instance(DataAccess.DataAccess.DAOClient.AdoNet).BettransSelDelSpecial_Outright(param);

            DataTableCollection lst = rs.ResultSet.Tables;
            int index = 0;
            int countTbl = 0;
            int cntNormalBet = 0, cntDeposit = 0, cntBetDaq = 0, cnt12EU = 0, cntMasterMHC = 0;

            cntNormalBet = rs.ResultSet.Tables[0].Rows.Count; //normal bet
            cntDeposit = rs.ResultSet.Tables[1].Rows.Count; //deposit bet
            cntBetDaq = rs.ResultSet.Tables[2].Rows.Count; //bet daq
            cnt12EU = rs.ResultSet.Tables[3].Rows.Count; //bet 12EU
            cntMasterMHC = rs.ResultSet.Tables[4].Rows.Count; //bet master MHC

            orlist.delete.rowcount = cntNormalBet.ToString();
            orlist.delete.rowcountDeposit = cntDeposit.ToString();
            orlist.delete.rowcountBD = cntBetDaq.ToString();
            orlist.delete.rowcount12EU = cnt12EU.ToString();
            orlist.delete.rowcountMHC = cntMasterMHC.ToString();

            if (cntNormalBet == 0 && cntDeposit == 0 && cntBetDaq == 0 && cnt12EU == 0 && cntMasterMHC == 0)
            {
                orlist.delete.Clear();
                orlist.bets.NormalHeader.DeleteOption.Clear();
                transrow_normal.DeleteOption.Clear();
            }
            else if (cntNormalBet > 0 && cntDeposit == 0 && cntBetDaq == 0 && cnt12EU == 0 && cntMasterMHC == 0)
            {
                orlist.bets.noinfo.Clear();
                orlist.bets.NormalHeader.DeleteOption.Clear();
                transrow_normal.DeleteOption.Clear();
            }
            else
            {
                orlist.bets.noinfo.Clear();
            }

            foreach (DataTable item in lst)
            {
                DataRow[] drs = item.Select();
                foreach (DataRow dr in drs)
                {
                    index++;

                    SpecialDeleteRow row = Mapping(dr);

                    if (countTbl == 0) //NormalBet
                    {
                        AssignDatarow(row, index, ref transrow_normal);
                        orlist.bets.NormalContent.Append(transrow_normal);
                    }
                    else if (countTbl == 1) //Deposit
                    {
                        if (index == 1)
                        {
                            GetHeaderSP("Deposit", "#6495ed", SpecialBettype.BetDeposit, ref header);
                            orlist.bets.SpecialContent.Append(header);
                        }
                        AssignDatarow(row, index, SpecialBettype.BetDeposit, ref transRowTpl_SB);
                        orlist.bets.SpecialContent.Append(transRowTpl_SB);
                    }
                    else if (countTbl == 2) //BetDaq
                    {
                        if (index == 1)
                        {
                            GetHeaderSP("Bet Daq", "#659E17", SpecialBettype.Betdaq, ref header);
                            orlist.bets.SpecialContent.Append(header);
                        }
                        AssignDatarow(row, index, SpecialBettype.Betdaq, ref transRowTpl_SB);
                        orlist.bets.SpecialContent.Append(transRowTpl_SB);
                    }
                    else if (countTbl == 3) //12EU
                    {
                        if (index == 1)
                        {
                            GetHeaderSP("12EU", "#cc9900", SpecialBettype.Bet12EU, ref header);
                            orlist.bets.SpecialContent.Append(header);
                        }
                        AssignDatarow(row, index, SpecialBettype.Bet12EU, ref transRowTpl_SB);
                        orlist.bets.SpecialContent.Append(transRowTpl_SB);
                    }
                    else if (countTbl == 4) //member of master: MHCEU (105181), MHCRM (118473), MHCUS(105183)
                    {
                        if (index == 1)
                        {
                            GetHeaderSP("Members belong to Master MHCEU, MHCRM and MHCUS", "#ff8040",
                                        SpecialBettype.BetMasterMHC, ref header);
                            orlist.bets.SpecialContent.Append(header);
                        }

                        AssignDatarow(row, index, SpecialBettype.BetMasterMHC, ref transRowTpl_SB);
                        orlist.bets.SpecialContent.Append(transRowTpl_SB);
                    }
                }

                index = 0;
                countTbl++;
            }
            return orlist.ToString();
        }

        #endregion GeneralBets

        #region Mapping

        private SpecialDeleteRow Mapping(DataRow dr)
        {
            var row = new SpecialDeleteRow();
            row.username = CastAide.AsString(dr["username"]);
            row.transid = CastAide.AsString(dr["transid"]);
            row.odds = CastAide.AsDecimal(dr["odds"]);
            row.currency = CastAide.AsInt32(dr["currency"]);
            row.transdate = CastAide.AsDateTime(dr["transdate"]);
            row.creator = CastAide.AsString(dr["creator"]);
            row.status = CastAide.AsString(dr["status"]);
            //row.Leagueid = CastAide.AsInt32(dr["leagueid"]);
            //row.Teamid= CastAide.AsInt32(dr["teamid"]);
            row.StakePositiontaking = CastAide.AsDecimal(dr["stake"]);

            return row;
        }

        #endregion Mapping

        #region AssignDatarow

        private void AssignDatarow(SpecialDeleteRow row, int index,
                                   ref OutrightList_bets_NormalContent_Block transrow_normal)
        {
            transrow_normal.no = index.ToString();
            transrow_normal.Account = row.username;
            transrow_normal.Refno = row.transid;
            transrow_normal.Odds = IBCUtils.FormatHandler.FormatNumber3(row.odds);
            transrow_normal.currency = CacheUtils.GetExchangeName(row.currency);
            transrow_normal.TransTime = row.transdate.ToString();
            transrow_normal.Creator = row.creator;
            transrow_normal.Status = row.status;
            //transrow_normal.LeagueName = CacheRef.GetLeagueName( row.Leagueid);
            //transrow_normal.TeamName = CacheRef.GetTeamName(row.Teamid);
            transrow_normal.Amount = NumberFormat.DecFormat(row.StakePositiontaking, 2);
        }

        #endregion AssignDatarow

        #region AssignDatarow

        private void AssignDatarow(SpecialDeleteRow row, int index, SpecialBettype type,
                                   ref OutrightList_bets_SpecialContent_Content_Block transRowTpl_SB)
        {
            transRowTpl_SB.no = index.ToString();
            transRowTpl_SB.Account = row.username;
            transRowTpl_SB.Refno = row.transid;
            transRowTpl_SB.Odds = IBCUtils.FormatHandler.FormatNumber3(row.odds);
            transRowTpl_SB.currency = CacheUtils.GetExchangeName(row.currency);
            transRowTpl_SB.SB = GetSpecialBettype(type);
            transRowTpl_SB.TransTime = row.transdate.ToString();
            transRowTpl_SB.Creator = row.creator;
            transRowTpl_SB.Status = row.status;
            //transRowTpl_SB.LeagueName = CacheRef.GetLeagueName(row.Leagueid);
            //transRowTpl_SB.TeamName = CacheRef.GetTeamName(row.Teamid);
            transRowTpl_SB.Amount = NumberFormat.DecFormat(row.StakePositiontaking, 2);

            string IsDelete = "0";
            if (AppSession.Username == "joe" || AppSession.Username == "steven")
                IsDelete = "1";
            else if (row.status == "running")
                IsDelete = "1";
            transRowTpl_SB.IsDelete = IsDelete;
        }

        #endregion AssignDatarow

        #region _OutrightList_bets_SpecialContent_Header

        private void GetHeaderSP(string name, string color, SpecialBettype type,
                                 ref OutrightList_bets_SpecialContent_Header_Block header)
        {
            header.SB_Name = name;
            header.SB_Color = color;
            header.SB = GetSpecialBettype(type);
        }

        #endregion _OutrightList_bets_SpecialContent_Header
    }
}