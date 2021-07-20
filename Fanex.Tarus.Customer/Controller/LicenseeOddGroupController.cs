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
using System.Collections.Generic;
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
        public ActionResult GetAgentOddGroups(string custids)
        {
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
    }
}
