namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class BetTypeSelCriteria : CriteriaBase
    {
        public BetTypeSelCriteria(int productId, string sportTypes)
        {
            ProductId = productId;
            SportTypes = sportTypes;
            BetTypes = "0";
        }

        public int ProductId { get; set; }

        [SpParam(SqlParamName = "pMutilSportType")]
        public string SportTypes { get; set; }

        [SpParam(SqlParamName = "pMutilBettypeType")]
        public string BetTypes { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_BetTypeSel";
    }
}
