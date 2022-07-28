namespace Fanex.Tarus.Customer.Entities.InitializingBetLimit
{
    public class BetLimitModel
    {
        public int SportTypeId { get; set; }

        public string SportType { get; set; }

        public int BetTypeId { get; set; }

        public string BetType { get; set; }

        public int SiteId { get; set; }

        public string Site { get; set; }

        public string CreditCurrency { get; set; }

        public string DepositCurrency { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal NewMinBet { get; set; }

        public decimal NewMaxBet { get; set; }

        public decimal NewMaxPerMatch { get; set; }

        public decimal NewMaxPerBall { get; set; }

        public int GameGroupId { get; set; }
    }
}
