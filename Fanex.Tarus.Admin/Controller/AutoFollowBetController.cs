namespace Fanex.Tarus.Admin.Controller
{
    using System.Web.Mvc;

    using Fanex.Tarus.Admin.Business.AutoFollowBet;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Fanex.Tarus.Admin.Entities;
    using DNAide.Core;
    using System.Collections.Generic;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;

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
        public JsonResult UpdAutoFollowedBet(string username, string rate)
        {
            var result = new Result();
            result = _autoFollowBetService.UpdFollowedCustomer(username, CastAide.AsDouble(rate));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [AuthenticateHost]
        public JsonResult DelAutoFollowedBet(string username)
        {
            var result = new Result();
            result = _autoFollowBetService.DelFollowedCustomer(username);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
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
        //public JsonResult ReloadData()
        //{
        //    var listFollowedCustomer  = new List<FollowedCustomer>();
        //    listFollowedCustomer = _autoFollowBetService.SelFollowedCustomer(status);
        //    var jsonResult = Json(listFollowedCustomer);
        //    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return jsonResult;
        //}

        public JsonResult GetAutoFollowedBet(GetAutoFollowedBetCriteria criteria)
        {
            var listFollowedCustomer = new List<FollowedCustomer>();
            listFollowedCustomer = _autoFollowBetService.SelFollowedCustomer(criteria);
            var jsonResult = Json(listFollowedCustomer);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetFollowType()
        {
            var result = _autoFollowBetService.GetFollowType();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
