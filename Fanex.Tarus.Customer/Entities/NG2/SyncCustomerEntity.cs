namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCustomerEntity : CustomerEntity
    {
        public int UplineId { get; set; }

        public string UplineUserName { get; set; }

        public string RoleName { get; set; }

        public string CurrencyName { get; set; }

        public bool IsSync { get; set; }

        public bool IsSync3rd { get; set; }
    }
}