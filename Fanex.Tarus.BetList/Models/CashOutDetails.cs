using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Models
{
    public class CashOutDetails
    {
        public TicketDetail TicketDetail { get; set; }

        public IList<CashOutTicket> CashOutTickets { get; set; }
    }
}
