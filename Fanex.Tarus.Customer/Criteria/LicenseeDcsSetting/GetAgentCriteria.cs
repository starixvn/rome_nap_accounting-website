using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetAgentCriteria : CriteriaBase
    {
        public GetAgentCriteria(string superId)
        {
            SRecommend = superId;
        }

        public string SRecommend { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelAgentListBySuper";

        public override bool IsValid()
            => SRecommend != "";
    }
}
