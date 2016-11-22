using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Admin;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Tarus.Business.Interface.Admin
{
    public interface ITicketToHeadlineService
    {
        TicketToHeadlineViewModel GetData(HttpPostedFileBase uploadFile, TicketToHeadlineCriteria criteria);

        TicketToHeadlineViewModel GetFakeData(int? pageSize, int? pageIndex, int rowNo = 400);
    }
}
