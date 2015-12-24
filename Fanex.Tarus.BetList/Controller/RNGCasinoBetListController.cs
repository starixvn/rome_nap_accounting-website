namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using Business.RNGCasino;
    using Fanex.Tarus.BetList.Criteria;

    public class RNGCasinoBetListController : Controller
    {
        private readonly IRNGCasinoService _rngCasinoService;

        public RNGCasinoBetListController()
        {
            _rngCasinoService = new RNGCasinoService();
        }

        [HttpGet]
        public ActionResult BetDetails(GetRNGCasinoBetDetailsCriteria criteria)
        {
            var result = _rngCasinoService.BetDetails(criteria);
            return View("~/Areas/RNGCasino/Views/BetDetails/Index.cshtml", result);
        }
    }
}
