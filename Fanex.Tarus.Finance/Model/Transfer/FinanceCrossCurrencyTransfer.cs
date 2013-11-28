namespace Fanex.Tarus.Finance.Model.Transfer
{
    public class FinanceCrossCurrencyTransfer
    {
        public int FromCustID { get; set; }

        public int ToCustID { get; set; }

        public double FromAmount { get; set; }

        public double ToAmount { get; set; }

        public double FromExRate { get; set; }

        public double ToExRate { get; set; }

        public string ForexUsername { get; set; }

        public string Remark { get; set; }
    }
}