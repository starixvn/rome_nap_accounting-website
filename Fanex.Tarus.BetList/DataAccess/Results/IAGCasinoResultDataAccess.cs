namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IAGCasinoResultDataAccess
    {
        List<Fanex.Result.Core.AGCasino.DTO.AGCasino> GetResult(GetAGCasinoResultCriteria criteria);
    }
}
