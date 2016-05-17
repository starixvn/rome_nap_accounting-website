namespace Fanex.BetList.Accounting.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListSystemParlayDetailsCriteria :  CriteriaBase
    {
        public long RefNo { get; set; }

        public long TransId { get; set; }

        public long BetId { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public decimal Outstanding { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetlistCombinationParlay_Details";
        }
    }
}
