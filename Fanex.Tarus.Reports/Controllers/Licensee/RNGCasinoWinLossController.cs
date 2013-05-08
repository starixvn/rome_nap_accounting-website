
namespace Fanex.Tarus.Reports.Controllers.Licensee
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Business.Licensee.RNGCasinoWinLoss;
    using Fanex.Tarus.Reports.Entities.Licensee.RNGCasinoWinLoss;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class RNGCasinoWinLossController : BaseController
    {
        IRNGCasinoWinLossService _rngCasinoWinLossService = null;
       
        public RNGCasinoWinLossController() : this(null)
        { 
        }
        public RNGCasinoWinLossController(IRNGCasinoWinLossService rngCasinoWinLossService) 
        {
            _rngCasinoWinLossService = rngCasinoWinLossService ?? new RNGCasinoWinLossService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Report/Licensee/RNGCasinoWinLoss/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetResellerProductCharge()
        {
            var result = _rngCasinoWinLossService.GetResellerProductCharge();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTSTGameGroup()
        {
            var result = _rngCasinoWinLossService.SelTSTGameGroups();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelTSTGameTypeByGroupId(string groupIds)
        {
            var result = _rngCasinoWinLossService.SelTSTGameTypesByGroup(groupIds);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
                
        public ActionResult SelRNGCasinoWinLossReport(string licensees, string fromDate, string toDate, string gameTypes)
        {
            var jsonLicensees = GetLicenseesFromDeserializeJsonString(licensees);
            var result = _rngCasinoWinLossService.SelRNGCasinoWinLossReport(jsonLicensees, fromDate, toDate, gameTypes);
            return PartialView("~/Views/Report/Licensee/RNGCasinoWinLoss/RNGWinLossPage.cshtml", result);
        }

        public ActionResult ExportWinLossReportToExcelFile(string licensees, string fromDate, string toDate, string gameTypes, string reportTitle) 
        {
            var jsonLicensees = GetLicenseesFromDeserializeJsonString(licensees);
            _rngCasinoWinLossService.ExportWinLossReportToExcelFile(jsonLicensees, fromDate, toDate, gameTypes, reportTitle);
            return new EmptyResult();
        }

        #region private function
        [NonAction]
        protected JsonSerializerSettings GetJsonSerializerSetting()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        }
        
        [NonAction]
        protected List<Licensee> GetLicenseesFromDeserializeJsonString(string strLicensees)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            List<Licensee> customer = JsonConvert.DeserializeObject<List<Licensee>>(strLicensees, settings);            
            return customer;
        }
        #endregion      
    }
}
