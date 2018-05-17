namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using Entities.BetList;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Criteria;

    public class AGCasinoBetListController : Controller
    {
        private readonly IAGCasinoService _agCasinoService;

        public AGCasinoBetListController()
        {
            _agCasinoService = new AGCasinoService();
        }

        [HttpGet]
        public ActionResult BetDetails(GetAGCasinoBetDetailsCriteria criteria)
        {
            var result = GetBetDetail(criteria);

            return View("~/Areas/AGCasino/Views/BetDetails/Index.cshtml", result);
        }

        [HttpGet]
        public ActionResult BetDetailsPaging(GetAGCasinoBetDetailsCriteria criteria)
        {
            var result = GetBetDetail(criteria);

            return View("~/Areas/AGCasino/Views/BetDetails/_Tickets.cshtml", result);
        }

        [NonAction]
        private BetDetail GetBetDetail(GetAGCasinoBetDetailsCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.PageSize == 0)
            {
                criteria.PageSize = 100;
            }

            var ticketDetails = _agCasinoService.BetDetails(criteria);

            return new BetDetail
            {
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalRows = criteria.TotalRows,
                TicketDetails = ticketDetails
            };
        }
    }
}
