// -----------------------------------------------------------------------
// <copyright file="VirtualSportsWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Licensee.VirtualSportWinLoss
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class VirtualSportsWinLoss
    {
        public List<VirtualSportsWinLossByGroupLicensees> LicenseesWinLoss { get; set; }
        public VirtualSportsWinLossDetail GrandTotal { get; set; }
    }

    [Serializable]
    public class VirtualSportsWinLossByGroupLicensees
    {
        public VirtualSportsWinLossDetail RevenueInfo { get; set; }
        public List<VirtualSportsWinLossByLicensee> Licensees { get; set; }
    }

    [Serializable]
    public class VirtualSportsWinLossByLicensee
    {
        public VirtualSportsWinLossDetail WinLossTotal { get; set; }
        public List<VirtualSportsWinLossDetail> LicenseeWinLossDetail { get; set; }
    }

    [Serializable]
    public class VirtualSportsWinLossDetail
    {
        public int ResellerType { get; set; }
        public string Licensee { get; set; }
        public string Turnover { get; set; }
        public string TotalTransaction { get; set; }
        public string GrossWin { get; set; }
        public string DistributorRevenue { get; set; }
        public string ONEWorksIncome { get; set; }
        public string AmountDueToInspired { get; set; }
        public string AmountDueToOneWorks { get; set; }
    }
}
