namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideNG2 { get; private set; } = true;

        public bool IsHideNG2Upline { get; private set; } = true;

        public bool IsSyncNG2 { get; private set; } = true;

        public bool IsSyncNG2Upline { get; private set; } = true;

        public bool HideCheckBoxNG2 => !WebSettings.IsShowNG2 || IsHideNG2Upline || !IsSyncNG2Upline || !IsSyncNG2 || IsHideNG2;
    }
}
