using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Requests.MultipleUpdateSuperStatus
{
    public class UpdateSupersSuspendedStatusRequest
    {
        public List<UpdatedCustomerIinfo> CustList { get; set; }

        public bool IsSuspended { get; set; }
    }
}
