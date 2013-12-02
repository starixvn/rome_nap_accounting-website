// --------------------------------------------------------------------------
// <copyright file="EditLicenseeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller.EditLicensee
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.EditLicensee;
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditLicenseeController : BaseController
    {
        private readonly IEditLicenseeService _editLicenseeService;

        public EditLicenseeController()
            : this(null)
        {
        }

        public EditLicenseeController(IEditLicenseeService editLicenseeService)
        {
            _editLicenseeService = editLicenseeService ?? new EditLicenseeService();
        }

        [HttpGet]
        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/DepositCustomer/EditLicensee/Index.cshtml");
        }

        [HttpPost]
        [AuthenticateRead]
        public JsonResult GetLicenseeInfo(GetLicenseeInfoCriteria criteria)
        {
            var result = _editLicenseeService.GetLicenseeInfo(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateLicenseeInfo(UpdateLicenseeInfoCriteria criteria)
        {
            var result = _editLicenseeService.UpdateLicenseeInfo(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
