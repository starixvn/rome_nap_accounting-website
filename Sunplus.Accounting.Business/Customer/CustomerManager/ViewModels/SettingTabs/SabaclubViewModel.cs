namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideSabaclubUpline { get; set; } = true;

        private bool SyncSabaclub { get; set; } = true;

        public bool SyncSabaclubUpline { get; private set; } = true;

        public bool HideEnabledCheckBoxSabaclub
            => !WebSettings.IsShowSabaclub || IsDeposit || !SyncSabaclubUpline || (SearchAccountRoleId < 4 && (HideSabaclubUpline || !SyncSabaclub));

        public bool HideSettingLinkSabaclub
            => !WebSettings.IsShowSabaclub || IsDeposit || HideSabaclubUpline || !SyncSabaclubUpline;
    }
}
