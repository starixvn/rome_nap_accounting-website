namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System.Collections.Generic;

    public interface IMicroGamingRepository
    {
        IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria);
    }
}
