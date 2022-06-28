namespace Fanex.Tarus.OTP.Services
{
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    public interface IOTPService
    {
        bool Verify(int userId, string otp);
    }

    public class OTPService : IOTPService
    {
        private const string NAPSYSTEM = "NAP";
        private readonly IUserService userService; 
        public OTPService()
        {
            userService = new UserService();
        }

        public bool Verify(int userId, string otp)
            => !string.IsNullOrWhiteSpace(otp) && userService.VerifyOtp(userId, NAPSYSTEM, otp);
    }
}
