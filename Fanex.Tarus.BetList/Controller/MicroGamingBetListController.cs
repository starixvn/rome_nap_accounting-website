namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
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
            var result = _microGamingService.BetDetails(criteria);
            return View("~/Areas/MicroGaming/Views/BetDetails/Index.cshtml", result);
        }
    }
}
