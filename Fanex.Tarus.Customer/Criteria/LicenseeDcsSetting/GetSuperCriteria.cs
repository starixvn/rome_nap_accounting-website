using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class GetSuperCriteria : CriteriaBase
    {
        public GetSuperCriteria(string site)
        {
            Site = site;
        }

        public string Site { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LM_LicenseeSuperList";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Site);
        }
    }
}
