// --------------------------------------------------------------------------
// <copyright file="AdminSecurityCodeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Controller.SecurityCode
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Tarus.Admin.Business.SecurityCode;
    using Fanex.Tarus.Admin.Criteria.SecurityCode;
    using Fanex.Tarus.Admin.Entities.SecurityCode;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class AdminSecurityCodeController : BaseController
    {        
        private readonly IAdminSecurityCodeService _reportService;

        public AdminSecurityCodeController()
            : this(null)
        {
        }

        public AdminSecurityCodeController(IAdminSecurityCodeService reportService)
        {
            this._reportService = reportService ?? new AdminSecurityCodeService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Admin/SecurityCode/UpdateMultiUsers/Index.cshtml");
        }

        [HttpPost, AuthenticateHost]
        public JsonResult GetUserList(GetUserListCriteria criteria)
        {
            var result = this._reportService.GetUserList(criteria);
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateUsersSecurityCode(List<CustomerInfo> criteria)
        {
            var result = this._reportService.UpdateUsersSecurityCode(criteria);
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidateUsers()
        {           
           if (HttpContext.Request.Files != null && HttpContext.Request.Files.Count > 0)
            {
                var excelFile = HttpContext.Request.Files[0];
                var result = _reportService.ValidateCustomers(excelFile);

                var jsonResult = Json(result);
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            return null;
        }
    }
}
