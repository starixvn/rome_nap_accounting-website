// --------------------------------------------------------------------------
// <copyright file="TicketDetail.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.12.14">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Models
{
    using System;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class TicketDetail
    {
        public ITicket Ticket { get; set; }

        public string OddHtml { get; set; }

        public string ChoiceHtml { get; set; }

        public string StakeHtml { get; set; }

        public decimal VirtualRate { get; set; }

        public bool IsShowOtExRate { get; set; }

        public decimal AgentPositionTaking2 { get; set; }

        public decimal MasterPositionTaking2 { get; set; }

        public decimal SuperPositionTaking2 { get; set; }

        public string FinalHomeScore { get; set; }

        public string FinalAwayScore { get; set; }

        public string HTHomeScore { get; set; }

        public string HTAwayScore { get; set; }

        public string BetFrom { get; set; }

        public bool AllowDelete { get; set; }

        public string Creator { get; set; }

        public string StatusHtml { get; set; }

        public string TransDateHtml { get; set; }

        public string SportName { get; set; }

        public string LeagueName { get; set; }
    }
}