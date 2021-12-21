namespace Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;

    public class InitializingCustomerSettingViewModel
    {
        public IEnumerable<SettingConfigModel> ConfigSettings { get; set; }
        public IEnumerable<BaseModel> Steps { get; set; }
    }
}
