using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.BetList.Criteria
{
    public class SelectProductCriteria : CriteriaBase
    {
        public int PageSize { get; set; } = 200;

        public int PageIndex { get; set; } = 1;

        public override string GetSettingKey() => "Acc_Product_Sel";
    }
}
