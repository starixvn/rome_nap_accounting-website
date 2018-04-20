using System.Collections;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.ExtraPtGroup
{
    public class GroupCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_GroupExtraPT_Group_Sel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
