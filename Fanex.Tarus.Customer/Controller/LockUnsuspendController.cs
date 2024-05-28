namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Criteria.LockUnsuspend;
    using Fanex.Tarus.Customer.DataAccess.LockUnsuspend;
    using Fanex.Tarus.Customer.ViewModels.LockUnsuspend;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Helper;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LockUnsuspendController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/Customer/LockUnsuspend/Index.cshtml";
        private readonly ILockUnsuspendService _lockUnsuspendService;

        public LockUnsuspendController() : this(null)
        { }

        public LockUnsuspendController(ILockUnsuspendService lockUnsuspendService)
        {
            _lockUnsuspendService = lockUnsuspendService ?? new LockUnsuspendService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {

            return View(_indexViewPath);
        }

        [AuthenticateRead]
        public JsonResult Search()
        {
            var data = _lockUnsuspendService.Search(new SearchCustomerCriteria()
            {
                UserID = AppSession.AccId
            });
            var viewModel = new List<LockUnsuspendResultViewModel>();
            foreach (var entity in data)
            {
                viewModel.Add(new LockUnsuspendResultViewModel()
                {
                    UserName = entity.UserName,
                    RoleId = entity.RoleId,
                    Status = entity.Status
                });
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult Update(string custList, bool isLock)
        {
            return Json(_lockUnsuspendService.Update(new UpdateCustomerCriteria()
            {
                CustList = custList.Trim(),
                Lock = isLock,
                IP = Utils.GetClientIP(),
                UserId = AppSession.AccId
            }));
        }
    }
}
