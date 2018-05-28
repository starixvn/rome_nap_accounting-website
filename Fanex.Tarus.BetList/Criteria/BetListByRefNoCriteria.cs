namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    using System;

    public class BetListByRefNoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_SelByRefNo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}