namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class CopyCustomerRequest
    {
        public int UplineId { get; set; }

        public byte CustLevel { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public decimal Credit { get; set; }

        public int SourceCustId { get; set; }

        public string Site { get; set; }

        public int Currency { get; set; }

        public bool IsInternal { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }
    }
}