namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool HideVoidbridgeUpline { get; private set; } = true;

        public bool SyncVoidbridge { get; private set; } = true;

        public bool SyncVoidbridgeUpline { get; private set; } = true;

        public bool RemoveEnabledCheckBoxVoidbridge 
            => !WebSettings.IsShowVoidbridge || !SyncVoidbridgeUpline || (RoleId < 4 && (HideVoidbridgeUpline || !SyncVoidbridge));

        public bool RemoveSettingLinkVoidbridge 
            => !WebSettings.IsShowVoidbridge || HideVoidbridgeUpline || !SyncVoidbridgeUpline;
    }
}
