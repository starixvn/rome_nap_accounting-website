using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Tarus.Reports.Business.PunterAnalysis;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;
using Fanex.Tarus.Reports.Entities.Sport;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Web.Controllers.Report
{
    public class PunterAnalysisController : BaseController
    {
        //
        // GET: /PunterAnalysis/

        IPunterAnalysisService _punterAnalysisService = null;

        public PunterAnalysisController() : this(null) { }
        public PunterAnalysisController(IPunterAnalysisService punterAnalysisService)
        {
            _punterAnalysisService = punterAnalysisService ?? new PunterAnalysisService();
        }

        [AuthenticateRead]
        public ActionResult PunterAnalysisReport()
        {
            PunterAnalysisTotalDTO model = new PunterAnalysisTotalDTO();
            model.Punters = new List<PunterAnalysisBO>();
            model.Total = new PunterAnalysisBO();

            return View("~/Views/Report/PunterAnalysis/PunterAnalysisReport.cshtml", model);
        }

        #region Post method

        [HttpPost]
        public JsonResult SelBettypeBySportId(int SportId)
        {
            var result = _punterAnalysisService.SelBettypeBySportId(SportId);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelSportsGroup(int byleague, string products)
        {
            PunterAnalysisGroupSportsDTO result;
            result = _punterAnalysisService.SelSportsGroup();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult SelLeagueBySportId(PunterAnalysisParam param)
        {            
            var result = _punterAnalysisService.SelLeagueBySportId(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterSummary(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO result = _punterAnalysisService.SelTopPunterSummary(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterSummaryByLeague(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO result = _punterAnalysisService.SelTopPunterSummaryByLeague(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterByCustID(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO result = _punterAnalysisService.SelTopPunterByCustID(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterBySporttype(PunterAnalysisParam param)
        {
            PunterAnalysisGrandTotalDTO result = _punterAnalysisService.SelTopPunterBySporttype(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterByLeagueCustID(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO result = _punterAnalysisService.SelTopPunterByLeagueCustID(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTopPunterByBettype(PunterAnalysisParam param)
        {
            PunterAnalysisGrandTotalDTO result = _punterAnalysisService.SelTopPunterByBettype(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;        
        }

        [HttpPost]
        public JsonResult SelTopPunterBySporttypeSum(PunterAnalysisParam param)
        { 
            PunterAnalysisTotalDTO result = _punterAnalysisService.SelTopPunterBySporttypeSum(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelIPByCustomer(PunterAnalysisParam param)
        {
            List<PunterAnalysisIpBO> result = _punterAnalysisService.SelIPByCustomer(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelBetCountByCustomer(PunterAnalysisParam param)
        {
            PunterAnalysisBetCountDTO result = _punterAnalysisService.SelBetCountByCustomer(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelBetCountByCustomer_Detail(PunterAnalysisParam param)
        {
            PunterAnalysisBetCountDTO result = _punterAnalysisService.SelBetCountByCustomer_Detail(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterSummary(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterSummary(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterSummaryByLeague(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterSummaryByLeague(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterByCustID(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterByCustID(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        //[HttpPost, AuthenticateRead]
        //public ActionResult ExportExcelTopPunterByLeagueCustID(PunterAnalysisParam param)
        //{
        //    _punterAnalysisService.ExportExcelTopPunterByLeagueCustID(param);

        //    int exportResult = 1;
        //    var jsonResult = Json(exportResult);
        //    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return jsonResult;
        //}

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterBySporttypeSum(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterBySporttypeSum(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterBySporttype(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterBySporttype(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost, AuthenticateRead]
        public ActionResult ExportExcelTopPunterByBettype(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelTopPunterByBettype(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        //[HttpPost, AuthenticateRead]
        public ActionResult ExportExcelIPByCustId_TopPunterSummary(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelIPByCustId_TopPunterSummary(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        //[HttpPost, AuthenticateRead]
        public ActionResult ExportExcelBetCountByCustId_TopPunterSummary(PunterAnalysisParam param)
        {
            _punterAnalysisService.ExportExcelBetCountByCustId_TopPunterSummary(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult SelProduct()
        {
            var products = _punterAnalysisService.SelProduct();
            var jsonResult = Json(products); 
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion
    }
}
