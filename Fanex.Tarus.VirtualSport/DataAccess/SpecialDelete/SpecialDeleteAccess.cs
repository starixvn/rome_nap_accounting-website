

using System.Collections;
using System.Data;
using Fanex.Data;

namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public class SpecialDeleteAccess : ISpecialDeleteAccess
    {
        #region IFileds

        private int _stringLength = 10000;
        private int _virtualSportType = 1;

        #endregion IFileds
        #region IMethods
        #region SpecialDelete   

        public DataSet CheckRefno(string matchid, string fromRefno, string toRefno, ref Hashtable outputParam)
        {
            DataSet result = null;

            using (ObjectDb objDB = new ObjectDb("Acc_SpecialDel_CheckMatchIDwithRefno"))
            {
                objDB.AddInParameter("type", DbType.Int64, _virtualSportType);
                objDB.AddInParameter("matchid", DbType.Int64, matchid);
                objDB.AddInParameter("fromrefno", DbType.Int64, fromRefno);
                objDB.AddInParameter("torefno", DbType.Int64, toRefno);
                objDB.AddOutParameter("errcode", DbType.Int64, (int)DbType.Int64);
                objDB.AddOutParameter("errmsg", DbType.String, _stringLength);
                result = objDB.ExecuteDataSet();
                outputParam = objDB.GetOuputParams();
            }

            return result;
        }

        #endregion SpecialDelete
        #endregion IMethods


        public DataTable SelMailingList()
        {
            DataTable dt = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_MailingListSel"))
            {
                var rs = objDB.ExecuteDataSet();
                if (rs.Tables.Count > 0)
                    dt = rs.Tables[0];
            }

            return dt;
        }
    }
}
