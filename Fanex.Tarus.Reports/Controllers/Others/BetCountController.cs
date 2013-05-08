// -----------------------------------------------------------------------
// <copyright file="BetCountController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.Others
{
    using System.Web.Mvc;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Reports.Business.Others;
    using Fanex.Tarus.Reports.Entities.Others;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetCountController : Controller
    {
        private readonly IBetCountService _betCountService;
        private readonly string _sessionKeyForBetCountReport = "_sessionKeyForBetCountReport";
        public BetCountController()
            : this(null)
        {

        }

        public BetCountController(IBetCountService betCountService)
        {
            _betCountService = betCountService ?? new BetCountService();
        }

        [AuthenticateRead]
        public ActionResult BetCountPage()
        {
            return View("~/Views/Report/Others/BetCount.cshtml");
        }

        [HttpPost]
        public JsonResult GetAllProducts()
        {
            var products = _betCountService.GetAllProducts();
            return Json(products);
        }
        
        [HttpPost]
        public JsonResult GetBetCountWithDateRange(string fromDate, string toDate, string products)
        {
            BetCount betcount = _betCountService.GetBetCountWithDateRange(fromDate, toDate, products);
            HttpContext.Session[_sessionKeyForBetCountReport] = betcount;
            return Json(betcount);
        }

        [HttpPost]
        public JsonResult SortingBetCountReport(string fromDate, string toDate, string products, string sortCol, string sortType)
        {
            BetCount betCount = HttpContext.Session[_sessionKeyForBetCountReport] as BetCount;
            var result = _betCountService.SortingBetCountReport(fromDate, toDate, products, sortCol, sortType, betCount);
            HttpContext.Session[_sessionKeyForBetCountReport] = result;
            return Json(result);
        }

        public ActionResult ExportBetCountToExcelFile(string fromDate, string toDate, string products)
        {
            BetCount betCount = HttpContext.Session[_sessionKeyForBetCountReport] as BetCount;
            ExcelFile betCountExcelFile = _betCountService.ExportBetCountWithExcelWithDateRange(fromDate, toDate, products, betCount);
            if (betCountExcelFile != null)
            {
                var response = HttpContext.Response;
                var fileName = string.Format("BetCountReport-From-{0}-To-{1}.xls", fromDate, toDate);
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "application/vnd.xls";
                betCountExcelFile.SaveXls(response.OutputStream);
                response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                response.End();
            }
            return new EmptyResult();
        }                
    }
}