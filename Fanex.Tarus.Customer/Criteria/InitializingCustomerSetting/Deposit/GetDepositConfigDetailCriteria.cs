namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.NAP.Framework.Data;

    public class GetDepositConfigDetailCriteria : CriteriaBase
    {
        public int ConfigId { get; set; }

        public GetDepositConfigDetailCriteria(int configId)
        {
            ConfigId = configId;
        }

        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_SettingConfigDetail_Sel";

        public override bool IsValid()
            => ConfigId > 0;
    }
}
