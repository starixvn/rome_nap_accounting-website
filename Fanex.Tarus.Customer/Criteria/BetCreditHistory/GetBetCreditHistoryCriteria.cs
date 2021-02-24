using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.BetCreditHistory
{
    public class GetBetCreditHistoryCriteria : CriteriaBase
    {
        public GetBetCreditHistoryCriteria(string member, DateTime fromDate, DateTime toDate)
        {
            Username = member;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "Acc_BalanceHistory_Mem";

        public override bool IsValid()
            => FromDate <= ToDate && !string.IsNullOrWhiteSpace(Username);
    }
}
