using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;

namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    public class DeleteDepositConfigCriteria : DeleteConfigCriteria
    {
        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_SettingConfig_Del";
    }
}
