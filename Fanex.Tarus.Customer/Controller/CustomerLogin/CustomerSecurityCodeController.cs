// --------------------------------------------------------------------------
// <copyright file="CustomerSecurityCodeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.CustomerLogin;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerSecurityCodeController : BaseController
    {
        private readonly ICustomerSecurityCodeService _reportService;

        public CustomerSecurityCodeController()
            : this(null)
        {
        }

        public CustomerSecurityCodeController(ICustomerSecurityCodeService reportService)
        {
            this._reportService = reportService ?? new CustomerSecurityCodeService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/CustomerLogin/CustomerSecurityCode/Index.cshtml");
        }

        [HttpPost, AuthenticateHost]
        public JsonResult ResetSecurityCode(ResetSecurityCodeCriteria criteria)
        {
            var result = this._reportService.ResetSecurityCode(criteria);
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
