using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.ViewModels.Customer;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Customer.Controller
{
    [SpecialSiteFilter]
    public class CustomerController : BaseController
    {
        protected static ICustomerSettingService _customerSettingService;

        public CustomerController()
            : this(null)
        {
        }

        public CustomerController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
        }

        public ActionResult GetSitesAndCurrencies()
            => Content(JsonConvert.SerializeObject(
                new
                {
                    sites = _customerSettingService.GetSites(AppSession.SYSTEM),
                    currencies = _customerSettingService.GetCurrencyList(AppSession.SYSTEM)
                },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        public ActionResult Create()
            => View("~/Views/Customer/Create.cshtml", new CustomerViewModel($"{Response.ApplyAppPathModifier(Request.ApplicationPath)}Customer"));
    }
}