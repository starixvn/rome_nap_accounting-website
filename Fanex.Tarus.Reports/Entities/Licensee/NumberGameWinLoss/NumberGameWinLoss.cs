// -----------------------------------------------------------------------
// <copyright file="NumberGameWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Licensee.NumberGameWinLoss
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class NumberGameWinLoss
    {
        public List<NumberGameWinLossByGroupLicensees> LicenseesWinLoss { get; set; }
        public NumberGameWinLossDetail GrandTotal { get; set; }
    }

    [Serializable]
    public class NumberGameWinLossByGroupLicensees
    {
        public NumberGameWinLossDetail RevenueInfo { get; set; }
        public List<NumberGameWinLossByLicensee> Licensees { get; set; }
    }

    [Serializable]
    public class NumberGameWinLossByLicensee
    {
        public NumberGameWinLossDetail WinLossTotal { get; set; }
        public List<NumberGameWinLossDetail> LicenseeWinLossDetail { get; set; }
    }

    [Serializable]
    public class NumberGameWinLossDetail
    {
        public int ResellerType { get; set; }
        public string Licensee { get; set; }
        public string Turnover { get; set; }
        public string TotalTransaction { get; set; }
        public string GrossWin { get; set; }
        public string DistributorRevenue { get; set; }
        public string LupineRevenue { get; set; }
        public string AmountDueToOneWorks { get; set; }
    }
}
