using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.CopySuperSetting;

namespace Fanex.Tarus.Customer.Requests.CopySuperSetting
{
    public class UpdateWinLossLimitRequest
    {
        public IEnumerable<SportType> SportTypes { get; set; }

        public IEnumerable<BaseModel> SuperList { get; set; }

        public int CustId { get; set; }

        public string SourceUsername { get; set; }

        public string SuperIds { get; set; }
    }
}
