namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        public bool HideAllbetUpline { get; private set; } = true;

        public bool SyncAllbet { get; private set; } = true;

        public bool SyncAllbetUpline { get; private set; } = true;

        public bool RemoveEnabledCheckBoxAllbet 
            => !WebSettings.IsShowAllBet || !SyncAllbetUpline || (RoleId < 4 && (HideAllbetUpline || !SyncAllbet));

        public bool RemoveSettingLinkAllbet
            => !WebSettings.IsShowAllBet || HideAllbetUpline || !SyncAllbetUpline;
    }
}
