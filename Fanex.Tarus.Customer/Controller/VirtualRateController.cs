// --------------------------------------------------------------------------
// <copyright file="VirtualRateController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Business.VirtualRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;

    /// <summary>
    /// Virtual Rate Controller.
    /// </summary>
    public class VirtualRateController : BaseController
    {
        private readonly IVirtualRateService _virtualRateService;

        public VirtualRateController()
            : this(null)
        {
        }

        public VirtualRateController(IVirtualRateService virtualRateService)
        {
            _virtualRateService = virtualRateService ?? new VirtualRateService();
        }

        public ActionResult Index(ViewVirtualRateCriteria criteria)
        {
            CustomerVirtualRateModel result = _virtualRateService.CustomerVirtualRate(criteria);
            return View("~/Areas/CustomerSetting/Views/VirtualRate/EditRate/Index.cshtml", result);
        }    

        [AuthenticateRead]
        public ActionResult HistoryLogView(ViewHistoryLogCriteria criteria)
        {
            HistoryLogReportModel result = _virtualRateService.HistoryLog(criteria);
            return View("~/Areas/CustomerSetting/Views/VirtualRate/HistoryLog/Index.cshtml", result);
        }

        [HttpPost]
        public JsonResult UpdateSettings(UpdateSettingsCriteria criteria)
        {
            var result = _virtualRateService.UpdateSettings(criteria);
            return Json(result);
        }
        

        public ActionResult GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria)
        {
            return View("~/Areas/CustomerSetting/Views/VirtualRate/SportBetTypesReferences/Index.cshtml", _virtualRateService.GetSportBetTypeReferences(criteria));
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult GetConfirmUpdateMessage(int custId)
        {
            return Json(_virtualRateService.GetConfirmUpdateMessage(custId));
        }
    }
}
