namespace Fanex.Tarus.Finance.Controller.Transfer
{
    using System.Net;
    using System.Web.Mvc;

    using Fanex.Tarus.Finance.Business.Transfer;
    using Fanex.Tarus.Finance.Model.Transfer;
    using Fanex.Tarus.Web.Controllers;

    using Sunplus.Accounting.Core.Common;

    public class CrossTransferController : BaseController
    {
        private readonly ICrossTransferService _crossTransferService;

        public CrossTransferController(ICrossTransferService crossTransferService)
        {
            _crossTransferService = crossTransferService ?? new CrossTransferService();
        }

        public CrossTransferController()
            : this(null)
        {
            _crossTransferService = new CrossTransferService();
        }

        public ActionResult Index()
        {
            var crossTransferModel = new CrossTransfer
            {
                IsHadPermissionOnCrossCurrencyTransfer = Sunplus.Accounting.Business.Security.Permissions.HasPermission("FinanceTransfer"),
                IsHadPermissionOnYesterdayTransfer = Sunplus.Accounting.Business.Security.Permissions.HasPermission("OtherTrans"),
                ForexAccount = Fanex.Tarus.Infrastructure.Caching.CacheHelper.Get<string>("Transfer_ForexAccount_" + AppSession.AccUserName)
            };
            if (!crossTransferModel.IsHadPermissionOnCrossCurrencyTransfer && !crossTransferModel.IsHadPermissionOnYesterdayTransfer)
            {
                return View("~/Views/Common/AccessDenied.cshtml");
            }
            var s = Request.QueryString["fromCustName"];
            return View("~/Views/Finance/Transfer/Index.cshtml", crossTransferModel);
        }

        public JsonResult TransferYesterdayTransactionHandler(string fromCustName, string toCustName, double exRate, double amount, string remark)
        {
            fromCustName = WebUtility.HtmlDecode(fromCustName);
            toCustName = WebUtility.HtmlDecode(toCustName);
            return Json(_crossTransferService.TransferYesterdayTransaction(AppSession.AccId, fromCustName, toCustName, exRate, amount, remark), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectCustomerExRate(string username)
        {
            username = WebUtility.HtmlDecode(username);
            return Json(_crossTransferService.SelectCustomerExRate(username), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CrossCurrencyTransferInsert(FinanceCrossCurrencyTransfer param)
        {
            return Json(_crossTransferService.CrossCurrencyTransferInsert(param), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectAllForexAccount()
        {
            return Json(_crossTransferService.SelectAllForexAccount(), JsonRequestBehavior.AllowGet);
        }
    }
}