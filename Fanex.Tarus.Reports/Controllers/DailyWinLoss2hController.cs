
using System.Web.Mvc;
using Fanex.Tarus.Reports.Business.DailyWinLoss2h;
using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Reports.Controllers
{
    public class DailyWinLoss2hController : BaseController
    {
        #region IFields

        //
        // GET: /WinLoss2hService/
        private readonly IDailyWinLoss2hService _dailyWinLoss2hService;

        #endregion IFields
        #region IConstructions

        public DailyWinLoss2hController()
            : this(null)
        {
        }

        public DailyWinLoss2hController(IDailyWinLoss2hService dailyWinLoss2hService)
        {
            _dailyWinLoss2hService = dailyWinLoss2hService ?? new DailyWinLoss2hService();
        }

        #endregion IConstructions
        #region IMethods

        [AuthenticateRead]
        public ActionResult DailyWinLoss2h()
        {
            return View("~/Views/Report/DailyWinLoss2h/DailyWinLoss2h.cshtml");
        }

        [AuthenticateRead]
        public ActionResult DailyWL_CombinationMixParlayManager()
        {
            return View("~/Views/Report/DailyWL_CombinationMixParlayManager/DailyWL_CombinationMixParlayManager.cshtml");
        }

        #region PostMethod

        [HttpPost]
        public JsonResult DailyWLDirect_Sporttype(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirect_Sporttype(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public void SaveReportContent(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.SaveReportContent(param);
        }

        [HttpPost]
        public ActionResult ExportToExcel(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DailyWLDirect_MixParlay(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirect_MixParlay(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ExportToExcel_DailyWLDirect_MixParlay(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_DailyWLDirect_MixParlay(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DailyWLDirectRacing(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirectRacing(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DailyWLDirectRacingBySport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirectRacingBySport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DailyWLDirectRacingBySportDetails(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirectRacingBySportDetails(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Export2ExcelDailyWLDirectRacing(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.Export2ExcelDailyWLDirectRacing(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Export2ExcelDailyWLDirectRacingBySport(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.Export2ExcelDailyWLDirectRacingBySport(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Export2ExcelDailyWLDirectRacingBySportDetails(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.Export2ExcelDailyWLDirectRacingBySportDetails(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelSoccerReportMain(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelSoccerReportMain(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelDailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelDailyWLDirectDetails(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_SoccerMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_SoccerMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_DailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_DailyWLDirectDetails(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DailyWLDirect_NumberGame(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.DailyWLDirect_NumberGame(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_NumberGameMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_NumberGameMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelCasinoReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelCasinoReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_CasinoMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_CasinoMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelBingoReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelBingoReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_BingoMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_BingoMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelFinanceReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelFinanceReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelLiveCasinoReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelLiveCasinoReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ExportExcel_LiveCasinoMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportExcel_LiveCasinoMain(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelAdvanceMixParlayReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelAdvanceMixParlayReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Export2Excel_MixParlayAdvanceMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.Export2Excel_MixParlayAdvanceMain(param);

            var jsonResult = Json("Success");
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelOtherSportsReport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelOtherSportsReport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelOtherSportsReportBySport(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hService.SelOtherSportsReportBySport(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsMain_NoneLive(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsMain_NoneLive(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsMain_Live(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsMain_Live(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsBySport(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsBySport(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsBySport_NoneLive(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsBySport_NoneLive(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_OtherSportsBySport_Live(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_OtherSportsBySport_Live(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_NumberGameDetails(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hService.ExportToExcel_NumberGameDetails(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion PostMethod
        #endregion IMethods
    }
}