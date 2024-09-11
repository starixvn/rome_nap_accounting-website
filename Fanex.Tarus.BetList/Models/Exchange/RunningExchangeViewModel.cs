namespace Fanex.Tarus.BetList.Models.Exchange
{
    using System.Collections.Generic;

    public class RunningExchangeViewModel
    {
        public RunningExchangeViewModel(
            IEnumerable<RunningExchangeTicket> details,
            int totalSummaryRows)
        {
            Details = details;
            TotalSummaryRows = totalSummaryRows;
        }

        public IEnumerable<RunningExchangeTicket> Details { get; private set; }

        public int TotalSummaryRows { get; }

        public int Index { get; set; }
    }
}
