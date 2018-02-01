namespace Fanex.Tarus.Customer.Controller.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Tarus.Customer.Business.CustomerLogin;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Web.Controllers;
    using Fanex.Tarus.Web.Controllers.Filters;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerOTPController : BaseController
    {
        private readonly ICustomerOTPService _service;

        public CustomerOTPController()
            : this(null)
        {
        }

        public CustomerOTPController(ICustomerOTPService service)
        {
            this._service = service ?? new CustomerOTPService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/CustomerLogin/CustomerOTP/Index.cshtml");
        }

        [HttpPost, AuthenticateHost]
        public JsonResult RemoveOTP(int custId)
        {
            var result = this._service.RemoveOTP(custId);
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
