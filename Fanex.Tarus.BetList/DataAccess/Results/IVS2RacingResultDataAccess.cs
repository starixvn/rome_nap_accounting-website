using Fanex.Result.Core.VirtualRacing.DTO;
using Fanex.Tarus.BetList.Criteria.Results;
using System.Collections.Generic;

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    public interface IVS2RacingResultDataAccess
    {
        IList<VirtualRacing> GetRacingPopupResult(GetVS2RacingResultCriteria criteria);
    }
}
