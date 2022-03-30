namespace Fanex.Tarus.BetList.Business
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Models;
    using System.Collections.Generic;

    public interface IBetListDetailService
    {
        CashOutHistoryModel GetCashOutHistory(GetCashOutHistoryCriteria criteria);

        CashOutDetails GetCashOutDetails(ICriteria criteria);

        IEnumerable<TicketDetail> GetLicenseeBonusDetails(ICriteria criteria);
    }
}
