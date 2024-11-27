namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    using System.Collections.Generic;

    public class CopyWinLostLimitResult : Result
    {
        public IEnumerable<Result> CopyErrors { get; set; }
    }
}
