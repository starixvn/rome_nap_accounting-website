using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.DataAccess.Interface.Admin
{
    public interface ITicketToHeadlineRepository
    {
        TicketToHeadlineViewModel GetData(TicketToHeadlineCriteria criteria);

        TicketToHeadlineViewModel GetFakeData(int? pageSize, int? pageIndex, int rowNo = 400);

        void RePaginationData(List<TicketToHeadline> data, int pageSize);
    }
}
