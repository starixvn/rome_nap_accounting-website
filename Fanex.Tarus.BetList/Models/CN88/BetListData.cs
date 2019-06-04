using System.Collections.Generic;
using System.Linq;
using Fanex.BetList.Core.Entities;

namespace Fanex.Tarus.BetList.Models.CN88
{
    public class BetListData
    {
        public BetListData(
            IEnumerable<TicketDto> tickets,
            IEnumerable<ITicketData> ticketData,
            int total)
        {
            Tickets = tickets ?? Enumerable.Empty<TicketDto>();
            TicketData = ticketData ?? Enumerable.Empty<ITicketData>();
            Total = total;
        }

        public IEnumerable<TicketDto> Tickets { get; }

        public IEnumerable<ITicketData> TicketData { get; }

        public int Total { get; }
    }
}
