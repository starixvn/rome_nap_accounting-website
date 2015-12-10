namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetMicroGamingBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        public int TransType { get; set; }

        [SpParam(Ignored = true)]
        public long TransID { get; set; }

        [SpParam(Ignored = true)]
        public DateTime WinLossDate { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_MicroGaming_BetDetails";
        }

        public override bool IsValid()
        {
            return Refno > 0;
        }
    }
}
