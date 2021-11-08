namespace Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;

    public class ConfigSettingViewModel
    {
        public ConfigModel ConfigDetail { get; set; }

        public IEnumerable<SportBetTypeBySettingModel> SportBettypeBySettings { get; set; }

        public IEnumerable<ProductModel> ProductModels { get; set; }

        public IEnumerable<IGrouping<int, SettingInforModel>> GroupSettings { get; set; }

        public IEnumerable<BaseModel> Currencies { get; set; }

        public IEnumerable<BaseModel> Sites { get; set; }
    }
}
