namespace Sunplus.Accounting.Business.Customer
{
    using Core.Configurations;

    public partial class CustomerInfoViewModel
    {
        private bool HideBitcoinUpline { get; set; } = true;

        private bool SyncBitcoin { get; set; } = true;

        public bool SyncBitcoinUpline { get; private set; } = true;

        public bool HideEnabledCheckBoxBitcoin
            => !WebSettings.IsShowBitcoinBetting || IsDeposit || !SyncBitcoinUpline || (SearchAccountRoleId < 4 && (HideBitcoinUpline || !SyncBitcoin));

        public bool HideSettingLinkBitcoinBetting
            => !WebSettings.IsShowBitcoinBetting || IsDeposit || HideBitcoinUpline || !SyncBitcoinUpline;
    }
}
