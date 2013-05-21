// -----------------------------------------------------------------------
// <copyright file="RacingDefaultSettingController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;
    using Fanex.Tarus.Admin.Entities;
    using DNAide.Core;
    using Fanex.Tarus.Admin.Business.RacingDefaultSetting;
    using Fanex.Tarus.Infrastructure.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RacingDefaultSettingController : BaseController
    {
         private readonly IRacingDefaultSettingService _racingDefaultSettingService;

         public RacingDefaultSettingController(IRacingDefaultSettingService racingDefaultSettingService)
        {
            _racingDefaultSettingService = racingDefaultSettingService ?? new RacingDefaultSettingService();
        }

        public RacingDefaultSettingController()
            : this(null)
        {
            _racingDefaultSettingService = new RacingDefaultSettingService();
        }
        [AuthenticateRead]
        public ActionResult Index()
        {
            List<Sport> sportList = new List<Sport>();

            var racings = Sunplus.Accounting.Core.Configurations.RacingSection.Instance.Racings;
            List<Tote> toteAllList = new List<Tote>();
            foreach (Sunplus.Accounting.Core.Configurations.RacingElement racing in racings)
            {
                if (!racing.Active)
                {
                    continue;
                }
                Sport sportTmp = new Sport();
                sportTmp.SportID = CastAide.AsInt32(racing.Id);
                sportTmp.SportName = racing.Name;

                foreach (Sunplus.Accounting.Core.Configurations.ToteElement tote in racing.Totes)
                {
                    if (tote.Active)
                    {
                        Tote toteTmp = new Tote();
                        toteTmp.ToteID = CastAide.AsInt32(tote.Id);
                        toteTmp.ToteName = tote.Name;
                        sportTmp.ToteList.Add(toteTmp);
                        toteAllList.Add(toteTmp);
                    }
                }

                if (sportTmp.ToteList.Count > 1)
                {
                    sportTmp.ToteList.Insert(0, new Tote { ToteID = -1, ToteName = "All" });
                }
                sportList.Add(sportTmp);
            }

            toteAllList.Insert(0, new Tote { ToteID = -1, ToteName = "All" });
            sportList.Insert(0,new Sport
            {
                SportName = "All",
                SportID = -1,
                ToteList = toteAllList.GroupBy(x => new { x.ToteID, x.ToteName }).Select(x => new Tote { ToteID = x.Key.ToteID, ToteName = x.Key.ToteName }).Distinct().ToList()
            }
            );

            return View("~/Views/Admin/RacingDefaultSetting/Index.cshtml", sportList);
        }

        public ActionResult SelRacingDefaultSetting(int SportType, int Bettype, string Currency)
        {
            var result = _racingDefaultSettingService.SelRacingDefaultSetting(SportType, Bettype, Currency);
            return PartialView("~/Views/Admin/RacingDefaultSetting/_Content.cshtml", result);
        }

        public JsonResult UpdateRacingDefaultSetting(RacingDefaultSetting racing)
        {
            var result = new Result();
            result = _racingDefaultSettingService.UpdateRacingDefaultSetting(AppSession.AccId ,CastAide.AsInt32(racing.SportTypeID), CastAide.AsInt32(racing.BettypeID),
                                                CastAide.AsInt32(racing.CurrencyID), CastAide.AsInt32(racing.MinBet));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult UpdateMultipleRacingDefaultSetting(List<RacingDefaultSetting> racingList)
        {
            var result = new List<Result>();
            result = _racingDefaultSettingService.UpdateMultipleRacingDefaultSetting(racingList);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
