namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using Fanex.Common;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideColossusBets => (Converter.ToBool(customerInfoRow["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (RoleId < 4);

        public bool IsShowColossusBetsForCredit => !IsHideColossusBets;
    }
}
