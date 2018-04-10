namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public sealed class GetLicenseeBonusDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefCode { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_LicenseeBonus_Detail";
        }

        public override bool IsValid()
        {
            return TransId > 0 && RefCode > 0;
        }
    }
}
