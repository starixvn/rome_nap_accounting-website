namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class SiteDBBetListByCustInfoByRefnoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_BetList_SelByRefNo_Mem";

        public override bool IsValid() => true;
    }
}