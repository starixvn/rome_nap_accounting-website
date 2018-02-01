namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class StatusViewModel : BaseSettingViewModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public byte BitmapOrder { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsDisabledDownline { get; set; }

        public bool HasDownline { get; set; }
    }
}
