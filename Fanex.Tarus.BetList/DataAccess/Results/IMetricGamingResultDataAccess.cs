namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IMetricGamingResultDataAccess
    {
        List<Fanex.Result.Core.MetricGaming.DTO.MetricGaming> GetResult(GetMetricGamingResultCriteria criteria);
    }
}
