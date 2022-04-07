namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.LicenseeMember;
    using Fanex.Tarus.Customer.Requests.LicenseeMember;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class LicenseeMemberController : BaseController
    {
        private readonly ILicenseeMemberService service;
        public LicenseeMemberController() : this(null)
        {
        }

        public LicenseeMemberController(ILicenseeMemberService memberService)
        {
            service = memberService ?? new LicenseeMemberService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/LicenseeMember/Index.cshtml");
        }

        [AuthenticateHost]
        public ActionResult CheckExistAgent(string agentName)
        {
            return Json(service.CheckExistAgent(agentName));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult InsertMemberToAgent(InsertMemberRequest request)
        {
            return Json(service.InsertMember(request, AppSession.AccId, Utils.GetClientIP()));
        }
    }
}
