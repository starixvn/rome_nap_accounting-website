namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideVoidbridgeUpline { get; set; } = true;

        private bool SyncVoidbridge { get; set; } = true;

        private bool SyncVoidbridgeUpline { get; set; } = true;

        public bool HideEnabledCheckBoxVoidbridge 
            => !WebSettings.IsShowVoidbridge || !SyncVoidbridgeUpline || (RoleId < 4 && (HideVoidbridgeUpline || !SyncVoidbridge));

        public bool HideSettingLinkVoidbridge 
            => !WebSettings.IsShowVoidbridge || HideVoidbridgeUpline || !SyncVoidbridgeUpline;
    }
}
