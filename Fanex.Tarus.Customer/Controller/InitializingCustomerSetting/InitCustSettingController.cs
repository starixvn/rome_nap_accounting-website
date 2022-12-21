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
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Index.cshtml";
        private readonly string _creditViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/Index.cshtml";
        private readonly string _viewConfigPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_ViewConfig.cshtml";
        private readonly string _configPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_ConfigDetail.cshtml";
        private readonly string _addEditPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_AddEditConfig.cshtml";
        private readonly IInitializingCustomerSettingService service;

        public InitCustSettingController()
        {
            service = new InitializingCustomerSettingService();
        }

        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet]
        public ActionResult Credit()
        {
            return PartialView(_creditViewPath);
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

        [HttpGet, AuthenticateHost]
        public ActionResult GetConfigDetail(int configId)
        {
            var viewModel = service.GetConfigDetail(configId);
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            return PartialView(_addEditPath, viewModel);
        }


        [HttpPost, AuthenticateHost]
        public ActionResult GenerateConfigSetting(GenerateConfigSettingRequest request)
        {
            return PartialView(_configPath, service.GenerateConfigSetting(request));
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateAddConfigSetting(ConfigModel request)
        {
            return Json(service.ValidateAddConfigSetting(request, HttpContext.Request.UserHostAddress));
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult AddConfigSetting(ConfigModel request)
        {
            return Json(service.AddConfigSetting(request, HttpContext.Request.UserHostAddress));
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateUpdateConfigSetting(ConfigModel request)
        {
            return Json(service.ValidateUpdateConfigSetting(request, HttpContext.Request.UserHostAddress));
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult UpdateConfigSetting(ConfigModel request)
        {
            return Json(service.UpdateConfigSetting(request, HttpContext.Request.UserHostAddress));
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ViewConfigDetail(int configId)
        {
            var viewModel = service.GetConfigDetail(configId);
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            return PartialView(_viewConfigPath, viewModel);
        }
    }
}
