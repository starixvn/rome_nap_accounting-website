namespace Fanex.Tarus.BetList.Models.Exchange
{
    using System.Collections.Generic;
    using System.Linq;

    public class ExchangeViewModel
    {
        public ExchangeViewModel(
            IEnumerable<ExchangeSummaryTicket> summaries,
            IEnumerable<ExchangeDetailTicket> details,
            decimal grandTotal,
            int totalSummaryRows)
        {
            Summaries = summaries.OrderByDescending(i => i.SettledDate);
            Details = details;
            TotalSummaryRows = totalSummaryRows;
            TotalCommission = grandTotal;
        }

        public IEnumerable<ExchangeSummaryTicket> Summaries { get; private set; }

        public IEnumerable<ExchangeDetailTicket> Details { get; private set; }

        public decimal TotalCommission { get; private set; }

        public int TotalSummaryRows { get; }

        public int Index { get; set; }
    }
}
