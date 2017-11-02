namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.CustomerSetting.Module.Core;
    using Security;

    public partial class CustomerInfoViewModel
    {
        public bool SyncRnGCasino => Utils.ToBoolean(customerInfoRow["synccasino"]);

        public bool AllowEditRnGCasino => ParentId.Equals(0) || Permissions.IsAllowEditCasino(ParentId, CustParent.IsCasinoSync);

        public bool DisabledRnGCasinoParent => !ParentId.Equals(0) && CustParent.IsDisableCasino;

        public bool DisabledRnGCasino => Utils.ToBoolean(customerInfoRow["disablecasino"]);

        public bool DisabledDownlinesRnGCasino => Utils.ToBoolean(customerInfoRow["DisableDownlinesCS"]);

        public bool HideSettingLinkRnGCasino
            => !Permissions.IsAllowCasino() && (WebSettings.disableCS || ((RoleId < 4) && !AllowEditRnGCasino));
    }
}
