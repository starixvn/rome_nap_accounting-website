using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.SpecialDelete.App_Templates.SpecialDelete;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.SpecialDelete;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for SpecialDelete
/// </summary>

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public class SpecialDelete : SpecialDeleteBase
    {
        #region GetSpecialDelete

        public string GetSpecialDelete(Hashtable param)
        {
           
            int matchid = 0;
            if (param.ContainsKey("matchid"))
                matchid = CastAide.AsInt32(param["matchid"]);
          
            if (!CheckMatchById(matchid))
            {
                return AlertMessage.GetMsg("NO MATCH WITH SUCH MATCH ID", AlertMessage.MsgType.Error); 
            }
            else
            {
                return BuildDeleteFormNew(param);   
            }
        }

        #endregion

        #region GetMatchInfoById

        private bool CheckMatchById(int matchid)
        {

            bool result = true;
            List<MatchInfoRow> lst = SpecialDeleteProvider.Instance(DataAccess.DataAccess.DAOClient.Entity).MatchSeById(matchid);
            if (lst.Count == 0)
            {
                result = false;
            }
          
            return result;
        }

        #endregion

        #region AssignDatarow

        private void AssignDatarow(SpecialDeleteRow row, int index,
                                   ref SpecialDeleteBets_Bets_NormalBets_ContentNomalBets_Block transrow_normal)
        {
            transrow_normal.no = index.ToString();
            transrow_normal.Account = row.username;
            transrow_normal.Refno = row.transid;
            transrow_normal.Bettype = CacheRef.GetBettype(row.bettype);
            transrow_normal.Bet = row.betteam;
            transrow_normal.Odds = FormatHandler.FormatNumber3(row.odds);
            transrow_normal.OddsType = CacheRef.GetOddsTypeName(row.oddstype);
            transrow_normal.Amount = FormatHandler.FormatNumber2(row.StakePositiontaking);
            transrow_normal.currency = CacheUtils.GetExchangeName(row.currency);
            switch (row.bettype)
            {
                case 1:
                case 3:
                case 7:
                case 8:
                case 22:
                    transrow_normal.Hdp = FormatHandler.FormatNumber2(row.hdp2 - row.hdp1);
                    break;
                default:
                    transrow_normal.Hdp = "--";
                    break;
            }

            transrow_normal.TransTime = row.transdate.ToString();
            transrow_normal.HomeScore = row.livehomescore.ToString();
            transrow_normal.AwayScore = row.liveawayscore.ToString();
            transrow_normal.Creator = row.creator;
            transrow_normal.Status = row.status;

            //return transrow_normal;
        }

        #endregion

        #region AssignDatarow

        private void AssignDatarow(SpecialDeleteRow row, int index, SpecialBettype type,
                                   ref SpecialDeleteBets_Bets_SpecialBets_ContentSpecialBets_Block transRowTpl_SB)
        {
            transRowTpl_SB.no = index.ToString();
            transRowTpl_SB.Account = row.username;
            transRowTpl_SB.Refno = row.transid;
            transRowTpl_SB.Bettype = CacheRef.GetBettype(row.bettype);
            transRowTpl_SB.Bet = row.betteam;
            transRowTpl_SB.Odds = FormatHandler.FormatNumber3(row.odds);
            transRowTpl_SB.OddsType = CacheRef.GetOddsTypeName(row.oddstype);
            transRowTpl_SB.Amount = FormatHandler.FormatNumber2(row.StakePositiontaking);
            transRowTpl_SB.currency = CacheUtils.GetExchangeName(row.currency);
            transRowTpl_SB.SB = GetSpecialBettype(type);
            switch (row.bettype)
            {
                case 1:
                case 3:
                case 7:
                case 8:
                case 22:
                    transRowTpl_SB.Hdp = FormatHandler.FormatNumber2(row.hdp2 - row.hdp1);
                    break;
                default:
                    transRowTpl_SB.Hdp = "--";
                    break;
            }
            transRowTpl_SB.TransTime = row.transdate.ToString();
            transRowTpl_SB.HomeScore = row.livehomescore.ToString();
            transRowTpl_SB.AwayScore = row.liveawayscore.ToString();
            transRowTpl_SB.Creator = row.creator;
            transRowTpl_SB.Status = row.status;

            string IsDelete = "0";
            if (AppSession.Username == "joe" || AppSession.Username == "steven")
                IsDelete = "1";
            else if (row.status == "running")
                IsDelete = "1";
            transRowTpl_SB.IsDelete = IsDelete;
        }

        #endregion

        #region Mapping

        private SpecialDeleteRow Mapping(DataRow dr)
        {
            var row = new SpecialDeleteRow();
            row.username = CastAide.AsString(dr["username"]);
            row.transid = CastAide.AsString(dr["transid"]);
            row.bettype = CastAide.AsInt32(dr["bettype"]);
            row.betteam = CastAide.AsString(dr["betteam"]);
            row.odds = CastAide.AsDecimal(dr["odds"]);
            row.oddstype = CastAide.AsInt32(dr["oddstype"]);
            row.StakePositiontaking = CastAide.AsDecimal(dr["stake"]);
            row.currency = CastAide.AsInt32(dr["currency"]);
            row.hdp1 = CastAide.AsDecimal(dr["hdp1"]);
            row.hdp2 = CastAide.AsDecimal(dr["hdp2"]);
            row.transdate = CastAide.AsDateTime(dr["transdate"]);
            row.livehomescore = CastAide.AsInt32(dr["livehomescore"]);
            row.liveawayscore = CastAide.AsInt32(dr["liveawayscore"]);
            row.creator = CastAide.AsString(dr["creator"]);
            row.status = CastAide.AsString(dr["status"]);
            return row;
        }

        #endregion

        #region BuildDeleteFormNew

        private string BuildDeleteFormNew(Hashtable param)
        {
            try
            {
                string hdp = CastAide.AsString(param["hdp"]);
                string bettypename = CastAide.AsString(param["typename"]);
                string frefno = CastAide.AsString(param["fromrefno"]);
                string trefno = CastAide.AsString(param["torefno"]);
                string matchId = CastAide.AsString(param["matchid"]);
                string betTypes = CastAide.AsString(param["bettype"]);
                string betteam1378 = CastAide.AsString(param["betteam1378"]);
                string betteam515 = CastAide.AsString(param["betteam515"]);
                string betteam4 = CastAide.AsString(param["betteam4"]);
                string liveindicator = CastAide.AsString(param["liveindicator"]);

                var tpl = new DeleteFormNew_Template();
                hdp = (string.IsNullOrEmpty(hdp)) ? "N/A" : hdp;

                tpl.FromRefno = frefno;
                tpl.ToRefno = trefno;
                tpl.MatchId = matchId;
                tpl.Bettype = betTypes;
                tpl.Betteam1378 = betteam1378;
                tpl.Betteam515 = betteam515;
                tpl.Hdp = hdp;
                tpl.liveindicator = liveindicator;
                tpl.Betteam4 = betteam4;

                return tpl.ToString();
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return string.Empty;
        }

        #endregion

        #region GetHeaderSpecialBets

        private void GetHeaderSpecialBets(string name, string color, SpecialBettype type,
                                          ref SpecialDeleteBets_Bets_SpecialBets_HeaderSpecialBets_Block header)
        {
            header.SB_Name = name;
            header.SB_Color = color;
            header.SB = GetSpecialBettype(type);
        }

        #endregion
    }
}