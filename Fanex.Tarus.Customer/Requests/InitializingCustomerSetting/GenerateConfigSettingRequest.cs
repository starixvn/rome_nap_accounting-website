using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Requests.InitializingCustomerSetting
{
    public class GenerateConfigSettingRequest
    {
        public int ProductId { get; set; }

        public IEnumerable<int> SportIdList { get; set; }

        public IEnumerable<int> BettypeIdList { get; set; }

        public int ConfigId { get; set; }
    }
}
