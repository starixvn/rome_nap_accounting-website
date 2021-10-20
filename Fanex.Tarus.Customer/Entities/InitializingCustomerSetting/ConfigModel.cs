namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class ConfigModel
    {
        public SettingConfigModel Config { get; set; }

        public IEnumerable<SettingInforModel> Settings { get; set; }
    }
}
