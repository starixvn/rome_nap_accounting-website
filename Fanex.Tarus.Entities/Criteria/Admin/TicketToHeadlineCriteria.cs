using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria
{
    public class TicketToHeadlineCriteria : CriteriaBase
    {
        public bool ExportExcel { get; set; }

        public string File { get; set; }

        public byte HeadlineType { get; set; }

        public int ProductId { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelGroupHeadlineByRefno";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
