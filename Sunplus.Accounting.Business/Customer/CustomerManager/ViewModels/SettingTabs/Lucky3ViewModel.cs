namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerInfoViewModel
    {
        private bool HideLucky3 { get; set; } = true;

        private bool HideLucky3Upline { get; set; } = true;

        public bool HideEnabledCheckBoxLucky3Credit => HideLucky3Upline || HideLucky3 || IsDeposit;

        public bool HideSettingLinkLucky3Credit => HideEnabledCheckBoxLucky3Credit;
    }
}
