namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.CustomerSetting.Module.Core.Constants;
    using Fanex.CustomerSetting.Module.Core.Services.Impl;
    using Fanex.CustomerSetting.Module.Core.Services.Interface;
    using Fanex.Tarus.Customer.Business.ParlayMaxPayout;
    using Fanex.Tarus.Customer.Requests.ParlayMaxPayout;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;

    public class ParlayMaxPayoutController : BaseController
    {
        private readonly IParlayMaxPayoutService _parlayMaxPayoutService;
        private readonly IServiceBase _serviceBase = new ServiceBase();

        public ParlayMaxPayoutController() : this(null) { }

        public ParlayMaxPayoutController(IParlayMaxPayoutService parlayMaxPayoutService)
        {
            _parlayMaxPayoutService = parlayMaxPayoutService ?? new ParlayMaxPayoutService();
        }

        [AuthenticateRead]
        public ActionResult Index(ParlayMaxPayoutSelectRequest request) => View("~/Views/Customer/ParlayMaxPayout/Index.cshtml", _parlayMaxPayoutService.Select(request));

        [AuthenticateRead]
        public JsonResult Check(ParlayMaxPayoutUpdateRequest request) => Json(_parlayMaxPayoutService.Check(request));

        [AuthenticateHost]
        public JsonResult Update(ParlayMaxPayoutUpdateRequest request)
        {
            var checkUmResult = _serviceBase.CheckUM(request.CustId, NonDcsProduct.Sportsbook, "en-US");
            if (!checkUmResult.IsSuccess)
            {
                return Json(checkUmResult);
            }

            return Json(_parlayMaxPayoutService.Update(request));
        }
    }
}
