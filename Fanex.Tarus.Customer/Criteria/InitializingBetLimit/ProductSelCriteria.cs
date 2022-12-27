namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;

    public class ProductSelCriteria : CriteriaBase
    {
        public ProductSelCriteria()
        {
            ProductID = 0;
        }

        public int ProductID { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_ProductSel";
    }
}
