namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    using Fanex.NAP.Framework.Data;

    public class GetHeadlineBelongSiteCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "Acc_CustomerInsMember_SiteHeadlineSel";
    }
}