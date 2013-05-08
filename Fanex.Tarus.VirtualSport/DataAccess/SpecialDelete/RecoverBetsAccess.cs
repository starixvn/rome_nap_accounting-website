

using System.Collections;
using System.Data;
using Fanex.Data;

namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public class RecoverBetsAccess : IRecoverBetsAccess
    {
        #region IFields

        private int _stringLength = 10000;
        private int _virtualSportType = 1;

        #endregion IFields
        #region IMethods
        #region RecoverBets

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

        public DataSet SelRecoverSpecialBets(string transid, ref Hashtable outputParam)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("BettransSelRecoverSpecialBets"))
            {
                objDB.AddInParameter("type", DbType.Int64, _virtualSportType);
                objDB.AddInParameter("transid", DbType.Int64, transid);
                objDB.AddOutParameter("errcode", DbType.Int64, (int)DbType.Int64);
                objDB.AddOutParameter("errmsg", DbType.String, _stringLength);
                result = objDB.ExecuteDataSet();
                outputParam = objDB.GetOuputParams();
            }

            return result;
        }

        public DataSet RecoverSpecialBets(string transid, int userId, string userIp, ref Hashtable outputParam)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("Acc_BettransRecoverSpecialBets"))
            {
                objDB.AddInParameter("type", DbType.Int64, _virtualSportType);
                objDB.AddInParameter("transid", DbType.Int64, transid);
                objDB.AddInParameter("userid", DbType.Decimal, userId);
                objDB.AddInParameter("ip", DbType.String, userIp);
                objDB.AddOutParameter("errcode", DbType.Int64, (int)DbType.Int64);
                objDB.AddOutParameter("errmsg", DbType.String, _stringLength);
                result = objDB.ExecuteDataSet();
                outputParam = objDB.GetOuputParams();
            }

            return result;

        }

        public DataSet RemovePrivateMsg(string transid, int userId)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("Acc_Msg_Remove"))
            {
                objDB.AddInParameter("transid", DbType.Decimal, transid);
                objDB.AddInParameter("userid", DbType.Decimal, userId);
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        #endregion RecoverBets
        #endregion IMethods
    }
}
