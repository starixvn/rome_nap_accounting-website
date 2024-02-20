using System;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Models;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Services;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.Criteria.CreditCustomer;
using Fanex.Tarus.Customer.Requests.Customer;
using Fanex.Tarus.Customer.ViewModels.Customer;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Agent.Utilities;
using Sunplus.Accounting.Business.Security;
using Fanex.Tarus.Customer.Filters;
using Fanex.Customer.Client;

namespace Fanex.Tarus.Customer.Controller
{
    [SpecialSiteFilter]
    public class CustomerController : BaseController
    {
        private const string HashKeySessionKey = "Fanex.Tarus.Customer.Controller.CustomerController.PasswordHash";
        private const string AssetsVersionCacheKey = "Fanex.Customer:AssetsVersion";
        private readonly string cdnUri;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly Encryptor encryptor;
        private readonly ICustomerService customerService;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public CustomerController()
            : this(null, null, null)
        {
        }

        internal CustomerController(ICustomerSettingService customerSettingService, Encryptor encryptor, ICustomerService customerService)
        {
            cdnUri = ConfigurationManager.AppSettings["Fanex.Customer:CdnUri"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            this.encryptor = encryptor ?? new EncryptorImpl();
            this.customerService = customerService ?? new CustomerService();
        }

        public ActionResult GetHeadlineBelongSite()
            => Content(JsonConvert.SerializeObject(
                _customerSettingService.GetHeadlineBelongSite(new GetHeadlineBelongSiteCriteria()),
                jsonSerializerSettings));

        public ActionResult GetNotAllowedSuperPrefixes()
            => Content(JsonConvert.SerializeObject(_customerSettingService.GetSuperPrefixes(new SuperPrefixCriteria
            {
                GroupName = null,
                Prefix = null,
                AllowNewSuper = false
            }), jsonSerializerSettings));

        [NAPCustomerAuthFilter]
        public ActionResult NewCustomer()
            => View("~/Views/Customer/NewCustomer.cshtml");

        [NAPCustomerAuthFilter]
        public ActionResult Copy()
            => View("~/Areas/CustomerSetting/Views/SearchCopy/Super/Index.cshtml");

        [HttpPost]
        [NAPCustomerAuthFilter(IsHost = true)]
        public async Task<ActionResult> Copy(CopyCustomerRequest request, string code)
        {
            var param = new CopyCustomerParam(
                AppSession.AccId,
                0,
                request.UplineId,
                request.CustLevel,
                string.Empty,
                request.UserName,
                IBCUtils.Encryption.CfsEnCode(Security.Services.Encryptor.Decrypt(request.Password, code)),
                request.Credit,
                0,
                request.SourceCustId,
                request.Site,
                false,
                Utils.GetClientIP(),
                isInternal: request.IsInternal)
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Fax = request.Fax,
                MobilePhone = request.MobilePhone
            };

            var result = await customerService.CopyCustomer(param);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCustomerInfoByUserId(int custid)
        {
            var custInfo = await customerService.GetCustomerInfo(custid);

            return Json(custInfo, JsonRequestBehavior.AllowGet);
        }

        [NAPCustomerAuthFilter]
        public ActionResult GetSitesAndCurrencies()
            => Content(JsonConvert.SerializeObject(
                new
                {
                    sites = _customerSettingService.GetSites(AppSession.SYSTEM),
                    currencies = _customerSettingService.GetCurrencyList(AppSession.SYSTEM)
                },
                jsonSerializerSettings));

        [NAPCustomerAuthFilter]
        public ActionResult CreateSuper()
        {
            var viewModel = new CreateCustomerViewModel(new CreateCustomerRequest(), new AppConfiguration(GetApplicationPath()), cdnUri, GetAssetsVersion(), GetHashKey());
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/CreateSuper-Debug.cshtml"
                    : "~/Views/Customer/CreateSuper.cshtml", viewModel);
        }

        [NAPCustomerAuthFilter]
        public ActionResult CreateCustomer(CreateCustomerRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Site)
                && request.UplineRoleId == RoleIdConstants.SUPER
                && (request.Site.Equals("76666B", StringComparison.OrdinalIgnoreCase) ||
                    request.Site.Equals("IBCIndo", StringComparison.OrdinalIgnoreCase)))
            {
                return View("~/Views/Customer/NotAllowAddDirectMaster.cshtml");
            }

            if (string.IsNullOrWhiteSpace(request.UplineUsername))
            {
                request.UplineUsername = AppSession.CustUName;
            }

            request.NoPermissionEditCredit = !Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

