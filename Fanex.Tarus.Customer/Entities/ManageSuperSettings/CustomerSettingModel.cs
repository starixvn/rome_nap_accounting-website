namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    using System.Collections.Generic;

    public class CustomerSettingModel
    {
        public GeneralInfoModel GeneralInfo { get; set; }

        public IEnumerable<ProductGroupModel> ProductGroups { get; set; }
    }
}
