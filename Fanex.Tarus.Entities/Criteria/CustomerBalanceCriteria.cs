using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria
{
    public class CustomerBalanceCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "CustomerSelClearBalance";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
