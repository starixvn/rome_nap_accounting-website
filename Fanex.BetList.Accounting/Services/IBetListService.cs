// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.12.10">Created</change>
// </history>

using System;
using System.Collections.Generic;
using Fanex.BetList.Accounting.Criteria;
using Fanex.BetList.Accounting.Models;

namespace Fanex.BetList.Accounting.Services
{
    public interface IBetListService
    {
        BetListDetail GetWinLossMemberBetList(WinLossMemberBetListCriteria winLossMemberBetListCriteria);

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