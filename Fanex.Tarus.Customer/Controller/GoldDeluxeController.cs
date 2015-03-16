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
    using Fanex.Tarus.Customer.Business.GoldDeluxe;
    using Fanex.Tarus.Customer.Criteria.GoldDeluxe;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Web.Controllers;
    using Newtonsoft.Json;

    /// <summary>
    /// Gold Deluxe controller.
    /// </summary>
    public class GoldDeluxeController : BaseController
    {
        private readonly IGoldDeluxeService _goldDeluxeService;

        public GoldDeluxeController()
            : this(null)
        {
        }

        public GoldDeluxeController(IGoldDeluxeService goldDeluxeService)
        {
            _goldDeluxeService = goldDeluxeService ?? new GoldDeluxeService();
        }

        /// <summary>
        /// Index the specified customer ID.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>The action result.</returns>
        public ActionResult Index(int custId, string username)
        {
            var criteria = new GetGoldDeluxeStatusCriteria { CustId = custId };
            var goldDeluxeStatus = _goldDeluxeService.GetStatus(criteria);
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

            return View("~/Areas/CustomerSetting/Views/GoldDeluxe/Index.cshtml", null, model);
        }

        /// <summary>
        /// Updates the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
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
