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
        [SpecialSiteFilter]
        public ActionResult Index()
        {
            var service = new SyncCurrencyService();
            service.AddSyncCurrency(new SyncCurrencyEntity());

            var model = new SyncCurrencyModel();
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.1M, CurrencyCode = "USD", CurrencyId = 1, CurrencyName = "US$" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.2M, CurrencyCode = "AUD", CurrencyId = 2, CurrencyName = "AUD" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.3M, CurrencyCode = "CAD", CurrencyId = 3, CurrencyName = "CAD" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.4M, CurrencyCode = "EUR", CurrencyId = 4, CurrencyName = "EURO" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.5M, CurrencyCode = "GBP", CurrencyId = 5, CurrencyName = "GBP" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.6M, CurrencyCode = "HKD", CurrencyId = 6, CurrencyName = "HKD" });
            model.Currencies.Add(new SyncCurrencyEntity { BaseRate = 1.7M, CurrencyCode = "UUS", CurrencyId = 7, CurrencyName = "UUS" });

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