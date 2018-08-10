using Fanex.Tarus.Customer.Business.Casino;
using Fanex.Tarus.Customer.Entities.NG2;
using Fanex.Tarus.Security.Permission;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Fanex.Tarus.Customer.Controller.Casino
{
    public class SyncCasinoController : BaseController
    {
        private readonly ISyncCasinoService _service = new SyncCasinoService();

        [HttpGet, CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Index() => View("~/Views/Admin/Casino/Index.cshtml");

        [HttpPost, CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Check(string usernames)
        {
            var result = _service.GetCustomersByUsernames(usernames);

            return Json(result);
        }

        [HttpPost, CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult UnsyncedList()
        {
            var result = _service.GetUnsyncedCustomers();

            return Json(result);
        }

        [HttpPost, CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Sync(string customers)
        {
            var customerModel = JsonConvert.DeserializeObject<SyncCustomerModel>(customers);
            var result = _service.SyncCustomers(customerModel);

            return Json(result);
        }
    }
}