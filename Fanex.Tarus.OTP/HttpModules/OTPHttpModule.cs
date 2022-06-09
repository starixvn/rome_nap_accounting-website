using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using Sunplus.Accounting.Core.Common;
using System;
using System.Web;

namespace Fanex.Tarus.OTP.HttpModules
{
    public sealed class OTPHttpModule : IHttpModule
    {
        private readonly IUserService userService;
        public OTPHttpModule()
            : this(null)
        {
        }

        internal OTPHttpModule(IUserService userService)
        {
            this.userService = userService ?? new UserService();
        }
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.AcquireRequestState += Context_AcquireRequestState;
            }
        }

        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            if (IsSkipVerifyOTP())
            {
                AppSession.IsOTPVerified = true;
                return;
            }

            if (IsNeedVerifyOTP(HttpContext.Current.Request))
            {
                HttpContext.Current.Response.Redirect("~/OTP/Verify?custId=" + AppSession.AccId + "&userName=" + AppSession.Username);
            }
        }

        private bool IsSkipVerifyOTP()
        {
            return AppSession.AccId > 0
                && (AppSession.IsOTPVerified || userService.IgnoredOtp(AppSession.AccId, "NAP"));
        }

        private bool IsNeedVerifyOTP(HttpRequest httpRequest)
        {
            var url = GetLowerAbsolutePath(HttpContext.Current.Request);

            return AppSession.AccId > 0
                && !AppSession.IsOTPVerified
                && AppSession.Username.ToLowerInvariant().Contains("starix")
                && IsShouldRedirectUrl(url);
        }

        private bool IsShouldRedirectUrl(string url)
        {
            return !(url.Contains("otp/verify") || url.Contains("signin") || url.Contains("logout"));
        }

        private string GetLowerAbsolutePath(HttpRequest httpRequest)
        {
            var virtualDirectory = httpRequest.ApplicationPath;

            var absolutePath = virtualDirectory == "/"
                ? httpRequest.Url.AbsolutePath
                : httpRequest.Url.AbsolutePath.Replace(virtualDirectory, string.Empty);

            return absolutePath.ToLowerInvariant();
        }
    }
}
