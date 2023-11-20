namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    public class SportTypeSelCriteria : CriteriaBase
    {
        public SportTypeSelCriteria(int productId)
        {
            ProductId = productId;
            SportTypes = "0";
            SettingStatus = WebSettings.SettingStatus;
        }

        public int ProductId { get; set; }

        [SpParam(SqlParamName = "pMutilSportType")]
        public string SportTypes { get; set; }

        public int SettingStatus { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_SportTypeSel";
    }
}
