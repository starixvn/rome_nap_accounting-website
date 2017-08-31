namespace Fanex.Tarus.BetList.Controller
{
    using System.Web.Mvc;
    using BetListDetail.Lucky3.Criteria;
    using BetListDetail.Lucky3.Services;
    using BetListDetail.Lucky3.ViewModels;
    using DataAccess;

    public class Lucky3Controller : Controller
    {
        private readonly ILucky3Service lucky3Service;

        public Lucky3Controller()
            : this(null)
        {
        }

        public Lucky3Controller(ILucky3Service lucky3Service)
        {
            this.lucky3Service = lucky3Service?? new Lucky3Service(new Lucky3Repository());
        }

        [HttpGet]
        public ActionResult ViewDetail(GetLucky3DetailParameter criteria)
        {
            var result = lucky3Service.GetBetListDetail(criteria);

            return View(
                "~/Views/BetListDetails/Lucky3/Index.cshtml",
                new Lucky3DetailViewModel(result, 4));
        }        
    }
}
