using Fanex.BetList.Accounting.Models;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Services
{
    public interface IBetListDetailService
    {
        CashOutHistory GetCashOutHistory(ICriteria criteria);
    }
}
