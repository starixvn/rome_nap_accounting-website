namespace Fanex.Tarus.Customer.Controller.InitializingBetLimit
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.Tarus.Customer.Business.InitializingBetLimit;
    using Fanex.Tarus.Customer.Requests.InitializingBetLimit;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class InitBetLimitController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingBetLimit/Index.cshtml";

        private readonly IInitializingBetLimitService service;

        public InitBetLimitController()
        {
            service = new InitializingBetLimitService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetFilterData()
        {
            var filterData = service.GetFilterData();

            return Json(filterData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetSportTypes(int productId)
        {
            var sportTypes = service.GetSportTypes(productId);

            return Json(sportTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult GetBetTypes(GetBettypeRequest request)
        {
            var betTypes = service.GetBetTypes(request.ProductID, request.SportTypes);

            return Json(betTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult GetBetLimits(GetBetLimitRequest request)
        {
            return Json(service.GetBetLimits(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ExportExcel()
        {
            var excelFile = service.GetExcelFile();
            var fileName = "GlobalBetLimit";
            ExcelExtension.ResponseStreamExcel(excelFile, fileName);

            return new EmptyResult();
        }

        [HttpPost, AuthenticateHost]
        public ActionResult CheckBetLimits(HttpPostedFileBase uploadFile)
        {
            return Json(service.ValidateSetting(uploadFile));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult InsertBetLimits(InsertBetLimitRequest request)
        {
            return Json(service.InsertSetting(request.BetLimits, AppSession.AccId, HttpContext.Request.UserHostAddress));
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetAllCurrency()
        {
            return Json(service.GetAllCurrency(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthenticateRead]
        public ActionResult GetProductCurrencyMatrix(GetProductCurrencyRequest request)
        {
            return Json(service.GetProductCurrencyMatrix(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ExportProductCurrencyExcel()
        {
            var excelFile = service.GetProductCurrencyExcelFile();
            var fileName = "Product Currency Matrix";
            ExcelExtension.ResponseStreamExcel(excelFile, fileName);

            return new EmptyResult();
        }
    }
}
