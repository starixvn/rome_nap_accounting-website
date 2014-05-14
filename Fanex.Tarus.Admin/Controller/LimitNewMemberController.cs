// --------------------------------------------------------------------------
// <copyright file="LimitNewMemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Admin.Business.LimitNewMember;
    using Fanex.Tarus.Admin.Criteria.LimitNewMember;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// LimitNewMemberController class.
    /// </summary>
    public class LimitNewMemberController : BaseController
    {
        private const string QueryMode = "Q";
        private const string UpdateMode = "U";

        private readonly ILimitNewMemberService _limitNewMemberService;

        public LimitNewMemberController()
            : this(null)
        { 
        }

        public LimitNewMemberController(ILimitNewMemberService limitNewMemberService)
        {
            _limitNewMemberService = limitNewMemberService ?? new LimitNewMemberService();
        }

        [AuthenticateRead]
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Admin/LimitNewMember/Index.cshtml");
        }

        [AuthenticateRead]
        [HttpGet]
        public ActionResult SearchSuper(LimitNewMemberCriteria criteria)
        {
            criteria.Mode = QueryMode;
            var limitSuper = _limitNewMemberService.GetLimitNewMember(criteria);
            return Json(limitSuper, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        [HttpPost]
        public ActionResult UpdateLimitNewMember(LimitNewMemberCriteria criteria)
        {
            criteria.Mode = UpdateMode;
            if (!criteria.IsValid())
            {
                return Json(new Result { ErrorCode = -1 }, JsonRequestBehavior.DenyGet);
            }

            var result = _limitNewMemberService.UpdateLimitNewMember(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
