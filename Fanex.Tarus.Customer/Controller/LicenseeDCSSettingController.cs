using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    public class LicenseeDcsSettingController : BaseController
    {
        private readonly ILicenseeDcsSettingService _settingService;

        public LicenseeDcsSettingController()
            : this(null)
        {
        }

        internal LicenseeDcsSettingController(ILicenseeDcsSettingService settingService)
        {
            _settingService = settingService ?? new LicenseeDcsSettingService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            var sites = _settingService.GetSites().ToList();

            var supers = sites.Count == 0 ?
                new List<BaseModel>() :
                _settingService.GetSupers(sites[0]).ToList();

            var agents = supers.Count == 0 ?
                Enumerable.Empty<BaseModel>() :
                _settingService.GetAgents(supers[0].Id);

            var products = _settingService.GetProducts();
            var model = new FilterViewModel(sites, supers, agents, products);

            return View("~/Views/Customer/LicenseeDCSSetting/Index.cshtml", model);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult Supers(string site)
            => Json(_settingService.GetSupers(site));

        [HttpPost]
        public ActionResult Agents(int superId)
            => Json(_settingService.GetAgents(superId));

        [HttpPost, AuthenticateRead]
        public ActionResult PT(PTViewModel model)
        {
            // Get first AgentId.
            var agentIds = ConvertToIntArray(model.AgentIds).ToList();

            if (agentIds.Count == 1)
            {
                model.SuperPT = _settingService.GetPositionTaking(model.ProductId, agentIds[0]);
            }

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_PT.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdatePT(PTViewModel model)
        {
            var result = _settingService.UpdatePositionTaking(
                model.ProductId,
                model.SuperId,
                model.SuperPT,
                ConvertToIntArray(model.AgentIds),
                model.IsAgentOnly,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult BetSetting(BetSettingViewModel model)
        {
            // Get first AgentId.
            var agentIds = ConvertToIntArray(model.AgentIds).ToList();
            var agentId = agentIds.Count == 1 ? agentIds[0] : 0;

            var betSettings = _settingService.GetBetSettings(model.ProductId, model.SuperId, agentId);
            model.SuperBetSettings = betSettings[0].ToDictionary(s => s.SportType, s => s);
            model.BetSettings = betSettings[1];

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_BetSetting.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateBetSetting(BetSettingViewModel model)
        {
            var result = _settingService.UpdateBetSetting(
                model.SuperId,
                model.ApplyExistedMem,
                model.BetSettings.FirstOrDefault(),
                ConvertToIntArray(model.AgentIds),
                model.IsAgentOnly,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult Status(StatusViewModel model)
        {
            var agentIds = ConvertToIntArray(model.AgentIds).ToList();
            var custId = agentIds.Count == 0 ? model.SuperId : agentIds[0];
            var isSelectedMultipleAgents = agentIds.Count > 1;

            model.Status = _settingService.GetStatus(model.ProductId, custId);

            if (isSelectedMultipleAgents)
            {
                model.Status.IsDisabled = true;
                model.Status.IsDisabledDownline = true;
            }

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_Status.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateStatus(StatusViewModel model)
        {
            var result = _settingService.UpdateStatus(
                model.ProductId,
                model.SuperId,
                model.Status,
                ConvertToIntArray(model.AgentIds),
                model.IsAgentOnly,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult GlobalStakeLevel(GlobalStakeLevelViewModel model)
        {
            model.GroupGlobalStakeLevels = _settingService.GetGameTypeGlobalStakeLevels(model.ProductId, model.SuperId);

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_GlobalStakeLevel.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateGlobalStakeLevel(int superId, GlobalStakeLevelEntity[] globalStakeLevels)
        {
            var result = _settingService.UpdateGlobalStakeLevels(
                superId,
                globalStakeLevels,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult StakeLevel(StakeLevelViewModel model)
        {
            var agentIds = ConvertToIntArray(model.AgentIds).ToList();
            var custId = agentIds.Count == 0 ? model.SuperId : agentIds[0];

            var criteria = new GetStakeLevelCriteria(model.ProductId, custId);
            var groupStakeLevels = _settingService.GetSportBetTypeStakeLevels(criteria);

            model.GroupStakeLevels = groupStakeLevels;
            model.ErrorCode = criteria.ErrCode;
            model.ErrorMessage = criteria.ErrMsg;
            model.IsSelectedMultipleAgents = agentIds.Count > 1;

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_StakeLevel.cshtml", model);
        }

        [HttpPost, AuthenticateHost]
        public ActionResult UpdateStakeLevel(
            int productId,
            int superId,
            StakeLevelEntity[] stakeLevels,
            string agentIds,
            bool isAgentOnly)
        {
            var result = _settingService.UpdateStakeLevels(
                productId,
                superId,
                stakeLevels,
                ConvertToIntArray(agentIds),
                isAgentOnly,
                AppSession.AccId,
                Utils.GetClientIP());

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult StakeLevelGroup(StakeLevelViewModel model)
        {
            var agentIds = ConvertToIntArray(model.AgentIds).ToList();
            var custId = agentIds.Count == 0 ? model.SuperId : agentIds[0];

            var criteria = new GetStakeLevelGroupCriteria(model.ProductId, custId);
            var groupStakeLevelGroups = _settingService.GetSportBetTypeStakeLevelGroups(criteria);

            model.GroupStakeLevels = groupStakeLevelGroups;
            model.ErrorCode = criteria.ErrCode;
            model.ErrorMessage = criteria.ErrMsg;
            model.IsSelectedMultipleAgents = agentIds.Count > 1;

            return PartialView("~/Views/Customer/LicenseeDCSSetting/_StakeLevelGroup.cshtml", model);
        }

        private static IEnumerable<int> ConvertToIntArray(string idList)
            => string.IsNullOrWhiteSpace(idList)
                ? Enumerable.Empty<int>()
                : idList
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.Parse(id));
    }
}
