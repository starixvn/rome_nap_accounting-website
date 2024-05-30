using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetSuperCriteria : CriteriaBase
    {
        public GetSuperCriteria(string site, int userId)
        {
            Site = site;
            UserId = userId;
        }

        public int UserId { get; }

        public string Site { get; }

        public override string GetSettingKey()
            => "NAP_LM_LicenseeSuperList";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Site);
    }
}
