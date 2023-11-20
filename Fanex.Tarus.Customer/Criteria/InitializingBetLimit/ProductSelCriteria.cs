namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.Core.Configurations;

    public class ProductSelCriteria : CriteriaBase
    {
        public ProductSelCriteria()
        {
            ProductID = 0;
            SettingStatus = WebSettings.SettingStatus;
        }

        public int ProductID { get; set; }

        public int SettingStatus { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_ProductSel";
    }
}
