// --------------------------------------------------------------------------
// <copyright file="AutoSuspendController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.AutoSuspend;
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoSuspendController : BaseController
    {
        private readonly IAutoSuspendService _autoSuspendService;

        public AutoSuspendController()
            : this(null)
        {
        }

        public AutoSuspendController(IAutoSuspendService autoSuspendService)
        {
            _autoSuspendService = autoSuspendService ?? new AutoSuspendService();
        }

        [HttpPost]
        [AuthenticateRead]
        public JsonResult GetCustomerStatus(GetCustomerStatusCriteria criteria)
        {
            var result = _autoSuspendService.GetCustomerStatus(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            var result = _autoSuspendService.UpdateCustomerStatus(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
