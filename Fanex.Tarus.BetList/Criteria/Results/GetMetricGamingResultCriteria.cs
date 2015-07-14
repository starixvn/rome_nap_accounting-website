namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;

    public class GetMetricGamingResultCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public long TransId { get; set; }

        [SpParam(Ignored = true)]
        public int BetType { get; set; }

        public long BetId { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "All_BetList_MatchResultDetail3rdSel";
        }

        public override bool IsValid()
        {
            return TransId > 0;
        }
    }
}
