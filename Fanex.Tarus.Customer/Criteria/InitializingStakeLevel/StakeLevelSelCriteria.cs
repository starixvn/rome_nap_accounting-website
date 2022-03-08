namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class StakeLevelSelCriteria : CriteriaBase
    {
        public int ProductID { get; set; }

        [SpParam(SqlParamName = "pMutilSportType")]
        public string SportTypes { get; set; }

        [SpParam(SqlParamName = "pMutilBetType")]
        public string BetTypes { get; set; }

        [SpParam(SqlParamName = "pMutilCurrencyDeposit")]
        public string DepositCurrencies { get; set; }

        [SpParam(SqlParamName = "pMutilCurrencyCredit")]
        public string CreditCurrencies { get; set; }

        public int ExportData { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelSel";
    }
}
