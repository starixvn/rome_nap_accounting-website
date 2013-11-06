namespace Fanex.Tarus.Finance.Controller.Statement
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Tarus.Finance.Business.Statement;
    using Fanex.Tarus.Finance.Entities;
    using Fanex.Tarus.Finance.Model.Statement;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Sunplus.Accounting.Core.Common;

    public class CustomerStatementController : BaseController
    {
        private IFinanceStatementBusiness _financeStatemenetService = null;

        private Helper _helper;

        public CustomerStatementController()
            : this(null)
        {
        }

        public CustomerStatementController(IFinanceStatementBusiness financeStatemenetServices)
        {
            _financeStatemenetService = financeStatemenetServices ?? new FinanceStatementBusiness();
            _helper = new Helper();
        }

        [AuthenticateRead]
        public ActionResult Index(int? CustID, string CustName)
        {
            ViewBag.Banlance = _helper.BalanceUpDate;
            ViewBag.CustID = CustID;
            ViewBag.CustName = System.Web.HttpUtility.UrlDecode(CustName);
            return View("~/Views/Finance/Statement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetFinanceStatement(string refNo, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int includeAccBall)
        {
            if (!_helper.CheckIsInternal(custName) && (custName != string.Empty))
            {
                return new JsonResult { Data = new { errorMessage = "This function supports to view statement of Finance Accounts only!" } };
            }

            balanceUpDay = _helper.BalanceUpDate.ToShortDateString();

            List<StatementDetail> result;
            if (CastAide.AsDateTime(fromdate) != CastAide.AsDateTime(balanceUpDay) && refNo == "-1")
            {
                includeAccBall = 1;
            }
            result = _financeStatemenetService.GetFinanceStatementBusiness(refNo, custName, filterBy, fromdate, toDay, balanceUpDay, includeAccBall);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFinanceStatementDetail(StatementDetail statementDetail, string username)
        {
            StatementDetail result;
            if (statementDetail.CheckerName == null)
            {
                statementDetail.CheckerName = string.Empty;
            }

            if (statementDetail.CheckerTime == null)
            {
                statementDetail.CheckerTime = string.Empty;
            }

            result = _financeStatemenetService.GetFinanceStatementDetail(statementDetail, username);

            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateVerifyTrans(string refNo, bool isVerify)
        {
            Result result;
            result = _financeStatemenetService.VerifyTransactionBusiness(refNo, isVerify, AppSession.AccUserName, AppSession.AccId);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DeleteTrans(string refNo, bool isVerify)
        {
            Result result;
            result = _financeStatemenetService.DeleteTransBusiness(refNo, AppSession.CustID, AppSession.AccId);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public void ExportExcelFinanceStatement(string refNo, string custName, string filterBy, string fromdate, string toDay, string balanceUpDay, int IncludeAccBall, string reportTitle)
        {
            _financeStatemenetService.ExportExcelFinanceStatement(refNo, custName, filterBy, fromdate, toDay, balanceUpDay, IncludeAccBall, reportTitle);
        }
    }
}