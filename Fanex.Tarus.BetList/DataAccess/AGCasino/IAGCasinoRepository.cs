namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System.Collections.Generic;

    public interface IAGCasinoRepository
    {
        IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria);
    }
}
