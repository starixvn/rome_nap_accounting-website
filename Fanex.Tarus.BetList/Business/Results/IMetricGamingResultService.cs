namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IMetricGamingResultService
    {
        string GetResult(GetMetricGamingResultCriteria criteria);
    }
}