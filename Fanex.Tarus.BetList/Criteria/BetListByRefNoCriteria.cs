namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.Tarus.BetList.Param;
    using NAP.Framework.Data;
    using System;

    public class BetListByRefNoCriteria : CriteriaBase
    {
        public string DeniedList { get; set; }

        public string RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Permission { get; set; }

        public bool ExcludeBettrans { get; set; }

        public override string GetSettingKey()
            => "BetList_SelByRefNo";

        public override bool IsValid()
            => !string.IsNullOrEmpty(RefNo);
    }
}