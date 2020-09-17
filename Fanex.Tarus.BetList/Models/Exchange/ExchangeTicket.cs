using System;
using Fanex.Tarus.BetList.Constants;
using Fanex.Tarus.BetList.Extensions;
namespace Fanex.Tarus.BetList.Models.Exchange
{
    public class ExchangeTicket
    {
        public ExchangeTicket()
        {

        }

        private const int CricketEvent = 4;

        public DateTime BetTaken { get; set; }

        public decimal OddsRequest { get; set; }

        public string OddsRequestText
            => CategoryType == ExchangeTypeConstants.Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsRequest)}" : OddsRequest.Format(2);

        public decimal Stake { get; set; }

        public decimal OddsMatched { get; set; }

        public string OddsMatchedText
            => CategoryType == ExchangeTypeConstants.Fancybet ? $"{Decimal.ToInt32(FancyBetRuns)}/{Decimal.ToInt32(OddsRequest)}" : OddsMatched.Format(2);

        public string Liability
        {
            get
            {
                var liability =  (OddsMatched - 1) * Stake;
                if (CategoryType == ExchangeTypeConstants.Fancybet)
                {
                    liability = (((OddsMatched / 100) + 1) * Stake) - Stake;
                }
                if (CategoryType == ExchangeTypeConstants.BookMaker && EventType == CricketEvent)
                {
                    liability = ((OddsMatched / 100 + 1) - 1) * Stake;
                }

                return liability.Format(2);
            }
        }

        public int EventType { get; set; }

        public bool HasLiability { get; set; }

        public decimal FancyBetRuns { get; set; }

        public Int16 CategoryType { get; set; }
    }
}
