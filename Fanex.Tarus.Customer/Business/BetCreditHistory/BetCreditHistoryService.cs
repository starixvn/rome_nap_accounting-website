using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.Customer.Criteria.BetCreditHistory;
using Fanex.Tarus.Customer.DataAccess.BetCreditHistory;
using Fanex.Tarus.Customer.Entities.BetCreditHistory;
using Sunplus.Accounting.DataAccess.Report;
using Result = Fanex.Tarus.Customer.Entities.Result;

namespace Fanex.Tarus.Customer.Business.BetCreditHistory
{
    public interface IBetCreditHistoryService
    {
        Tuple<Result, IList<BetCreditHistoryItem>> GetBetCreditHistory(string member, DateTime fromDate, DateTime toDate);
    }

    public class BetCreditHistoryService : IBetCreditHistoryService
    {
        private readonly IBetCreditHistoryDataAccess dataAccess;

        public BetCreditHistoryService(IBetCreditHistoryDataAccess dataAccess = null)
        {
            this.dataAccess = dataAccess ?? new BetCreditHistoryDataAccess();
        }

        public Tuple<Result, IList<BetCreditHistoryItem>> GetBetCreditHistory(string member, DateTime fromDate, DateTime toDate)
        {
            var balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;
            var criteria = new GetBetCreditHistoryCriteria(
                member?.Trim(),
                fromDate < balanceUpDay ? balanceUpDay : fromDate,
                toDate);

            var betCreditHistory = criteria.IsValid()
                ? dataAccess.GetBetCreditHistory(criteria).ToList()
                : new List<BetCreditHistoryItem>();
            var result = new Result(criteria.ErrorCode, criteria.ErrorMess);

            return new Tuple<Result, IList<BetCreditHistoryItem>>(result, betCreditHistory);
        }
    }
}
