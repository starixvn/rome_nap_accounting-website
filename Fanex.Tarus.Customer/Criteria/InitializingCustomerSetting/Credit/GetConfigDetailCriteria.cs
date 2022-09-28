namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit
{
    using Fanex.NAP.Framework.Data;

    public class GetConfigDetailCriteria : CriteriaBase
    {
        public GetConfigDetailCriteria(int configId)
        {
            ConfigId = configId;
        }

        public int ConfigId { get; set; }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_SettingConfigDetail_Sel";

        public override bool IsValid()
            => ConfigId > 0;
    }
}
