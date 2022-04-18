namespace Fanex.Tarus.OTP.Controllers
{
    using Fanex.Tarus.OTP.Requests;
    using Fanex.Tarus.OTP.ViewModels;
    using Sunplus.Accounting.Core.Common;
    using System.Web.Mvc;

    public class OTPController : Controller
    {
        [HttpGet]
        public ActionResult Verify(VerifyRequest request)
        {
            return View(new VerifyViewModel { CustId = request.CustId, UserName = request.UserName });
        }

        [HttpPost]
        public ActionResult Verify(VerifyCommand request)
        {
            AppSession.IsOTPVerified = true;
            return Redirect(Url.Content("~/Index.aspx"));
            //return View(new VerifyViewModel { CustId = request.CustId,UserName = request.UserName, ErrorMessage = "Error. Please try again !" });
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
    }
}
