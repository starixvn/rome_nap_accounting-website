using System.Web.Mvc;
using Fanex.Tarus.Admin.Business.SiteManagement;
using Fanex.Tarus.Admin.Criteria.SiteManagement;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Admin.Controller
{
    public class SiteManagementController : BaseController
    {
        private readonly ISiteManagementService siteManagementService;

        public SiteManagementController()
            : this(null)
        {
        }

        public SiteManagementController(ISiteManagementService siteManagementService)
        {
            this.siteManagementService = siteManagementService ?? new SiteManagementService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Admin/SiteManagement/Index.cshtml");
        }

        [AuthenticateRead]
        public JsonResult ValidateSite(ValidateSiteCriteria criteria)
        {
            var result = siteManagementService.ValidateSite(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult AddSite(AddSiteCriteria criteria)
        {
            var result = siteManagementService.AddSite(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
