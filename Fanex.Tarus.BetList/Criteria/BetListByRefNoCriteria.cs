namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.Tarus.BetList.Param;
    using NAP.Framework.Data;
    using System;

    public class BetListByRefNoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Permission { get; set; }

        public string Site { get; set; }

        public bool ExcludeBettrans { get; set; }

        public override string GetSettingKey()
            => "BetList_SelByRefNo";

        public override bool IsValid()
            => RefNo > 0 && UserId > 0;
    }
}