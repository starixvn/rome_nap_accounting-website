using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.CopySuperSetting
{
    public class CheckSuperListResult
    {
        public IEnumerable<BaseModel> SportTypes { get; set; }

        public IEnumerable<BaseModel> SuperList { get; set; }

        public int CustId { get; set; }

        public Result Result { get; set; }
    }
}
