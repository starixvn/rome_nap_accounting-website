namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideNG2 { get; set; } = true;

        private bool HideNG2Upline { get; set; } = true;

        private bool SyncNG2 { get; set; } = true;

        private bool SyncNG2Upline { get; set; } = true;

        public bool HideSettingLinkNG2Credit => !WebSettings.IsShowNG2 || HideNG2Upline || !SyncNG2Upline || HideNG2 || IsDeposit;

        public bool HideEnabledCheckBoxNG2Credit => HideSettingLinkNG2Credit || !SyncNG2;
    }
}
