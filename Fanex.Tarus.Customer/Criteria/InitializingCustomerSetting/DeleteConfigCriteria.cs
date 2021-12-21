namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting
{
    using Fanex.NAP.Framework.Data;

    public class DeleteConfigCriteria : CriteriaBase
    {
        public int ConfigId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_SettingConfig_Del";

        public override bool IsValid()
            => ConfigId > 0;
    }
}
