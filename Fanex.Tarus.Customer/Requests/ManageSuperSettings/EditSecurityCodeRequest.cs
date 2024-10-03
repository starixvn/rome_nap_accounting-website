namespace Fanex.Tarus.Customer.Requests.ManageSuperSettings
{
    public class EditSecurityCodeRequest
    {
        public string SecurityCode { get; set; }
        public string ConfirmedSecurityCode { get; set; }
        public bool ForceChangeSecurityCode { get; set; }
    }
}
