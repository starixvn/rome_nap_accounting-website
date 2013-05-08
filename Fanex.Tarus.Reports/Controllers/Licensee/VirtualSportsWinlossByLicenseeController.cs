

namespace Fanex.Tarus.Reports.Controllers.Licensee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Business.Licensee.VirtualSportWinLoss;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Mvc;
    using DNAide.Core;

 
    public class VirtualSportsWinlossByLicenseeController: BaseController
    {
         IVirtualSportWinlossByLicenseeService _virtualSportsService = null;
        public VirtualSportsWinlossByLicenseeController(): this(null) { }
        public VirtualSportsWinlossByLicenseeController(IVirtualSportWinlossByLicenseeService numberGameService)
        {
            _virtualSportsService = numberGameService ?? new VirtualSportWinlossByLicenseeService();
        }

        //[AuthenticateRead]
        public ActionResult VirtualSportWinLoss()
        {
            return View("~/Views/Report/Licensee/VirtualSportWinLoss/WinlossByLicenseeReport.cshtml");
        }

        [HttpPost]
        public JsonResult VirtualSportsLicensees(string productCode, bool isExcludeBA, bool isExcludeTest)
        {
            var result = _virtualSportsService.SelAllLicensees(productCode, isExcludeBA, isExcludeTest);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelVirtualSportWinLoss(string normalLicensee, string operators, string fromDate, string toDate, string sportList)
        {
            var result = _virtualSportsService.SelVirtualSportsWinLoss(normalLicensee, operators, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), sportList);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelAllVirtualSportName()
        {
            var result = _virtualSportsService.SelSportType();
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
