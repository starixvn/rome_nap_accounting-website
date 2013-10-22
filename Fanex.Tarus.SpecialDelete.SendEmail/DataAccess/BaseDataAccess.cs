using System.Data;
using Fanex.Data;
using Fanex.Tarus.SpecialDelete.SendEmail.Core;
using System.Collections;

namespace Fanex.Tarus.SpecialDelete.SendEmail.DataAccess
{
    public class BaseDataAccess
    {        

        public DataSet GetLeagues(string fdate, string tdate)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_Set_LeagueSel"))
            {
                objDB.AddInParameter("fromdate", DbType.DateTime, fdate);
                objDB.AddInParameter("todate", DbType.DateTime, tdate);
                objDB.AddInParameter("sporttype", DbType.Int32, 162);
                var ds = objDB.ExecuteDataSet();
                return ds;
            }
        }

        public string CheckMatchIDwithRefno(string MatchID, string FromRefNo, string ToRefNo, Sport.SportType sportType)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_SpecialDel_CheckMatchIDwithRefno"))
            {
                Hashtable h = new Hashtable();

                h.Add("matchid", MatchID);
                h.Add("fromrefno", FromRefNo);
                h.Add("torefno", ToRefNo);
                h.Add("type", sportType);
                h.Add("errcode", 0);
                h.Add("errmsg", "");

                objDB.AssignParamValues(h);
                
                objDB.ExecuteNonQuery();

                return DNAide.Core.CastAide.AsString(objDB.GetParameterValue("errmsg"));
            }
        }

        public DataSet SelExchangeCurrency()
        {
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeCurrencySel"))
            {
                var rs = objDB.ExecuteDataSet();
                return rs;
            }
        }

        public DataSet SelBettype(Product.ProductType productType)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_BetTypeSel"))
            {
                Hashtable h = new Hashtable();
                h.Add("mode", productType);
                objDB.AssignParamValues(h);

                var rs = objDB.ExecuteDataSet();
                return rs;
            }
        }

        /// <summary>
        /// SelNegativebalance
        /// </summary>
        /// <param name="transids">transids</param>
        /// <param name="actionType">actionType</param>
        /// <param name="excludeRunning">excludeRunning = true when deleting bets of LCS </param>
        /// <returns></returns>
        public DataSet SelNegativebalance(string transids, Function.ActionType actionType, bool excludeRunning)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_BettransCheckNegativeBetCredit"))
            {
                Hashtable h = new Hashtable();
                h.Add("transids", transids);
                h.Add("type", actionType);
                h.Add("excluderunning", excludeRunning);

                objDB.AssignParamValues(h);

                var rs = objDB.ExecuteDataSet();
                return rs;
            }
        }

        public DataTable  SelMailingList()
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
