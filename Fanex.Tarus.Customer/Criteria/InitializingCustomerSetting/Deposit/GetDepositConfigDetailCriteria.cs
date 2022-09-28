namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;

    public class GetDepositConfigDetailCriteria : GetConfigDetailCriteria
    {
        public GetDepositConfigDetailCriteria(int configId) : base(configId)
        {
        }

        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_SettingConfigDetail_Sel";
    }
}
