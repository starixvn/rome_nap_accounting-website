namespace Fanex.Tarus.Customer.DangerousGrouping.Models
{
    public class CustomerDanger
    {
        public string Username { get; set; }

        public string CusId { get; set; }

        public bool IsBA { get; set; }

        public bool IsMember { get; set; }

        public DangerDetail OriginDanger { get; set; }

        public DangerDetail ABIDanger { get; set; }

        public DangerDetail BasketballDanger { get; set; }

        public DangerDetail ESportsDanger { get; set; }

        public DangerDetail OtherSportDanger { get; set; }
    }
}