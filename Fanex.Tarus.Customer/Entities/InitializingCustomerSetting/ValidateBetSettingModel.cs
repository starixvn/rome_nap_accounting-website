using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    public class ValidateBetSettingModel
    {
        public ValidateBetSettingModel()
        {
            ErrorMessage = string.Empty;
            BetSettings = new List<BetSettingModel>();
        }
        public string ErrorMessage { get; set; }

        public int SettingType { get; set; }

        public IEnumerable<BetSettingModel> BetSettings { get; set; }
    }
}
