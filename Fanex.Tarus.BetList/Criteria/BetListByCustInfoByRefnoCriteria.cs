namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListByCustInfoByRefnoCriteria : CriteriaBase
    {

        public int CustId { get; set; }

        public string ShadowIds { get; set; }

        public string RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public bool ExcludeBettrans { get; set; }

        public override string GetSettingKey()
            => "Acc_BetsSelBetList_ByTransID";

        public override bool IsValid() => true;
    }
}