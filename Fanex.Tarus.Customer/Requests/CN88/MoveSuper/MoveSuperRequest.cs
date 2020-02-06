namespace Fanex.Tarus.Customer.Requests.CN88.MoveSuper
{
    public class MoveSuperRequest
    {
        public int UserId { get; set; }

        public int SuperId { get; set; }

        public string HeadlineUsername { get; set; }

        public int SportsbookPossess { get; set; }

        public int NonSportPossess { get; set; }

        public string EffectiveDate { get; set; }

        public string Ip { get; set; }
    }
}
