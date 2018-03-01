using Fanex.BetList.Accounting.Models;
using System.Collections.Generic;
using Fanex.BetList.Core.Entities;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Repositories
{
    public interface IBetListDetailRepository
    {
        CashOutDetails GetCashOutDetails(ICriteria criteria);
        
        IEnumerable<ITicket> GetLicenseeBonusDetails(ICriteria criteria);
    }
}
