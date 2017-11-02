namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool IsHideAllBetForCredit { get; private set; } = true;

        public bool IsHideAllbetUpline { get; private set; } = true;

        public bool IsSyncAllbet { get; private set; } = true;

        public bool IsSyncAllbetUpline { get; private set; } = true;

        public bool HideCheckBoxAllbet => !WebSettings.IsShowAllBet ||IsHideAllbetUpline || !IsSyncAllbetUpline;
    }
}
