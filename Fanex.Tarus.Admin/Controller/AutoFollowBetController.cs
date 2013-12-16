// --------------------------------------------------------------------------
// <copyright file="AutoFollowBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Controller
{
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Tarus.Admin.Business.AutoFollowBet;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class AutoFollowBetController : BaseController
    {
        private readonly IAutoFollowBetService _autoFollowBetService;

        public AutoFollowBetController(IAutoFollowBetService crossTransferService)
        {
            _autoFollowBetService = crossTransferService ?? new AutoFollowBetService();
        }

        public AutoFollowBetController()
            : this(null)
        {
            _autoFollowBetService = new AutoFollowBetService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Admin/AutoFollowBetCustomer.cshtml");
        }

        [AuthenticateHost]
        public JsonResult InsAutoFollowedBet(string username, string rate)
        {
            var result = new Result();
            result = _autoFollowBetService.InsFollowedCustomer(username, CastAide.AsDouble(rate));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetFollowType()
        {
            var result = _autoFollowBetService.GetFollowType();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoFollowedBetCustomer(GetAutoFollowedBetCustomerCriteria criteria)
        {
            var result = _autoFollowBetService.GetAutoFollowedBetCustomer(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult DeleteAutoFollowedBetCustomer(DeleteAutoFollowedBetCustomerCriteria criteria)
        {
            var result = _autoFollowBetService.DeleteAutoFollowedBetCustomer(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public JsonResult UpdateAutoFollowedBetCustomer(UpdateAutoFollowedBetCustomerCriteria criteria)
        {
            var result = _autoFollowBetService.UpdateAutoFollowedBetCustomer(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
