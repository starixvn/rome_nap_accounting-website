// -----------------------------------------------------------------------
// <copyright file="NumberGameWinLossByBettypeController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.NumberGameWinloss
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Reports.Business.NumberGame;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss;
    using Fanex.Tarus.Reports.Business.NumberGameWinLoss.NumberGameWinLossByBettype;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameWinLossByBettypeController : BaseController
    {
        INumberGameWinLossService _numberGameWinLossService = null;
        INumberGameWinLossByBettypeService _numberGameWinLossByBettypeService = null;

        public NumberGameWinLossByBettypeController(): this(null, null) { }
        public NumberGameWinLossByBettypeController(INumberGameWinLossService numberGameWinLossService, INumberGameWinLossByBettypeService numberGameWinLossByBettypeService) 
        {
            _numberGameWinLossService = numberGameWinLossService ?? new NumberGameWinLossService();
            _numberGameWinLossByBettypeService = numberGameWinLossByBettypeService ?? new NumberGameWinLossByBettypeService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Report/NumberGame/NumberGameWinLossByBettype/NumberGameWinLossByBettype.cshtml");
        }

        [HttpPost]
        public JsonResult GetLicensees()
        {
            List<OptionItem> result;
            result = _numberGameWinLossService.GetLicensees();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Getcurrencies()
        {
            List<OptionItem> result;
            result = _numberGameWinLossService.GetCurrency();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelNumberGameWinLossByBettype(string wlDate, string currencies, string companies, int liveStatus)
        {
            var result = _numberGameWinLossByBettypeService.SelNumberGameWinLosByBettype(wlDate, currencies, companies, liveStatus);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelNumberGameWinLossByBettypeDetail(string wlDate, string currencies, string companies, int liveStatus, int bettype)
        {
            var result = _numberGameWinLossByBettypeService.SelNumberGameWinLosByBettypeDetail(wlDate, currencies, companies, liveStatus, bettype);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
