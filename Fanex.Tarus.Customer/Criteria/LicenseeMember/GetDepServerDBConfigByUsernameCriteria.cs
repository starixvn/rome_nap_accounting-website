using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    public class GetDepServerDBConfigByUsernameCriteria : CriteriaBase
    {
        public GetDepServerDBConfigByUsernameCriteria(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public override string GetSettingKey() => "Acc_Dep_ServerDBConfigSelByCust";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Username);
    }
}