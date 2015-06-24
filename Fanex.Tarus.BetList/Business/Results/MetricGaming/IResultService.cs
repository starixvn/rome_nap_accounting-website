namespace Fanex.Tarus.BetList.Business.Results.MetricGaming
{
    using Fanex.Tarus.BetList.Criteria.Results.MetricGaming;

    public interface IResultService
    {
        string GetResult(ViewResultCriteria criteria);
    }
}