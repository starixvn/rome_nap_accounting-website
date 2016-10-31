using Fanex.Tarus.Business.Implement.Admin;
using Fanex.Tarus.Business.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers.Admin
{
    public class TicketToHeadlineController : BaseController
    {
        private ITicketToHeadlineService _ticketToHeadlineService;

        public TicketToHeadlineController()
        {
            this._ticketToHeadlineService = new TicketToHeadlineService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData(TicketToHeadlineCriteria criteria)
        {
            return Json(_ticketToHeadlineService.GetData(criteria));
        }
    }
}
