namespace Fanex.Tarus.Customer.Controller.Setting
{
    using DataAccess.SettingCustomer.Suspend;
    using Infrastructure.Common;
    using Infrastructure.Helper;
    using System.Web.Mvc;
    using Web.Controllers;

    public class UnsuspendCustomerController : BaseController
    {
        private SuspendStatusService _singleUpdateSuspendService;
        private string _singleSuspendViewPath = "~/Views/Customer/CustomerSetting/Suspend/Single/SingleUnsuspend.cshtml";
        private string _suspendMatrixViewPath = "~/Views/Customer/CustomerSetting/Suspend/SuspendMatrix.cshtml";

        public UnsuspendCustomerController() : this(null)
        { }

        public UnsuspendCustomerController(SuspendStatusService singleUpdateSuspendService)
        {
            _singleUpdateSuspendService = singleUpdateSuspendService ?? new SuspendStatusService();
        }

        [HttpGet]
        public ActionResult SingleSuspendView(int custId, string userName)
        {
            return View(_singleSuspendViewPath, _singleUpdateSuspendService.GetViewModel(custId, userName));
        }

        [HttpPost]
        public JsonResult GetDownlinesInfo(int custId, bool isAffected, int downlineRoleId, int pageSize = 50, int pageIndex = 0)
        {
            return Json(_singleUpdateSuspendService.GetDownlinesDetailInfo(custId, isAffected, downlineRoleId, pageSize, pageIndex));
        }

        [HttpPost]
        public JsonResult Update(int custId)
        {
            return Json(_singleUpdateSuspendService.Update(custId, AppSession.AccId, false, Utils.GetClientIP()));
        }

        [HttpGet]
        public ActionResult SuspendMatrixView(string userName)
        {
            return View(_suspendMatrixViewPath, model: userName);
        }

        [HttpPost]
        public JsonResult GetSuspendMatrix(string userName)
        {
            return Json(_singleUpdateSuspendService.GetSuspendMatrixInfo(userName));
        }
    }
}
