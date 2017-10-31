using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class GetPositionTakingCriteria : CriteriaBase
    {
        public GetPositionTakingCriteria(int productId, int custId)
        {
            ProductId = productId;
            CustId = custId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_PositionTakingSettingSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0 && CustId > 0;
        }
    }
}
