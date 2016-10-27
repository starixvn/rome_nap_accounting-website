using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Customer
{
    public class CustomerBalanceRequest
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public bool IsCached { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }
    }
}
