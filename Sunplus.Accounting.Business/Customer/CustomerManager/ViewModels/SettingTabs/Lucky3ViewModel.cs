namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideLucky3 { get; private set; } = true;

        public bool IsHideLucky3Upline { get; private set; } = true;

        public bool HideCheckBoxLucky3 => !WebSettings.IsShowLucky3 || IsHideLucky3Upline || IsHideLucky3;
    }
}
