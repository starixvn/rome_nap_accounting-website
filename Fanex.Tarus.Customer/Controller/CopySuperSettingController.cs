using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Web.Controllers;
using Fanex.Tarus.Customer.Business.CopySuperSetting;
using Fanex.Tarus.Customer.Requests.CopySuperSetting;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    public class CopySuperSettingController : BaseController
    {
        private readonly string indexPath = "~/Views/Customer/CopySuperSetting/Index.cshtml";
        private readonly ICopySuperSettingService service;

        public CopySuperSettingController()
        {
            service = new CopySuperSettingService();
        }

        public ActionResult Index()
        {
            return View(indexPath);
        }


        [HttpPost, AuthenticateHost]
        public ActionResult CheckSuper(CheckSuperListRequest request)
        {
            return Json(service.CheckSuperList(request));
        }
    }
}
