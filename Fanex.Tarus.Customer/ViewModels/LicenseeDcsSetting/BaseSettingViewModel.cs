namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public abstract class BaseSettingViewModel
    {
        public string Site { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int SuperId { get; set; }

        public string SuperName { get; set; }
    }
}
