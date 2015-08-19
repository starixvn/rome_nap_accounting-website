// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Customer.Business.GoldDeluxe;
    using Fanex.Tarus.Customer.Criteria.GoldDeluxe;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Sunplus.Accounting.Core.Common;
using Fanex.Tarus.Security.Permission;

    /// <summary>
    /// Gold Deluxe controller.
    /// </summary>
    public class GoldDeluxeController : BaseController
    {
        private readonly IGoldDeluxeService _goldDeluxeService;
        private PermissionService _permissionService;
        public GoldDeluxeController()
            : this(null)
        {
            this._permissionService = this._permissionService ?? new PermissionService();
        }

        public GoldDeluxeController(IGoldDeluxeService goldDeluxeService)
        {
            this._permissionService = this._permissionService ?? new PermissionService();
            _goldDeluxeService = goldDeluxeService ?? new GoldDeluxeService();
        }

        /// <summary>
        /// Index the specified customer ID.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>The action result.</returns>
        [AuthenticateRead]
        public ActionResult Index(int custId, string username)
        {
            var criteria = new GetGoldDeluxeStatusCriteria { CustId = custId };
            var goldDeluxeStatus = _goldDeluxeService.GetStatus(criteria);
            var roleAccess = this._permissionService.GetRoleAccess(AppSession.AccId);
            if (goldDeluxeStatus != null)
            {
                goldDeluxeStatus.CustId = custId;
                goldDeluxeStatus.Username = Customer_Labels.Super + ": " + username;
            }

            var model = JsonConvert.SerializeObject(
                goldDeluxeStatus, 
                Formatting.None, 
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.HasHostPermission = this._permissionService.GetHostPermission(Enumerations.Functions.Customer.ViewInfo, roleAccess) ? 1 : 0;
            return View("~/Areas/CustomerSetting/Views/GoldDeluxe/Index.cshtml", null, model);
        }

        /// <summary>
        /// Updates the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AuthenticateHost]
        public ActionResult Update(GoldDeluxeStatus status)
        {
            var criteria = new UpdateGoldDeluxeStatusCriteria 
            {
                CustId = status.CustId,
                IsEnable = status.Enable,
                ApplyToMember = status.DownlineEnable
            };

            var result = _goldDeluxeService.UpdateStatus(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
