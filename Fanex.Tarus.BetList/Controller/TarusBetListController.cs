// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.BetList.Controller
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.BetList.Accounting.Criteria;
    using Fanex.BetList.Accounting.Models;
    using Fanex.BetList.Accounting.Services;
    using Fanex.BetList.Accounting;
    using Sunplus.Accounting.Core.Common;

    using DNAide.Core;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TarusBetListController : Controller
    {
        private readonly IBetListService _betListService;

        public TarusBetListController()
        {
            this._betListService = new BetListService();
        }

        /// <summary>
        /// Betlist by refno`
        /// </summary>
        /// <returns></returns>
        
        public ActionResult BetListByRefNo_Main()
        {
            return View("~/Views/BetList/BetListByRefNo/Index.cshtml");
        }


        /// <summary>
        /// Call Main Page Belist for Customer Info
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
       
        public ActionResult BetListByCustInfo(int CustID)
        {
            ViewBag.CustID = CustID;
            var dateTmp = new DateTime(0001, 1, 1);
            DateTime fromDate = CastAide.AsDateTime(AppSession.GetSession(System.Web.HttpContext.Current.Session, "FROM_DATE"));
            DateTime toDate = CastAide.AsDateTime(AppSession.GetSession(System.Web.HttpContext.Current.Session, "TO_DATE")) ;
            ViewBag.FromDate = fromDate == dateTmp ? DateTime.Today.AddDays(-1) : fromDate;
            ViewBag.ToDate = toDate == dateTmp ? DateTime.Today : toDate;
            return View("~/Views/BetList/BetlistByCustInfo/Index.cshtml");
        }

        /// <summary>
        /// Render partial detail for  Customer Info -  BetList Tab with date range
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="Bettype"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="IsCache"></param>
        /// <returns></returns>
        public ActionResult GetBetListByCustInfo_DateRange(int CustID, DateTime FromDate, DateTime ToDate, int Bettype, int PageSize, int PageIndex, bool IsCache)
        {
            int quantity = 0;
            var result = _betListService.GetBetListByCustInfo_DateRange(CustID, FromDate, ToDate, Bettype, PageSize, PageIndex, ref quantity, IsCache);
            ViewBag.Quantity = quantity;
            ViewBag.NumberBegining = (PageSize * (PageIndex - 1)) + 1;
            AppSession.AddSession(System.Web.HttpContext.Current.Session, "FROM_DATE", FromDate);
            AppSession.AddSession(System.Web.HttpContext.Current.Session, "TO_DATE", ToDate);
            return PartialView("~/Views/BetList/BetlistByCustInfo/Betlist/_BetList.cshtml", result);
        }


 
        public ActionResult BetListByIP()
        {
            return View("~/Views/BetList/BetListByIP/Index.cshtml");
        }
    }
}
