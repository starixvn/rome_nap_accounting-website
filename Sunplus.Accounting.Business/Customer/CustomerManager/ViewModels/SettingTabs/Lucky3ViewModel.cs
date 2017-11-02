namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        public bool HideLucky3 { get; private set; } = true;

        public bool HideLucky3Upline { get; private set; } = true;

        public bool HideEnabledCheckBoxLucky3Credit => HideLucky3Upline || HideLucky3 || IsDeposit;

        public bool HideSettingLinkLucky3Credit => HideEnabledCheckBoxLucky3Credit;
    }
}
