using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Business.NG2;
using Fanex.Tarus.Security.Permission;
using Fanex.Tarus.Web.Controllers;

namespace Fanex.Tarus.Customer.Controller.NG2
{
    public class SyncCurrencyController : BaseController
    {
        [HttpGet]
        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Read)]
        public ActionResult Index()
        {            
            return View("~/Views/ExchangeRate/SyncCurrency.cshtml");
        }

        [HttpPost]
        [CheckPermission(Permission = CheckPermissionAttribute.Auth.Host)]
        public ActionResult Add(string currencyCode)
        {
            var service = new SyncCurrencyService();
            var model = service.GetCurrencyByName(currencyCode);

            var result = new Result(model.CurrencyId == 0 ? 1 : 0, model.CurrencyId == 0 ? "This currency is not available in system!": string.Empty);
            if (result.IsSuccess)
            {
                result = service.AddSyncCurrency(model);
            }

            return Json(result);
        }
    }
}