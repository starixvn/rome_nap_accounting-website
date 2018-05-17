namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IAGCasinoResultService
    {
        string GetResult(GetAGCasinoResultCriteria criteria);
    }
}
