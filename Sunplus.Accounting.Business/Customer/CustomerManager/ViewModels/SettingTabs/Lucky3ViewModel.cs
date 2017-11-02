namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public bool HideLucky3 { get; private set; } = true;

        public bool HideLucky3Upline { get; private set; } = true;

        public bool RemoveEnabledCheckBoxLucky3Credit => HideLucky3Upline || HideLucky3;

        public bool RemoveSettingLinkLucky3Credit => RemoveEnabledCheckBoxLucky3Credit;
    }
}
