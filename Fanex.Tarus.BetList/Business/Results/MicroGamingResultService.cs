namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core.MicroGaming;
    using Fanex.Result.Core.MicroGaming.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    public class MicroGamingResultService : IMicroGamingResultService
    {
        public string GetResult(GetMicroGamingResultCriteria criteria)
        {
            return ParserService.Instance.Parser(new MicroGaming
            {
                DepositCustId = criteria.CustId,
                SiteName = "spondemo" ?? criteria.SiteName,
                GameId = criteria.GameId
            });
        }
    }
}