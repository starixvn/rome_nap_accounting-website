namespace Fanex.Tarus.Customer.Entities.InitializingStakeLevel
{
    using System.Collections.Generic;

    public class StakeLevelResponse
    {
        public IEnumerable<StakeLevelModel> StakeLevels { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }
    }
}
