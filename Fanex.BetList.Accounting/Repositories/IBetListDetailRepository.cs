using Fanex.BetList.Core.Entities;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Repositories
{
    public interface IBetListDetailRepository
    {
        Ticket GetBetListDetail(ICriteria criteria);
    }
}
