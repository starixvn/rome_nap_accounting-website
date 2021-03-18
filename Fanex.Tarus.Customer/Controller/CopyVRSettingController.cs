namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Business.CopyVRSetting;
    using Fanex.Tarus.Customer.Criteria.CopyVRSetting;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Sunplus.Accounting.Core.Helper;
    using System.Web.Mvc;

    public class CopyVRSettingController : BaseController
    {
        private readonly ICopyVRSettingService _copyVRSettingService;

        public CopyVRSettingController() : this(null) { }

        public CopyVRSettingController(ICopyVRSettingService copyVRSettingService)
        {
            this._copyVRSettingService = copyVRSettingService ?? new CopyVRSettingService();
        }

        [HttpGet, AuthenticateRead]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthenticateHost]
        public ActionResult Copy(CopyVRSettingCriteria criteria)
        {
            criteria.UserId = AppSession.AccId;
            criteria.IP = Utils.GetClientIP();
            return Json(this._copyVRSettingService.Copy(criteria));
        }
    }
}
