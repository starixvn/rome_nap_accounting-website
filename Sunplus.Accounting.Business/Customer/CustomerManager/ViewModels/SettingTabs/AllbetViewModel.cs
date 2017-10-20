namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideAllBet { get; set; } = true;

        public bool IsHideAllbetUpline { get; set; } = true;

        public bool IsSyncAllbet { get; set; } = true;

        public bool IsSyncAllbetUpline { get; set; } = true;

        public bool IsSyncAllbetDownline { get; set; } = true;

        public bool HideCheckBoxAllbet => !WebSettings.IsShowAllBet || (RoleId < 4 && IsHideAllbetUpline) || !IsSyncAllbetUpline;
    }
}
