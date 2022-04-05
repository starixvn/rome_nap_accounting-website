namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class BetSettingWithMaxPerBallEntity : BetSettingEntity
    {
        public decimal MaxPerBall { get; set; }

        public decimal SuperMaxPerBall { get; set; }

        public BetSettingWithMaxPerBallEntity()
        {
        }

        public BetSettingWithMaxPerBallEntity(int sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch, decimal maxPerBall)
            : base(sportType, betType, minBet, maxBet, maxPerMatch)
        {
            MaxPerBall = maxPerBall;
        }
    }
}
