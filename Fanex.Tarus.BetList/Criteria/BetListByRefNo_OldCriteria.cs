namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListByRefNoOldCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string Permission { get; set; }

        public string Site { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_SelByRefNo_Old";
        
        public override bool IsValid()
            => RefNo > 0 && !string.IsNullOrEmpty(Site);
    }
}