

namespace Fanex.Tarus.Reports.Controllers.Gaming
{
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Business.Gaming.NumberGame;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class NumberGameMonthlyWinLossController: BaseController
    {
        protected INumberGameMonthlyWinLossService _ngMonthlyWinLossService = null;
        public NumberGameMonthlyWinLossController(): this(null)
        {

        }
        public NumberGameMonthlyWinLossController(INumberGameMonthlyWinLossService ngMonthlyWinLossService)
        {
            _ngMonthlyWinLossService = ngMonthlyWinLossService ?? new NumberGameMonthlyWinLossService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Report/Gaming/NumberGameFinancialReport/Index.cshtml");
        }

        public ActionResult WinLossPage(string fromDate, string toDate)
        {
            var result = _ngMonthlyWinLossService.GetNumberGameMonthlyWinLossReport(fromDate, toDate);
            return PartialView("~/Views/Report/Gaming/NumberGameFinancialReport/WinLossPage.cshtml", result);
        }

        public ActionResult ExportToExcelNumberGameMonthlyWinLoss(string fromDate, string toDate, string reportTitle)
        {
            _ngMonthlyWinLossService.ExportToExcelNumberGameMonthlyWinLossReport(fromDate, toDate, reportTitle);
            return new EmptyResult();
        }
    }

}
