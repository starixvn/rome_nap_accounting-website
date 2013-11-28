namespace Fanex.Tarus.Finance.Entities
{
    public class ExchangeRate
    {
        public string Code { get; set; }

        public double ActualRate { get; set; }

        public string Currency { get; set; }

        public int ExchangeId { get; set; }
    }
}