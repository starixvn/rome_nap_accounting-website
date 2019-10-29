namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Tarus.Customer.Business.ManageRobots2;

    public class ManageRobots2Controller : BaseController
    {
        private readonly IManageRobots2Service service;

        public ManageRobots2Controller() : this(null)
        {
        }

        public ManageRobots2Controller(IManageRobots2Service service)
        {
            this.service = service ?? new ManageRobots2Service();
        }

        [HttpGet]
        public JsonResult CheckIsRobot(int custId)
        {
            return Json(service.CheckIsRobot(custId), JsonRequestBehavior.AllowGet);
        }
    }
}
