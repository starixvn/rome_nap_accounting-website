namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Customer.Business.MicroGaming;
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Sunplus.Accounting.Core.Common;

    public class MicroGamingController : BaseController
    {
        private readonly IMicroGamingService _microGamingService;

        public MicroGamingController()
            : this(null)
        {
        }

        public MicroGamingController(IMicroGamingService microGamingService)
        {
            _microGamingService = microGamingService ?? new MicroGamingService();
        }

        [AuthenticateRead]
        public ActionResult UpdateStatusView(GetMicroGamingStatusCriteria criteria)
        {
            var goldDeluxeStatus = _microGamingService.GetStatus(criteria);
            var model = JsonConvert.SerializeObject(
                goldDeluxeStatus,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.HasHostPermission = PermissionService.HasHostPermission(Enumerations.Functions.Customer.ViewInfo) ? 1 : 0;
            return View("~/Areas/CustomerSetting/Views/MicroGaming/UpdateStatus/Index.cshtml", null, model);
        }

        [HttpPost]
        [AuthenticateHost]
        public ActionResult UpdateStatus(UpdateMicroGamingStatusCriteria criteria)
        {
            var result = _microGamingService.UpdateStatus(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [AuthenticateRead]
        public ActionResult EditSetting(EditSettingCriteria criteria)
        {
            return View("~/Areas/CustomerSetting/Views/MicroGaming/EditSettig/Index.cshtml");
        }
    }
}
