using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Entities.Sport;
using Fanex.Tarus.VirtualSport.Business.Report;
using Fanex.Tarus.VirtualSport.Business.Report.WinLoss;
using Fanex.Tarus.VirtualSport.Entities.Report.Winloss;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.VirtualSport.Controllers.Report.Controllers
{
    public class VirtualSportReportController : BaseController
    {
        public readonly IWinlossService _winlossService;
        public VirtualSportReportController()
            : this(null)
        { }

        public VirtualSportReportController(IWinlossService winlossService)
        {
            this._winlossService = winlossService ?? new WinlossService();
        }

       [AuthenticateRead]
        public ActionResult WinLossReport()
        {
            return View("~/Views/VirtualSport/Report/Winloss/WinLoss.cshtml");
        }

        [HttpPost]
        public JsonResult GetSportType()
        {
            var sportType = new List<BaseModel>();
            sportType = _winlossService.SelSportType();
            var jsonResult = Json(sportType);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
            
        }
        [HttpPost]
        public JsonResult GetDailyWinloss(string fromDate, string toDate, bool excludeTest, int drawFilter, string sportType)
        {
            var result = new Winloss();
            result = _winlossService.SelDailyWinloss(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), excludeTest, drawFilter, sportType);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetWinlossDetailByCurrency(string date,  bool excludeTest, int drawFilter, string sportType, string currency)
        {
            var result = new WinlossByCurrency();
            result = _winlossService.SelDailyWinlossByCurrency(CastAide.AsDateTime(date), excludeTest, drawFilter, sportType, currency);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult ExportExcelWinlossDaily(DateTime fromDate, DateTime toDate, bool isExcludeTest, int drawFilter, string sportType)
        {

            ExcelFile result = _winlossService.ExportExcelWinlossDaily(fromDate, toDate, isExcludeTest, drawFilter, sportType);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult ExportExcelWinlossDetailByCurr(DateTime reportDate, bool isExcludeTest, int drawFilter, string sportType, string currency)
        {

            ExcelFile result = _winlossService.ExportExcelWinlossDetailByCurrency(reportDate, isExcludeTest, drawFilter, sportType, currency);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}