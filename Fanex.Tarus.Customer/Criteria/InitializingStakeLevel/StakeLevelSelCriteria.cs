namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class StakeLevelSelCriteria : CriteriaBase
    {
        public int ProductID { get; set; }

        public string SportTypes { get; set; }

        public string BetTypes { get; set; }

        public string DepositCurrencies { get; set; }

        public string CreditCurrencies { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelSel";
    }
}
