using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetStatusCriteria : CriteriaBase
    {
        public GetStatusCriteria(int productId, int custId)
        {
            ProductId = productId;
            CustId = custId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_StatusSelect";

        public override bool IsValid()
            => ProductId > 0 && CustId > 0;
    }
}
