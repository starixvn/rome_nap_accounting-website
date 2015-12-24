namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetRNGCasinoBetDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public long TransId { get; set; }

        public int StatusId { get; set; }

        [SpParam(Ignored = true)]
        public long TransID { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_RNGCasino_BetDetails";
        }

        public override bool IsValid()
        {
            return TransId > 0;
        }
    }
}
