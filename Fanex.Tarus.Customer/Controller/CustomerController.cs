using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.Criteria.CreditCustomer;
using Fanex.Tarus.Customer.Requests;
using Fanex.Tarus.Customer.ViewModels.Customer;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;
using Sunplus.Agent.Utilities;

namespace Fanex.Tarus.Customer.Controller
{
    [SpecialSiteFilter]
    public class CustomerController : BaseController
    {
        private const string AssetsVersionCacheKey = "Customer:AssetsVersion";
        private readonly HttpClient httpClient;
        private readonly string cdnUri;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly Encryptor encryptor;

        public CustomerController()
            : this(null)
        {
        }

        public CustomerController(ICustomerSettingService customerSettingService)
        {
            cdnUri = ConfigurationManager.AppSettings["Customer:CdnUri"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(ConfigurationManager.AppSettings["Customer:ApiUri"])
            };
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            encryptor = new EncryptorImpl();
        }

        private string GetAssetsVersion()
        {
            ObjectCache cache = MemoryCache.Default;
            var version = cache.Get(AssetsVersionCacheKey) as string;
            if (string.IsNullOrWhiteSpace(version))
            {
                version = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                cache.Set(AssetsVersionCacheKey, version, DateTime.Now.AddHours(4));
            }
            return version;
        }

        public ActionResult GetHeadlineBelongSite()
            => Content(JsonConvert.SerializeObject(
                _customerSettingService.GetHeadlineBelongSite(new GetHeadlineBelongSiteCriteria()),
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        public ActionResult GetNotAllowedSuperPrefixes()
            => Content(JsonConvert.SerializeObject(_customerSettingService.GetSuperPrefixes(new SuperPrefixCriteria
            {
                GroupName = null,
                Prefix = null,
                AllowNewSuper = false
            }), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        [AuthenticateRead]
        public ActionResult GetSitesAndCurrencies()
            => Content(JsonConvert.SerializeObject(
                new
                {
                    sites = _customerSettingService.GetSites(AppSession.SYSTEM),
                    currencies = _customerSettingService.GetCurrencyList(AppSession.SYSTEM)
                },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        [AuthenticateRead]
        public ActionResult CreateSuper()
        {
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            var viewModel = new CreateCustomerViewModel(new AppConfiguration(applicationPath), cdnUri, GetAssetsVersion());
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateSuper.cshtml", viewModel);
        }

        public ActionResult CreateCustomer(int uplineId, byte uplineRoleId, string username)
        {
            //debug cho nay
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);

            var viewModel = new CreateCustomerViewModel(new AppConfiguration(applicationPath), cdnUri, GetAssetsVersion(), uplineId, uplineRoleId, username);

            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateCustomer.cshtml", viewModel);
        }

        public async Task<ActionResult> GetSetting(int custId, int roleId, int productGroupId = 1, string language = "en-US")
            => Content(await httpClient.GetStringAsync($"customer?custId={custId}&roleId={roleId}&productGroupId={productGroupId}&language={language}"));

        public async Task<ActionResult> CheckExistedCustomer(string username)
            => Content(await httpClient.GetStringAsync($"/customer/{username}/exist"));
        
        [AuthenticateRead]
        public async Task<ActionResult> GetGlobalSetting(string site, int currency, int productGroupId = 1, string language = "en-US")
            => Content(await httpClient.GetStringAsync($"customer/globalsetting?site={site}&currency={currency}&productGroupId={productGroupId}&language={language}"));

        public async Task<ActionResult> GetProductSettingDownLine(int upLineCusId, int upLineRoleId)
            => Content(await PostAsStringAsync("customer-settings/create-downline/query",
                new
                {
                    uplineCustId = upLineCusId,
                    uplineRoleId = upLineRoleId,
                    language = "en-US"
                }));

        public async Task<string> PostAsStringAsync(string requestUri, object payload = null)
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

        [HttpPost]
        [AuthenticateHost]
        public async Task<ActionResult> CreateSuper(CreateCustomerRequest request)
        {
            var generalInfo = request.GeneralInfo;
            generalInfo.Password = encryptor.CfsEnCode(generalInfo.Password);

            var response = await PostAsStringAsync("customer", new
            {
                context = new { creatorId = AppSession.AccId, systemId = 1, ip = Utils.GetClientIP(), Mode = 1 },
                settings = new { generalInfo, productGroups = JArray.Parse(request.ProductGroups) }
            });
            return Content(response);
        }
    }
}