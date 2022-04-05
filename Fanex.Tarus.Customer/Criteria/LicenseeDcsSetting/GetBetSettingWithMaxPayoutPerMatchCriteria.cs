namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetBetSettingWithMaxPayoutPerMatchCriteria : GetBetSettingCriteria
    {
        public GetBetSettingWithMaxPayoutPerMatchCriteria(int productId, int superId, int agentId) : base(productId, superId, agentId)
        {
        }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_Game_BetLimitFullLineSettingSelect";
    }
}
