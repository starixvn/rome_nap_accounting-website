using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class StatusViewModel : BaseSettingViewModel
    {
        public bool IsAgentOnly { get; set; }

        public StatusEntity Status { get; set; }
    }
}
