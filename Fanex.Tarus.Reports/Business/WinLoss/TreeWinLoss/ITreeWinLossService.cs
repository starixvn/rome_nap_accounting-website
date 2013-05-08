// -----------------------------------------------------------------------
// <copyright file="ITreeWinLossService.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.WinLoss.TreeWinLoss
{
    using System.Collections.Generic;
    using Fanex.Tarus.Reports.Entities.WinLoss.TreeWinLoss;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITreeWinLossService
    {
        TreeWinLoss GetTreeWinLossReportByProduct(string fromDate,
                                                    string toDate,
                                                    bool isShowSB,
                                                    bool isShowNG,
                                                    bool isShowRB,
                                                    bool isShowLCS,
                                                    bool isShowCS,
                                                    bool isShowP2P,
                                                    bool isShowBI,
                                                    bool isShowVS);

        TreeWinLoss GetTreeWinLossReportByCurrency(string fromDate,
                                                string toDate,
                                                int currency,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS);

        TreeWinLoss GetTreeWinLossReportBySuper(string fromDate,
                                                string toDate,
                                                int superId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS);

        TreeWinLoss GetTreeWinLossReportByMaster(string fromDate,
                                                string toDate,
                                                int masterId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS);

        TreeWinLoss GetTreeWinLossReportByAgent(string fromDate,
                                                string toDate,
                                                int agentId,
                                                bool isShowSB,
                                                bool isShowNG,
                                                bool isShowRB,
                                                bool isShowLCS,
                                                bool isShowCS,
                                                bool isShowP2P,
                                                bool isShowBI,
                                                bool isShowVS);

        IList<HistoryDetail> GetVirtualRateUpdateHistory(int customerId);

        void ExportTreeWinLossReportToExcelFile(string reportTitle,
                                            int reportType,
                                            string fromDate,
                                            string toDate,
                                            int uplineId,
                                            string uplineName,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS);

        TreeWinLoss SortingTreeWinLossReport(string sortCol, string sortType);
    }
}
