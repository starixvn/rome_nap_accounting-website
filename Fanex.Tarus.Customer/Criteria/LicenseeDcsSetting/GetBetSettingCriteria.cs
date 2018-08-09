using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetBetSettingCriteria : CriteriaBase
    {
        public GetBetSettingCriteria(int productId, int superId, int agentId)
        {
            ProductId = productId;
            SuperId = superId;
            AgentId = agentId;
        }

        public int ProductId { get; }

        public int SuperId { get; }

        public int AgentId { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_BetLimitFullLineSettingSelect";

        public override bool IsValid()
            => ProductId > 0 && SuperId > 0 && AgentId > 0;
    }
}
