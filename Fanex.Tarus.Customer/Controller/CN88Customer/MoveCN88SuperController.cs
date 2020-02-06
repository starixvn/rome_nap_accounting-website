namespace Fanex.Tarus.Customer.Controller.CN88Customer
{
    using Fanex.Tarus.Customer.Business.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Requests.CN88.MoveSuper;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Helper;
    using System.Web.Mvc;

    public class MoveCN88SuperController : BaseController
    {
        private IMoveSuperService _moveSuperService;
        private readonly string _indexViewPath = "~/Views/Customer/CustomerSetting/CN88/MoveSuper/Index.cshtml";

        public MoveCN88SuperController() : this(null) { }

        public MoveCN88SuperController(IMoveSuperService moveSuperService)
        {
            _moveSuperService = moveSuperService ?? new MoveSuperService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [AuthenticateRead]
        public JsonResult GetHeadlineUserNameSuggestion(string query)
        {
            return Json(_moveSuperService.GetHeadlineUserNameSuggestion(query), JsonRequestBehavior.AllowGet);
        }

        [AuthenticateRead]
        public JsonResult SearchSuper(string userName)
        {
            return Json(_moveSuperService.SearchSuper(userName));
        }

        [AuthenticateHost]
        public JsonResult MoveSuper(MoveSuperRequest moveSuperRequest)
        {
            moveSuperRequest.UserId = AppSession.AccId;
            moveSuperRequest.Ip = Utils.GetClientIP();
            return Json(_moveSuperService.MoveSuper(moveSuperRequest));
        }
    }
}
