namespace Fanex.Tarus.Admin.Controller
{
    using System;
    using System.Web.Mvc;
    using Business.IMBCRestriction;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;
    public class IMBCAdminController : BaseController
    {
        private readonly IMBCAdminService _service = new IMBCAdminService();

        [AuthenticateRead]
        public ActionResult RestrictIMBC()
        {
            var result = false;
            
            try
            {
                result = _service.IMBCRestrictionStatus();
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return View("~/Areas/CustomerSetting/Views/IMBCRestriction/Index.cshtml", result);
        }

        [AuthenticateHost]
        public ActionResult Update(bool isAllowed)
        {
            var result = false;
            var message = string.Empty;
            try
            {
                result = _service.UpdateIMBCRestrictionStatus(isAllowed);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                message = ex.Message;
            }
            
            return Json(new { ErrorCode = result ? 0 : 1, ErrorMessage = message });
        }
    }
}
