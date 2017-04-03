using Fanex.Tarus.Business.Implement.Customer;
using Fanex.Tarus.Infrastructure.Configurations;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using System.Collections.Generic;
using System.Web.Mvc;
using ThirdPartyProducts.StatusSetting.Model;
using System.Web.Script.Serialization;

namespace Fanex.Tarus.Web.Controllers.Customer
{
    public class ThirdPartyProductStatusController : Controller
    {
        private ThirdPartyProductStatusService thirdPartyProductStatusService;

        public ThirdPartyProductStatusController()
        {
            thirdPartyProductStatusService = new ThirdPartyProductStatusService();
        }

        public ActionResult Index(int custId, int roleId, int productId)
        {
            return View(
                "~/Areas/CustomerSetting/Views/EnableThirdPartyProductStatus/Index.cshtml", 
                thirdPartyProductStatusService.Select(
                    custId, 
                    roleId,
                    productId
                    ));
        }

        public JsonResult Update(
            int custId,
            int productId,
            string productStatus)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            
            return Json(thirdPartyProductStatusService.Update(
                custId,
                0,
                productId,
                AppSession.AccId,
                (int)Enumerations.AccountingSystem.Accounting,
                Utils.GetClientIP(),
                js.Deserialize<List<ProductStatus>>(productStatus)));
        }
    }
}
