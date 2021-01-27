using System;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.BetCreditHistory;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using Sunplus.Accounting.DataAccess.Report;

namespace Fanex.Tarus.Customer.Controller
{
    public class BetCreditHistoryController : BaseController
    {
        private const string basePath = "~/Views/Customer/BetCreditHistory/{0}.cshtml";
        private readonly IBetCreditHistoryService service;

        public BetCreditHistoryController()
           : this(null)
        {
        }

        internal BetCreditHistoryController(IBetCreditHistoryService service)
        {
            this.service = service ?? new BetCreditHistoryService();
        }

        [AuthenticateRead]
        public ActionResult Index()
            => View(string.Format(basePath, nameof(Index)), ReportUtils.BalanceUp.BalanceUpDay);

        [AuthenticateRead]
        public ActionResult Search(string member, DateTime fromDate, DateTime toDate)
            => PartialView(string.Format(basePath, nameof(Search)), service.GetBetCreditHistory(member, fromDate, toDate));
    }
}
