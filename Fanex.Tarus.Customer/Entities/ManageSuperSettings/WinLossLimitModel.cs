using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;

namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    public class WinLossLimitModel
    {
        public IEnumerable<SportType> SportTypes { get; set; }

        public IEnumerable<SuperStatusEntity> SuperList { get; set; }

        public string SuperListIds { get; set; }

        public int CustId { get; set; }

        public Result Result { get; set; }
    }
}
