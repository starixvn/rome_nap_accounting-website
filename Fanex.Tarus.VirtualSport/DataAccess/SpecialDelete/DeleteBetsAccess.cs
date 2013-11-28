

using System.Collections;
using System.Data;
using DNAide.Core;
using DNAide.Utils;
using Fanex.Data;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;

namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public class DeleteBetsAccess : IDeleteBetsAccess
    {
        private int _stringLength = 10000;
        #region IMethods
        #region SpecialDelete

        public DataSet SelBettypes(int type)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("Acc_BetTypeSel"))
            {
                objDB.AddInParameter("mode", DbType.Int64, type);
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        public DataSet SelMatchInfo(int matchId, long transid)
        {
            DataSet result = null;
            using (ObjectDb objDB = new ObjectDb("SelMatchInfo"))
            {
                objDB.AddInParameter("matchid", DbType.Int64, matchId);
                objDB.AddInParameter("transid", DbType.Int64, transid);
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        public DataSet BettransSelSpecialDel(SpecialDeleteParam deleteParam)
        {
            DataSet result = null;
            using (ObjectDb objDB = new ObjectDb("BettransSelSpecialDel_VirtualSports"))
            {
                objDB.AddInParameter("matchid", DbType.Int64, CastAide.AsInt64(deleteParam.MatchId));
                objDB.AddInParameter("fromrefno", DbType.Int64, CastAide.AsInt64(deleteParam.FromRefNo));
                objDB.AddInParameter("torefno", DbType.Int64, CastAide.AsInt64(deleteParam.ToRefNo));
                objDB.AddInParameter("hdp", DbType.Decimal, CastAide.AsDecimal(deleteParam.Hdp));
                //objDB.AddInParameter("liveindicator", DbType.String, deleteParam.Liveindicator);
                objDB.AddInParameter("bettypes", DbType.String, deleteParam.Bettypes);
                if (deleteParam.Betteam1378 == null)
                {
                    deleteParam.Betteam1378 = string.Empty;
                }

                if (deleteParam.Betteam515 == null)
                {
                    deleteParam.Betteam515 = string.Empty;
                }
                if (deleteParam.Betteam430 == null)
                {
                    deleteParam.Betteam430 = string.Empty;
                }
                objDB.AddInParameter("betteam1201", DbType.String, deleteParam.Betteam1378);
                objDB.AddInParameter("betteam1205", DbType.String, deleteParam.Betteam515);
                objDB.AddInParameter("betteam1204", DbType.String, deleteParam.Betteam430);
                objDB.AddInParameter("username", DbType.String, deleteParam.UserName);
                //adding filter option
                if (deleteParam.Betteam6 == null)
                {
                    deleteParam.Betteam6 = string.Empty;
                }
                objDB.AddInParameter("betteam1206", DbType.String, deleteParam.Betteam6);
                //objDB.AddInParameter("betteam14", DbType.String, deleteParam.Betteam14);
                //objDB.AddInParameter("betteam16", DbType.String, deleteParam.Betteam16);
                //objDB.AddInParameter("safilter", DbType.Int64, CastAide.AsInt32(deleteParam.SAFilter));
                //var minusHours = -12;
                //if (!string.IsNullOrEmpty(deleteParam.HomeScore) && !string.IsNullOrEmpty(deleteParam.AwayScore))
                //{
                //    objDB.AddInParameter("livehomescore", DbType.String, deleteParam.HomeScore);
                //    objDB.AddInParameter("liveawayscore", DbType.String, deleteParam.AwayScore);
                //}
                //if (!string.IsNullOrEmpty(deleteParam.FromTransdate) && !string.IsNullOrEmpty(deleteParam.ToTransdate))
                //{
                //    objDB.AddInParameter("fromtransdate", DbType.DateTime,
                //       CastAide.AsDateTime(deleteParam.FromTransdate).AddHours(minusHours));
                //    objDB.AddInParameter("totransdate", DbType.DateTime,
                //        CastAide.AsDateTime(deleteParam.ToTransdate).AddHours(minusHours));
                //}                    
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        public DataSet DeleteSpecialTransaction(SpecialDeleteParam deleteParam, ref Hashtable outputParam)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("BettransDelSpecial_VirtualSports"))
            {
                objDB.AddInParameter("matchid", DbType.Int64, CastAide.AsInt64(deleteParam.MatchId));
                objDB.AddInParameter("f_transid", DbType.Int64, CastAide.AsInt64(deleteParam.FromRefNo));
                objDB.AddInParameter("t_transid", DbType.Int64, CastAide.AsInt64(deleteParam.ToRefNo));
                objDB.AddInParameter("bettypes", DbType.String, deleteParam.Bettypes);
                if (deleteParam.Betteam1378 == null)
                {
                    deleteParam.Betteam1378 = string.Empty;
                }

                if (deleteParam.Betteam515 == null)
                {
                    deleteParam.Betteam515 = string.Empty;
                }
                if (deleteParam.Betteam430 == null)
                {
                    deleteParam.Betteam430 = string.Empty;
                }
                objDB.AddInParameter("betteam1201", DbType.String, deleteParam.Betteam1378);
                objDB.AddInParameter("betteam1205", DbType.String, deleteParam.Betteam515);
                objDB.AddInParameter("betteam1204", DbType.String, deleteParam.Betteam430);
                objDB.AddInParameter("hdp", DbType.Decimal, CastAide.AsDecimal(deleteParam.Hdp));
                objDB.AddInParameter("username", DbType.String, deleteParam.UserName);

                objDB.AddInParameter("userpwd", DbType.String, Encryption.CfsEnCode(deleteParam.UserPassword));
                objDB.AddInParameter("userid", DbType.Int64, deleteParam.UserId);

                objDB.AddInParameter("deloption", DbType.Int32, deleteParam.DelOptions);
                if (deleteParam.TransIds != null)
                {
                    objDB.AddInParameter("transids", DbType.String, deleteParam.TransIds);
                }

                objDB.AddInParameter("deloption_bd", DbType.Int32, deleteParam.DelOption_BD);
                if (deleteParam.TransIds_BD != null)
                {
                    objDB.AddInParameter("transids_bd", DbType.String, deleteParam.TransIds_BD);
                }
                objDB.AddInParameter("deloption_12eu", DbType.Int32, deleteParam.DelOption_12EU);
                if (deleteParam.TransIds_12EU != null)
                {
                    objDB.AddInParameter("transids_12eu", DbType.String, deleteParam.TransIds_12EU);
                }
                objDB.AddInParameter("deloption_mhc", DbType.Int32, deleteParam.DelOption_MHC);
                if (deleteParam.TransIds_MHC != null)
                {
                    objDB.AddInParameter("transids_mhc", DbType.String, deleteParam.TransIds_MHC);
                }

                //objDB.AddInParameter("liveindicator", DbType.Int32, CastAide.AsInt32(deleteParam.LiveIndicator, 2));
                objDB.AddInParameter("ip", DbType.String, deleteParam.UserIp);


                objDB.AddOutParameter("errcode", DbType.Int64, (int)DbType.Int64);
                objDB.AddOutParameter("errmsg", DbType.String, _stringLength);

                //adding filter option
                if (deleteParam.Betteam6 == null)
                {
                    deleteParam.Betteam6 = string.Empty;
                }
                objDB.AddInParameter("betteam1206", DbType.String, deleteParam.Betteam6);
                //objDB.AddInParameter("betteam14", DbType.String, deleteParam.Betteam14);
                //objDB.AddInParameter("betteam16", DbType.String, deleteParam.Betteam16);
                //objDB.AddInParameter("safilter", DbType.Int64, deleteParam.SAFilter);

                //var minusHours = -12;
                //if (!string.IsNullOrEmpty(deleteParam.HomeScore) && !string.IsNullOrEmpty(deleteParam.AwayScore))
                //{
                //    objDB.AddInParameter("livehomescore", DbType.String, deleteParam.HomeScore);
                //    objDB.AddInParameter("liveawayscore", DbType.String, deleteParam.AwayScore);
                //}
                //if (!string.IsNullOrEmpty(deleteParam.FromTransDate) && !string.IsNullOrEmpty(deleteParam.ToTransDate))
                //{
                //    objDB.AddInParameter("fromtransdate", DbType.DateTime,
                //        CastAide.AsDateTime(deleteParam.FromTransDate).AddHours(minusHours));
                //    objDB.AddInParameter("totransdate", DbType.DateTime,
                //        CastAide.AsDateTime(deleteParam.ToTransDate).AddHours(minusHours));
                //}

                result = objDB.ExecuteDataSet();
                outputParam = objDB.GetOuputParams();
            }

            return result;
        }

        #endregion SpecialDelete
        #endregion IMethods
    }
}
