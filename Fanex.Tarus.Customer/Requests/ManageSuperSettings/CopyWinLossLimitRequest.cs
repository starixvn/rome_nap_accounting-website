using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.ManageSuperSettings;
using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;

namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class CopyWinLossLimitRequest
    {
        public IEnumerable<SportType> SportTypes { get; set; }

        public IEnumerable<SuperStatusEntity> SuperList { get; set; }

        public int CustId { get; set; }

        public string SourceUsername { get; set; }

        public string SuperIds { get; set; }
    }
}
