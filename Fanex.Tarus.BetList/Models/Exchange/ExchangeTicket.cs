using System;
using Fanex.Tarus.BetList.Extensions;
namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeTicket
    {
        public ExchangeTicket()
        {

        }

        const int Fancybet = 2;

        public DateTime BetTaken { get; set; }

        public decimal OddsRequest { get; set; }

        public string OddsRequestText
            => CategoryType == Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsRequest)}" : OddsRequest.Format(2);

        public decimal Stake { get; set; }

        public decimal OddsMatched { get; set; }

        public string OddsMatchedText
            => CategoryType == Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsRequest)}" : OddsMatched.Format(2);

        public string Liability
        {
            get
            {
                var liability = CategoryType == Fancybet ? (((OddsMatched / 100) + 1) * Stake) - Stake : (OddsMatched - 1) * Stake;
                return liability.Format(2);
            }
        }

        public bool HasLiability { get; set; }

        public decimal FancyBetRuns { get; set; }

        public Int16 CategoryType { get; set; }
    }
}
