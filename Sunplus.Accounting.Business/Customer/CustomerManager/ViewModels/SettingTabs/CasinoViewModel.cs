namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncRnGCasino => Converter.ToBool(customerInfoRow["synccasino"]);

        public bool AllowEditRnGCasino => ParentId.Equals(0) || Permissions.IsAllowEditCasino(ParentId, CustParent.IsCasinoSync);

        public bool DisabledRnGCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableCasino;

        public bool DisabledRnGCasino => Converter.ToBool(customerInfoRow["disablecasino"]);

        public bool DisabledDownlinesRnGCasino => Converter.ToBool(customerInfoRow["DisableDownlinesCS"]);
    }
}
