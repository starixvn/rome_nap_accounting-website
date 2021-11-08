using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.InitializingCustomerSetting;
using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
using Fanex.Tarus.Customer.Requests.InitializingCustomerSetting;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller.InitializingCustomerSetting
{
    public class InitCustSettingController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/InitializingCustomerSetting/Index.cshtml";
        private readonly string _viewConfigPath = "~/Views/InitializingCustomerSetting/_ViewConfig.cshtml";
        private readonly string _configPath = "~/Views/InitializingCustomerSetting/_ConfigDetail.cshtml";
        private readonly string _addEditPath = "~/Views/InitializingCustomerSetting/_AddEditConfig.cshtml";
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

        [HttpGet, AuthenticateHost]
        public ActionResult GetProductSportBettype()
        {
            return PartialView(_addEditPath, service.GetProductSportBettype());
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetConfigDetail(int configId)
        {
            return PartialView(_addEditPath, service.GetConfigDetail(configId));
        }


        [HttpPost, AuthenticateHost]
        public ActionResult GenerateConfigSetting(GenerateConfigSettingRequest request)
        {
            return PartialView(_configPath, service.GenerateConfigSetting(request));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult ValidateAddConfigSetting(ConfigModel request)
        {
            return Json(service.ValidateAddConfigSetting(request));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult AddConfigSetting(ConfigModel request)
        {
            return Json(service.AddConfigSetting(request));
        }
        
        [HttpPost, AuthenticateHost]
        public ActionResult UpdateConfigSetting(ConfigModel request)
        {
            return Json(service.UpdateConfigSetting(request));
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ViewConfigDetail(int configId)
        {
            return PartialView(_viewConfigPath, service.GetConfigDetail(configId));
        }
    }
}
