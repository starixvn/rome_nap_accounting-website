using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Requests.InitializingBetLimit;
using Sunplus.Accounting.Core.Configurations;

namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    public class ProductCurrencyMatrixCriteria : CriteriaBase
    {
        public ProductCurrencyMatrixCriteria(GetProductCurrencyRequest request)
        {
            ProductIds = request.ProductIds;
            Currencies = request.Currencies;
            SettingStatus = WebSettings.SettingStatus;
        }

        [SpParam(SqlParamName = "pMutilProductID")]
        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "pMutilCurrency")]
        public string Currencies { get; set; }

        public int SettingStatus { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_ProductCurrencySel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(ProductIds) || !string.IsNullOrWhiteSpace(Currencies);
    }
}
