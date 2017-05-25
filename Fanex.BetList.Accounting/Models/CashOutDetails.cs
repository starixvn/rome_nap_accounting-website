using System.Collections.Generic;

namespace Fanex.BetList.Accounting.Models
{
    public class CashOutDetails
    {
        public TicketDetail TicketDetail { get; set; }

        public IList<CashOutTicket> CashOutTickets { get; set; }
    }
}
