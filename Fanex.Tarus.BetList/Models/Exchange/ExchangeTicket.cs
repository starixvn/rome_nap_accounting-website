using System;
using Fanex.Tarus.BetList.Extensions;
namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeTicket
    {
        const int Fancybet = 2;

        public DateTime BetTaken { get; set; }

        public decimal OddsRequest { get; set; }

        public string OddsRequestText
            => CategoryType == Fancybet ? $"{FancyBetRuns}/{OddsRequest}" : OddsRequest.Format(2);

        public decimal Stake { get; set; }

        public decimal OddsMatched { get; set; }

        public string OddsMatchedText
            => CategoryType == Fancybet ? $"{FancyBetRuns}/{OddsRequest}" : OddsMatched.Format(2);

        public string Liability
        {
            get
            {
                var liability = CategoryType == FancyBetRuns ? (((OddsMatched / 100) + 1) * Stake) - Stake : (OddsMatched - 1) * Stake;
                return liability.Format(2);
            }
        }

        public bool HasLiability { get; set; }

        public decimal FancyBetRuns { get; set; }

        public int CategoryType { get; set; }
    }
}
