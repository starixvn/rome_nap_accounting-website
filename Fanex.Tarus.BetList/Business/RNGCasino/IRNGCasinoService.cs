namespace Fanex.Tarus.BetList.Business.RNGCasino
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;
    using Fanex.Tarus.BetList.Criteria;

    public  interface IRNGCasinoService
    {
        IEnumerable<TicketDetail> BetDetails(GetRNGCasinoBetDetailsCriteria criteria);
    }
}
