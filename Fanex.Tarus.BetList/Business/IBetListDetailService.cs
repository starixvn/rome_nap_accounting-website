namespace Fanex.Tarus.BetList.Business
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.BetList.Models;
    using System.Collections.Generic;

    public interface IBetListDetailService
    {
        CashOutDetails GetCashOutDetails(ICriteria criteria);

        IEnumerable<TicketDetail> GetLicenseeBonusDetails(ICriteria criteria);
    }
}
