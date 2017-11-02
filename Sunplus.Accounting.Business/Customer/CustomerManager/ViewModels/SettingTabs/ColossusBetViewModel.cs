namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public bool HideColossusBets => (Converter.ToBool(customerInfoRow["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (RoleId < 4);

        public bool ShowEnabledCheckboxColossusBets => !HideColossusBets;

        public bool RemoveSettingLinkColossusBets => HideColossusBets || IsDeposit;
    }
}
