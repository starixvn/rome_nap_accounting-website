using Sunplus.Accounting.Business.Customer;

namespace Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus
{
    public class SuperStatusEntity
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public bool IsOfferedProtectedMode => CustomerAppSettings.IsOfferProtectedMode(SiteLabel);

        public string Site { get; set; }

        public string SiteLabel { get; set; }

        public bool Closed { get; set; }

        public bool Suspended { get; set; }

        public bool Protected { get; set; }
    }
}
