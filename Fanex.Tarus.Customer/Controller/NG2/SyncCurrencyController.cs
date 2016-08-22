using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Entities.NG2;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.Controller.NG2
{
    [SpecialSiteFilter]
    public class SyncCurrencyController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new SyncCurrencyModel();
            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", model);
        }

        [HttpPost]
        public ActionResult Update(string entity)
        {
            var ent = JsonConvert.DeserializeObject<SyncCurrencyEntity>(entity);
            var model = new SyncCurrencyModel();
            ViewBag.ErrorMessage = string.Empty;

            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", model);
        }
    }
}