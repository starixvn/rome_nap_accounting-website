namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    public class VirtualRateEntity
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool IsLive { get; set; }

        public decimal Rate { get; set; }
    }
}
