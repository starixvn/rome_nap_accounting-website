using Fanex.Tarus.Security.Authenticate;
using Fanex.Tarus.Security.Domain;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.BetList.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Business;
    using Core;
    using Fanex.Common;
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using Security.Services;
    using SmartPunterFilter = AuthenticationFilter.AuthenticateSmartPunterAction;

    public class BetListController : Controller
    {
        private readonly IBetListService _betListService;
        private readonly IResultService _resultService;
        private readonly IBetListDetailService _betListDetailService;

        public BetListController()
        {
            this._betListService = new BetListService();
            this._resultService = new ResultService();
            _betListDetailService = new BetListDetailService();
        }

        public ActionResult GetMixParlayDetail(long transId, long refNo, decimal? hdp1, decimal? hdp2, string betCheck, string betTeam)

        {
            var result = _betListService.GetMixParlayTickets(
                new MixParlayDetailCriteria
                {
                    TransId = transId,
                    RefNo = refNo
                });

            ViewBag.CombinationTypeComboNames = string.IsNullOrEmpty(betCheck) ?
                string.Empty :
                _betListService.GetCombinationTypeComboNames(betTeam, betCheck, hdp1.Value, hdp2.Value, result);

            return PartialView("~/Areas/BetList/Views/MemberBetListReport/_MixParlayDetail.cshtml", result);
        }

        /// <summary>
        /// Gets the bet list by reference no.
        /// </summary>
        /// <param name="refno">The ref no.</param>
        /// <returns>The bet list by reference no.</returns>
        [AuthorizeFunctions("BetListByIP")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read, SmartPunterActionId.ViewBetListByRefNo)]
        public ActionResult GetBetListByRefNo(string refno)
        {
            IList<TicketDetail> result = new List<TicketDetail>();
            var siteType = CreditOrDepositCustomerUtils.GetSiteTypeParamValue(AppSession.AccId);

            if (!CreditOrDepositCustomerUtils.IsNoneCreditAndDepositPermission(siteType))
            {
                BalanceUp balanceUp = ReportService.BalanceUp;

                if (BetListHelper.IsNumeric(refno.Trim()))
                {
                    result = _betListService.GetBetListByRefNo(new BetListByRefNoCriteria
                    {
                        UserId = AppSession.AccId,
                        RefNo = Converter.To<long>(refno),
                        MoveBetsDay = balanceUp.MoveBetsDay,
                        BalanceUpDay = balanceUp.BalanceUpDay,
                        SiteType = siteType
                    });
                }
            }

            Hashtable paramHastable = new Hashtable();
            int custId = result.Count == 0 ? 0 : Converter.To<int>(result[0].Ticket.CustId);
            paramHastable.Add("custId", custId);

            TempData["paramHastable"] = paramHastable;

            return PartialView("~/Views/BetList/BetListByRefNo/_BetListByRefNo.cshtml", result);
        }

        /// <summary>
        /// Gets the bet list by reference no.
        /// </summary>
        /// <param name="betListByProductCriteria">The criteria.</param>
        /// <returns>The bet list by reference no.</returns>
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read, SmartPunterActionId.ViewBetListByRefNo)]
        public ActionResult GetBetListByProduct(BetListByProductParam betListByProductCriteria)
        {
            IList<TicketDetail> result = new List<TicketDetail>();
            var siteType = CreditOrDepositCustomerUtils.GetSiteTypeParamValue(AppSession.AccId);

            if (!CreditOrDepositCustomerUtils.IsNoneCreditAndDepositPermission(siteType))
            {
                BalanceUp balanceUp = ReportService.BalanceUp;
                betListByProductCriteria.UserId = AppSession.AccId;
                betListByProductCriteria.MoveBetsDay = balanceUp.MoveBetsDay;
                betListByProductCriteria.BalanceUpDay = balanceUp.BalanceUpDay;
                betListByProductCriteria.SiteType = siteType;

                result = _betListService.GetBetListByProduct(betListByProductCriteria);
            }

            Hashtable paramHastable = new Hashtable();
            int custId = result.Count == 0 ? 0 : Converter.To<int>(result[0].Ticket.CustId);
            paramHastable.Add("custId", custId);

            TempData["paramHastable"] = paramHastable;

            return PartialView("~/Views/BetList/BetListByRefNo/_BetListByRefNo.cshtml", result);
        }

        /// <summary>
        /// Gets the bet types.
        /// </summary>
        /// <param name="betTypeByDateCriteria">The bet type by date criteria.</param>
        /// <returns>Bet types after change date range.</returns>
        [HttpPost]
        public JsonResult GetBetTypes(BetTypeByDateCriteria betTypeByDateCriteria)
        {
            var result = _betListService.GetBetTypeByDate(betTypeByDateCriteria);
            return Json(result);
        }

        /// <summary>
        /// Gets the bet list by customer info ref no.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The bet list by customer info ref no.</returns>
        [AuthorizeFunctions("ViewInfo")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListByCustInfo_Refno(string transactionId, int customerId)
        {
            IList<TicketDetail> result = new List<TicketDetail>();
            if (BetListHelper.IsNumeric(transactionId.Trim()))
            {
                result = _betListService.GetBetListByCustInfo_Refno(transactionId, customerId);
            }

            Hashtable paramHastable = new Hashtable();
            paramHastable.Add("custId", customerId);

            TempData["paramHastable"] = paramHastable;

            ViewBag.Quantity = 1;
            ViewBag.NumberBegining = 1;
            return PartialView("~/Views/BetList/BetlistByCustInfo/Betlist/_BetList.cshtml", result);
        }

        /// <summary>
        /// Render Partial for SystemParlay Tab - Customer Info.
        /// </summary>
        /// <param name="transactionId">Transaction ID.</param>
        /// <param name="customerId">Customer ID.</param>
        /// <returns>Partial for SystemParlay Tab - Customer Info.</returns>
        [AuthorizeFunctions("ViewInfo")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListByCustInfo_SystemParLayTab(string transactionId, int customerId)
        {
            SystemParlayModel result = new SystemParlayModel();
            if (BetListHelper.IsNumeric(transactionId.Trim()))
            {
                result = _betListService.GetBetListByCustInfo_SystemParLayTab(transactionId, customerId);
            }

            Hashtable paramHastable = new Hashtable();
            paramHastable.Add("custId", customerId);

            TempData["paramHastable"] = paramHastable;
            ViewBag.Quantity = 1;
            ViewBag.NumberBegining = 1;
            return PartialView("~/Views/BetList/BetlistByCustInfo/SystemParlay/_SystemParlay.cshtml", result);
        }

        public ActionResult ExportBetListByCustInfo_SystemParLayTab(BetListSystemParlayByRefnoCriteria criteria)
        {
            ISystemParlayExcel systemParlayExcel = new SystemParlayExcel();

            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment; filename=Combination_Details.xls");

            systemParlayExcel.ExportCombinationDetails(criteria, response.OutputStream);

            return new EmptyResult();
        }

        public ActionResult ExportBetList_SystemParlayDetails(BetListSystemParlayDetailsCriteria criteria)
        {
            ISystemParlayExcel systemParlayExcel = new SystemParlayExcel();

            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment; filename=SystemParlay_Details.xls");

            systemParlayExcel.ExportSystemParlayDetails(criteria, response.OutputStream);

            return new EmptyResult();
        }

        [AuthorizeFunctions("BetListByIP")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListByIP_Username(
            string ip,
            DateTime fromDate,
            int pageSize,
            int pageIndex)
        {
            var quantity = 0;
            IList<BetListByIP_Username> result = new List<BetListByIP_Username>();
            var siteType = CreditOrDepositCustomerUtils.GetSiteTypeParamValue(AppSession.AccId);

            if (!CreditOrDepositCustomerUtils.IsNoneCreditAndDepositPermission(siteType))
            {
                result = _betListService.GetListByIP_Username(ip, fromDate, pageSize, pageIndex, siteType, ref quantity);
            }
            
            var dateTmp = fromDate.ToShortDateString();
            ViewBag.NumberBegining = (pageSize * (pageIndex - 1)) + 1;
            ViewBag.Quantity = quantity;
            ViewBag.FromDate = dateTmp;
            ViewBag.IP = ip;
            return PartialView("~/Views/BetList/BetListByIP/_UsernameByIP.cshtml", result);
        }

        /// <summary>
        /// Gets the bet list by IP soccer.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="isSoccer">If set to <c>true</c> [is soccer].</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns>The bet list by IP soccer.</returns>
        [AuthorizeFunctions("BetListByIP")]
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read, SmartPunterActionId.ViewBetListByIp)]
        public ActionResult GetBetListByIP_Soccer(
            int customerId,
            string ip,
            DateTime fromDate,
            bool isSoccer,
            int pageSize,
            int pageIndex)
        {
            Hashtable paramHastable = new Hashtable();
            paramHastable.Add("custId", customerId);
            TempData["paramHastable"] = paramHastable;

            decimal totalComm = 0;
            decimal totalWinLost = 0;
            int quantity = 0;
            var result = _betListService.GetBetListByIP_Soccer(
                customerId,
                ip,
                fromDate,
                isSoccer,
                pageSize,
                pageIndex,
                ref quantity,
                ref totalComm,
                ref totalWinLost);
            ViewBag.Quantity = quantity;
            ViewBag.TotalComm = totalComm;
            ViewBag.NumberBegining = (pageSize * (pageIndex - 1)) + 1;
            ViewBag.TotalWinLos = totalWinLost;
            return PartialView("~/Views/BetList/BetListByIP/_BetListByIP.cshtml", result);
        }

        /// <summary>
        /// Gets the bet list statement.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="transType">Type of the trans.</param>
        /// <param name="fromDate">From date.</param>
        /// <returns>The bet list statement.</returns>
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read, SmartPunterActionId.ViewBetListFromStatement)]
        public ActionResult GetBetListStatement(string username, int transType, DateTime fromDate)
        {
            Hashtable paramHastable = new Hashtable();
            paramHastable.Add("username", username);
            TempData["paramHastable"] = paramHastable;

            decimal totalComm = 0;
            decimal totalWinLost = 0;
            int quantity = 0;
            int customerId = _betListService.GetCustIDByUsername(username);
            var result = _betListService.GetBetListStatement(customerId, transType, fromDate, ref quantity, ref totalComm, ref totalWinLost);
            ViewBag.Quantity = quantity;
            ViewBag.TotalComm = totalComm;
            ViewBag.NumberBegining = 1;
            ViewBag.TotalWinLos = totalWinLost;

            ViewBag.CustID = customerId;
            ViewBag.TransType = transType;
            ViewBag.FromDate = fromDate.ToShortDateString();
            ViewBag.CustName = username;

            return PartialView("~/Views/BetList/BetListStatement/_BetListStatement.cshtml", result);
        }


        /// <summary>
        /// Gets the bet list by reference no.
        /// </summary>
        /// <param name="refno">The ref no.</param>
        /// <returns>The bet list by reference no.</returns>
        [SmartPunterFilter(SmartPunterFilter.AuthenticationType.Read)]
        public ActionResult GetBetListBetTradeDetails(long transId, long refNo)
        {
            var result = _betListService.GetBetListBetTradeDetails(new BetListBetTradeDetailCriteria
            {
                TransId = transId,
                RefNo = refNo
            });

            Hashtable paramHastable = new Hashtable();
            int custId = result.Count == 0 ? 0 : Converter.To<int>(result[0].Ticket.CustId);
            paramHastable.Add("custId", custId);

            TempData["paramHastable"] = paramHastable;

            return PartialView("~/Views/BetList/BetTradeDetails/Index.cshtml", result);
        }

        /// <summary>
        /// Exports the bet list statement.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="transType">Type of the trans.</param>
        /// <param name="fromDate">From date.</param>
        /// <returns>The bet list statement.</returns>
        [HttpGet]
        public ActionResult ExportBetlistStatement(string username, int transType, DateTime fromDate)
        {
            string filename = string.Empty;
            int customerId = _betListService.GetCustIDByUsername(username);
            ExportExcelBetlist export = new ExportExcelBetlist();
            var ms = export.ExportExcelBetListStatement(customerId, transType, fromDate, ref filename);

            if (ms != null)
            {
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", filename);
            }

            return new EmptyResult();
        }

        [AuthorizeFunctions("ViewInfo")]
        public ActionResult GetCashOutDetails(GetCashOutDetailCriteria criteria)
        {
            var cashOutDetails = _betListDetailService.GetCashOutDetails(criteria);

            return View("~/Views/BetList/CashOut/Details.cshtml", cashOutDetails);
        }

        [HttpGet]
        public ActionResult ViewBonusDetails(GetLicenseeBonusDetailCriteria criteria)
        {
            var result = _betListDetailService.GetLicenseeBonusDetails(criteria);

            return View("~/Views/BetList/LicenseeBonus/Details.cshtml", result);
        }
    }
}