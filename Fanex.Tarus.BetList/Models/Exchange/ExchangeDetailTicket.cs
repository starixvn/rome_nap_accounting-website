using System;
using Fanex.Tarus.BetList.Extensions;

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

        public string OddsAvgText
        {
            get
            {
                const int Fancybet = 2;
                     
                return CategoryType == Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsAvg)}": OddsAvg.Format(2);
            }
        }
        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }        

        public int CategoryType { get; set; }

        public decimal FancyBetRuns { get; set; }
    }
}
