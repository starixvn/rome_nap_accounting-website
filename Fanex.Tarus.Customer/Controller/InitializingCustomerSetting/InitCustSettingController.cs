using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.InitializingCustomerSetting;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller.InitializingCustomerSetting
{
    public class InitCustSettingController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/InitializingCustomerSetting/Index.cshtml";
        private readonly IInitializingCustomerSettingService service;

        public InitCustSettingController()
        {
            service = new InitializingCustomerSettingService();
        }

        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetSetting()
        {
            return Json(service.GetSetting(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult DeleteSetting(int configId)
        {
            return Json(service.DeleteConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult AbortSetting(int configId)
        {
            return Json(service.AbortConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress));
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetProductSportBettype()
        {
            return Json(service.GetProductSportBettype(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetConfigDetail(int configId)
        {
            return Json(service.GetConfigDetail(configId), JsonRequestBehavior.AllowGet);
        }
    }
}
