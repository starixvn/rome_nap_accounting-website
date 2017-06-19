using CasinoLibrary.Model.UserException;
using Fanex.Module.Logs;
using Fanex.Module.Logs.Services;
using Fanex.Tarus.Business.Implement.Customer;
using Fanex.Tarus.Web.Controllers.Filters;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThirdPartyProducts.StatusSetting.Enums;
using ThirdPartyProducts.StatusSetting.Model;

namespace Fanex.Tarus.Web.Controllers.Customer
{
    public class ThirdPartyProductStatusController : BaseController
    {
        private ThirdPartyProductStatusService thirdPartyProductStatusService;

        public ThirdPartyProductStatusController()
        {
            thirdPartyProductStatusService = new ThirdPartyProductStatusService();
        }

        [AuthenticateRead]
        public ActionResult Index(int custId, string custName, int roleId, int productId)
        {
            return View(
                "~/Areas/CustomerSetting/Views/EnableThirdPartyProductStatus/Index.cshtml",
                thirdPartyProductStatusService.Select(
                    custId,
                    custName,
                    roleId,
                    productId
                    ));
        }

        [AuthenticateHost]
        public JsonResult Update(
            int custId,
            int productId,
            string productStatus)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var productStatusModel = js.Deserialize<List<ProductStatus>>(productStatus);
            var result = thirdPartyProductStatusService.Update(
                custId,
                0,
                productId,
                AppSession.AccId,
                (int)Enumerations.AccountingSystem.Accounting,
                Utils.GetClientIP(),
                productStatusModel);

            if (productId == (int)Product.Casino && result[0].ErrorCode.Equals("0"))
            {
                var customer = new CasinoLibrary.SBAgent.Customer();
                try
                {
                    customer.UpdateCustEnableCasino(
                        custId,
                        !productStatusModel[0].IsDisabled,
                        !productStatusModel[0].IsDisabledDownline,
                        Utils.GetClientIP(),
                        AppSession.AccId,
                        AppSession.AccUserName,
                        3);
                }
                catch (ErrorException casinoEx)
                {
                    casinoEx.WriteCasinoException(Formater.LogType.Warning);
                }
                catch (Exception ex)
                {
                    ex.Write(Formater.LogType.Warning);
                }
            }

            return Json(result);
        }
    }
}
