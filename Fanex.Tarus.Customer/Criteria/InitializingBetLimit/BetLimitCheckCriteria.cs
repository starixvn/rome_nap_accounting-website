namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    public class BetLimitCheckCriteria : BetLimitInsCriteria
    {
        public BetLimitCheckCriteria() : base() { }

        public int ProductId { get; set; }

        public int NotiCode { get; set; }

        public string NotiMsg { get; set; }

        public bool IsCheck { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_BetLimitCheck";
    }
}
