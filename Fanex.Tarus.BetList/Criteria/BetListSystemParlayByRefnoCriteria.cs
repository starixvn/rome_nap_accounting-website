namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListSystemParlayByRefnoCriteria : CriteriaBase, ICriteria
    {
        [SpParam(Ignored = true)]
        public bool IsSiteDB { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "transid")]
        public string TransactionId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public bool IsParlayBoost { get; set; }

        public override string GetSettingKey()
            => IsSiteDB ? "SiteDB_Acc_BetList_Parlay_SelByRefNo" : "BetList_CustInfo_SystemParlay";

        public override bool IsValid()
        {
            return true;
        }
    }
}