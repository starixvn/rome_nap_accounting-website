using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    public class CopySettingInfoModel
    {
        public IEnumerable<SportType> SportTypes { get; set; }

        public IEnumerable<BaseModel> SuperList { get; set; }

        public string SuperListIds { get; set; }

        public int CustId { get; set; }

        public Result Result { get; set; }
    }
}
