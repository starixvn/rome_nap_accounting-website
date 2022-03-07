namespace Fanex.Tarus.Customer.Controller.InitializingStakeLevel
{
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;
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

        [HttpGet, AuthenticateRead]
        public ActionResult GetStakeLevels()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult CheckStakeLevels()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AuthenticateRead]
        public ActionResult InsertStakeLevels()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
