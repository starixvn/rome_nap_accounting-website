namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Models;
    using Fanex.Tarus.BetList.Models.CN88;

    public interface ICN88BetListService
    {
        string GetCombinationTypeComboNames(
            string betTeam,
            string betCheck,
            decimal hdp1,
            decimal hdp2,
            IList<TicketDto> ticketDetails);

        IList<TicketDto> GetMixParlayTickets(MixParlayDetailCriteria mixParlayDetailCriteria);

        BetListData GetBetListByCustInfo_DateRange(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            int betType,
            string statusIds,
            int pageSize,
            int pageIndex);

        BetListData GetBetListByCustInfo_Refno(string transactionId, int customerId);
    }
}