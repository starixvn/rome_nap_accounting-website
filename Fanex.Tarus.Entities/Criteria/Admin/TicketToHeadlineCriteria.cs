using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria
{
    public class TicketToHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "TicketToHeadline";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
