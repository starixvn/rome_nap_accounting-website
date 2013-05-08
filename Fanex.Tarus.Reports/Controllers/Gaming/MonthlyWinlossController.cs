// -----------------------------------------------------------------------
// <copyright file="MonthlyWinloss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Web.Controllers;
    using System.Web.Mvc;
using Fanex.Tarus.Reports.Business.Gaming;
    using Fanex.Tarus.Reports.Entities.Gaming;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyWinlossController: BaseController
    {
        public readonly IBingoMonthlyWinlossService _bingoMonthlyWLService;

        public MonthlyWinlossController()
            : this(null)
        { }

        public MonthlyWinlossController(IBingoMonthlyWinlossService bingoMonthlyWLService)
        {
            this._bingoMonthlyWLService = bingoMonthlyWLService ?? new BingoMonthlyWinlossService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Report/Gaming/BingoMonthlyWinloss.cshtml");
        }

      
        public JsonResult GetBingoWLByMonth(string month, string year)
        {
            var bingoMonthlyWL = new BingoMonthlyWL();
            bingoMonthlyWL = _bingoMonthlyWLService.GetDataBingoMonthlyWL(month, year);
            var jsonResult = Json(bingoMonthlyWL);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
            
        }

        public ActionResult ExportExcelBingoMonthlyWL(string month, string year)
        {

            ExcelFile result = _bingoMonthlyWLService.ExportExcelBingoMonthly(month, year);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


    }
}
