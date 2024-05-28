using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetAgentCriteria : CriteriaBase
    {
        public GetAgentCriteria(string superId, int userId)
        {
            SRecommend = superId;
            UserID = userId;
        }

        public int UserID { get; set; }

        public string SRecommend { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelAgentListBySuper";

        public override bool IsValid()
            => SRecommend != "";
    }
}
