namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public class GetAGCasinoBetDetailsInSiteDBCriteria : CriteriaBase
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

        public override string GetSettingKey()
        {
            return "SiteDB_Acc_BetList_LC_Detail";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
