namespace Fanex.Tarus.Customer.Criteria.ManageSuperSettings
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    public class CheckSuperListCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string SuperList { get; set; }

        public bool IsCopy { get; set; }

        public string Mode { get; set; }

        public string DeniedList { get; set; }

        public byte SettingStatus => WebSettings.SettingStatus;

        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_CustomerCheckSuperList";

        public override bool IsValid()
            => CustId > 0;
    }
}
