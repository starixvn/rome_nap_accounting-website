namespace Fanex.Tarus.Customer.Entities.InitializingBetLimit
{
    using System.Collections.Generic;
    public class ValidateBetLimitResponse
    {
        public IEnumerable<BetLimitModel> BetLimits { get; set; }

        public int ProductId { get; set; }

        public IEnumerable<int> SportTypeIds { get; set; }

        public IEnumerable<int> BetTypeIds { get; set; }

        public IEnumerable<string> CreditCurrencies { get; set; }

        public IEnumerable<string> DepositCurrencies { get; set; }

        public Result Error { get; set; }

        public Result Warning { get; set; }
    }
}
