namespace Fanex.Tarus.BetList.DataAccess
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.Tarus.BetList.Criteria;

    public interface IColossusBetsRepository
    {
        IEnumerable<ITicket> BetDetails(GetColossusBetsBetDetailsCriteria criteria);
    }
}
