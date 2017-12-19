using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public class GetStakeLevelCriteria : CriteriaBase
    {
        public GetStakeLevelCriteria(int productId, int custId)
        {
            ProductId = productId;
            CustId = custId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_StakeLevelSelect";
        }

        public override bool IsValid()
        {
            return ProductId > 0 && CustId > 0;
        }
    }
}
