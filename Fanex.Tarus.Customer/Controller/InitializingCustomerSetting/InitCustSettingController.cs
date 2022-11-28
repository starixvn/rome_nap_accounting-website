using System.Web;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Tarus.Customer.Business.InitializingCustomerSetting;
using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;
using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit;
using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
using Fanex.Tarus.Customer.Requests.InitializingCustomerSetting;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller.InitializingCustomerSetting
{
    [RoutePrefix("InitCustSetting")]
    public class InitCustSettingController : BaseController
    {
        private readonly string CreditType = "credit";
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Index.cshtml";
        private readonly string _creditViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/Index.cshtml";
        private readonly string _depositViewPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/Index.cshtml";
        private readonly string _viewConfigCreditPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_ViewConfig.cshtml";
        private readonly string _addEditCreditPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_AddEditConfig.cshtml";
        private readonly string _configCreditPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Credit/_ConfigDetail.cshtml";
        private readonly string _addEditDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_AddEditConfig.cshtml";
        private readonly string _configDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_ConfigDetail.cshtml";
        private readonly string _viewConfigDepositPath = "~/Views/ITConfiguration/InitializingCustomerSetting/Deposit/_ViewConfig.cshtml";
        private readonly IInitializingCreditSettingService creditService;
        private readonly IInitializingDepositSettingService depositService;

        public InitCustSettingController()
        {
            creditService = new InitializingCreditSettingService();
            depositService = new InitializingDepositSettingService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet]
        public ActionResult Credit()
        {
            return PartialView(_creditViewPath);
        }

        [HttpGet]
        public ActionResult Deposit()
        {
            return PartialView(_depositViewPath);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetSetting(string type)
            => Json(IsCredit(type) ? creditService.GetSetting() : depositService.GetSetting(), JsonRequestBehavior.AllowGet);

        [HttpPost, AuthenticateHost, Route("{type}/DeleteSetting")]
        public ActionResult DeleteSetting(string type, int configId)
        {
            var result = IsCredit(type)
                ? creditService.DeleteConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress)
                : depositService.DeleteConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult AbortSetting(string type, int configId)
        {
            var result = IsCredit(type)
                   ? creditService.AbortConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress)
                   : depositService.AbortConfigSetting(configId, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpGet, AuthenticateHost]
        public ActionResult GetProductSportBettype(string type)
        {
            var productMatrix = IsCredit(type) ? creditService.GetProductSportBettype() : depositService.GetProductSportBettype();
            var path = IsCredit(type) ? _addEditCreditPath : _addEditDepositPath;

            return PartialView(path, productMatrix);
        }

        [HttpGet, AuthenticateHost]
        public ActionResult GetConfigDetail(string type, int configId)
        {
            var viewModel = IsCredit(type)
                ? creditService.GetConfigDetail(new GetConfigDetailCriteria(configId))
                : depositService.GetConfigDetail(new GetDepositConfigDetailCriteria(configId));
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            var path = IsCredit(type) ? _addEditCreditPath : _addEditDepositPath;

            return PartialView(path, viewModel);
        }


        [HttpPost, AuthenticateHost]
        public ActionResult GenerateConfigSetting(string type, GenerateConfigSettingRequest request)
        {
            var viewModel = IsCredit(type) ? creditService.GenerateConfigSetting(request) : depositService.GenerateConfigSetting(request);
            var path = IsCredit(type) ? _configCreditPath : _configDepositPath;

            return PartialView(path, viewModel);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateAddConfigSetting(string type, ConfigModel request)
        {
            var result = IsCredit(type)
                ? creditService.ValidateAddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId)
                : depositService.ValidateAddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult AddConfigSetting(string type, ConfigModel request)
        {
            var result = IsCredit(type)
                   ? creditService.AddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId)
                   : depositService.AddConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult ValidateUpdateConfigSetting(string type, ConfigModel request)
        {
            var result = IsCredit(type)
                ? creditService.ValidateUpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId)
                : depositService.ValidateUpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpPost, AuthenticateHost]
        [ValidateInput(false)]
        public ActionResult UpdateConfigSetting(string type, ConfigModel request)
        {
            var result = IsCredit(type)
                  ? creditService.UpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId)
                  : depositService.UpdateConfigSetting(request, HttpContext.Request.UserHostAddress, AppSession.AccId);

            return Json(result);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ViewConfigDetail(string type, int configId)
        {
            var viewModel = IsCredit(type)
                ? creditService.GetConfigDetail(new GetConfigDetailCriteria(configId))
                : depositService.GetConfigDetail(new GetDepositConfigDetailCriteria(configId));
            if (viewModel.Error != null)
            {
                return Json(viewModel.Error, JsonRequestBehavior.AllowGet);
            }

            var path = IsCredit(type) ? _viewConfigCreditPath : _viewConfigDepositPath;

            return PartialView(path, viewModel);
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

        private bool IsCredit(string type) => type.ToLowerInvariant() == CreditType;
    }
}
