using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Web.Controllers;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using DNAide.Core;

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
    }
}
