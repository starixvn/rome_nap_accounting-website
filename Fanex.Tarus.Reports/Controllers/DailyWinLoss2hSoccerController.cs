// --------------------------------------------------------------------------
// <copyright file="DailyWinLoss2hSoccerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Controllers
{
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Business.DailyWinLoss2hSoccer;
    using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class DailyWinLoss2hSoccerController : BaseController
    {
        #region IFields

        //
        // GET: /WinLoss2hService/
        private readonly IDailyWinLoss2hSoccerService _dailyWinLoss2hSoccerService;

        #endregion IFields
        #region IConstructions

        public DailyWinLoss2hSoccerController()
            : this(null)
        {
        }

        public DailyWinLoss2hSoccerController(IDailyWinLoss2hSoccerService dailyWinLoss2hSoccerService)
        {
            _dailyWinLoss2hSoccerService = dailyWinLoss2hSoccerService ?? new DailyWinLoss2hSoccerService();
        }

        #endregion IConstructions
        #region IMethods

        [AuthenticateRead]
        public ActionResult DailyWinLoss2hSoccer()
        {
            return View("~/Views/Report/DailyWinLoss2hSoccer/DailyWinLoss2hSoccer.cshtml");
        }
    
        #region PostMethod

        [HttpPost]
        public JsonResult DailyWLDirect_Sporttype(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hSoccerService.DailyWLDirect_Sporttype(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    
        [HttpPost]
        public ActionResult ExportToExcel(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hSoccerService.ExportToExcel(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }        
     
        [HttpPost]
        public JsonResult SelSoccerReportMain(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hSoccerService.SelSoccerReportMain(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelDailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            var result = _dailyWinLoss2hSoccerService.SelDailyWLDirectDetails(param);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_SoccerMain(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hSoccerService.ExportToExcel_SoccerMain(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel_DailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            _dailyWinLoss2hSoccerService.ExportToExcel_DailyWLDirectDetails(param);

            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

     
        #endregion PostMethod
        #endregion IMethods
    }
}