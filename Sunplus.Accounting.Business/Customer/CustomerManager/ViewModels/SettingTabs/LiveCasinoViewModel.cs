namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.Common;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncLiveCasino => WebSettings.EnableLiveCasino && Converter.ToBool(customerInfoRow["synclivecasino"]);

        public bool DisabledLiveCasino => !WebSettings.EnableLiveCasino || Converter.ToBool(customerInfoRow["disableLiveCasino"]);

        public bool DisabledDownlinesLiveCasino => !WebSettings.EnableLiveCasino || Converter.ToBool(customerInfoRow["DisableDownlinesLiveCS"]);

        public bool IsAllowEditLiveCasino => ParentId.Equals(0) || Permissions.IsAllowEditLiveCasino(ParentId, CustParent.IsLiveCasinoSync);

        public bool DisabledLiveCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableLiveCasino;
    }
}
