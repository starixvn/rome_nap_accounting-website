// --------------------------------------------------------------------------
// <copyright file="ConfigureDirectAccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;
    using Fanex.Tarus.Customer.Business.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// AG Casino Controller.
    /// </summary>
    public class ConfigureDirectAccountController : BaseController
    {
        private readonly IConfigureDirectAccountService _configureDirectAccountService;

        public ConfigureDirectAccountController()
            : this(null)
        {
        }

        public ConfigureDirectAccountController(IConfigureDirectAccountService configureDirectAccountService)
        {
            _configureDirectAccountService = configureDirectAccountService ?? new ConfigureDirectAccountService();
        }

        /// <summary>
        /// Updates the direct account status.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isCheckedDirectStatus">If set to <c>true</c> [is checked direct status].</param>
        /// <returns>Update result.</returns>
        [AuthenticateRead]
        public ActionResult UpdateDirectAccountStatus(string username, int userId, bool isCheckedDirectStatus)
        {
            var custInfo = new CustomerInfo
            {
                UserName = username,
                CustId = userId,
                DirectAccountStatusInfo = new DirectAccountStatusInfo
                {
                    IsCheckedDirectStatus = isCheckedDirectStatus
                }
            };

            return View("~/Areas/CustomerSetting/Views/ConfigureDirectAccount/ChangeStatus/Index.cshtml", custInfo);
        }

        [AuthenticateRead]
        public ActionResult ImportDirectAccount()
        {
            return View("~/Areas/CustomerSetting/Views/ConfigureDirectAccount/ImportDirectAccount/Index.cshtml");
        }

        [AuthenticateRead]
        public JsonResult UpdateDirectAccountStatusEffect(UpdateDirectStatusEffectCriteria criteria)
        {
            var result = _configureDirectAccountService.UpdateDirectAccountStatusEffect(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateDirectAccountStatus(UpdateDirectAccountStatusCriteria criteria)
        {
            var result = _configureDirectAccountService.UpdateDirectAccountStatus(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult DownloadImportDirectAccountTemplate()
        {
            _configureDirectAccountService.DownloadImportDirectAccountTemplate();
            return new EmptyResult();
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult UpdateImportDirectAccount(UpdateImportDirectAccountCriteria criteria)
        {
            var result = _configureDirectAccountService.UpdateImportDirectAccount(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
