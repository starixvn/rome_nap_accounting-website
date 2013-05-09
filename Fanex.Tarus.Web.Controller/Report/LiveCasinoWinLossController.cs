using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Business.Implement.Report;
using Fanex.Tarus.Business.Interface.Report;
using Fanex.Tarus.Entities.Report;
using Fanex.Tarus.Entities.Sport;
using Fanex.Tarus.Web.Controllers.Filters;
using Fanex.Tarus.Web.ViewModel.Report.Models;


namespace Fanex.Tarus.Web.Controllers.Report
{
    public class LiveCasinoWinLossController : BaseController
    {
        private readonly ILiveCasinoWinLossService _LiveCasinoWinLossService;
        public LiveCasinoWinLossController()
            : this(null)
        {
        }

        public LiveCasinoWinLossController(ILiveCasinoWinLossService LiveCasinoWinLossService)
        {
            _LiveCasinoWinLossService = LiveCasinoWinLossService ?? new LiveCasinoWinLossService();
        }

        //
        // GET: /LiveCasinoWinLoss/
        [HttpGet]
        public ActionResult Index()
        {
            LiveCasinoWinLossViewModel ViewModel = new LiveCasinoWinLossViewModel();

            GameSearch result = _LiveCasinoWinLossService.GetGameFilterInfo();
            result.GameTable = _LiveCasinoWinLossService.GetGameTableByGameType(0);

            Category All = new Category {Id = 0, Name ="All", Parent="",Active=false, Selected= false };

            result.Company.Insert(0, All);
            result.GameType.Insert(0, All);
            result.GameTable.Insert(0, All);
            result.Currency.Insert(0, All);

            ViewBag.TimePeriods = GetTimePeriodSelectList();

            ViewModel.GameSearchViewModel = result;
            ViewModel.LiveCasinoWinLoss = new LiveCasinoWinLoss();

            return View("~/Views/Report/LiveCasinoWinLoss/LiveCasinoWinLoss.cshtml", ViewModel);
        }


        [HttpPost]
        public ActionResult GameTable(string value)
        {
            var GameType = DNAide.Core.CastAide.AsInt32(value);

            List<Category> GameTable = _LiveCasinoWinLossService.GetGameTableByGameType(GameType);
            Category All = new Category { Id = 0, Name = "All", Parent = "", Active = false, Selected = false };

            GameTable.Insert(0, All);

            return PartialView("~/Views/Report/LiveCasinoWinLoss/_GameTableDropdowlist.cshtml", GameTable);
        }

        public SelectList GetTimePeriodSelectList()
        {
            var timePeriods = TimePeriod.GetTimePeriod();
            return new SelectList(timePeriods.ToArray(), "ID", "Value");
        }


        [HttpPost]
        public ActionResult GetReport(LiveCasinoWinLossParams dbparams)
        {
            if (dbparams.UserName == null)
            {
                dbparams.UserName = "";
            }
            if (dbparams.Sort.SortCol == null || dbparams.Sort.SortType == null)
            {
                dbparams.Sort.SortType = String.Empty;
                dbparams.Sort.SortCol = String.Empty;
            }

            ViewBag.ReportBy = DNAide.Core.CastAide.AsInt32(dbparams.ReportBy);
            ViewBag.Username = dbparams.UserName;
            ViewBag.isAgency = dbparams.isAgency;
            LiveCasinoWinLossViewModel ViewModel = new LiveCasinoWinLossViewModel();
            ViewModel.LiveCasinoWinLoss = _LiveCasinoWinLossService.GetLiveCasinoWinLoss(dbparams);

            return PartialView("~/Views/Report/LiveCasinoWinLoss/_GetReport.cshtml", ViewModel.LiveCasinoWinLoss);
        }
    }
}
