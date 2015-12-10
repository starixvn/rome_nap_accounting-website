namespace Fanex.Tarus.BetList.DataAccess
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;
    using Fanex.Tarus.BetList.Criteria;

    public interface IAGCasinoRepository
    {
        IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria);
    }
}
