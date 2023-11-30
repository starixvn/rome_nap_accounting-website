using Fanex.Customer.Client.Models;

namespace Fanex.Tarus.Customer.Requests
{
    public class CreateCustomerRequest
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string ProductGroups { get; set; }
    }
}