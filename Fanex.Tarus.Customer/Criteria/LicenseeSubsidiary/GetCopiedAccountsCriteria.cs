using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary
{
    internal class GetCopiedAccountsCriteria : CriteriaBase
    {
        public GetCopiedAccountsCriteria(string username, int number)
        {
            Username = username;
            Num = number;
        }

        public string Username { get; }

        public int Num { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustomerGenLicenseeSubUsername";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && 0 < Num && Num <=500;
    }
}
