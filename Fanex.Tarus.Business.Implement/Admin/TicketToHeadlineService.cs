using Fanex.Tarus.Business.Interface.Admin;
using Fanex.Tarus.DataAccess.Implement.Admin;
using Fanex.Tarus.DataAccess.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System.Collections.Generic;

namespace Fanex.Tarus.Business.Implement.Admin
{
    public class TicketToHeadlineService : ITicketToHeadlineService
    {
        private ITicketToHeadlineRepository _ticketToHeadlineRepository;

        public TicketToHeadlineService()
        {
            this._ticketToHeadlineRepository = new TicketToHeadlineRepository();
        }

        public List<TicketToHeadline> GetData(TicketToHeadlineCriteria criteria)
        {
            return this._ticketToHeadlineRepository.GetData(criteria);
        }
    }
}
