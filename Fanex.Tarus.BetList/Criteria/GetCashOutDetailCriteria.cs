namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class GetCashOutDetailCriteria : CriteriaBase, ICriteria
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public short BetType { get; set; }

        public long OriginalTransId { get; set; }

        [SpParam(Ignored = true)]
        public string Title { get; set; }

        public DateTime MoveBetsDay => ReportUtils.BalanceUp.MoveBetsDay;

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public override string GetSettingKey()
        {
            return "Acc_BetList_CashOut_Detail";
        }

        public override bool IsValid()
        {
            return TransId > 0 &&
                RefNo > 0 &&
                BetType > 0 &&
                OriginalTransId > 0;
        }
    }
}
