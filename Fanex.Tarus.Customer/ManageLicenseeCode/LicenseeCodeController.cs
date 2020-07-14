namespace Fanex.Tarus.Customer.ManageLicenseeCode
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Request;

    public class LicenseeCodeController : BaseController
    {
        private readonly string indexPath = "~/Views/LicenseeCode/Index.cshtml";
        private readonly IService service;

        public LicenseeCodeController()
        {
            service = new Service();
        }

        public ActionResult Index()
        {
            var licensees = service.GetActiveLicensee();
            ViewBag.Licensees = licensees;

            var status = service.GetStatus();
            ViewBag.Status = status;

            return View(indexPath);
        }
        
        [HttpPost]
        public JsonResult SearchLicenseeCode([System.Web.Http.FromBody]SearchRequest request)
        {
            var result = service.GetLicenseeCodes(request);

            return Json(result);
        }

        [HttpPost]
        public JsonResult NewLicenseeCode([System.Web.Http.FromBody]UpdNewLicenseeCodeRequest request)
        {
            var result = service.AddLicenseeCodes(request, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateLicenseeCode([System.Web.Http.FromBody]UpdNewLicenseeCodeRequest request)
        {
            var result = service.UpdateLicenseeCodes(request, AppSession.AccId, HttpContext.Request.UserHostAddress);

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateLicenseeCode([System.Web.Http.FromBody]UpdNewLicenseeCodeRequest request)
        {
            var result = service.ValidateLicenseeCodes(request);

            return Json(result);
        }
    }
    
}
