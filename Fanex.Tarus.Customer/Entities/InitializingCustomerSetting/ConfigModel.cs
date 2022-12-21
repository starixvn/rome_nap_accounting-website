namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class ConfigModel
    {
        public SettingConfigModel Config { get; set; }

        public List<SettingInforModel> Settings { get; set; }

        public List<BetSettingModel> BetSettings { get; set; }
    }
}
