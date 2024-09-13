using System.Web.Mvc;
using System.Web.SessionState;
using Fanex.Tarus.Customer.Business.LicenseeSubsidiary;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LicenseeSubsidiaryController : BaseController
    {
        private readonly ILicenseeSubsidiaryService licenseeService;

        public LicenseeSubsidiaryController()
            : this(null)
        {
        }

        internal LicenseeSubsidiaryController(ILicenseeSubsidiaryService licenseeService)
        {
            this.licenseeService = licenseeService ?? new LicenseeSubsidiaryService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/LicenseeSubsidiary/Index.cshtml");
        }

        [HttpPost, AuthenticateRead]
        public ActionResult Generate(string username, int? number)
        {
            var copiedAccounts = licenseeService.Generate(username, number ?? 0);

            return Json(copiedAccounts, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult Create(string username, string existedUsername)
        {
            var result = licenseeService.Create(username, existedUsername, AppSession.AccId, Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}