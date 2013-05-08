// -----------------------------------------------------------------------
// <copyright file="CheckBetController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.Others
{
    using Fanex.Tarus.Reports.Business.Others;
    using Fanex.Tarus.Reports.Entities.Others;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckBetController: BaseController
    {
        public readonly ICheckBetService _checkBetService;

        public CheckBetController()
            : this(null)
        {
        }

        public CheckBetController(ICheckBetService checkBetService)
        {
            this._checkBetService = checkBetService ?? new CheckBetService();
        }

        [AuthenticateRead]
        public ActionResult Index() 
        {
            return View("~/Views/Report/Others/CheckBet.cshtml");
        }

        public JsonResult GetCheckBet(string date)
        {
            var checkBet = new CheckBet();
            checkBet = this._checkBetService.GetCheckBet(date);
            var jsonResult = Json(checkBet);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetCheckNormalBet(string date)
        {
            var checkNormalBet = new ChangedTickets();
            checkNormalBet = this._checkBetService.GetCheckNormalBet(date);
            var jsonResult = Json(checkNormalBet);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetCheckRejectBet(string date)
        {
            var checkRejectBet = new ChangedTickets();
            checkRejectBet = this._checkBetService.GetCheckRejectBet(date);
            var jsonResult = Json(checkRejectBet);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
