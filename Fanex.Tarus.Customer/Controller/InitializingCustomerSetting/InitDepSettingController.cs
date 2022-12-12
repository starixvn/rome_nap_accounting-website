namespace Fanex.Tarus.Customer.Controller.InitializingCustomerSetting
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.Tarus.Customer.Business.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Requests.InitializingCustomerSetting;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class InitDepSettingController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Index.cshtml";
        private readonly string _depositViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/Index.cshtml";
        private readonly string _addEditDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_AddEditConfig.cshtml";
        private readonly string _configDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_ConfigDetail.cshtml";
        private readonly string _viewConfigDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_ViewConfig.cshtml";
        private readonly IInitializingDepositSettingService depositService;

        public InitDepSettingController()
        {
            depositService = new InitializingDepositSettingService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet]
        public ActionResult Deposit()
        {
            return PartialView(_depositViewPath);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetSetting()
            => Json(depositService.GetSetting(), JsonRequestBehavior.AllowGet);
        
        public ActionResult DeleteSetting(int configId)
        {
            var result = depositService.DeleteConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult AbortSetting(int configId)
        {
            var result = depositService.AbortConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpGet, AuthenticateHost]
        public ActionResult GetProductSportBettype()
        {
            var productMatrix = depositService.GetProductSportBettype();

            return PartialView(_addEditDepositPath, productMatrix);
        }

        [HttpGet, AuthenticateHost]
        public ActionResult GetConfigDetail(int configId)
        {
            var viewModel = depositService.GetConfigDetail(configId);
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            return PartialView(_addEditDepositPath, viewModel);
        }


        [HttpPost, AuthenticateHost]
        public ActionResult GenerateConfigSetting(GenerateConfigSettingRequest request)
        {
            var viewModel = depositService.GenerateConfigSetting(request);

            return PartialView(_configDepositPath, viewModel);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateAddConfigSetting(ConfigModel request)
        {
            var result = depositService.ValidateAddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult AddConfigSetting(ConfigModel request)
        {
            var result = depositService.AddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateUpdateConfigSetting(ConfigModel request)
        {
            var result = depositService.ValidateUpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult UpdateConfigSetting(ConfigModel request)
        {
            var result = depositService.UpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ViewConfigDetail(int configId)
        {
            var viewModel = depositService.GetConfigDetail(configId, true);
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            return PartialView(_viewConfigDepositPath, viewModel);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ParseBetSetting(int settingType, HttpPostedFileBase uploadFile)
        {
            var result = depositService.ParseBetSetting(uploadFile, settingType);

            return Json(result);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ViewBetSetting(int configId)
        {
            var result = depositService.GetBetSettings(configId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
