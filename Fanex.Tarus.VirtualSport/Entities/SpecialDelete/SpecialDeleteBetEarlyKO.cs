namespace Fanex.Tarus.VirtualSport.Entities.SpecialDelete
{
    public class SpecialDeleteBetEarlyKO : BetTransactionInfomation
    {
        public string TransId { get; set; }
        public string SubTransId { get; set; }
        public string CurrencyId { get; set; }
        public int ParlayType { get; set; }
    }
}
