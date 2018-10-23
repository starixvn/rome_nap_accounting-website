namespace Fanex.Tarus.BetList.Controllers
{
    using System.Web.Mvc;
    using Business;
    using Criteria;
    using Fanex.Tarus.Security.Authenticate;

    public class ExchangeBetListController : Controller
    {
        private readonly IExchangeBetListService exchangeService;

        public ExchangeBetListController()
        {
            this.exchangeService = new ExchangeBetListService();
        }
        
        [AuthorizeFunctions("ViewInfo")]
        public ActionResult GetData(ExchangeBetListCriteria criteria)
        {
            var tickets = exchangeService.GetBetList(criteria);

            return PartialView("~/Views/BetList/BetlistByCustInfo/Exchange/Index.cshtml", tickets);
        }

        [AuthorizeFunctions("ViewInfo")]
        public ActionResult GetDetails(ExchangeBetDetailCriteria criteria)
        {
            var details = exchangeService.GetBetDetail(criteria);
            return PartialView("~/Views/BetList/BetlistByCustInfo/Exchange/Details.cshtml", details);
        }
    }
}