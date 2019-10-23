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

        [HttpPost]
        public JsonResult CheckIsRobot(string username)
        {
            return Json(service.CheckIsRobot(username), JsonRequestBehavior.AllowGet);
        }
    }
}
