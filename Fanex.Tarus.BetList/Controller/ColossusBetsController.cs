namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Entities.ColossusBets;

    public class ColossusBetsController : Controller
    {
        private readonly IColossusBetsService _colossusBetsService;

        public ColossusBetsController()
        {
            _colossusBetsService = new ColossusBetsService();
        }

        [HttpGet]
        public ActionResult BetDetails(GetColossusBetsBetDetailsCriteria criteria)
        {
            var result = new ColossusBetsDetailModel
            {
                MemberChoice = _colossusBetsService.MemberChoice(criteria),
                TicketDetails = _colossusBetsService.BetDetails(criteria)
            };

            return View("~/Areas/ColossusBets/Views/BetDetails/Index.cshtml", result);
        }
    }
}
