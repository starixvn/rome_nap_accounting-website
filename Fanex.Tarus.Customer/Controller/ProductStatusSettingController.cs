using System.Web.Mvc;
using Fanex.ProductStatus.Setting;
using Fanex.Tarus.Customer.ViewModels.ProductStatus;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    public class ProductStatusSettingController : BaseController
    {
        [AuthenticateRead]
        public ActionResult Index(ProductStatusViewModel viewModel)
        {
            var model = ProductStatusSettingHelper.GetProductStatus(viewModel.ProductId, viewModel.CustId);

            viewModel.ProductStatuses = model;

            return View("~/Views/Customer/ProductStatusSetting/Index.cshtml", viewModel);
        }

        [AuthenticateHost]
        public JsonResult Update()
        {
            return Json(new { });
        }
    }
}
