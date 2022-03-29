namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class SportTypeSelCriteria : CriteriaBase
    {
        public SportTypeSelCriteria(int productId)
        {
            ProductId = productId;
            SportTypes = "0";
        }

        public int ProductId { get; set; }

        [SpParam(SqlParamName = "pMutilSportType")]
        public string SportTypes { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_SportTypeSel";
    }
}
