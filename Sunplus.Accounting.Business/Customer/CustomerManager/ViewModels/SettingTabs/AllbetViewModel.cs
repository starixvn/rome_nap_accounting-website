namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideLiveCasinoUpline { get; set; } = true;

        private bool SyncLiveCasino { get; set; } = true;

        public bool SyncLiveCasinoUpline { get; private set; } = true;

        public bool HideEnabledCheckBoxLiveCasino
            => !WebSettings.IsShowLiveCasino || IsDeposit || !SyncLiveCasinoUpline || (SearchAccountRoleId < 4 && (HideLiveCasinoUpline || !SyncLiveCasino));

        public bool HideSettingLinkLiveCasino
            => !WebSettings.IsShowLiveCasino || IsDeposit || HideLiveCasinoUpline || !SyncLiveCasinoUpline;
    }
}
