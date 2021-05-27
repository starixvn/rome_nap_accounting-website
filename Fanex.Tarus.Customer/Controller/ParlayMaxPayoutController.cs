namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Business.ParlayMaxPayout;
    using Fanex.Tarus.Customer.Requests.ParlayMaxPayout;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Web.Controllers;
    using Sunplus.Accounting.Core.Common;
    using System.Web.Mvc;

    public class ParlayMaxPayoutController : BaseController
    {
        private readonly IParlayMaxPayoutService _parlayMaxPayoutService;

        public ParlayMaxPayoutController() : this(null) { }

        public ParlayMaxPayoutController(IParlayMaxPayoutService parlayMaxPayoutService)
        {
            _parlayMaxPayoutService = parlayMaxPayoutService ?? new ParlayMaxPayoutService();
        }

        public ActionResult Index(ParlayMaxPayoutSelectRequest request)
        {
            var model = _parlayMaxPayoutService.Select(request);
            return View("~/Views/Customer/ParlayMaxPayout/Index.cshtml", _parlayMaxPayoutService.Select(request));
        }

        public JsonResult Update(ParlayMaxPayoutUpdateRequest request)
        {
            return Json(_parlayMaxPayoutService.Update(request));
        }
    }
}
