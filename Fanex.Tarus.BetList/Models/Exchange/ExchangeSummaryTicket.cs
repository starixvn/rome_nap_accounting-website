using System;

namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeSummaryTicket
    {
        public int EventId { get; set; }
        
        public string EventTypeName { get; set; }

        public string EventName { get; set; }

        public string MarketId { get; set; }

        public string MarketName { get; set; }

        public decimal WinLost { get; set; }

        public decimal Commission { get; set; }

        public DateTime SettledDate { get; set; }
    }
}
