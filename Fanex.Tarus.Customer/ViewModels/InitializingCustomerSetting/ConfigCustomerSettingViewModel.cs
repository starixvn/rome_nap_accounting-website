namespace Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting
{
    using Fanex.Tarus.Admin.ViewModel;
    using System.Collections.Generic;

    public class ConfigCustomerSettingViewModel : ConfigSettingViewModel
    {
        public IEnumerable<SiteViewModel> SiteList { get; set; }
    }
}
