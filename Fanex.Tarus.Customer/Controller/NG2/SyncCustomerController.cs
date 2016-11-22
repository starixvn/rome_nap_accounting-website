using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.NG2;
using Fanex.Tarus.Customer.Entities.NG2;
using Fanex.Tarus.Security.Permission;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Controller.NG2
{
    public class SyncCustomerController : BaseController
    {
        private readonly ISyncCustomerService _service = new SyncCustomerService();

        [HttpGet]
        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Index()
        {            
            return View("~/Views/Admin/NG2/Index.cshtml");
        }

        [HttpPost]
        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Check(string usernames)
        {
            var result = _service.GetCustomersByUsernames(usernames);

            return Json(result);
        }

        [HttpPost]
        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Sync(string syncCustomerModel)
        {
            var model = JsonConvert.DeserializeObject<SyncCustomerModel>(syncCustomerModel);
            var result = _service.SyncCustomers(model);

            return Json(result);
        }
    }
}