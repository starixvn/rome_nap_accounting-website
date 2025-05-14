namespace Fanex.Tarus.BetList.Entities
{
    public class ShadowAccountModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public override string ToString()
            => $"<b>{UserName}</b> ({CustId})";
    }
}
