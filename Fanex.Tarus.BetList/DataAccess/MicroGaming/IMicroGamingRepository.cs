namespace Fanex.Tarus.BetList.DataAccess
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;
    using Fanex.Tarus.BetList.Criteria;

    public interface IMicroGamingRepository
    {
        IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria);
    }
}
