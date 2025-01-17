using System;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Customer.Client;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Models;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Services;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.Criteria.CreditCustomer;
using Fanex.Tarus.Customer.Filters;
using Fanex.Tarus.Customer.Requests.Customer;
using Fanex.Tarus.Customer.ViewModels.Customer;
using Fanex.Tarus.Web.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Agent.Utilities;
using Fanex.Tarus.Customer.Requests.ManageSuperSettings;

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

        [HttpPost]
        public ActionResult CheckSyncedNovaCasino(string username)
            => Json(new { IsSynced = _customerSettingService.IsSyncedNovaCasino(username) });

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
        {
            ViewBag.NoPermissionEditCredit = !Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);
            return View("~/Areas/CustomerSetting/Views/SearchCopy/Super/Index.cshtml");
        }

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
                Security.Services.Encryptor.Decrypt(request.Password, code),
                request.Credit,
                0,
                request.SourceCustId,
                request.Site,
                request.Currency,
                false,
                Utils.GetClientIP(),
                ObjectDbExtensions.GetSiteDBConfigBySite(request.Site).CustDBGroupID,
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

        private static bool IsMember(string level)
            => level.Equals("member", StringComparison.OrdinalIgnoreCase);

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
            var viewModel = new CreateCustomerViewModel(new CreateCustomerRequest
            {
                UplineRoleId = RoleIdConstants.SUPER + 1,
                RoleId = RoleIdConstants.SUPER,
                NoPermissionEditCredit = !Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit)
            }, new AppConfiguration(GetApplicationPath()), cdnUri, GetAssetsVersion(), GetHashKey());

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
        public ActionResult CreateDirectMember(CreateDirectMemberRequest request)
        {
            request.NoPermissionEditCredit = !Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

            var viewModel = new CreateDirectMemberViewModel(
                request,
                new AppConfiguration(GetApplicationPath()),
                cdnUri,
                GetAssetsVersion(),
                GetHashKey());

            return View("~/Views/Customer/CreateDirectMember.cshtml", viewModel);
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

        public ActionResult NAPSportBookInformation(SportBookInformationRequest request)
        {
            var appPath = GetApplicationPath();
            request.ApplicationPath = appPath;
            var viewModel = new SportBookInformationViewModel(
                request,
                new AppConfiguration(appPath),
                cdnUri,
                GetAssetsVersion());
            return View(HttpContext.IsDebuggingEnabled
                ? "~/Views/Customer/SportBookInformation/Index-Debug.cshtml"
                : "~/Views/Customer/SportBookInformation/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> GetCustomerSetting(GetCustomerSettingRequest request)
            => Content(await customerService.GetCustomerSetting(
                request.CustId,
                request.RoleId,
                CustomerClientManager.ClientConfigs.SystemId,
                request.UplineRoleId,
                request.Language,
                request.IsGetUpLine,
                request.ProductGroupId,
                request.SiteLabel,
                request.IsMultipleMode,
                "",
                request.IsCopy,
                request.AgentId));

        public async Task<ActionResult> GetCurrentWinLossAmount(int custId, int sportType, int betType, string language = "en-US")
            => Content(await customerService.GetCustomerCurrentWinLossAmounts(custId, sportType, betType, language));

        public async Task<ActionResult> CheckExistedCustomer(string username)
            => Json(await customerService.CheckExistedCustomer(username), JsonRequestBehavior.AllowGet);

        public async Task<ActionResult> GetGlobalSetting(string siteLabel, string site, int currency)
            => Content(await customerService.GetGlobalSetting(siteLabel, site, currency, CustomerClientManager.ClientConfigs.SystemId));

        public async Task<ActionResult> GetMaxPayoutSettingInformation(string site, string siteLabel = "", string language = "en-US")
            => Content(await customerService.GetMaxPayoutSettingInformation(site, siteLabel, language));

        public async Task<ActionResult> GetPTSettingInformation(int? custId, string site, string language = "en-US")
            => Content(await customerService.GetPTSettingInformation(custId, AccCommon.IsLocalTest, site, language));

        public async Task<ActionResult> GetCommissionHint(int? custId, string site, string language = "en-US")
            => Content(await customerService.GetCommissionHint(custId ?? 0, site, language));

        public async Task<ActionResult> GetProductCategories(int? custId, string site, string siteLabel, int currency, string language = "en-US")
            => Content(await customerService.GetProductCategories(custId, site, siteLabel, currency, language));

        public async Task<ActionResult> GetWinLossLimitInformation(int? custId, string site, string siteLabel, int currency, string language = "en-US")
           => Content(await customerService.GetWinLossLimitInformation(custId, site, siteLabel, currency, language));

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
            generalInfo.Password = rawPassword;

            var response = await customerService.CreateCustomer(new CreateCustomerParam(
                generalInfo,
                request.ProductGroups,
                AppSession.AccId,
                0,
                Utils.GetClientIP(),
                generalInfo.Site,
                generalInfo.SiteLabel,
                ObjectDbExtensions.GetSiteDBConfigBySite(request.GeneralInfo.Site).CustDBGroupID,
                request.Group,
                request.NumberOfAccounts,
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
                ObjectDbExtensions.GetSiteDBConfigByCustId(request.CustId)?.CustDBGroupID ?? 0,
                request.Site,
                request.SiteLabel,
                request.Group,
                request.AgentId));

            return Content(response);
        }

        [HttpPost]
        [NAPCustomerAuthFilter(IsHost = true)]
        public async Task<ActionResult> UpdateMultipleCustomers(UpdateMultipleCustomerRequest request)
        {
            var response = await customerService.UpdateMultipleCustomers(
                  new UpdateMultipleCustomerParam(
                      request.ProductGroups,
                      AppSession.AccId,
                      0,
                      request.CustId,
                      "",
                      request.RoleId,
                      request.UplineRoleId,
                      HttpContext.Request.UserHostAddress,
                      ObjectDbExtensions.GetSiteDBConfigByCustId(request.CustId)?.CustDBGroupID ?? 0,
                      request.CustomerIds,
                      request.CustomerNames,
                      request.Site,
                      request.SiteLabel,
                      string.Empty,
                      0));

            return Content(response);
        }

        public ActionResult SportBookInformation(SportBookInformationRequest request)
        {
            const int SystemCode_NAP = 1;
            var appPath = GetApplicationPath();
            request.ApplicationPath = appPath;
            var viewModel = new ViewModels.ManageSuperSettings.SportBookInformationViewModel(
                new ViewModels.ManageSuperSettings.AppConfigurationViewModel(appPath, SystemCode_NAP),
                ConfigurationManager.AppSettings["Fanex.Customer:CdnUri"],
                GetAssetsVersion(),
                request.ToJson());

            return PartialView("SportBookInformation", viewModel);
        }

        public async Task<ActionResult> SearchSuggestDirectMembers(int uplineCustId, string searchUsername)
            => Json(await customerService.SearchSuggestDirectMembers(uplineCustId, searchUsername), JsonRequestBehavior.AllowGet);

        public async Task<int> CheckDirectMemberUserName(int uplineCustId, string searchUsername)
            => await customerService.CheckDirectMemberUserName(uplineCustId, searchUsername);

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