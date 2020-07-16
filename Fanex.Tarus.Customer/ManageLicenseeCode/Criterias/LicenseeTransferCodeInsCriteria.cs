namespace Fanex.Tarus.Customer.ManageLicenseeCode.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class LicenseeTransferCodeInsCriteria : CriteriaBase
    {
        public string LicenseeName { get; set; }

        public string LicenseeCode { get; set; }

        public bool CodeStatus { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
            => "Acc_LicenseeTransferCode_Ins";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(LicenseeName) && !string.IsNullOrWhiteSpace(LicenseeCode);
    }
}
