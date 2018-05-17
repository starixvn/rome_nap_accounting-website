using System;

namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeDetailTicket
    {
        public int EventId { get; set; }

        public string MarketId { get; set; }

        public long BetId { get; set; }

        public string SelectionName { get; set; }

        public string Type { get; set; }

        public DateTime BetPlaced { get; set; }

        public decimal OddsAvg { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }        
    }
}
