namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using DNAide.Core;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncLiveCasino => WebSettings.EnableLiveCasino && CastAide.AsBoolean(customerInfoRow["synclivecasino"], false);

        public bool DisabledLiveCasino => !WebSettings.EnableLiveCasino || CastAide.AsBoolean(customerInfoRow["disableLiveCasino"], false);

        public bool DisabledDownlinesLiveCasino => !WebSettings.EnableLiveCasino || CastAide.AsBoolean(customerInfoRow["DisableDownlinesLiveCS"], false);

        public bool IsAllowEditLiveCasino => ParentId.Equals(0) || Permissions.IsAllowEditLiveCasino(ParentId, CustParent.IsLiveCasinoSync);

        public bool DisabledLiveCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableLiveCasino;
    }
}
