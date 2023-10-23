using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private static ICustomerSettingService _customerSettingService;
        private static HttpClient httpClient;
        private static string cdnUri;
        private static string assetsVersion;

        public CustomerController()
            : this(null)
        {
            cdnUri = ConfigurationManager.AppSettings["Customer:CdnUri"];
            assetsVersion = ConfigurationManager.AppSettings["Customer:AssetsVersion"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(ConfigurationManager.AppSettings["Customer:ApiUri"])
            };
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

        public ActionResult CreateSuper()
        {
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            var viewModel = new CustomerViewModel($"{applicationPath}Customer", cdnUri, assetsVersion);
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateSuper.cshtml", viewModel);
        }

        public ActionResult CreateCustomer(int custId, int roleId,  string siteName)
        {
            //debug cho nay
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            var viewModel = new CustomerViewModel($"{applicationPath}Customer", cdnUri, assetsVersion);
            viewModel.CustId = custId;
            viewModel.RoleId = roleId;
            viewModel.SiteName = siteName;
            
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateCustomer.cshtml", viewModel);
        }
        
        public async Task<ActionResult> GetProductSetting(string site, int currency)
            => Content(await PostAsStringAsync("customer-settings/create-super/query",
                new { site, currency, language = "en-US" }));

        public async Task<ActionResult> GetProductSettingDownLine(int upLineCusId, int upLineRoleId)
            => Content(await PostAsStringAsync("customer-settings/create-downline/query",
                new
                {
                    uplineCustId = upLineCusId,
                    uplineRoleId = upLineRoleId,
                    language = "en-US"
                }));

        public static async Task<string> PostAsStringAsync(string requestUri, object payload = null)
        {
            using (var stringContent =
                   new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = stringContent })
                {
                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                               .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}