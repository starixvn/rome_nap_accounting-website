namespace Fanex.Tarus.Customer.DangerousGrouping.Request
{
    public class ValidateUsernamesRequest
    {
        public string Usernames { get; set; }

        public byte DangerType { get; set; }

        public int DangerLevel { get; set; }
    }
}