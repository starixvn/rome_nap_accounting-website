namespace Fanex.Tarus.BetList.Criteria
{
    using Business;
    using Fanex.NAP.Framework.Data;
    using System;

    public class BetTypeByDateInSiteDBCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay => ReportService.BalanceUp.MoveBetsDay;

        public override string GetSettingKey() => "SiteDB_Acc_BetList_SelBettype";

        public override bool IsValid() => true;
    }
}