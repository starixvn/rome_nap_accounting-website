namespace Fanex.Tarus.BetList.Models
{
    public class Match
    {
        public int HomeID { get; set; }

        public int AwayID { get; set; }

        public string Event { get; set; }

        public bool IsNeutral { get; set; }
    }
}