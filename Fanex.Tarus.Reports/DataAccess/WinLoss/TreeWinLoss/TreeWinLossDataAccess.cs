// -----------------------------------------------------------------------
// <copyright file="TreeWinLossAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.WinLoss.TreeWinLoss
{
    using System.Collections;
    using System.Data;
    using DNAide.Core;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TreeWinLossDataAccess: ITreeWinLossDataAccess
    {

        public DataSet GetTreeWinLossReportByProducts(string fromDate,
                                                string toDate,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse,
                                                ref Hashtable outputPrams)
        {
            var spName = "Acc_Rpt_TreeWinlostByProduct_Old";
            using (ObjectDb objdb = new ObjectDb(spName, 300))
            {
                var param = new Hashtable();
                param.Add("fromdate", CastAide.AsDateTime(fromDate));
                param.Add("todate", CastAide.AsDateTime(toDate));
                param.Add("showsb", isShowSB);
                param.Add("showng", isShowNG);
                param.Add("showrb", isShowRB);
                param.Add("showlivecasino", isShowLCS);
                param.Add("showcasino", isShowCS);
                param.Add("showp2p", isShowP2P);
                param.Add("showbi", isShowBI);
                param.Add("showvs", isShowVS);
                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                outputPrams = objdb.GetOuputParams();
                return winloss;
            }
        }


        public DataSet GetTreeWinLossReportByCurrency(string fromDate,
                                                string toDate,
                                                int currency,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var spName = "Acc_Rpt_TreeWinlostByCom_Old";
            using (ObjectDb objdb = new ObjectDb(spName, 300))
            {
                var param = new Hashtable();
                //param.Add("winlostdate", CastAide.AsDateTime(fromDate));
                param.Add("fromdate", CastAide.AsDateTime(fromDate));
                param.Add("todate", CastAide.AsDateTime(toDate));
                param.Add("currency", currency);
                param.Add("showsb", isShowSB);
                param.Add("showng", isShowNG);
                param.Add("showrb", isShowRB);
                param.Add("showlivecasino", isShowLCS);
                param.Add("showcasino", isShowCS);
                param.Add("showp2p", isShowP2P);
                param.Add("showbi", isShowBI);
                param.Add("showvs", isShowVS);
                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                return winloss;
            }
        }

        public DataSet GetTreeWinLossReportBySuper(string fromDate,
                                                string toDate,
                                                int superId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var spName = "Acc_Rpt_TreeWinlostBySAg_Old";
            using (ObjectDb objdb = new ObjectDb(spName, 300))
            {
                var param = new Hashtable();
                //param.Add("winlostdate", CastAide.AsDateTime(fromDate));
                param.Add("fromdate", CastAide.AsDateTime(fromDate));
                param.Add("todate", CastAide.AsDateTime(toDate));
                param.Add("srecommend", superId);
                param.Add("showsb", isShowSB);
                param.Add("showng", isShowNG);
                param.Add("showrb", isShowRB);
                param.Add("showlivecasino", isShowLCS);
                param.Add("showcasino", isShowCS);
                param.Add("showp2p", isShowP2P);
                param.Add("showbi", isShowBI);
                param.Add("showvs", isShowVS);
                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                return winloss;
            }
        }

        public DataSet GetTreeWinLossReportByMaster(string fromDate,
                                                string toDate,
                                                int masterId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var spName = "Acc_Rpt_TreeWinlostByMAg_Old";
            using (ObjectDb objdb = new ObjectDb(spName, 300))
            {
                var param = new Hashtable();
                //param.Add("winlostdate", CastAide.AsDateTime(fromDate));
                param.Add("fromdate", CastAide.AsDateTime(fromDate));
                param.Add("todate", CastAide.AsDateTime(toDate));
                param.Add("mrecommend", masterId);
                param.Add("showsb", isShowSB);
                param.Add("showng", isShowNG);
                param.Add("showrb", isShowRB);
                param.Add("showlivecasino", isShowLCS);
                param.Add("showcasino", isShowCS);
                param.Add("showp2p", isShowP2P);
                param.Add("showbi", isShowBI);
                param.Add("showvs", isShowVS);
                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                return winloss;
            }
        }

        public DataSet GetTreeWinLossReportByAgent(string fromDate,
                                                string toDate,
                                                int agentId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS,
                                                bool isWarehouse)
        {
            var spName = "Acc_Rpt_TreeWinlostByAg_Old";
            using (ObjectDb objdb = new ObjectDb(spName, 300))
            {
                var param = new Hashtable();
                //param.Add("winlostdate", CastAide.AsDateTime(fromDate));
                param.Add("fromdate", CastAide.AsDateTime(fromDate));
                param.Add("todate", CastAide.AsDateTime(toDate));
                param.Add("recommend", agentId);
                param.Add("showsb", isShowSB);
                param.Add("showng", isShowNG);
                param.Add("showrb", isShowRB);
                param.Add("showlivecasino", isShowLCS);
                param.Add("showcasino", isShowCS);
                param.Add("showp2p", isShowP2P);
                param.Add("showbi", isShowBI);
                param.Add("showvs", isShowVS);
                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                return winloss;
            }
        }


        public DataSet GetVirtualRateUpdateHistory(int customerId)
        {
            using (ObjectDb objdb = new ObjectDb("Acc_CustomerSelVirtualRateHistory"))
            {
                var param = new Hashtable();
                param.Add("custid", customerId);

                objdb.AssignParamValues(param);
                var winloss = objdb.ExecuteDataSet();
                return winloss;
            }
        }
    }
}
