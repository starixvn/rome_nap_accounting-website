namespace Fanex.Tarus.BetList.Controller
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Business;
    using Constants;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.ViewModel;
    using Fanex.Tarus.Security.Authenticate;
    using SmartPunterFilter = AuthenticationFilter.AuthenticateSmartPunterAction;

    public class CN88BetListController : Controller
    {
        private readonly ICN88BetListService _cn88BetListService;

        public CN88BetListController()
        {
            _cn88BetListService = new CN88BetListService();
        }

        [AuthorizeFunctions("ViewInfo")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListByCustInfo_DateRange(int custId, DateTime FromDate, DateTime ToDate, int Bettype, int PageSize, int PageIndex, string language = Language.Default)
        {
            SetCulture(language);
            PageIndex = PageIndex <= 0 ? 1 : PageIndex;
            var result = _cn88BetListService.GetBetListByCustInfo_DateRange(custId, FromDate, ToDate, Bettype, PageSize, PageIndex);

            return PartialView("~/Views/CN88BetList/_BetList.cshtml", new CN88BetListViewModel(custId, language, result));
        }

        [AuthorizeFunctions("ViewInfo")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListByCustInfo_Refno(string transactionId, int customerId, string language = Language.Default)
        {
            SetCulture(language);
            var result = _cn88BetListService.GetBetListByCustInfo_Refno(transactionId, customerId);

            return PartialView("~/Views/CN88BetList/_BetList.cshtml", new CN88BetListViewModel(customerId, language, result));
        }

        public ActionResult GetMixParlayDetail(long transId, long refNo, decimal? hdp1, decimal? hdp2, string betCheck, string betTeam, int custId, string language = Language.Default)
        {
            SetCulture(language);
            var tickets = _cn88BetListService.GetMixParlayTickets(
                new MixParlayDetailCriteria
                {
                    TransId = transId,
                    RefNo = refNo,
                    CustId = custId,
                    IsCN88 = true
                });

            ViewBag.CombinationTypeComboNames = string.IsNullOrEmpty(betCheck) ?
                string.Empty :
                _cn88BetListService.GetCombinationTypeComboNames(betTeam, betCheck, hdp1.Value, hdp2.Value, tickets);

            return PartialView(
                "~/Areas/CN88BetList/Views/MemberBetListReport/_MixParlayDetail.cshtml",
                new CN88MixParlayDetailViewModel(tickets));
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
