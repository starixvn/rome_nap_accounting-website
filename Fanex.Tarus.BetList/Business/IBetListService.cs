namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Models;
    using System;
    using System.Collections.Generic;

    public interface IBetListService
    {

        IList<TicketDetail> GetMixParlayTickets(MixParlayDetailCriteria mixParlayDetailCriteria);

        IList<TicketDetail> GetBetListByRefNo(BetListByRefNoCriteria betListByRefNoCriteria);

        IList<TicketDetail> GetBetListBetTradeDetails(BetListBetTradeDetailCriteria criteria);

        IList<TicketDetail> GetBetListByProduct(BetListByProductCriteria betListByProductCriteria);

        IList<BetType> GetBetTypeByDate(BetTypeByDateCriteria betTypebyDateCriteria);

        IList<TicketDetail> GetBetListByCustInfo_DateRange(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            int betType,
            int pageSize,
            int pageIndex,
            ref int quantity,
            bool isLimitForU3 = false);

        IList<TicketDetail> GetBetListByCustInfo_Refno(string transactionId, int customerId);

        SystemParlayModel GetBetListByCustInfo_SystemParLayTab(string transId, int custId);

        IList<BetListByIP_Username> GetListByIP_Username(
            string ip,
            DateTime fromdate,
            int pageSize,
            int pageIndex,
            ref int quanlity);

        IList<TicketDetail> GetBetListByIP_Soccer(
            int custId,
            string ip,
            DateTime fromDate,
            bool isSoccer,
            int pageSize,
            int pageIndex,
            ref int quanlity,
            ref decimal totalComm,
            ref decimal totalWinlost);

        IList<TicketDetail> GetBetListStatement(
            int custId,
            int transType,
            DateTime fromDate,
            ref int quanlity,
            ref decimal totalComm,
            ref decimal totalWinlost);

        int GetCustIDByUsername(string username);

        bool IsU3SpecialCustomers(int custId);

        string GetCombinationTypeComboNames(string betTeam, string betCheck, decimal hdp1, decimal hdp2, IList<TicketDetail> ticketDetails);

        string GetSiteNameByCustId(int custId);
    }
}