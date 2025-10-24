namespace Fanex.Tarus.BetList.Controller
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.Tarus.BetList.Business;
    using Fanex.Tarus.BetList.Constants;

    public class BetBuilderPlusController : Controller
    {
        private readonly IBetListService _betListService;
        private readonly IResultService _resultService;
        private readonly IBetListDetailService _betListDetailService;

        public BetBuilderPlusController()
        {
            _betListService = new BetListService();
            _resultService = new ResultService();
            _betListDetailService = new BetListDetailService();
        }

        public ActionResult GetBetBuilderPlusDetail(long transId, long refNo, int custId, string transDesc, bool removeSabaWord = false, string language = Language.Default)
        {
            SetCulture(language);
            var result = _betListService.GetBetBuilderPlusTicketsDetailsHtml(transId, refNo, custId, transDesc);

            return Content(result);
        }
        private void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            var defaultCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;
        }
    }
}
