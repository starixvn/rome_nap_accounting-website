using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary
{
    internal sealed class CreateLicenseeSubsidiaryCriteria : CriteriaBase
    {
        public CreateLicenseeSubsidiaryCriteria(
            string username,
            string existedUsername,
            int userId,
            string ip)
        {
            Username = username;
            ExistedUsername = existedUsername;
            Creator = userId;
            IP = ip;
        }

        public string Username { get; }

        public string ExistedUsername { get; }

        public int Creator { get; }

        public string IP { get; }

        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_Dep_CustomerInsLicenseeSubsidiary";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(ExistedUsername)
            && Creator > 0;
    }
}
