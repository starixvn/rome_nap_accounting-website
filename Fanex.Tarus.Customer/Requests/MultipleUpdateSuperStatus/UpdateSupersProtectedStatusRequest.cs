using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Requests.MultipleUpdateSuperStatus
{
    public class UpdateSupersProtectedStatusRequest
    {
        public List<UpdatedCustomerIinfo> CustList { get; set; }

        public bool IsProtected { get; set; }
    }
}
