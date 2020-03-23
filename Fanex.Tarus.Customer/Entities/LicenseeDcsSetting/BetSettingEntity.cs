using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class BetSettingEntity
    {
        public BetSettingEntity()
        {
        }

        public BetSettingEntity(int sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch)
        {
            SportType = sportType;
            BetType = betType;
            MinBet = minBet;
            MaxBet = maxBet;
            MaxPerMatch = maxPerMatch;
        }

        public int SportType { get; set; }

        public string SportTypeName
        {
            get
            {
                var sportName = CacheRef.GetSportName(SportType);
                return string.IsNullOrWhiteSpace(sportName) ? SportType.ToString() : sportName;
            }
        }

        public int BetType { get; set; }

        public string BetTypeName
        {
            get
            {
                var bettypeName = CacheRef.GetBettype(BetType);
                return string.IsNullOrWhiteSpace(bettypeName) ? BetType.ToString() : bettypeName;
            }
        }

        public decimal MinBet { get; set; }

        public decimal SuperMinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal SuperMaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal SuperMaxPerMatch { get; set; }
    }
}
