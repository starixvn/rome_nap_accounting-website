namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetCashOutHistoryInSiteDBCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long CustId { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_BetList_CashOut_History";

        public override bool IsValid()
            => TransId > 0 && CustId > 0;
    }
}
