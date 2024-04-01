using Fanex.Tarus.Customer.Business.CopySuperSetting;
using Fanex.Tarus.Customer.Requests.CopySuperSetting;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [HttpPost, AuthenticateHost]
        public async Task<ActionResult> CopyWinLossLimit(UpdateWinLossLimitRequest request)
        {
            return Json(await service.CopyWinLossLimit(request, AppSession.AccId, Utils.GetClientIP()));
        }
    }
}
