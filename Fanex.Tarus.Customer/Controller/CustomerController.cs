using System;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Customer.Client.Configurations;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Models;
using Fanex.Customer.Client.NETFramework;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Services;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.Criteria.CreditCustomer;
using Fanex.Tarus.Customer.Requests;
using Fanex.Tarus.Customer.ViewModels.Customer;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Agent.Utilities;

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

        public CustomerController()
            : this(null, null, null)
        {
        }

        internal CustomerController(ICustomerSettingService customerSettingService, Encryptor encryptor,
            ICustomerService customerService)
        {
            cdnUri = ConfigurationManager.AppSettings["Fanex.Customer:CdnUri"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            this.encryptor = encryptor ?? new EncryptorImpl();
            this.customerService =
                customerService ?? new CustomerService(new CustomerClientConfiguration().Initialize());
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
        public ActionResult NewCustomer()
            => View("~/Views/Customer/NewCustomer.cshtml");

        [AuthenticateRead]
        public ActionResult Copy()
            => View("~/Areas/CustomerSetting/Views/SearchCopy/Super/Index.cshtml");

        [HttpPost]
        public async Task<ActionResult> Copy(CopyCustomerRequest request, string code)
        {
            var param = new CopyCustomerParam(
                AppSession.AccId,
                0,
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
            var viewModel = new CreateCustomerViewModel(new AppConfiguration(GetApplicationPath()), cdnUri,
                GetAssetsVersion(), GetHashKey());
            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateSuper.cshtml", viewModel);
        }

        [AuthenticateRead]
        public ActionResult CreateCustomer(int uplineId, byte uplineRoleId, string uplineUsername, string site,
            string siteLabel)
        {
            if (!string.IsNullOrWhiteSpace(site)
                && uplineRoleId == RoleIdConstants.SUPER
                && (site.Equals("76666B", StringComparison.OrdinalIgnoreCase) ||
                    site.Equals("IBCIndo", StringComparison.OrdinalIgnoreCase)))
            {
                return View("~/Views/Customer/NotAllowAddDirectMaster.cshtml");
            }

            if (string.IsNullOrWhiteSpace(uplineUsername))
            {
                uplineUsername = AppSession.CustUName;
            }

            var viewModel = new CreateCustomerViewModel(
                new AppConfiguration(GetApplicationPath()),
                cdnUri,
                GetAssetsVersion(),
                GetHashKey(),
                uplineId,
                uplineRoleId,
                uplineUsername,
                site,
                siteLabel);

            return View(
                HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/Create-Debug.cshtml"
                    : "~/Views/Customer/CreateCustomer.cshtml", viewModel);
        }

        [AuthenticateRead]
        public ActionResult EditCustomer(int custId, int roleId, string uname, string siteName)
        {
            var editCustomerViewModel = new EditCustomerViewModel(
                  new AppConfiguration(GetApplicationPath()),
                  cdnUri,
                  GetAssetsVersion(),
                  roleId, uname)
            ;
            return View(HttpContext.IsDebuggingEnabled
                    ? "~/Views/Customer/EditCustomer-Debug.cshtml"
                    : "~/Views/Customer/EditCustomer.cshtml", editCustomerViewModel);
        }

        public async Task<ActionResult> GetCustomerSetting(int custId, byte roleId, string language = "en-US", bool isGetUpline = false)
        => Content(await customerService.GetCustomerSetting(custId, roleId, language, isGetUpline));

        public async Task<ActionResult> CheckExistedCustomer(string username)
            => Json(await customerService.CheckExistedCustomer(username), JsonRequestBehavior.AllowGet);

        public async Task<ActionResult> GetGlobalSetting(string site, int currency)
            => Content(await customerService.GetGlobalSetting(site, currency));

        [HttpPost]
        [AuthenticateHost]
        public async Task<ActionResult> Create(CreateCustomerRequest request)
        {
            var generalInfo = request.GeneralInfo;

            var checkResult = CheckRequest(request);

            if (checkResult.Code != 0)
            {
                return Content(JsonConvert.SerializeObject(checkResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
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

        private string GetApplicationPath()
        {
            var applicationPath = Response.ApplyAppPathModifier(Request.ApplicationPath);
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            return applicationPath;
        }

        private Result CheckRequest(CreateCustomerRequest request)
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