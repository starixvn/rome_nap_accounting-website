namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.MicroGaming;
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

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
        public ActionResult EditSettingView(GetSettingCriteria criteria)
        {
            return View(
                "~/Areas/CustomerSetting/Views/MicroGaming/EditSetting/Index.cshtml",
                _microGamingService.CustomerSetting(criteria));
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult EditSetting(EditSettingCriteria criteria)
        {
            var result = _microGamingService.EditSetting(criteria);
            return Json(result);
        }
    }
}
        }

        [HttpPost]
        [AuthenticateHost]
        public ActionResult UpdateStatus(UpdateMicroGamingStatusCriteria criteria)
        {
            var result = _microGamingService.UpdateStatus(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
