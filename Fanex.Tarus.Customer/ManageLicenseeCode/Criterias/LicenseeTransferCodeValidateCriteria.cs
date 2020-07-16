namespace Fanex.Tarus.Customer.ManageLicenseeCode.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class LicenseeTransferCodeValidateCriteria : CriteriaBase
    {
        public string LicenseeName { get; set; }

        public string LicenseeCode { get; set; }

        public override string GetSettingKey()
            => "Acc_LicenseeTransferCode_Verify";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(LicenseeName) && !string.IsNullOrWhiteSpace(LicenseeCode);
    }
}
