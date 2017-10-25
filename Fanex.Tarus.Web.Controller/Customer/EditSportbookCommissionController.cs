using Fanex.Tarus.Business.Implement.Customer;
using Fanex.Tarus.Business.Interface.Customer;
using Fanex.Tarus.Entities.Criteria.Customer;
using Fanex.Tarus.Web.Controllers.Filters;
using Sunplus.Accounting.Core.Common;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers.Customer
{
    public class EditSportbookCommissionController : BaseController
    {
        private ISportbookCommissionService sportbookCommissionService;

        public EditSportbookCommissionController()
        {
            sportbookCommissionService = new SportbookCommissionService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            var model = sportbookCommissionService.GetSporttypeBettype(new SportbookCommissionSporttypeBettypeSelectCriteria());
            return View("~/Views/Customer/EditSportbookCommission/Index.cshtml", model);
        }

        [AuthenticateHost]
        public JsonResult CheckMembers(SportbookCommissionCheckUsersCriteria criteria)
        {
            return Json(sportbookCommissionService.CheckMembers(criteria));
        }

        [AuthenticateHost]
        public JsonResult Update(SportbookCommissionUpdateCriteria criteria)
        {
            criteria.UserID = AppSession.AccId;
            return Json(sportbookCommissionService.UpdateCommission(criteria));
        }
    }
}
