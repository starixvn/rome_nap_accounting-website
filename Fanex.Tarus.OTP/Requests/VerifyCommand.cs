namespace Fanex.Tarus.OTP.Requests
{
    public class VerifyCommand
    {
        public string UserName { get; set; }

        public int CustId { get; set; }
        
        public string OTP { get; set; }
    }
}
