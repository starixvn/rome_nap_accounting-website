namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using BetDetails.BetBuilder.Criteria;
    using BetDetails.BetBuilder.Services;
    using DataAccess;

    public class BetBuilderController : Controller
    {
        private readonly IBetBuilderService service;

        public BetBuilderController()
            : this(null)
        {
        }

        public BetBuilderController(IBetBuilderService betBuilderService)
        {
            this.service = betBuilderService?? new BetBuilderService(new BetBuilderRepository());
        }

        public ActionResult ViewDetail(GetBetBuilderDetailParameter criteria)
        {
            var result = service.GetBetListDetail(criteria);

            return PartialView("~/Views/BetListDetails/BetBuilder/Index.cshtml", result);
        }        
    }
}
