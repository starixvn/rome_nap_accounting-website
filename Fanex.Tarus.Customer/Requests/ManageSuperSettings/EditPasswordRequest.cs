namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class EditPasswordRequest
    {
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public bool ForceChangePassword { get; set; }
    }
}
