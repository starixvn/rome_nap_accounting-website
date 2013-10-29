using System;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.Business.BetList
{
    public interface IBetListManager
    {
        List<BetListDetail> GetLatestBetList(int totalRow);

        List<BetMonitor> GetLatestBABetList(int totalRow, int amount, Int64 lastTransId);

        BABetList GetBABetList(int pagesize, int pageIndex, string fromdate, string todate, string refno, string username, int currency, int bettype, int fromStake, int toStake, int excludeTest);

        List<BetListDetail> GetMixParlayTickets(string refNo);

        //BABetList GetBABetListByTransID(string refno);
        LicenseeDTO GetDatetimeOption(string option);

        List<Bettype> GetBettype();

        List<CustDetail> GetCurrency();

        CombinationDetailMP GetCombinationDetailMP(string refNo);

        TicketsListingBetlistDTO GetTicketsListingBetlist(bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, int Language, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter);

        SystemParlayDetail GetBetListSystemParlayDetailByTransId(string refNo, int custId);
    }
}