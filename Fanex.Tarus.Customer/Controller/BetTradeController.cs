namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Customer.Business.BetTrade;
    using Fanex.Tarus.Customer.Criteria.BetTrade;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Sunplus.Accounting.Core.Common;

    public class BetTradeController : BaseController
    {
        private readonly IBetTradeService _betTradeService;

        public BetTradeController()
            : this(null)
        {
        }

        public BetTradeController(IBetTradeService betTradeService)
        {
            _betTradeService = betTradeService ?? new BetTradeService();
        }

        [AuthenticateRead]
        public ActionResult UpdateStatusView(GetBetTradeStatusCriteria criteria)
        {
            var status = _betTradeService.GetStatus(criteria);
            var model = JsonConvert.SerializeObject(
                status,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.HasHostPermission = PermissionService.HasHostPermission(Enumerations.Functions.Customer.ViewInfo) ? 1 : 0;
            return View("~/Areas/CustomerSetting/Views/BetTrade/UpdateStatus/Index.cshtml", null, model);
        }

        [HttpPost]
        [AuthenticateHost]
        public ActionResult UpdateStatus(UpdateBetTradeStatusCriteria criteria)
        {
            var result = _betTradeService.UpdateStatus(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
