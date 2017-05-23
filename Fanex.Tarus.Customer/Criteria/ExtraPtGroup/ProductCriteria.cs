using System.Collections;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.ExtraPtGroup
{
    public class ProductCriteria : CriteriaBase
    {
        public bool IsExtra { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GroupExtraPT_Product_Sel";
        }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("isextra", IsExtra);

            return param;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
