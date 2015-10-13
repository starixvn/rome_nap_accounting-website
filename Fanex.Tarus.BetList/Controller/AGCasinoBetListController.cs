namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
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
            var result = _agCasinoService.BetDetails(criteria);
            return View("~/Areas/AGCasino/Views/BetDetails/Index.cshtml", result);
        }
    }
}
