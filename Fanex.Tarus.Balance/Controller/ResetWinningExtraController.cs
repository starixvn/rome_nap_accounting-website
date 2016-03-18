namespace Fanex.Tarus.Balance.Controller
{
    using System.Web.Mvc;
    using Fanex.Tarus.Balance.Business;
    using Fanex.Tarus.Balance.Criteria;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class ResetWinningExtraController : BaseController
    {
        private IResetWinningService _resetWinningService;

        public ResetWinningExtraController(IResetWinningService resetWinningService)
        {
            _resetWinningService = resetWinningService ?? new ResetWinningExtraService();
        }

        public ResetWinningExtraController()
            : this(null)
        {
        }

        [AuthenticateRead]
        public ViewResult Index()
        {
            ViewBag.HasPermission = _resetWinningService.CheckPermissionToReset();
            return View("~/Views/Balance/ResetWinningExtraPT/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetResetWinningAfterMoveBetDayReport(GetResetWinningExtraCriteria criteria)
        {
            var result = _resetWinningService.GetResetWinningDataAfterMoveBetDay(criteria);
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetResetWinningBeforeMoveBetDayReport(GetResetWinningBeforeMoveBetDayCriteria criteria)
        {
            var result = _resetWinningService.GetResetWinningDataBeforeMoveBetDay(criteria);
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult ResetWinningByDay(string resetWinningDate)
        {
            GetResetWinningCriteria criteria = new GetResetWinningExtraCriteria
            {
                FromDate = resetWinningDate,
                ToDate = resetWinningDate,
                IsExtraPT = true
            };

            var result = _resetWinningService.ResetWinningByDay(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetResetWinningByDay(string resetWinningDate)
        {
            GetResetWinningCriteria criteria = new GetResetWinningExtraCriteria
            {
                FromDate = resetWinningDate,
                ToDate = resetWinningDate,
                ViewOrResetStatus = 0,
                IsExtraPT = true
            };

            var result = _resetWinningService.GetResetWinningByDay(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}