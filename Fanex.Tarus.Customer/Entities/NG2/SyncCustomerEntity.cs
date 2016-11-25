namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCustomerEntity : CustomerEntity
    {
        public SyncCustomerEntity()
        {
            IsSelected = true;
        }

        public int UplineId { get; set; }

        public string UplineUserName { get; set; }

        public string RoleName { get; set; }

        public string CurrencyName { get; set; }

        public bool IsSync { get; set; }

        public bool IsSync3rd { get; set; }

        public bool IsSelected { get; set; }

        public bool? IsUpdated { get; set; }

        public string ErrorMessage { get; set; }
    }
}