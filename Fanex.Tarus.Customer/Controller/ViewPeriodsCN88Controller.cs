namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Business.ViewPeriodsCN88;
    using Fanex.Tarus.Customer.Criteria.ViewPeriodsCN88;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;

    public class ViewPeriodsCN88Controller : BaseController
    {
        private readonly IViewPeriodsCN88Service _viewPeriodsCN88Service;

        public ViewPeriodsCN88Controller() : this(null)
        { }

        public ViewPeriodsCN88Controller(IViewPeriodsCN88Service viewPeriodsCN88Service)
        {
            _viewPeriodsCN88Service = viewPeriodsCN88Service ?? new ViewPeriodsCN88Service();
        }

        [HttpGet]
        [AuthenticateRead]
        public ActionResult Index() => View("~/Views/Customer/ViewPeriodCN88/Index.cshtml");

        [HttpGet]
        [AuthenticateRead]
        public JsonResult Search(string headlineUserName) => Json(_viewPeriodsCN88Service.Search(new SearchHeadlineCriteria() { UserName = headlineUserName }), JsonRequestBehavior.AllowGet);

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdatePeriod(int headlineId, bool isViewAllPeriods)
            => Json(_viewPeriodsCN88Service.UpdatePeriod(
                new UpdateHeadlinePeriodCriteria
                {
                    HeadlineId = headlineId,
                    IsViewAllPeriods = isViewAllPeriods,
                    UserId = AppSession.AccId,
                    IP = Utils.GetClientIP()
                }));
    }
}
