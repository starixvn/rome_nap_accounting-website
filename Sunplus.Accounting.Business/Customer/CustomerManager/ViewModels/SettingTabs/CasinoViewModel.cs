namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Core;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncCasino => CastAide.AsBoolean(customerInfoRow["synccasino"], false);

        public bool IsAllowEditCasino => ParentId.Equals(0) || Permissions.IsAllowEditCasino(ParentId, CustParent.IsCasinoSync);

        public bool DisabledCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableCasino;

        public bool DisabledCasino => CastAide.AsBoolean(customerInfoRow["disablecasino"], false);

        public bool DisabledDownlinesCasino => CastAide.AsBoolean(customerInfoRow["DisableDownlinesCS"], false);
    }
}
