using Fanex.Tarus.Customer.Business.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Business.LicenseeOddsGroup;
using Fanex.Tarus.Customer.Criteria.LicenseeOddsGroup;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Requests.LicenseeOddsGroup;
using Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Tarus.Customer.Controller
{
    public class LicenseeOddsGroupController : BaseController
    {
        private readonly ILicenseeDcsSettingService _settingDcsService;
        private readonly ILicenseeOddsGroupService _oddsGroupService;
        private const string ViewPathPrefix = "~/Views/Customer/LicenseeOddGroup/";

        public LicenseeOddsGroupController()
            : this(null, null)
        {
        }

        internal LicenseeOddsGroupController(ILicenseeDcsSettingService settingDcsService, ILicenseeOddsGroupService oddsGroupService)
        {
            _settingDcsService = settingDcsService ?? new LicenseeDcsSettingService();
            _oddsGroupService = oddsGroupService ?? new LicenseeOddsGroupService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            try
            {
                var model = GetFilterViewModel();
                return View($"{ViewPathPrefix}Index.cshtml", model);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [AuthenticateRead]
        public ActionResult GetAgentOddGroups(string custids, string siteName)
        {
            try
            {
                ConfigureOddsGroupVisibility(siteName);
                var model = _oddsGroupService.GetAgentOddsGroups(custids).ToList();
                return PartialView($"{ViewPathPrefix}_OddsGroup.cshtml", model);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateOddsGroup(UpdateOddsGroupRequest request)
        {
            try
            {
                var criteria = CreateUpdateOddsGroupCriteria(request);
                var result = _oddsGroupService.UpdateOddsGroup(criteria);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while updating odds group." });
            }
        }

        [HttpPost]
        public ActionResult Agents(int superId)
        {
            try
            {
                var agents = _oddsGroupService.GetAgents(superId);
                return Json(agents);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to retrieve agents." });
            }
        }

        #region Private Methods

        private FilterViewModel GetFilterViewModel()
        {
            var sites = _settingDcsService.GetSites().ToList();
            var supers = sites.Any()
                ? _settingDcsService.GetSupers(sites[0]).ToList()
                : new List<BaseModel>();
            var agents = supers.Any()
                ? _oddsGroupService.GetAgents(supers[0].Id)
                : Enumerable.Empty<BaseModel>();

            return new FilterViewModel(sites, supers, agents);
        }

        private UpdateOddsGroupCriteria CreateUpdateOddsGroupCriteria(UpdateOddsGroupRequest request)
        {
            return new UpdateOddsGroupCriteria
            {
                CustId = request.CustId,
                OddsGroup = request.OddsGroup,
                ApplyExistingMember = request.ApplyExistingMember,
                IsActive = request.IsActive,
                IP = Utils.GetClientIP(),
                UserId = AppSession.AccId
            };
        }

        private void ConfigureOddsGroupVisibility(string siteName)
        {
            var oddsGroupTypes = new[] { "I", "M", "Q", "G", "K", "HJ", "NOPRSTULVWXYZ" };

            foreach (var type in oddsGroupTypes)
            {
                string siteList = ConfigurationManager.AppSettings[$"OfferOddGroup{type}Sites"] ?? string.Empty;
                bool isVisible = !string.IsNullOrEmpty(siteList) &&
                                 siteList.Split(',')
                                         .Any(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase));
                ViewData[$"IsDisplayOddsGroup{type}"] = isVisible;
            }
        }

        private ActionResult HandleError(Exception ex)
        {
            // Log the error (replace with your logging mechanism)
            return View("Error", new HandleErrorInfo(ex, "LicenseeOddsGroup", "Index"));
        }

        #endregion
    }
}