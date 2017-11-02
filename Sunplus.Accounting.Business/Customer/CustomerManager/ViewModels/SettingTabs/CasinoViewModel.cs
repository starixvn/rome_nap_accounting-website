namespace Sunplus.Accounting.Business.Customer
{
    using Fanex.Common;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncCasino => Converter.ToBool(customerInfoRow["synccasino"]);

        public bool IsAllowEditCasino => ParentId.Equals(0) || Permissions.IsAllowEditCasino(ParentId, CustParent.IsCasinoSync);

        public bool DisabledCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableCasino;

        public bool DisabledCasino => Converter.ToBool(customerInfoRow["disablecasino"]);

        public bool DisabledDownlinesCasino => Converter.ToBool(customerInfoRow["DisableDownlinesCS"]);
    }
}
