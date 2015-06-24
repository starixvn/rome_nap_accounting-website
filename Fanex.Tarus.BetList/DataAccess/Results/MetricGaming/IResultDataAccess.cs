namespace Fanex.Tarus.BetList.DataAccess.Results.MetricGaming
{
    using System.Collections.Generic;
    using Fanex.Tarus.BetList.Criteria.Results.MetricGaming;

    public interface IResultDataAccess
    {
        List<Fanex.Result.Core.MetricGaming.DTO.MetricGaming> GetResult(ViewResultCriteria criteria);
    }
}