            var viewModel = new CreateCustomerViewModel(
                request,
                new AppConfiguration(GetApplicationPath()),
                cdnUri,
                GetAssetsVersion(),
                GetHashKey());

            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/CreateCustomer-Debug.cshtml"
                    : "~/Views/Customer/CreateCustomer.cshtml", viewModel);
        }

        [NAPCustomerAuthFilter]
        public ActionResult EditCustomer(EditCustomerRequest request)
        {
            request.NoPermissionEditCredit = !Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

            var editCustomerViewModel = new EditCustomerViewModel(
                request,
                new AppConfiguration(GetApplicationPath()),
                cdnUri,
                GetAssetsVersion());

            return View(HttpContext.IsDebuggingEnabled
                ? "~/Views/Customer/EditCustomer-Debug.cshtml"
                : "~/Views/Customer/EditCustomer.cshtml", editCustomerViewModel);
        }

        public ActionResult SportBookInformation(SportBookInformationRequest request)
        {
            var viewModel = new SportBookInformationViewModel(
                request,
                new AppConfiguration(GetApplicationPath()),
                cdnUri,
                GetAssetsVersion());
            return View(HttpContext.IsDebuggingEnabled
                ? "~/Views/Customer/SportBookInformation/Index-Debug.cshtml"
                : "~/Views/Customer/SportBookInformation/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> GetCustomerSetting(int custId, byte roleId, byte uplineRoleId = 0, string language = "en-US", bool isGetUpline = false)
        {
            return Content(await customerService.GetCustomerSetting(custId, roleId, CustomerClientManager.ClientConfigs.SystemId, uplineRoleId, language, isGetUpline));
        }

        public async Task<ActionResult> CheckExistedCustomer(string username)
            => Json(await customerService.CheckExistedCustomer(username), JsonRequestBehavior.AllowGet);

        public async Task<ActionResult> GetGlobalSetting(string siteLabel, string site, int currency)
            => Content(await customerService.GetGlobalSetting(siteLabel, site, currency, CustomerClientManager.ClientConfigs.SystemId));

        [HttpPost]
        [NAPCustomerAuthFilter(IsHost = true)]
        public async Task<ActionResult> Create(CreateCustomerCommand request)
        {
            var generalInfo = request.GeneralInfo;

            var checkResult = CheckRequest(request);

            if (checkResult.Code != 0)
            {
                return Content(JsonConvert.SerializeObject(checkResult, jsonSerializerSettings));
            }

            var rawPassword = Security.Services.Encryptor.Decrypt(generalInfo.Password,
                HttpContext.Session[HashKeySessionKey].ToString());
            generalInfo.Password = encryptor.CfsEnCode(rawPassword);

            var response = await customerService.CreateCustomer(new CreateCustomerParam(
                request.GeneralInfo,
                request.ProductGroups,
                AppSession.AccId,
                0,
                Utils.GetClientIP(),
                CreationModes.FullMode));

            return Content(response);
        }

        [HttpPost]
        [NAPCustomerAuthFilter(IsHost = true)]
        public async Task<ActionResult> Update(UpdateCustomerRequest request)
        {
            var response = await customerService.UpdateCustomer(new UpdateCustomerParam(
                request.GeneralInfo,
                request.ProductGroups,
                AppSession.AccId,
                0,
                request.CustId,
                request.UName,
                request.RoleId,
                request.UplineRoleId,
                Utils.GetClientIP(),
                "en-US"));

            return Content(response);
        }

        private string GetApplicationPath()
        {
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            return applicationPath;
        }

        private Result CheckRequest(CreateCustomerCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.GeneralInfo.Site))
            {
                return new Result
                {
                    Code = 1,
                    Message = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertSite")
                };
            }

            if (request.GeneralInfo.Currency == 0)
            {
                return new Result()
                {
                    Code = 1,
                    Message = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertCurrency")
                };
            }

            if (request.GeneralInfo.IsInternal)
            {
                var result = CheckPermissionInsert(request.GeneralInfo.IsInternal);
                if (result.Code != 0) return result;
            }

            return new Result
            {
                Code = 0,
                Message = "Success"
            };
        }

        private Result CheckPermissionInsert(bool isInternal)
        {
            var result = new Result
            {
                Code = 0,
                Message = "Success"
            };

            if (!string.Equals(AppSession.SYSTEM, Enumerations.AccountingSystem.Accounting.ToString(),
                    StringComparison.CurrentCultureIgnoreCase))
            {
                result.Code = 1000;
                result.Message = "You are not authorized on this function";
                return result;
            }

            if (isInternal && !_customerSettingService.CheckPermissionFinance())
            {
                result.Code = 1000;
                result.Message = "You're not authorized to create Finance (Internal) account.";
            }

            return result;
        }

        private string GetHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            HttpContext.Session.Add(HashKeySessionKey, hashKey);

            return hashKey;
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
    }
}