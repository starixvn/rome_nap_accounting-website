// -----------------------------------------------------------------------
// <copyright file="ParlayWonController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers
{
    using System.Web.Mvc;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Fanex.Tarus.Reports.Business.ParlayWon;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Reports.Entities.ParlayWon;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ParlayWonController:BaseController
    {
        public readonly IParlayWonService _parlayWonService;

        private Helper _helper;

        public ParlayWonController()
            : this(null)
        {
 
        }
            public ParlayWonController(IParlayWonService parlayWonService)
        {
            this._parlayWonService = parlayWonService ?? new ParlayWonService();
                _helper = new Helper();
        }

        [AuthenticateRead]
        public ActionResult ParlayWonReport() 
        {
            ViewBag.BanlanceUpDate = _helper.BalanceUpDate;
            ViewBag.MoveBetsDay = _helper.MovebetsDate;
            return View("~/Views/Report/ParlayWon/ParlayWonReport.cshtml");
        }

        [HttpPost]
        public JsonResult GetData(string winlossDate, int betType, bool isSoccer, string balanceUpDate, string moveBetsDay, bool fromCache, string sortDirection, int pageIndex, int pageSize, int sortBy)
        {
            ParlayWonModel result;

            result = _parlayWonService.GetParlayBusiness(winlossDate, betType, isSoccer,moveBetsDay, balanceUpDate , fromCache, sortDirection, pageIndex, pageSize, sortBy);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
