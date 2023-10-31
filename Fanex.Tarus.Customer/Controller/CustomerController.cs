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
using Fanex.Tarus.Web.Controllers.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;
using Sunplus.Agent.Utilities;
using TelegramBot.Client.Extensions;

namespace Fanex.Tarus.Customer.Controller
{
    public class GeneralInfo
    {
        public string Username { get; set; }

        public int UpLineRoleId { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public decimal MaxCredit { get; set; }

        public decimal MemberMaxCredit { get; set; }

        public string Fax { get; set; }

        public bool DisableAutoMPT { get; set; }

        public string TransferOption { get; set; }

        public bool IsAutomateTransfer { get; set; }

        public string Site { get; set; }

        public string SiteLabel { get; set; }

        public int Currency { get; set; }

        public string Group { get; set; }
    }

    public class CreateCustomerRequest
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string ProductGroups { get; set; }
    }

    [SpecialSiteFilter]
    public class CustomerController : BaseController
    {
        private readonly HttpClient httpClient;
        private readonly string cdnUri;
        private readonly string assetsVersion;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly Encryptor encryptor;

        public CustomerController()
            : this(null)
        {
        }

        public CustomerController(ICustomerSettingService customerSettingService)
        {
            cdnUri = ConfigurationManager.AppSettings["Customer:CdnUri"];
            assetsVersion = ConfigurationManager.AppSettings["Customer:AssetsVersion"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(ConfigurationManager.AppSettings["Customer:ApiUri"])
            };
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            encryptor = new EncryptorImpl();
        }

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

            var viewModel = new CustomerViewModel(applicationPath, cdnUri, assetsVersion);
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateSuper.cshtml", viewModel);
        }

        public ActionResult CreateCustomer(int custId, int roleId, string siteName)
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

        [AuthenticateRead]
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

            var response = await PostAsStringAsync("customer-settings/super/create", new
            {
                context = new { creatorId = AppSession.AccId, systemId = 1 },
                settings = new { generalInfo, productGroups = JArray.Parse(request.ProductGroups) }
            });
            return Content(response);
        }
    }
}