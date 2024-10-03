using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Business.ManageSuperSettings;
using Fanex.Tarus.Customer.Business.MultipleUpdateSuperStatus;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Requests.ManageSuperSettings;
using Fanex.Tarus.Customer.ViewModels.ManageSuperSettings;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Configurations;

namespace Fanex.Tarus.Customer.Controller
{
    public class ManageSuperSettingsController : BaseController
    {
        private const int SystemCode_NAP = 1;

        private readonly IManageSuperSettingsService service;
        private readonly IMultipleUpdateSuperStatusService multipleUpdateSuperStatusService;

        public ManageSuperSettingsController()
        {
            service = new ManageSuperSettingsService();
            multipleUpdateSuperStatusService = multipleUpdateSuperStatusService ?? new MultipleUpdateSuperStatusService();
        }

        [AuthenticateRead]
        public ActionResult Index(IndexRequest request)
        {
            return View("~/Views/Customer/ManageSuperSettings/Index.cshtml", request);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult CheckSuperList(CheckSuperListRequest request)
        {
            var result = service.CheckSuperList(request);
            return Json(result);
        }

        public ActionResult CopyBetSettings(CopyBetSettingsRequest request)
        {
            request.IsCopy = true;
            request.Mode = "BetSetting";
            request.RoleId = RoleIdConstants.SUPER;
            request.UplineRoleId = 5;

            var checkResult = service.CheckSuperList(request);
            request.CustId = checkResult.CustId;
            request.CustIds = checkResult.SuperListIds;
            request.Usernames = request.SuperList;

            var appConfig = new AppConfigurationViewModel(GetApplicationPath(), SystemCode_NAP);
            var viewModel = new CopyBetSettingsViewModel(
                appConfig,
                ConfigurationManager.AppSettings["Fanex.Customer:CdnUri"],
                WebSettings.Version,
                request.ToJson());

            var viewPath = "~/Views/Customer/ManageSuperSettings/CopyBetSettings.cshtml";
            return View(viewPath, viewModel);
        }

        public ActionResult EditBetSettings(CopyBetSettingsRequest request)
        {
            request.IsCopy = false;
            request.Mode = "BetSetting";
            request.RoleId = RoleIdConstants.SUPER;
            request.UplineRoleId = 5;
            var checkResult = service.CheckSuperList(request);

            request.CustId = checkResult.CustId;
            request.CustIds = checkResult.SuperListIds;
            request.Usernames = request.SuperList;

            var appConfig = new AppConfigurationViewModel(GetApplicationPath(), SystemCode_NAP);
            var viewModel = new CopyBetSettingsViewModel(
                appConfig,
                ConfigurationManager.AppSettings["Fanex.Customer:CdnUri"],
                WebSettings.Version,
                request.ToJson());

            return View("~/Views/Customer/ManageSuperSettings/CopyBetSettings.cshtml", viewModel);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult EditSuperInfo(EditSuperInfoRequest request)
        {
            if (request.EditGivenCreditRequest != null)
            {
                var isallowupdategivencredit = Permissions.GetHostPermission(Sunplus.Accounting.Core.Common.Enumerations.Functions.Customer.Credit);
                if (!isallowupdategivencredit)
                {
                    return Json(new List<Result> { new Result { ErrorCode = 1, ErrorMessage = "You do not have permission on update credit." } });
                }
            }

            if (request.EditPasswordRequest != null)
            {
                var canUpdate = Permissions.HasHostPermission(Sunplus.Accounting.Core.Common.Enumerations.Functions.Customer.Password);
                if (!canUpdate)
                {
                    return Json(new List<Result> { new Result { ErrorCode = 1, ErrorMessage = "You do not have permission on update password." } });
                }
            }

            if (request.EditSecurityCodeRequest != null)
            {
                var canUpdate = Permissions.HasHostPermission("SecurityCode");
                if (!canUpdate)
                {
                    return Json(new List<Result> { new Result { ErrorCode = 1, ErrorMessage = "You do not have permission on update security code." } });
                }
            }

            return Json(service.EditSuperInfo(request, AppSession.AccId, Utils.GetClientIP()));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult GetWinLossLimit(GetWinLossLimitRequest request)
        {
            var winLossLimit = service.GetWinLossLimit(request);
            return Json(winLossLimit);
        }

        [HttpPost, AuthenticateHost]
        public async Task<ActionResult> CopyWinLossLimit(CopyWinLossLimitRequest request)
        {
            return Json(await service.CopyWinLossLimit(request, AppSession.AccId, Utils.GetClientIP()));
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
    }
}
