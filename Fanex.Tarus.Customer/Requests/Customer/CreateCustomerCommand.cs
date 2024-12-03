using Fanex.Customer.Client.Models;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class CreateCustomerCommand
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string Group { get; set; }

        public string ProductGroups { get; set; }

        public bool IsDirectMember { get; set; } = false;

        public int NumberOfAccounts { get; set; } = 1;
    }
}