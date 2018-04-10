namespace Fanex.Tarus.BetList.Controllers
{
    using Business;
    using Criteria;
    using CustomerSetting.Module.Core.Attributes;
    using Fanex.Tarus.Security.Authenticate;
    using System.Web.Mvc;

    [SpecialSiteFilter]
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