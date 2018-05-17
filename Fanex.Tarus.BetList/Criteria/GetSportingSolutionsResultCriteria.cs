namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    
    public class GetSportingSolutionsResultCriteria
    {
        [SpParam(SqlParamName = "Matchid")]
        public int MatchId { get; set; }

        public string Resourceid { get; set; }

        public string GetSettingKey()
        {
            return "SoprtingSolutionBetlistResult";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}