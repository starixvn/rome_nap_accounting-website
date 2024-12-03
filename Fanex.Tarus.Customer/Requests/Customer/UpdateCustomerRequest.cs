using Fanex.Customer.Client.Models;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class UpdateCustomerRequest
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string Group { get; set; }

        public string ProductGroups { get; set; }

        public int CustId { get; set; }

        public string UName { get; set; }

        public int RoleId { get; set; }

        public int UplineRoleId { get; set; }

        public string Site { get; set; }

        public string SiteLabel { get; set; }

        public bool IsDirectMember { get; set; } = false;
    }
}