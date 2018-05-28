using System;

namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeTicket
    {
        public DateTime BetTaken { get; set; }

        public decimal OddsRequest { get; set; }

        public decimal Stake { get; set; }

        public decimal OddsMatched { get; set; }

        public decimal Liability
        {
            get
            {
                return (OddsMatched - 1) * Stake;
            }
        }

        public bool HasLiability { get; set; }
    }
}
