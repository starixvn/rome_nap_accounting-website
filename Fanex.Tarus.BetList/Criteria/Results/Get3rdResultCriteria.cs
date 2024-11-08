namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;
    
    public class Get3rdResultCriteria : CriteriaBase
    {
        public int MatchId { get; set; }
        public override string GetSettingKey()
        {
            return "Acc_BetList_Match3rdResultSel";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}
