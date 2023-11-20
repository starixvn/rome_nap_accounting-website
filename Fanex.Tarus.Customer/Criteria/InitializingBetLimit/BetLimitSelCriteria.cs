namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Requests.InitializingBetLimit;
    using Sunplus.Accounting.Core.Configurations;

    public class BetLimitSelCriteria : CriteriaBase
    {
        public BetLimitSelCriteria(GetBetLimitRequest request)
        {
            ProductID = request.ProductID;
            SportTypes = request.SportTypes;
            BetTypes = request.BetTypes;
            DepositCurrencies = request.DepositCurrency;
            CreditCurrencies = request.CreditCurrency;
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
            ExportData = 0;
            SettingStatus = WebSettings.SettingStatus;
        }
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

        public int SettingStatus { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_BetLimitSel";
    }
}
