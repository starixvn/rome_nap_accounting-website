namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.Tarus.BetList.Param;
    using NAP.Framework.Data;
    using System;

    public class SiteDBBetListByRefNoCriteria : CriteriaBase
    {
        public string DeniedList { get; set; }

        public string RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string Permission { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_BetList_SelByRefNo";

        public override bool IsValid()
            => !string.IsNullOrEmpty(RefNo);
    }
}