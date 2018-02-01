namespace Fanex.Tarus.Customer.Entities.MicroGaming
{
    using Fanex.Tarus.Customer.Resources;

    public class MicroGamingStatus
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public bool Enable { get; set; }

        public bool DownlineEnable { get; set; }

        public int RoleId { get; set; }
    }
}
