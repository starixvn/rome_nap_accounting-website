namespace Fanex.Tarus.Customer.Controller.InitializingStakeLevel
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.Tarus.Customer.Business.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Requests.InitializingStakeLevel;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class InitStakeLevelController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingStakeLevel/Index.cshtml";

        private readonly IInitializingStakeLevelService service;

        public InitStakeLevelController()
        {
            service = new InitializingStakeLevelService();
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
        public ActionResult GetStakeLevels(GetStakeLevelRequest request)
        {
            return Json(service.GetStakeLevels(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult ExportExcel()
        {
            var excelFile = service.GetExcelFile();
            var fileName = "StakeLevels";
            ExcelExtension.ResponseStreamExcel(excelFile, fileName);

            return new EmptyResult();
        }

        [HttpPost, AuthenticateHost]
        public ActionResult CheckStakeLevels(HttpPostedFileBase uploadFile)
        {
            return Json(service.ValidateSetting(uploadFile));
        }

        [HttpPost, AuthenticateHost]
        public ActionResult InsertStakeLevels(InsertStakeLevelRequest request)
        {
            return Json(service.InsertSetting(request.StakeLevels, AppSession.AccId, HttpContext.Request.UserHostAddress));
        }
    }
}
