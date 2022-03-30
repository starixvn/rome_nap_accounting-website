namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class GetCashOutHistoryCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long CustId { get; set; }

        public DateTime WinlostDate { get; set; }

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public override string GetSettingKey()
            => "Acc_BetList_CashOut_History";

        public override bool IsValid()
            => TransId > 0 && CustId > 0;
    }
}
