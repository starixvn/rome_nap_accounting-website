// -----------------------------------------------------------------------
// <copyright file="BetCount.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Others
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SiteTicket
    {
        public string SiteName { get; set; }
        public string NumOfTicket { get; set; }
        public string TotalTurnover { get; set; }
        public string MemberWinloss { get; set; }
        public string MarginPercent { get; set; }
        public string AgentWinLoss { get; set; }
        public string MasterWinLoss { get; set; }
        public string SuperWinLoss { get; set; }
        public string Company { get; set; }
    }

    [Serializable]
    public class SiteTicketGroup
    {
        public SiteTicket TotalSiteTicket { get; set; }
        public List<SiteTicket> SiteTickets { get; set; }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class BetCount
    {
        public bool HasData { get; set; }
        public SiteTicket TotalBetCount { get; set; }
        public List<SiteTicketGroup> SiteTicketGroups { get; set; }
    }

    [Serializable]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
