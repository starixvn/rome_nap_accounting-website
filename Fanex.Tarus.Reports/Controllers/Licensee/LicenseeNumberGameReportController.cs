// -----------------------------------------------------------------------
// <copyright file="LicenseeNumberGameReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.Licensee
{
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Business.Licensee;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeNumberGameReportController: BaseController
    {
        INumberGameWinLossService _numberGameService = null;
        public LicenseeNumberGameReportController(): this(null) { }
        public LicenseeNumberGameReportController(INumberGameWinLossService numberGameService) {
            _numberGameService = numberGameService ?? new NumberGameWinLossService();
        }

        [AuthenticateRead]
        public ActionResult NumberGameWinLoss()
        {
            return View("~/Views/Report/Licensee/NumberGameWinLoss/NumberGameWinLoss.cshtml");
        }

        [HttpPost]
        public JsonResult SelAllLicensees()
        {
            var result = _numberGameService.SelAllLicensees();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelNumberGameWinLoss(string normalLicensee, string operators, string fromDate, string toDate)
        {
            var result = _numberGameService.SelNumberGameWinLoss(normalLicensee, operators, fromDate, toDate);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
