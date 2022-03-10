namespace Fanex.Tarus.Customer.Entities.InitializingStakeLevel
{
    using System.Collections.Generic;
    public class ValidateStakeLevelResponse
    {
        public IEnumerable<StakeLevelModel> StakeLevels { get; set; }

        public int ProductId { get; set; }

        public IEnumerable<int> SportTypeIds { get; set; }

        public IEnumerable<int> BetTypeIds { get; set; }

        public IEnumerable<string> CreditCurrencies { get; set; }

        public IEnumerable<string> DepositCurrencies { get; set; }

        public Result Error { get; set; }

        public Result Warning { get; set; }
    }
}
