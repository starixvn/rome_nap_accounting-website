namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetBetSettingWithMaxPerBallCriteria : GetBetSettingCriteria
    {
        public GetBetSettingWithMaxPerBallCriteria(int productId, int superId, int agentId) : base(productId, superId, agentId)
        {
        }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_Game_BetLimitFullLineSettingSelect";
    }
}
