namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.NAP.Framework.Data;
    
    public class GetDepositBetSettingCriteria : CriteriaBase
    {
        public GetDepositBetSettingCriteria(int configId)
        {
            ConfigId = configId;
        }

        public int ConfigId { get; set; }

        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_BetSetting_Sel";

        public override bool IsValid()
            => ConfigId > 0;
    }
}
