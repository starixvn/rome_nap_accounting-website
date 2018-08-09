using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetSuperCriteria : CriteriaBase
    {
        public GetSuperCriteria(string site)
        {
            Site = site;
        }

        public string Site { get; }

        public override string GetSettingKey()
            => "NAP_LM_LicenseeSuperList";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Site);
    }
}
