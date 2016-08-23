using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Business.NG2;
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
            var service = new SyncCurrencyService();
            var model = service.SyncCurrencies();

            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", model);
        }

        [HttpPost]
        public ActionResult Update(string entities)
        {
            var model = JsonConvert.DeserializeObject<SyncCurrencyModel>(entities);
            ViewBag.ErrorMessage = string.Empty;

            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", model);
        }

        public ActionResult Add(string entity)
        {
            var ent = JsonConvert.DeserializeObject<SyncCurrencyEntity>(entity);
            ViewBag.ErrorMessage = string.Empty;

            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", ent);
        }
    }
}