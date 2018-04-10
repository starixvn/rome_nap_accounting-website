namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System.Collections.Generic;

    public interface IMicroGamingService
    {
        IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria);
    }
}
