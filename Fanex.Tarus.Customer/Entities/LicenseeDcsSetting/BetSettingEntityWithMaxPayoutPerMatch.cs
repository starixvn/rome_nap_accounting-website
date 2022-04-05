namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class BetSettingWithMaxPayoutPerMatchEntity : BetSettingEntity
    {
        public decimal MaxPayoutPerMatch { get; set; }

        public decimal SuperMaxPayoutPerMatch { get; set; }

        public BetSettingWithMaxPayoutPerMatchEntity()
        {
        }

        public BetSettingWithMaxPayoutPerMatchEntity(int sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch, decimal maxPayoutPerMatch)
            : base(sportType, betType, minBet, maxBet, maxPerMatch)
        {
            MaxPayoutPerMatch = maxPayoutPerMatch;
        }
    }
}
