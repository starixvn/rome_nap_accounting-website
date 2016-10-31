using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System.Collections.Generic;

namespace Fanex.Tarus.Business.Interface.Admin
{
    public interface ITicketToHeadlineService
    {
        List<TicketToHeadline> GetData(TicketToHeadlineCriteria criteria);
    }
}
