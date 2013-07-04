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

        TreeWinLoss GetTreeWinLossReport(string fromDate,
                                              string toDate,
                                               int customID,
                                              bool isShowSB,
                                              bool isShowNG,
                                              bool isShowRB,
                                              bool isShowLCS,
                                              bool isShowCS,
                                              bool isShowP2P,
                                              bool isShowBI,
                                              bool isShowVS,
                                              int reportType);

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
