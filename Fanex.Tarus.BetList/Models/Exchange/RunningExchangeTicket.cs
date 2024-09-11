using Fanex.Tarus.BetList.Constants;
using System;
using Fanex.Tarus.BetList.Extensions;

namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class RunningExchangeTicket
    {

        public long BetId { get; set; }

        public int CategoryType { get; set; }

        public int EventId { get; set; }
        
        public string EventTypeName { get; set; }

        public string EventName { get; set; }

        public string MarketId { get; set; }

        public string MarketName { get; set; }

        public string SelectionName { get; set; }

        public string Type { get; set; }

        public DateTime BetPlaced { get; set; }

        public decimal OddsAvg { get; set; }

        public string OddsAvgText
            => CategoryType == ExchangeTypeConstants.Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsAvg)}" : OddsAvg.Format(2);

        public decimal FancyBetRuns { get; set; }

        public decimal Stake { get; set; }
    }
}
