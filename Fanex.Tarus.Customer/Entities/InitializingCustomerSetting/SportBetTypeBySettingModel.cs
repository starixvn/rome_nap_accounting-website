namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class SportBetTypeBySettingModel
    {
        public int SettingId { get; set; }
        public List<SportModel> Sports { get; set; }
    }
}
