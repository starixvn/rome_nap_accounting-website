namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class GetAGCasinoBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        public int TransType { get; set; }

        [SpParam(Ignored = true)]
        public long TransID { get; set; }

        [SpParam(Ignored = true)]
        public DateTime WinLossDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public DateTime MoveBetsDay => ReportUtils.BalanceUp.MoveBetsDay;

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public bool ExcludeBettrans { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_AGCasino_BetDetails";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
