using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Web.Controllers;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using DNAide.Core;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller.Setting
{
    public class SMATurnoverController: BaseController
    {
        public ISMATurnoverService _SMATurnover;

        public SMATurnoverController():this(null)
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
            var result = new List<BaseModel>();
            result = _SMATurnover.GetSportType(CastAide.AsInt32(mode));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult GetLeague(string sport, string fromDate, string toDate)
        {
            var result = new List<BaseModel>();
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
            var result = _SMATurnover.AddSMATurnover(CastAide.AsInt32(matchId), custName, CastAide.AsDouble(maxTurnover));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [AuthenticateHost]
        public JsonResult UpdateSMATurnover(string matchId, string custId, string maxTurnover)
        {
            var result = _SMATurnover.UpdateSMATurnover(CastAide.AsInt32(matchId), CastAide.AsInt32(custId), CastAide.AsDouble(maxTurnover));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult CheckMaxTurnoverValue( string custName, string matchId, string maxTurnover, string mode)
        {
            var result = _SMATurnover.CheckMaxTurnoverValue(CastAide.AsInt32(matchId), custName, CastAide.AsDouble(maxTurnover), CastAide.AsInt32(mode));
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
