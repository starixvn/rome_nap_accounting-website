namespace Fanex.Tarus.Customer.Requests.InitializingBetLimit
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities.InitializingBetLimit;

    public class InsertBetLimitRequest
    {
        public IEnumerable<BetLimitModel> BetLimits { get; set; }
    }
}
