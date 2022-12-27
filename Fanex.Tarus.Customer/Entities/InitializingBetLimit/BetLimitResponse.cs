namespace Fanex.Tarus.Customer.Entities.InitializingBetLimit
{
    using System.Collections.Generic;

    public class BetLimitResponse
    {
        public IEnumerable<BetLimitModel> BetLimits { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }
    }
}
