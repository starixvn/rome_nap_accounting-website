namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Criteria;

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
            string result = _colossusBetsService.BetDetails(criteria);
            return View("~/Areas/ColossusBets/Views/BetDetails/Index.cshtml", null, result);
        }
    }
}
