namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideVoidbridge { get; set; } = true;

        public bool IsHideVoidbridgeUpline { get; set; } = true;

        public bool IsSyncVoidbridge { get; set; } = true;

        public bool IsSyncVoidbridgeUpline { get; set; } = true;

        public bool IsSyncVoidbridgeDownline { get; set; } = true;

        public bool HideCheckBoxVoidbridge => !WebSettings.IsShowVoidbridge || (RoleId < 4 && IsHideVoidbridgeUpline) || !IsSyncVoidbridgeUpline;
    }
}
