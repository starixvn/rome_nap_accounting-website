namespace Fanex.Tarus.BetList.Criteria
{
    using NAP.Framework.Data;
    using System;

    public class BetListByRefNoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Permission { get; set; }

        public override string GetSettingKey()
            => "BetList_SelByRefNo";

        public override bool IsValid()
            => true;
    }
}