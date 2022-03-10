namespace Fanex.Tarus.Customer.Requests.InitializingStakeLevel
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;

    public class InsertStakeLevelRequest
    {
        public IEnumerable<StakeLevelModel> StakeLevels { get; set; }
    }
}
