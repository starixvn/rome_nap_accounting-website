using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;

namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting
{
    public class EditSettingConfigCriteria : AddSettingConfigCriteria
    {
        public EditSettingConfigCriteria(ConfigModel request) : base(request)
        {
            ConfigID = request.Config.ConfigId;
        }

        public int ConfigID { get; set; }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_SettingConfig_Upd";
    }
}
