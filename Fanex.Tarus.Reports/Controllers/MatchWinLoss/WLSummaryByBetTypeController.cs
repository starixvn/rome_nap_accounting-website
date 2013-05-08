


using System;
using System.Web.Mvc;
using DNAide.Core;
using Fanex.Tarus.Reports.Business.MatchWinLoss;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;


namespace Fanex.Tarus.Reports.Controllers.MatchWinLoss
{
    public class WLSummaryByBetTypeController : BaseController
    {
        #region IFields

        //
        // GET: /MatchWinLossByBetTypeService/
        private readonly IWLSummaryByBetTypeService _matchWinLossByBetTypeService;

        #endregion IFields
        #region IConstructions

        public WLSummaryByBetTypeController()
            : this(null)
        {
        }

        public WLSummaryByBetTypeController(IWLSummaryByBetTypeService matchWinLossByBetTypeService)
        {
            _matchWinLossByBetTypeService = matchWinLossByBetTypeService ?? new WLSummaryByBetTypeService();
        }

        #endregion IConstructions
        #region IMethods

        [AuthenticateRead]
        public ActionResult MatchWinLoss2a()
        {
            return View("~/Views/Report/MatchWinLoss2a/MatchWinLoss2a.cshtml");
        }

        #region PostMethod

        #endregion PostMethod

        [HttpPost]
        public JsonResult SelSummaryReportByBetType(string reportDate)
        {
            var result = _matchWinLossByBetTypeService.SelSummaryReportByBetType(CastAide.AsDateTime(reportDate));

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportWLSummaryByBetTypeReport(DateTime reportDate, string reportName)
        {
            _matchWinLossByBetTypeService.ExportWLSummaryByBetTypeReport(reportDate, reportName);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion IMethods
    }
}
