namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    public class StakeLevelCheckCriteria : StakeLevelInsCriteria
    {
        public StakeLevelCheckCriteria() : base() { }

        public int ProductId { get; set; }

        public int NotiCode { get; set; }

        public string NotiMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelCheck";
    }
}
