namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool HideNG2 { get; private set; } = true;

        public bool HideNG2Upline { get; private set; } = true;

        public bool SyncNG2 { get; private set; } = true;

        public bool SyncNG2Upline { get; private set; } = true;

        public bool RemoveEnabledCheckBoxNG2 => !WebSettings.IsShowNG2 || HideNG2Upline || !SyncNG2Upline || HideNG2 || !SyncNG2;

        public bool RemoveSettingLinkNG2 => !WebSettings.IsShowNG2 || HideNG2Upline || !SyncNG2Upline || HideNG2;
    }
}
