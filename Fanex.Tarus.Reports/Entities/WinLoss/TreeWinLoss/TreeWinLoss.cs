// -----------------------------------------------------------------------
// <copyright file="TreeWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.WinLoss.TreeWinLoss
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class TreeWinLoss
    {
        public List<TreeWinLossDetail> WinLoss { get; set; }
        public TreeWinLossDetail Total { get; set; }
        public TreeWinLossDetail Total8RM { get; set; }
        public TreeWinLossDetail TotalBARM { get; set; }
        public TreeWinLossDetail GrandTotal { get; set; }
        public TreeWinLoss()
        {
            Total = new TreeWinLossDetail();
            Total8RM = new TreeWinLossDetail();
            TotalBARM = new TreeWinLossDetail();
            GrandTotal = new TreeWinLossDetail();
            WinLoss = new List<TreeWinLossDetail>();
        }
    }

    [Serializable]
    public class TreeWinLossDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double Turnover { get; set; }
        public double WinLoss { get; set; }
        public double Comm { get; set; }
        public double Total { get; set; }
        public double BaseRMTurnover { get; set; }
        public double BaseRMTotal { get; set; }
        public double BaseRMVirtualTotal { get; set; }
        public double BaseRMVariance { get; set; }
        public double VirtualRate { get; set; }
        public double Margin { get; set; }
    }
        
    public class HistoryDetail
    {
        public string UpdatedDate { get; set; }
        public string Value { get; set; }
        public string ChangedBy { get; set; }
    }

}
