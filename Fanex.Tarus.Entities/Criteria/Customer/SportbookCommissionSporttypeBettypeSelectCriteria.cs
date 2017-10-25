using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria.Customer
{
    public class SportbookCommissionSporttypeBettypeSelectCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CustomerNoCommSelBettypes";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
