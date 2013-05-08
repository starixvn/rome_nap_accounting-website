// -----------------------------------------------------------------------
// <copyright file="ITreeWinLossAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.WinLoss.TreeWinLoss
{
    using System.Collections;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITreeWinLossDataAccess
    {
        DataSet GetTreeWinLossReportByProducts(string fromDate,
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
                                                ref Hashtable outputPrams);

        DataSet GetTreeWinLossReportByCurrency(string fromDate,
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
                                                bool isWarehouse);

        DataSet GetTreeWinLossReportBySuper(string fromDate,
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
                                                bool isWarehouse);

        DataSet GetTreeWinLossReportByMaster(string fromDate,
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
                                                bool isWarehouse);

        DataSet GetTreeWinLossReportByAgent(string fromDate,
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
                                                bool isWarehouse);

        DataSet GetVirtualRateUpdateHistory(int customerId);
    }
}
