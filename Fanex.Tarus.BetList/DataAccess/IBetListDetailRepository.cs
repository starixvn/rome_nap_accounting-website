namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Models;
    using System.Collections.Generic;

    public interface IBetListDetailRepository
    {
        CashOutDetails GetCashOutDetails(ICriteria criteria);
        
        IEnumerable<ITicket> GetLicenseeBonusDetails(ICriteria criteria);

        CashOutHistoryModel GetCashOutHistory(GetCashOutHistoryInSiteDBCriteria criteria, int custId);
    }
}
