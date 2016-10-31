using Fanex.Tarus.DataAccess.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System;
using System.Collections.Generic;

namespace Fanex.Tarus.DataAccess.Implement.Admin
{
    public class TicketToHeadlineRepository: ITicketToHeadlineRepository
    {
        public List<TicketToHeadline> GetData(TicketToHeadlineCriteria criteria)
        {
            var result = new List<TicketToHeadline>();
            

            return result;
        }
    }
}
