using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetAgentCriteria : CriteriaBase
    {
        public GetAgentCriteria(int superId)
        {
            SRecommend = superId;
        }

        public int SRecommend { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelAgentListBySuper";

        public override bool IsValid()
            => SRecommend > 0;
    }
}
