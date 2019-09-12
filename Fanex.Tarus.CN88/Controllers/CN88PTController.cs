namespace Fanex.Tarus.CN88.Controllers
{
    using System.Web.Mvc;
    using Fanex.Tarus.CN88.Criteria.PositionTaking;
    using Fanex.Tarus.CN88.Models;
    using Fanex.Tarus.CN88.Services;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class CN88PTController : BaseController
    {
        private readonly IPTService service;

        public CN88PTController() : this(null)
        {
        }

        public CN88PTController(IPTService service)
        {
            this.service = service ?? new PTService();
        }

        [AuthenticateRead]
        public ActionResult Index(string username)
            => View("~/Areas/CN88/Views/PositionTaking/Index.cshtml", new CustInfoModel { Username = username ?? string.Empty });

        [HttpPost]
        [AuthenticateRead]
        public JsonResult GetPT(GetPTCriteria criteria)
        {
            var result = service.GetPT(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateRead]
        public ActionResult GetComm(GetCommCriteria criteria)
            => View("~/Areas/CN88/Views/PositionTaking/CustomerComm.cshtml", service.GetComm(criteria));
    }
}
