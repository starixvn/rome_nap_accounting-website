namespace Fanex.Tarus.BetList.Business
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;
    using Fanex.Tarus.BetList.Criteria;

    public interface IMicroGamingService
    {
        IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria);
    }
}
