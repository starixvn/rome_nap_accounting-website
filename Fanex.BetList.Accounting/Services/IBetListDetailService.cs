using System.Collections.Generic;
using Fanex.BetList.Accounting.Models;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Services
{
    public interface IBetListDetailService
    {
        CashOutDetails GetCashOutDetails(ICriteria criteria);
        
        IEnumerable<TicketDetail> GetLicenseeBonusDetails(ICriteria criteria);
    }
}
