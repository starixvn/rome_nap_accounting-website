namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.CustomerSetting.Module.Core;

    public partial class CustomerInfoViewModel
    {
        private bool HideColossusBets => !WebSettings.IsShowColossusBets || (RoleId < 4 && Utils.ToBoolean(customerInfoRow["HideColossusBet"]));//(Utils.ToBoolean(customerInfoRow["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (RoleId < 4);

        public bool ShowEnabledCheckboxColossusBets => !HideColossusBets;

        public bool HideSettingLinkColossusBetsCredit => HideColossusBets || IsDeposit;
    }
}
