using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Entities.Criteria.Admin;
using Fanex.Tarus.Web.Models.Admin;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Tarus.DataAccess.Interface.Admin
{
    public interface ITicketToHeadlineRepository
    {
        List<SelectListItem> GetSuportedProduct(GetTicketToHeadlineSupportedProductCriteria criteria);

        TicketToHeadlineViewModel GetData(TicketToHeadlineCriteria criteria);

        TicketToHeadlineViewModel GetFakeData(int? pageSize, int? pageIndex, int rowNo = 400);

        void RePaginationData(List<TicketToHeadline> data, int pageSize);
    }
}
