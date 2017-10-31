namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideVoidbridge { get; private set; } = true;

        public bool IsHideVoidbridgeUpline { get; private set; } = true;

        public bool IsSyncVoidbridge { get; private set; } = true;

        public bool IsSyncVoidbridgeUpline { get; private set; } = true;

        public bool HideCheckBoxVoidbridge => !WebSettings.IsShowVoidbridge || (RoleId < 4 && IsHideVoidbridgeUpline) || !IsSyncVoidbridgeUpline;
    }
}
