// --------------------------------------------------------------------------
// <copyright file="SMATurnoverController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller.Setting
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover;
    using Fanex.Tarus.Customer.Criteria.SMATurnover;
    using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class SMATurnoverController : BaseController
    {
        public ISMATurnoverService _SMATurnover;

        public SMATurnoverController()
            : this(null)
        { }

        public SMATurnoverController(ISMATurnoverService SMATurnover)
        {
            _SMATurnover = SMATurnover ?? new SMATurnoverService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/CustomerSetting/SMATurnover/SMATurnover.cshtml");
        }

        public JsonResult GetSportType(string mode)
        {
            var result = new List<Sport>();
            result = _SMATurnover.GetSportType(CastAide.AsInt32(mode));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetLeague(string sport, string fromDate, string toDate)
        {
            var result = new List<League>();
            result = _SMATurnover.GetLeague(CastAide.AsInt32(sport), CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetInfoByMatchId(string matchId)
        {
            var result = _SMATurnover.GetInfoByMatchId(CastAide.AsInt32(matchId));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetSMATurnoverList(string sportId, string LeagueId, string fromDate, string toDate)
        {
            var result = _SMATurnover.GetSMATurnoverList(CastAide.AsInt32(sportId), CastAide.AsInt32(LeagueId), CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [AuthenticateHost]
        public JsonResult AddSMATurnover(string matchId, string custName, string maxTurnover)
        {
            var result = _SMATurnover.AddSMATurnover(CastAide.AsInt32(matchId), custName, CastAide.AsDecimal(maxTurnover));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [AuthenticateHost]
        public JsonResult UpdateSMATurnover(string matchId, string custId, string maxTurnover)
        {
            var result = _SMATurnover.UpdateSMATurnover(CastAide.AsInt32(matchId), CastAide.AsInt32(custId), CastAide.AsDecimal(maxTurnover));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult CheckMaxTurnoverValue(string custName,
            string matchId,
            string maxTurnover,
            string mode)
        {
            var result = _SMATurnover.CheckMaxTurnoverValue(CastAide.AsInt32(matchId), custName, CastAide.AsDecimal(maxTurnover), CastAide.AsInt32(mode));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UploadSMAList()
        {
            if (HttpContext.Request.Files != null && HttpContext.Request.Files.Count > 0)
            {
                var excelFile = HttpContext.Request.Files[0];
                var criteria = new AddSMAListCriteria
                {
                    SportId = HttpContext.Request.Params["SportId"],
                    SportName = HttpContext.Request.Params["SportName"],
                    LeagueId = HttpContext.Request.Params["LeagueId"],
                    LeagueName = HttpContext.Request.Params["LeagueName"],
                    FromDate = HttpContext.Request.Params["FromDate"],
                    ToDate = HttpContext.Request.Params["ToDate"],
                    MatchIds = HttpContext.Request.Params["MatchIds"]
                };

                var result = this._SMATurnover.UploadSMAList(excelFile, criteria);
                var jsonResult = Json(result);
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }

            return null;
        }

        public JsonResult GetMatchInfo_AddSMAList(GetMatchInfo_AddSMAListCriteria criteria)
        {
            var result = this._SMATurnover.GetMatchInfo_AddSMAList(criteria);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult DownloadTemplate_AddSMAList()
        {
            this._SMATurnover.DownloadTemplate_AddSMAList();
            int exportResult = 1;
            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
