namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.CustomerSetting.Module.Core;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncLiveCasino => WebSettings.EnableLiveCasino && Utils.ToBoolean(customerInfoRow["synclivecasino"]);

        public bool DisabledLiveCasino => !WebSettings.EnableLiveCasino || Utils.ToBoolean(customerInfoRow["disableLiveCasino"]);

        public bool DisabledDownlinesLiveCasino => !WebSettings.EnableLiveCasino || Utils.ToBoolean(customerInfoRow["DisableDownlinesLiveCS"]);

        public bool AllowEditLiveCasino => ParentId.Equals(0) || Permissions.IsAllowEditLiveCasino(ParentId, CustParent.IsLiveCasinoSync);

        public bool DisabledLiveCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableLiveCasino;

        public bool ShowSettingLinkLiveCasino => WebSettings.EnableLiveCasino && AllowEditLiveCasino;
    }
}
