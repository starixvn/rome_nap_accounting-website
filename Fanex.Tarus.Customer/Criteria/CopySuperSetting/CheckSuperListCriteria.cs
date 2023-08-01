namespace Fanex.Tarus.Customer.Criteria.CopySuperSetting
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    public class CheckSuperListCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string SuperList { get; set; }

        public string ProductList { get; set; }

        public int CustId { get; set; }

        public byte SettingStatus => WebSettings.SettingStatus;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_CopySetting_CustomerCheckSuperList";

        public override bool IsValid()
            => CustId > 0;
    }
}
