namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class GetStakeLevelGroupCriteria : GetStakeLevelCriteria
    {
        public GetStakeLevelGroupCriteria(int productId, int custId)
            : base(productId, custId)
        {
        }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_StakeLevelGroupSelect";
    }
}
