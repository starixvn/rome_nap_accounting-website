namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System.Collections.Generic;

    public interface IAGCasinoService
    {
        IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria);
    }
}
