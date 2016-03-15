namespace Fanex.Tarus.BetList.Entities.BetList
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;

    public class BetDetail
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public IEnumerable<TicketDetail> TicketDetails { get; set; }
    }
}
