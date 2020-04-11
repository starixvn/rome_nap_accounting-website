namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System.Collections.Generic;

    public interface IBBinService
    {
        IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria);

        IEnumerable<TicketDetail> BetDetails(GetBBinBetDetailsCriteria criteria);        
    }
}
