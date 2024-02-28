namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class SiteDBMixParlayDetailCriteria : CriteriaBase
    {

        [SpParam(SqlParamName = "transid")]
        public long TransId { get; set; }

        [SpParam(SqlParamName = "refno")]
        public long RefNo { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_BetList_SelParlayDetail";
    }
}