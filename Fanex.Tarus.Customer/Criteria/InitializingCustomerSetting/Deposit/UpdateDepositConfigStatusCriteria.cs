namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;

    public class UpdateDepositConfigStatusCriteria : UpdateConfigStatusCriteria
    {
        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_ConfigStatus_Upd";
    }
}
