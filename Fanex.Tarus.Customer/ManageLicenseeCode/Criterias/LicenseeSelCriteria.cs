namespace Fanex.Tarus.Customer.ManageLicenseeCode.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class LicenseeSelCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_LicenseeSel";

        public override bool IsValid()
            => true;
    }
}
