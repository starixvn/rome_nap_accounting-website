using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetSuperCriteria : CriteriaBase
    {
        public GetSuperCriteria(string site, string deniedList)
        {
            Site = site;
            DeniedList = deniedList;
        }

        public string DeniedList { get; }

        public string Site { get; }

        public override string GetSettingKey()
            => "NAP_LM_LicenseeSuperList";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Site);
    }
}
