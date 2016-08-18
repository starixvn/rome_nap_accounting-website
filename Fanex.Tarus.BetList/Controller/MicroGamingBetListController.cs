namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using Entities.BetList;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Criteria;

    public class MicroGamingBetListController : Controller
    {
        private readonly IMicroGamingService _microGamingService;

        public MicroGamingBetListController()
        {
            _microGamingService = new MicroGamingService();
        }

        [HttpGet]
        public ActionResult BetDetails(GetMicroGamingBetDetailsCriteria criteria)
        {
            var result = GetBetDetailReport(criteria);

            return View("~/Areas/MicroGaming/Views/BetDetails/Index.cshtml", result);
        }

        [HttpGet]
        public ActionResult BetDetailsPaging(GetMicroGamingBetDetailsCriteria criteria)
        {
            var result = GetBetDetailReport(criteria);

            return PartialView("~/Areas/MicroGaming/Views/BetDetails/_Tickets.cshtml", result);
        }

        [NonAction]
        private BetDetail GetBetDetailReport(GetMicroGamingBetDetailsCriteria criteria)
        {
            criteria.PageIndex++;

            if (criteria.PageSize == 0)
            {
                criteria.PageSize = 100;
            }

            return new BetDetail
            {
                TicketDetails = _microGamingService.BetDetails(criteria),
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRows = criteria.TotalRows
            };
        }
    }
}
