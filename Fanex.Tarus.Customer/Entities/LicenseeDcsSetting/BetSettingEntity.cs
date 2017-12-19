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
                return CacheRef.GetSportName(SportType);
            }
        }

        public int BetType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}
