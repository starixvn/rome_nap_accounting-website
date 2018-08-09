namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdatePositionTakingCriteria : NonQueryCriteria
    {
        public UpdatePositionTakingCriteria(
            int productId,
            int superId,
            decimal superPT,
            int agentId,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ProductId = productId;
            SuperId = superId;
            PT = superPT;
            AgentId = agentId;
            IsAgentOnly = isAgentOnly;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int SuperId { get; }

        public decimal PT { get; }

        public int AgentId { get; }

        public bool IsAgentOnly { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_PositionTakingSettingUpd";

        public override bool IsValid()
            => base.IsValid() && ProductId > 0 && 0 <= PT && PT <= 100 && AgentId > 0;
    }
}
