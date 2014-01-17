// -----------------------------------------------------------------------
// <copyright file="BACustomerController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;
using Fanex.Tarus.Admin.Business.BACustomer;
using Fanex.Tarus.Admin.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BACustomerController : BaseController
    {
        private readonly IBACustomerService _baCustomerService;

        public BACustomerController(IBACustomerService baCustomerService)
        {
            _baCustomerService = baCustomerService ?? new BACustomerService();
        }

        public BACustomerController()
            : this(null)
        {
            _baCustomerService = new BACustomerService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Admin/BACustomer.cshtml", _baCustomerService.GetLicenseeBA() );
        }

        [AuthenticateRead]
        public JsonResult GetBACustomerList(int LicenseeId, int status, string username, int pageSize, int pageIndex, int fromCache)
        {
            var followedCustomer = new FollowedCustomerDTO();
            followedCustomer = _baCustomerService.SelBACustomerList(LicenseeId, status, username, pageSize, pageIndex, fromCache);
            var jsonResult = Json(followedCustomer);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;

        }

        [AuthenticateHost]
        public JsonResult InsMultiBACustomer(string strCustId, string Rate, string follower)
        {
            var result = new Result();
            result = _baCustomerService.InsBACustomerList(strCustId, Rate, follower);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
