namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideVGamingUpline { get; set; } = true;

        private bool SyncVGaming { get; set; } = true;

        public bool SyncVGamingUpline { get; private set; } = true;

        public bool HideEnabledCheckBoxVGaming
            => !WebSettings.IsShowSabaclub || IsDeposit || !SyncVGamingUpline || (SearchAccountRoleId < 4 && (HideVGamingUpline || !SyncVGaming));

        public bool HideSettingLinkVGaming
            => !WebSettings.IsShowSabaclub || IsDeposit || HideVGamingUpline || !SyncVGamingUpline;
    }
}
