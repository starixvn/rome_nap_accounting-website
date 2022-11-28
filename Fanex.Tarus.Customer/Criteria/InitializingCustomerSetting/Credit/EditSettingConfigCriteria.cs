namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit
{
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    
    public class EditSettingConfigCriteria : AddSettingConfigCriteria
    {
        public EditSettingConfigCriteria(ConfigModel request, string ip, int userId) : base(request, ip, userId)
        {
            ConfigID = request.Config.ConfigId;
        }

        public int ConfigID { get; set; }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_SettingConfig_Upd";
    }
}
