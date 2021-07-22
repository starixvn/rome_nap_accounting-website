namespace Fanex.Tarus.Customer.Requests.CustomerSetting.OpenCustomer
{
    using Infrastructure.Common;
    using Infrastructure.Helper;

    public class UpdateClosedStatusQuery
    {
        public int CustId { get; set; }

        public int AccId => AppSession.AccId;

        public bool IsClosed { get; set; }

        public string Ip => Utils.GetClientIP();
    }
}