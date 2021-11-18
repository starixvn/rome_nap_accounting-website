namespace Fanex.Tarus.Customer.Requests.MultipleUpdateSuperStatus
{
    using System.Collections.Generic;

    public class UpdateSupersClosedStatusRequest
    {
        public List<UpdatedCustomerIinfo> CustList { get; set; }

        public bool IsClosed { get; set; }
    }
}
