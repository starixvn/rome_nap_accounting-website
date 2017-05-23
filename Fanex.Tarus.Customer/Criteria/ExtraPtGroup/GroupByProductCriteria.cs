using System.Collections;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.ExtraPtGroup
{
    public class GroupByProductCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GroupExtraPT_GroupByProduct_Sel";
        }
        
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("productid", ProductId);

            return param;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
