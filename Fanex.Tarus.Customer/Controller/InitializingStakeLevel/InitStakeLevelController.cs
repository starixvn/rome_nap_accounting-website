namespace Fanex.Tarus.Customer.Controller.InitializingStakeLevel
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.InitializingCustomerSetting;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class InitStakeLevelController : BaseController
    {
        private readonly string _indexViewPath = "~/Views/ITConfiguration/InitializingStakeLevel/Index.cshtml";

        private readonly IInitializingCustomerSettingService initCustSettingService;

        public InitStakeLevelController()
        {
            initCustSettingService = new InitializingCustomerSettingService();
        }

        public ActionResult Index()
        {
            return View(_indexViewPath);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetProductMatrix()
        {
            var filterData = initCustSettingService.GetProductSportBettype();

            return Json(filterData.ProductModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult GetSetting()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
