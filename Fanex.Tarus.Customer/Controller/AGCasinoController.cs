// --------------------------------------------------------------------------
// <copyright file="AGCasinoController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.AGCasino;
    using Fanex.Tarus.Customer.Criteria.AGCasino;
    using Fanex.Tarus.Customer.Entities.AGCasino;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// AG Casino Controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules",
        "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "Reviewed. Suppression is OK here.")]
    public class AGCasinoController : BaseController
    {
        private readonly IAGCasinoService _agCasinoService;

        public AGCasinoController()
            : this(null)
        {
        }

        public AGCasinoController(IAGCasinoService agCasinoService)
        {
            _agCasinoService = agCasinoService ?? new AGCasinoService();
        }

        [AuthenticateRead]
        public ActionResult AGCasinoStatus(int custId, int roleId)
        {
            return View(
                "~/Areas/CustomerSetting/Views/AGCasino/Status/Index.cshtml",
                new AGCasinoCustomerInfo
                {
                    CustomerId = custId,
                    RoleId = roleId
                });
        }

        /// <summary>
        /// Edits the AG casino game setting.
        /// </summary>
        /// <param name="customerInfo">The customer information.</param>
        /// <returns>Edit view.</returns>
        [AuthenticateRead]
        public ActionResult EditAGCasinoGameSetting(AGCasinoCustomerInfo customerInfo)
        {
            return View(
                "~/Areas/CustomerSetting/Views/AGCasino/EditGameSetting/Index.cshtml",
                new AGCasinoCustomerInfo
                {
                    CustomerId = customerInfo.CustomerId,
                    RoleId = customerInfo.RoleId,
                    UplineId = customerInfo.UplineId
                });
        }

        [AuthenticateRead]
        public ActionResult AGCasinoAPIResult(GetAGCasinoAPIResultCriteria criteria)
        {
            return View("~/Areas/AGCasino/Views/APITest/Index.cshtml", null, _agCasinoService.AGCasinoAPIResult(criteria));
        }

        [HttpPost]
        [AuthenticateRead]
        public JsonResult CustomerStatus(GetCustomerStatusCriteria criteria)
        {
            var result = _agCasinoService.CustomerStatus(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            var result = _agCasinoService.UpdateCustomerStatus(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateRead]
        public JsonResult CustomerSetting(GetCustomerSettingCriteria criteria)
        {
            var result = _agCasinoService.CustomerSetting(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateCustomerSetting(UpdateCustomerSettingCriteria criteria)
        {
            var result = _agCasinoService.UpdateCustomerSetting(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
