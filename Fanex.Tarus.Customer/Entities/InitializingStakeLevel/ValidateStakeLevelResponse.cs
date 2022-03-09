namespace Fanex.Tarus.Customer.Entities.InitializingStakeLevel
{
    using System.Collections.Generic;
    public class ValidateStakeLevelResponse
    {
        public IEnumerable<StakeLevelModel> StakeLevels { get; set; }

        public int ProductId { get; set; }

        public Result Error { get; set; }

        public Result Warning { get; set; }
    }
}
