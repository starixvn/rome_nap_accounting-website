namespace Fanex.Tarus.OTP.Controllers
{
    using Fanex.Tarus.OTP.Requests;
    using Fanex.Tarus.OTP.Services;
    using Fanex.Tarus.OTP.ViewModels;
    using Sunplus.Accounting.Core.Common;
    using System.Web.Mvc;

    public class OTPController : Controller
    {
        private readonly IOTPService service;
        public OTPController()
        {
            service = new OTPService();
        }

        [HttpGet]
        public ActionResult Verify(VerifyRequest request)
        {
            return View(new VerifyViewModel { CustId = request.CustId, UserName = request.UserName });
        }

        [HttpPost]
        public ActionResult Verify(VerifyCommand request)
        {
            if(service.Verify(request.CustId, request.OTP))
            {
                AppSession.IsOTPVerified = true;
                return Redirect(Url.Content("~/Index.aspx"));
            }

            return View(new VerifyViewModel { CustId = request.CustId, UserName = request.UserName, ErrorMessage = "Wrong OTP, please try again." });
        }
    }
}
