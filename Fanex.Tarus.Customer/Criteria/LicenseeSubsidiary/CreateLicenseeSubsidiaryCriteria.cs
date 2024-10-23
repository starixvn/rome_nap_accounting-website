using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary
{
    internal sealed class CreateLicenseeSubsidiaryCriteria : CriteriaBase
    {
        public CreateLicenseeSubsidiaryCriteria(
            int custId,
            string username,
            string existedUsername,
            int existedCustId,
            int mRecommend,
            int userId,
            string ip,
            int custDBGroupId)
        {
            CustId = custId;
            Username = username;
            ExistedUsername = existedUsername;
            ExistedCustId = existedCustId;
            MRecommend = mRecommend;
            Creator = userId;
            IP = ip;
            CustDBGroupId = custDBGroupId;
        }

        public string Username { get; }

        public string ExistedUsername { get; }

        public int ExistedCustId { get; }

        public int MRecommend { get; }

        public int Creator { get; }

        public string IP { get; }

        public int CustId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public int CustDBGroupId { get; }

        public override string GetSettingKey()
        {
            if (CustDBGroupId == 0)
            {
                return "Acc_Dep_CustomerInsLicenseeSubsidiary";
            }

            return $"SiteDB{CustDBGroupId}_Acc_Dep_CustomerInsLicenseeSubsidiary";
        }

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(ExistedUsername)
            && Creator > 0;
    }
}