using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    public class LicenseeDcsSettingController : BaseController
    {
        private readonly ILicenseeDcsSettingService _settingService;

        public LicenseeDcsSettingController()
            : this(null)
        {
        }

        public LicenseeDcsSettingController(ILicenseeDcsSettingService settingService)
        {
            _settingService = settingService ?? new LicenseeDcsSettingService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            var sites = _settingService.GetSites();
            var supers = sites.Any() ?
                _settingService.GetSupers(sites.First()) :
                Enumerable.Empty<BaseModel>();
            var products = _settingService.GetProducts();

            var model = new FilterViewModel(sites, supers, products);

            return View("~/Views/Customer/LicenseeDCSSetting/Index.cshtml", model);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult Supers(string site)
        {
            var supers = _settingService.GetSupers(site);

            return Json(supers);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult PT(PTViewModel model)
        {
            model.SuperPT = _settingService.GetPositionTaking(model.ProductId, model.SuperId);

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_PT.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdatePT(PTViewModel model)
        {
            var result = _settingService.UpdatePositionTaking(
                model.ProductId,
                model.SuperId,
                model.SuperPT,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult BetSetting(BetSettingViewModel model)
        {
            model.BetSettings = _settingService.GetBetSettings(model.ProductId, model.SuperId);

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_BetSetting.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateBetSetting(BetSettingViewModel model)
        {
            var result = _settingService.UpdateBetSetting(
                model.SuperId,
                model.ApplyExistedMem,
                model.BetSettings.FirstOrDefault(),
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult Status(StatusViewModel model)
        {
            var status = _settingService.GetStatus(model.ProductId, model.SuperId);
            model.SportType = status.SportType;
            model.BetType = status.BetType;
            model.BitmapOrder = status.BitmapOrder;
            model.IsDisabled = status.IsDisabled;
            model.IsDisabledDownline = status.IsDisabledDownline;
            model.HasDownline = status.HasDownline;

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_Status.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateStatus(StatusViewModel model)
        {
            var result = _settingService.UpdateStatus(
                model.ProductId,
                model.SuperId,
                model.IsDisabled,
                model.IsDisabledDownline,
                model.SportType,
                model.BetType,
                model.BitmapOrder,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
