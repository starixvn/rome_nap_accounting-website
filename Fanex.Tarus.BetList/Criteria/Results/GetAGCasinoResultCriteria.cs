namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;

    public class GetAGCasinoResultCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int BetType { get; set; }

        public long TransId { get; set; }

        public override string GetSettingKey()
        {
            return "All_BetList_MatchResultAGSel";
        }

        public override bool IsValid()
        {
            return TransId > 0;
        }
    }
}
