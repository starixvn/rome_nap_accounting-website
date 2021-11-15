namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class GetBBinBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        public int TransType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public DateTime MoveBetsDay => ReportUtils.BalanceUp.MoveBetsDay;

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public override string GetSettingKey()
        {
            return "Acc_BetList_G_Detail";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
