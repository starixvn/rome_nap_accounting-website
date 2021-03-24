namespace Fanex.Tarus.Customer.ManageLicenseeCode.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Module.Core.Models;
    public class NonQueryResponse
    {
        public IEnumerable<Result> ValidateResponse { get; set; }

        public Result NonQueryResult { get; set; }
    }
}
