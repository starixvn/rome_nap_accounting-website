namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class BetListBetTradeDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay => ReportUtils.BalanceUp.MoveBetsDay;

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public override string GetSettingKey()
        {
            return "BetList_BetTradeDetails";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}