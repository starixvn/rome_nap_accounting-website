using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Reports.Controllers.Balance
{
    using System.Web.Mvc;

    using Fanex.Tarus.Reports.Business.Balance;
    using Fanex.Tarus.Web.Controllers;
    using ExcelUtil.Spreadsheet;

    public class SuperBalanceReportController:BaseController
    {
        private ISuperBalanceService _superBalanceService;


        public SuperBalanceReportController()
        {
            _superBalanceService = new SuperBalanceService();   
        }

        [HttpPost]
        public JsonResult GetSuperBalanceTotal(string db, bool isYesterday, string usernameSearch, string firstletter, string currency)
        {
            string totalSuperBalance = _superBalanceService.GetSuperBalanceTotal(db, isYesterday, usernameSearch, firstletter, currency);
            var jsonResult = Json(totalSuperBalance);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        //public ActionResult ExportExcelSuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency, string title)
        //{
        //    ExcelFile result = _superBalanceService.ExportExcelSuperBalance(db, isYesterday, usernameSearch, firstletter, currency, title);
        //    int exportResult = 0;
        //    if (result == null)
        //    {
        //        exportResult = 1;
        //    }
        //    var jsonResult = Json(exportResult);
        //    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return jsonResult;
        //}

        public JsonResult ExportExcelSuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency, string title)
        {
            ExcelFile result = _superBalanceService.ExportExcelSuperBalance(db, isYesterday, usernameSearch, firstletter, currency, title);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
