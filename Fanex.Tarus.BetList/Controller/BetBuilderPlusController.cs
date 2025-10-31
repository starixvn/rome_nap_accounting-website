namespace Fanex.Tarus.BetList.Controller
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Param;

    public class BetBuilderPlusController : Controller
    {
        private readonly IBetListService _betListService;

        public BetBuilderPlusController()
        {
            _betListService = new BetListService();
        }

        [HttpPost]
        public ActionResult GetBetBuilderPlusDetail(BetBuilderPlusDetailParam betBuilderPlusDetailParam)
        {
            SetCulture(betBuilderPlusDetailParam.Language);
            var result = _betListService.GetBetBuilderPlusTicketsDetailsHtml(betBuilderPlusDetailParam);

            return Content(result);
        }
        private static void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            var defaultCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;
        }
    }
}
