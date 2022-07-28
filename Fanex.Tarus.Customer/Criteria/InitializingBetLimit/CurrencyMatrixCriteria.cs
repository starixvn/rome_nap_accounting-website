namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;

    public class CurrencyMatrixCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "pMutilProductID")]
        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "pMutilCurrency")]
        public string CurrencyId { get; set; }

        [SpParam(SqlParamName = "ExportData")]
        public int IsExportData { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_ProductCurrencySel";
    }
}
