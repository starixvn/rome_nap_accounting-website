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
        private readonly ILicenseeDcsSettingService _settingDCSService;
        private readonly ILicenseeOddsGroupService _settingOddsGroupService;

        public LicenseeOddsGroupController()
            : this(null, null)
        {
        }

        internal LicenseeOddsGroupController(ILicenseeDcsSettingService settingDCSService, ILicenseeOddsGroupService settingOddsGroupService)
        {
            _settingDCSService = settingDCSService ?? new LicenseeDcsSettingService();
            _settingOddsGroupService = settingOddsGroupService ?? new LicenseeOddsGroupService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            var sites = _settingDCSService.GetSites().ToList();

            var supers = sites.Count == 0 ?
                new List<BaseModel>() :
                _settingDCSService.GetSupers(sites[0]).ToList();

            var agents = supers.Count == 0 ?
                Enumerable.Empty<BaseModel>() :
                _settingOddsGroupService.GetAgents(supers[0].Id);

            var model = new FilterViewModel(sites, supers, agents);

            return View("~/Views/Customer/LicenseeOddGroup/Index.cshtml", model);
        }

        [AuthenticateRead]
        public ActionResult GetAgentOddGroups(string custids, string siteName)
        {
            string listOfferOddGroupISites = ConfigurationManager.AppSettings["OfferOddGroupISites"].ToString();
            ViewBag.IsDisplayOddsGroupI = listOfferOddGroupISites.Split(',').ToList().FindIndex(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase)) != -1;

            string listOfferOddGroupMSites = ConfigurationManager.AppSettings["OfferOddGroupMSites"].ToString();
            ViewBag.IsDisplayOddsGroupM = listOfferOddGroupMSites.Split(',').ToList().FindIndex(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase)) != -1;

            string listOfferOddGroupQSites = ConfigurationManager.AppSettings["OfferOddGroupQSites"].ToString();
            ViewBag.IsDisplayOddsGroupQ = listOfferOddGroupQSites.Split(',').ToList().FindIndex(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase)) != -1;

            string listOfferOddGroupGSites = ConfigurationManager.AppSettings["OfferOddGroupGSites"].ToString();
            ViewBag.IsDisplayOddsGroupG = listOfferOddGroupGSites.Split(',').ToList().FindIndex(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase)) != -1;

            string listOfferOddGroupKSites = ConfigurationManager.AppSettings["OfferOddGroupKSites"].ToString();
            ViewBag.IsDisplayOddsGroupK = listOfferOddGroupKSites.Split(',').ToList().FindIndex(x => x.Equals(siteName, StringComparison.OrdinalIgnoreCase)) != -1;

            var model = _settingOddsGroupService.GetAgentOddsGroups(custids);            
            return PartialView("~/Views/Customer/LicenseeOddGroup/_OddsGroup.cshtml", model.ToList());
        }

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateOddsGroup(UpdateOddsGroupRequest request)
        {
            var criteria = new UpdateOddsGroupCriteria()
            {
                CustId = request.CustId,
                OddsGroup = request.OddsGroup,
                ApplyExistingMember = request.ApplyExistingMember,
                IsActive = request.IsActive,
                IP = Utils.GetClientIP(),
                UserId = AppSession.AccId
            };

            return Json(_settingOddsGroupService.UpdateOddsGroup(criteria));
        }

        [HttpPost]
        public ActionResult Agents(int superId)
            => Json(_settingOddsGroupService.GetAgents(superId));
    }
}
