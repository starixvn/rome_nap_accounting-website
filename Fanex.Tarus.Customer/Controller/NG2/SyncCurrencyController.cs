using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.CustomerSetting.Module.Core.Models;
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
            return View("~/Views/ExchangeRate/SyncCurrency.cshtml");
        }

        [HttpPost]
        public ActionResult Update(string entities)
        {
            var model = JsonConvert.DeserializeObject<SyncCurrencyModel>(entities);
            ViewBag.ErrorMessage = string.Empty;

            return View("~/Views/ExchangeRate/SyncCurrency.cshtml", model);
        }

        [HttpPost]
        public ActionResult Add(string currencyCode)
        {
            var service = new SyncCurrencyService();
            var model = service.GetCurrencyByName(currencyCode);

            var result = new Result(model.CurrencyId == 0 ? 1 : 0, "This currency is not available in system!");
            if (result.IsSuccess)
            {
                result = service.AddSyncCurrency(model);
            }

            return Json(result);
        }
    }
}