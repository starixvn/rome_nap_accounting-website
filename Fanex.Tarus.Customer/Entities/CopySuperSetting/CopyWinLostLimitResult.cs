namespace Fanex.Tarus.Customer.Entities.CopySuperSetting
{
    using System.Collections.Generic;

    public class CopyWinLostLimitResult : Result
    {
        public IEnumerable<Result> CopyErrors { get; set; }
    }
}
