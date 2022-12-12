namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit
{
    using Fanex.NAP.Framework.Data;
    
    public class DeleteDepositConfigCriteria : CriteriaBase
    {
        public int ConfigId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => ConfigId > 0;

        public override string GetSettingKey()
            => "Acc_Dep_Initial_DCS_SettingConfig_Del";
    }
}
