
namespace Fanex.Tarus.Web.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Entities.Sport;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Reports.Business.NumberGame;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss;
    using Fanex.Tarus.Reports.Business.NumberGame.ByCurrency;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss.NumberGameWinLossByCurrency;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameWinLossByCurrencyController : BaseController
    {
         INumberGameWinLossService _numberGameWinLossService= null;
         IByCurrencyService _byCurrencyService = null;
         

        public NumberGameWinLossByCurrencyController() : this(null, null) { }
        public NumberGameWinLossByCurrencyController(INumberGameWinLossService numberGameWinLossService, IByCurrencyService byCurrencyService)
        {
            _numberGameWinLossService = numberGameWinLossService ?? new NumberGameWinLossService();
            _byCurrencyService = byCurrencyService ?? new ByCurrencyService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Report/NumberGame/NumberGameWinLossByCurrency/NumberGameWinLossByCurrency.cshtml");
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
        public JsonResult GetBettypes()
        {
            List<OptionItem> result;
            result = _numberGameWinLossService.GetBettypes();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetNumberGameWinLossByCurMain(NumberGameWinLossByCurrency_MainParams param)
        {
            NumberGameWinLossByCurrency_MainBO result;
            result = _byCurrencyService.GetNumberGameMainData(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult GetNumberGameWinLossByCurDetail(NumberGameWinLossByCurrency_DetailParams param)
        {
            NumberGameWinLossByCurrency_DetailBO result;
            result = _byCurrencyService.GetNumberGameDetailData(param);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
