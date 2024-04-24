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
    using System.Resources;
    using Fanex.BetList.Core.Entities;
    using Fanex.Tarus.BetList.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class BookedBet
    {
        public ITicket Ticket { get; set; }

        public string TransDateHtml { get; set; }

        public string StatusHtml { get; set; }

        public string OddsValue { get; set; }

        public string OddsType { get; set; }

        public string ChoiceHtml { get; set; }

        public string StakeHtml { get; set; }

        public string Country { get; set; }

        public string Reason { get; set; }

        public string ReasonText { get; set; }

        public decimal OddsLower { get; set; }

        public decimal OddsUpper { get; set; }
    }
}