using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class CheckSuperListRequest
    {
        public string SourceSuper { get; set; }

        public string SuperList { get; set; }

        public bool IsCopy { get; set; }

        public string Mode { get; set; }

        public int UserId => AppSession.AccId;
    }
}
