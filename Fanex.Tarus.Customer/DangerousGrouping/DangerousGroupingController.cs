namespace Fanex.Tarus.Customer.DangerousGrouping
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Tarus.Customer.DangerousGrouping.Request;
    using Fanex.Tarus.Customer.DangerousGrouping.Services;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class DangerousGroupingController : BaseController
    {
        private readonly string indexPath = "~/Views/Customer/DangerousGrouping/Index.cshtml";

        private readonly IDangerousGroupingService service;

        public DangerousGroupingController()
        {
            service = new DangerousGroupingService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            var dangerLevels = service.GetDangerLevelList();

            return View(indexPath, dangerLevels);
        }

        [AuthenticateRead]
        public ActionResult CustomerDanger(string username)
        {
            ViewBag.CustomerDanger = service.GetCustomerDanger(username, AppSession.AccId, AppSession.AccUserName).CustomerDanger;
            var dangerLevels = service.GetDangerLevelList();

            return View(indexPath, dangerLevels);
        }

        [AuthenticateRead]
        [HttpGet]
        public JsonResult GetCustomerDanger(string username)
        {
            var customerdanger = service.GetCustomerDanger(username, AppSession.AccId, AppSession.AccUserName);

            return Json(customerdanger, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        [HttpPost]
        public JsonResult UpdateCustomerDanger([System.Web.Http.FromBody]CustomerDangerUpdateRequest customerDangerUpdateRequest)
        {
            var result = service.UpdateCustomerDanger(customerDangerUpdateRequest, AppSession.AccId, AppSession.AccUserName, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [AuthenticateRead]
        [HttpPost]
        public JsonResult ValidateUsernames([System.Web.Http.FromBody]ValidateUsernamesRequest validateUsernamesRequest)
        {
            var result = service.ValdiateUsernames(validateUsernamesRequest, AppSession.AccId, AppSession.AccUserName, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [AuthenticateHost]
        [HttpPost]
        public JsonResult UpdateMultipleCustomersDanger([System.Web.Http.FromBody] MultipleCustomersDangerUpdateRequest multipleCustomersDangerUpdateRequest)
        {
            var result = service.UpdateMultipleCustomersDanger(multipleCustomersDangerUpdateRequest, AppSession.AccId, AppSession.AccUserName, HttpContext.Request.UserHostAddress);

            return Json(result);
        }
    }
}