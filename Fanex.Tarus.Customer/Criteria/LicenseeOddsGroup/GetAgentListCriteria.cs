using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeOddsGroup
{
    public class GetAgentListCriteria : CriteriaBase
    {
        public GetAgentListCriteria(int superId)
        {
            SRecommend = superId;
        }

        public int SRecommend { get; }

        public override string GetSettingKey()
            => "Acc_Dep_LicenseeSelAgentList";

        public override bool IsValid()
            => SRecommend > 0;
    }
}
