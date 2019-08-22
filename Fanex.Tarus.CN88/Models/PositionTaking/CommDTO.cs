namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    public class CommDTO
    {
        public int SportType { get; set; }
        public int BetType { get; set; }
        public bool IsLive { get; set; }
        public int RoleId { get; set; }
        public string GroupName { get; set; }
        public decimal Comm { get; set; }
    }
}
