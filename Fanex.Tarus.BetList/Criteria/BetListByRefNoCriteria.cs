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

        public string Permission { get; set; }

        public string Site { get; set; }

        [SpParam(Ignored = true)]
        public int SiteDBGroupId { get; set; }

        public override string GetSettingKey()
            => SiteDBGroupId == 0 ? "BetList_SelByRefNo" : "SiteDB_Acc_BetList_SelByRefNo";

        public override bool IsValid()
            => RefNo > 0 && !string.IsNullOrEmpty(Site);
    }
}