namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    using Fanex.Tarus.Infrastructure.Caching;
    using Sunplus.Accounting.Core;
    using Sunplus.Accounting.Core.Extensions;

    public class BetSettingEntity
    {
        private const int MixParlayBettypeId = 9;
        private const int SoccerSporttypeId = 1;
        private const int OthersSporttypeId = 99;

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
                if (BetType == Sunplus.Accounting.Core.Constant.BetType.MixParlay
                    && SportType == Sunplus.Accounting.Core.Constant.Sport.Soccer)
                {
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "SoccerParlay");
                }

                if (BetType == Sunplus.Accounting.Core.Constant.BetType.MixParlay
                    && SportType == Sunplus.Accounting.Core.Constant.Sport.OtherSports)
                {
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMixedSports");
                }

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
