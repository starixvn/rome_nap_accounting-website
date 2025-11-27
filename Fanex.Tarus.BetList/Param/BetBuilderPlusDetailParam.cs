namespace Fanex.Tarus.BetList.Param
{
    public class BetBuilderPlusDetailParam
    {
        public BetBuilderPlusDetailParam()
        {
            Language = Constants.Language.Default;
        }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int CustId { get; set; }

        public string TransDesc { get; set; }

        public decimal Handicap1 { get; set; }

        public decimal Handicap2 { get; set; }

        public string BetCheck { get; set; }

        public string BetTeam { get; set; }

        public decimal Stake { get; set; }

        public string Currency { get; set; }

        public bool RemoveSabaWord { get; set; }

        public string Language { get; set; }

    }
}