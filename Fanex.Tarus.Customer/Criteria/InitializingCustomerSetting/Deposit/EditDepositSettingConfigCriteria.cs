using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;

namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    public class EditDepositSettingConfigCriteria : AddDepositSettingConfigCriteria
    {
        public EditDepositSettingConfigCriteria(ConfigModel request, string ip, int userId) : base(request, ip, userId)
        {
            ConfigID = request.Config.ConfigId;
        }

        public int ConfigID { get; set; }

        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_SettingConfig_Upd";

    }
}
