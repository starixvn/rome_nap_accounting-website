namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetColossusBetsBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        [SpParam(Ignored = true)]
        public long TransID { get; set; }

        [SpParam(Ignored = true)]
        public DateTime WinLossDate { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_ColossusBets_BetDetails";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
