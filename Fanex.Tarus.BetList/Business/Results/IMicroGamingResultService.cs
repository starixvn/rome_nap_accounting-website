namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IMicroGamingResultService
    {
        string GetResult(GetMicroGamingResultCriteria criteria);
    }
}