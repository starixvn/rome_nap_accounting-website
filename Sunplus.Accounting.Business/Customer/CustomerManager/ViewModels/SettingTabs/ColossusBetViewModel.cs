namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;
    using DNAide.Core;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideColossusBets => (CastAide.AsBoolean(customerInfoRow["HideColossusBet"]) || !WebSettings.IsShowColossusBets) && (RoleId < 4);

        public bool IsShowColossusBetsForCredit => !IsHideColossusBets;
    }
}
