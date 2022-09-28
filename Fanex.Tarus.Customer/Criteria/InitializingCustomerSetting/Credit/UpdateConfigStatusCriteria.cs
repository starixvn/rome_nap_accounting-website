namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit
{
    using Fanex.NAP.Framework.Data;

    public class UpdateConfigStatusCriteria : CriteriaBase
    {
        public int ConfigId { get; set; }

        public int Status { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_Initial_DCS_ConfigStatus_Upd";

        public override bool IsValid()
            => ConfigId > 0;
    }
}
