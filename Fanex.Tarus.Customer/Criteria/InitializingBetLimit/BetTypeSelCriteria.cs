namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    public class BetTypeSelCriteria : CriteriaBase
    {
        public BetTypeSelCriteria(int productId, string sportTypes)
        {
            ProductId = productId;
            SportTypes = sportTypes;
            BetTypes = "0";
            SettingStatus = WebSettings.SettingStatus;
        }

        public int ProductId { get; set; }

        [SpParam(SqlParamName = "pMutilSportType")]
        public string SportTypes { get; set; }

        [SpParam(SqlParamName = "pMutilBettypeType")]
        public string BetTypes { get; set; }

        public int SettingStatus { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_BetTypeSel";
    }
}
